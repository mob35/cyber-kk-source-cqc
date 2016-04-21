using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Dal.Tables;

namespace COC.Biz
{
    public class OptionBiz
    {
        public static List<ControlListData> GetOptionList(string optionType)
        {
            return KKSlmMsOptionDal.GetOptionList(optionType);
        }

        public static List<ControlListData> GetCocStatusList(string optionType)
        {
            return KKSlmMsOptionDal.GetCocStatusList(optionType);
        }

        public static List<ControlListData> GetCocSubStatusList(string optionType, string cocStatusCode)
        {
            return KKSlmMsOptionDal.GetCocSubStatusList(optionType, cocStatusCode);
        }

        public static string GetWorkingEndTime()
        {
            return KKSlmMsOptionDal.GetWorkingEndTime();
        }

        public static void UpdateWorkingEndTime(string workingEndTime)
        {
            KKSlmMsOptionDal.UpdateWorkingEndTime(workingEndTime);
        }
    }
}
