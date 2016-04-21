using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Resource;

namespace COC.Dal.Tables
{
    public class KKSlmMsPositionDal
    {
        /// <summary>
        /// GetPositionList Flag 1=Active Branch, 2=Inactive Branch, 3=All
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static List<ControlListData> GetPositionList(int flag)
        {
            List<ControlListData> list = null;
            SLMDBEntities slmdb = new SLMDBEntities();

            if (flag == COCConstant.Position.Active)
                list = slmdb.kkslm_ms_position.Where(p => p.is_Deleted == false).OrderBy(p => p.slm_PositionNameTH).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_PositionNameTH, ValueField = p.slm_Position_id.ToString() }).ToList();
            else if (flag == COCConstant.Position.InActive)
                list = slmdb.kkslm_ms_position.Where(p => p.is_Deleted == true).OrderBy(p => p.slm_PositionNameTH).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_PositionNameTH, ValueField = p.slm_Position_id.ToString() }).ToList();
            else if (flag == COCConstant.Position.All)
                list = slmdb.kkslm_ms_position.OrderBy(p => p.slm_PositionNameTH).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_PositionNameTH, ValueField = p.slm_Position_id.ToString() }).ToList();
            else
                list = new List<ControlListData>();

            return list;
        }
    }
}
