using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKCocMsWSDal
    {
        public static List<ControlListData> GetWebserviceList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkcoc_ms_ws.Where(p => p.coc_WebserviceSystem == "COC").OrderBy(p => p.coc_Seq).Select(p => new ControlListData { TextField = p.coc_WebserviceName, ValueField = p.coc_WebserviceName }).ToList();
        }
    }
}
