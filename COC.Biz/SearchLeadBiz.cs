using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class SearchLeadBiz
    {
        public static List<SearchLeadData> SearchLeadData(SearchLeadCondition data, string username, string recursiveList, string teamRecursiveList, string staffTypeId)
        {
            return SearchLeadDal.SearchLeadData(data, username, recursiveList, teamRecursiveList, staffTypeId);
        }

        public static LeadData GetLeadInfo(string ticketId)
        {
            return SearchLeadDal.GetLeadInfo(ticketId);
        }

        public static List<CampaignWSData> GetCampaignFinalList(string ticketId)
        {
            return SearchLeadDal.GetCampaignFinalList(ticketId);
        }

        public static List<SearchLeadData> SearchExistingLead(string citizenId, string telNo1)
        {
            return SearchLeadDal.SearchExistingLead(citizenId, telNo1);
        }

        public static List<ExistingProductData> SearchExistingProduct(string citizenId)
        {
            return SearchLeadDal.SearchExistingProduct(citizenId);
        }

        public static List<OwnerLoggingData> SearchOwnerLogging(string ticketId)
        {
            return SearchLeadDal.SearchOwnerLogging(ticketId);
        }

        public static List<PhoneCallHistoryData> SearchPhoneCallHistory(string citizenId, string telNo1)
        {
            return SearchLeadDal.SearchPhoneCallHistory(citizenId, telNo1);
        }

        public static List<NoteHistoryData> SearchNoteHistory(string ticketId)
        {
            return SearchLeadDal.SearchNoteHistory(ticketId);
        }

        public static string GetNumOfUnassignLead(string username)
        {
            return SearchLeadDal.GetNumOfUnassignLead(username);
        }

        public static LeadDataForAdam GetLeadDataForAdam(string ticketId)
        {
            SearchLeadDal search = new SearchLeadDal();
            return search.GetLeadDataForAdam(ticketId);
        }
    }
}
