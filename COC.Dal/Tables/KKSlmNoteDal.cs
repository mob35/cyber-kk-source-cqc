using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using COC.Resource;
using COC.Resource.Data;
using System.IO;

namespace COC.Dal.Tables
{
    public class KKSlmNoteDal
    {
        private static string SLMDBName = COCConstant.SLMDBName;

        public static void InsertNoteHistory(string ticketId, bool sendEmail, string emailSubject, string noteDetail, List<string> emailList, string createBy)
        {
            try
            {
                SLMDBEntities slmdb = new SLMDBEntities();

                kkcoc_prepare_email email = null;
                string emailTemplate = "";
                DateTime createDate = DateTime.Now;

                kkslm_note note = new kkslm_note();
                note.slm_TicketId = ticketId;
                note.slm_NoteDetail = noteDetail;
                note.slm_CreateBy = createBy;
                note.slm_CreatedBy_Position = KKSlmMsStaffDal.GetPositionId(createBy);
                note.slm_CreateDate = createDate;
                note.slm_SendEmailFlag = sendEmail;

                if (sendEmail)
                {
                    note.slm_EmailSubject = emailSubject;
                    OwnerDelegateEmailData data = GetOwnerOrDelegateEmail(slmdb, ticketId);
                    if (data != null)
                    {
                        if (data.Owner != null && !string.IsNullOrEmpty(data.OwnerEmail))
                        {
                            emailTemplate = GetEmailTemplate(slmdb, ticketId, noteDetail);
                            email = new kkcoc_prepare_email();
                            email.coc_EmailAddress = data.OwnerEmail;
                            email.coc_EmailContent = emailTemplate;
                            email.coc_EmailSubject = emailSubject;
                            email.coc_EmailSender = createBy;
                            email.coc_ticketId = ticketId;
                            email.coc_ExportStatus = "0";
                            email.is_Deleted = 0;
                            email.coc_CreatedBy = createBy;
                            email.coc_CreatedDate = createDate;
                            slmdb.kkcoc_prepare_email.AddObject(email);
                        }
                        if (data.Delegate != null && !string.IsNullOrEmpty(data.DelegateEmail))
                        {
                            email = new kkcoc_prepare_email();
                            email.coc_EmailAddress = data.DelegateEmail;
                            if (emailTemplate == "")
                                emailTemplate = GetEmailTemplate(slmdb, ticketId, noteDetail);
                            email.coc_EmailContent = emailTemplate;
                            email.coc_EmailSubject = emailSubject;
                            email.coc_EmailSender = createBy;
                            email.coc_ticketId = ticketId;
                            email.coc_ExportStatus = "0";
                            email.is_Deleted = 0;
                            email.coc_CreatedBy = createBy;
                            email.coc_CreatedDate = createDate;
                            slmdb.kkcoc_prepare_email.AddObject(email);
                        }
                        if (data.MarketingOwner != null && !string.IsNullOrEmpty(data.MarketingOwnerEmail))
                        {
                            email = new kkcoc_prepare_email();
                            email.coc_EmailAddress = data.MarketingOwnerEmail;
                            if (emailTemplate == "")
                                emailTemplate = GetEmailTemplate(slmdb, ticketId, noteDetail);
                            email.coc_EmailContent = emailTemplate;
                            email.coc_EmailSubject = emailSubject;
                            email.coc_EmailSender = createBy;
                            email.coc_ticketId = ticketId;
                            email.coc_ExportStatus = "0";
                            email.is_Deleted = 0;
                            email.coc_CreatedBy = createBy;
                            email.coc_CreatedDate = createDate;
                            slmdb.kkcoc_prepare_email.AddObject(email);
                        }
                        if (data.LastOwner != null && !string.IsNullOrEmpty(data.LastOwnerEmail))
                        {
                            string flag = ConfigurationManager.AppSettings["SendEmailLastOwner"];
                            if (flag == null || flag.Trim().ToUpper() == "Y")
                            {
                                email = new kkcoc_prepare_email();
                                email.coc_EmailAddress = data.LastOwnerEmail;
                                if (emailTemplate == "")
                                    emailTemplate = GetEmailTemplate(slmdb, ticketId, noteDetail);
                                email.coc_EmailContent = emailTemplate;
                                email.coc_EmailSubject = emailSubject;
                                email.coc_EmailSender = createBy;
                                email.coc_ticketId = ticketId;
                                email.coc_ExportStatus = "0";
                                email.is_Deleted = 0;
                                email.coc_CreatedBy = createBy;
                                email.coc_CreatedDate = createDate;
                                slmdb.kkcoc_prepare_email.AddObject(email);
                            }
                        }
                    }

                    if (emailList != null)
                    {
                        foreach (string email_address in emailList)
                        {
                            kkcoc_prepare_email prepare_eamil = new kkcoc_prepare_email();
                            prepare_eamil.coc_EmailAddress = email_address;
                            if (emailTemplate == "")
                                emailTemplate = GetEmailTemplate(slmdb, ticketId, noteDetail);
                            prepare_eamil.coc_EmailContent = emailTemplate;
                            prepare_eamil.coc_EmailSubject = emailSubject;
                            prepare_eamil.coc_EmailSender = createBy;
                            prepare_eamil.coc_ticketId = ticketId;
                            prepare_eamil.coc_ExportStatus = "0";
                            prepare_eamil.is_Deleted = 0;
                            prepare_eamil.coc_CreatedBy = createBy;
                            prepare_eamil.coc_CreatedDate = createDate;
                            slmdb.kkcoc_prepare_email.AddObject(prepare_eamil);
                        }
                    }
                }

                slmdb.kkslm_note.AddObject(note);

                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();
                if (lead != null)
                {
                    lead.slm_NoteFlag = "1";
                    lead.slm_UpdatedBy = createBy;
                    lead.slm_UpdatedDate = createDate;
                }

                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static OwnerDelegateEmailData GetOwnerOrDelegateEmail(SLMDBEntities slmdb, string ticketId)
        {
            try
            {
                string sql = @"SELECT LEAD.slm_Owner AS Owner, STAFF.slm_StaffEmail AS OwnerEmail, LEAD.slm_Delegate AS Delegate, STAFF2.slm_StaffEmail AS DelegateEmail
                                , STAFF3.slm_UserName AS MarketingOwner, STAFF3.slm_StaffEmail AS MarketingOwnerEmail
                                , STAFF4.slm_UserName AS LastOwner, STAFF4.slm_StaffEmail AS LastOwnerEmail
                                FROM " + SLMDBName + @".dbo.kkslm_tr_lead LEAD
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff STAFF ON LEAD.slm_Owner = STAFF.slm_UserName AND STAFF.is_Deleted = 0
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff STAFF2 ON LEAD.slm_Delegate = STAFF2.slm_UserName AND STAFF2.is_Deleted = 0
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff STAFF3 ON LEAD.coc_MarketingOwner = STAFF3.slm_EmpCode AND STAFF3.is_Deleted = 0
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff STAFF4 ON LEAD.coc_LastOwner = STAFF4.slm_EmpCode AND STAFF4.is_Deleted = 0
                                WHERE LEAD.slm_ticketId = '" + ticketId + "' AND LEAD.is_Deleted = 0";

                return slmdb.ExecuteStoreQuery<OwnerDelegateEmailData>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetEmailTemplate(SLMDBEntities slmdb, string ticketId, string noteDetail)
        {
            try
            {
                string template = "";
                string filePath = System.Configuration.ConfigurationManager.AppSettings["EmailTemplatePath"];
                if (filePath == null) { throw new Exception("ไม่พบ Config EmailTemplatePath ใน Configuration File"); }

                string sql = @"SELECT lead.slm_ticketId AS TicketId, cam.slm_CampaignName AS CampaignName, lead.slm_ChannelId AS Channel, lead.slm_Name AS Firstname, lead.slm_LastName AS Lastname, own.slm_StaffNameTH AS OwnerName
                                , lead.slm_AssignedDate AS AssignedDate, del.slm_StaffNameTH AS DelegateName, lead.slm_DelegateDate AS DelegateDate, ISNULL(cre.slm_StaffNameTH, lead.slm_CreatedBy) AS CreatedBy, lead.slm_CreatedDate AS CreatedDate
                                ,lead.slm_AvailableTime AS AvailableTime, opt.slm_OptionDesc AS StatusDesc, lead.slm_Product_Name AS ProductName, pg.product_name AS ProductGroupName, lead.slm_TelNo_1 AS TelNo1, prod.slm_LicenseNo AS LicenseNo
                                ,opt2.slm_OptionDesc AS CocStatusDesc, mktowner.slm_StaffNameTH AS MarketingOwnerName, lastowner.slm_StaffNameTH AS LastOwnerName
                                FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign cam ON cam.slm_CampaignId = lead.slm_CampaignId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff own ON own.slm_UserName = lead.slm_Owner
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff del ON del.slm_UserName = lead.slm_Delegate
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff cre ON cre.slm_UserName = lead.slm_CreatedBy
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON opt.slm_OptionCode = lead.slm_Status AND opt.slm_OptionType = 'lead status'
                                LEFT JOIN " + SLMDBName + @".dbo.CMT_MS_PRODUCT_GROUP pg ON pg.product_id = lead.slm_Product_Group_Id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prod ON prod.slm_TicketId = lead.slm_ticketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt2 ON opt2.slm_OptionCode = lead.coc_Status AND ISNULL(opt2.slm_OptionSubCode, '0123456789') = ISNULL(lead.coc_SubStatus, '0123456789') AND opt2.slm_OptionType = 'coc_status'
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON mktowner.slm_EmpCode = lead.coc_MarketingOwner
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lastowner.slm_EmpCode = lead.coc_LastOwner
                                WHERE lead.slm_ticketId = '" + ticketId + "'";

                var data = slmdb.ExecuteStoreQuery<EmailTemplateData>(sql).FirstOrDefault();
                if (data != null)
                {
                    template = File.ReadAllText(filePath);
                    template = template.Replace("%Note%", noteDetail)
                                        .Replace("%TicketId%", ticketId)
                                        .Replace("%Campaign%", data.CampaignName)
                                        .Replace("%Channel%", data.Channel)
                                        .Replace("%ProductGroupName%", data.ProductGroupName)
                                        .Replace("%ProductName%", data.ProductName)
                                        .Replace("%LeadStatus%", data.StatusDesc)
                                        .Replace("%CustomerName%", data.Firstname)
                                        .Replace("%CustomerLastName%", data.Lastname)
                                        .Replace("%OwnerName%", data.OwnerName)
                                        .Replace("%AssignedDate%", data.AssignedDate != null ? data.AssignedDate.Value.Year.ToString() + data.AssignedDate.Value.ToString("-MM-dd HH:mm:ss") : "")
                                        .Replace("%DelegateName%", data.DelegateName)
                                        .Replace("%DelegateDate%", data.DelegateDate != null ? data.DelegateDate.Value.Year.ToString() + data.DelegateDate.Value.ToString("-MM-dd HH:mm:ss") : "")
                                        .Replace("%CreatedBy%", data.CreatedBy)
                                        .Replace("%CreatedDate%", data.CreatedDate != null ? data.CreatedDate.Value.Year.ToString() + data.CreatedDate.Value.ToString("-MM-dd HH:mm:ss") : "")
                                        .Replace("%TelNo1%", data.TelNo1)
                                        .Replace("%LicenseNo%", data.LicenseNo)
                                        .Replace("%CocStatusDesc%", data.CocStatusDesc)
                                        .Replace("%MarketingOwnerName%", data.MarketingOwnerName)
                                        .Replace("%LastOwnerName%", data.LastOwnerName);

                    if (data.AvailableTime != null && data.AvailableTime.Length == 6)
                        template = template.Replace("%AvailableTime%", data.AvailableTime.Substring(0, 2) + ":" + data.AvailableTime.Substring(2, 2) + ":" + data.AvailableTime.Substring(4, 2));
                    else
                        template = template.Replace("%AvailableTime%", "");
                }

                return template != "" ? template : noteDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
