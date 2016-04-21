using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Application.Utilities;
using COC.Biz;
using COC.Resource.Data;
using log4net;

namespace COC.Application.Shared.Tabs
{
    public partial class Tab005 : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Tab005));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GetExistingLeadList(string citizenId, string telNo1)
        {
            try
            {
                txtTelNo1.Text = telNo1;
                txtCitizenId.Text = citizenId;
                List<SearchLeadData> result = SearchLeadBiz.SearchExistingLead(txtCitizenId.Text.Trim(), txtTelNo1.Text.Trim());
                BindGridview((COC.Application.Shared.GridviewPageController)pcTop, result.ToArray(), 0);
                upResult.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void imbView_Click(object sender, EventArgs e)
        {
            Response.Redirect("COC_SCR_003.aspx?ticketid=" + ((ImageButton)sender).CommandArgument + "&ReturnUrl=" + Server.UrlEncode(Request["ReturnUrl"]));
        }

        #region Page Control

        private void BindGridview(COC.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvExistingLead);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                List<SearchLeadData> result = SearchLeadBiz.SearchExistingLead(txtCitizenId.Text.Trim(), txtTelNo1.Text.Trim());
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