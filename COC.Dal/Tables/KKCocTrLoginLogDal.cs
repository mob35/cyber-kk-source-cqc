using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COC.Dal.Tables
{
    public class KKCocTrLoginLogDal
    {
        public static void InsertLoginLog(string domainName, string username, string clientIP, bool loginResult, string errorDetail)
        {
            try
            {
                SLMDBEntities slmdb = new SLMDBEntities();
                kkcoc_tr_login_log log = new kkcoc_tr_login_log();
                log.coc_Domain = domainName;
                log.coc_Username = username;
                log.coc_ClientIP = clientIP;
                log.coc_LoginResult = loginResult ? "SUCCESS" : "FAIL";
                if (!loginResult) log.coc_LoginErrorDetail = errorDetail;
                log.coc_LoginDate = DateTime.Now;

                slmdb.kkcoc_tr_login_log.AddObject(log);
                slmdb.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
