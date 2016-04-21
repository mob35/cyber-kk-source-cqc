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
    public class RankingDealerBiz
    {
        public static bool ValidateData(int? rankingId, string DealerCode)
        {
            KKCocTrRankingDealerDal search = new KKCocTrRankingDealerDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.ValidateData(rankingId, DealerCode);
        }
        public static void InsertData(int? RankingId, string DealerCode, string DealerName, string createByUsername)
        {
            KKCocTrRankingDealerDal search = new KKCocTrRankingDealerDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            search.InsertData(RankingId, DealerCode, DealerName, createByUsername);
        }
        public static List<RankingDealerData> GetRankingDealerList(string rankingId)
        {
            KKCocTrRankingDealerDal search = new KKCocTrRankingDealerDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.GetRankingDealerList(rankingId);

        }
        public static List<RankingDealerData> SearchRankingDealer(string rankingDealerId)
        {
            KKCocTrRankingDealerDal search = new KKCocTrRankingDealerDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            return search.SearchRankingDealer(rankingDealerId);
        }
        public static void DeleteData(decimal rankingDealerId)
        {
            KKCocTrRankingDealerDal search = new KKCocTrRankingDealerDal();
            //string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            //string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;

            search.DeleteData(rankingDealerId);
        }
    }
}
