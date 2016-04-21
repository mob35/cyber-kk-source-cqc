using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Resource;

namespace COC.Dal.Tables
{
    public class KKCocTrFlowLogDal
    {
        private static string SLMDBName = COCConstant.SLMDBName;

        public static List<FlowLogData> GetFlowLogList(DateTime action_date)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string actionDate = action_date.Year.ToString() + action_date.ToString("-MM-dd");

            string sql = @"SELECT flowlog.coc_FlowLogId AS FlowLogId, flowlog.coc_TicketId AS TicketId, flowlog.coc_TeamFrom AS TeamFrom, flowlog.coc_TeamTo AS TeamTo
                                    , flowlog.coc_FlowType AS FlowType, flowlog.coc_ActionDate AS ActionDate, flowlog.coc_EmpCodeTeamFrom AS TeamFromEmpCode
                                    FROM " + SLMDBName + @".dbo.kkcoc_tr_flowlog flowlog
                                    INNER JOIN " + SLMDBName + @".dbo.kkslm_tr_lead lead ON lead.slm_ticketId = flowlog.coc_TicketId AND lead.is_Deleted = '0'
                                    WHERE CONVERT(DATE, flowlog.coc_ActionDate) = '" + actionDate + @"'
                                    ORDER BY flowlog.coc_TicketId, flowlog.coc_ActionDate ";

            return slmdb.ExecuteStoreQuery<FlowLogData>(sql).ToList();
        }

    }
}
