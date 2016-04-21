using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource;
using COC.Resource.Data;


namespace COC.Dal.Tables
{
    public class KKCocWsLogDal
    {
        private static string SLMDBName = COCConstant.SLMDBName;

        public static List<MonitoringWSData> GetWSLog(DateTime datefrom,DateTime dateto,string wsname,string status)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT coc_OperationDate AS OperationDate
                                  ,coc_Operation AS WSName
                                  ,coc_ChannelId AS Channel
                                  ,coc_TicketId AS TicketId
                                  ,coc_ResponseCode AS ResponseCode
                                  ,coc_ResponseDesc AS ResponseDesc
                                  ,coc_ResponseDate AS ResponseDate
                                  ,coc_ResponseTime AS ResponseTime
                                  ,coc_CauseError AS CauseError
                            FROM " + SLMDBName + @".dbo.kkcoc_ws_log ";


            string whr = "";
            string whrDateFrom = datefrom.Year != 1 ? datefrom.Year + datefrom.ToString("-MM-dd") : string.Empty;
            string whrDateTo = dateto.Year != 1 ? dateto.Year + dateto.ToString("-MM-dd") : string.Empty;

            whr += (whrDateFrom == "" || whrDateTo == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, coc_OperationDate) Between '" + whrDateFrom + "' and '" + whrDateTo + "' ");
            
            if(wsname != "")
                whr += (string.IsNullOrEmpty(wsname) ? "" : (whr == "" ? "" : " AND ") + " UPPER(coc_Operation) = '" + wsname.ToUpper() + "' ");

            if(status == "SUCCESS")
                whr += (whr == "" ? "" : " AND ") + " UPPER(coc_ResponseDesc) = '" + status.ToUpper() + "' ";
            else if (status == "NOTSUCCESS")
                whr += (whr == "" ? "" : " AND ") + " UPPER(coc_ResponseDesc) <> 'SUCCESS' ";

            sql += (whr == "" ? "" : " WHERE " + whr);
            sql += " ORDER BY coc_OperationDate DESC";

            return slmdb.ExecuteStoreQuery<MonitoringWSData>(sql).ToList();
        }
    }
}
