using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Text;
using COC.Application.Utilities;
using COC.Biz;
using COC.Resource.Data;
using log4net;
using System.Collections;
using COC.Resource;

namespace COC.Application
{
    public partial class COC_SCR_012 : System.Web.UI.Page
    {
        
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_013));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "ค้นหาข้อมูล Ranking (Search)";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_101");
                    if (priData == null || priData.IsView != 1)
                    {
                        AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_101.aspx");
                        return;
                    }
                    DoSearchRankingData();
                    
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }     

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
              
                DoSearchRankingData(0);//, SortExpressionProperty, SortDirectionProperty
               
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }


        protected void btnSaveRanking_Click(object sender, EventArgs e)
        {
            try
            {
                int[] RankingIds = (from p in Request.Form["RankingId"].Split(',')
                                     select int.Parse(p)).ToArray();
                int preference = 0;

                List<RankingData> rankingdatas = new List<RankingData>();
                foreach (int RankingId in RankingIds)
                {
                    RankingData rankingdata = new RankingData();
                    rankingdata.coc_RankingId = RankingId;
                    rankingdata.coc_Seq = preference += 1;
                    rankingdatas.Add(rankingdata);
                }

                RankingBiz.UpdateSeq(rankingdatas, HttpContext.Current.User.Identity.Name);
                DoSearchRankingData(0);
                //btnSaveRanking.Visible = false;
                //btnCancel.Visible = false;
                //btnEdit.Visible = true;
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void DoSearchRankingData(int pageIndex = 0)//, string sortExpression, SortDirection sortDirection)
        {
            try
            {
                string orderByFlag = "";

                
                List<SearchRankingResult> result = RankingBiz.SearchRankingData();

               
                
                BindGridview( result.ToArray(), pageIndex);//(SLM.Application.Shared.GridviewPageController)pcTop,
                upResult.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        protected void btnAddRanking_Click(object sender, EventArgs e)
        {
            //Session[searchcondition] = GetSearchCondition();
            Response.Redirect("COC_SCR_102.aspx");
            //Response.Redirect("SLM_SCR_010.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.AbsoluteUri));
        }

        public string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SearchLeadCondition));
            MemoryStream ms = new MemoryStream();

            xmlSerializer.Serialize(ms, toSerialize);
            byte[] b = ms.GetBuffer();
            return Convert.ToBase64String(b);
        }

        protected void imbEdit_Click(object sender, EventArgs e)
        {
            //Session[searchcondition] = GetSearchCondition();
            Response.Redirect("COC_SCR_102.aspx?rankingid=" + ((ImageButton)sender).CommandArgument);
            //Response.Redirect("SLM_SCR_011.aspx?ticketid=" + ((ImageButton)sender).CommandArgument + "&ReturnUrl=" + Server.UrlEncode(Request.Url.AbsoluteUri));
        }



        #region Page Control

        private void BindGridview( object[] items, int pageIndex)//,SLM.Application.Shared.GridviewPageController pageControl
        {
            //pageControl.SetGridview(gvResult);
            //pageControl.Update(items, pageIndex);
            gvResult.DataSource=items;
            gvResult.DataBind();
            upResult.Update();
        }

        //protected void btnEdit_Click(object sender, EventArgs e)
        //{
        //    btnSaveRanking.Visible = true;
        //    btnCancel.Visible = true;
        //}
        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    DoSearchRankingData();
        //    btnSaveRanking.Visible = false;
        //    btnCancel.Visible = false;
            
        //}
        //protected void PageSearchChange(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var pageControl = (SLM.Application.Shared.GridviewPageController)sender;
        //        DoSearchLeadData(pageControl.SelectedPageIndex, SortExpressionProperty, SortDirectionProperty);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}

        #endregion

        //protected void lbSaleTool_Click(object sender, EventArgs e)
        //{
        //    int index = int.Parse(((LinkButton)sender).CommandArgument);
        //    string ticketid = ((ImageButton)gvResult.Rows[index].FindControl("imbView")).CommandArgument;
        //    string username = HttpContext.Current.User.Identity.Name;
        //    string saleToolHost = System.Configuration.ConfigurationManager.AppSettings["SaleToolHost"].ToString();

        //    WebRequest request = WebRequest.Create("http://" + saleToolHost + "/saletool/default.aspx");

        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";

        //    //ASCIIEncoding encoding = new ASCIIEncoding();
        //    //byte[] data = encoding.GetBytes(postData);

        //    string postData = "ticketid=" + ticketid + "&username=" + username;
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    request.ContentLength = byteArray.Length;

        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    //Response
        //    WebResponse response = request.GetResponse();
        //    //Response.Write(((HttpWebResponse)response).StatusDescription);

        //    StreamReader reader = new StreamReader(response.GetResponseStream());
        //    string responseFromServer = reader.ReadToEnd();
        //    AppUtil.ClientAlert(Page, responseFromServer);

        //    reader.Close();
        //    response.Close();

        //}

        #region Sorting

        //protected void gvResult_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    try
        //    {
        //        if (SortExpressionProperty != e.SortExpression)         //เมื่อเปลี่ยนคอลัมน์ในการ sort
        //            SortDirectionProperty = SortDirection.Ascending;
        //        else
        //        {
        //            if (SortDirectionProperty == SortDirection.Ascending)
        //                SortDirectionProperty = SortDirection.Descending;
        //            else
        //                SortDirectionProperty = SortDirection.Ascending;
        //        }

        //        SortExpressionProperty = e.SortExpression;
        //        DoSearchLeadData(0, SortExpressionProperty, SortDirectionProperty);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}

        //public SortDirection SortDirectionProperty
        //{
        //    get
        //    {
        //        if (ViewState["SortingState"] == null)
        //        {
        //            ViewState["SortingState"] = SortDirection.Ascending;
        //        }
        //        return (SortDirection)ViewState["SortingState"];
        //    }
        //    set
        //    {
        //        ViewState["SortingState"] = value;
        //    }
        //}

        //public string SortExpressionProperty
        //{
        //    get
        //    {
        //        if (ViewState["ExpressionState"] == null)
        //        {
        //            ViewState["ExpressionState"] = string.Empty;
        //        }
        //        return ViewState["ExpressionState"].ToString();
        //    }
        //    set
        //    {
        //        ViewState["ExpressionState"] = value;
        //    }
        //}

        #endregion

        //protected void cbOptionAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (cbOptionAll.Checked)
        //    {
        //        foreach (ListItem li in cbOptionList.Items)
        //        {
        //            li.Selected = true;
        //        }
        //    }
        //    else
        //    {
        //        foreach (ListItem li in cbOptionList.Items)
        //        {
        //            li.Selected = false;
        //        }
        //    }
        //}

        //protected void cbOptionList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CheckAllCondition();
        //}

        //private void CheckAllCondition()
        //{
        //    int count = 0;
        //    foreach (ListItem li in cbOptionList.Items)
        //    {
        //        if (!li.Selected) { count += 1; }
        //    }

        //    cbOptionAll.Checked = count > 0 ? false : true;
        //}

        protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (RankingBiz.isLastRankingData(((SearchRankingResult) e.Row.DataItem).coc_RankingId.ToString()))
                {
                    ((ImageButton)e.Row.FindControl("imbEdit")).Visible = false;
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("imbEdit")).Visible = true;
                }
            }
        }

        //protected void lbAolSummaryReport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int index = int.Parse(((ImageButton)sender).CommandArgument);
        //        string appNo = ((Label)gvResult.Rows[index].FindControl("lblAppNo")).Text.Trim();   //"1002363";
        //        string productId = ((Label)gvResult.Rows[index].FindControl("lblProductId")).Text.Trim();
        //        string privilegeNCB = "";

        //        if (txtStaffTypeId.Text.Trim() != "")
        //            privilegeNCB = SlmScr003Biz.GetPrivilegeNCB(productId, Convert.ToDecimal(txtStaffTypeId.Text.Trim()));

        //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "callaolsummaryreport", AppUtil.GetCallAolSummaryReportScript(appNo, txtEmpCode.Text.Trim(), txtStaffTypeDesc.Text.Trim(), privilegeNCB), true);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}

        //protected void lbDocument_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LeadDataForAdam leadData = SlmScr003Biz.GetLeadDataForAdam(((ImageButton)sender).CommandArgument);
        //        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "calladam", AppUtil.GetCallAdamScript(leadData, HttpContext.Current.User.Identity.Name, txtEmpCode.Text.Trim(), false), true);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}

        //protected void lbAdvanceSearch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (pnAdvanceSearch.Style["display"] == "" || pnAdvanceSearch.Style["display"] == "none")
        //        {
        //            lbAdvanceSearch.Text = "[-] <b>Advance Search</b>";
        //            pnAdvanceSearch.Style["display"] = "block";
        //            txtAdvanceSearch.Text = "Y";
        //        }
        //        else
        //        {
        //            lbAdvanceSearch.Text = "[+] <b>Advance Search</b>";
        //            pnAdvanceSearch.Style["display"] = "none";
        //            txtAdvanceSearch.Text = "N";
        //        }
        //        StaffBiz.SetCollapse(HttpContext.Current.User.Identity.Name, txtAdvanceSearch.Text.Trim() == "N" ? true : false);
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}

        //protected void gvResult_DataBound(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (gvResult.Rows.Count != 0) {
        //            GridViewRow row = gvResult.Rows[gvResult.Rows.Count - 1];
        //            ((ImageButton)row.FindControl("imbEdit")).Visible = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        _log.Debug(message);
        //        AppUtil.ClientAlert(Page, message);
        //    }
        //}

        //        private string GetCallAdamScript(LeadDataForAdam leadData, string username, string userId)
        //        {
        //            string script = @"var form = document.createElement('form');
        //                                var ticketid = document.createElement('input');
        //                                var username = document.createElement('input');
        //                                var product_group_id = document.createElement('input');
        //                                var product_id = document.createElement('input');
        //                                var campaign = document.createElement('input');
        //                                var firstname = document.createElement('input');
        //                                var lastname = document.createElement('input');
        //                                var license_plate = document.createElement('input');
        //                                var state = document.createElement('input');
        //                                var mobile = document.createElement('input');
        //                                var user_id = document.createElement('input');
        //
        //                                form.action = '" + System.Configuration.ConfigurationManager.AppSettings["AdamlUrl"] + @"';
        //                                form.method = 'post';
        //                                form.setAttribute('target', '_blank');
        //
        //                                ticketid.name = 'ticket_id';
        //                                ticketid.value = '" + leadData.TicketId + @"';
        //                                form.appendChild(ticketid);
        //
        //                                username.name = 'username';
        //                                username.value = '" + username + @"';
        //                                form.appendChild(username);
        //
        //                                product_group_id.name = 'productgroup_id';
        //                                product_group_id.value = '" + leadData.ProductGroupId + @"';
        //                                form.appendChild(product_group_id);
        //
        //                                product_id.name = 'product_id';
        //                                product_id.value = '" + leadData.ProductId + @"';
        //                                form.appendChild(product_id);
        //
        //                                campaign.name = 'campaign';
        //                                campaign.value = '" + leadData.Campaign + @"';
        //                                form.appendChild(campaign);
        //
        //                                firstname.name = 'name';
        //                                firstname.value = '" + leadData.Firstname + @"';
        //                                form.appendChild(firstname);
        //
        //                                lastname.name = 'lastname';
        //                                lastname.value = '" + leadData.Lastname + @"';
        //                                form.appendChild(lastname);
        //
        //                                license_plate.name = 'license_plate';
        //                                license_plate.value = '" + leadData.LicenseNo + @"';
        //                                form.appendChild(license_plate);
        //
        //                                state.name = 'state';
        //                                state.value = '" + leadData.RegisterProvinceCode + @"';
        //                                form.appendChild(state);
        //
        //                                mobile.name = 'mobile';
        //                                mobile.value = '" + leadData.TelNo1 + @"';
        //                                form.appendChild(mobile);
        //
        //                                user_id.name = 'user_id';
        //                                user_id.value = '" + userId + @"';
        //                                form.appendChild(user_id);
        //                
        //                                document.body.appendChild(form);
        //                                form.submit();
        //
        //                                document.body.removeChild(form);";

        //            return script;
        //        }

        //        private string GetCallAdamScript(string ticketId, string productGroupId, string productId, string campaignId, string firstname, string lastname, string license_plate, string state, string mobile)
        //        {
        //            string script = @"var form = document.createElement('form');
        //                                var input_ticketid = document.createElement('input');
        //                                var input_username = document.createElement('input');
        //                                var input_product_group_id = document.createElement('input');
        //                                var input_product_id = document.createElement('input');
        //                                var input_campaign = document.createElement('input');
        //                                var input_name = document.createElement('input');
        //                                var input_lastname = document.createElement('input');
        //                                var input_license_plate = document.createElement('input');
        //                                var input_state = document.createElement('input');
        //                                var input_mobile = document.createElement('input');
        //                                var input_user_id = document.createElement('input');
        //
        //                                form.action = '" + System.Configuration.ConfigurationManager.AppSettings["AdamlUrl"] + @"';
        //                                form.method = 'post';
        //                                form.setAttribute('target', '_blank');
        //
        //                                input_ticketid.name = 'ticket_id';
        //                                input_ticketid.value = '" + ticketId + @"';
        //                                form.appendChild(input_ticketid);
        //
        //                                input_username.name = 'username';
        //                                input_username.value = '" + HttpContext.Current.User.Identity.Name + @"';
        //                                form.appendChild(input_username);
        //
        //                                input_product_group_id.name = 'productgroup_id';
        //                                input_product_group_id.value = '" + productGroupId + @"';
        //                                form.appendChild(input_product_group_id);
        //
        //                                input_product_id.name = 'product_id';
        //                                input_product_id.value = '" + productId + @"';
        //                                form.appendChild(input_product_id);
        //
        //                                input_campaign.name = 'campaign';
        //                                input_campaign.value = '" + campaignId + @"';
        //                                form.appendChild(input_campaign);
        //
        //                                input_name.name = 'name';
        //                                input_name.value = '" + firstname + @"';
        //                                form.appendChild(input_name);
        //
        //                                input_lastname.name = 'lastname';
        //                                input_lastname.value = '" + lastname + @"';
        //                                form.appendChild(input_lastname);
        //
        //                                input_license_plate.name = 'license_plate';
        //                                input_license_plate.value = '" + license_plate + @"';
        //                                form.appendChild(input_license_plate);
        //
        //                                input_state.name = 'state';
        //                                input_state.value = '" + state + @"';
        //                                form.appendChild(input_state);
        //
        //                                input_mobile.name = 'mobile';
        //                                input_mobile.value = '" + mobile + @"';
        //                                form.appendChild(input_mobile);
        //
        //                                input_user_id.name = 'user_id';
        //                                input_user_id.value = '103431';
        //                                form.appendChild(input_user_id);
        //                
        //                                document.body.appendChild(form);
        //                                form.submit();
        //
        //                                document.body.removeChild(form);";

        //            return script;
        //        }
    }
}