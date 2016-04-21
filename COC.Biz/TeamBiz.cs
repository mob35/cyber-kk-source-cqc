using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class TeamBiz
    {
        public static List<AppInfoData> GetTeamList(bool isViewMonitoring)
        {
            return KKCocMsTeamDal.GetTeamList(isViewMonitoring);
        }

        public static string GetSubStatusWaitingList()
        {
            return KKCocMsTeamDal.GetSubStatusWaitingList();
        }
    }
}
