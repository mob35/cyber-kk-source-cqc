using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using COC.Application.Utilities;
using COC.Biz;
using COC.Resource.Data;
using log4net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace COC.Application
{
    public partial class Login : System.Web.UI.Page
    {
        private string _displayName = "";
        private static readonly ILog _log = LogManager.GetLogger(typeof(Login));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    Response.Redirect(FormsAuthentication.DefaultUrl);
                }
            }
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {
                LoginUserData staffData = StaffBiz.GetLoginUserData(Login1.UserName.Trim());

                if (staffData != null)
                //if (staffData != null && IsAuthenticated(Login1.UserName.Trim(), Login1.Password.Trim()))
                {
                    Guid sessionId = Guid.NewGuid();
                    LoginBiz.InsertSession(Login1.UserName.Trim(), sessionId);

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        Login1.UserName.Trim(),
                        DateTime.Now,
                        DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                        Login1.RememberMeSet,
                        staffData != null ? staffData.StaffNameTH + "|" + staffData.BranchName + "|" + sessionId.ToString() : _displayName + "|" + "",
                        FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    //Response.Redirect(FormsAuthentication.GetRedirectUrl(Login1.UserName, Login1.RememberMeSet), false);

                    InsertLoginLog(true, "");

                    if (Request["ticketid"] != null && Request["accflag"] == "email")
                        Response.Redirect("COC_SCR_003.aspx?ticketid=" + Request["ticketid"], false);
                    else
                        Response.Redirect(FormsAuthentication.DefaultUrl, false);
                }
                else
                {
                    InsertLoginLog(false, "Logon failure: unknown user name or bad password.");
                    _log.Debug("Logon failure: unknown user name or bad password.");
                    AppUtil.ClientAlert(Page, "Logon failure: unknown user name or bad password.");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                InsertLoginLog(false, message);
                _log.Debug("(" + Login1.UserName + ") " + message);
                AppUtil.ClientAlert(Page, "Logon failure: unknown user name or bad password.");
            }
        }

        private bool IsAuthenticated(string username, string password)
        {
            try
            {
                string domainName = ConfigurationManager.AppSettings["LoginDomain"].ToString();
                string domainAndUsername = string.Format(@"{0}\{1}", domainName, username);

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domainName, domainAndUsername, password);

                UserPrincipal user = new UserPrincipal(ctx);
                user.SamAccountName = username;

                PrincipalSearcher search = new PrincipalSearcher(user);
                UserPrincipal result = (UserPrincipal)search.FindOne();

                if (result != null)
                {
                    _displayName = result.DisplayName;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetIP4Address()
        {
            string IP4Address = ""; // Context.Request.ServerVariables["REMOTE_ADDR"];
            
            //string qq = Dns.GetHostAddresses(Dns.GetHostName()).Where(p => p.AddressFamily.ToString() == "InterNetwork").Select(p => p.ToString()).FirstOrDefault();

            foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostName))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (string.IsNullOrEmpty(IP4Address))
                IP4Address = Request.UserHostAddress;

            if (IP4Address == "::1")
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }
            }

            return IP4Address;
        }

        private void InsertLoginLog(bool loginResult, string errorDetail)
        {
            try
            {
                LoginBiz.InsertLoginLog(ConfigurationManager.AppSettings["LoginDomain"].ToString(), Login1.UserName, GetIP4Address(), loginResult, errorDetail);
            }
            catch(Exception ex)
            {
                _log.Debug(Login1.UserName + ", Method=InsertLoginLog, Error=" + ex.Message);
            }
        }
    }
}