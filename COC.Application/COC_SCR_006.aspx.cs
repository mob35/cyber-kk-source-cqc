using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Text;
using COC.Application.Utilities;
using COC.Biz;
using COC.Resource.Data;
using log4net;
using COC.Resource;

namespace COC.Application
{
    public partial class COC_SCR_006 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_006));
        private string coc_staffid = "staffid";
        private string coc_staffsearchcondition = AppConstant.SessionName.COC_StaffSearchcondition;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "ค้นหาข้อมูลพนักงาน";
            Page.Form.DefaultButton = btnSearch.UniqueID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_006");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_002.aspx");
                        return;
                    }

                    InitialControl();
                    SetDept();
                    if (Session[coc_staffsearchcondition] != null)
                    {
                        SetSerachCondition((StaffDataManagement)Session[coc_staffsearchcondition]);  //Page Load กลับมาจากหน้าอื่น
                        Session[coc_staffsearchcondition] = null;
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

        private void InitialControl()
        {
            ////Role
            cmbStaffTypeSearch.DataSource = StaffBiz.GetStaffTyeList();
            cmbStaffTypeSearch.DataTextField = "TextField";
            cmbStaffTypeSearch.DataValueField = "ValueField";
            cmbStaffTypeSearch.DataBind();
            cmbStaffTypeSearch.Items.Insert(0, new ListItem("", ""));

            ////Market Branch
            cmbBranchSearch.DataSource = BranchBiz.GetBranchList();
            cmbBranchSearch.DataTextField = "TextField";
            cmbBranchSearch.DataValueField = "ValueField";
            cmbBranchSearch.DataBind();
            cmbBranchSearch.Items.Insert(0, new ListItem("", ""));

            ////Department
            cmbDepartmentSearch.DataSource = DepartmentBiz.GetDepartmentList();
            cmbDepartmentSearch.DataTextField = "TextField";
            cmbDepartmentSearch.DataValueField = "ValueField";
            cmbDepartmentSearch.DataBind();
            cmbDepartmentSearch.Items.Insert(0, new ListItem("", ""));

            ////COC Team
            cmbCOCTeamSearch.DataSource = StaffBiz.GetTeamList();
            cmbCOCTeamSearch.DataTextField = "TextField";
            cmbCOCTeamSearch.DataValueField = "ValueField";
            cmbCOCTeamSearch.DataBind();
            cmbCOCTeamSearch.Items.Insert(0, new ListItem("", ""));

            //Position
            cmbPositionSearch.DataSource = PositionBiz.GetPositionList(COCConstant.Position.All);
            cmbPositionSearch.DataTextField = "TextField";
            cmbPositionSearch.DataValueField = "ValueField";
            cmbPositionSearch.DataBind();
            cmbPositionSearch.Items.Insert(0, new ListItem("", ""));

            upSearch.Update();
        }

        private void SetDept()
        {
            decimal? stafftype = StaffBiz.GetStaffType(HttpContext.Current.User.Identity.Name);
            if (stafftype != null)
            {
                if (stafftype == COCConstant.StaffType.ITAdministrator)
                {
                    cmbDepartmentSearch.Enabled = true;
                    cmbDepartmentSearch.SelectedIndex = -1;
                }
                else
                {
                    cmbDepartmentSearch.Enabled = false;
                    int? dept = StaffBiz.GetDepartmentId(HttpContext.Current.User.Identity.Name);
                    if (dept != null)
                    {
                        cmbDepartmentSearch.SelectedIndex = cmbDepartmentSearch.Items.IndexOf(cmbDepartmentSearch.Items.FindByValue(dept.ToString()));
                    }
                }
            }
        }
        private void SetSerachCondition(StaffDataManagement conn)
        {
            bool dosearch = false;
            try
            {
                if (!string.IsNullOrEmpty(conn.Username))
                {
                    txtUsernameSearch.Text = conn.Username;
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.BranchCode))
                {
                    cmbBranchSearch.SelectedIndex = cmbBranchSearch.Items.IndexOf(cmbBranchSearch.Items.FindByValue(conn.BranchCode));
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.EmpCode))
                {
                    txtEmpCodeSearch.Text = conn.EmpCode;
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.MarketingCode))
                {
                    txtMarketingCodeSearch.Text = conn.MarketingCode;
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.StaffNameTH))
                {
                    txtStaffNameTHSearch.Text = conn.StaffNameTH;
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.PositionId))
                {
                    cmbPositionSearch.SelectedIndex = cmbPositionSearch.Items.IndexOf(cmbPositionSearch.Items.FindByValue(conn.PositionId));
                    dosearch = true;
                }
                if (conn.StaffTypeId != null)
                {
                    cmbStaffTypeSearch.SelectedIndex = cmbStaffTypeSearch.Items.IndexOf(cmbStaffTypeSearch.Items.FindByValue(conn.StaffTypeId.Value.ToString()));
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.Team))
                {
                    txtTeamSearch.Text = conn.Team;
                    dosearch = true;
                }
                if (conn.DepartmentId != null)
                {
                    cmbDepartmentSearch.SelectedIndex = cmbDepartmentSearch.Items.IndexOf(cmbDepartmentSearch.Items.FindByValue(conn.DepartmentId.Value.ToString()));
                    dosearch = true;
                }
                if (conn.CocTeam != null)
                {
                    cmbCOCTeamSearch.SelectedIndex = cmbCOCTeamSearch.Items.IndexOf(cmbCOCTeamSearch.Items.FindByValue(conn.CocTeam));
                    dosearch = true;
                }

                if (dosearch)
                    DoSearchData(conn.PageIndex != null ? conn.PageIndex.Value : 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DoSearchData(int pageIndex)
        {
            try
            {
                List<StaffDataManagement> list = StaffBiz.SearchStaffList(txtUsernameSearch.Text.Trim(), cmbBranchSearch.SelectedItem.Value, txtEmpCodeSearch.Text.Trim(), txtMarketingCodeSearch.Text.Trim()
                    , txtStaffNameTHSearch.Text.Trim(), cmbPositionSearch.SelectedItem.Value, cmbStaffTypeSearch.SelectedItem.Value, txtTeamSearch.Text.Trim(), cmbDepartmentSearch.SelectedItem.Value, cmbCOCTeamSearch.SelectedItem.Value);

                BindGridview((COC.Application.Shared.GridviewPageController)pcTop, list.ToArray(), pageIndex);
                pcTop.Visible = true;
                upResult.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Page Control

        private void BindGridview(COC.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvResult);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                var pageControl = (COC.Application.Shared.GridviewPageController)sender;
                DoSearchData(pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                    DoSearchData(0);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtUsernameSearch.Text = string.Empty;
                cmbBranchSearch.SelectedIndex = -1;
                txtEmpCodeSearch.Text = string.Empty;
                txtMarketingCodeSearch.Text = string.Empty;
                txtStaffNameTHSearch.Text = string.Empty;
                cmbPositionSearch.SelectedIndex = -1;
                cmbStaffTypeSearch.SelectedIndex = -1;
                cmbCOCTeamSearch.SelectedIndex = -1;
                txtTeamSearch.Text = string.Empty;
                SetDept();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        private bool ValidateData()
        {
            if (txtUsernameSearch.Text.Trim() == string.Empty && cmbBranchSearch.SelectedItem.Value == string.Empty && txtEmpCodeSearch.Text.Trim() == string.Empty
                && txtMarketingCodeSearch.Text.Trim() == string.Empty && txtStaffNameTHSearch.Text.Trim() == string.Empty && cmbPositionSearch.SelectedItem.Value == string.Empty
                && cmbStaffTypeSearch.SelectedItem.Value == string.Empty && txtTeamSearch.Text.Trim() == string.Empty && cmbDepartmentSearch.SelectedItem.Value == string.Empty
                && cmbCOCTeamSearch.SelectedItem.Value == string.Empty)
            {
                AppUtil.ClientAlert(Page, "กรุณาระบุเงื่อนไขอย่างน้อย 1 อย่าง");
                return false;
            }
            else
                return true;
        }

        private StaffDataManagement GetSearchCondition()
        {
            StaffDataManagement data = new StaffDataManagement();
            data.Username = txtUsernameSearch.Text.Trim();
            data.BranchCode = cmbBranchSearch.Items.Count > 0 ? cmbBranchSearch.SelectedItem.Value : string.Empty;
            data.EmpCode = txtEmpCodeSearch.Text.Trim();
            data.MarketingCode = txtMarketingCodeSearch.Text.Trim();
            data.StaffNameTH = txtStaffNameTHSearch.Text.Trim();
            data.PositionId = cmbPositionSearch.SelectedItem.Value;
            data.Team = txtTeamSearch.Text.Trim();
            if (cmbStaffTypeSearch.Items.Count > 0 && cmbStaffTypeSearch.SelectedItem.Value != string.Empty)
                data.StaffTypeId = decimal.Parse(cmbStaffTypeSearch.SelectedItem.Value);
            if (cmbDepartmentSearch.Items.Count > 0 && cmbDepartmentSearch.SelectedItem.Value != string.Empty)
                data.DepartmentId = int.Parse(cmbDepartmentSearch.SelectedItem.Value);
            if (cmbCOCTeamSearch.Items.Count > 0 && cmbCOCTeamSearch.SelectedItem.Value != string.Empty)
                data.CocTeam = cmbCOCTeamSearch.SelectedItem.Value;

            data.PageIndex = pcTop.SelectedPageIndex;

            return data;
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                Session[coc_staffsearchcondition] = GetSearchCondition();
                Response.Redirect("COC_SCR_009.aspx");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void imbAction_Click(object sender, EventArgs e)
        {
            try
            {
                Session[coc_staffid] = ((ImageButton)sender).CommandArgument;
                Session[coc_staffsearchcondition] = GetSearchCondition();
                Response.Redirect("COC_SCR_007.aspx");
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            //Response.Redirect("SLM_SCR_011.aspx?ticketid=" + ((ImageButton)sender).CommandArgument + "&ReturnUrl=" + Server.UrlEncode(Request.Url.AbsoluteUri));
        }
    }
}