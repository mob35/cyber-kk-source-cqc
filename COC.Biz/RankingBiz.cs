using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class RankingBiz
    {

        public static List<SearchRankingResult> SearchRankingData()
        {
            KKCocTrRankingDal search = new KKCocTrRankingDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.SearchRankingData();
        }

        public static RankingData SearchRankingData(string rankingId)
        {
            KKCocTrRankingDal search = new KKCocTrRankingDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.SearchRankingData(rankingId);
        }

        public static void UpdateSeq(List<RankingData> rankingdatas, string username)
        {
            KKCocTrRankingDal search = new KKCocTrRankingDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            search.UpdateSeq(rankingdatas, username);
        }


        public static int AddRanking(RankingData ranking, List<RankingCampaignData> rankingcampaigns, List<RankingDealerData> rankingdealers, string username)
        {
            KKCocTrRankingDal search = new KKCocTrRankingDal();
            return search.AddRanking(ranking, rankingcampaigns, rankingdealers, username);
        }

        public static void EditRanking(RankingData ranking, List<RankingCampaignData> rankingCampaigns, List<RankingDealerData> rankingDealers, string username)
        {
            KKCocTrRankingDal search = new KKCocTrRankingDal();
            search.EditRanking(ranking, rankingCampaigns, rankingDealers, username);
        }

        public static bool CheckDeleteRanking(string rankingID)
        {

            KKCocTrRankingDal search = new KKCocTrRankingDal();
            RankingData rankingdata = search.SearchRankingData(rankingID);
            if (rankingdata.coc_Seq == 1 || !search.isLastRankingData(rankingdata.coc_Seq))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isLastRankingData(string rankingID)
        {

            KKCocTrRankingDal search = new KKCocTrRankingDal();
            RankingData rankingdata = search.SearchRankingData(rankingID);
            if (search.isLastRankingData(rankingdata.coc_Seq))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DeleteRanking(int? rankingID, string username) {
            KKCocTrRankingDal search = new KKCocTrRankingDal();
            search.DeleteRanking(rankingID, username);
        }

        public static bool CheckNameExist(string Name, int? rankingId)
        {
            //
            KKCocTrRankingDal search = new KKCocTrRankingDal();

            return search.CheckNameRanking(Name, rankingId);

            
        }
    }
}
