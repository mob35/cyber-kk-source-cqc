using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class ChannelBiz
    {
        public static List<ControlListData> GetChannelList()
        {
            return KKSlmMsChannelDal.GetChannelList();
        }
    }
}
