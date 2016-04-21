using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COC.Resource.Data
{
    public class SearchLeadData
    {
        public string No { get; set; }
        public string TicketId { get; set; }
        public string CitizenId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public decimal? Counting { get; set; }
        public string StatusDesc { get; set; }
        public string CampaignName { get; set; }
        public string ChannelDesc { get; set; }
        public string OwnerName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string NoteFlag { get; set; }
        public string OwnerBranchName { get; set; }
        public string DelegateBranchName { get; set; }
        public string DelegateName { get; set; }
        public string BranchCreateBranchName { get; set; }
        public string CreateName { get; set; }
        public int SEQ { get; set; }
        public string ProductGroupId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public bool? HasAdamUrl { get; set; }
        public string CampaignId { get; set; }
        public string LicenseNo { get; set; }
        public string TelNo1 { get; set; }
        public int? ProvinceRegis { get; set; }
        public string CalculatorUrl { get; set; }

        //COC
        public DateTime? CocAssignedDate { get; set; }
        public string MarketingOwnerName { get; set; }
        public string LastOwner { get; set; }
        public string LastOwnerName { get; set; }
        public string CocTeam { get; set; }
        public string CocStatusDesc { get; set; }
        public decimal? CocCounting { get; set; }
        public string FlowType { get; set; }
        public string AppNo { get; set; }
    }
}
