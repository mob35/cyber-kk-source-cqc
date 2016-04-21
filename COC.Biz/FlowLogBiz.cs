using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class FlowLogBiz
    {
        public static List<FlowLogData> GetFlowLogList(DateTime action_date)
        {
            return KKCocTrFlowLogDal.GetFlowLogList(action_date);
        }

    }
}
