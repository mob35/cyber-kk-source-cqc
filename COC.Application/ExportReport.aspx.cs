using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using COC.Application.Utilities;

namespace COC.Application
{
    public partial class ExportReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request["reportflag"] == "RPT_WORKDETAIL")
                {
                    GenerateReportWorkingDetail(Page, Request["datefrom"], Request["dateto"], Request["countrows"]);
                }
                else if (Request["reportflag"] == "RPT_STAFF_WORKING")
                {
                    GenerateReportStaffWorkingDetail(Page, Request["datefrom"], Request["dateto"], Request["countrows"]);
                }
                else if (Request["reportflag"] == "RPT_SNAP_MONITORING")
                {
                    GenerateReportSnapMonitoring(Page, Request["datefrom"], Request["dateto"], Server.UrlDecode(Request["teamlist"]));
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void GenerateReportWorkingDetail(Control page, string dateFrom, string dateTo,string countrows)
        {
            try
            {
                string filename = "";
                string reportName = "";

                filename = "RPT_WORKDETAIL_" + DateTime.Today.Year.ToString() + DateTime.Today.Date.ToString("MMdd");
                reportName = "RPT_WORKDETAIL";
                ReportViewer report = new ReportViewer();
                report.ServerReport.ReportServerCredentials = new ReportServerNetworkCredentials();

                report.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServer"].ToString());
                report.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString() + reportName;


                ReportParameter[] reportParameterCollection = new ReportParameter[2];
                reportParameterCollection[0] = new ReportParameter("STARTDATE", (dateFrom.Trim() != "" ? dateFrom : " "));
                reportParameterCollection[1] = new ReportParameter("ENDDATE", (dateTo.Trim() != "" ? dateTo : " "));
                report.ServerReport.SetParameters(reportParameterCollection);
                report.ServerReport.Refresh();

                //Export to Excel
                string mimeType;
                string encoding;
                string extension;
                string[] streams;
                Microsoft.Reporting.WebForms.Warning[] warnings;

                byte[] pdfContent = report.ServerReport.Render("Excel", null, out mimeType, out encoding, out extension, out streams, out warnings);

                // Return Excel
                ((Page)page).Response.Clear();
                ((Page)page).Response.ContentType = "application/vnd.ms-excel";
                ((Page)page).Response.AddHeader("Content-disposition", "attachment; filename=" + filename + ".xls");
                ((Page)page).Response.BinaryWrite(pdfContent);
                ((Page)page).Response.Flush();
                ((Page)page).Response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private ReportParameter[] SetReportWorkingDetailParameters(string dateFrom, string dateTo)
        {
            ReportParameter[] reportParameterCollection = new ReportParameter[2];
            reportParameterCollection[0] = new ReportParameter("STARTDATE", (dateFrom.Trim() != "" ? dateFrom : " "));
            reportParameterCollection[1] = new ReportParameter("ENDDATE", (dateTo.Trim() != "" ? dateTo : " "));
            return reportParameterCollection;
        }

        private void GenerateReportStaffWorkingDetail(Control page, string dateFrom, string dateTo, string countrows)
        {
            try
            {
                string filename = "RPT_STAFF_WORKING" + DateTime.Today.Year.ToString() + DateTime.Today.Date.ToString("MMdd");
                string reportName = "RPT_STAFF_WORKING";
                ReportViewer report = new ReportViewer();
                report.ServerReport.ReportServerCredentials = new ReportServerNetworkCredentials();

                report.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServer"].ToString());
                report.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString() + reportName;

                ReportParameter[] reportParameterCollection = SetReportWorkingDetailParameters(dateFrom, dateTo);
                report.ServerReport.SetParameters(reportParameterCollection);
                report.ServerReport.Refresh();

                //Export to Excel
                string mimeType;
                string encoding;
                string extension;
                string[] streams;
                Microsoft.Reporting.WebForms.Warning[] warnings;

                byte[] pdfContent = report.ServerReport.Render("Excel", null, out mimeType, out encoding, out extension, out streams, out warnings);

                // Return Excel 
                ((Page)page).Response.Clear();
                ((Page)page).Response.ContentType = "application/vnd.ms-excel";
                ((Page)page).Response.AddHeader("Content-disposition", "attachment; filename=" + filename + ".xls");
                ((Page)page).Response.BinaryWrite(pdfContent);
                ((Page)page).Response.Flush();
                ((Page)page).Response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateReportSnapMonitoring(Control page, string dateFrom, string dateTo, string teamlist)
        {
            try
            {
                string filename = "";
                string reportName = "";

                filename = "RPT_SNAP_MONITORING_" + DateTime.Today.Year.ToString() + DateTime.Today.Date.ToString("MMdd");
                reportName = "RPT_SNAP_MONITORING";
                ReportViewer report = new ReportViewer();
                report.ServerReport.ReportServerCredentials = new ReportServerNetworkCredentials();

                report.ServerReport.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServer"].ToString());
                report.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString() + reportName;


                ReportParameter[] reportParameterCollection = new ReportParameter[12];
                reportParameterCollection[0] = new ReportParameter("STARTDATE", (dateFrom.Trim() != "" ? dateFrom : " "));
                reportParameterCollection[1] = new ReportParameter("ENDDATE", (dateTo.Trim() != "" ? dateTo : " "));

                List<string> teams = teamlist.Trim().Replace("'", "").Split(',').ToList();
                while (teams.Count < 10)
                {
                    teams.Add("");
                }

                reportParameterCollection[2] = new ReportParameter("COC1", (teams.Where(p => p == "COC1").FirstOrDefault() != null ? teams.Where(p => p == "COC1").FirstOrDefault() : " "));
                reportParameterCollection[3] = new ReportParameter("COC2", teams.Where(p => p == "COC2").FirstOrDefault() != null ? teams.Where(p => p == "COC2").FirstOrDefault() : " ");
                reportParameterCollection[4] = new ReportParameter("COC3", teams.Where(p => p == "COC3").FirstOrDefault() != null ? teams.Where(p => p == "COC3").FirstOrDefault() : " ");
                reportParameterCollection[5] = new ReportParameter("COC4", teams.Where(p => p == "COC4").FirstOrDefault() != null ? teams.Where(p => p == "COC4").FirstOrDefault() : " ");
                reportParameterCollection[6] = new ReportParameter("COC5", teams.Where(p => p == "COC5").FirstOrDefault() != null ? teams.Where(p => p == "COC5").FirstOrDefault() : " ");
                reportParameterCollection[7] = new ReportParameter("COC6", teams.Where(p => p == "COC6").FirstOrDefault() != null ? teams.Where(p => p == "COC6").FirstOrDefault() : " ");
                reportParameterCollection[8] = new ReportParameter("COC7", teams.Where(p => p == "COC7").FirstOrDefault() != null ? teams.Where(p => p == "COC7").FirstOrDefault() : " ");
                reportParameterCollection[9] = new ReportParameter("COC8", teams.Where(p => p == "COC8").FirstOrDefault() != null ? teams.Where(p => p == "COC8").FirstOrDefault() : " ");
                reportParameterCollection[10] = new ReportParameter("COC9", teams.Where(p => p == "COC9").FirstOrDefault() != null ? teams.Where(p => p == "COC9").FirstOrDefault() : " ");
                reportParameterCollection[11] = new ReportParameter("COC10", teams.Where(p => p == "COC10").FirstOrDefault() != null ? teams.Where(p => p == "COC10").FirstOrDefault() : " ");

                report.ServerReport.SetParameters(reportParameterCollection);
                report.ServerReport.Refresh();

                //Export to Excel
                string mimeType;
                string encoding;
                string extension;
                string[] streams;
                Microsoft.Reporting.WebForms.Warning[] warnings;

                byte[] pdfContent = report.ServerReport.Render("Excel", null, out mimeType, out encoding, out extension, out streams, out warnings);

                // Return Excel
                ((Page)page).Response.Clear();
                ((Page)page).Response.ContentType = "application/vnd.ms-excel";
                ((Page)page).Response.AddHeader("Content-disposition", "attachment; filename=" + filename + ".xls");
                ((Page)page).Response.BinaryWrite(pdfContent);
                ((Page)page).Response.Flush();
                ((Page)page).Response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Serializable]
        public sealed class ReportServerNetworkCredentials : IReportServerCredentials
        {
            #region IReportServerCredentials Members

            /// <summary>
            /// Provides forms authentication to be used to connect to the report server.
            /// </summary>
            /// <param name="authCookie">A Report Server authentication cookie.</param>
            /// <param name="userName">The name of the user.</param>
            /// <param name="password">The password of the user.</param>
            /// <param name="authority">The authority to use when authenticating the user, such as a Microsoft Windows domain.</param>
            /// <returns></returns>
            public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string userName,
                out string password, out string authority)
            {
                authCookie = null;
                userName = null;
                password = null;
                authority = null;

                return false;
            }

            /// <summary>
            /// Specifies the user to impersonate when connecting to a report server.
            /// </summary>
            /// <value></value>
            /// <returns>A WindowsIdentity object representing the user to impersonate.</returns>
            public System.Security.Principal.WindowsIdentity ImpersonationUser
            {
                get
                {
                    return null;
                }
            }

            /// <summary>
            /// Returns network credentials to be used for authentication with the report server.
            /// </summary>
            /// <value></value>
            /// <returns>A NetworkCredentials object.</returns>
            public System.Net.ICredentials NetworkCredentials
            {
                get
                {
                    string userName = System.Configuration.ConfigurationManager.AppSettings["ReportUser"].ToString();
                    string domainName = System.Configuration.ConfigurationManager.AppSettings["ReportDomain"].ToString();
                    string password = System.Configuration.ConfigurationManager.AppSettings["ReportPass"].ToString();

                    return new System.Net.NetworkCredential(userName, password, domainName);
                }
            }

            #endregion
        }
    }
}