using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Dal.Tables;

namespace COC.Biz
{
    public class PositionBiz
    {
        public static List<ControlListData> GetPositionList(int flag)
        {
            return KKSlmMsPositionDal.GetPositionList(flag);
        }
    }
}
