using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COC.Resource.Data
{
    public class SearchLeadCondition
    {
        public string TicketId { get; set; }
        public string CitizenId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string StatusList { get; set; }
        public string CampaignId { get; set; }
        public string ChannelId { get; set; }
        public string OwnerUsername { get; set; }       //Owner Lead
        public DateTime CreatedDate { get; set; }
        public DateTime AssignedDate { get; set; }
        public int PageIndex { get; set; }
        public decimal? StaffType { get; set; }
        public string OwnerBranch { get; set; }         //Owner Branch
        public string DelegateBranch { get; set; }      //Delegate Branch
        public string DelegateLead { get; set; }        //Delegate Lead
        public string CreateByBranch { get; set; }      //CreateBy Branch
        public string CreateBy { get; set; }            //CreateBy Lead

        //COC
        public DateTime CocAssignedDate { get; set; }
        public string MarketingOwner { get; set; }
        public string LastOwner { get; set; }
        public string CocTeam { get; set; }
        public string CocStatus { get; set; }
        public string CocSubStatus { get; set; }

        public string SortExpression { get; set; }
        public string SortDirection { get; set; }
    }
}
