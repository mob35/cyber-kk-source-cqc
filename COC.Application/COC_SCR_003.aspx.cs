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
    public partial class COC_SCR_003 : System.Web.UI.Page
    {
        private LeadData lead = null;
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_003));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_003");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_002.aspx");
                        return;
                    }

                    if (Request["ticketid"] != null && Request["ticketid"].ToString().Trim() != string.Empty)
                    {
                        txtTicketID.Text = Request["ticketid"].ToString();

                        if (!CheckTicketIdPrivilege(txtTicketID.Text.Trim())) { return; }

                        ((Label)Page.Master.FindControl("lblTopic")).Text = "แสดงข้อมูล Lead: " + txtTicketID.Text.Trim() + " (View)";

                        StaffData staff = StaffBiz.GetStaffInfo(HttpContext.Current.User.Identity.Name);
                        if (staff != null)
                            txtUserLoginChannelId.Text = staff.ChannelId;

                        GetLeadData();
                        tabExistingLead.GetExistingLeadList(txtCitizenId.Text.Trim(), txtTelNo1.Text.Trim());
                        tabExistingProduct.GetExistingProductList(txtCitizenId.Text.Trim());
                        tabOwnerLogging.GetOwnerLogingList(txtTicketID.Text.Trim());
                        tabPhoneCallHistory.InitialControl(lead);
                        tabNoteHistory.InitialControl(lead);
                        upTabMain.Update();
                    }
                    else
                        AppUtil.ClientAlertAndRedirect(Page, "Ticket Id not found", "COC_SCR_002.aspx");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlertAndRedirect(Page, message, "COC_SCR_002.aspx");
            }
        }

        private bool CheckTicketIdPrivilege(string ticketId)
        {
            decimal? staffTypeId = StaffBiz.GetStaffType(HttpContext.Current.User.Identity.Name);
            string staff_type_id = staffTypeId != null ? staffTypeId.Value.ToString() : "";
            string username = HttpContext.Current.User.Identity.Name;

            if (!RoleBiz.CheckTicketIdPrivilege(ticketId, username, AppUtil.GetRecursiveStaff(username), AppUtil.GetRecursiveTeam(username), staff_type_id))
            {
                string message = "ข้อมูลผู้มุ่งหวังรายนี้ ท่านไม่มีสิทธิในการมองเห็น";
                string lastOwnerName = LeadBiz.GetLastOwnerName(ticketId);

                if (!string.IsNullOrEmpty(lastOwnerName))
                    message += " ณ ปัจจุบันผู้เป็นเจ้าของ คือ " + lastOwnerName.ToString().Trim();

                AppUtil.ClientAlertAndRedirect(Page, message, "COC_SCR_002.aspx");
                return false;
            }
            else
                return true;
        }

        private void GetLeadData()
        {
            try
            {
                lead = SearchLeadBiz.GetLeadInfo(txtTicketID.Text.Trim());
                if (lead != null)
                {
                    tabLeadInfo.GetLeadData(lead);
                    txtstatus.Text = lead.StatusName;
                    txtFirstname.Text = lead.Name;
                    txtFirstname.ToolTip = lead.Name;
                    txtLastname.Text = lead.LastName;
                    txtLastname.ToolTip = lead.LastName;
                    txtCampaignId.Text = lead.CampaignId;
                    txtCampaignName.Text = lead.CampaignName;
                    txtCampaignName.ToolTip = lead.CampaignName;
                    txtInterestedProd.Text = lead.InterestedProd;
                    txtInterestedProd.ToolTip = lead.InterestedProd;
                    txtCitizenId.Text = lead.CitizenId;
                    txtChannelId.Text = lead.ChannelId;
                    txtTelNo1.Text = lead.TelNo_1;
                    if (lead.ContactLatestDate != null)
                        txtContactLatestDate.Text = lead.ContactLatestDate.Value.ToString("dd/MM/") + lead.ContactLatestDate.Value.Year.ToString() + " " + lead.ContactLatestDate.Value.ToString("HH:mm:ss");
                    if (lead.AssignedDateView != null)
                        txtAssignDate.Text = lead.AssignedDateView.Value.ToString("dd/MM/") + lead.AssignedDateView.Value.Year.ToString() + " " + lead.AssignedDateView.Value.ToString("HH:mm:ss");
                    if (lead.ContactFirstDate != null)
                        txtContactFirstDate.Text = lead.ContactFirstDate.Value.ToString("dd/MM/") + lead.ContactFirstDate.Value.Year.ToString() + " " + lead.ContactFirstDate.Value.ToString("HH:mm:ss");
                    if (lead.CocAssignedDate != null)
                        txtCOCAssignDate.Text = lead.CocAssignedDate.Value.ToString("dd/MM/") + lead.CocAssignedDate.Value.Year.ToString() + " " + lead.CocAssignedDate.Value.ToString("HH:mm:ss");
                    txtOwnerLead.Text = lead.OwnerName;
                    txtOwnerLead.ToolTip = lead.OwnerName;
                    txtDelegateLead.Text = lead.DelegateName;
                    txtDelegateLead.ToolTip = lead.DelegateName;
                    txtDelegateBranch.Text = lead.DelegatebranchName;
                    txtDelegateBranch.ToolTip = lead.DelegatebranchName;
                    txtOwnerBranch.Text = lead.OwnerBranchName;
                    txtOwnerBranch.ToolTip = lead.OwnerBranchName;
                    txtTelNo_1.Text = lead.TelNo_1;
                    txtTelNo2.Text = lead.TelNo_2;
                    txtExt2.Text = lead.Ext_2;
                    txtTelNo3.Text = lead.TelNo_3;
                    txtExt3.Text = lead.Ext_3;
                    txtMarketingOwner.Text = lead.MarketingOwnerName;
                    txtMarketingOwner.ToolTip = lead.MarketingOwnerName;
                    txtLastOwner.Text = lead.LastOwnerName;
                    txtLastOwner.ToolTip = lead.LastOwnerName;
                    txtCocTeam.Text = lead.CocTeam;
                    txtCocTeam.ToolTip = lead.CocTeam;
                    txtCocStatus.Text = lead.CocStatusDesc;
                    txtCocStatus.ToolTip = lead.CocStatusDesc;

                    GetCampaignList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetCampaignList()
        {
            try
            {
                List<CampaignWSData> cList = SearchLeadBiz.GetCampaignFinalList(txtTicketID.Text.Trim());
                lbSum.Text = "<font class='hilightGreen'><b>" + cList.Count.ToString("#,##0") + "</b></font>";
                gvCampaign.DataSource = cList;
                gvCampaign.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvCampaign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblCampaignDesc = (Label)e.Row.FindControl("lbCampaignDetail");
                    if (lblCampaignDesc.Text.Trim().Length > AppConstant.Campaign.DisplayCampaignDescMaxLength)
                    {
                        lblCampaignDesc.Text = lblCampaignDesc.Text.Trim().Substring(0, AppConstant.Campaign.DisplayCampaignDescMaxLength) + "...";
                        LinkButton lbShowCampaignDesc = (LinkButton)e.Row.FindControl("lbShowCampaignDesc");
                        lbShowCampaignDesc.Visible = true;
                        lbShowCampaignDesc.OnClientClick = AppUtil.GetShowCampaignDescScript(Page, lbShowCampaignDesc.CommandArgument, "003_campaign_campaigndesc_" + lbShowCampaignDesc.CommandArgument);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/COC_SCR_002.aspx");
        }
    }
}