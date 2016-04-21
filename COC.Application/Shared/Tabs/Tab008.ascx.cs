using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Resource.Data;
using COC.Biz;
using COC.Application.Utilities;
using log4net;

namespace COC.Application.Shared.Tabs
{
    public partial class Tab008 : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Tab008));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void InitialControl(LeadData data)
        {
            try
            {
                //txtTicketID.Text = data.TicketId;
                txtCitizenId.Text = data.CitizenId;
                //txtFirstname.Text = data.Name;
                //txtLastname.Text = data.LastName;
                //txtOwnerLead.Text = data.OwnerName;
                //txtOwner.Text = data.Owner;
                //txtCampaign.Text = data.CampaignName;
                txtTelNo1.Text = data.TelNo_1;
                //txtExt1.Text = data.Ext_1;

                //cmbLeadStatus.DataSource = SlmScr008Biz.GetLeadStatus(AppConstant.OptionType.LeadStatus);
                //cmbLeadStatus.DataTextField = "TextField";
                //cmbLeadStatus.DataValueField = "ValueField";
                //cmbLeadStatus.DataBind();

                //if (cmbLeadStatus.Items.Count > 0)
                //{
                //    cmbLeadStatus.SelectedIndex = cmbLeadStatus.Items.IndexOf(cmbLeadStatus.Items.FindByValue(data.Status));
                //    txtOldStatus.Text = data.Status;
                //}

                //cmbLeadStatus.Enabled = data.AssignedFlag == "1" ? true : false;
                //if (data.AssignedFlag == "1" && data.Status != "00")
                //{
                //    cmbLeadStatus.Items.Remove(cmbLeadStatus.Items.FindByValue("00"));  //ถ้าจ่ายงานแล้ว และสถานะปัจจุบันไม่ใช่สนใจ ให้เอาสถานะ สนใจ ออก
                //}

                pcTop.SetVisible = false;
                DoBindGridview();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DoBindGridview()
        {
            List<PhoneCallHistoryData> result = SearchLeadBiz.SearchPhoneCallHistory(txtCitizenId.Text.Trim(), txtTelNo1.Text.Trim());
            BindGridview((COC.Application.Shared.GridviewPageController)pcTop, result.ToArray(), 0);
            upResult.Update();
        }

        #region Page Control

        private void BindGridview(COC.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvPhoneCallHistoty);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                List<PhoneCallHistoryData> result = SearchLeadBiz.SearchPhoneCallHistory(txtCitizenId.Text.Trim(), txtTelNo1.Text.Trim());
                var pageControl = (COC.Application.Shared.GridviewPageController)sender;
                BindGridview(pageControl, result.ToArray(), pageControl.SelectedPageIndex);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #endregion
    }
}