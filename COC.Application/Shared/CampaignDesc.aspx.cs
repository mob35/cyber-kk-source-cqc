using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using COC.Biz;
using COC.Resource.Data;
using COC.Application.Utilities;

namespace COC.Application.Shared
{
    public partial class CampaignDesc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request["campaignid"] != null)
                    {
                        CampaignWSData campaign = CampaignBiz.GetCampaignDesc(Request["campaignid"]);
                        if (campaign != null)
                        {
                            lblCampaignName.Text = campaign.CampaignName;
                            ltCampaignDesc.Text = campaign.CampaignDetail;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtil.ClientAlert(Page, ex.Message);
            }
        }
    }
}