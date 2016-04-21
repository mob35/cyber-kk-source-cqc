using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKCocMsAolDal
    {
        public static string GetPrivilegeNCB(string productId, decimal staffTypeId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            var privilegeNCB = slmdb.kkcoc_ms_aol.Where(p => p.is_Deleted == 0 && p.coc_Product_Id == productId && p.coc_StaffTypeId == staffTypeId).Select(p => p.coc_Privilege_NBC).FirstOrDefault();
            return privilegeNCB != null ? privilegeNCB : "";
        }
    }
}
