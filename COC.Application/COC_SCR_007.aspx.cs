using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Application.Utilities;
using log4net;
using COC.Biz;
using COC.Resource.Data;
using COC.Resource;

namespace COC.Application
{
    public partial class COC_SCR_007 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_007));
        private string coc_staffid = "staffid";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "แก้ไขข้อมูลพนักงาน";
            Page.Form.DefaultButton = btnSave.UniqueID;
            AppUtil.SetIntTextBox(txtEmpCode);
            AppUtil.SetIntTextBox(txtMarketingCode);
            AppUtil.SetIntTextBox(txtTellNo);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_007");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_002.aspx");
                        return;
                    }

                    if (Session[coc_staffid] != null)
                    {
                        txtStaffId.Text = Session[coc_staffid].ToString();
                        InitialControl();
                        LoadStaffData();
                        CheckConditionOper();
                        SetDept();

                        //งานค้างในมือ
                        GetStaffByTeam(txtCurrentCocTeam.Text.Trim());  //set ค่า txtCurrentCocTeam.Text ใน Method LoadStaffData();
                        GetJobOnHandList();
                    }
                    else
                    {
                        if (txtStaffId.Text.Trim() == string.Empty)
                            AppUtil.ClientAlertAndRedirect(Page, "Staff Id not found", "COC_SCR_006.aspx");
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

        private void SetDept()
        {
            //decimal? stafftype = StaffBiz.GetStaffType(HttpContext.Current.User.Identity.Name);
            //if (stafftype != null)
            //{
            //    if (stafftype == COCConstant.StaffType.ITAdministrator)
            //        cmbDepartment.Enabled = true;
            //    else
            //    {
                    cmbDepartment.Enabled = false;
            //    }
            //}
        }

        private void InitialControl()
        {
            ////Role
            cmbStaffType.DataSource = StaffBiz.GetStaffTyeList();
            cmbStaffType.DataTextField = "TextField";
            cmbStaffType.DataValueField = "ValueField";
            cmbStaffType.DataBind();
            cmbStaffType.Items.Insert(0, new ListItem("", ""));

            //Market Branch
            cmbBranchCode.DataSource = BranchBiz.GetBranchList();
            cmbBranchCode.DataTextField = "TextField";
            cmbBranchCode.DataValueField = "ValueField";
            cmbBranchCode.DataBind();
            cmbBranchCode.Items.Insert(0, new ListItem("", ""));

            //Department
            cmbDepartment.DataSource = DepartmentBiz.GetDepartmentList();
            cmbDepartment.DataTextField = "TextField";
            cmbDepartment.DataValueField = "ValueField";
            cmbDepartment.DataBind();
            cmbDepartment.Items.Insert(0, new ListItem("", ""));

            ////COC Team
            cmbCocTeam.DataSource = StaffBiz.GetTeamList();
            cmbCocTeam.DataTextField = "TextField";
            cmbCocTeam.DataValueField = "ValueField";
            cmbCocTeam.DataBind();
            cmbCocTeam.Items.Insert(0, new ListItem("", ""));

            //Position
            cmbPosition.DataSource = PositionBiz.GetPositionList(COCConstant.Position.Active);
            cmbPosition.DataTextField = "TextField";
            cmbPosition.DataValueField = "ValueField";
            cmbPosition.DataBind();
            cmbPosition.Items.Insert(0, new ListItem("", ""));
        }
        private void LoadStaffData()
        {
            try
            {
                StaffDataManagement staff = new StaffDataManagement();
                if (txtStaffId.Text.Trim() != "")
                    staff = StaffBiz.GetStaff(int.Parse(txtStaffId.Text.Trim()));

                if (staff != null)
                {
                    txtUsername.Text = staff.Username;
                    lblUsername.Text = staff.Username;
                    txtEmpCode.Text = staff.EmpCode;
                    txtMarketingCode.Text = staff.MarketingCode;
                    txtStaffNameTH.Text = staff.StaffNameTH;
                    txtTellNo.Text = staff.TelNo;
                    txtStaffEmail.Text = staff.StaffEmail;
                    cmbPosition.SelectedIndex = cmbPosition.Items.IndexOf(cmbPosition.Items.FindByValue(staff.PositionId));
                    if (staff.StaffTypeId != null)
                        cmbStaffType.SelectedIndex = cmbStaffType.Items.IndexOf(cmbStaffType.Items.FindByValue(staff.StaffTypeId.ToString()));
                    txtTeam.Text = staff.Team;
                    cmbBranchCode.SelectedIndex = cmbBranchCode.Items.IndexOf(cmbBranchCode.Items.FindByValue(staff.BranchCode));
                    txtOldBranchCode.Text = staff.BranchCode;
                    cmbBranchCode_SelectedIndexChanged();

                    if (staff.HeadStaffId != null)
                        cmbHeadStaffId.SelectedIndex = cmbHeadStaffId.Items.IndexOf(cmbHeadStaffId.Items.FindByValue(staff.HeadStaffId.ToString()));
                    if (staff.DepartmentId != null)
                        cmbDepartment.SelectedIndex = cmbDepartment.Items.IndexOf(cmbDepartment.Items.FindByValue(staff.DepartmentId.ToString()));
                    if (!string.IsNullOrEmpty(staff.CocTeam) && cmbCocTeam.Items.Count > 0)
                    {
                        cmbCocTeam.SelectedIndex = cmbCocTeam.Items.IndexOf(cmbCocTeam.Items.FindByValue(staff.CocTeam));
                        txtCurrentCocTeam.Text = staff.CocTeam;
                    }
                    else
                        txtCurrentCocTeam.Text = "";

                    if (staff.Is_Deleted != null)
                    {
                        txtOldIsDeleted.Text = staff.Is_Deleted.ToString();
                        txtNewIsDeleted.Text = staff.Is_Deleted.ToString();
                        if (staff.Is_Deleted == 0)
                        {
                            rdNormal.Checked = true;
                        }
                        else if (staff.Is_Deleted == 1)
                        {
                            rdRetire.Checked = true;
                        }
                        else
                        {
                            rdNormal.Checked = false;
                            rdRetire.Checked = false;
                        }
                    }
                }
                upInfo.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cmbBranchCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbBranchCode_SelectedIndexChanged();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void cmbBranchCode_SelectedIndexChanged()
        {
            var list = StaffBiz.GetStaffHeadList(cmbBranchCode.SelectedItem.Value);
            var editedStaff = list.Where(p => p.ValueField == txtStaffId.Text.Trim()).FirstOrDefault();
            list.Remove(editedStaff);

            cmbHeadStaffId.DataSource = list;
            cmbHeadStaffId.DataTextField = "TextField";
            cmbHeadStaffId.DataValueField = "ValueField";
            cmbHeadStaffId.DataBind();
            cmbHeadStaffId.Items.Insert(0, new ListItem("", ""));
        }

        private bool ValidateData()
        {
            int i = 0;
            //************************************Windows Username********************************************
            if (txtUsername.Text.Trim() == "")
            {
                vtxtUsername.Text = "กรุณาระบุ Windows Username";
                vtxtUsername.ForeColor = System.Drawing.Color.Red;
                i += 1;
            }
            else
            {
                vtxtUsername.Text = "";
                if (StaffBiz.CheckUsernameExist(txtUsername.Text.Trim(), int.Parse(txtStaffId.Text.Trim())))
                {
                    vtxtUsername.Text = "Windows Username นี้มีอยู่แล้วในระบบแล้ว";
                    vtxtUsername.ForeColor = System.Drawing.Color.Red;
                    i += 1;
                }
                else
                    vtxtUsername.Text = "";
            }

            //************************************รหัสพนักงานธนาคาร********************************************
            if (txtEmpCode.Text.Trim() == "")
            {
                vtxtEmpCode.Text = "กรุณาระบุรหัสพนักงานธนาคาร";
                i += 1;
            }
            else
            {
                vtxtEmpCode.Text = "";
                if (StaffBiz.CheckEmpCodeExist(txtEmpCode.Text.Trim(), int.Parse(txtStaffId.Text.Trim())))
                {
                    vtxtEmpCode.Text = "รหัสพนักงานธนาคารนี้มีอยู่แล้วในระบบแล้ว";
                    i += 1;
                }
                else
                    vtxtEmpCode.Text = "";
            }

            //************************************รหัสเจ้าหน้าที่การตลาด********************************************
            if (txtMarketingCode.Text.Trim() == "")
            {
                //vtxtMarketingCode.Text = "กรุณาระบุรหัสเจ้าหน้าที่การตลาด";
                //i += 1;
            }
            else
            {
                vtxtMarketingCode.Text = "";
                if (StaffBiz.CheckMarketingCodeExist(txtMarketingCode.Text.Trim(), int.Parse(txtStaffId.Text.Trim())))
                {
                    vtxtMarketingCode.Text = "รหัสเจ้าหน้าที่การตลาดนี้มีอยู่แล้วในระบบแล้ว";
                    i += 1;
                }
                else
                    vtxtMarketingCode.Text = "";
            }

            //************************************ชื่อ-นามสกุลพนักงาน********************************************
            if (txtStaffNameTH.Text.Trim() == "")
            {
                vtxtStaffNameTH.Text = "กรุณาระบุชื่อ-นามสกุลพนักงาน";
                i += 1;
            }
            else
                vtxtStaffNameTH.Text = "";

            //************************************E-mail********************************************
            if (txtStaffEmail.Text.Trim() == "")
            {
                vtxtStaffEmail.Text = "กรุณาระบุ E-mail";
                i += 1;
            }
            else
            {
                if (!ValidateEmail())
                {
                    vtxtStaffEmail.Text = "กรุณาระบุ E-mail ให้ถูกต้อง";
                    i += 1;
                }
                else
                    vtxtStaffEmail.Text = "";
            }

            //************************************ตำแหน่ง********************************************
            if (cmbPosition.SelectedItem.Value == "")
            {
                vtxtPositionName.Text = "กรุณาระบุ ตำแหน่ง";
                i += 1;
            }
            else
                vtxtPositionName.Text = "";

            //************************************Role********************************************
            if (cmbStaffType.SelectedItem.Value == "")
            {
                vcmbStaffType.Text = "กรุณาระบุ Role";
                i += 1;
            }
            else
                vcmbStaffType.Text = "";

            //************************************ทีมการตลาด********************************************
            //if (txtTeam.Text.Trim() == "")
            //{
            //    vtxtTeam.Text = "กรุณาระบุ ทีมการตลาด";
            //    i += 1;
            //}
            //else
            //    vtxtTeam.Text = "";

            //************************************สาขา********************************************
            if (cmbBranchCode.SelectedItem.Value == "")
            {
                vcmbBranchCode.Text = "กรุณาระบุ สาขา";
                i += 1;
            }
            else
                vcmbBranchCode.Text = "";

            //************************************ COC Team ********************************************
            if (cmbStaffType.SelectedItem.Value == COCConstant.StaffType.Oper.ToString() || cmbStaffType.SelectedItem.Value == COCConstant.StaffType.SupervisorOper.ToString())
            {
                if (cmbCocTeam.SelectedItem.Value == "")
                {
                    vCocTeam.Text = "กรุณาระบุ COC Team";
                    i += 1;
                }
                else
                    vCocTeam.Text = "";
            }
            else
                vCocTeam.Text = "";

            if (i > 0)
                return false;
            else
                return true;
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ValidateEmail() == false && txtStaffEmail.Text.Trim() != "")
                    vtxtStaffEmail.Text = "กรุณาระบุ E-mail ให้ถูกต้อง";
                else
                    vtxtStaffEmail.Text = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateEmail()
        {
            string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern);
            return reg.IsMatch(txtStaffEmail.Text.Trim());

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string desc;
                if (LeadBiz.CheckHeadStaff(txtStaffId.Text, cmbHeadStaffId.SelectedValue.Trim(), out desc))
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนหัวหน้างานได้เนื่องจาก " + txtStaffNameTH.Text + "เป็นหัวหน้างาน" + cmbHeadStaffId.Text.Trim());
                    return;
                }
                //เช็ก Lead on hand ในส่วนของ slm
                if (cmbBranchCode.SelectedItem.Value != txtOldBranchCode.Text.Trim())
                {
                    if (LeadBiz.CheckExistLeadOnHand(txtUsername.Text.Trim(), txtEmpCode.Text.Trim()) == false)
                    {
                        SaveData();
                    }
                    else
                    {
                        AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนข้อมูลสาขาได้ เนื่องจากยังมีงานค้างอยู่");
                        return;
                    }
                }
                else if (txtOldIsDeleted.Text.Trim() != txtNewIsDeleted.Text.Trim())
                {
                    if (LeadBiz.CheckExistLeadOnHand(txtUsername.Text.Trim(), txtEmpCode.Text.Trim()) == false)
                    {
                        SaveData();
                    }
                    else
                    {
                        AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนสถานะพนักงานได้ เนื่องจากยังมีงานค้างอยู่");
                        return;
                    }
                }
                else if (cmbCocTeam.SelectedItem.Value != txtCurrentCocTeam.Text.Trim())
                {
                    //if (LeadBiz.CheckLastOwnerOnHand(txtEmpCode.Text.Trim()) == false)
                    //{
                    //    SaveData();
                    //}
                    //else
                    //{
                    //    AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนทีมพนักงานได้ เนื่องจากยังมีงานค้างอยู่");
                    //}

                    List<LeadDataPopupMonitoring> list = DoGetJobOnHandList();
                    if (list.Count > 0)
                    {
                        AppUtil.ClientAlert(Page, "ไม่สามารถเปลี่ยนทีมพนักงานได้ เนื่องจากยังมีงานค้างอยู่");
                        return;
                    }
                    else
                    {
                        string oldCocTeam = txtCurrentCocTeam.Text.Trim();
                        SaveData();

                        _log.Debug("==================== Start Log ====================");
                        _log.Debug("Action: Change COC Team");
                        _log.Debug("ActionBy: " + HttpContext.Current.User.Identity.Name);
                        _log.Debug("DateTime: " + DateTime.Now.ToString("dd-MM-") + DateTime.Now.Year.ToString() + " " + DateTime.Now.ToString("HH:mm:ss"));
                        _log.Debug("Username: " + txtUsername.Text.Trim());
                        _log.Debug("TeamFrom: " + oldCocTeam);
                        _log.Debug("TeamTo: " + cmbCocTeam.SelectedItem.Value);
                        _log.Debug("==================== End Log =====================");
                        _log.Debug(" ");
                    }
                }
                else
                    SaveData();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void SaveData()
        {
            try
            {
                if (ValidateData())
                {
                    int flag = 0;
                    StaffDataManagement data = new StaffDataManagement();
                    data.Username = txtUsername.Text.Trim();
                    data.EmpCode = txtEmpCode.Text.Trim();
                    data.MarketingCode = txtMarketingCode.Text.Trim();
                    data.StaffNameTH = txtStaffNameTH.Text.Trim();
                    data.TelNo = txtTellNo.Text.Trim();
                    data.StaffEmail = txtStaffEmail.Text.Trim();
                    data.PositionId = cmbPosition.SelectedItem.Value;
                    data.StaffTypeId = decimal.Parse(cmbStaffType.SelectedItem.Value);
                    data.Team = txtTeam.Text.Trim();
                    data.BranchCode = cmbBranchCode.SelectedItem.Value;
                    data.StaffId = int.Parse(txtStaffId.Text.Trim());
                    if (rdNormal.Checked == true)
                        data.Is_Deleted = 0;
                    else if (rdRetire.Checked == true)
                        data.Is_Deleted = 1;

                    if (cmbHeadStaffId.Items.Count > 0 && cmbHeadStaffId.SelectedItem.Value != "")
                        data.HeadStaffId = int.Parse(cmbHeadStaffId.SelectedItem.Value);
                    else
                        data.HeadStaffId = null;

                    if (cmbDepartment.Items.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(cmbDepartment.SelectedItem.Value)) { data.DepartmentId = int.Parse(cmbDepartment.SelectedItem.Value); }
                    }
                    if (cmbCocTeam.Items.Count > 0)
                    {
                        data.CocTeam = cmbCocTeam.SelectedItem.Value;
                    }

                    if (txtOldIsDeleted.Text.Trim() != txtNewIsDeleted.Text.Trim())
                    {
                        flag = 1;
                    }

                    string staffId = StaffBiz.UpdateStaff(data, HttpContext.Current.User.Identity.Name, flag);

                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลเจ้าหน้าที่สำเร็จ");
                    txtStaffId.Text = staffId;
                    InitialControl();
                    LoadStaffData();
                    CheckConditionOper();
                    SetDept();
                    upInfo.Update();
                }
                else
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุข้อมูลให้ครบถ้วน");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Session[coc_staffid] = null;
                Response.Redirect("~/COC_SCR_006.aspx");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        
        protected void rdNormal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNewIsDeleted.Text = "0";
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void rdRetire_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNewIsDeleted.Text = "1";
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void GetStaffByTeam(string cocTeam)
        {
            cmbTransferee.DataSource = StaffBiz.GetStaffByTeam(cocTeam);
            cmbTransferee.DataTextField = "TextField";
            cmbTransferee.DataValueField = "ValueField";
            cmbTransferee.DataBind();
            cmbTransferee.Items.Remove(cmbTransferee.Items.FindByValue(txtEmpCode.Text.Trim()));
            cmbTransferee.Items.Insert(0, new ListItem("", ""));

            btnTransferJob.Enabled = cmbTransferee.Items.Count > 0 ? true : false;
        }

        private void GetJobOnHandList()
        {
            try
            {
                List<LeadDataPopupMonitoring> list = DoGetJobOnHandList();
                BindJobOnHandGridview((COC.Application.Shared.GridviewPageController)pcJobOnHand, list.ToArray(), 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<LeadDataPopupMonitoring> DoGetJobOnHandList()
        {
            return LeadBiz.GetJobOnHandList(txtCurrentCocTeam.Text.Trim(), txtEmpCode.Text.Trim());
        }

        #region gvJobOnHand Paging

        private void BindJobOnHandGridview(COC.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvJobOnHand);
            pageControl.Update(items, pageIndex);
            pageControl.GenerateRecordNumber(2, pageIndex);
            upJobOnHand.Update();
        }

        protected void PageSearchChangeJobOnHand(object sender, EventArgs e)
        {
            try
            {
                List<LeadDataPopupMonitoring> list = DoGetJobOnHandList();
                var pageControl = (COC.Application.Shared.GridviewPageController)sender;
                BindJobOnHandGridview(pageControl, list.ToArray(), pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion  

        protected void btnRefreshJobOnHand_Click(object sender, EventArgs e)
        {
            try
            {
                GetJobOnHandList();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnTransferJob_Click(object sender, EventArgs e)
        {
            try
            {
                var status = StaffBiz.GetStaffStatus(txtUsername.Text.Trim());
                if (status == 1)
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถโอนงานได้ เนื่องจากพนักงานยังมีสถานะพร้อมรับงานอยู่");
                    return;
                }

                if (cmbTransferee.SelectedItem.Value == "")
                {
                    AppUtil.ClientAlert(Page, "กรุณาเลือกพนักงานที่จะโอนงานให้");
                    return;
                }

                List<string> ticketIdList = new List<string>();
                foreach (GridViewRow row in gvJobOnHand.Rows)
                {
                    if (((CheckBox)row.FindControl("cbTranserJob")).Checked)
                        ticketIdList.Add(((Label)row.FindControl("lblTicketId")).Text.Trim());
                }

                if (ticketIdList.Count > 0)
                {
                    LeadBiz.TransferJob(ticketIdList, cmbTransferee.SelectedItem.Value, txtCurrentCocTeam.Text.Trim(), HttpContext.Current.User.Identity.Name);

                    List<LeadDataPopupMonitoring> mainList = DoGetJobOnHandList();
                    BindJobOnHandGridview((COC.Application.Shared.GridviewPageController)pcJobOnHand, mainList.ToArray(), 0);
                    upJobOnHand.Update();

                    AppUtil.ClientAlert(Page, "โอนงานเรียบร้อย");
                }
                else
                {
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

        protected void cmbStaffType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CheckConditionOper();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void CheckConditionOper()
        {
            if (cmbStaffType.SelectedItem.Value == COCConstant.StaffType.Oper.ToString() || cmbStaffType.SelectedItem.Value == COCConstant.StaffType.SupervisorOper.ToString())
            {
                cmbCocTeam.Enabled = true;
                lblCocTeam.Visible = true;
            }
            else
            {
                cmbCocTeam.SelectedIndex = -1;
                cmbCocTeam.Enabled = false;
                lblCocTeam.Visible = false;
                vCocTeam.Text = "";
            }
        }
    }
}