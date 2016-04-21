using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Resource;

namespace COC.Dal.Tables
{
    public class KKCocTrSnapMonitoringDetailDal
    {
        private static string SLMDBName = COCConstant.SLMDBName;

        public static List<UserMonitoringSnapData> GetSnapUserMonitoringList(DateTime assignDateFrom, DateTime assignDateTo, string teamList)
        {
            string datefrom = assignDateFrom.Year.ToString() + assignDateFrom.ToString("-MM-dd");
            string dateto = assignDateTo.Year.ToString() + assignDateTo.ToString("-MM-dd");

            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT coc_Team AS Team, coc_EmpName AS StaffFullname
                            , SUM(coc_OnHand_New_Job) AS AmountNewJobNew, SUM(coc_OnHand_Current) AS AmountNewJobOnHand, SUM(coc_OnHand_Total) AS AmountNewJobAll
                            , SUM(coc_Finish_Forward) AS AmountDoneJobForward, SUM(coc_Finish_RouteBack_COC) AS AmountDoneJobRouteBackCoc, SUM(coc_Finish_RouteBack_MKT) AS AmountDoneJobRouteBackMkt
                            , SUM(coc_Finish_Total) AS AmountDoneJobAll, SUM(coc_Total) AS AmountAllJob
                            , SUM(coc_Working_Sec) AS WorkingSec, '0.00' AS AvgSuccessPerHour, '0.00' AS AvgTotalPerHour
                            FROM " + SLMDBName + @".dbo.kkcoc_tr_snap_monitoring_detail
                            WHERE CONVERT(DATE, coc_Date) BETWEEN '" + datefrom + "' AND '" + dateto + @"'
                            AND coc_Team IN (" + teamList + @")
                            GROUP BY coc_Team, coc_EmpName 
                            ORDER BY coc_Team, coc_EmpName ";

            return slmdb.ExecuteStoreQuery<UserMonitoringSnapData>(sql).ToList();
        }
    }
}
