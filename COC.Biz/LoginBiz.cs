using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Dal.Tables;


namespace COC.Biz
{
    public class LoginBiz
    {
        public static void InsertLoginLog(string domainName, string username, string clientIP, bool loginResult, string errorDetail)
        {
            KKCocTrLoginLogDal.InsertLoginLog(domainName, username, clientIP, loginResult, errorDetail);
        }

        public static void InsertSession(string username, Guid sessionId)
        {
            KKCocTrSessionDal.InsertSession(username, sessionId);
        }

        public static bool VerifySession(string username, Guid sessionId)
        {
            return KKCocTrSessionDal.VerifySession(username, sessionId);
        }
    }
}
