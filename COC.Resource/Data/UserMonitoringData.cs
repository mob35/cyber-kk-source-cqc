using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COC.Resource.Data
{
    public class UserMonitoringData
    {
        public string Team { get; set; }
        public string StaffFullname { get; set; }
        public decimal? Active { get; set; }
        public int? AmountNewJobNew { get; set; }
        public int? AmountNewJobOnHand { get; set; }
        public int? AmountNewJobAll { get; set; }
        public int? AmountDoneJobForward { get; set; }
        public int? AmountDoneJobRouteBackCoc { get; set; }
        public int? AmountDoneJobRouteBackMkt { get; set; }
        public int? AmountDoneJobAll { get; set; }
        public int? AmountAllJob { get; set; }
        public string EmpCode { get; set; }
    }

    public class UserMonitoringSnapData : UserMonitoringData
    {
        public decimal? WorkingMin { get; set; }
        public int? WorkingSec { get; set; }
        public string WorkingMinDisplay { get; set; }
        public string AvgSuccessPerHour { get; set; }
        public string AvgTotalPerHour { get; set; }
    }

    public class AppInfoData
    {
        public string Team { get; set; }
        public int? AppInPoolNewJob { get; set; }
        public int? AppInPoolOldJob { get; set; }
        public int? AppInPoolAllJob { get; set; }
        public int? AppWaitAssignNewJob { get; set; }
        public int? AppWaitAssignOldJob { get; set; }
        public int? AppWaitAssignAllJob { get; set; }
        public int? AppAssignedNewJob { get; set; }
        public int? AppAssignedOldJob { get; set; }
        public int? AppAssignedAllJob { get; set; }
        public int? AppTotalAllJob { get; set; }
    }

    public class TicketIdByTeamData
    {
        public string Team { get; set; }
        public string TicketId { get; set; }
        public Int64? FlowLogId { get; set; }
    }

    public class AmountByTeamData
    {
        public string Team { get; set; }
        public int? Amount { get; set; }
    }

    public class LeadDataPopupMonitoring
    {
        public string No { get; set; }
        public string TicketId { get; set; }
        public decimal? CocCounting { get; set; }
        public string AppNo { get; set; }
        public string JobType { get; set; }
        public string CocAssignTypeDesc { get; set; }
        public string MarketingOwner { get; set; }
        public string MarketingOwnerName { get; set; }
        public string CocStatusDesc { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string Channel { get; set; }
        public string CarType { get; set; }
        public string OwnerBranchName { get; set; }
        public string ClientFirstname { get; set; }
        public string ClientLastname { get; set; }
        public string TelNo1 { get; set; }
        public string CardNo { get; set; }
        public string SlmOwnerName { get; set; }
        public string SlmDelegateName { get; set; }
        public string LastOwnerName { get; set; }
        public DateTime? LeadCreatedDate { get; set; }
        public DateTime? CocFirstAssignDate { get; set; }
        public DateTime? CocFirstTeamAssign { get; set; }
        public DateTime? CocAssignedDate { get; set; }
        public string Team { get; set; }
        public string AppAging { get; set; }
        public string LeadAging { get; set; }
        public string TeamAging { get; set; }
        public Int64? FlowLogId { get; set; }
        public string DoneJobType { get; set; }
        public DateTime? ActionDate { get; set; }
        public int? RankingId { get; set; }
        public string RankingName { get; set; }
        public string CampaignName { get; set; }
        
    }

    public class ForecastReportData
    {
        public string Team { get; set; }
        public int? AmountOfJob { get; set; }
        public int? Sla { get; set; }
        public int? AmountOfAvailableStaff { get; set; }
        public int? AmountOfPredictionSuccess { get; set; }
        public int? AmountOfJobExceedEndTime { get; set; }
        public int? AmountOfAdditionalTime { get; set; }
        public string AdditionalTimePerPerson { get; set; }
    }

    // UserMonitoring
    public class UserMonitoringNewJobData
    {
        public string TicketId { get; set; }
        public string Team { get; set; }
        public string LastOwner { get; set; }
        public Int64? FlowLogId { get; set; }
    }

    public class AmountByTeamAndLastOwnerData
    {
        public string Team { get; set; }
        public string LastOwner { get; set; }
        public int? Amount { get; set; }
    }
}
