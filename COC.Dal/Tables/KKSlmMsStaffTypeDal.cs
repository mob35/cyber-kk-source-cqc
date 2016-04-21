using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKSlmMsStaffTypeDal
    {
        public static List<ControlListData> GetStaffTyeList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            decimal[] dec = { COCConstant.StaffType.ITAdministrator };
            return slmdb.kkslm_ms_staff_type.Where(p => p.is_Deleted == 0 && dec.Contains(p.slm_StaffTypeId) == false).OrderBy(p => p.slm_StaffTypeDesc).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_StaffTypeDesc, ValueField = p.slm_StaffTypeId.ToString() }).ToList();
        }
    }
}
