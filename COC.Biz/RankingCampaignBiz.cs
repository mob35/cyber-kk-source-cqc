using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal;
using COC.Resource.Data;
using COC.Resource;
using COC.Dal.Tables;

namespace COC.Biz
{
    public class RankingCampaignBiz
    {
        public static bool ValidateData(int? rankingId, string CampaignCode)
        {
            KKCocTrRankingCampaignDal search = new KKCocTrRankingCampaignDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.ValidateData(rankingId, CampaignCode);
        }


        public static void InsertData(int? RankingId, string campaignCode, string campaignName, string createByUsername)
        {
            KKCocTrRankingCampaignDal search = new KKCocTrRankingCampaignDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            search.InsertData(RankingId, campaignCode, campaignName, createByUsername);
        }

        public static List<RankingCampaignData> GetRankingCampaignList(string rankingId)
        {
            KKCocTrRankingCampaignDal search = new KKCocTrRankingCampaignDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.GetRankingCampaignList(rankingId);
        }

        public static List<CampaignMasterData> GetCampaignMasterList()
        {
            KKCocTrRankingCampaignDal search = new KKCocTrRankingCampaignDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.GetCampaignMasterList();
        }
        public static List<RankingCampaignData> SearchRankingCampaign(string rankingId)
        {
            KKCocTrRankingCampaignDal search = new KKCocTrRankingCampaignDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.SearchRankingCampaign(rankingId);
        }

        public static void DeleteData(decimal rankingCampaignId)
        {
            KKCocTrRankingCampaignDal search = new KKCocTrRankingCampaignDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            search.DeleteData(rankingCampaignId);
        }
    }
}
