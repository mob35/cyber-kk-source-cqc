using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class MonitoringWSBiz
    {
        public static List<ControlListData> GetWebserviceList()
        {
            return KKCocMsWSDal.GetWebserviceList();
        }

        public static List<MonitoringWSData> GetWSLog(DateTime datefrom, DateTime dateto, string wsname, string status)
        {
            return KKCocWsLogDal.GetWSLog(datefrom, dateto, wsname, status);
        }
    }
}
