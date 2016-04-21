using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKSlmScreenDAL
    {
        public static List<ControlListData> GetScreenList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_screen.Where(p => p.slm_System == "COC").OrderBy(p => p.slm_Description).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_Description, ValueField = p.slm_ScreenId.ToString() }).ToList();
        }

        public static List<UserRoleMatrixData> GetUserRoleMatrixList(string screenId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT   CONVERT(varchar, ROW_NUMBER() OVER(ORDER BY slm_StaffTypeDesc ASC)) AS CNT,
		                            MT.slm_StaffTypeDesc AS StaffTypeName ,CONVERT(varchar, Z.is_View) AS isView 
		                            ,convert(varchar,Z.slm_ScreenId) AS ScreenId
		                            ,convert(varchar,Z.slm_ValidateId) AS ValidateId
		                            ,convert(varchar,MT.slm_StaffTypeId) AS StaffTypeId
                            FROM kkslm_ms_staff_type MT LEFT JOIN 
	                            (SELECT	MV.slm_StaffTypeId,MV.is_View,mv.slm_ScreenId ,sc.slm_Description ,MV.slm_ValidateId 
	                             FROM	kkslm_ms_validate MV INNER JOIN kkslm_screen SC ON SC.slm_ScreenId = MV.slm_ScreenId 
	                             WHERE	SC.slm_System = 'COC' AND SC.slm_ScreenId = '" + screenId + @"') AS Z ON Z.slm_StaffTypeId = MT.slm_StaffTypeId 
	                             ORDER BY MT.slm_StaffTypeDesc ";
            return slmdb.ExecuteStoreQuery<UserRoleMatrixData>(sql).ToList();
        }
    }
}
