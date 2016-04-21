using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Application.Utilities;
using log4net;
using COC.Biz;
using COC.Resource.Data;
using COC.Resource;
using System.Globalization;

namespace COC.Application
{
    public partial class COC_SCR_013 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_013));
        private string ss_rankingid = "rankingid";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "Ranking Management";
            Page.Form.DefaultButton = btnSave.UniqueID;
            //AppUtil.SetIntTextBox(txtEmpCode);
            //AppUtil.SetIntTextBox(txtMarketingCode);
            //AppUtil.SetIntTextBox(txtTellNo);
            //txtEmpCode.Attributes.Add("OnBlur", "ChkIntOnBlurClear(this)");
            //txtMarketingCode.Attributes.Add("OnBlur", "ChkIntOnBlurClear(this)");
            //txtTellNo.Attributes.Add("OnBlur", "ChkIntOnBlurClear(this)");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "Ranking");
                    //ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_102");
                    //if (priData == null || priData.IsView != 1)
                    //{
                    //    AppUtil.ClientAlertAndRedirect(Page, "คุณไม่มีสิทธิ์เข้าใช้หน้าจอนี้", "COC_SCR_102.aspx");
                    //    return;
                    //}

                    InitialControl();
                    //SetDept();
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
            AppUtil.SetIntTextBox(txtSkip);
            if (Request.QueryString["rankingid"] == null)
            {
                divEdit.Visible = true;
                trAdd.Visible = false;
                trSkip.Visible = false;
                btnDeleteAll.Visible = false;
            }
            else
            {

                divEdit.Visible = true;
                trAdd.Visible = false;

                DoSearchRanking(Request.QueryString["rankingid"]);

                if (RankingBiz.isLastRankingData(Request.QueryString["rankingid"]))
                {
                    upResultCampaign.Visible = false;
                    upResultDealer.Visible = false;
                }

                DoSearchCampaign(0);

                DoSearchDealer(0);
            }

        }

        private void DoSearchRanking(string rankingId)
        {

            RankingData ranking = RankingBiz.SearchRankingData(rankingId);

            hidRankingId.Value = ranking.coc_RankingId.ToString();
            txtSeq.Text = ranking.coc_Seq.ToString();

            if (ranking.coc_Name != null)
            {
                txtName.Text = ranking.coc_Name.ToString();
            }

            txtSkip.Text = ranking.coc_SkipDate.ToString();


            if (ranking.coc_IsAllDealer == null || ranking.coc_IsAllDealer == "" || ranking.coc_IsAllDealer == "N")
            {
                chkAllDealer.Checked = false;
            }
            else
            {
                chkAllDealer.Checked = true;
            }

            if (ranking.coc_Seq == 1)
            {
                trSkip.Visible = true;
                divEdit.Visible = false;
                trAdd.Visible = true;
            }
            else
            {
                trSkip.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    RankingData data = new RankingData();
                    data.coc_Name = txtName.Text;

                    if (hidRankingId.Value == "")
                    {
                        int rankingId = RankingBiz.AddRanking(data, (List<RankingCampaignData>)ViewState["Campaign"], (List<RankingDealerData>)ViewState["Dealer"], HttpContext.Current.User.Identity.Name);

                        Session[ss_rankingid] = rankingId;
                    }
                    else
                    {
                        RankingBiz.EditRanking(data, null, null, HttpContext.Current.User.Identity.Name);
                    }
                    AppUtil.ClientAlertAndRedirect(Page, "บันทึกข้อมูล Ranking สำเร็จ", "COC_SCR_101.aspx");
                }
                else
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุข้อมูลให้ครบถ้วน");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateData())
                {
                    RankingData data = new RankingData();

                    data.coc_RankingId = AppUtil.SafeInt(hidRankingId.Value);
                    data.coc_Name = txtName.Text;
                    data.coc_SkipDate = AppUtil.SafeInt(txtSkip.Text);
                    if (chkAllDealer.Checked)
                    {
                        
                        data.coc_IsAllDealer = "Y";
                    }
                    else
                    {
                        data.coc_IsAllDealer = "N";
                    }

                    if (hidRankingId.Value == "")
                    {
                        int rankingId = RankingBiz.AddRanking(data, (List<RankingCampaignData>)ViewState["Campaign"], (List<RankingDealerData>)ViewState["Dealer"], HttpContext.Current.User.Identity.Name);

                        Session[ss_rankingid] = rankingId;
                    }
                    else
                    {
                        RankingBiz.EditRanking(data, (List<RankingCampaignData>)ViewState["Campaign"], (List<RankingDealerData>)ViewState["Dealer"], HttpContext.Current.User.Identity.Name);

                    }
                    AppUtil.ClientAlertAndRedirect(Page, "บันทึกข้อมูล Ranking สำเร็จ", "COC_SCR_101.aspx");
                }
                else
                {
                    AppUtil.ClientAlert(Page, "กรุณาระบุข้อมูลให้ครบถ้วน");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (RankingBiz.CheckDeleteRanking(hidRankingId.Value))
                {
                    RankingBiz.DeleteRanking(AppUtil.SafeInt(hidRankingId.Value),HttpContext.Current.User.Identity.Name);

                    AppUtil.ClientAlertAndRedirect(Page, "บันทึกข้อมูล Ranking สำเร็จ", "COC_SCR_101.aspx");
                }
                else
                {
                    AppUtil.ClientAlert(Page, "ข้อมูลที่เลือกไม่สามารถลบได้");
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnCloseAll_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("COC_SCR_101.aspx");
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        private bool ValidateData()
        {
            int i = 0;
            //************************************Windows Username********************************************
            if (txtName.Text.Trim() == "")
            {
                vtxtName.Text = "กรุณาระบุชื่อลำดับที่";
                vtxtName.ForeColor = System.Drawing.Color.Red;
                i += 1;
            }
            else
            {
                vtxtName.Text = "";
                //if (SlmScr019Biz.CheckUsernameExist(txtUserName.Text.Trim(), null))
                if (RankingBiz.CheckNameExist(txtName.Text.Trim(), AppUtil.SafeInt(hidRankingId.Value)))
                {
                    vtxtName.Text = "ชื่อลำดับที่นี้มีอยู่แล้วในระบบแล้ว";
                    vtxtName.ForeColor = System.Drawing.Color.Red;
                    i += 1;
                }
                else
                    vtxtName.Text = "";
            }



            if (i > 0)
                return false;
            else
                return true;
        }

        protected void btnAddCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCampaignCode.DataSource = RankingCampaignBiz.GetCampaignMasterList();  //C=Customer
                cmbCampaignCode.DataTextField = "slm_CampaignCode";
                cmbCampaignCode.DataValueField = "slm_CampaignCode";
                cmbCampaignCode.DataBind();
                cmbCampaignCode.Items.Insert(0, new ListItem("", ""));

                cmbCampaignName.DataSource = RankingCampaignBiz.GetCampaignMasterList();  //C=Customer
                cmbCampaignName.DataTextField = "slm_CampaignName";
                cmbCampaignName.DataValueField = "slm_CampaignCode";
                cmbCampaignName.DataBind();
                cmbCampaignName.Items.Insert(0, new ListItem("", ""));
                mpePopupAddCampaign.Show();
                upPopupAddCampaign.Update();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void imbEditCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCampaignCode.DataSource = RankingCampaignBiz.GetCampaignMasterList();  //C=Customer
                cmbCampaignCode.DataTextField = "slm_CampaignCode";
                cmbCampaignCode.DataValueField = "slm_CampaignCode";
                cmbCampaignCode.DataBind();
                cmbCampaignCode.Items.Insert(0, new ListItem("", ""));

                cmbCampaignName.DataSource = RankingCampaignBiz.GetCampaignMasterList();  //C=Customer
                cmbCampaignName.DataTextField = "slm_CampaignName";
                cmbCampaignName.DataValueField = "slm_CampaignCode";
                cmbCampaignName.DataBind();
                cmbCampaignName.Items.Insert(0, new ListItem("", ""));

                decimal RankingCampaignId = decimal.Parse(((ImageButton)sender).CommandArgument);

                List<RankingCampaignData> rcs = (List<RankingCampaignData>)ViewState["Campaign"];
                RankingCampaignData rc = rcs.Where(r => r.coc_RankingCampaignId == RankingCampaignId).FirstOrDefault();

                cmbCampaignCode.SelectedValue = rc.coc_CampaignCode;
                cmbCampaignName.SelectedValue = rc.coc_CampaignCode;

                mpePopupAddCampaign.Show();
                upPopupAddCampaign.Update();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void imbDeleteCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                decimal RankingCampaignId = decimal.Parse(((ImageButton)sender).CommandArgument);
                //new RankingCampaignBiz().DeleteData(RankingCampaignId);
                List<RankingCampaignData> rcs = (List<RankingCampaignData>)ViewState["Campaign"];
                RankingCampaignData rc = rcs.Where(r => r.coc_RankingCampaignId == RankingCampaignId).FirstOrDefault();
                rcs.Remove(rc);
                DoSearchCampaign(0);
                AppUtil.ClientAlert(Page, "ลบข้อมูลเรียบร้อย");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        #region Popup Add Config Campaign

        private void ClearPopupAddCampaign()
        {
            lblAlertCampaignCode.Text = "";
            lblAlertCampaignName.Text = "";
            //lblAlertAddAssignType_Cus.Text = "";

            cmbCampaignCode.Items.Clear();
            cmbCampaignName.Items.Clear();
        }

        protected void cmbCampaignCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCampaignCode.SelectedItem.Value != "")
                {
                    cmbCampaignName.SelectedValue = cmbCampaignCode.SelectedItem.Value;
                }
                mpePopupAddCampaign.Show();
                upPopupAddCampaign.Update();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void cmbCampaignName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCampaignName.SelectedItem.Value != "")
                {
                    cmbCampaignCode.SelectedValue = cmbCampaignName.SelectedItem.Value;
                }
                mpePopupAddCampaign.Show();
                upPopupAddCampaign.Update();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnCancelPopupAddCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPopupAddCampaign();
                mpePopupAddCampaign.Hide();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnSavePopupAddCampaign_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidatePopupAddCampaign())
                {
                    //RankingCampaignBiz biz = new RankingCampaignBiz();
                    //if (biz.ValidateData(AppUtil.SafeInt(hidRankingId.Value), cmbCampaignCode.SelectedItem.Value))
                    //{
                    //biz.InsertData(AppUtil.SafeInt(hidRankingId.Value), cmbCampaignCode.SelectedItem.Value, cmbCampaignName.SelectedItem.Value, HttpContext.Current.User.Identity.Name.ToLower());


                    RankingCampaignData rc = new RankingCampaignData();
                    rc.coc_RankingId = AppUtil.SafeInt(hidRankingId.Value);
                    rc.coc_CampaignCode = cmbCampaignCode.SelectedItem.Text;
                    rc.coc_CampaignName = cmbCampaignName.SelectedItem.Text;

                    List<RankingCampaignData> RankingCampaigns;

                    if (ViewState["Campaign"] != null)
                    {
                        RankingCampaigns = (List<RankingCampaignData>)ViewState["Campaign"];

                    }
                    else
                    {
                        RankingCampaigns = new List<RankingCampaignData>();
                    }

                    //if (ValidatePopupAddCampaign())
                    //{
                    RankingCampaigns.Add(rc);
                    //}
                    ViewState["Campaign"] = RankingCampaigns;
                    ClearPopupAddCampaign();
                    mpePopupAddCampaign.Hide();

                    DoSearchCampaign(0);
                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลเรียบร้อย");
                    //}
                    //else
                    //{
                    //    AppUtil.ClientAlert(Page, biz.ErrorMessage);
                    //    mpePopupAddCampaign.Show();
                    //}
                }
                else
                {
                    mpePopupAddCampaign.Show();
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private bool ValidatePopupAddCampaign()
        {
            if (cmbCampaignCode.SelectedItem == null || cmbCampaignName.SelectedItem == null || cmbCampaignCode.SelectedItem.Value == "" || cmbCampaignName.SelectedItem.Value == "")
            {
                AppUtil.ClientAlert(Page, "กรุณาระบุข้อมูลให้ครบถ้วน ก่อนการบันทึก");
                return false;
            }

            if (ViewState["Campaign"] != null)
            {

                List<RankingCampaignData> rcs = (List<RankingCampaignData>)ViewState["Campaign"];

                if (rcs.Where(r => r.coc_CampaignCode == cmbCampaignCode.SelectedItem.Value).Count() > 0)
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถบันทึกข้อมูลได้ เนื่องจากข้อมูลซ้ำกับในระบบ");
                    return false;
                }
            }

            return true;
        }

        private void DoSearchCampaign(int pageIndex)
        {
            try
            {
                //RankingCampaignBiz biz = new RankingCampaignBiz();

                if (ViewState["Campaign"] == null)
                {
                    List<RankingCampaignData> list = RankingCampaignBiz.GetRankingCampaignList(hidRankingId.Value.Trim());
                    ViewState["Campaign"] = list;
                }
                BindGridview_gvAddCampaign(((List<RankingCampaignData>)ViewState["Campaign"]).ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindGridview_gvAddCampaign(object[] items, int pageIndex = 0)
        {
            gvAddCampaign.DataSource = items;
            gvAddCampaign.DataBind();
            upResultCampaign.Update();
        }

        #endregion

        protected void chkAllDealer_CheckedChanged(object sender, EventArgs e)
        {

            if (((CheckBox) sender).Checked)
            {
         
                //const string DevConfirm = "if (!confirm('Confirm changing developer status.')) return false;";
                //chkAllDealer.Attributes.Add("onclick", DevConfirm);
                //AppUtil.ClientAlert(Page, "บันทึกข้อมูลเรียบร้อย");
                //chkAllDealer.Attributes.Add("onclick","if(!confirm('คุณต้องการลบ All Dealer ใช่หรือไม่'))return false;");
               
                btnAddDealer.Enabled = false;
                ViewState["Dealer"] = null;
                DoSearchDealer(0);
            }
            else
            {
                btnAddDealer.Enabled = true;
            }
        }

        protected void btnAddDealer_Click(object sender, EventArgs e)
        {
            try
            {
                mpePopupAddDealer.Show();
                upPopupAddDealer.Update();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void imbEditDealer_Click(object sender, EventArgs e)
        {
            try
            {
                txtDealerCode.Text = "";
                txtDealerName.Text = "";


                decimal RankingDealerId = decimal.Parse(((ImageButton)sender).CommandArgument);

                List<RankingDealerData> rds = (List<RankingDealerData>)ViewState["Dealer"];
                RankingDealerData rd = rds.Where(r => r.coc_RankingDealerId == RankingDealerId).FirstOrDefault();

                txtDealerCode.Text = rd.coc_DealerCode;
                txtDealerName.Text = rd.coc_DealerName;

                mpePopupAddDealer.Show();
                upPopupAddDealer.Update();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void imbDeleteDealer_Click(object sender, EventArgs e)
        {
            try
            {
                decimal rankingDealerId = decimal.Parse(((ImageButton)sender).CommandArgument);
                List<RankingDealerData> rds = (List<RankingDealerData>)ViewState["Dealer"];
                RankingDealerData rd = rds.Where(r => r.coc_RankingDealerId == rankingDealerId).FirstOrDefault();
                rds.Remove(rd);

                DoSearchDealer(0);
                AppUtil.ClientAlert(Page, "ลบข้อมูลเรียบร้อย");
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("COC_SCR_101.aspx");
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(Page, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }


        #region Popup Add Config Dealer

        private void ClearPopupAddDealer()
        {
            lblAlertDealerCode.Text = "";
            lblAlertDealerName.Text = "";
            //lblAlertAddAssignType_Cus.Text = "";

            txtDealerCode.Text = "";
            txtDealerName.Text = "";
        }


        protected void btnCancelPopupAddDealer_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPopupAddDealer();
                mpePopupAddDealer.Hide();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        protected void btnSavePopupAddDealer_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidatePopupAddDealer())
                {
                    RankingDealerBiz biz = new RankingDealerBiz();
                    //if (biz.ValidateData(AppUtil.SafeInt(hidRankingId.Value), txtDealerCode.Text))
                    //{
                    //biz.InsertData(AppUtil.SafeInt(hidRankingId.Value), txtDealerCode.Text, txtDealerName.Text, HttpContext.Current.User.Identity.Name.ToLower());


                    RankingDealerData rd = new RankingDealerData();
                    rd.coc_RankingId = AppUtil.SafeInt(hidRankingId.Value);
                    rd.coc_DealerCode = txtDealerCode.Text;
                    rd.coc_DealerName = txtDealerName.Text;

                    List<RankingDealerData> RankingDealers;

                    if (ViewState["Dealer"] != null)
                    {
                        RankingDealers = (List<RankingDealerData>)ViewState["Dealer"];

                    }
                    else
                    {
                        RankingDealers = new List<RankingDealerData>();
                    }

                    //if (ValidatePopupAddCampaign())
                    //{
                    RankingDealers.Add(rd);
                    //}
                    ViewState["Dealer"] = RankingDealers;
                    ClearPopupAddDealer();
                    mpePopupAddDealer.Hide();

                    DoSearchDealer(0);
                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลเรียบร้อย");
                    //}
                    //else
                    //{
                    //    AppUtil.ClientAlert(Page, biz.ErrorMessage);
                    //    mpePopupAddDealer.Show();
                    //}
                }
                else
                {
                    mpePopupAddDealer.Show();
                }
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private bool ValidatePopupAddDealer()
        {
            if (txtDealerCode.Text == "" || txtDealerName.Text == "")
            {
                AppUtil.ClientAlert(Page, "กรุณาระบุข้อมูลให้ครบถ้วน ก่อนการบันทึก");
                return false;
            }

            if (ViewState["Dealer"] != null)
            {

                List<RankingDealerData> rds = (List<RankingDealerData>)ViewState["Dealer"];

                if (rds.Where(r => r.coc_DealerCode == txtDealerCode.Text).Count() > 0)
                {
                    AppUtil.ClientAlert(Page, "ไม่สามารถบันทึกข้อมูลได้ เนื่องจากข้อมูลซ้ำกับในระบบ");
                    return false;
                }
            }

            return true;
        }

        private void DoSearchDealer(int pageIndex)
        {
            try
            {

                if (ViewState["Dealer"] == null)
                {
                    List<RankingDealerData> list = RankingDealerBiz.GetRankingDealerList(hidRankingId.Value.Trim());
                    ViewState["Dealer"] = list;
                }
                BindGridview_gvAddDealer(((List<RankingDealerData>)ViewState["Dealer"]).ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindGridview_gvAddDealer(object[] items, int pageIndex = 0)
        {
            gvAddDealer.DataSource = items;
            gvAddDealer.DataBind();
            upResultDealer.Update();
        }

        #endregion
    }
}