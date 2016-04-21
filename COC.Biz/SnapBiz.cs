using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Dal.Tables;

namespace COC.Biz
{
    public class SnapBiz
    {
        public static List<AppInfoData> GetSnapWaitDetailList(DateTime assignDateFrom, DateTime assignDateTo, string teamList)
        {
            return KKCocTrSnapWaitAppDetailDal.GetSnapWaitDetailList(assignDateFrom, assignDateTo, teamList);
        }

        public static List<UserMonitoringSnapData> GetSnapUserMonitoringList(DateTime assignDateFrom, DateTime assignDateTo, string teamList)
        {
            return KKCocTrSnapMonitoringDetailDal.GetSnapUserMonitoringList(assignDateFrom, assignDateTo, teamList);
        }
    }
}
