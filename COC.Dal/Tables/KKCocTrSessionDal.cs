using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COC.Dal.Tables
{
    public class KKCocTrSessionDal
    {
        public static void InsertSession(string username, Guid sessionId)
        {
            try
            {
                SLMDBEntities slmdb = new SLMDBEntities();

                var session = slmdb.kkcoc_tr_session.Where(p => p.coc_UserName == username).FirstOrDefault();
                if (session == null)
                {
                    kkcoc_tr_session data = new kkcoc_tr_session()
                    {
                        coc_UserName = username,
                        coc_SessionGuid = sessionId,
                        coc_SessionCreatedDate = DateTime.Now
                    };
                    
                    slmdb.kkcoc_tr_session.AddObject(data);
                }
                else
                {
                    session.coc_SessionGuid = sessionId;
                    session.coc_SessionCreatedDate = DateTime.Now;
                }

                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool VerifySession(string username, Guid sessionId)
        {
            try
            {
                SLMDBEntities slmdb = new SLMDBEntities();
                var current_session = slmdb.kkcoc_tr_session.Where(p => p.coc_UserName == username && p.coc_SessionGuid == sessionId).FirstOrDefault();
                return current_session != null ? true : false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
