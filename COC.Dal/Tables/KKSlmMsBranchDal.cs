using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKSlmMsBranchDal
    {
        public static List<ControlListData> GetBranchList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_branch.OrderBy(p => p.slm_BranchName).Select(p => new ControlListData { TextField = p.slm_BranchName, ValueField = p.slm_BranchCode }).ToList();
        }
    }
}
