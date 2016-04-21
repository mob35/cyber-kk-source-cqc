using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKSlmMsdepartmentDal
    {
        public static List<ControlListData> GetDepartmentList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_department.Where(p => p.is_Deleted == 0).OrderBy(p => p.slm_DepartmentName).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_DepartmentName, ValueField = p.slm_DepartmentId.ToString() }).ToList();
        }
    }
}
