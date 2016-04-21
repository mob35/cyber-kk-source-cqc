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
    public partial class Tab006 : System.Web.UI.UserControl
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Tab006));

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GetExistingProductList(string citizenId)
        {
            try
            {
                txtCitizenId.Text = citizenId;
                List<ExistingProductData> result = SearchLeadBiz.SearchExistingProduct(txtCitizenId.Text.Trim());
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
            pageControl.SetGridview(gvExistProduct);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageSearchChange(object sender, EventArgs e)
        {
            try
            {
                List<ExistingProductData> result = SearchLeadBiz.SearchExistingProduct(txtCitizenId.Text.Trim());
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