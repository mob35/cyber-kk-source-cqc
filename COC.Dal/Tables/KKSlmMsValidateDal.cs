using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKSlmMsValidateDal
    {
        private static string SLMDBName = COCConstant.SLMDBName;

        public static ScreenPrivilegeData GetScreenPrivilege(string username, string screenDesc)
        {
            SLMDBEntities slmdb = new SLMDBEntities();

            string sql = @"SELECT V.slm_StaffTypeId AS StaffTypeId, V.is_Save AS IsSave, V.is_View AS IsView, S.slm_ScreenDesc AS ScreenDesc 
                            FROM " + SLMDBName + @".dbo.kkslm_ms_validate V
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_screen S ON V.slm_ScreenId = S.slm_ScreenId
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON staff.slm_StaffTypeId = V.slm_StaffTypeId
                            WHERE V.is_Deleted = 0 AND S.is_Deleted = 0
                            AND staff.slm_UserName = '" + username + "' AND S.slm_ScreenDesc = '" + screenDesc + "'";

            return slmdb.ExecuteStoreQuery<ScreenPrivilegeData>(sql).FirstOrDefault();
        }

        public static bool InsertAndUpdateValidate(List<UserRoleMatrixData> data, string username)
        {
            try
            {
                SLMDBEntities slmdb = new SLMDBEntities();
                if (data.Count > 0)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        kkslm_ms_validate validate = new kkslm_ms_validate();
                        if (data[i].ValidateId == null) //Insert Validate
                        {
                            if(!string.IsNullOrEmpty(data[i].ScreenId))
                                validate.slm_ScreenId = Convert.ToInt16("0" + data[i].ScreenId);
                            if (!string.IsNullOrEmpty(data[i].StaffTypeId))
                            validate.slm_StaffTypeId = Convert.ToInt16("0" + data[i].StaffTypeId);
                            if (!string.IsNullOrEmpty(data[i].isView))
                                validate.is_View = Convert.ToInt16("0" + data[i].isView);
                            slmdb.kkslm_ms_validate.AddObject(validate);
                        }
                        else  //Update Validate
                        {
                            Int16 validateId = Convert.ToInt16("0" + data[i].ValidateId);
                            var validateData = slmdb.kkslm_ms_validate.Where(p => p.slm_ValidateId == validateId).FirstOrDefault();
                            if (validateData != null)
                            {
                                if (!string.IsNullOrEmpty(data[i].isView))
                                    validateData.is_View = Convert.ToInt16("0" + data[i].isView);
                            }
                        }
                    }
                    slmdb.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
