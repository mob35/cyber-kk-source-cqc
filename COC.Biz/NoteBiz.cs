using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal;
using COC.Dal.Tables;

namespace COC.Biz
{
    public class NoteBiz
    {
        public static void InsertNoteHistory(string ticketId, bool sendEmail, string emailSubject, string noteDetail, List<string> emailList, string createBy)
        {
            KKSlmNoteDal.InsertNoteHistory(ticketId, sendEmail, emailSubject, noteDetail, emailList, createBy);
        }
    }
}
