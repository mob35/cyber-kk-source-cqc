using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class ReportBiz
    {
        public static Int64 CheckReportWorkDetailExist(string datefrom, string dateto)
        {
            int countrows = ReportDal.Report_SP_RPT_WORKDETAIL_Exist(datefrom, dateto);
            return countrows;
        }

        public static Int64 CheckReportStaffWorkingExist(string datefrom, string dateto)
        {
            int countrows = ReportDal.Report_SP_RPT_STAFF_WORKING_Exist(datefrom, dateto);
            return countrows;
        }

        public static Int64 CheckReportSnapMonitoringExist(string datefrom, string dateto)
        {
            int countrows = ReportDal.Report_SP_RPT_SNAP_MONITORING_Exist(datefrom, dateto);
            return countrows;
        }
    }
}
