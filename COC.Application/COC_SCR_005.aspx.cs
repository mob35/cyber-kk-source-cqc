using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Application.Utilities;
using COC.Biz;
using COC.Resource.Data;
using COC.Resource;
using log4net;

namespace COC.Application
{
    public partial class COC_SCR_005 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_005));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "รายงาน 1";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_005");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_002.aspx");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (tdmStartDate.DateValue.Year.ToString() == "1" || tdmEndDate.DateValue.Year.ToString() == "1")
                {
                    AppUtil.ClientAlert(Page, "กรุณากรอกวันที่ให้ครบถ้วน");
                    return;
                }

                if (tdmStartDate.DateValue.Year.ToString() != "1" && tdmEndDate.DateValue.Year.ToString() != "1")
                {
                    if (tdmStartDate.DateValue > tdmEndDate.DateValue)
                    {
                        AppUtil.ClientAlert(Page, "วันที่เริ่มต้นต้องน้อยกว่าหรือเท่ากับวันที่สิ้นสุด");
                        return;
                    }
                }

                if (tdmStartDate.DateValue > DateTime.Today || tdmEndDate.DateValue > DateTime.Today)
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถเลือกช่วงเวลาเกินวันปัจจุบันได้");
                    return;
                }

                TimeSpan ts = tdmEndDate.DateValue.Subtract(tdmStartDate.DateValue);
                if ((ts.TotalDays + 1) > AppConstant.SnapReportDateRange)
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถเลือกช่วงเวลาได้เกิน " + AppConstant.SnapReportDateRange + " วัน");
                    return;
                }

                string datefrom = tdmStartDate.DateValue.Year.ToString() + tdmStartDate.DateValue.ToString("-MM-dd");
                string dateto = tdmEndDate.DateValue.Year.ToString() + tdmEndDate.DateValue.ToString("-MM-dd");
                Int64 countrow = 0;

                if (rbAppReport.Checked == true)
                {
                    countrow = ReportBiz.CheckReportWorkDetailExist(datefrom, dateto);
                    if (countrow == 0)
                    {
                        AppUtil.ClientAlert(Page, "ไม่พบข้อมูล");
                        return;
                    }
                    else
                    {
                        string script = "window.open('" + Page.ResolveUrl("~/ExportReport.aspx?reportflag=RPT_WORKDETAIL&countrows=" + countrow + "&datefrom=" + datefrom + "&dateto=" + dateto) + "', 'reports', 'status=yes, toolbar=no, scrollbars=no, menubar=no, width=600, height=400, resizable=no');";
                        //string script = "window.open('" + (Request.ApplicationPath.EndsWith("/") ? Request.ApplicationPath : Request.ApplicationPath + "/") + "ExportReport.aspx?reportflag=RPT_WORKDETAIL&countrows=" + countrow + "&datefrom=" + datefrom + "&dateto=" + dateto + "', 'reports', 'status=yes, toolbar=no, scrollbars=no, menubar=no, width=800, height=600, resizable=no');";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "reportgeneral", script, true);
                    }
                }
                else if (rbAccessReport.Checked == true)
                {
                    countrow = ReportBiz.CheckReportStaffWorkingExist(datefrom, dateto);
                    if (countrow == 0)
                    {
                        AppUtil.ClientAlert(Page, "ไม่พบข้อมูล");
                        return;
                    }
                    else
                    {
                        string script = "window.open('" + Page.ResolveUrl("~/ExportReport.aspx?reportflag=RPT_STAFF_WORKING&countrows=" + countrow + "&datefrom=" + datefrom + "&dateto=" + dateto) + "', 'reports', 'status=yes, toolbar=no, scrollbars=no, menubar=no, width=600, height=400, resizable=no');";
                        //string script = "window.open('" + (Request.ApplicationPath.EndsWith("/") ? Request.ApplicationPath : Request.ApplicationPath + "/") + "ExportReport.aspx?reportflag=RPT_STAFF_WORKING&countrows=" + countrow + "&datefrom=" + datefrom + "&dateto=" + dateto + "', 'reports', 'status=yes, toolbar=no, scrollbars=no, menubar=no, width=800, height=600, resizable=no');";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "reportgeneral", script, true);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }
    }
}