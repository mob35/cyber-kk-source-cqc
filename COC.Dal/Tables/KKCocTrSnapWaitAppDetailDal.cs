using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Resource;

namespace COC.Dal.Tables
{
    public class KKCocTrSnapWaitAppDetailDal
    {
        private static string SLMDBName = COCConstant.SLMDBName;

        public static List<AppInfoData> GetSnapWaitDetailList(DateTime assignDateFrom, DateTime assignDateTo, string teamList)
        {
            string datefrom = assignDateFrom.Year.ToString() + assignDateFrom.ToString("-MM-dd");
            string dateto = assignDateTo.Year.ToString() + assignDateTo.ToString("-MM-dd");

            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT coc_Team AS Team, SUM(coc_Pool_New_Job) AS AppInPoolNewJob, SUM(coc_Pool_Old_Job) AS AppInPoolOldJob, SUM(coc_Pool_Total) AS AppInPoolAllJob
                            , SUM(coc_Wait_Receive_New_Job) AS AppWaitAssignNewJob, SUM(coc_Wait_Receive_Old_Job) AS AppWaitAssignOldJob, SUM(coc_Wait_Receive_Total) AS AppWaitAssignAllJob
                            , SUM(coc_Receive_New_Job) AS AppAssignedNewJob, SUM(coc_Receive_Old_Job) AS AppAssignedOldJob, SUM(coc_Receive_Total) AS AppAssignedAllJob
                            , SUM(coc_WaitApp_Total) AS AppTotalAllJob
                            FROM " + SLMDBName + @".dbo.kkcoc_tr_snap_waitapp_detail
                            WHERE CONVERT(DATE, coc_Date) BETWEEN '" + datefrom + "' AND '" + dateto + @"'
                            AND coc_Team IN (" + teamList + @")
                            GROUP BY coc_Team 
                            ORDER BY coc_Team ";

            return slmdb.ExecuteStoreQuery<AppInfoData>(sql).ToList();

        }
    }
}
