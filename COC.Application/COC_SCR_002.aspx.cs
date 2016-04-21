using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using COC.Application.Utilities;
using COC.Biz;
using COC.Resource.Data;
using COC.Resource;
using log4net;

namespace COC.Application
{
    public partial class COC_SCR_002 : System.Web.UI.Page
    {
        private string coc_searchcondition = AppConstant.SessionName.COC_Searchcondition;
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_002));

        protected override void OnInit(EventArgs e)
        {
            //pnAdvanceSearch.Style["display"] = "block";
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "ค้นหาข้อมูล Lead (Search)";
            Page.Form.DefaultButton = btnSearch.UniqueID;
            AppUtil.SetIntTextBox(txtTicketID);
            AppUtil.SetIntTextBox(txtCitizenId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    InitialControl();

                    //เก็บไว้ใช้ตอนกด link Aol Summary Report
                    StaffData staff =  StaffBiz.GetStaff(HttpContext.Current.User.Identity.Name);
                    txtLoginEmpCode.Text = staff.EmpCode;
                    txtLoginStaffTypeId.Text = staff.StaffTypeId != null ? staff.StaffTypeId.ToString() : "";
                    txtLoginStaffTypeDesc.Text = staff.StaffTypeDesc;

                    if (Session[coc_searchcondition] != null)
                    {
                        SetSerachCondition((SearchLeadCondition)Session[coc_searchcondition]);  //Page Load กลับมาจากหน้าอื่น
                        Session[coc_searchcondition] = null;
                    }
                    //else
                    //    DoSearchLeadData(0, string.Empty, SortDirection.Ascending);    //First Load ครั้งแรก
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
            txtRecursiveList.Text = AppUtil.GetRecursiveStaff(HttpContext.Current.User.Identity.Name);
            txtTeamRecursiveList.Text = AppUtil.GetRecursiveTeam(HttpContext.Current.User.Identity.Name);

            cmbCampaign.DataSource = CampaignBiz.GetCampaignList(false);
            cmbCampaign.DataTextField = "TextField";
            cmbCampaign.DataValueField = "ValueField";
            cmbCampaign.DataBind();
            cmbCampaign.Items.Insert(0, new ListItem("", ""));

            cmbChannel.DataSource = ChannelBiz.GetChannelList();
            cmbChannel.DataTextField = "TextField";
            cmbChannel.DataValueField = "ValueField";
            cmbChannel.DataBind();
            cmbChannel.Items.Insert(0, new ListItem("", ""));

            //Owner Branch
            cmbOwnerBranchSearch.DataSource = BranchBiz.GetBranchList();
            cmbOwnerBranchSearch.DataTextField = "TextField";
            cmbOwnerBranchSearch.DataValueField = "ValueField";
            cmbOwnerBranchSearch.DataBind();
            cmbOwnerBranchSearch.Items.Insert(0, new ListItem("", ""));
            BindOwnerLead();

            //Delegate Branch
            cmbDelegateBranchSearch.DataSource = BranchBiz.GetBranchList();
            cmbDelegateBranchSearch.DataTextField = "TextField";
            cmbDelegateBranchSearch.DataValueField = "ValueField";
            cmbDelegateBranchSearch.DataBind();
            cmbDelegateBranchSearch.Items.Insert(0, new ListItem("", ""));
            BindDelegateLead();

            //CreateBy Branch
            cmbCreatebyBranchSearch.DataSource = BranchBiz.GetBranchList();
            cmbCreatebyBranchSearch.DataTextField = "TextField";
            cmbCreatebyBranchSearch.DataValueField = "ValueField";
            cmbCreatebyBranchSearch.DataBind();
            cmbCreatebyBranchSearch.Items.Insert(0, new ListItem("", ""));
            BindCreateByLead();

            cmbMarketingOwner.DataSource = StaffBiz.GetStaffListByStaffTypeId(COCConstant.StaffType.Marketing, "");
            cmbMarketingOwner.DataTextField = "TextField";
            cmbMarketingOwner.DataValueField = "ValueField";    //empcode
            cmbMarketingOwner.DataBind();
            cmbMarketingOwner.Items.Insert(0, new ListItem("", ""));

            cmbLastOwner.DataSource = StaffBiz.GetStaffListByStaffTypeId(COCConstant.StaffType.Oper, txtRecursiveList.Text.Trim());
            cmbLastOwner.DataTextField = "TextField";
            cmbLastOwner.DataValueField = "ValueField";         //empcode
            cmbLastOwner.DataBind();
            cmbLastOwner.Items.Insert(0, new ListItem("", ""));

            cmbCocTeam.DataSource = StaffBiz.GetTeamList();
            cmbCocTeam.DataTextField = "TextField";
            cmbCocTeam.DataValueField = "ValueField";
            cmbCocTeam.DataBind();
            cmbCocTeam.Items.Insert(0, new ListItem("", ""));

            cmbCocStatus.DataSource = OptionBiz.GetCocStatusList(AppConstant.OptionType.CocStatus);
            cmbCocStatus.DataTextField = "TextField";
            cmbCocStatus.DataValueField = "ValueField";
            cmbCocStatus.DataBind();
            cmbCocStatus.Items.Insert(0, new ListItem("", ""));

            cbOptionList.DataSource = OptionBiz.GetOptionList(AppConstant.OptionType.LeadStatus);
            cbOptionList.DataTextField = "TextField";
            cbOptionList.DataValueField = "ValueField";
            cbOptionList.DataBind();

            //ListItem lst = cbOptionList.Items.FindByValue("00");
            //if (lst != null) lst.Selected = true;
            //lst = cbOptionList.Items.FindByValue("01");
            //if (lst != null) lst.Selected = true;

            pcTop.SetVisible = false;
        }

        protected void cmbCocStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCocSubStatus();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void BindCocSubStatus()
        {
            List<ControlListData> list = OptionBiz.GetCocSubStatusList(AppConstant.OptionType.CocStatus, cmbCocStatus.SelectedItem.Value);
            cmbCocSubStatus.DataSource = list;
            cmbCocSubStatus.DataTextField = "TextField";
            cmbCocSubStatus.DataValueField = "ValueField";
            cmbCocSubStatus.DataBind();
            cmbCocSubStatus.Items.Insert(0, new ListItem("", ""));
            cmbCocSubStatus.Visible = list.Count > 0;
            lblCocSubStatus.Visible = list.Count > 0;
        }

        private void SetSerachCondition(SearchLeadCondition conn)
        {
            bool dosearch = false;
            try
            {
                if (!string.IsNullOrEmpty(conn.TicketId))
                {
                    txtTicketID.Text = conn.TicketId;
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.Firstname))
                {
                    txtFirstname.Text = conn.Firstname;
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.Lastname))
                {
                    txtLastname.Text = conn.Lastname;
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.CitizenId))
                {
                    txtCitizenId.Text = conn.CitizenId;
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.CampaignId))
                {
                    cmbCampaign.SelectedIndex = cmbCampaign.Items.IndexOf(cmbCampaign.Items.FindByValue(conn.CampaignId));
                    //GetOwnerLead(cmbCampaign.SelectedItem.Value);
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.ChannelId))
                {
                    cmbChannel.SelectedIndex = cmbChannel.Items.IndexOf(cmbChannel.Items.FindByValue(conn.ChannelId));
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.OwnerBranch))
                {
                    cmbOwnerBranchSearch.SelectedIndex = cmbOwnerBranchSearch.Items.IndexOf(cmbOwnerBranchSearch.Items.FindByValue(conn.OwnerBranch));
                    BindOwnerLead();
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.OwnerUsername))
                {
                    cmbOwnerLeadSearch.SelectedIndex = cmbOwnerLeadSearch.Items.IndexOf(cmbOwnerLeadSearch.Items.FindByValue(conn.OwnerUsername));
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.DelegateBranch))
                {
                    cmbDelegateBranchSearch.SelectedIndex = cmbDelegateBranchSearch.Items.IndexOf(cmbDelegateBranchSearch.Items.FindByValue(conn.DelegateBranch));
                    BindDelegateLead();
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.DelegateLead))
                {
                    cmbDelegateLeadSearch.SelectedIndex = cmbDelegateLeadSearch.Items.IndexOf(cmbDelegateLeadSearch.Items.FindByValue(conn.DelegateLead));
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.CreateByBranch))
                {
                    cmbCreatebyBranchSearch.SelectedIndex = cmbCreatebyBranchSearch.Items.IndexOf(cmbCreatebyBranchSearch.Items.FindByValue(conn.CreateByBranch));
                    BindCreateByLead();
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.CreateBy))
                {
                    cmbCreatebySearch.SelectedIndex = cmbCreatebySearch.Items.IndexOf(cmbCreatebySearch.Items.FindByValue(conn.CreateBy));
                    dosearch = true;
                }
                if (conn.CreatedDate.Year != 1)
                {
                    tdmCreateDate.DateValue = conn.CreatedDate;
                    dosearch = true;
                }
                if (conn.AssignedDate.Year != 1)
                {
                    tdmAssignDate.DateValue = conn.AssignedDate;
                    dosearch = true;
                }

                //COC
                if (conn.CocAssignedDate.Year != 1)
                {
                    tdmCocAssignDate.DateValue = conn.CocAssignedDate;
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.MarketingOwner))
                {
                    cmbMarketingOwner.SelectedIndex = cmbMarketingOwner.Items.IndexOf(cmbMarketingOwner.Items.FindByValue(conn.MarketingOwner));
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.LastOwner))
                {
                    cmbLastOwner.SelectedIndex = cmbLastOwner.Items.IndexOf(cmbLastOwner.Items.FindByValue(conn.LastOwner));
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.CocTeam))
                {
                    cmbCocTeam.SelectedIndex = cmbCocTeam.Items.IndexOf(cmbCocTeam.Items.FindByValue(conn.CocTeam));
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.CocStatus))
                {
                    cmbCocStatus.SelectedIndex = cmbCocStatus.Items.IndexOf(cmbCocStatus.Items.FindByValue(conn.CocStatus));
                    BindCocSubStatus();
                    dosearch = true;
                }
                if (!string.IsNullOrEmpty(conn.CocSubStatus))
                {
                    cmbCocSubStatus.SelectedIndex = cmbCocSubStatus.Items.IndexOf(cmbCocSubStatus.Items.FindByValue(conn.CocSubStatus));
                    dosearch = true;
                }

                foreach (ListItem lst in cbOptionList.Items)
                {
                    lst.Selected = false;
                }

                if (!string.IsNullOrEmpty(conn.StatusList))
                {
                    string[] vals = conn.StatusList.Split(',');
                    foreach (string val in vals)
                    {
                        ListItem lst = cbOptionList.Items.FindByValue(val.Replace("'", ""));
                        if (lst != null) lst.Selected = true;
                        dosearch = true;
                    }
                }

                CheckAllCondition();

                SortExpressionProperty = conn.SortExpression;
                SortDirectionProperty = conn.SortDirection == SortDirection.Ascending.ToString() ? SortDirection.Ascending : SortDirection.Descending;

                if (dosearch)
                    DoSearchLeadData(conn.PageIndex, SortExpressionProperty, SortDirectionProperty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindOwnerLead()
        {
            cmbOwnerLeadSearch.DataSource = StaffBiz.GetStaffList(cmbOwnerBranchSearch.SelectedItem.Value);
            cmbOwnerLeadSearch.DataTextField = "TextField";
            cmbOwnerLeadSearch.DataValueField = "ValueField";
            cmbOwnerLeadSearch.DataBind();
            cmbOwnerLeadSearch.Items.Insert(0, new ListItem("", ""));
        }

        private void BindDelegateLead()
        {
            cmbDelegateLeadSearch.DataSource = StaffBiz.GetStaffList(cmbDelegateBranchSearch.SelectedItem.Value);
            cmbDelegateLeadSearch.DataTextField = "TextField";
            cmbDelegateLeadSearch.DataValueField = "ValueField";
            cmbDelegateLeadSearch.DataBind();
            cmbDelegateLeadSearch.Items.Insert(0, new ListItem("", ""));
        }

        private void BindCreateByLead()
        {
            cmbCreatebySearch.DataSource = StaffBiz.GetStaffList(cmbCreatebyBranchSearch.SelectedItem.Value);
            cmbCreatebySearch.DataTextField = "TextField";
            cmbCreatebySearch.DataValueField = "ValueField";
            cmbCreatebySearch.DataBind();
            cmbCreatebySearch.Items.Insert(0, new ListItem("", ""));
        }

        protected void cmbOwnerBranchSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindOwnerLead();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void cmbDelegateBranchSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindDelegateLead();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void cmbCreatebyBranchSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCreateByLead();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void cbOptionAll_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOptionAll.Checked)
            {
                foreach (ListItem li in cbOptionList.Items)
                {
                    li.Selected = true;
                }
            }
            else
            {
                foreach (ListItem li in cbOptionList.Items)
                {
                    li.Selected = false;
                }
            }
        }

        protected void cbOptionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAllCondition();
        }

        private void CheckAllCondition()
        {
            int count = 0;
            foreach (ListItem li in cbOptionList.Items)
            {
                if (!li.Selected) { count += 1; }
            }

            cbOptionAll.Checked = count > 0 ? false : true;
        }

        private bool ValidateData()
        {
            bool selected = false;
            foreach (ListItem li in cbOptionList.Items)
            {
                if (li.Selected)
                {
                    selected = true;
                    break;
                }
            }

            if (txtTicketID.Text.Trim() == string.Empty && txtFirstname.Text.Trim() == string.Empty && txtLastname.Text.Trim() == string.Empty
                && txtCitizenId.Text.Trim() == string.Empty && cmbCampaign.SelectedIndex == 0 && cmbChannel.SelectedIndex == 0
                && cmbOwnerBranchSearch.SelectedIndex == 0 && cmbOwnerLeadSearch.SelectedIndex == 0
                && cmbDelegateBranchSearch.SelectedIndex == 0 && cmbDelegateLeadSearch.SelectedIndex == 0
                && cmbCreatebyBranchSearch.SelectedIndex == 0 && cmbCreatebySearch.SelectedIndex == 0
                && tdmCreateDate.DateValue.Year == 1 && tdmAssignDate.DateValue.Year == 1 && selected == false
                && tdmCocAssignDate.DateValue.Year == 1 && cmbMarketingOwner.SelectedIndex == 0 && cmbLastOwner.SelectedIndex == 0
                && cmbCocTeam.SelectedIndex == 0 && cmbCocStatus.SelectedIndex == 0)
            {
                return false;
            }
            else
                return true;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    SortExpressionProperty = string.Empty;
                    SortDirectionProperty = SortDirection.Ascending;
                    DoSearchLeadData(0, SortExpressionProperty, SortDirectionProperty);
                }
                else
                    AppUtil.ClientAlert(Page, "กรุณาเลือกเงื่อนไขอย่างน้อย 1 อย่าง");
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
                txtTicketID.Text = "";
                txtFirstname.Text = "";
                txtLastname.Text = "";
                txtCitizenId.Text = "";
                cmbCampaign.SelectedIndex = -1;
                cmbChannel.SelectedIndex = -1;
                ((COC.Application.Shared.TextDateMask)tdmCreateDate).DateValue = new DateTime();
                ((COC.Application.Shared.TextDateMask)tdmAssignDate).DateValue = new DateTime();
                ((COC.Application.Shared.TextDateMask)tdmCocAssignDate).DateValue = new DateTime();
                foreach (ListItem li in cbOptionList.Items)
                {
                    li.Selected = false;
                }
                cbOptionAll.Checked = false;
                cmbOwnerBranchSearch.SelectedIndex = -1;
                BindOwnerLead();
                cmbDelegateBranchSearch.SelectedIndex = -1;
                BindDelegateLead();
                cmbCreatebyBranchSearch.SelectedIndex = -1;
                BindCreateByLead();
                cmbMarketingOwner.SelectedIndex = -1;
                cmbLastOwner.SelectedIndex = -1;
                cmbCocTeam.SelectedIndex = -1;
                cmbCocStatus.SelectedIndex = -1;
                BindCocSubStatus();

                upSearch.Update();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void DoSearchLeadData(int pageIndex, string sortExpression, SortDirection sortDirection)
        {
            try
            {
                List<SearchLeadData> result = SearchLeadBiz.SearchLeadData(GetSearchCondition(), HttpContext.Current.User.Identity.Name, txtRecursiveList.Text.Trim(), txtTeamRecursiveList.Text.Trim(), txtLoginStaffTypeId.Text.Trim());

                if (sortExpression == "CampaignName")
                {
                    if (sortDirection == SortDirection.Ascending)
                        result = result.OrderBy(p => p.CampaignName).ToList();
                    else
                        result = result.OrderByDescending(p => p.CampaignName).ToList();
                }
                else if (sortExpression == "StatusDesc")
                {
                    if (sortDirection == SortDirection.Ascending)
                        result = result.OrderBy(p => p.StatusDesc).ToList();
                    else
                        result = result.OrderByDescending(p => p.StatusDesc).ToList();
                }

                BindGridview((COC.Application.Shared.GridviewPageController)pcTop, result.ToArray(), pageIndex);
                upResult.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SearchLeadCondition GetSearchCondition()
        {
            SearchLeadCondition data = new SearchLeadCondition();
            data.TicketId = txtTicketID.Text.Trim();
            data.Firstname = txtFirstname.Text.Trim();
            data.Lastname = txtLastname.Text.Trim();
            data.CitizenId = txtCitizenId.Text.Trim();
            data.CampaignId = cmbCampaign.SelectedItem.Value;
            data.ChannelId = cmbChannel.SelectedItem.Value;
            data.OwnerUsername = cmbOwnerLeadSearch.Items.Count > 0 ? cmbOwnerLeadSearch.SelectedItem.Value : string.Empty;     //Owner Lead
            data.OwnerBranch = cmbOwnerBranchSearch.Items.Count > 0 ? cmbOwnerBranchSearch.SelectedItem.Value : string.Empty;   //Owner Branch
            data.DelegateBranch = cmbDelegateBranchSearch.Items.Count > 0 ? cmbDelegateBranchSearch.SelectedItem.Value : string.Empty;  //Delegate Branch
            data.DelegateLead = cmbDelegateLeadSearch.Items.Count > 0 ? cmbDelegateLeadSearch.SelectedItem.Value : string.Empty;    //Delegate Lead
            data.CreateByBranch = cmbCreatebyBranchSearch.Items.Count > 0 ? cmbCreatebyBranchSearch.SelectedItem.Value : string.Empty;  //CreateBy Branch
            data.CreateBy = cmbCreatebySearch.Items.Count > 0 ? cmbCreatebySearch.SelectedItem.Value : string.Empty;    //CreateBy
            data.CreatedDate = tdmCreateDate.DateValue;
            data.AssignedDate = tdmAssignDate.DateValue;
            data.StatusList = GetStatusList();
            data.PageIndex = pcTop.SelectedPageIndex > -1 ? pcTop.SelectedPageIndex : 0;
            data.StaffType = StaffBiz.GetStaffType(HttpContext.Current.User.Identity.Name);

            //COC
            data.CocAssignedDate = tdmCocAssignDate.DateValue;
            data.MarketingOwner = cmbMarketingOwner.Items.Count > 0 ? cmbMarketingOwner.SelectedItem.Value : string.Empty;
            data.LastOwner = cmbLastOwner.Items.Count > 0 ? cmbLastOwner.SelectedItem.Value : string.Empty;
            data.CocTeam = cmbCocTeam.Items.Count > 0 ? cmbCocTeam.SelectedItem.Value : string.Empty;
            data.CocStatus = cmbCocStatus.Items.Count > 0 ? cmbCocStatus.SelectedItem.Value : string.Empty;
            data.CocSubStatus = cmbCocSubStatus.Items.Count > 0 ? cmbCocSubStatus.SelectedItem.Value : string.Empty;

            data.SortExpression = SortExpressionProperty;
            data.SortDirection = SortDirectionProperty.ToString();
            return data;
        }

        private string GetStatusList()
        {
            string list = string.Empty;
            foreach (ListItem li in cbOptionList.Items)
            {
                if (li.Selected)
                    list += (list == string.Empty ? "" : ",") + "'" + li.Value + "'";
            }
            return list;
        }

        //protected void lbBO_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("test1.aspx");
        //    //Response.Write("<script>");
        //    //Response.Write("window.open('test1.html','_blank')");
        //    //Response.Write("</script>");
        //}

        protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((Label)e.Row.FindControl("lblCalculatorUrl")).Text.Trim() != "")
                {
                    ((ImageButton)e.Row.FindControl("imbCal")).Visible = true;
                    ((ImageButton)e.Row.FindControl("imbCal")).OnClientClick = GetCallCalculatorScript(((Label)e.Row.FindControl("lblTicketId")).Text.Trim()
                                                                                                            , ((Label)e.Row.FindControl("lblCalculatorUrl")).Text.Trim());
                }
                else
                    ((ImageButton)e.Row.FindControl("imbCal")).Visible = false;


                //แนบเอกสาร เปลี่ยนเป็นใช้ imageButton
                if (((Label)e.Row.FindControl("lblHasAdamUrl")).Text.Trim().ToUpper() == "Y")
                {
                    ((ImageButton)e.Row.FindControl("imbDoc")).Visible = true;
                }
                else
                    ((ImageButton)e.Row.FindControl("imbDoc")).Visible = false;


                //ปุ่ม Others
                if (((Label)e.Row.FindControl("lblAppNo")).Text.Trim() != "")
                {
                    string privilegeNCB = StaffBiz.GetPrivilegeNCB(((Label)e.Row.FindControl("lblProductId")).Text.Trim(), Convert.ToDecimal(txtLoginStaffTypeId.Text.Trim()));
                    ((ImageButton)e.Row.FindControl("imbOthers")).Visible = privilegeNCB != "" ? true : false;
                }
                else
                    ((ImageButton)e.Row.FindControl("imbOthers")).Visible = false;
            }
        }

        protected void imbView_Click(object sender, EventArgs e)
        {
            Session[coc_searchcondition] = GetSearchCondition();
            Response.Redirect("COC_SCR_003.aspx?ticketid=" + ((ImageButton)sender).CommandArgument);
        }

        protected void imbDocument_Click(object sender, EventArgs e)
        {
            try
            {
                LeadDataForAdam leadData = SearchLeadBiz.GetLeadDataForAdam(((ImageButton)sender).CommandArgument);
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "calladam", GetCallAdamScript(leadData, HttpContext.Current.User.Identity.Name, txtLoginEmpCode.Text.Trim()), true);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
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
                DoSearchLeadData(pageControl.SelectedPageIndex, SortExpressionProperty, SortDirectionProperty);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion

        #region Sorting

        protected void gvResult_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                SortExpressionProperty = e.SortExpression;

                if (SortDirectionProperty == SortDirection.Ascending)
                    SortDirectionProperty = SortDirection.Descending;
                else
                    SortDirectionProperty = SortDirection.Ascending;

                DoSearchLeadData(0, SortExpressionProperty, SortDirectionProperty);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        public SortDirection SortDirectionProperty
        {
            get
            {
                if (ViewState["SortingState"] == null)
                {
                    ViewState["SortingState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["SortingState"];
            }
            set
            {
                ViewState["SortingState"] = value;
            }
        }

        public string SortExpressionProperty
        {
            get
            {
                if (ViewState["ExpressionState"] == null)
                {
                    ViewState["ExpressionState"] = string.Empty;
                }
                return ViewState["ExpressionState"].ToString();
            }
            set
            {
                ViewState["ExpressionState"] = value;
            }
        }

        protected void lbAolSummaryReport_Click(object sender, EventArgs e)
        {
            try
            {
                int index = int.Parse(((ImageButton)sender).CommandArgument);
                string appNo = ((Label)gvResult.Rows[index].FindControl("lblAppNo")).Text.Trim();   //"1002363";
                string productId = ((Label)gvResult.Rows[index].FindControl("lblProductId")).Text.Trim();
                string privilegeNCB = "";

                if (txtLoginStaffTypeId.Text.Trim() != "")
                    privilegeNCB = StaffBiz.GetPrivilegeNCB(productId, Convert.ToDecimal(txtLoginStaffTypeId.Text.Trim()));

                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "callaolsummaryreport", GetCallAolSummaryReportScript(appNo, txtLoginEmpCode.Text.Trim(), txtLoginStaffTypeDesc.Text.Trim(), privilegeNCB), true);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private string GetCallAolSummaryReportScript(string appNo, string empCode, string empTitle, string privilegeNCB)
        {
            string script = @"var form = document.createElement('form');
                                    var app_no = document.createElement('input');
                                    var emp_code = document.createElement('input');
                                    var emp_title = document.createElement('input');
                                    var privilege_ncb = document.createElement('input');

                                    form.action = '" + System.Configuration.ConfigurationManager.AppSettings["AolSummaryReportlUrl"] + @"';
                                    form.method = 'post';
                                    form.setAttribute('target', '_blank');

                                    app_no.name = 'app_no';
                                    app_no.value = '" + appNo + @"';
                                    form.appendChild(app_no);

                                    emp_code.name = 'emp_code';
                                    emp_code.value = '" + empCode + @"';
                                    form.appendChild(emp_code);

                                    emp_title.name = 'emp_title';
                                    emp_title.value = '" + empTitle + @"';
                                    form.appendChild(emp_title);

                                    privilege_ncb.name = 'privilege_ncb';
                                    privilege_ncb.value = '" + privilegeNCB + @"';
                                    form.appendChild(privilege_ncb);

                                    document.body.appendChild(form);
                                    form.submit();

                                    document.body.removeChild(form);";

            return script;
        }

        #endregion

        #region Script
        private string GetCallCalculatorScript(string ticketId, string url)
        {
            string script = @"var form = document.createElement('form');
                                    var input_ticketid = document.createElement('input');
                                    var input_username = document.createElement('input');

                                    form.action = '" + url + @"';
                                    form.method = 'post';
                                    form.setAttribute('target', '_blank');

                                    input_ticketid.name = 'ticketid';
                                    input_ticketid.value = '" + ticketId + @"';
                                    form.appendChild(input_ticketid);

                                    input_username.name = 'username';
                                    input_username.value = '" + HttpContext.Current.User.Identity.Name + @"';
                                    form.appendChild(input_username);

                                    document.body.appendChild(form);
                                    form.submit();

                                    document.body.removeChild(form);";

            return script;
        }

        private string GetCallAdamScript(LeadDataForAdam leadData, string username, string userId)
        {
            string script = @"var form = document.createElement('form');
                                var ticketid = document.createElement('input');
                                var username = document.createElement('input');
                                var userid = document.createElement('input');
                                var productgroupid = document.createElement('input');
                                var productid = document.createElement('input');
                                var campaign = document.createElement('input');
                                var firstname = document.createElement('input');
                                var lastname = document.createElement('input');
                                var telno1 = document.createElement('input');
                                var telno2 = document.createElement('input');
                                var telno3 = document.createElement('input');
                                var extno1 = document.createElement('input');
                                var extno2 = document.createElement('input');
                                var extno3 = document.createElement('input');
                                var email = document.createElement('input');
                                var buildingname = document.createElement('input');
                                var addrno = document.createElement('input');
                                var floor = document.createElement('input');
                                var soi = document.createElement('input');
                                var street = document.createElement('input');
                                var tambol = document.createElement('input');
                                var amphur = document.createElement('input');
                                var province = document.createElement('input');
                                var postalcode = document.createElement('input');
                                var occupation = document.createElement('input');
                                var basesalary = document.createElement('input');
                                var iscustomer = document.createElement('input');
                                var customercode = document.createElement('input');
                                var dateofbirth = document.createElement('input');
                                var cid = document.createElement('input');
                                var leadstatus = document.createElement('input');
                                var topic = document.createElement('input');
                                var detail = document.createElement('input');
                                var pathlink = document.createElement('input');
                                var telesalename = document.createElement('input');
                                var availabletime = document.createElement('input');
                                var contactbranch = document.createElement('input');
                                var interestedprodandtype = document.createElement('input');
                                var licenseno = document.createElement('input');
                                var yearofcar = document.createElement('input');
                                var yearofcarregis = document.createElement('input');
                                var registerprovince = document.createElement('input');
                                var brand = document.createElement('input');
                                var model = document.createElement('input');
                                var submodel = document.createElement('input');
                                var downpayment = document.createElement('input');
                                var downpercent = document.createElement('input');
                                var carprice = document.createElement('input');
                                var financeamt = document.createElement('input');
                                var term = document.createElement('input');
                                var paymenttype = document.createElement('input');
                                var balloonamt = document.createElement('input');
                                var balloonpercent = document.createElement('input');
                                var plantype = document.createElement('input');
                                var coveragedate = document.createElement('input');
                                var acctype = document.createElement('input');
                                var accpromotion = document.createElement('input');
                                var accterm = document.createElement('input');
                                var interest = document.createElement('input');
                                var invest = document.createElement('input');
                                var loanod = document.createElement('input');
                                var loanodterm = document.createElement('input');
                                var slmbank = document.createElement('input');
                                var slmatm = document.createElement('input');
                                var otherdetail1 = document.createElement('input');
                                var otherdetail2 = document.createElement('input');
                                var otherdetail3 = document.createElement('input');
                                var otherdetail4 = document.createElement('input');
                                var cartype = document.createElement('input');
                                var channelid = document.createElement('input');
                                var date = document.createElement('input');
                                var time = document.createElement('input');
                                var createuser = document.createElement('input');
                                var ipaddress = document.createElement('input');
                                var company = document.createElement('input');
                                var branch = document.createElement('input');
                                var branchno = document.createElement('input');
                                var machineno = document.createElement('input');
                                var clientservicetype = document.createElement('input');
                                var documentno = document.createElement('input');
                                var commpaidcode = document.createElement('input');
                                var zone = document.createElement('input');
                                var transid = document.createElement('input');

                                form.action = '" + System.Configuration.ConfigurationManager.AppSettings["AdamlUrl"] + @"';
                                form.method = 'post';
                                form.setAttribute('target', '" + leadData.TicketId + @"');

                                ticketid.name = 'ticketid';
                                ticketid.value = '" + leadData.TicketId + @"';
                                form.appendChild(ticketid);

                                username.name = 'username';
                                username.value = '" + username + @"';
                                form.appendChild(username);

                                userid.name = 'userid';
                                userid.value = '" + userId + @"';
                                form.appendChild(userid);

                                productgroupid.name = 'productgroupid';
                                productgroupid.value = '" + leadData.ProductGroupId + @"';
                                form.appendChild(productgroupid);

                                productid.name = 'productid';
                                productid.value = '" + leadData.ProductId + @"';
                                form.appendChild(productid);

                                campaign.name = 'campaign';
                                campaign.value = '" + leadData.Campaign + @"';
                                form.appendChild(campaign);

                                firstname.name = 'firstname';
                                firstname.value = '" + (leadData.Firstname != null ? leadData.Firstname.Replace("'", "") : "") + @"';
                                form.appendChild(firstname);

                                lastname.name = 'lastname';
                                lastname.value = '" + (leadData.Lastname != null ? leadData.Lastname.Replace("'", "") : "") + @"';
                                form.appendChild(lastname);

                                telno1.name = 'telno1';
                                telno1.value = '" + leadData.TelNo1 + @"';
                                form.appendChild(telno1);

                                telno2.name = 'telno2';
                                telno2.value = '" + leadData.TelNo2 + @"';
                                form.appendChild(telno2);

                                telno3.name = 'telno3';
                                telno3.value = '" + leadData.TelNo3 + @"';
                                form.appendChild(telno3);

                                extno1.name = 'extno1';
                                extno1.value = '" + leadData.ExtNo1 + @"';
                                form.appendChild(extno1);

                                extno2.name = 'extno2';
                                extno2.value = '" + leadData.ExtNo2 + @"';
                                form.appendChild(extno2);

                                extno3.name = 'extno3';
                                extno3.value = '" + leadData.ExtNo3 + @"';
                                form.appendChild(extno3);

                                email.name = 'email';
                                email.value = '" + leadData.Email + @"';
                                form.appendChild(email);

                                buildingname.name = 'buildingname';
                                buildingname.value = '" + leadData.BuildingName + @"';
                                form.appendChild(buildingname);

                                addrno.name = 'addrno';
                                addrno.value = '" + leadData.AddrNo + @"';
                                form.appendChild(addrno);
    
                                floor.name = 'floor';
                                floor.value = '" + leadData.Floor + @"';
                                form.appendChild(floor);

                                soi.name = 'soi';
                                soi.value = '" + leadData.Soi + @"';
                                form.appendChild(soi);

                                street.name = 'street';
                                street.value = '" + leadData.Street + @"';
                                form.appendChild(street);

                                tambol.name = 'tambol';
                                tambol.value = '" + leadData.TambolCode + @"';
                                form.appendChild(tambol);

                                amphur.name = 'amphur';
                                amphur.value = '" + leadData.AmphurCode + @"';
                                form.appendChild(amphur);

                                province.name = 'province';
                                province.value = '" + leadData.ProvinceCode + @"';
                                form.appendChild(province);

                                postalcode.name = 'postalcode';
                                postalcode.value = '" + leadData.PostalCode + @"';
                                form.appendChild(postalcode);

                                occupation.name = 'occupation';
                                occupation.value = '" + leadData.OccupationCode + @"';
                                form.appendChild(occupation);

                                basesalary.name = 'basesalary';
                                basesalary.value = '" + (leadData.BaseSalary != null ? leadData.BaseSalary.Value.ToString() : "") + @"';
                                form.appendChild(basesalary);

                                iscustomer.name = 'iscustomer';
                                iscustomer.value = '" + leadData.IsCustomer + @"';
                                form.appendChild(iscustomer);
    
                                customercode.name = 'customercode';
                                customercode.value = '" + leadData.CustomerCode + @"';
                                form.appendChild(customercode);

                                dateofbirth.name = 'dateofbirth';
                                dateofbirth.value = '" + (leadData.DateOfBirth != null ? leadData.DateOfBirth.Value.Year.ToString() + leadData.DateOfBirth.Value.ToString("MMdd") : "") + @"';
                                form.appendChild(dateofbirth);

                                cid.name = 'cid';
                                cid.value = '" + leadData.Cid + @"';
                                form.appendChild(cid);

                                leadstatus.name = 'status';
                                leadstatus.value = '" + leadData.Status + @"';
                                form.appendChild(leadstatus);

                                topic.name = 'topic';
                                topic.value = '" + leadData.Topic + @"';
                                form.appendChild(topic);

                                detail.name = 'detail';
                                detail.value = '" + (leadData.Detail != null ? leadData.Detail.Replace("\n", "").Replace("'", "") : "") + @"';
                                form.appendChild(detail);

                                pathlink.name = 'pathlink';
                                pathlink.value = '" + leadData.PathLink + @"';
                                form.appendChild(pathlink);

                                telesalename.name = 'telesalename';
                                telesalename.value = '" + leadData.TelesaleName + @"';
                                form.appendChild(telesalename);

                                availabletime.name = 'availabletime';
                                availabletime.value = '" + leadData.AvailableTime + @"';
                                form.appendChild(availabletime);

                                contactbranch.name = 'contactbranch';
                                contactbranch.value = '" + leadData.ContactBranch + @"';
                                form.appendChild(contactbranch);

                                interestedprodandtype.name = 'interestedprodandtype';
                                interestedprodandtype.value = '" + leadData.InterestedProdAndType + @"';
                                form.appendChild(interestedprodandtype);

                                licenseno.name = 'licenseno';
                                licenseno.value = '" + leadData.LicenseNo + @"';
                                form.appendChild(licenseno);

                                yearofcar.name = 'yearofcar';
                                yearofcar.value = '" + leadData.YearOfCar + @"';
                                form.appendChild(yearofcar);
                                
                                yearofcarregis.name = 'yearofcarregis';
                                yearofcarregis.value = '" + leadData.YearOfCarRegis + @"';
                                form.appendChild(yearofcarregis);

                                registerprovince.name = 'registerprovince';
                                registerprovince.value = '" + leadData.RegisterProvinceCode + @"';
                                form.appendChild(registerprovince);

                                brand.name = 'brand';
                                brand.value = '" + leadData.BrandCode + @"';
                                form.appendChild(brand);

                                model.name = 'model';
                                model.value = '" + leadData.ModelFamily + @"';
                                form.appendChild(model);

                                submodel.name = 'submodel';
                                submodel.value = '" + leadData.SubmodelRedBookNo + @"';
                                form.appendChild(submodel);
            
                                downpayment.name = 'downpayment';
                                downpayment.value = '" + (leadData.DownPayment != null ? leadData.DownPayment.Value.ToString() : "") + @"';
                                form.appendChild(downpayment);

                                downpercent.name = 'downpercent';
                                downpercent.value = '" + (leadData.DownPercent != null ? leadData.DownPercent.Value.ToString() : "") + @"';
                                form.appendChild(downpercent);

                                carprice.name = 'carprice';
                                carprice.value = '" + (leadData.CarPrice != null ? leadData.CarPrice.Value.ToString() : "") + @"';
                                form.appendChild(carprice);

                                financeamt.name = 'financeamt';
                                financeamt.value = '" + (leadData.FinanceAmt != null ? leadData.FinanceAmt.Value.ToString() : "") + @"';
                                form.appendChild(financeamt);

                                term.name = 'term';
                                term.value = '" + leadData.Term + @"';
                                form.appendChild(term);

                                paymenttype.name = 'paymenttype';
                                paymenttype.value = '" + leadData.PaymentTypeCode + @"';
                                form.appendChild(paymenttype);

                                balloonamt.name = 'balloonamt';
                                balloonamt.value = '" + (leadData.BalloonAmt != null ? leadData.BalloonAmt.Value.ToString() : "") + @"';
                                form.appendChild(balloonamt);

                                balloonpercent.name = 'balloonpercent';
                                balloonpercent.value = '" + (leadData.BalloonPercent != null ? leadData.BalloonPercent.Value.ToString() : "") + @"';
                                form.appendChild(balloonpercent);

                                plantype.name = 'plantype';
                                plantype.value = '" + leadData.Plantype + @"';
                                form.appendChild(plantype);

                                coveragedate.name = 'coveragedate';
                                coveragedate.value = '" + leadData.CoverageDate + @"';
                                form.appendChild(coveragedate);

                                acctype.name = 'acctype';
                                acctype.value = '" + leadData.AccTypeCode + @"';
                                form.appendChild(acctype);

                                accpromotion.name = 'accpromotion';
                                accpromotion.value = '" + leadData.AccPromotionCode + @"';
                                form.appendChild(accpromotion);
                
                                accterm.name = 'accterm';
                                accterm.value = '" + leadData.AccTerm + @"';
                                form.appendChild(accterm);
                        
                                interest.name = 'interest';
                                interest.value = '" + leadData.Interest + @"';
                                form.appendChild(interest);

                                invest.name = 'invest';
                                invest.value = '" + leadData.Invest + @"';
                                form.appendChild(invest);

                                loanod.name = 'loanod';
                                loanod.value = '" + leadData.LoanOd + @"';
                                form.appendChild(loanod);

                                loanodterm.name = 'loanodterm';
                                loanodterm.value = '" + leadData.LoanOdTerm + @"';
                                form.appendChild(loanodterm);

                                slmbank.name = 'slmbank';
                                slmbank.value = '" + leadData.SlmBank + @"';
                                form.appendChild(slmbank);
        
                                slmatm.name = 'slmatm';
                                slmatm.value = '" + leadData.SlmAtm + @"';
                                form.appendChild(slmatm);

                                otherdetail1.name = 'otherdetail1';
                                otherdetail1.value = '" + leadData.OtherDetail1 + @"';
                                form.appendChild(otherdetail1);

                                otherdetail2.name = 'otherdetail2';
                                otherdetail2.value = '" + leadData.OtherDetail2 + @"';
                                form.appendChild(otherdetail2);

                                otherdetail3.name = 'otherdetail3';
                                otherdetail3.value = '" + leadData.OtherDetail3 + @"';
                                form.appendChild(otherdetail3);

                                otherdetail4.name = 'otherdetail4';
                                otherdetail4.value = '" + leadData.OtherDetail4 + @"';
                                form.appendChild(otherdetail4);

                                cartype.name = 'cartype';
                                cartype.value = '" + leadData.CarType + @"';
                                form.appendChild(cartype);

                                channelid.name = 'channelid';
                                channelid.value = '" + leadData.ChannelId + @"';
                                form.appendChild(channelid);

                                date.name = 'date';
                                date.value = '" + (leadData.RequestDate != null ? leadData.RequestDate.Value.Year.ToString() + leadData.RequestDate.Value.ToString("MMdd") : "") + @"';
                                form.appendChild(date);

                                time.name = 'time';
                                time.value = '" + (leadData.RequestDate != null ? leadData.RequestDate.Value.ToString("HHmmss") : "") + @"';
                                form.appendChild(time);

                                createuser.name = 'createuser';
                                createuser.value = '" + leadData.CreateUser + @"';
                                form.appendChild(createuser);
                        
                                ipaddress.name = 'ipaddress';
                                ipaddress.value = '" + leadData.Ipaddress + @"';
                                form.appendChild(ipaddress);

                                company.name = 'company';
                                company.value = '" + leadData.Company + @"';
                                form.appendChild(company);

                                branch.name = 'branch';
                                branch.value = '" + leadData.Branch + @"';
                                form.appendChild(branch);

                                branchno.name = 'branchno';
                                branchno.value = '" + leadData.BranchNo + @"';
                                form.appendChild(branchno);

                                machineno.name = 'machineno';
                                machineno.value = '" + leadData.MachineNo + @"';
                                form.appendChild(machineno);

                                clientservicetype.name = 'clientservicetype';
                                clientservicetype.value = '" + leadData.ClientServiceType + @"';
                                form.appendChild(clientservicetype);

                                documentno.name = 'documentno';
                                documentno.value = '" + leadData.DocumentNo + @"';
                                form.appendChild(documentno);

                                commpaidcode.name = 'commpaidcode';
                                commpaidcode.value = '" + leadData.CommPaidCode + @"';
                                form.appendChild(commpaidcode);

                                zone.name = 'zone';
                                zone.value = '" + leadData.Zone + @"';
                                form.appendChild(zone);

                                transid.name = 'transid';
                                transid.value = '" + leadData.TransId + @"';
                                form.appendChild(transid);

                                document.body.appendChild(form);
                                form.submit();

                                document.body.removeChild(form);";

            return script;
        }
        #endregion
    }
}