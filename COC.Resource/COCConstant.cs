using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COC.Resource
{
    public class COCConstant
    {
        public static string SLMDBName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SLMDBName"].ToString();
                }
                catch
                {
                    return "SLMDB";
                }
            }
        }

        public static class StaffType
        {
            public const decimal Manager = 1;
            public const decimal Supervisor = 2;
            public const decimal UserAdministrator = 3;
            public const decimal Telesales = 4;
            public const decimal CallCenter = 5;
            public const decimal Leader = 6;
            public const decimal ITAdministrator = 7;
            public const decimal Marketing = 8;
            public const decimal ManagerOper = 10;
            public const decimal SupervisorOper = 11;
            public const decimal Oper = 12;
        }
        public static class StatusCode
        {
            public const string Interest = "00";        //สนใจ
            public const string NoContact = "01";       //ติดต่อไม่ได้
            public const string ContactDoc = "02";      //ติดต่อได้ รอเอกสาร
            public const string ContactCall = "03";     //ติดต่อได้ใ ห้โทรกลับ
            public const string FollowDoc = "04";       //ติดตามเอกสาร
            public const string WaitConsider = "05";    //รอผลการพิจารณา 
            public const string ApproveAccept = "06";   //อนุมัติ - ลูกค้าตกลง
            public const string ApproveEdit = "07";     //อนุมัติ - ส่งกลับแก้ไข
            public const string Reject = "08";          //Reject
            public const string Cancel = "09";          //Cancel
            public const string Close = "10";           //ปิดงาน
        }
        public static class CampaignType
        {
            public const string Mass = "01";
        }
        public static class ChannelId
        {
            public const string Branch = "BRANCH";
            public const string CallCenter = "CALLCENTER";
            public const string Telesales = "TELESALES";
        }
        public static class Position
        {
            public const int Active = 1;
            public const int InActive = 2;
            public const int All = 3;
        }
    }
}
