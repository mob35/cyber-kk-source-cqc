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
    public partial class Tab007 : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Tab007));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GetOwnerLogingList(string ticketId)
        {
            try
            {
                txtTicketId.Text = ticketId;
                List<OwnerLoggingData> result = SearchLeadBiz.SearchOwnerLogging(ticketId);
                BindGridview((COC.Application.Shared.GridviewPageController)pcTop, result.ToArray(), 0);
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
            pageControl.SetGridview(gvOwnerLogging);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                List<OwnerLoggingData> result = SearchLeadBiz.SearchOwnerLogging(txtTicketId.Text.Trim());
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