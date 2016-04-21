using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Biz;
using COC.Application.Utilities;
using COC.Resource.Data;
using log4net;

namespace COC.Application
{
    public partial class COC_SCR_010 : System.Web.UI.Page
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(COC_SCR_010));

        protected override void OnInit(EventArgs e)
        {
            //pnAdvanceSearch.Style["display"] = "block";
            base.OnInit(e);
            ((Label)Page.Master.FindControl("lblTopic")).Text = "User Role Matrix";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ScreenPrivilegeData priData = RoleBiz.GetScreenPrivilege(HttpContext.Current.User.Identity.Name, "COC_SCR_010");
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
            cmbScreen.DataSource = UserRoleMatrixBiz.GetScreenList();
            cmbScreen.DataTextField = "TextField";
            cmbScreen.DataValueField = "ValueField";
            cmbScreen.DataBind();
            cmbScreen.Items.Insert(0, new ListItem("-------------เลือก-------------", ""));

            imgResult.Visible = false;
            btnSave.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                 SearchValidateData();
                
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _log.Debug(message);
                AppUtil.ClientAlert(Page, message);
            }
        }

        private void SearchValidateData()
        {
            if (cmbScreen.SelectedItem.Value == "")
            {
                gvResult.DataSource = null;
                gvResult.DataBind();
                imgResult.Visible = false;
                btnSave.Visible = false;
                upResult.Update();
            }
            else
            {
                txtScreen.Text = cmbScreen.SelectedItem.Value;
                List<UserRoleMatrixData> uData = UserRoleMatrixBiz.SearchUserRoleMatrix(cmbScreen.SelectedItem.Value);
                gvResult.DataSource = uData;
                gvResult.DataBind();
                imgResult.Visible = true;
                btnSave.Visible = true;
                upResult.Update();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool ret = true;
                List<UserRoleMatrixData> dataInsert = new List<UserRoleMatrixData>();
                if (gvResult.Rows.Count > 0)
                {
                    for (int i = 0; i < gvResult.Rows.Count; i++)
                    {
                        RadioButton rdHavePrivilege = (RadioButton)gvResult.Rows[i].FindControl("rdHavePrivilege");
                        RadioButton rdNoPrivilege = (RadioButton)gvResult.Rows[i].FindControl("rdNoPrivilege");
                        Label lblValidateId = (Label)gvResult.Rows[i].FindControl("lblValidateId");
                        Label lblScreenId = (Label)gvResult.Rows[i].FindControl("lblScreenId");
                        Label lblStaffTypeId = (Label)gvResult.Rows[i].FindControl("lblStaffTypeId");

                        if (rdHavePrivilege != null && rdNoPrivilege != null)
                        {
                            if (rdHavePrivilege.Checked == true || rdNoPrivilege.Checked == true)//กรณีมีการ check มีสิทธิหรือไม่มีสิทธิ
                            {
                                UserRoleMatrixData udata = new UserRoleMatrixData();
                                if (lblValidateId != null && !string.IsNullOrEmpty(lblValidateId.Text.Trim()))
                                    udata.ValidateId = lblValidateId.Text.Trim();
                                if (lblStaffTypeId != null && !string.IsNullOrEmpty(lblStaffTypeId.Text.Trim()))
                                    udata.StaffTypeId = lblStaffTypeId.Text.Trim();

                                if (rdHavePrivilege.Checked == true)
                                    udata.isView = "1";
                                else if (rdNoPrivilege.Checked == true)
                                    udata.isView = "0";

                                udata.ScreenId = txtScreen.Text;

                                dataInsert.Add(udata);
                            }
                        }
                    }
                }
                ret = UserRoleMatrixBiz.InsertOrUpdateValidateData(dataInsert, HttpContext.Current.User.Identity.Name);
                if (ret)
                {
                    AppUtil.ClientAlert(Page, "บันทึกข้อมูลสำเร็จ");
                    SearchValidateData();
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