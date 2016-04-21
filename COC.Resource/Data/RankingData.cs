using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace COC.Resource.Data
{
    [Serializable]
    public class RankingData
    {
        public int coc_RankingId { get; set; }
        public int coc_Seq { get; set; }
        public string coc_Name { get; set; }
        public Nullable<int> coc_SkipDate { get; set; }
        public string coc_IsAllDealer { get; set; }

        public int? Is_Delete { get; set; }
        public string coc_UpdatedBy { get; set; }
        public DateTime? coc_UpdatedDate { get; set; }
        public string coc_CreatedBy { get; set; }
        public DateTime? coc_CreatedDate { get; set; }

    }
    [Serializable]
    public class RankingCampaignData
    {
        public int coc_RankingCampaignId { get; set; }
        public int coc_RankingId { get; set; }
        public string coc_CampaignCode { get; set; }
        public string coc_CampaignName { get; set; }

        public int? Is_Delete { get; set; }
        public string coc_UpdatedBy { get; set; }
        public DateTime? coc_UpdatedDate { get; set; }
        public string coc_CreatedBy { get; set; }
        public DateTime? coc_CreatedDate { get; set; }

    }
    [Serializable]
    public class RankingDealerData
    {
        public int coc_RankingDealerId { get; set; }
        public int coc_RankingId { get; set; }
        public string coc_DealerCode { get; set; }
        public string coc_DealerName { get; set; }

        public int? Is_Delete { get; set; }
        public string coc_UpdatedBy { get; set; }
        public DateTime? coc_UpdatedDate { get; set; }
        public string coc_CreatedBy { get; set; }
        public DateTime? coc_CreatedDate { get; set; }

    }
    //[Serializable]
    //public class CampaignMasterData
    //{
    //    public string coc_CampaignCode { get; set; }
    //    public string coc_CampaignName { get; set; }
    //}

    public class SearchRankingResult
    {
        public int coc_RankingId { get; set; }
        public int? coc_Seq { get; set; }

        public string coc_Name { get; set; }
        public Nullable<int> coc_SkipDate { get; set; }
        public string coc_UpdatedBy { get; set; }
        public DateTime? coc_UpdatedDate { get; set; }
        public string coc_CreatedBy { get; set; }
        public DateTime? coc_CreatedDate { get; set; }
    }

    public class SearchRankingCampaignResult
    {
        public int coc_RankingCampaignId { get; set; }
        public int coc_RankingId { get; set; }
        public string coc_CampaignCode { get; set; }
        public string coc_CampaignName { get; set; }

        public string Is_Delete { get; set; }
        public string coc_UpdatedBy { get; set; }
        public DateTime? coc_UpdatedDate { get; set; }
        public string coc_CreatedBy { get; set; }
        public DateTime? coc_CreatedDate { get; set; }
    }

    public class SearchRankingDealerResult
    {
        public int coc_RankingDealerId { get; set; }
        public int coc_RankingId { get; set; }
        public string coc_DealerCode { get; set; }
        public string coc_DealerName { get; set; }

        public string Is_Delete { get; set; }
        public string coc_UpdatedBy { get; set; }
        public DateTime? coc_UpdatedDate { get; set; }
        public string coc_CreatedBy { get; set; }
        public DateTime? coc_CreatedDate { get; set; }
    }
}
