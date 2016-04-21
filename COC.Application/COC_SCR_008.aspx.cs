using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Application.Utilities;
using COC.Biz;
using log4net;
using COC.Resource.Data;

namespace COC.Application
{
    public partial class COC_SCR_008 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_008));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "รายงาน 2";
            Page.Form.DefaultButton = btnSearch.UniqueID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_008");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_002.aspx");
                        return;
                    }

                    txtTeamList.Text = AppUtil.GetRecursiveTeam(HttpContext.Current.User.Identity.Name);
                    //tdAssignDateFrom.DateValue = DateTime.Now;
                    //tdAssignDateTo.DateValue = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (tdAssignDateFrom.DateValue.Year == 1 || tdAssignDateTo.DateValue.Year == 1)
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุช่วงเวลาค้นหาให้ครบถ้วน");
                    return;
                }

                if (tdAssignDateFrom.DateValue.Year.ToString() != "1" && tdAssignDateTo.DateValue.Year.ToString() != "1")
                {
                    if (tdAssignDateFrom.DateValue > tdAssignDateTo.DateValue)
                    {
                        AppUtil.ClientAlert(Page, "วันที่เริ่มต้นต้องน้อยกว่าหรือเท่ากับวันที่สิ้นสุด");
                        return;
                    }
                }

                if (tdAssignDateFrom.DateValue > DateTime.Today || tdAssignDateTo.DateValue > DateTime.Today)
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถเลือกช่วงเวลาเกินวันปัจจุบันได้");
                    return;
                }

                TimeSpan ts = tdAssignDateTo.DateValue.Subtract(tdAssignDateFrom.DateValue);
                if ((ts.TotalDays + 1) > AppConstant.SnapReportDateRange)
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถเลือกช่วงเวลาได้เกิน " + AppConstant.SnapReportDateRange + " วัน");
                    return;
                }

                //ถ้าคนที่ login ทำการ recursive แล้วไม่พบลูกน้อง จะไม่สามารถค้นหาได้
                if (txtTeamList.Text.Trim() != "")
                {
                    bool ret = GetAppInfoData(tdAssignDateFrom.DateValue, tdAssignDateTo.DateValue, txtTeamList.Text.Trim());
                    if (ret)
                        GetUserMonitoringData(tdAssignDateFrom.DateValue, tdAssignDateTo.DateValue, txtTeamList.Text.Trim());
                    else
                    {
                        rptAppInfo.DataSource = null;
                        rptAppInfo.DataBind();
                        imgAppInfo.Visible = false;

                        rptUserMonitoring.DataSource = null;
                        rptUserMonitoring.DataBind();
                        imgUserMonitoring.Visible = false;

                        AppUtil.ClientAlert(Page, "ไม่พบข้อมูล");
                        return;
                    }
                }
                else
                {
                    AppUtil.ClientAlert(Page, "ไม่พบข้อมูล");
                    return;
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private bool GetAppInfoData(DateTime assignDateFrom, DateTime assignDateTo, string teamList)
        {
            try
            {
                List<AppInfoData> list = SnapBiz.GetSnapWaitDetailList(assignDateFrom, assignDateTo, teamList);
                if (list.Count > 0)
                {
                    rptAppInfo.DataSource = list;
                    rptAppInfo.DataBind();
                    imgAppInfo.Visible = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetUserMonitoringData(DateTime assignDateFrom, DateTime assignDateTo, string teamList)
        {
            try
            {
                List<UserMonitoringSnapData> list = SnapBiz.GetSnapUserMonitoringList(assignDateFrom, assignDateTo, teamList);

                foreach (UserMonitoringSnapData data in list)
                {
                    data.WorkingMinDisplay = "0.00";

                    if (data.WorkingSec > 0)
                    {
                        data.AvgSuccessPerHour = Math.Round((Convert.ToDecimal(data.AmountDoneJobForward) / Convert.ToDecimal(data.WorkingSec)) * 3600, 2).ToString("#,##0.00");
                        data.AvgTotalPerHour = Math.Round((Convert.ToDecimal(data.AmountDoneJobAll) / Convert.ToDecimal(data.WorkingSec)) * 3600, 2).ToString("#,##0.00");

                        int min = data.WorkingSec.Value / 60;                          //หานาที
                        int seconds = data.WorkingSec.Value - (min * 60);              //หาวินาทีที่เหลือ

                        decimal workingMin = Math.Round(Convert.ToDecimal(min.ToString("00") + "." + seconds.ToString("00")), 2);

                        data.WorkingMinDisplay = workingMin.ToString("0.00");
                    }

                    //data.WorkingHourDisplay = data.WorkingHour != null ? data.WorkingHour.Value.ToString("0") : "0";  
                    //workingMin = data.WorkingHour != null ? data.WorkingHour.Value : 0;  

                    //if (workingMin > 0)
                    //{
                    //    if (workingMin < 60)
                    //        workingMin = 60;

                    //    workingHour = Math.Round((workingMin / 60), 2);

                    //    data.AvgSuccessPerHour = (Math.Round((data.AmountDoneJobAll.Value / workingHour), 2)).ToString("0.00");
                    //    data.AvgTotalPerHour = (Math.Round((data.AmountAllJob.Value / workingHour), 2)).ToString("0.00");
                    //}
                }

                rptUserMonitoring.DataSource = list;
                rptUserMonitoring.DataBind();
                imgUserMonitoring.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (tdAssignDateFrom.DateValue.Year == 1 || tdAssignDateTo.DateValue.Year == 1)
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุช่วงเวลาค้นหาให้ครบถ้วน");
                    return;
                }

                TimeSpan ts = tdAssignDateTo.DateValue.Subtract(tdAssignDateFrom.DateValue);
                if ((ts.TotalDays + 1) > AppConstant.SnapReportDateRange)
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถเลือกช่วงเวลาได้เกิน " + AppConstant.SnapReportDateRange + " วัน");
                    return;
                }

                string datefrom = tdAssignDateFrom.DateValue.Year.ToString() + tdAssignDateFrom.DateValue.ToString("-MM-dd");
                string dateto = tdAssignDateTo.DateValue.Year.ToString() + tdAssignDateTo.DateValue.ToString("-MM-dd");
                Int64 countrow = 0;

                countrow = ReportBiz.CheckReportStaffWorkingExist(datefrom, dateto);
                if (countrow == 0)
                    AppUtil.ClientAlert(Page, "ไม่พบข้อมูล");
                else
                {
                    string script = "window.open('" + Page.ResolveUrl("~/ExportReport.aspx?reportflag=RPT_SNAP_MONITORING&teamlist=" + Server.UrlEncode(txtTeamList.Text.Trim()) + "&datefrom=" + datefrom + "&dateto=" + dateto) + "', 'reports', 'status=yes, toolbar=no, scrollbars=no, menubar=no, width=600, height=400, resizable=no');";
                    //string script = "window.open('" + Request.ApplicationPath + "ExportReport.aspx?reportflag=RPT_SNAP_MONITORING&teamlist=" + Server.UrlEncode(txtTeamList.Text.Trim()) + "&datefrom=" + datefrom + "&dateto=" + dateto + "', 'reports', 'status=yes, toolbar=no, scrollbars=no, menubar=no, width=800, height=600, resizable=no');";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "reportsnapmonitoring", script, true);
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
    }

}