using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Dal.Tables;

namespace COC.Biz
{
    public class DepartmentBiz
    {
        public static List<ControlListData> GetDepartmentList()
        {
            return KKSlmMsdepartmentDal.GetDepartmentList();
        }
    }
}
