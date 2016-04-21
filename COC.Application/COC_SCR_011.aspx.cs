using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Resource.Data;
using log4net;
using COC.Biz;
using COC.Application.Utilities;

namespace COC.Application
{
    public partial class COC_SCR011 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR011));

        protected override void OnInit(EventArgs e)
        {
            //pnAdvanceSearch.Style["display"] = "block";
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "Monitoring Webservice";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_011");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_002.aspx");
                        return;
                    }
                    else
                        InitialControl();
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
            cmbWSName.DataSource = MonitoringWSBiz.GetWebserviceList();
            cmbWSName.DataTextField = "TextField";
            cmbWSName.DataValueField = "ValueField";
            cmbWSName.DataBind();
            cmbWSName.Items.Insert(0, new ListItem("ทั้งหมด", ""));

            pcTop.SetVisible = false;
            imgResult.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (tdSearchFrom.DateValue.Year != 1 && tdSearchTo.DateValue.Year == 1)
                    AppUtil.ClientAlert(Page, "กรุณาระบุวันที่ให้ครบถ้วน");
                else if (tdSearchFrom.DateValue.Year == 1 && tdSearchTo.DateValue.Year != 1)
                    AppUtil.ClientAlert(Page, "กรุณาระบุวันที่ให้ครบถ้วน");
                else
                    DoSearchWSLog(0);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void DoSearchWSLog(int pageIndex)
        {
            List<MonitoringWSData> uData = MonitoringWSBiz.GetWSLog(tdSearchFrom.DateValue, tdSearchTo.DateValue, cmbWSName.SelectedItem.Value, cmbStatus.SelectedItem.Value);
            imgResult.Visible = true;
            BindGridview((COC.Application.Shared.GridviewPageController)pcTop, uData.ToArray(), pageIndex);
            upResult.Update();
        }


        #region Page Control
        private void BindGridview(COC.Application.Shared.GridviewPageController pageControl, object[] items, int pageIndex)
        {
            pageControl.SetGridview(gvResult);
            pageControl.Update(items, pageIndex);
            upResult.Update();
        }

        protected void PageChange(object sender, EventArgs e)
        {
            try
            {
                var pageControl = (COC.Application.Shared.GridviewPageController)sender;
                DoSearchWSLog(pageControl.SelectedPageIndex);
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