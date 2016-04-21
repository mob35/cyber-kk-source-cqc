using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace COC.Application.Utilities
{
    public static class AppConstant
    {
        public static class SessionName
        {
            public const string COC_Searchcondition = "coc_searchcondition";
            public const string COC_StaffSearchcondition = "coc_staffsearchcondition";
        }

        public static int TextMaxLength
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["TextMaxLength"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["TextMaxLength"]) : 4000;
                }
                catch
                {
                    return 4000;
                }
            }
        }

        public static int SnapReportDateRange
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SnapReportDateRange"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["SnapReportDateRange"]) : 5;
                }
                catch
                {
                    return 5;
                }
            }
        }

        public static class Campaign
        {
            public static int DisplayCampaignDescMaxLength
            {
                get
                {
                    try
                    {
                        return ConfigurationManager.AppSettings["DisplayCampaignDescMaxLength"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["DisplayCampaignDescMaxLength"]) : 100;
                    }
                    catch { return 100; }
                }
            }
        }

        public static class OptionType
        {
            public const string LeadStatus = "lead status";
            public const string CocStatus = "coc_status";
        }
    }
}