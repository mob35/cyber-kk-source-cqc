using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COC.Resource.Data
{
    public class CampaignWSData
    {
        public decimal? CampaignFinalId { get; set; }
        public string CampaignId { get; set; }
        public string CampaignCode { get; set; }
        public string CampaignName { get; set; }
        public string CampaignDetail { get; set; }
        public string ChannelName { get; set; }
        public string OfferName { get; set; }
        public string OfferDate { get; set; }
        public string Interest { get; set; }
        public string TicketId { get; set; }
        public string Action { get; set; }
        public string ActionName { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal GroupID { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string StartDate { get; set; }
        public string HasOffered { get; set; }
        public string IsInterested { get; set; }
        public string UpdatedBy { get; set; }
        public string BranchName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CampaignDetailCut { get; set; }
    }
}
