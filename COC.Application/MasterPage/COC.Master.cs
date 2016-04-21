using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using COC.Application.Utilities;
using COC.Biz;
using log4net;

namespace COC.Application.MasterPage
{
    public partial class COC : System.Web.UI.MasterPage
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC));

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Cache.SetCacheability(HttpCacheability.NoCache); //will not save the file to temp internet files like you mentioned. 
            //Response.Cache.SetExpires(DateTime.Now.AddMinutes(-1));
            //Response.Cache.SetNoStore();      //Prevents the browser from caching the ASPX page, will not save the request or the response to and from the server

            try
            {
                if (!VerifySession())
                {
                    Logout();
                }

                if (!IsPostBack)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        DisplayUserFullname();
                        GetCurrentStatus();
                        //SetMenu();
                    }
                    else
                        Response.Redirect(FormsAuthentication.LoginUrl);
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void DisplayUserFullname()
        {
            FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket ticket = identity.Ticket;
            string[] data = ticket.UserData.Split('|');
            lblUserFullname.Text = data[0];
            lblBranchName.Text = data[1];
            txtUsername.Text = ticket.Name;

            //HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            //FormsAuthenticationTicket decTicket = FormsAuthentication.Decrypt(cookie.Value);
            //lblUserFullname.Text = decTicket.UserData;
        }

        private bool VerifySession()
        {
            FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket ticket = identity.Ticket;
            string[] data = ticket.UserData.Split('|');
            Guid sessionId = new Guid(data[2]);

            return LoginBiz.VerifySession(ticket.Name, sessionId);
        }

        protected void imbLogout_Click(object sender, ImageClickEventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            Session[AppConstant.SessionName.COC_Searchcondition] = null;
            Session[AppConstant.SessionName.COC_StaffSearchcondition] = null;
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl);
            //FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnNotAvailable_Click(object sender, EventArgs e)
        {
            try
            {
                SetCurrentStatus(0);
                SetUnavailable();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnAvailable_Click(object sender, EventArgs e)
        {
            try
            {
                SetCurrentStatus(1);
                SetAvailable();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void lbSearchLead_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/COC_SCR_002.aspx");
        }

        protected void lbUserMonitoring_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/COC_SCR_004.aspx");
        }

        protected void lbUserManagement_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/COC_SCR_006.aspx");
        }

        protected void lbReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/COC_SCR_005.aspx");
        }

        protected void lbReport2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/COC_SCR_008.aspx");
        }

        protected void lbUserRoleMatrix_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/COC_SCR_010.aspx");
        }

        protected void lblbMonitoringWS_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/COC_SCR_011.aspx");
        }

        protected void lbRanking_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/COC_SCR_101.aspx");
        }


        private void GetCurrentStatus()
        {
            string status = StaffBiz.GetCurrentStatus(txtUsername.Text.Trim());
            if (status == string.Empty)
                HideStatusDetail();
            else if (status == "1")
                SetAvailable();
            else
                SetUnavailable();
        }

        private void HideStatusDetail()
        {
            imgAvailable.Visible = false;
            imgNotAvailable.Visible = false;
            lblStatusDesc.Text = string.Empty;
            btnAvailable.Visible = false;
            btnNotAvailable.Visible = false;
        }

        private void SetAvailable()
        {
            imgAvailable.Visible = true;
            imgNotAvailable.Visible = false;
            lblStatusDesc.Text = "<b>สถานะ : </b>พร้อมทำงาน (Available)";
            btnAvailable.Visible = false;
            btnNotAvailable.Visible = true;
        }

        private void SetUnavailable()
        {
            imgAvailable.Visible = false;
            imgNotAvailable.Visible = true;
            lblStatusDesc.Text = "<b>สถานะ : </b>ไม่พร้อมทำงาน (Unavailable)";
            btnAvailable.Visible = true;
            btnNotAvailable.Visible = false;
        }

        private void SetCurrentStatus(int status)
        {
            StaffBiz.SetCurrentStatus(txtUsername.Text.Trim(), status);
        }
    }
}