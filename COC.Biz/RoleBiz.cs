using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class RoleBiz
    {
        public static ScreenPrivilegeData GetScreenPrivilege(string username, string screenDesc)
        {
            return KKSlmMsValidateDal.GetScreenPrivilege(username, screenDesc);
        }

        public static bool CheckTicketIdPrivilege(string ticketId, string username, string recursiveList, string teamRecursiveList, string staffTypeId)
        {
            SearchLeadCondition condition = new SearchLeadCondition();
            condition.TicketId = ticketId;

            List<SearchLeadData> list = SearchLeadDal.SearchLeadData(condition, username, recursiveList, teamRecursiveList, staffTypeId);
            return list.Count > 0 ? true : false;
        }
    }
}
