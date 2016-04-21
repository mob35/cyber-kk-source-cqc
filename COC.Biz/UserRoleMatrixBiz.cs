using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;
using COC.Resource.Data;

namespace COC.Biz
{
    public class UserRoleMatrixBiz
    {
        private string _Error = "";
        public string ErrorMessage
        {
            get { return _Error; }
        }

        private Dictionary<string, string> _errList = new Dictionary<string, string>();
        public Dictionary<string, string> ErrorList
        {
            get { return _errList; }
        } 

        public static List<ControlListData> GetScreenList()
        {
            return KKSlmScreenDAL.GetScreenList();
        }

        public static List<UserRoleMatrixData> SearchUserRoleMatrix(string screenid)
        {
            return KKSlmScreenDAL.GetUserRoleMatrixList(screenid);
        }

        public static bool InsertOrUpdateValidateData(List<UserRoleMatrixData> uData, string UserId)
        {
            try
            {
                bool result = true;
                return result = KKSlmMsValidateDal.InsertAndUpdateValidate(uData, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
