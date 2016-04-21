using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Resource;

namespace COC.Dal.Tables
{
    public class KKSlmMsOptionDal
    {
        public static List<ControlListData> GetOptionList(string optionType)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_option.Where(p => p.slm_OptionType == optionType && p.is_Deleted == 0 && (p.slm_SystemView == null || p.slm_SystemView == "COC")).OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_OptionDesc, ValueField = p.slm_OptionCode }).ToList();
        }

        public static List<ControlListData> GetCocStatusList(string optionType)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_option.Where(p => p.slm_OptionType == optionType && p.is_Deleted == 0 && p.slm_OptionSubCode == null).OrderBy(p => p.slm_OptionCode).Select(p => new ControlListData { TextField = p.slm_OptionDesc, ValueField = p.slm_OptionCode }).Distinct().ToList();
        }

        public static List<ControlListData> GetCocSubStatusList(string optionType, string cocStatusCode)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_option.Where(p => p.slm_OptionType == optionType && p.is_Deleted == 0 && p.slm_OptionCode == cocStatusCode && p.slm_OptionSubCode != null).OrderBy(p => p.slm_Seq).Select(p => new ControlListData { TextField = p.slm_OptionDesc, ValueField = p.slm_OptionSubCode }).ToList();
        }

        public static string GetWorkingEndTime()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_option.Where(p => p.slm_OptionCode == "endWorkingTime" && p.is_Deleted == 0).Select(p => p.slm_OptionDesc).FirstOrDefault();
        }

        public static void UpdateWorkingEndTime(string workingEndTime)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            var option = slmdb.kkslm_ms_option.Where(p => p.slm_OptionCode == "endWorkingTime" && p.is_Deleted == 0).FirstOrDefault();
            if (option != null)
            {
                option.slm_OptionDesc = workingEndTime;
                slmdb.SaveChanges();
            }
            else
                throw new Exception("Update Fail ไม่พบ OptionCode = endWorkingTime");
        }
    }
}
