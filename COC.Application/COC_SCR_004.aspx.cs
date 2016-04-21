using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Application.Utilities;
using COC.Biz;
using COC.Resource.Data;
using log4net;

namespace COC.Application
{
    public partial class COC_SCR_004 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_004));
        private string LeadListPopup = "LeadListPopup";
        private int colIndex_DoneJobType = 4;
        private int colIndex_ActionDate = 5;
        private int colIndex_JobType = 7;
        private int colIndex_CocAssignTypeDesc = 8;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "User Monitoring";
            Page.Form.DefaultButton = btnSearchUserMonitoring.UniqueID;
            AppUtil.SetIntTextBox(txtWorkEndHour);
            AppUtil.SetIntTextBox(txtWorkEndMin);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_004");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_002.aspx");
                        return;
                    }
                    else
                    {
                        SetScript();
                        DateTime date = DateTime.Now;
                        Label1.Text = date.ToString("dd/MM/") + date.Year.ToString() + " " + date.ToString("HH:mm:ss");
                        Label4.Text = date.ToString("dd/MM/") + date.Year.ToString() + " " + date.ToString("HH:mm:ss");

                        GenerateAppInfo();

                        cmbStaffStatus.SelectedIndex = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void SetScript()
        {
            string script = "";
            //txtWorkEndHour
            script = @" var hour = document.getElementById('" + txtWorkEndHour.ClientID + @"').value; 
                        if(hour.length > 0)
                        {
                            while(hour.length < 2)
                            {
                                hour = '0' + hour;
                            }

                            if (hour < 0 || hour > 23)
                            { 
                                alert('กรุณากรอกเวลาอยู่ระหว่าง 0-23 น.'); document.getElementById('" + txtWorkEndHour.ClientID + @"').focus(); 
                                 document.getElementById('" + txtWorkEndHour.ClientID + @"').value = ''
                            }
                            else
                            {
                                document.getElementById('" + txtWorkEndHour.ClientID + @"').value = hour;
                            }
                        }";

            txtWorkEndHour.Attributes.Add("onblur", script);

            script = "if (document.getElementById('" + txtWorkEndHour.ClientID + @"').value.length == 2 && document.getElementById('" + txtWorkEndHour.ClientID + @"').value <= 23)
                             {document.getElementById('" + txtWorkEndMin.ClientID + @"').focus(); }";
            txtWorkEndHour.Attributes.Add("onkeyup", script);

            //txtWorkEndMin
            script = @" var min = document.getElementById('" + txtWorkEndMin.ClientID + @"').value; 
                        if(min.length > 0)
                        {
                            while(min.length < 2)
                            {
                                min = '0' + min;
                            }

                            if (min < 0 || min > 59)
                            { 
                                alert('กรุณากรอกเวลาอยู่ระหว่าง 0-59 นาที'); document.getElementById('" + txtWorkEndMin.ClientID + @"').focus(); 
                                 document.getElementById('" + txtWorkEndMin.ClientID + @"').value = ''
                            }
                            else
                            {
                                document.getElementById('" + txtWorkEndMin.ClientID + @"').value = min;
                            }
                        }";

            txtWorkEndMin.Attributes.Add("onblur", script);
        }

        private List<string> GetRecursiveTeam()
        {
            List<string> teamList = new List<string>();

            List<StaffData> staffList = StaffBiz.GetStaffList();
            string username = HttpContext.Current.User.Identity.Name;

            var logInStaffId = staffList.Where(p => p.UserName.Trim().ToUpper() == username.Trim().ToUpper()).Select(p => p.StaffId).FirstOrDefault();
            var logInCocTeam = staffList.Where(p => p.UserName.Trim().ToUpper() == username.Trim().ToUpper()).Select(p => p.CocTeam).FirstOrDefault();

            if (logInStaffId != null)
            {
                if (!string.IsNullOrEmpty(logInCocTeam) && logInCocTeam.Trim().ToUpper().StartsWith("COC"))
                    teamList.Add(logInCocTeam);

                FindStaffRecusive(logInStaffId, teamList, staffList);
            }

            string tmpList = "";
            foreach (string team in teamList)
            {
                tmpList += (tmpList == "" ? "" : ",") + "'" + team + "'";
            }
            txtTeamList.Text = tmpList;     //เก็บไว้ใช้ตอนกดปุ่ม เรียกดู(Forecast)

            //Initial Control cmbTeam
            if (cmbTeam.Items.Count == 0)
            {
                teamList = teamList.OrderBy(p => p).ToList();
                foreach (string team in teamList)
                {
                    cmbTeam.Items.Add(new ListItem(team, team));
                }
                cmbTeam.Items.Insert(0, new ListItem("ทั้งหมด", ""));
            }

            return teamList;
        }

        private void FindStaffRecusive(int? headId, List<string> teamList, List<StaffData> staffList)
        {
            foreach (StaffData staff in staffList)
            {
                if (staff.HeadStaffId == headId)
                {
                    if (!string.IsNullOrEmpty(staff.CocTeam) && !teamList.Contains(staff.CocTeam) && staff.CocTeam.Trim().ToUpper().StartsWith("COC")) 
                        teamList.Add(staff.CocTeam);

                    FindStaffRecusive(staff.StaffId, teamList, staffList);
                }
            }
        }

        #region Popup Forecast

        protected void btnSaveWorkEndTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtWorkEndHour.Text.Trim() != "" && txtWorkEndMin.Text.Trim() != "")
                {
                    if (Convert.ToInt32(txtWorkEndHour.Text.Trim()) < 0 || Convert.ToInt32(txtWorkEndHour.Text.Trim()) > 23)
                    {
                        AppUtil.ClientAlert(Page, "กรุณาระบุชั่วโมงให้ถูกต้อง (0-23)");
                        mpePopupForecast.Show();
                        return;
                    }
                    if (Convert.ToInt32(txtWorkEndMin.Text.Trim()) < 0 || Convert.ToInt32(txtWorkEndMin.Text.Trim()) > 59)
                    {
                        AppUtil.ClientAlert(Page, "กรุณาระบุชั่วโมงให้ถูกต้อง (0-59)");
                        mpePopupForecast.Show();
                        return;
                    }
                }
                else
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุเวลาให้ครบถ้วน");
                    mpePopupForecast.Show();
                    return;
                }

                DateTime tmp = DateTime.Now;
                DateTime now = new DateTime(tmp.Year, tmp.Month, tmp.Day, tmp.Hour, tmp.Minute, 0);
                DateTime workEndTime = new DateTime(tmp.Year, tmp.Month, tmp.Day, Convert.ToInt32(txtWorkEndHour.Text.Trim()), Convert.ToInt32(txtWorkEndMin.Text.Trim()), 0);
                if (workEndTime <= now)
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุเวลาให้มากกว่าเวลาปัจจุบัน");
                    mpePopupForecast.Show();
                    return;
                }

                OptionBiz.UpdateWorkingEndTime(txtWorkEndHour.Text.Trim() + ":" + txtWorkEndMin.Text.Trim());

                //Update ข้อมูลใน Gridview รายงาน Forecast
                gvForecastReport.DataSource = GetForecastReport(DateTime.Now, txtTeamList.Text.Trim());
                gvForecastReport.DataBind();

                AppUtil.ClientAlert(Page, "บันทึกข้อมูลเรียบร้อย");
                mpePopupForecast.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnForecast_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime forecastTime = DateTime.Now;
                Label6.Text = forecastTime.ToString("dd/MM/") + forecastTime.Year.ToString() + " " + forecastTime.ToString("HH:mm:ss");
                
                string workEndTime = OptionBiz.GetWorkingEndTime();
                if (!string.IsNullOrEmpty(workEndTime))
                { 
                    string[] str = workEndTime.Split(':');
                    if (str.Count() == 2)
                    {
                        txtWorkEndHour.Text = str[0];
                        txtWorkEndMin.Text = str[1];
                        upPopupForecast.Update();
                    }
                }

                gvForecastReport.DataSource = GetForecastReport(forecastTime, txtTeamList.Text.Trim());
                gvForecastReport.DataBind();

                mpePopupForecast.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private List<ForecastReportData> GetForecastReport(DateTime forecastTime, string teamList)
        {
            try
            {
                List<ForecastReportData> forecastList = new List<ForecastReportData>();

                if (teamList != "")
                    forecastList = StaffBiz.GetDataForForecastReport(teamList);

                if (forecastList.Count > 0)
                {
                    foreach (RepeaterItem item in rptAppInfo.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            var data = forecastList.Where(p => p.Team.Trim() == ((Label)item.FindControl("lblTeam")).Text.Trim()).FirstOrDefault();
                            if (data != null)
                            {
                                data.AmountOfJob = Convert.ToInt32(((LinkButton)item.FindControl("lbAppWaitAssignAllJob")).Text.Trim())
                                                    + Convert.ToInt32(((LinkButton)item.FindControl("lbAppAssignedAllJob")).Text.Trim());
                               
                                //data.AmountOfJob = ((LinkButton)item.FindControl("lbAppTotalAllJob")).Text.Trim();
                            }
                        }
                    }

                    string workEndTime = OptionBiz.GetWorkingEndTime();
                    string[] str = workEndTime.Split(':');
                    decimal minDiff = 0;
                    if (str.Count() == 2)
                    {
                        //จำนวนงานที่คาดว่าจะเสร็จ
                        DateTime workingEndTime = new DateTime(forecastTime.Year, forecastTime.Month, forecastTime.Day, int.Parse(str[0]), int.Parse(str[1]), 0);
                        TimeSpan ts = workingEndTime.Subtract(forecastTime);
                        minDiff = Convert.ToDecimal(ts.TotalMinutes);
                        int accumAmountOfJob = 0;

                        foreach (ForecastReportData data in forecastList)
                        {
                            if (data.Sla == null || data.Sla.Value == 0 || minDiff < 0)
                            {
                                data.AmountOfPredictionSuccess = null;     //เวลาที่ต้องใช้เพิ่มเติม(นาที)
                                data.AmountOfAdditionalTime = null;        //เวลาที่ต้องใช้เพิ่มเติม(นาที)
                                data.AdditionalTimePerPerson = "N/A";       //เวลาที่ใช้เพิ่มเติมต่อคน(นาที)
                            }
                            else
                            {
                                //1.จำนวนงานที่คาดว่าจะเสร็จทันภายในเวลาเลิกงาน
                                data.AmountOfPredictionSuccess = Convert.ToInt32(Math.Floor((minDiff / data.Sla.Value) * Convert.ToDecimal(data.AmountOfAvailableStaff)));

                                //2.จำนวนงานที่ต้องทำเกินเวลาเลิกงาน
                                if (data.AmountOfPredictionSuccess > data.AmountOfJob)
                                    data.AmountOfJobExceedEndTime = 0;
                                else
                                    data.AmountOfJobExceedEndTime = data.AmountOfJob - data.AmountOfPredictionSuccess;

                                //3.เวลาที่ต้องใช้เพิ่มเติม(นาที)
                                //  แสดงบนหน้าจอเป็นจำนวนงานของแต่ละทีม แต่เวลาคำนวณให้ accumulate จำนวนตามด้านล่าง
                                //  จำนวนงานทั้งหมดของ COC1 = จำนวนงานของทีม COC1
                                //  จำนวนงานทั้งหมดของ COC2 = จำนวนงานของทีม COC1 + COC2
                                //  จำนวนงานทั้งหมดของ COC3 = จำนวนงานของทีม COC1 + COC2 + COC3

                                accumAmountOfJob += Convert.ToInt32(data.AmountOfJob);

                                if (data.AmountOfPredictionSuccess > accumAmountOfJob)
                                    data.AmountOfAdditionalTime = 0;
                                else
                                {
                                    data.AmountOfAdditionalTime = (accumAmountOfJob - data.AmountOfPredictionSuccess) * data.Sla.Value;
                                }

                                //if (Convert.ToInt32(data.AmountOfPredictionSuccess) > Convert.ToInt32(data.AmountOfJob))
                                //    data.AmountOfAdditionalTime = "0";
                                //else
                                //{
                                //    data.AmountOfAdditionalTime = ((Convert.ToInt32(data.AmountOfJob) - Convert.ToInt32(data.AmountOfPredictionSuccess)) * data.Sla).ToString();
                                //}

                                //เวลาที่ใช้เพิ่มเติมต่อคน(นาที)
                                //if (data.AmountOfAvailableStaff == null || data.AmountOfAvailableStaff.Value == 0)
                                //    data.AdditionalTimePerPerson = "N/A";
                                //else
                                //{
                                //    data.AdditionalTimePerPerson = (Convert.ToInt32(data.AmountOfAdditionalTime) / data.AmountOfAvailableStaff.Value).ToString();
                                //}
                            }
                        }
                    }
                }

                return forecastList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClosePopupForecast_Click(object sender, EventArgs e)
        {
            mpePopupForecast.Hide();
        }

        #endregion

        #region AppInfo

        protected void lbRefreshAppInfo_Click(object sender, EventArgs e)
        {
            try
            {
                GetAppInfoData();
                Label1.Text = DateTime.Now.ToString("dd/MM/") + DateTime.Now.Year.ToString() + " " + DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void rptAppInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (((LinkButton)e.Item.FindControl("lbAppInPoolAllJob")).Text.Trim() == "0")
                    ((Button)e.Item.FindControl("btnAppTranfer")).Visible = false;
                else
                    ((Button)e.Item.FindControl("btnAppTranfer")).Visible = true;
            }
        }

        private void GenerateAppInfo()
        {
            try
            {
                GetAppInfoData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetAppInfoData()
        {
            List<AppInfoData> appList = TeamBiz.GetTeamList(true);
            if (appList.Count > 0)
            {
                GenerateAppInPool(appList);
                GenerateAppWaitAssign(appList);
                GenerateAppAssigned(appList);

                foreach (AppInfoData data in appList)
                {
                    data.AppTotalAllJob = data.AppInPoolAllJob.Value + data.AppWaitAssignAllJob.Value + data.AppAssignedAllJob.Value;
                }

                string[] teamArr = GetRecursiveTeam().ToArray();    //เลือกเฉพาะทีมที่คน login มีสิทธิเห็น

                if (teamArr.Count() == 0)
                {
                    btnForecast.Enabled = false;                //กรณี recursive แล้วไม่พบทีมเลย ให้ปิดปุ่ม forecast
                    btnSearchUserMonitoring.Enabled = false;    //กรณี recursive แล้วไม่พบทีมเลย ให้ปิดปุ่ม ค้นหา
                }

                rptAppInfo.DataSource = appList.Where(p => teamArr.Contains(p.Team)).ToList(); ;
                rptAppInfo.DataBind();
            }
        }

        private void GenerateAppInPool(List<AppInfoData> appList)
        {
            try
            {
                List<TicketIdByTeamData> mainList = LeadBiz.GetNumberOfAppInPoolAllJob();

                //AppInPool งานทั้งหมด
                List<AmountByTeamData> allJoblist = mainList.GroupBy(p => p.Team).Select(p => new AmountByTeamData { Team = p.Key, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamData data in allJoblist)
                {
                    AppInfoData appinfo = appList.Where(p => p.Team.Trim() == data.Team.Trim()).FirstOrDefault();
                    if (appinfo != null)
                        appinfo.AppInPoolAllJob = data.Amount != null ? data.Amount : 0;
                }

                var oldJob = mainList.Where(p => p.FlowLogId != null).ToList();     //งานเก่า
                var newJob = mainList.Where(p => p.FlowLogId == null).ToList();     //งานใหม่

                //AppInPool งานเก่า
                List<AmountByTeamData> oldJobList = oldJob.GroupBy(p => p.Team).Select(p => new AmountByTeamData { Team = p.Key, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamData data in oldJobList)
                {
                    AppInfoData appinfo = appList.Where(p => p.Team.Trim() == data.Team.Trim()).FirstOrDefault();
                    if (appinfo != null)
                        appinfo.AppInPoolOldJob = data.Amount != null ? data.Amount : 0;
                }

                //AppInPool งานใหม่
                List<AmountByTeamData> newJobList = newJob.GroupBy(p => p.Team).Select(p => new AmountByTeamData { Team = p.Key, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamData data in newJobList)
                {
                    AppInfoData appinfo = appList.Where(p => p.Team.Trim() == data.Team.Trim()).FirstOrDefault();
                    if (appinfo != null)
                        appinfo.AppInPoolNewJob = data.Amount != null ? data.Amount : 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateAppWaitAssign(List<AppInfoData> appList)
        {
            try
            {
                string subStatusWaitingList = TeamBiz.GetSubStatusWaitingList();
                List<TicketIdByTeamData> mainList = LeadBiz.GetNumberOfAppWaitAssignAllJob(subStatusWaitingList);

                //App รอจ่าย งานทั้งหมด
                List<AmountByTeamData> allJobList = mainList.GroupBy(p => p.Team).Select(p => new AmountByTeamData { Team = p.Key, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamData data in allJobList)
                {
                    AppInfoData appinfo = appList.Where(p => p.Team.Trim() == data.Team.Trim()).FirstOrDefault();
                    if (appinfo != null)
                        appinfo.AppWaitAssignAllJob = data.Amount != null ? data.Amount : 0;
                }

                var oldJob = mainList.Where(p => p.FlowLogId != null).ToList();     //งานเก่า
                var newJob = mainList.Where(p => p.FlowLogId == null).ToList();     //งานใหม่

                //App รอจ่าย งานเก่า
                List<AmountByTeamData> oldJobList = oldJob.GroupBy(p => p.Team).Select(p => new AmountByTeamData { Team = p.Key, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamData data in oldJobList)
                {
                    AppInfoData appinfo = appList.Where(p => p.Team.Trim() == data.Team.Trim()).FirstOrDefault();
                    if (appinfo != null)
                        appinfo.AppWaitAssignOldJob = data.Amount != null ? data.Amount : 0;
                }

                //App รอจ่าย งานใหม่
                List<AmountByTeamData> newJobList = newJob.GroupBy(p => p.Team).Select(p => new AmountByTeamData { Team = p.Key, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamData data in newJobList)
                {
                    AppInfoData appinfo = appList.Where(p => p.Team.Trim() == data.Team.Trim()).FirstOrDefault();
                    if (appinfo != null)
                        appinfo.AppWaitAssignNewJob = data.Amount != null ? data.Amount : 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateAppAssigned(List<AppInfoData> appList)
        {
            try
            {
                List<TicketIdByTeamData> mainList = LeadBiz.GetNumberOfAppAssignedAllJob();

                //App จ่ายแล้ว งานทั้งหมด
                List<AmountByTeamData> allJobList = mainList.GroupBy(p => p.Team).Select(p => new AmountByTeamData { Team = p.Key, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamData data in allJobList)
                {
                    AppInfoData appinfo = appList.Where(p => p.Team.Trim() == data.Team.Trim()).FirstOrDefault();
                    if (appinfo != null)
                        appinfo.AppAssignedAllJob = data.Amount != null ? data.Amount : 0;
                }

                var oldJob = mainList.Where(p => p.FlowLogId != null).ToList();     //งานเก่า
                var newJob = mainList.Where(p => p.FlowLogId == null).ToList();     //งานใหม่

                //App จ่ายแล้ว งานเก่า
                List<AmountByTeamData> oldJobList = oldJob.GroupBy(p => p.Team).Select(p => new AmountByTeamData { Team = p.Key, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamData data in oldJobList)
                {
                    AppInfoData appinfo = appList.Where(p => p.Team.Trim() == data.Team.Trim()).FirstOrDefault();
                    if (appinfo != null)
                        appinfo.AppAssignedOldJob = data.Amount != null ? data.Amount : 0;
                }

                //App จ่ายแล้ว งานใหม่
                List<AmountByTeamData> newJobList = newJob.GroupBy(p => p.Team).Select(p => new AmountByTeamData { Team = p.Key, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamData data in newJobList)
                {
                    AppInfoData appinfo = appList.Where(p => p.Team.Trim() == data.Team.Trim()).FirstOrDefault();
                    if (appinfo != null)
                        appinfo.AppAssignedNewJob = data.Amount != null ? data.Amount : 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AppInPool
        //AppInPoolJob

        protected void lbAppInPoolNewJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring>  mainList = LeadBiz.GetNumberOfAppInPoolAllJobByTeam(((LinkButton)sender).CommandArgument);
                var newJobList = mainList.Where(p => p.FlowLogId == null).ToList();     //งานใหม่

                Session[LeadListPopup] = newJobList;    //ใช้ใน PageSearchChangePopup
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, newJobList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        protected void lbAppInPoolOldJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetNumberOfAppInPoolAllJobByTeam(((LinkButton)sender).CommandArgument);
                var oldJobList = mainList.Where(p => p.FlowLogId != null).ToList();     //งานเก่า

                Session[LeadListPopup] = oldJobList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, oldJobList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        protected void lbAppInPoolAllJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetNumberOfAppInPoolAllJobByTeam(((LinkButton)sender).CommandArgument);
                
                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        protected void btnAppTranfer_Click(object sender, EventArgs e)
        {
            try
            {
                string cocTeam = ((Button)sender).CommandArgument;
                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetNumberOfAppInPoolAllJobByTeam(cocTeam);

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);

                cmbStaffTranserJob.DataSource = StaffBiz.GetStaffByTeam(cocTeam);
                cmbStaffTranserJob.DataTextField = "TextField";
                cmbStaffTranserJob.DataValueField = "ValueField";
                cmbStaffTranserJob.DataBind();
                cmbStaffTranserJob.Items.Insert(0, new ListItem("", ""));

                SetDisplayTransferSection(true);

                txtPopupCocTeam.Text = cocTeam;
                cbDoRefreshAppInfo.Checked = false;
                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void SetDisplayTransferSection(bool display)
        {
            gvPopup.Columns[0].Visible = display;
            pnTranserJob.Visible = display;

            if (display)
            {
                if (gvPopup.Rows.Count > 0)
                {
                    cmbStaffTranserJob.Enabled = true;
                    btnPopupTranserJob.Enabled = true;
                }
                else
                {
                    cmbStaffTranserJob.Enabled = false;
                    btnPopupTranserJob.Enabled = false;
                }
            }
        }

        #endregion

        #region AppWaitAssignJob
        //App รอจ่าย

        protected void lbAppWaitAssignNewJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetNumberOfAppWaitAssignAllJobByTeam(((LinkButton)sender).CommandArgument);
                var newJobList = mainList.Where(p => p.FlowLogId == null).ToList();     //งานใหม่

                Session[LeadListPopup] = newJobList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, newJobList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        protected void lbAppWaitAssignOldJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetNumberOfAppWaitAssignAllJobByTeam(((LinkButton)sender).CommandArgument);
                var oldJobList = mainList.Where(p => p.FlowLogId != null).ToList();     //งานใหม่

                Session[LeadListPopup] = oldJobList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, oldJobList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        protected void lbAppWaitAssignAllJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetNumberOfAppWaitAssignAllJobByTeam(((LinkButton)sender).CommandArgument);

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        #region AppAssignedJob
        //App จ่ายแล้ว
        protected void lbAppAssignedNewJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetNumberOfAppAssignedAllJobByTeam(((LinkButton)sender).CommandArgument);
                var newJobList = mainList.Where(p => p.FlowLogId == null).ToList();     //งานใหม่

                Session[LeadListPopup] = newJobList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, newJobList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        protected void lbAppAssignedOldJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetNumberOfAppAssignedAllJobByTeam(((LinkButton)sender).CommandArgument);
                var oldJobList = mainList.Where(p => p.FlowLogId != null).ToList();     //งานใหม่

                Session[LeadListPopup] = oldJobList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, oldJobList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        protected void lbAppAssignedAllJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetNumberOfAppAssignedAllJobByTeam(((LinkButton)sender).CommandArgument);

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbAppTotalAllJob_Click(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> appInPoolList = LeadBiz.GetNumberOfAppInPoolAllJobByTeam(((LinkButton)sender).CommandArgument);
                List<LeadDataPopupMonitoring> appWaitAssign = LeadBiz.GetNumberOfAppWaitAssignAllJobByTeam(((LinkButton)sender).CommandArgument);
                List<LeadDataPopupMonitoring> appAssigned = LeadBiz.GetNumberOfAppAssignedAllJobByTeam(((LinkButton)sender).CommandArgument);

                List<LeadDataPopupMonitoring> allJob = appInPoolList.Union(appWaitAssign).Union(appAssigned).OrderBy(p => p.CocFirstAssignDate).ToList();

                Session[LeadListPopup] = allJob;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, allJob.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        #region UserMonitoring

        protected void rptUserMonitoring_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (((Image)e.Item.FindControl("imgNotAvailable")).Visible == true)
                {
                    if (((LinkButton)e.Item.FindControl("lbNewJobAll")).Text.Trim() == "0")
                        ((Button)e.Item.FindControl("btnUserMonitoringTranferJob")).Visible = false;
                    else
                        ((Button)e.Item.FindControl("btnUserMonitoringTranferJob")).Visible = true;
                }
            }
        }

        protected void btnSearchUserMonitoring_Click(object sender, EventArgs e)
        {
            try
            {
                GetUserMonitoringList();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void GetUserMonitoringList()
        {
            try
            {
                txtStaffStatusSelected.Text = cmbStaffStatus.SelectedItem.Value;
                List<UserMonitoringData> mainList = StaffBiz.GetUserMonotoringStaffList(cmbStaffStatus.SelectedItem.Value, cmbTeam.SelectedItem.Value, txtTeamList.Text.Trim());

                GetUserMonitoringNewJob(mainList);
                GetUserMonitoringDoneJob(mainList);

                foreach (UserMonitoringData data in mainList)
                {
                    data.AmountAllJob = data.AmountNewJobAll + data.AmountDoneJobAll;
                }

                rptUserMonitoring.DataSource = mainList;
                rptUserMonitoring.DataBind();

                Label4.Text = DateTime.Now.ToString("dd/MM/") + DateTime.Now.Year.ToString() + " " + DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetUserMonitoringNewJob(List<UserMonitoringData> mainList)
        {
            try
            {
                //งานรอรับ
                List<UserMonitoringNewJobData> newJobList = LeadBiz.GetUserMonitoringNewJobWaitAssignList(cmbStaffStatus.SelectedItem.Value, cmbTeam.SelectedItem.Value, txtTeamList.Text.Trim());

                var allJob = newJobList.GroupBy(p => new { p.Team, p.LastOwner }).Select(p => new AmountByTeamAndLastOwnerData { Team = p.Key.Team, LastOwner = p.Key.LastOwner, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamAndLastOwnerData data in allJob)
                {
                    UserMonitoringData mtData = mainList.Where(p => p.Team.Trim() == data.Team && p.EmpCode == data.LastOwner).FirstOrDefault();
                    if (mtData != null)
                        mtData.AmountNewJobNew = data.Amount != null ? data.Amount : 0;
                }

                //งานที่รับแล้ว
                List<UserMonitoringNewJobData> assignedJobList = LeadBiz.GetUserMonitoringNewJobAssignedList(cmbStaffStatus.SelectedItem.Value, cmbTeam.SelectedItem.Value, txtTeamList.Text.Trim());

                var allAssignedJob = assignedJobList.GroupBy(p => new { p.Team, p.LastOwner }).Select(p => new AmountByTeamAndLastOwnerData { Team = p.Key.Team, LastOwner = p.Key.LastOwner, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                foreach (AmountByTeamAndLastOwnerData data in allAssignedJob)
                {
                    UserMonitoringData mtData = mainList.Where(p => p.Team.Trim() == data.Team && p.EmpCode == data.LastOwner).FirstOrDefault();
                    if (mtData != null)
                        mtData.AmountNewJobOnHand = data.Amount != null ? data.Amount : 0;
                }

                //งานระหว่างดำเนินการทั้งหมด
                foreach (UserMonitoringData data in mainList)
                {
                    data.AmountNewJobAll = data.AmountNewJobNew + data.AmountNewJobOnHand;
                }

                #region Backup
                //งานทั้งหมด
                //var allJob = newJobList.GroupBy(p => new { p.Team, p.LastOwner }).Select(p => new AmountByTeamAndLastOwnerData { Team = p.Key.Team, LastOwner = p.Key.LastOwner, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                //foreach (AmountByTeamAndLastOwnerData data in allJob)
                //{
                //    UserMonitoringData mtData = mainList.Where(p => p.Team.Trim() == data.Team && p.EmpCode == data.LastOwner).FirstOrDefault();
                //    if (mtData != null)
                //        mtData.AmountNewJobAll = data.Amount != null ? data.Amount : 0;
                //}

                ////งานเก่า
                //var tmp1 = newJobList.Where(p => p.FlowLogId != null).ToList();
                //var oldJob = tmp1.GroupBy(p => new { p.Team, p.LastOwner }).Select(p => new AmountByTeamAndLastOwnerData { Team = p.Key.Team, LastOwner = p.Key.LastOwner, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                //foreach (AmountByTeamAndLastOwnerData data in oldJob)
                //{
                //    UserMonitoringData mtData = mainList.Where(p => p.Team.Trim() == data.Team && p.EmpCode == data.LastOwner).FirstOrDefault();
                //    if (mtData != null)
                //        mtData.AmountNewJobOnHand = data.Amount != null ? data.Amount : 0;
                //}

                ////งานใหม่
                //var tmp2 = newJobList.Where(p => p.FlowLogId == null).ToList();
                //var newJob = tmp2.GroupBy(p => new { p.Team, p.LastOwner }).Select(p => new AmountByTeamAndLastOwnerData { Team = p.Key.Team, LastOwner = p.Key.LastOwner, Amount = p.Count() }).OrderBy(p => p.Team).ToList();
                //foreach (AmountByTeamAndLastOwnerData data in newJob)
                //{
                //    UserMonitoringData mtData = mainList.Where(p => p.Team.Trim() == data.Team && p.EmpCode == data.LastOwner).FirstOrDefault();
                //    if (mtData != null)
                //        mtData.AmountNewJobNew = data.Amount != null ? data.Amount : 0;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetUserMonitoringDoneJob(List<UserMonitoringData> mainList)
        {
            try
            {
                List<FlowLogData> flowLogList = FlowLogBiz.GetFlowLogList(DateTime.Now);
                if (flowLogList.Count > 0)
                {
                    foreach (UserMonitoringData mainData in mainList)
                    {
                        mainData.AmountDoneJobForward = flowLogList.Where(p => p.FlowType == "F" && p.TeamFrom.Trim() == mainData.Team.Trim() && p.TeamFromEmpCode == mainData.EmpCode).Count();
                        mainData.AmountDoneJobRouteBackCoc = flowLogList.Where(p => p.FlowType == "R" && p.TeamTo.Contains("COC") && p.TeamFrom.Trim() == mainData.Team.Trim() && p.TeamFromEmpCode == mainData.EmpCode).Count();
                        mainData.AmountDoneJobRouteBackMkt = flowLogList.Where(p => p.FlowType == "R" && p.TeamTo.Contains("MARKETING") && p.TeamFrom.Trim() == mainData.Team.Trim() && p.TeamFromEmpCode == mainData.EmpCode).Count();
                        mainData.AmountDoneJobAll = mainData.AmountDoneJobForward + mainData.AmountDoneJobRouteBackCoc + mainData.AmountDoneJobRouteBackMkt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSetStatus_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnSetStatus = ((Button)sender);
                int index = Convert.ToInt32(btnSetStatus.CommandArgument);
                string empCode = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();
                int newStatus = (btnSetStatus.Text.Trim().ToUpper() == "UNAVAILABLE" ? 0 : 1);

                StaffBiz.SetStaffStatus(empCode, newStatus, HttpContext.Current.User.Identity.Name);

                if (newStatus == 0)
                {
                    ((Image)rptUserMonitoring.Items[index].FindControl("imgAvailable")).Visible = false;
                    ((Image)rptUserMonitoring.Items[index].FindControl("imgNotAvailable")).Visible = true;

                    ((Label)rptUserMonitoring.Items[index].FindControl("lblStatusDesc")).Text = "Unavailable";
                    
                    if (((LinkButton)rptUserMonitoring.Items[index].FindControl("lbNewJobAll")).Text.Trim() == "0")
                        ((Button)rptUserMonitoring.Items[index].FindControl("btnUserMonitoringTranferJob")).Visible = false;
                    else
                        ((Button)rptUserMonitoring.Items[index].FindControl("btnUserMonitoringTranferJob")).Visible = true;
                        
                    btnSetStatus.Text = "Available";
                }
                else if (newStatus == 1)
                {
                    ((Image)rptUserMonitoring.Items[index].FindControl("imgAvailable")).Visible = true;
                    ((Image)rptUserMonitoring.Items[index].FindControl("imgNotAvailable")).Visible = false;

                    ((Label)rptUserMonitoring.Items[index].FindControl("lblStatusDesc")).Text = "Available";
                    ((Button)rptUserMonitoring.Items[index].FindControl("btnUserMonitoringTranferJob")).Visible = false;

                    btnSetStatus.Text = "Unavailable";
                }

                if (txtStaffStatusSelected.Text != "")                      //ถ้าเป็นช่องว่างแปลว่าเลือกทั้งหมด
                    rptUserMonitoring.Items[index].Visible = false;

            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbNewJobNew_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                string cocTeam = ((Label)rptUserMonitoring.Items[index].FindControl("lblTeam")).Text.Trim();
                string empCode = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();

                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetUserMonitoringNewJobListForPopup(cocTeam, empCode, "waitassign");

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbNewJobOnHand_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                string cocTeam = ((Label)rptUserMonitoring.Items[index].FindControl("lblTeam")).Text.Trim();
                string empCode = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();

                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetUserMonitoringNewJobListForPopup(cocTeam, empCode, "assigned");

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbNewJobAll_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                string cocTeam = ((Label)rptUserMonitoring.Items[index].FindControl("lblTeam")).Text.Trim();
                string empCode = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();

                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetUserMonitoringNewJobListForPopup(cocTeam, empCode, "");

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnUserMonitoringTranferJob_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(((Button)sender).CommandArgument);
                string cocTeam = ((Label)rptUserMonitoring.Items[index].FindControl("lblTeam")).Text.Trim();
                string lastOwner = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();

                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetUserMonitoringNewJobListForPopup(cocTeam, lastOwner, "");

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);

                cmbStaffTranserJob.DataSource = StaffBiz.GetStaffByTeam(cocTeam);
                cmbStaffTranserJob.DataTextField = "TextField";
                cmbStaffTranserJob.DataValueField = "ValueField";
                cmbStaffTranserJob.DataBind();
                cmbStaffTranserJob.Items.Remove(cmbStaffTranserJob.Items.FindByValue(lastOwner));
                cmbStaffTranserJob.Items.Insert(0, new ListItem("", ""));

                SetDisplayTransferSection(true);

                txtPopupCocTeam.Text = cocTeam;
                txtPopupLastOwner.Text = lastOwner;
                cbDoRefreshAppInfo.Checked = false;
                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbDoneJobForward_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                string cocTeam = ((Label)rptUserMonitoring.Items[index].FindControl("lblTeam")).Text.Trim();
                string lastOwner = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();

                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetUserMonitoringDoneJobListForPopup(DateTime.Now, cocTeam, lastOwner, "F", "");

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);
                gvPopup.Columns[colIndex_DoneJobType].Visible = true;   //ประเภทงานเสร็จ
                gvPopup.Columns[colIndex_ActionDate].Visible = true;    //วันที่ Action
                gvPopup.Columns[colIndex_JobType].Visible = false;      //ประเภทงาน
                gvPopup.Columns[colIndex_CocAssignTypeDesc].Visible = false;      //CocAssignTypeDesc

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbDoneJobRouteBackCoc_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                string cocTeam = ((Label)rptUserMonitoring.Items[index].FindControl("lblTeam")).Text.Trim();
                string lastOwner = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();

                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetUserMonitoringDoneJobListForPopup(DateTime.Now, cocTeam, lastOwner, "R", "COC");

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);
                gvPopup.Columns[colIndex_DoneJobType].Visible = true;
                gvPopup.Columns[colIndex_ActionDate].Visible = true;
                gvPopup.Columns[colIndex_JobType].Visible = false;      //ประเภทงาน
                gvPopup.Columns[colIndex_CocAssignTypeDesc].Visible = false;      //CocAssignTypeDesc

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbDoneJobRouteBackMkt_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                string cocTeam = ((Label)rptUserMonitoring.Items[index].FindControl("lblTeam")).Text.Trim();
                string lastOwner = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();

                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetUserMonitoringDoneJobListForPopup(DateTime.Now, cocTeam, lastOwner, "R", "MARKETING");

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);
                gvPopup.Columns[colIndex_DoneJobType].Visible = true;
                gvPopup.Columns[colIndex_ActionDate].Visible = true;
                gvPopup.Columns[colIndex_JobType].Visible = false;      //ประเภทงาน
                gvPopup.Columns[colIndex_CocAssignTypeDesc].Visible = false;      //CocAssignTypeDesc

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbDoneJobAll_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                string cocTeam = ((Label)rptUserMonitoring.Items[index].FindControl("lblTeam")).Text.Trim();
                string lastOwner = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();

                List<LeadDataPopupMonitoring> mainList = LeadBiz.GetUserMonitoringDoneJobListForPopup(DateTime.Now, cocTeam, lastOwner, "", "");

                Session[LeadListPopup] = mainList;
                BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                SetDisplayTransferSection(false);
                gvPopup.Columns[colIndex_DoneJobType].Visible = true;
                gvPopup.Columns[colIndex_ActionDate].Visible = true;
                gvPopup.Columns[colIndex_JobType].Visible = false;      //ประเภทงาน
                gvPopup.Columns[colIndex_CocAssignTypeDesc].Visible = false;      //CocAssignTypeDesc

                upPopup.Update();
                mpePopup.Show();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbAllJob_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    int index = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                    string cocTeam = ((Label)rptUserMonitoring.Items[index].FindControl("lblTeam")).Text.Trim();
                    string lastOwner = ((Label)rptUserMonitoring.Items[index].FindControl("lblEmpCode")).Text.Trim();

                    List<LeadDataPopupMonitoring> allNewJobList = LeadBiz.GetUserMonitoringNewJobListForPopup(cocTeam, lastOwner, "");
                    List<LeadDataPopupMonitoring> allDoneJobList = LeadBiz.GetUserMonitoringDoneJobListForPopup(DateTime.Now, cocTeam, lastOwner, "", "");

                    List<LeadDataPopupMonitoring> allJob = allNewJobList.Union(allDoneJobList).OrderBy(p => p.CocAssignedDate).ToList();

                    Session[LeadListPopup] = allJob;
                    BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, allJob.ToArray(), 0);
                    SetDisplayTransferSection(false);

                    upPopup.Update();
                    mpePopup.Show();
                }
                catch (Exception ex)
                {
                    string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    _log.Debug(message);
                    AppUtil.ClientAlert(Page, message);
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        #region Page Control

        private void BindPopupGridview(COC.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvPopup);
            pageControl.Update(items, pageIndex);
            pageControl.GenerateRecordNumber(2, pageIndex);
            upPopup.Update();
            mpePopup.Show();
        }

        protected void PageSearchChangePopup(object sender, EventArgs e)
        {
            try
            {
                if (Session[LeadListPopup] != null)
                {
                    List<LeadDataPopupMonitoring> list = (List<LeadDataPopupMonitoring>)Session[LeadListPopup];
                    var pageControl = (COC.Application.Shared.GridviewPageController)sender;
                    BindPopupGridview(pageControl, list.ToArray(), pageControl.SelectedPageIndex);
                }
                else
                {
                    AppUtil.ClientAlert(Page, "Session LeadListPopup Expired");
                    mpePopup.Hide();
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        #region Popup Transfer Job

        protected void btnPopupTranserJob_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbStaffTranserJob.SelectedItem.Value == "")
                {
                    mpePopup.Show();
                    AppUtil.ClientAlert(Page, "กรุณาเลือกพนักงานที่จะโอนงานให้");
                    return;
                }

                List<string> ticketIdList = new List<string>();
                foreach (GridViewRow row in gvPopup.Rows)
                {
                    if (((CheckBox)row.FindControl("cbTranserJob")).Checked)
                        ticketIdList.Add(((Label)row.FindControl("lblTicketId")).Text.Trim());
                }

                if (ticketIdList.Count > 0)
                {
                    cbDoRefreshAppInfo.Checked = true;
                    LeadBiz.TransferJob(ticketIdList, cmbStaffTranserJob.SelectedItem.Value, txtPopupCocTeam.Text.Trim(), HttpContext.Current.User.Identity.Name);

                    List<LeadDataPopupMonitoring> mainList = null;
                    if (txtPopupCocTeam.Text.Trim() != "" && txtPopupLastOwner.Text.Trim() != "")
                    {
                        mainList = LeadBiz.GetUserMonitoringNewJobListForPopup(txtPopupCocTeam.Text.Trim(), txtPopupLastOwner.Text.Trim(), "");
                    }
                    else
                    {
                        mainList = LeadBiz.GetNumberOfAppInPoolAllJobByTeam(txtPopupCocTeam.Text.Trim());
                    }

                    Session[LeadListPopup] = mainList;
                    BindPopupGridview((COC.Application.Shared.GridviewPageController)pcPopup, mainList.ToArray(), 0);
                    SetDisplayTransferSection(true);

                    upPopup.Update();
                    mpePopup.Show();

                    AppUtil.ClientAlert(Page, "โอนงานเรียบร้อย");
                }
                else
                {
                    mpePopup.Show();
                    AppUtil.ClientAlert(Page, "กรุณาเลือกงานที่ต้องการโอน");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnPopupTransferJobClose_Click(object sender, EventArgs e)
        {
            try
            {
                gvPopup.Columns[colIndex_DoneJobType].Visible = false;
                gvPopup.Columns[colIndex_ActionDate].Visible = false;
                gvPopup.Columns[colIndex_JobType].Visible = true;      //ประเภทงาน
                gvPopup.Columns[colIndex_CocAssignTypeDesc].Visible = true;      //CocAssignTypeDesc

                if (cbDoRefreshAppInfo.Checked)
                {
                    GetAppInfoData();
                    upAppInfo.Update();

                    GetUserMonitoringList();
                    upUserMonitoring.Update();

                    //Clear ค่าบน popup
                    txtPopupCocTeam.Text = "";
                    txtPopupLastOwner.Text = "";
                    cbDoRefreshAppInfo.Checked = false;
                }

                mpePopup.Hide();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion


        //=============================================================================================================================

    }
}