using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKCocMsTeamDal
    {
        public static List<AppInfoData> GetTeamList(bool isViewMonitoring)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkcoc_ms_team.Where(p => p.is_Deleted == 0 && p.is_ViewMonitoring == isViewMonitoring).OrderBy(p => p.coc_Seq).Select(p => new AppInfoData 
            { 
                Team = p.coc_TeamId,
                AppInPoolNewJob = 0,
                AppInPoolOldJob = 0,
                AppInPoolAllJob = 0,
                AppWaitAssignNewJob = 0,
                AppWaitAssignOldJob = 0,
                AppWaitAssignAllJob = 0,
                AppAssignedNewJob = 0,
                AppAssignedOldJob = 0,
                AppAssignedAllJob = 0,
                AppTotalAllJob = 0
            }).ToList();

        }

        public static string GetSubStatusWaitingList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string subStatusList = "";

            var teamList = slmdb.kkcoc_ms_team.Where(p => p.is_Deleted == 0 && p.is_ViewMonitoring == true).OrderBy(p => p.coc_Seq).ToList();
            foreach (kkcoc_ms_team team in teamList)
            {
                if (!string.IsNullOrEmpty(team.coc_SubStatusWaiting))
                {
                    string[] substatus = team.coc_SubStatusWaiting.Split(',');
                    foreach (string str in substatus)
                    {
                        subStatusList += (subStatusList != "" ? "," : "") + "'" + str + "'";
                    }
                }
            }

            return subStatusList;
        }

        public static string GetSubStatusAssignedList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string subStatusList = "";

            var teamList = slmdb.kkcoc_ms_team.Where(p => p.is_Deleted == 0 && p.is_ViewMonitoring == true).OrderBy(p => p.coc_Seq).ToList();
            foreach (kkcoc_ms_team team in teamList)
            {
                if (!string.IsNullOrEmpty(team.coc_SubStatusReceive))
                {
                    string[] substatus = team.coc_SubStatusReceive.Split(',');
                    foreach (string str in substatus)
                    {
                        subStatusList += (subStatusList != "" ? "," : "") + "'" + str + "'";
                    }
                }
            }

            return subStatusList;
        }
    }
}
