using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource.Data;
using COC.Resource;

namespace COC.Dal.Tables
{
    public class KKSlmTrLeadDal
    {
        private static string SLMDBName = COCConstant.SLMDBName;

        public static bool CheckExistLeadOnHand(string username, string empCode)
        {
            SLMDBEntities slmdb = new SLMDBEntities();

            string sql = @" SELECT  COUNT(LEAD.SLM_TICKETID) AS CNT
                            FROM    " + SLMDBName + @".DBO.kkslm_tr_lead LEAD 
                            WHERE LEAD.is_Deleted = 0 AND LEAD.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
                                    AND (LEAD.slm_Owner = '" + username + "' OR LEAD.slm_Delegate = '" + username + "' OR LEAD.coc_LastOwner = '" + empCode + "')";

            var result = slmdb.ExecuteStoreQuery<int>(sql).Select(p => p.ToString()).FirstOrDefault();
            if (result != null)
            {
                int cnt = int.Parse(result);
                if (cnt > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static bool CheckLastOwnerOnHand(string empCode)
        {
            SLMDBEntities slmdb = new SLMDBEntities();

            string sql = @" SELECT  COUNT(LEAD.SLM_TICKETID) AS CNT
                            FROM    " + SLMDBName + @".DBO.kkslm_tr_lead LEAD 
                            WHERE LEAD.is_Deleted = 0 AND LEAD.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
                                    AND (LEAD.coc_LastOwner = '" + empCode + "')";

            var result = slmdb.ExecuteStoreQuery<int>(sql).Select(p => p.ToString()).FirstOrDefault();
            if (result != null)
            {
                int cnt = int.Parse(result);
                if (cnt > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static string GetLastOwnerName(string ticketId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();

            var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();
            if (lead == null)
                throw new Exception("ไม่พบ Ticket Id " + ticketId + " ในระบบ");

            string sql = @"SELECT ISNULL(staff1.slm_StaffNameTH, LEAD.coc_LastOwner) AS LastOwnerName
                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead LEAD
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff1 ON LEAD.coc_LastOwner = staff1.slm_EmpCode
                            WHERE LEAD.slm_ticketId = '" + ticketId + "'";

            return slmdb.ExecuteStoreQuery<string>(sql).FirstOrDefault();
        }

        public static bool HasOwnerOrDelegate(string ticketId)
        {
            try
            {
                SLMDBEntities slmdb = new SLMDBEntities();

                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId && p.is_Deleted == 0).FirstOrDefault();
                if (lead != null)
                {
                    if (string.IsNullOrEmpty(lead.slm_Owner) && string.IsNullOrEmpty(lead.slm_Delegate))
                        return false;
                    else
                        return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ChangeNoteFlag(string ticketId, bool noteFlag, string updateBy)
        {
            try
            {
                SLMDBEntities slmdb = new SLMDBEntities();

                var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();
                if (lead != null)
                {
                    lead.slm_NoteFlag = noteFlag ? "1" : "0";
                    lead.slm_UpdatedBy = updateBy;
                    lead.slm_UpdatedDate = DateTime.Now;
                    slmdb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region AppInPool

        public static List<TicketIdByTeamData> GetNumberOfAppInPoolAllJob()
        {
            SLMDBEntities slmdb = new SLMDBEntities();

            string sql = @"SELECT A.slm_ticketId AS TicketId, A.coc_CurrentTeam AS Team, flowlog.coc_FlowLogId AS FlowLogId
                            FROM(
	                            SELECT lead.slm_ticketId, lead.coc_CurrentTeam
	                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
	                            INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
	                            WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1
                                AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
	                            AND lead.coc_CurrentTeam IS NOT NULL AND lead.coc_LastOwner IS NULL AND team.is_ViewMonitoring = '1'
                                AND lead.coc_AssignedFlag = '0'
	                            ) A
                            LEFT JOIN (
                                SELECT MAX(coc_FlowLogId) AS coc_FlowLogId, coc_TeamFrom, coc_TicketId 
                                FROM " + SLMDBName + @".dbo.kkcoc_tr_flowlog
                                WHERE coc_TeamFrom <> 'MARKETING'
	                            GROUP BY coc_TeamFrom, coc_TicketId         
                                ) flowlog ON flowlog.coc_TeamFrom = A.coc_CurrentTeam AND flowlog.coc_TicketId = A.slm_ticketId
                            ";

            return slmdb.ExecuteStoreQuery<TicketIdByTeamData>(sql).ToList();
        }

        public static List<LeadDataPopupMonitoring> GetNumberOfAppInPoolAllJobByTeam(string cocTeam)
        {
            //แก้ไข Position แล้ว
            SLMDBEntities slmdb = new SLMDBEntities();

            string sql = @"SELECT A.TicketId, A.CocCounting, A.AppNo, A.MarketingOwner, A.MarketingOwnerName, A.CocStatusDesc, A.DealerCode, A.DealerName, A.Channel, A.CarType, A.OwnerBranchName, A.ClientFirstname, A.ClientLastname, A.TelNo1
                            , A.CardNo, A.SlmOwnerName, A.SlmDelegateName, A.LastOwnerName, A.CocFirstAssignDate, A.CocAssignedDate, A.Team, flowlog.coc_FlowLogId AS FlowLogId, A.LeadAging, A.TeamAging
                            , CASE WHEN flowlog.coc_FlowLogId IS NULL THEN 'งานที่ส่งเข้าทีมครั้งแรก' ELSE 'งานที่เคยส่งเข้าทีม' END AS JobType, A.CocAssignTypeDesc, A.CocFirstTeamAssign, A.LeadCreatedDate, A.AppAging
, CampaignName,  RankingName     , RankingId                       
FROM(
                                SELECT lead.slm_ticketId AS TicketId, lead.coc_Counting AS CocCounting, lead.coc_Appno AS AppNo, lead.coc_MarketingOwner AS MarketingOwner
                                , cocstatus.slm_OptionDesc AS CocStatusDesc, lead.slm_DealerCode AS DealerCode, lead.slm_DealerName AS DealerName, lead.slm_ChannelId AS Channel
                                , CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                                       WHEN poMktOwner.slm_PositionNameABB IS NULL THEN mktowner.slm_StaffNameTH 
                                       ELSE poMktOwner.slm_PositionNameABB + ' - ' + mktowner.slm_StaffNameTH END MarketingOwnerName
                                , CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
                                       WHEN poLastOwner.slm_PositionNameABB IS NULL THEN lastowner.slm_StaffNameTH 
                                       ELSE poLastOwner.slm_PositionNameABB + ' - ' + lastowner.slm_StaffNameTH END LastOwnerName
                                , CASE WHEN poOwner.slm_PositionNameABB IS NULL THEN slmowner.slm_StaffNameTH
	                                   ELSE poOwner.slm_PositionNameABB + ' - ' + slmowner.slm_StaffNameTH  END SlmOwnerName
	                            , CASE WHEN poDelegate.slm_PositionNameABB IS NULL THEN slmdelegate.slm_StaffNameTH 
		                               ELSE poDelegate.slm_PositionNameABB + ' - ' + slmdelegate.slm_StaffNameTH  END SlmDelegateName
                                , CASE WHEN prodinfo.slm_CarType IS NULL THEN ''
                                        WHEN prodinfo.slm_CarType = '0' THEN 'รถใหม่'
                                        WHEN prodinfo.slm_CarType = '1' THEN 'รถเก่า'
                                        ELSE '' END AS CarType
                                , ownerbranch.slm_BranchName AS OwnerBranchName, lead.slm_Name AS ClientFirstname, lead.slm_LastName AS ClientLastname, lead.slm_TelNo_1 AS TelNo1, cusinfo.slm_CitizenId AS CardNo
                                , lead.coc_FirstAssign AS CocFirstAssignDate
                                , lead.coc_AssignedDate AS CocAssignedDate, lead.coc_CurrentTeam AS Team
                                , CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstAssign, GETDATE())) AS LeadAging, CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstTeamAssign, GETDATE())) AS TeamAging
                                , CASE WHEN lead.coc_AssignedType = '1' THEN 'RuleAssign'
                                        WHEN lead.coc_AssignedType = '2' THEN 'RuleReAssign'
                                        WHEN lead.coc_AssignedType = '3' THEN 'SupervisorAssign'
                                        ELSE '' END CocAssignTypeDesc
                                , lead.coc_FirstTeamAssign AS CocFirstTeamAssign, lead.slm_CreatedDate AS LeadCreatedDate, CONVERT(VARCHAR, DATEDIFF(day, lead.slm_CreatedDate, GETDATE())) AS AppAging
                                , campaign.slm_CampaignName AS CampaignName, ranking.coc_Name AS RankingName, ranking.coc_RankingId AS RankingId

                                FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                                INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poMktOwner ON lead.coc_MarketingOwner_Position = poMktOwner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status'
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch ownerbranch ON lead.slm_Owner_Branch = ownerbranch.slm_BranchCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo cusinfo ON lead.slm_ticketId = cusinfo.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmowner ON lead.slm_Owner = slmowner.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmdelegate ON lead.slm_Delegate = slmdelegate.slm_UserName
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poLastOwner ON lead.coc_LastOwner_Position = poLastOwner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                                LEFT JOIN " + SLMDBName + @".DBO.kkcoc_tr_ranking ranking ON lead.coc_RankingId = ranking.coc_RankingId
                                WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1
                                AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
                                AND lead.coc_CurrentTeam IS NOT NULL AND lead.coc_LastOwner IS NULL AND team.is_ViewMonitoring = '1'
                                AND lead.coc_AssignedFlag = '0' AND lead.coc_CurrentTeam = '" + cocTeam + @"'
                                ) A
                            LEFT JOIN (
                                SELECT MAX(coc_FlowLogId) AS coc_FlowLogId, coc_TeamFrom, coc_TicketId 
                                FROM " + SLMDBName + @".dbo.kkcoc_tr_flowlog
                                WHERE coc_TeamFrom <> 'MARKETING'
	                            GROUP BY coc_TeamFrom, coc_TicketId   
                                ) flowlog ON flowlog.coc_TeamFrom = A.Team AND flowlog.coc_TicketId = A.TicketId
                            ORDER BY A.CocFirstAssignDate
                            ";

            return slmdb.ExecuteStoreQuery<LeadDataPopupMonitoring>(sql).ToList();
        }

        #endregion

        #region AppWaitAssign

        public static List<TicketIdByTeamData> GetNumberOfAppWaitAssignAllJob(string subStatusWaitingList)
        {
            SLMDBEntities slmdb = new SLMDBEntities();

            if (subStatusWaitingList != "")
            {
                string sql = @"SELECT A.slm_ticketId AS TicketId, A.coc_CurrentTeam AS Team, flowlog.coc_FlowLogId AS FlowLogId
                                FROM(
	                                SELECT lead.slm_ticketId, lead.coc_CurrentTeam
	                                FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
	                                INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
	                                WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1 
                                    AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
	                                AND lead.coc_CurrentTeam IS NOT NULL AND lead.coc_LastOwner IS NOT NULL AND team.is_ViewMonitoring = '1'
	                                AND lead.coc_SubStatus IN (" + subStatusWaitingList + @")
                                    AND lead.coc_AssignedFlag = '1'
                                )A
                                LEFT JOIN (
                                    SELECT MAX(coc_FlowLogId) AS coc_FlowLogId, coc_TeamFrom, coc_TicketId 
                                    FROM " + SLMDBName + @".dbo.kkcoc_tr_flowlog
                                    WHERE coc_TeamFrom <> 'MARKETING'
	                                GROUP BY coc_TeamFrom, coc_TicketId
                                ) flowlog ON flowlog.coc_TeamFrom = A.coc_CurrentTeam AND flowlog.coc_TicketId = A.slm_ticketId";

                return slmdb.ExecuteStoreQuery<TicketIdByTeamData>(sql).ToList();
            }
            else
                return new List<TicketIdByTeamData>();
        }

        public static List<LeadDataPopupMonitoring> GetNumberOfAppWaitAssignAllJobByTeam(string cocTeam)
        {
            //แก้ไข Position แล้ว
            SLMDBEntities slmdb = new SLMDBEntities();

            string subStatusWaitingList = KKCocMsTeamDal.GetSubStatusWaitingList();

            if (subStatusWaitingList != "")
            {
                string sql = @"SELECT A.TicketId, A.CocCounting, A.AppNo, A.MarketingOwner, A.MarketingOwnerName, A.CocStatusDesc, A.DealerCode, A.DealerName, A.Channel, A.CarType, A.OwnerBranchName, A.ClientFirstname, A.ClientLastname, A.TelNo1
                            , A.CardNo, A.SlmOwnerName, A.SlmDelegateName, A.LastOwnerName, A.CocFirstAssignDate, A.CocAssignedDate, A.Team, flowlog.coc_FlowLogId AS FlowLogId, A.LeadAging, A.TeamAging
                            , CASE WHEN flowlog.coc_FlowLogId IS NULL THEN 'งานที่ส่งเข้าทีมครั้งแรก' ELSE 'งานที่เคยส่งเข้าทีม' END AS JobType, A.CocAssignTypeDesc, A.CocFirstTeamAssign, A.LeadCreatedDate, A.AppAging
,  CampaignName,  RankingName   ,RankingId                         
FROM(
                                SELECT lead.slm_ticketId AS TicketId, lead.coc_Counting AS CocCounting, lead.coc_Appno AS AppNo, lead.coc_MarketingOwner AS MarketingOwner
                                    , cocstatus.slm_OptionDesc AS CocStatusDesc, lead.slm_DealerCode AS DealerCode, lead.slm_DealerName AS DealerName, lead.slm_ChannelId AS Channel
                                    , CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                                           WHEN poMktOwner.slm_PositionNameABB IS NULL THEN mktowner.slm_StaffNameTH 
                                           ELSE poMktOwner.slm_PositionNameABB + ' - ' + mktowner.slm_StaffNameTH END MarketingOwnerName
                                    , CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
                                           WHEN poLastOwner.slm_PositionNameABB IS NULL THEN lastowner.slm_StaffNameTH 
                                           ELSE poLastOwner.slm_PositionNameABB + ' - ' + lastowner.slm_StaffNameTH END LastOwnerName
                                    , CASE WHEN poOwner.slm_PositionNameABB IS NULL THEN slmowner.slm_StaffNameTH
	                                       ELSE poOwner.slm_PositionNameABB + ' - ' + slmowner.slm_StaffNameTH  END SlmOwnerName
	                                , CASE WHEN poDelegate.slm_PositionNameABB IS NULL THEN slmdelegate.slm_StaffNameTH 
		                                   ELSE poDelegate.slm_PositionNameABB + ' - ' + slmdelegate.slm_StaffNameTH  END SlmDelegateName
                                    , CASE WHEN prodinfo.slm_CarType IS NULL THEN ''
                                            WHEN prodinfo.slm_CarType = '0' THEN 'รถใหม่'
                                            WHEN prodinfo.slm_CarType = '1' THEN 'รถเก่า'
                                            ELSE '' END AS CarType
                                    , ownerbranch.slm_BranchName AS OwnerBranchName, lead.slm_Name AS ClientFirstname, lead.slm_LastName AS ClientLastname, lead.slm_TelNo_1 AS TelNo1, cusinfo.slm_CitizenId AS CardNo
                                    , lead.coc_FirstAssign AS CocFirstAssignDate
                                    , lead.coc_AssignedDate AS CocAssignedDate, lead.coc_CurrentTeam AS Team
                                    , CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstAssign, GETDATE())) AS LeadAging, CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstTeamAssign, GETDATE())) AS TeamAging
                                    , CASE WHEN lead.coc_AssignedType = '1' THEN 'RuleAssign'
                                            WHEN lead.coc_AssignedType = '2' THEN 'RuleReAssign'
                                            WHEN lead.coc_AssignedType = '3' THEN 'SupervisorAssign'
                                            ELSE '' END CocAssignTypeDesc
                                    , lead.coc_FirstTeamAssign AS CocFirstTeamAssign, lead.slm_CreatedDate AS LeadCreatedDate, CONVERT(VARCHAR, DATEDIFF(day, lead.slm_CreatedDate, GETDATE())) AS AppAging
, campaign.slm_CampaignName AS CampaignName, ranking.coc_Name AS RankingName    , ranking.coc_RankingId AS RankingId                             
FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                                INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poMktOwner ON lead.coc_MarketingOwner_Position = poMktOwner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status'
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch ownerbranch ON lead.slm_Owner_Branch = ownerbranch.slm_BranchCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo cusinfo ON lead.slm_ticketId = cusinfo.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmowner ON lead.slm_Owner = slmowner.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmdelegate ON lead.slm_Delegate = slmdelegate.slm_UserName
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poLastOwner ON lead.coc_LastOwner_Position = poLastOwner.slm_Position_id
LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                                LEFT JOIN " + SLMDBName + @".DBO.kkcoc_tr_ranking ranking ON lead.coc_RankingId = ranking.coc_RankingId                                
WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1
                                AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
                                AND lead.coc_CurrentTeam IS NOT NULL AND lead.coc_LastOwner IS NOT NULL AND team.is_ViewMonitoring = '1'
                                AND lead.coc_SubStatus IN (" + subStatusWaitingList + @")
                                AND lead.coc_AssignedFlag = '1' AND lead.coc_CurrentTeam = '" + cocTeam + @"'
                                ) A
                            LEFT JOIN (
                                SELECT MAX(coc_FlowLogId) AS coc_FlowLogId, coc_TeamFrom, coc_TicketId 
                                FROM " + SLMDBName + @".dbo.kkcoc_tr_flowlog
                                WHERE coc_TeamFrom <> 'MARKETING'
	                            GROUP BY coc_TeamFrom, coc_TicketId
                                ) flowlog ON flowlog.coc_TeamFrom = A.Team AND flowlog.coc_TicketId = A.TicketId
                            ORDER BY A.CocFirstAssignDate
                            ";

                return slmdb.ExecuteStoreQuery<LeadDataPopupMonitoring>(sql).ToList();
            }
            else
                return new List<LeadDataPopupMonitoring>();
        }

        #endregion

        #region AppAssigned

        public static List<TicketIdByTeamData> GetNumberOfAppAssignedAllJob()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string subStatusList = "";

            var teamList = slmdb.kkcoc_ms_team.Where(p => p.is_Deleted == 0 && p.is_ViewMonitoring == true).OrderBy(p => p.coc_Seq).ToList();
            foreach (kkcoc_ms_team team in teamList)
            {
                if (!string.IsNullOrEmpty(team.coc_SubStatusReceive))
                {
                    string[] substatus = team.coc_SubStatusReceive.Split(',');
                    foreach (string str in substatus)
                    {
                        subStatusList += (subStatusList != "" ? "," : "") + "'" + str + "'";
                    }
                }
            }

            if (subStatusList != "")
            {
                string sql = @"SELECT A.slm_ticketId AS TicketId, A.coc_CurrentTeam AS Team, flowlog.coc_FlowLogId AS FlowLogId
                                FROM(
	                                SELECT lead.slm_ticketId, lead.coc_CurrentTeam
	                                FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
	                                INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
	                                WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1 
                                    AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
	                                AND lead.coc_CurrentTeam IS NOT NULL AND lead.coc_LastOwner IS NOT NULL AND team.is_ViewMonitoring = '1'
	                                AND lead.coc_SubStatus IN (" + subStatusList + @")
                                    AND lead.coc_AssignedFlag = '1'
                                )A
                                LEFT JOIN (
                                    SELECT MAX(coc_FlowLogId) AS coc_FlowLogId, coc_TeamFrom, coc_TicketId 
                                    FROM " + SLMDBName + @".dbo.kkcoc_tr_flowlog
                                    WHERE coc_TeamFrom <> 'MARKETING'
	                                GROUP BY coc_TeamFrom, coc_TicketId
                                ) flowlog ON flowlog.coc_TeamFrom = A.coc_CurrentTeam AND flowlog.coc_TicketId = A.slm_ticketId
                                ";

                return slmdb.ExecuteStoreQuery<TicketIdByTeamData>(sql).ToList();
            }
            else
                return new List<TicketIdByTeamData>();
        }

        public static List<LeadDataPopupMonitoring> GetNumberOfAppAssignedAllJobByTeam(string cocTeam)
        {
            //แก้ไข Position แล้ว
            SLMDBEntities slmdb = new SLMDBEntities();
            string subStatusList = "";

            var teamList = slmdb.kkcoc_ms_team.Where(p => p.is_Deleted == 0 && p.is_ViewMonitoring == true).OrderBy(p => p.coc_Seq).ToList();
            foreach (kkcoc_ms_team team in teamList)
            {
                if (!string.IsNullOrEmpty(team.coc_SubStatusReceive))
                {
                    string[] substatus = team.coc_SubStatusReceive.Split(',');
                    foreach (string str in substatus)
                    {
                        subStatusList += (subStatusList != "" ? "," : "") + "'" + str + "'";
                    }
                }
            }

            if (subStatusList != "")
            {
                string sql = @"SELECT A.TicketId, A.CocCounting, A.AppNo, A.MarketingOwner, A.MarketingOwnerName, A.CocStatusDesc, A.DealerCode, A.DealerName, A.Channel, A.CarType, A.OwnerBranchName, A.ClientFirstname, A.ClientLastname, A.TelNo1
                            , A.CardNo, A.SlmOwnerName, A.SlmDelegateName, A.LastOwnerName, A.CocFirstAssignDate, A.CocAssignedDate, A.Team, flowlog.coc_FlowLogId AS FlowLogId, A.LeadAging, A.TeamAging
                            , CASE WHEN flowlog.coc_FlowLogId IS NULL THEN 'งานที่ส่งเข้าทีมครั้งแรก' ELSE 'งานที่เคยส่งเข้าทีม' END AS JobType, A.CocAssignTypeDesc, A.CocFirstTeamAssign, A.LeadCreatedDate, A.AppAging
, CampaignName,  RankingName, RankingId                                
FROM(
                                SELECT lead.slm_ticketId AS TicketId, lead.coc_Counting AS CocCounting, lead.coc_Appno AS AppNo, lead.coc_MarketingOwner AS MarketingOwner
                                , cocstatus.slm_OptionDesc AS CocStatusDesc, lead.slm_DealerCode AS DealerCode, lead.slm_DealerName AS DealerName, lead.slm_ChannelId AS Channel
                                , CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                                       WHEN poMktOwner.slm_PositionNameABB IS NULL THEN mktowner.slm_StaffNameTH 
                                       ELSE poMktOwner.slm_PositionNameABB + ' - ' + mktowner.slm_StaffNameTH END MarketingOwnerName
                                , CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
                                       WHEN poLastOwner.slm_PositionNameABB IS NULL THEN lastowner.slm_StaffNameTH 
                                       ELSE poLastOwner.slm_PositionNameABB + ' - ' + lastowner.slm_StaffNameTH END LastOwnerName
                                , CASE WHEN poOwner.slm_PositionNameABB IS NULL THEN slmowner.slm_StaffNameTH
	                                   ELSE poOwner.slm_PositionNameABB + ' - ' + slmowner.slm_StaffNameTH  END SlmOwnerName
	                            , CASE WHEN poDelegate.slm_PositionNameABB IS NULL THEN slmdelegate.slm_StaffNameTH 
		                               ELSE poDelegate.slm_PositionNameABB + ' - ' + slmdelegate.slm_StaffNameTH  END SlmDelegateName
                                , CASE WHEN prodinfo.slm_CarType IS NULL THEN ''
                                        WHEN prodinfo.slm_CarType = '0' THEN 'รถใหม่'
                                        WHEN prodinfo.slm_CarType = '1' THEN 'รถเก่า'
                                        ELSE '' END AS CarType
                                , ownerbranch.slm_BranchName AS OwnerBranchName, lead.slm_Name AS ClientFirstname, lead.slm_LastName AS ClientLastname, lead.slm_TelNo_1 AS TelNo1, cusinfo.slm_CitizenId AS CardNo
                                , lead.coc_FirstAssign AS CocFirstAssignDate
                                , lead.coc_AssignedDate AS CocAssignedDate, lead.coc_CurrentTeam AS Team
                                , CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstAssign, GETDATE())) AS LeadAging, CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstTeamAssign, GETDATE())) AS TeamAging
                                , CASE WHEN lead.coc_AssignedType = '1' THEN 'RuleAssign'
                                        WHEN lead.coc_AssignedType = '2' THEN 'RuleReAssign'
                                        WHEN lead.coc_AssignedType = '3' THEN 'SupervisorAssign'
                                        ELSE '' END CocAssignTypeDesc
                                , lead.coc_FirstTeamAssign AS CocFirstTeamAssign, lead.slm_CreatedDate AS LeadCreatedDate, CONVERT(VARCHAR, DATEDIFF(day, lead.slm_CreatedDate, GETDATE())) AS AppAging
, campaign.slm_CampaignName AS CampaignName, ranking.coc_Name AS RankingName , ranking.coc_RankingId AS RankingId                               
FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                                INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poMktOwner ON lead.coc_MarketingOwner_Position = poMktOwner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status'
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch ownerbranch ON lead.slm_Owner_Branch = ownerbranch.slm_BranchCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo cusinfo ON lead.slm_ticketId = cusinfo.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmowner ON lead.slm_Owner = slmowner.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmdelegate ON lead.slm_Delegate = slmdelegate.slm_UserName
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poLastOwner ON lead.coc_LastOwner_Position = poLastOwner.slm_Position_id
LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                                LEFT JOIN " + SLMDBName + @".DBO.kkcoc_tr_ranking ranking ON lead.coc_RankingId = ranking.coc_RankingId                               
WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1
                                AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
                                AND lead.coc_CurrentTeam IS NOT NULL AND lead.coc_LastOwner IS NOT NULL AND team.is_ViewMonitoring = '1'
                                AND lead.coc_SubStatus IN (" + subStatusList + @")
                                AND lead.coc_AssignedFlag = '1' AND lead.coc_CurrentTeam = '" + cocTeam + @"'
                                                                
) A
                            LEFT JOIN (
                                SELECT MAX(coc_FlowLogId) AS coc_FlowLogId, coc_TeamFrom, coc_TicketId 
                                FROM " + SLMDBName + @".dbo.kkcoc_tr_flowlog
                                WHERE coc_TeamFrom <> 'MARKETING'
	                            GROUP BY coc_TeamFrom, coc_TicketId
                                ) flowlog ON flowlog.coc_TeamFrom = A.Team AND flowlog.coc_TicketId = A.TicketId
                            ORDER BY A.CocFirstAssignDate
                            ";

                return slmdb.ExecuteStoreQuery<LeadDataPopupMonitoring>(sql).ToList();
            }
            else
                return new List<LeadDataPopupMonitoring>();
        }

        #endregion

        #region UserMonitoring

        public static List<UserMonitoringNewJobData> GetUserMonitoringNewJobWaitAssignList(string available, string cocTeam, string teamList)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string subStatusWaitingList = KKCocMsTeamDal.GetSubStatusWaitingList();

            if (subStatusWaitingList != "")
            {
                string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.coc_CurrentTeam AS Team, lead.coc_LastOwner AS LastOwner
	                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
	                            INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.coc_LastOwner = staff.slm_EmpCode
	                            WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1
                                AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
	                            AND lead.coc_CurrentTeam IS NOT NULL AND lead.coc_LastOwner IS NOT NULL AND team.is_ViewMonitoring = '1'
                                AND lead.coc_AssignedFlag = '1'
                                AND lead.coc_SubStatus IN (" + subStatusWaitingList + @")
	                            AND staff.coc_Team IN (" + teamList + @") {0}
                                ORDER BY lead.coc_CurrentTeam, lead.coc_LastOwner  ";

                string whr = "";
                whr += (available == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_IsActive = '" + available + "' ");
                whr += (cocTeam == "" ? "" : (whr == "" ? "" : " AND ") + " staff.coc_Team = '" + cocTeam + "' ");

                sql = string.Format(sql, (whr == "" ? "" : " AND " + whr));

                return slmdb.ExecuteStoreQuery<UserMonitoringNewJobData>(sql).ToList();
            }
            else
                return new List<UserMonitoringNewJobData>();

        }

        public static List<UserMonitoringNewJobData> GetUserMonitoringNewJobAssignedList(string available, string cocTeam, string teamList)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string subStatusList = KKCocMsTeamDal.GetSubStatusAssignedList();

            if (subStatusList != "")
            {
                string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.coc_CurrentTeam AS Team, lead.coc_LastOwner AS LastOwner
	                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
	                            INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
	                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.coc_LastOwner = staff.slm_EmpCode
	                            WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1 
                                AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
	                            AND lead.coc_CurrentTeam IS NOT NULL AND lead.coc_LastOwner IS NOT NULL AND team.is_ViewMonitoring = '1'
                                AND lead.coc_AssignedFlag = '1'
                                AND lead.coc_SubStatus IN (" + subStatusList + @")
	                            AND staff.coc_Team IN (" + teamList + @") {0}
                                ORDER BY lead.coc_CurrentTeam, lead.coc_LastOwner  ";

                string whr = "";
                whr += (available == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_IsActive = '" + available + "' ");
                whr += (cocTeam == "" ? "" : (whr == "" ? "" : " AND ") + " staff.coc_Team = '" + cocTeam + "' ");

                sql = string.Format(sql, (whr == "" ? "" : " AND " + whr));

                return slmdb.ExecuteStoreQuery<UserMonitoringNewJobData>(sql).ToList();
            }
            else
                return new List<UserMonitoringNewJobData>();
        }

        public static List<LeadDataPopupMonitoring> GetUserMonitoringDoneJobListForPopup(DateTime action_date, string cocTeam, string lastOwner, string flowType, string routeBackTo)
        {
            //แก้ไข Position แล้ว
            SLMDBEntities slmdb = new SLMDBEntities();
            string whr = "";
            string sql = "";
            string actionDate = action_date.Year.ToString() + action_date.ToString("-MM-dd");

            sql = @"SELECT lead.slm_ticketId AS TicketId, lead.coc_Counting AS CocCounting, lead.coc_Appno AS AppNo, lead.coc_MarketingOwner AS MarketingOwner
                    , cocstatus.slm_OptionDesc AS CocStatusDesc, lead.slm_DealerCode AS DealerCode, lead.slm_DealerName AS DealerName, lead.slm_ChannelId AS Channel
                    , CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                           WHEN poMktOwner.slm_PositionNameABB IS NULL THEN mktowner.slm_StaffNameTH 
                           ELSE poMktOwner.slm_PositionNameABB + ' - ' + mktowner.slm_StaffNameTH END MarketingOwnerName
                    , CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
                           WHEN poLastOwner.slm_PositionNameABB IS NULL THEN lastowner.slm_StaffNameTH 
                           ELSE poLastOwner.slm_PositionNameABB + ' - ' + lastowner.slm_StaffNameTH END LastOwnerName
                    , CASE WHEN poOwner.slm_PositionNameABB IS NULL THEN slmowner.slm_StaffNameTH
                           ELSE poOwner.slm_PositionNameABB + ' - ' + slmowner.slm_StaffNameTH  END SlmOwnerName
                    , CASE WHEN poDelegate.slm_PositionNameABB IS NULL THEN slmdelegate.slm_StaffNameTH 
	                       ELSE poDelegate.slm_PositionNameABB + ' - ' + slmdelegate.slm_StaffNameTH  END SlmDelegateName
                    , CASE WHEN prodinfo.slm_CarType IS NULL THEN ''
                            WHEN prodinfo.slm_CarType = '0' THEN 'รถใหม่'
                            WHEN prodinfo.slm_CarType = '1' THEN 'รถเก่า'
                            ELSE '' END AS CarType
                    , ownerbranch.slm_BranchName AS OwnerBranchName, lead.slm_Name AS ClientFirstname, lead.slm_LastName AS ClientLastname, lead.slm_TelNo_1 AS TelNo1, cusinfo.slm_CitizenId AS CardNo
                    , lead.coc_FirstAssign AS CocFirstAssignDate
                    , lead.coc_AssignedDate AS CocAssignedDate, lead.coc_CurrentTeam AS Team
                    , CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstAssign, GETDATE())) AS LeadAging, CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstTeamAssign, GETDATE())) AS TeamAging
                    , CASE WHEN flowlog.coc_FlowType = 'F' THEN 'ส่งต่อ'
                            WHEN flowlog.coc_FlowType = 'R' AND flowlog.coc_TeamTo LIKE '%COC%' THEN 'ส่งกลับ - COC'
                            WHEN flowlog.coc_FlowType = 'R' AND flowlog.coc_TeamTo LIKE '%MARKETING%' THEN 'ส่งกลับ - MARKETING'
                            ELSE '' END AS DoneJobType
                    , flowlog.coc_ActionDate AS ActionDate, lead.coc_FirstTeamAssign AS CocFirstTeamAssign, lead.slm_CreatedDate AS LeadCreatedDate, CONVERT(VARCHAR, DATEDIFF(day, lead.slm_CreatedDate, GETDATE())) AS AppAging
, campaign.slm_CampaignName AS CampaignName, ranking.coc_Name AS RankingName  , ranking.coc_RankingId                  
FROM " + SLMDBName + @".dbo.kkcoc_tr_flowlog flowlog
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_lead lead ON flowlog.coc_TicketId = lead.slm_ticketId AND lead.is_Deleted = '0'
                    INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poMktOwner ON lead.coc_MarketingOwner_Position = poMktOwner.slm_Position_id
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status'
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch ownerbranch ON lead.slm_Owner_Branch = ownerbranch.slm_BranchCode
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo cusinfo ON lead.slm_ticketId = cusinfo.slm_TicketId
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmowner ON lead.slm_Owner = slmowner.slm_UserName
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmdelegate ON lead.slm_Delegate = slmdelegate.slm_UserName
                    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id
                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poLastOwner ON lead.coc_LastOwner_Position = poLastOwner.slm_Position_id
LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                                LEFT JOIN " + SLMDBName + @".DBO.kkcoc_tr_ranking ranking ON lead.coc_RankingId = ranking.coc_RankingId                    
WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1 
                    AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
                    {0} 
                    ORDER BY flowlog.coc_ActionDate ";

            if (flowType == "F")
            {
                whr = @" flowlog.coc_FlowType = 'F' AND flowlog.coc_TeamFrom = '" + cocTeam + "' AND flowlog.coc_EmpCodeTeamFrom = '" + lastOwner + @"'
                         AND CONVERT(DATE, flowlog.coc_ActionDate) = '" + actionDate + @"' ";
            }
            else if (flowType == "R")
            {
                if (routeBackTo == "COC")
                {
                    whr = @" flowlog.coc_FlowType = 'R' AND flowlog.coc_TeamTo LIKE '%COC%' AND flowlog.coc_TeamFrom = '" + cocTeam + @"' AND flowlog.coc_EmpCodeTeamFrom = '" + lastOwner + @"'
                             AND CONVERT(DATE, flowlog.coc_ActionDate) = '" + actionDate + @"' ";
                }
                else
                {
                    //RouteBackTo MARKETING
                    whr = @" flowlog.coc_FlowType = 'R' AND flowlog.coc_TeamTo LIKE '%MARKETING%' AND flowlog.coc_TeamFrom = '" + cocTeam + @"' AND flowlog.coc_EmpCodeTeamFrom = '" + lastOwner + @"'
                             AND CONVERT(DATE, flowlog.coc_ActionDate) = '" + actionDate + @"' ";
                }
            }
            else if (flowType == "")
            {
                //คอลัมน์ UserMonitoring > งานเสร็จ > งานเสร็จทั้งหมด 
                whr = @" flowlog.coc_TeamFrom = '" + cocTeam + @"' AND flowlog.coc_EmpCodeTeamFrom = '" + lastOwner + @"'
                         AND CONVERT(DATE, flowlog.coc_ActionDate) = '" + actionDate + @"' ";
            }

            sql = string.Format(sql, (whr != "" ? " AND " + whr : ""));

            return slmdb.ExecuteStoreQuery<LeadDataPopupMonitoring>(sql).ToList();
        }

        public static List<LeadDataPopupMonitoring> GetUserMonitoringNewJobListForPopup(string cocTeam, string lastOwner, string assignedFlag)
        {
            //แก้ไข Position แล้ว
            SLMDBEntities slmdb = new SLMDBEntities();
            string subStatusList = "";

            if (assignedFlag == "waitassign")
                subStatusList = KKCocMsTeamDal.GetSubStatusWaitingList();
            else if (assignedFlag == "assigned")
                subStatusList = KKCocMsTeamDal.GetSubStatusAssignedList();

            string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.coc_Counting AS CocCounting, lead.coc_Appno AS AppNo, lead.coc_MarketingOwner AS MarketingOwner
                            , cocstatus.slm_OptionDesc AS CocStatusDesc, lead.slm_DealerCode AS DealerCode, lead.slm_DealerName AS DealerName, lead.slm_ChannelId AS Channel
                            , CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                                   WHEN poMktOwner.slm_PositionNameABB IS NULL THEN mktowner.slm_StaffNameTH 
                                   ELSE poMktOwner.slm_PositionNameABB + ' - ' + mktowner.slm_StaffNameTH END MarketingOwnerName
                            , CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
                                   WHEN poLastOwner.slm_PositionNameABB IS NULL THEN lastowner.slm_StaffNameTH 
                                   ELSE poLastOwner.slm_PositionNameABB + ' - ' + lastowner.slm_StaffNameTH END LastOwnerName
                            , CASE WHEN poOwner.slm_PositionNameABB IS NULL THEN slmowner.slm_StaffNameTH
                                   ELSE poOwner.slm_PositionNameABB + ' - ' + slmowner.slm_StaffNameTH  END SlmOwnerName
                            , CASE WHEN poDelegate.slm_PositionNameABB IS NULL THEN slmdelegate.slm_StaffNameTH 
	                               ELSE poDelegate.slm_PositionNameABB + ' - ' + slmdelegate.slm_StaffNameTH  END SlmDelegateName
                            , CASE WHEN prodinfo.slm_CarType IS NULL THEN ''
                                    WHEN prodinfo.slm_CarType = '0' THEN 'รถใหม่'
                                    WHEN prodinfo.slm_CarType = '1' THEN 'รถเก่า'
                                    ELSE '' END AS CarType
                            , ownerbranch.slm_BranchName AS OwnerBranchName, lead.slm_Name AS ClientFirstname, lead.slm_LastName AS ClientLastname, lead.slm_TelNo_1 AS TelNo1, cusinfo.slm_CitizenId AS CardNo
                            , lead.coc_FirstAssign AS CocFirstAssignDate
                            , lead.coc_AssignedDate AS CocAssignedDate, lead.coc_CurrentTeam AS Team
                            , CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstAssign, GETDATE())) AS LeadAging, CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstTeamAssign, GETDATE())) AS TeamAging
                            , CASE WHEN lead.coc_SubStatus IN ('01','21','31','02','32','03') THEN 'งานรอรับ' 
                                    WHEN lead.coc_SubStatus IN ('11','22','33') THEN 'งานที่รับแล้ว'
                                    ELSE '' END AS JobType
                            , CASE WHEN lead.coc_AssignedType = '1' THEN 'RuleAssign'
                                    WHEN lead.coc_AssignedType = '2' THEN 'RuleReAssign'
                                    WHEN lead.coc_AssignedType = '3' THEN 'SupervisorAssign'
                                    ELSE '' END CocAssignTypeDesc
                            , lead.coc_FirstTeamAssign AS CocFirstTeamAssign, lead.slm_CreatedDate AS LeadCreatedDate, CONVERT(VARCHAR, DATEDIFF(day, lead.slm_CreatedDate, GETDATE())) AS AppAging
, campaign.slm_CampaignName AS CampaignName, ranking.coc_Name AS RankingName  , ranking.coc_RankingId AS RankingId                              
FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                            INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poMktOwner ON lead.coc_MarketingOwner_Position = poMktOwner.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status'
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch ownerbranch ON lead.slm_Owner_Branch = ownerbranch.slm_BranchCode
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo cusinfo ON lead.slm_ticketId = cusinfo.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmowner ON lead.slm_Owner = slmowner.slm_UserName
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmdelegate ON lead.slm_Delegate = slmdelegate.slm_UserName
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poLastOwner ON lead.coc_LastOwner_Position = poLastOwner.slm_Position_id
LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                                LEFT JOIN " + SLMDBName + @".DBO.kkcoc_tr_ranking ranking ON lead.coc_RankingId = ranking.coc_RankingId                            
WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1 
                            AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
                            AND lead.coc_CurrentTeam IS NOT NULL AND lead.coc_LastOwner IS NOT NULL AND team.is_ViewMonitoring = '1' 
                            AND lead.coc_AssignedFlag = '1' 
                            {0}
                            AND lead.coc_LastOwner = '" + lastOwner + @"' AND lastowner.coc_Team = '" + cocTeam + @"'
                            AND lead.coc_CurrentTeam = '" + cocTeam + @"'
                            ORDER BY lead.coc_AssignedDate ";

            if (subStatusList != "")
                sql = string.Format(sql, " AND lead.coc_SubStatus IN (" + subStatusList + ") ");
            else
                sql = string.Format(sql, "");

            return slmdb.ExecuteStoreQuery<LeadDataPopupMonitoring>(sql).ToList();
        }

        #endregion

        public static void TransferJob(List<string> ticketIdList, string empCodeOfTransferee, string cocTeam, string updatedBy)
        {
            try
            {
                SLMDBEntities slmdb = new SLMDBEntities();
                DateTime updateDate = DateTime.Now;

                foreach (string ticketId in ticketIdList)
                {
                    var lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();
                    if (lead != null)
                    {
                        lead.coc_OldLastOwner = lead.coc_LastOwner; //เก็บค่า LastOwner เดิม
                        lead.coc_LastOwner = empCodeOfTransferee;   //LastOwner คนใหม่
                        lead.coc_LastOwner_Position = KKSlmMsStaffDal.GetPositionIdByEmpCode(empCodeOfTransferee);
                        lead.coc_AssignedType = "3";
                        lead.coc_AssignedFlag = "0";
                        lead.coc_AssignedBy = KKSlmMsStaffDal.GetEmpCode(updatedBy);
                        lead.coc_AssignedDate = updateDate;
                        lead.slm_UpdatedBy = updatedBy;
                        lead.slm_UpdatedDate = updateDate;

                        kkslm_tr_activity act = new kkslm_tr_activity();
                        act.slm_TicketId = ticketId;
                        act.slm_NewDelegate = empCodeOfTransferee;
                        act.slm_NewDelegate_Position = KKSlmMsStaffDal.GetPositionIdByEmpCode(empCodeOfTransferee);
                        act.slm_Type = "05";
                        act.slm_SystemAction = "COC";
                        act.coc_Team = cocTeam;
                        act.slm_CreatedBy = updatedBy;
                        act.slm_CreatedBy_Position = KKSlmMsStaffDal.GetPositionId(updatedBy);
                        act.slm_CreatedDate = updateDate;
                        act.slm_UpdatedBy = updatedBy;
                        act.slm_UpdatedDate = updateDate;
                        slmdb.kkslm_tr_activity.AddObject(act);
                    }
                }

                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region User Management

        public static List<LeadDataPopupMonitoring> GetJobOnHandList(string cocTeam, string lastOwner)
        {
            //แก้ไข Position แล้ว
            SLMDBEntities slmdb = new SLMDBEntities();

            string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.coc_Counting AS CocCounting, lead.coc_Appno AS AppNo, lead.coc_MarketingOwner AS MarketingOwner
                            , cocstatus.slm_OptionDesc AS CocStatusDesc, lead.slm_DealerCode AS DealerCode, lead.slm_DealerName AS DealerName, lead.slm_ChannelId AS Channel
                            , CASE WHEN prodinfo.slm_CarType IS NULL THEN ''
                                    WHEN prodinfo.slm_CarType = '0' THEN 'รถใหม่'
                                    WHEN prodinfo.slm_CarType = '1' THEN 'รถเก่า'
                                    ELSE '' END AS CarType
                            , CASE WHEN poOwner.slm_PositionNameABB IS NULL THEN slmowner.slm_StaffNameTH
	                               ELSE poOwner.slm_PositionNameABB + ' - ' + slmowner.slm_StaffNameTH  END SlmOwnerName
                            , CASE WHEN poDelegate.slm_PositionNameABB IS NULL THEN slmdelegate.slm_StaffNameTH 
	                               ELSE poDelegate.slm_PositionNameABB + ' - ' + slmdelegate.slm_StaffNameTH  END SlmDelegateName
                            , CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                                   WHEN poMktOwner.slm_PositionNameABB IS NULL THEN mktowner.slm_StaffNameTH 
                                   ELSE poMktOwner.slm_PositionNameABB + ' - ' + mktowner.slm_StaffNameTH END MarketingOwnerName
                            , CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
                                   WHEN poLastOwner.slm_PositionNameABB IS NULL THEN lastowner.slm_StaffNameTH 
                                   ELSE poLastOwner.slm_PositionNameABB + ' - ' + lastowner.slm_StaffNameTH END LastOwnerName
                            , ownerbranch.slm_BranchName AS OwnerBranchName, lead.slm_Name AS ClientFirstname, lead.slm_LastName AS ClientLastname, lead.slm_TelNo_1 AS TelNo1, cusinfo.slm_CitizenId AS CardNo
                            , lead.coc_FirstAssign AS CocFirstAssignDate
                            , lead.coc_AssignedDate AS CocAssignedDate, lead.coc_CurrentTeam AS Team
                            , CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstAssign, GETDATE())) AS LeadAging, CONVERT(VARCHAR, DATEDIFF(day, lead.coc_FirstTeamAssign, GETDATE())) AS TeamAging
                            , lead.coc_FirstTeamAssign AS CocFirstTeamAssign, lead.slm_CreatedDate AS LeadCreatedDate, CONVERT(VARCHAR, DATEDIFF(day, lead.slm_CreatedDate, GETDATE())) AS AppAging
, campaign.slm_CampaignName AS CampaignName , ranking.coc_Name AS RankingName, ranking.coc_RankingId AS RankingId                    
FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                            INNER JOIN " + SLMDBName + @".dbo.kkcoc_ms_team team ON lead.coc_CurrentTeam = team.coc_TeamId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poMktOwner ON lead.coc_MarketingOwner_Position = poMktOwner.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status'
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch ownerbranch ON lead.slm_Owner_Branch = ownerbranch.slm_BranchCode
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo cusinfo ON lead.slm_ticketId = cusinfo.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmowner ON lead.slm_Owner = slmowner.slm_UserName
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff slmdelegate ON lead.slm_Delegate = slmdelegate.slm_UserName
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poLastOwner ON lead.coc_LastOwner_Position = poLastOwner.slm_Position_id
LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                                LEFT JOIN " + SLMDBName + @".DBO.kkcoc_tr_ranking ranking ON lead.coc_RankingId = ranking.coc_RankingId                            
WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1 
                            AND lead.slm_Status NOT IN ('" + COCConstant.StatusCode.Reject + @"','" + COCConstant.StatusCode.Cancel + @"','" + COCConstant.StatusCode.Close + @"') 
                            AND lead.coc_AssignedFlag = '1' 
                            AND lead.coc_LastOwner = '" + lastOwner + @"' AND lastowner.coc_Team = '" + cocTeam + @"'
                            AND lead.coc_CurrentTeam = '" + cocTeam + @"'
                            ORDER BY lead.coc_AssignedDate ";

            return slmdb.ExecuteStoreQuery<LeadDataPopupMonitoring>(sql).ToList();
        }

        public bool CheckHeadStaff(string UserId, string HeadStaffId, out string desc)
        {
            try
            {

                SLMDBEntities slmdb = new SLMDBEntities();

                var result = slmdb.SP_FINDHEADERRECURSION(UserId, HeadStaffId).FirstOrDefault();

                desc = result.STAFF;

                if (desc == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                desc = ex.Message;
                return false;
            }
        }
        #endregion
    }
}
