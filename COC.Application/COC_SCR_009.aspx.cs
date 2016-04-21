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
    public partial class COC_SCR_009 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_009));
        private string coc_staffid = "staffid";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "เพิ่มข้อมูลพนักงาน";
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
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_009");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_002.aspx");
                        return;
                    }

                    InitialControl();
                    SetDept();
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void InitialControl()
        {
            //Role
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

        private void SetDept()
        {
            decimal? stafftype = StaffBiz.GetStaffType(HttpContext.Current.User.Identity.Name);
            if (stafftype != null)
            {
                if (stafftype == COCConstant.StaffType.ITAdministrator)
                {
                    cmbDepartment.Enabled = true;
                }
                else
                {
                    cmbDepartment.Enabled = false;
                    int? dept = StaffBiz.GetDepartmentId(HttpContext.Current.User.Identity.Name);
                    if (dept != null)
                    {
                        cmbDepartment.SelectedIndex = cmbDepartment.Items.IndexOf(cmbDepartment.Items.FindByValue(dept.ToString()));
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    StaffDataManagement data = new StaffDataManagement();
                    data.Username = txtUserName.Text.Trim();
                    data.EmpCode = txtEmpCode.Text.Trim();
                    data.MarketingCode = txtMarketingCode.Text.Trim();
                    data.StaffNameTH = txtStaffNameTH.Text.Trim();
                    data.TelNo = txtTellNo.Text.Trim();
                    data.StaffEmail = txtStaffEmail.Text.Trim();
                    data.PositionId = cmbPosition.SelectedItem.Value;
                    data.StaffTypeId = decimal.Parse(cmbStaffType.SelectedItem.Value);
                    data.Team = txtTeam.Text.Trim();
                    data.BranchCode = cmbBranchCode.SelectedItem.Value;
                    if (cmbHeadStaffId.Items.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(cmbHeadStaffId.SelectedItem.Value)) { data.HeadStaffId = int.Parse(cmbHeadStaffId.SelectedItem.Value); }
                    }
                    if (cmbDepartment.Items.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(cmbDepartment.SelectedItem.Value)) { data.DepartmentId = int.Parse(cmbDepartment.SelectedItem.Value); }
                    }
                    if (cmbCocTeam.Items.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(cmbCocTeam.SelectedItem.Value)) { data.CocTeam = cmbCocTeam.SelectedItem.Value; }
                    }

                    string staffId = StaffBiz.InsertStaff(data, HttpContext.Current.User.Identity.Name);
                    Session[coc_staffid] = staffId;
                    AppUtil.ClientAlertAndRedirect(Page, "บันทึกข้อมูลเจ้าหน้าที่สำเร็จ", "COC_SCR_007.aspx");
                }
                else
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุข้อมูลให้ครบถ้วน");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
        private bool ValidateData()
        {
            int i = 0;
            //************************************Windows Username********************************************
            if (txtUserName.Text.Trim() == "")
            {
                vtxtUserName.Text = "กรุณาระบุ Windows Username";
                vtxtUserName.ForeColor = System.Drawing.Color.Red;
                i += 1;
            }
            else
            {
                vtxtUserName.Text = "";
                if (StaffBiz.CheckUsernameExist(txtUserName.Text.Trim(), null))
                {
                    vtxtUserName.Text = "Windows Username นี้มีอยู่แล้วในระบบแล้ว";
                    vtxtUserName.ForeColor = System.Drawing.Color.Red;
                    i += 1;
                }
                else
                    vtxtUserName.Text = "";
            }

            string desc;
            if (LeadBiz.CheckHeadStaff(txtEmpCode.Text, cmbHeadStaffId.SelectedValue.Trim(), out desc))
            {
                vcmbHeadStaffId.Text = "ไม่สามารถเปลี่ยนหัวหน้างานได้เนื่องจาก " + txtStaffNameTH.Text + "เป็นหัวหน้างาน" + cmbHeadStaffId.Text.Trim();
                i += 1;
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
                if (StaffBiz.CheckEmpCodeExist(txtEmpCode.Text.Trim(), null))
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
                if (StaffBiz.CheckMarketingCodeExist(txtMarketingCode.Text.Trim(), null))
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
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        private bool ValidateEmail()
        {
            string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern);
            return reg.IsMatch(txtStaffEmail.Text.Trim());
        }

        protected void cmbBranchCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbHeadStaffId.DataSource = StaffBiz.GetStaffHeadList(cmbBranchCode.SelectedItem.Value);
                cmbHeadStaffId.DataTextField = "TextField";
                cmbHeadStaffId.DataValueField = "ValueField";
                cmbHeadStaffId.DataBind();
                cmbHeadStaffId.Items.Insert(0, new ListItem("", ""));
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnCheckUsername_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text.Trim() != string.Empty)
                {
                    vtxtUserName.Text = "";

                    if (StaffBiz.CheckUsernameExist(txtUserName.Text.Trim(), null))
                    {
                        vtxtUserName.Text = "Windows Username นี้มีอยู่แล้วในระบบแล้ว";
                        vtxtUserName.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        vtxtUserName.Text = "Windows Username ใช้งานได้";
                        vtxtUserName.ForeColor = System.Drawing.Color.Green;
                    }
                }
                else
                    vtxtUserName.Text = "";
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("COC_SCR_006.aspx");
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        protected void cmbStaffType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
    }
}