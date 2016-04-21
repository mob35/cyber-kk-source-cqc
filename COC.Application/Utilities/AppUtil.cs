using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Resource.Data;
using COC.Biz;

namespace COC.Application.Utilities
{
    public class AppUtil
    {
        public static void ClientAlert(Control control, string message)
        {
            if (control != null && message != null)
                ScriptManager.RegisterClientScriptBlock(control, control.GetType(), "error", "alert('" + message.Replace("'", "\\'").Replace("\r\n", "\\n") + "');", true);
        }

        public static void ClientAlertAndRedirect(Control control, string message, string url)
        {
            if (control != null && message != null)
            {
                ScriptManager.RegisterClientScriptBlock(control, control.GetType(), "error", "alert('" + message.Replace("'", "\\'").Replace("\r\n", "\\n") + "'); document.location='" + url + "';", true);
            }
        }

        public static string GetShowCampaignDescScript(Page page, string campaignId, string scriptName)
        {
            return "window.open('" + page.ResolveUrl("~/Shared/CampaignDesc.aspx?campaignid=" + campaignId) + "', '" + scriptName + "', 'status=yes, toolbar=no, scrollbars=yes, menubar=no, width=800, height=600, resizable=yes'); return false;";
        }

        public static void SetIntTextBox(TextBox textbox)
        {
            textbox.Attributes.Add("OnKeyPress", "return ChkInt(event)");
        }

        public static void SetIntTextBox(TextBox textbox, Label label, string errorMsg)
        {
            textbox.Attributes.Add("OnKeyPress", "return ChkInt(event)");
            textbox.Attributes.Add("OnBlur", "ChkIntOnBlur(this, '" + label.ClientID + "', '" + errorMsg + "')");
        }

        public static void SetMoneyTextBox(TextBox textbox)
        {
            textbox.Attributes.Add("OnKeyPress", "return ChkDbl(event, this)");
            textbox.Attributes.Add("OnBlur", "valDbl(this)");
            textbox.Attributes.Add("OnFocus", "prepareNum(this)");
        }

        public static void SetMoneyTextBox(TextBox textbox, string label_clientId, string errMsg, int maxlength)
        {
            textbox.Attributes.Add("OnKeyPress", "return ChkDbl(event, this)");
            textbox.Attributes.Add("OnBlur", "valDbl2(this, '" + label_clientId + "', '" + errMsg + "', " + maxlength.ToString() + ")");
            textbox.Attributes.Add("OnFocus", "prepareNum(this)");
        }

        public static void SetPercentTextBox(TextBox textbox, string label_clientId, string errMsg)
        {
            textbox.Attributes.Add("OnKeyPress", "return ChkDbl(event, this)");
            textbox.Attributes.Add("OnBlur", "valPercent(this, '" + label_clientId + "', '" + errMsg + "')");
            textbox.Attributes.Add("OnFocus", "prepareNum(this)");
        }

        public static void SetMultilineMaxLength(TextBox textbox, string labelId, string maxlength)
        {
            textbox.Attributes.Add("onkeyup", "return validateLimit(this, '" + labelId + "', " + maxlength + ")");
            textbox.Attributes.Add("onblur", "return validateLimit(this, '" + labelId + "', " + maxlength + ")");
        }

        public static void SetNotThaiCharacter(TextBox textbox)
        {
            textbox.Attributes.Add("OnKeyPress", "return ChkNotThaiCharacter(event)");
        }

        public static string SetDecimalFormat(int maxlength, TextBox textbox)
        {
            string value = textbox.Text.Trim().Replace(",", "");

            if (value.IndexOf(".") >= 0)
            {
                string[] val = value.Split('.');
                if (val[0].Trim().Length == 0 && val[1].Trim().Length == 0)
                    return "0.00";

                if (val[0].Trim().Length > maxlength)
                    return "error";

                return Convert.ToDecimal(value).ToString("#,##0.00");
            }
            else
            {
                if (value == string.Empty)
                    return string.Empty;
                else
                {
                    return value.Length > maxlength ? "error" : Convert.ToDecimal(value).ToString("#,##0.00");
                }
            }
        }
        public static string SetPercentFormat(int maxlength, TextBox textbox)
        {
            string value = textbox.Text.Trim().Replace(",", "");

            if (value.IndexOf(".") >= 0)
            {
                string[] val = value.Split('.');
                if (val[0].Trim().Length == 0 && val[1].Trim().Length == 0)
                    return "0.00";

                if (val[0].Trim().Length > maxlength)
                    return "error";

                if (Convert.ToDecimal(value) > 100)
                    return "error100";

                return Convert.ToDecimal(value).ToString("0.00");
            }
            else
            {
                if (value == string.Empty)
                    return string.Empty;
                else
                {
                    if (Convert.ToDecimal(value) > 100)
                        return "error100";

                    return value.Length > maxlength ? "error" : Convert.ToDecimal(value).ToString("0.00");
                }
            }
        }

        /// <summary>
        /// <br>Method Name : ConvertToDateTime</br>
        /// <br>Purpose     : To convert date and time string from xml to class DateTime.</br>
        /// </summary>
        /// <param name="date">string format yyyyMMdd</param>
        /// <param name="time">string format HHmmss</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string date, string time)
        {
            try
            {
                if (string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date) || date.Length != 8)
                {
                    return new DateTime();
                }
                else
                {
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);

                    if (string.IsNullOrEmpty(time) || string.IsNullOrWhiteSpace(time) || time.Length != 6)
                    {
                        return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                    }
                    else
                    {
                        string hour = time.Substring(0, 2);
                        string min = time.Substring(2, 2);
                        string sec = time.Substring(4, 2);
                        return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(min), int.Parse(sec));
                    }
                }
            }
            catch
            {
                return new DateTime();
            }
        }

        public static string GetRecursiveStaff(string username)
        {
            string userList = "";
            ArrayList arrlist = new ArrayList();

            List<StaffData> staffList = StaffBiz.GetStaffList();
            var logInStaffId = staffList.Where(p => p.UserName.Trim().ToUpper() == username.Trim().ToUpper()).Select(p => p.StaffId).FirstOrDefault();
            var logInEmpCode = staffList.Where(p => p.UserName.Trim().ToUpper() == username.Trim().ToUpper()).Select(p => p.EmpCode).FirstOrDefault();

            if (logInStaffId != null)
            {
                if (!string.IsNullOrEmpty(logInEmpCode)) arrlist.Add("'" + logInEmpCode + "'");

                FindStaffRecusive(logInStaffId, arrlist, staffList);
            }

            foreach (string empCode in arrlist)
            {
                userList += (userList == "" ? "" : ",") + empCode;
            }

            return userList;
        }

        private static void FindStaffRecusive(int? headId, ArrayList arr, List<StaffData> staffList)
        {
            foreach (StaffData staff in staffList)
            {
                if (staff.HeadStaffId == headId)
                {
                    if (!string.IsNullOrEmpty(staff.EmpCode)) arr.Add("'" + staff.EmpCode + "'");

                    FindStaffRecusive(staff.StaffId, arr, staffList);
                }
            }
        }

        public static string GetRecursiveTeam(string username)
        {
            List<string> teamList = new List<string>();

            List<StaffData> staffList = StaffBiz.GetStaffList();
            var logInStaffId = staffList.Where(p => p.UserName.Trim().ToUpper() == username.Trim().ToUpper()).Select(p => p.StaffId).FirstOrDefault();
            var logInCocTeam = staffList.Where(p => p.UserName.Trim().ToUpper() == username.Trim().ToUpper()).Select(p => p.CocTeam).FirstOrDefault();

            if (logInStaffId != null)
            {
                if (!string.IsNullOrEmpty(logInCocTeam) && logInCocTeam.Trim().ToUpper().StartsWith("COC"))
                    teamList.Add(logInCocTeam);

                FindTeamRecusive(logInStaffId, teamList, staffList);
            }

            string tmpList = "";
            foreach (string team in teamList)
            {
                tmpList += (tmpList == "" ? "" : ",") + "'" + team + "'";
            }

            return tmpList;
        }

        private static void FindTeamRecusive(int? headId, List<string> teamList, List<StaffData> staffList)
        {
            foreach (StaffData staff in staffList)
            {
                if (staff.HeadStaffId == headId)
                {
                    if (!string.IsNullOrEmpty(staff.CocTeam) && !teamList.Contains(staff.CocTeam) && staff.CocTeam.Trim().ToUpper().StartsWith("COC"))
                        teamList.Add(staff.CocTeam);

                    FindTeamRecusive(staff.StaffId, teamList, staffList);
                }
            }
        }
        public static int SafeInt(string val)
        {
            int d; int.TryParse(val, out d);
            return d;
        }
    }
}