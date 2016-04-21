using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class LeadBiz
    {
        public static bool CheckExistLeadOnHand(string username, string empCode)
        {
            return KKSlmTrLeadDal.CheckExistLeadOnHand(username, empCode);
        }

        public static bool CheckLastOwnerOnHand(string empCode)
        {
            return KKSlmTrLeadDal.CheckLastOwnerOnHand(empCode);
        }

        public static string GetLastOwnerName(string ticketId)
        {
            return KKSlmTrLeadDal.GetLastOwnerName(ticketId);
        }

        public static bool HasOwnerOrDelegate(string ticketId)
        {
            return KKSlmTrLeadDal.HasOwnerOrDelegate(ticketId);
        }

        public static void ChangeNoteFlag(string ticketId, bool noteFlag, string updateBy)
        {
            KKSlmTrLeadDal.ChangeNoteFlag(ticketId, noteFlag, updateBy);
        }

        #region AppInPool

        public static List<TicketIdByTeamData> GetNumberOfAppInPoolAllJob()
        {
            return KKSlmTrLeadDal.GetNumberOfAppInPoolAllJob();
        }

        public static List<LeadDataPopupMonitoring> GetNumberOfAppInPoolAllJobByTeam(string cocTeam)
        {
            return KKSlmTrLeadDal.GetNumberOfAppInPoolAllJobByTeam(cocTeam);
        }

        #endregion

        #region AppWaitAssign

        public static List<TicketIdByTeamData> GetNumberOfAppWaitAssignAllJob(string subStatusWaitingList)
        {
            return KKSlmTrLeadDal.GetNumberOfAppWaitAssignAllJob(subStatusWaitingList);
        }

        public static List<LeadDataPopupMonitoring> GetNumberOfAppWaitAssignAllJobByTeam(string cocTeam)
        {
            return KKSlmTrLeadDal.GetNumberOfAppWaitAssignAllJobByTeam(cocTeam);
        }

        #endregion

        #region AppAssigned

        public static List<TicketIdByTeamData> GetNumberOfAppAssignedAllJob()
        {
            return KKSlmTrLeadDal.GetNumberOfAppAssignedAllJob();
        }

        public static List<LeadDataPopupMonitoring> GetNumberOfAppAssignedAllJobByTeam(string cocTeam)
        {
            return KKSlmTrLeadDal.GetNumberOfAppAssignedAllJobByTeam(cocTeam);
        }

        #endregion

        #region UserMonotoring

        public static List<UserMonitoringNewJobData> GetUserMonitoringNewJobWaitAssignList(string available, string cocTeam, string teamList)
        {
            return KKSlmTrLeadDal.GetUserMonitoringNewJobWaitAssignList(available, cocTeam, teamList);
        }

        public static List<UserMonitoringNewJobData> GetUserMonitoringNewJobAssignedList(string available, string cocTeam, string teamList)
        {
            return KKSlmTrLeadDal.GetUserMonitoringNewJobAssignedList(available, cocTeam, teamList);
        }

        public static List<LeadDataPopupMonitoring> GetUserMonitoringNewJobListForPopup(string cocTeam, string lastOwner, string assignedFlag)
        {
            return KKSlmTrLeadDal.GetUserMonitoringNewJobListForPopup(cocTeam, lastOwner, assignedFlag);
        }

        public static List<LeadDataPopupMonitoring> GetUserMonitoringDoneJobListForPopup(DateTime action_date, string cocTeam, string lastOwner, string flowType, string routeBackTo)
        {
            return KKSlmTrLeadDal.GetUserMonitoringDoneJobListForPopup(action_date, cocTeam, lastOwner, flowType, routeBackTo);
        }

        #endregion

        public static void TransferJob(List<string> ticketIdList, string empCodeOfTransferee, string cocTeam, string updatedBy)
        {
            KKSlmTrLeadDal.TransferJob(ticketIdList, empCodeOfTransferee, cocTeam, updatedBy);
        }

        #region User Management

        public static List<LeadDataPopupMonitoring> GetJobOnHandList(string cocTeam, string lastOwner)
        {
            return KKSlmTrLeadDal.GetJobOnHandList(cocTeam, lastOwner);
        }

        public static bool CheckHeadStaff(string UserId, string HeadStaffId, out string desc)
        {
            if (HeadStaffId == "0")
            {
                desc = null;
                return true;
            }
            else
            {
                KKSlmTrLeadDal chann = new KKSlmTrLeadDal();
                return chann.CheckHeadStaff(UserId, HeadStaffId, out   desc);
            }
        }
        #endregion
    }
}
