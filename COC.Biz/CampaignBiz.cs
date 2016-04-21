using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class CampaignBiz
    {
        public static List<ControlListData> GetCampaignList(bool onlyMass)
        {
            return KKSlmMsCampaignDal.GetCampaignList(onlyMass);
        }

        public static CampaignWSData GetCampaignDesc(string campaignId)
        {
            return KKSlmMsCampaignDal.GetCampaignDesc(campaignId);
        }
    }
}
