using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKSlmMsCampaignDal
    {
        public static List<ControlListData> GetCampaignList(bool onlyMass)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            if (onlyMass)
                return slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && p.slm_Status == "A" && p.slm_CampaignType == COCConstant.CampaignType.Mass).OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_CampaignName, ValueField = p.slm_CampaignId }).ToList();
            else
                return slmdb.kkslm_ms_campaign.Where(p => p.is_Deleted == 0 && p.slm_Status == "A").OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_CampaignName, ValueField = p.slm_CampaignId }).ToList();
        }

        public static CampaignWSData GetCampaignDesc(string campaignId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_campaign.Where(p => p.slm_CampaignId == campaignId).Select(p => new CampaignWSData { CampaignName = p.slm_CampaignName, CampaignDetail = p.slm_Offer + " : " + p.slm_criteria }).FirstOrDefault();
        }
    }
}
