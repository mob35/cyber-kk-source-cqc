using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKSlmMsChannelDal
    {
        public static List<ControlListData> GetChannelList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_channel.Where(p => p.is_Deleted == 0).OrderBy(p => p.slm_ChannelDesc).Select(p => new ControlListData { TextField = p.slm_ChannelDesc, ValueField = p.slm_ChannelId }).ToList();
        }
    }
}
