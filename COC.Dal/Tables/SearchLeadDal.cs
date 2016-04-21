using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using COC.Resource;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class SearchLeadDal
    {
        private static string SLMDBName = COCConstant.SLMDBName;

        #region Backup SearchLeadData 1

//        public static List<SearchLeadData> SearchLeadData1(SearchLeadCondition data, string username, string recursiveList)
//        {
//            string sql = "";
//            SLMDBEntities slmdb = new SLMDBEntities();

//            string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
//            string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;
//            string cocAssignedDate = data.CocAssignedDate.Year != 1 ? data.CocAssignedDate.Year + data.CocAssignedDate.ToString("-MM-dd") : string.Empty;

//            sql = @"SELECT A.* FROM (
//                    SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
//                        info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc, 
//                        staff.slm_PositionName +' - '+staff.slm_StaffNameTH AS OwnerName, lead.slm_CreatedDate AS CreatedDate, 
//                        lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag, staff.slm_StaffId,
//                        delegate.slm_PositionName + ' - ' + delegate.slm_StaffNameTH AS DelegateName,ownerbranch.slm_BranchName AS OwnerBranchName,
//	                    Delegatebranch.slm_BranchName AS DelegateBranchName, ISNULL(createby.slm_PositionName + ' - ' + createby.slm_StaffNameTH, LEAD.slm_CreatedBy) AS CreateName,
//	                    CreateBybranch.slm_BranchName AS BranchCreateBranchName, lead.slm_Product_Name AS ProductName, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
//                        , lead.slm_CampaignId AS CampaignId, prodinfo.slm_LicenseNo AS LicenseNo, lead.slm_TelNo_1 AS TelNo1, prodinfo.slm_ProvinceRegis AS ProvinceRegis
//                        , campaign.slm_Url AS CalculatorUrl, lead.slm_Product_Group_Id AS ProductGroupId, lead.slm_Product_Id AS ProductId
//                        , lead.coc_AssignedDate AS CocAssignedDate, ISNULL(mktowner.slm_PositionName + ' - ' + mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) AS MarketingOwnerName, lead.coc_LastOwner AS LastOwner, ISNULL(lastowner.slm_PositionName + ' - ' + lastowner.slm_StaffNameTH, lead.coc_LastOwner) AS LastOwnerName
//                        , lead.coc_CurrentTeam AS CocTeam, cocstatus.slm_OptionDesc AS CocStatusDesc, lead.coc_Counting AS CocCounting, lead.coc_FlowType AS FlowType
//                        , lead.coc_Appno AS AppNo
//                    FROM " + SLMDBName + @".DBO.kkslm_tr_lead lead 
//                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
//                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
//                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
//                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
//                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' AND opt.is_Deleted = 0 
//                    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff delegate on delegate.slm_UserName = lead.slm_Delegate
//					LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch ownerbranch on lead.slm_Owner_Branch = ownerbranch.slm_BranchCode 
//					LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on lead.slm_Delegate_Branch = Delegatebranch.slm_BranchCode 
//					LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
//					LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch CreateBybranch on lead.slm_CreatedBy_Branch = CreateBybranch.slm_BranchCode 
//                    LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
//                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
//                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
//                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
//                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status' AND cocstatus.is_Deleted = 0 
//                    WHERE lead.is_Deleted = 0 AND lead.coc_LastOwner IS NOT NULL {0} ) A 
//                    {1}
//                    ORDER BY CASE WHEN A.FlowType IS NULL THEN '0' 
//                                    WHEN A.FlowType = 'F' THEN '0'
//                                    WHEN A.FlowType = 'R' THEN '1' END DESC, A.CreatedDate DESC ";

//            string whr = "";

//            whr += (string.IsNullOrEmpty(data.TicketId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ticketId LIKE '%" + data.TicketId + "%' ");
//            whr += (string.IsNullOrEmpty(data.Firstname) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Name LIKE @firstname ");
//            whr += (string.IsNullOrEmpty(data.Lastname) ? "" : (whr == "" ? "" : " AND ") + " info.slm_LastName LIKE @lastname ");
//            whr += (string.IsNullOrEmpty(data.CitizenId) ? "" : (whr == "" ? "" : " AND ") + " info.slm_CitizenId LIKE '%" + data.CitizenId + "%' ");
//            whr += (string.IsNullOrEmpty(data.CampaignId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CampaignId = '" + data.CampaignId + "' ");
//            whr += (string.IsNullOrEmpty(data.ChannelId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ChannelId = '" + data.ChannelId + "' ");
//            whr += (string.IsNullOrEmpty(data.OwnerUsername) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner = '" + data.OwnerUsername + "' ");      //Owner Lead
//            whr += (createDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_CreatedDate) = '" + createDate + "' ");
//            whr += (assignDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_AssignedDate) = '" + assignDate + "' ");
//            whr += (string.IsNullOrEmpty(data.OwnerBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner_Branch = '" + data.OwnerBranch + "' ");           //Owner Branch
//            whr += (string.IsNullOrEmpty(data.DelegateBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate_Branch = '" + data.DelegateBranch + "' ");  //Delegate Branch
//            whr += (string.IsNullOrEmpty(data.DelegateLead) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate = '" + data.DelegateLead + "' ");             //Delegate Lead
//            whr += (string.IsNullOrEmpty(data.CreateByBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy_Branch = '" + data.CreateByBranch + "' "); //CreateBy Branch
//            whr += (string.IsNullOrEmpty(data.CreateBy) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy = '" + data.CreateBy + "' ");                    //CreateBy Lead
//            whr += (string.IsNullOrEmpty(data.StatusList) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Status IN (" + data.StatusList + ") ");

//            //COC
//            whr += (cocAssignedDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.coc_AssignedDate) = '" + cocAssignedDate + "' ");
//            whr += (string.IsNullOrEmpty(data.MarketingOwner) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_MarketingOwner = '" + data.MarketingOwner + "' ");
//            whr += (recursiveList == "" ? "" : (whr == "" ? "" : " AND ") + " lead.coc_LastOwner IN (" + recursiveList + ") ");
//            whr += (string.IsNullOrEmpty(data.CocTeam) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_CurrentTeam = '" + data.CocTeam + "' ");
//            whr += (string.IsNullOrEmpty(data.CocStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_Status = '" + data.CocStatus + "' ");
//            whr += (string.IsNullOrEmpty(data.CocSubStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_SubStatus = '" + data.CocSubStatus + "' ");

//            whr = whr == "" ? "" : " AND " + whr;

//            //Where ในตำแหน่ง {1}
//            string whr1 = "";
//            if (!string.IsNullOrEmpty(data.LastOwner)) whr1 = " WHERE A.LastOwner = '" + data.LastOwner + "' ";

//            sql = string.Format(sql, whr, whr1);

//            object[] param = new object[] 
//            { 
//                new SqlParameter("@firstname", "%" + data.Firstname + "%")
//                , new SqlParameter("@lastname", "%" + data.Lastname + "%")
//            };

//            return slmdb.ExecuteStoreQuery<SearchLeadData>(sql, param).ToList();
//        }

        #endregion

        #region Backup SearchLeadData 2

//        public static List<SearchLeadData> SearchLeadData2(SearchLeadCondition data, string username, string recursiveList, string teamRecursiveList, string staffTypeId)
//        {
//            if (recursiveList.Trim() == "")
//                return new List<SearchLeadData>();

//            string sql = "";
//            SLMDBEntities slmdb = new SLMDBEntities();

//            sql = @"SELECT A.* 
//                    FROM ( " + GetSearchByLastOwnerQuery(data, username, recursiveList);

//            if ((staffTypeId == COCConstant.StaffType.ManagerOper.ToString() || staffTypeId == COCConstant.StaffType.SupervisorOper.ToString()) && teamRecursiveList.Trim() != "")
//            {
//                sql += @" UNION 
//                        " + GetSearchNoLastOwnerQuery(data, username, teamRecursiveList);
//            }

//            sql += @" ) A 
//                     {0}
//                     ORDER BY CASE WHEN A.FlowType IS NULL THEN '0' 
//                                    WHEN A.FlowType = 'F' THEN '0'
//                                    WHEN A.FlowType = 'R' THEN '1' END DESC, A.CreatedDate DESC ";

//            string whr = "";
//            whr += (string.IsNullOrEmpty(data.LastOwner) ? "" : (whr == "" ? "" : " AND ") + " A.LastOwner = '" + data.LastOwner + "' ");
//            whr += (string.IsNullOrEmpty(data.CocTeam) ? "" : (whr == "" ? "" : " AND ") + " A.CocTeam = '" + data.CocTeam + "' ");

//            sql = string.Format(sql, (whr == "" ? "" : " WHERE " + whr));

//            object[] param = new object[] 
//            { 
//                new SqlParameter("@firstname", "%" + data.Firstname + "%")
//                , new SqlParameter("@lastname", "%" + data.Lastname + "%")
//            };

//            return slmdb.ExecuteStoreQuery<SearchLeadData>(sql, param).ToList();
//        }

//        private static string GetSearchByLastOwnerQuery(SearchLeadCondition data, string username, string recursiveList)
//        {
//            string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
//            string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;
//            string cocAssignedDate = data.CocAssignedDate.Year != 1 ? data.CocAssignedDate.Year + data.CocAssignedDate.ToString("-MM-dd") : string.Empty;

//            string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
//                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc, 
//                            staff.slm_PositionName +' - '+staff.slm_StaffNameTH AS OwnerName, lead.slm_CreatedDate AS CreatedDate, 
//                            lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag, staff.slm_StaffId,
//                            delegate.slm_PositionName + ' - ' + delegate.slm_StaffNameTH AS DelegateName,ownerbranch.slm_BranchName AS OwnerBranchName,
//	                        Delegatebranch.slm_BranchName AS DelegateBranchName, ISNULL(createby.slm_PositionName + ' - ' + createby.slm_StaffNameTH, LEAD.slm_CreatedBy) AS CreateName,
//	                        CreateBybranch.slm_BranchName AS BranchCreateBranchName, lead.slm_Product_Name AS ProductName, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
//                            , lead.slm_CampaignId AS CampaignId, prodinfo.slm_LicenseNo AS LicenseNo, lead.slm_TelNo_1 AS TelNo1, prodinfo.slm_ProvinceRegis AS ProvinceRegis
//                            , campaign.slm_Url AS CalculatorUrl, lead.slm_Product_Group_Id AS ProductGroupId, lead.slm_Product_Id AS ProductId
//                            , lead.coc_AssignedDate AS CocAssignedDate, ISNULL(mktowner.slm_PositionName + ' - ' + mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) AS MarketingOwnerName, lead.coc_LastOwner AS LastOwner, ISNULL(lastowner.slm_PositionName + ' - ' + lastowner.slm_StaffNameTH, lead.coc_LastOwner) AS LastOwnerName
//                            , lead.coc_CurrentTeam AS CocTeam, cocstatus.slm_OptionDesc AS CocStatusDesc, lead.coc_Counting AS CocCounting, lead.coc_FlowType AS FlowType
//                            , lead.coc_Appno AS AppNo
//                        FROM " + SLMDBName + @".DBO.kkslm_tr_lead lead 
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' AND opt.is_Deleted = 0 
//                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff delegate on delegate.slm_UserName = lead.slm_Delegate
//					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch ownerbranch on lead.slm_Owner_Branch = ownerbranch.slm_BranchCode 
//					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on lead.slm_Delegate_Branch = Delegatebranch.slm_BranchCode 
//					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
//					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch CreateBybranch on lead.slm_CreatedBy_Branch = CreateBybranch.slm_BranchCode 
//                        LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status' AND cocstatus.is_Deleted = 0 
//                        WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1 AND lead.coc_LastOwner IS NOT NULL AND lead.coc_LastOwner IN (" + recursiveList + @") ";

//            string whr = "";

//            whr += (string.IsNullOrEmpty(data.TicketId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ticketId LIKE '%" + data.TicketId + "%' ");
//            whr += (string.IsNullOrEmpty(data.Firstname) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Name LIKE @firstname ");
//            whr += (string.IsNullOrEmpty(data.Lastname) ? "" : (whr == "" ? "" : " AND ") + " info.slm_LastName LIKE @lastname ");
//            whr += (string.IsNullOrEmpty(data.CitizenId) ? "" : (whr == "" ? "" : " AND ") + " info.slm_CitizenId LIKE '%" + data.CitizenId + "%' ");
//            whr += (string.IsNullOrEmpty(data.CampaignId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CampaignId = '" + data.CampaignId + "' ");
//            whr += (string.IsNullOrEmpty(data.ChannelId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ChannelId = '" + data.ChannelId + "' ");
//            whr += (string.IsNullOrEmpty(data.OwnerUsername) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner = '" + data.OwnerUsername + "' ");      //Owner Lead
//            whr += (createDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_CreatedDate) = '" + createDate + "' ");
//            whr += (assignDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_AssignedDate) = '" + assignDate + "' ");
//            whr += (string.IsNullOrEmpty(data.OwnerBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner_Branch = '" + data.OwnerBranch + "' ");           //Owner Branch
//            whr += (string.IsNullOrEmpty(data.DelegateBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate_Branch = '" + data.DelegateBranch + "' ");  //Delegate Branch
//            whr += (string.IsNullOrEmpty(data.DelegateLead) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate = '" + data.DelegateLead + "' ");             //Delegate Lead
//            whr += (string.IsNullOrEmpty(data.CreateByBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy_Branch = '" + data.CreateByBranch + "' "); //CreateBy Branch
//            whr += (string.IsNullOrEmpty(data.CreateBy) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy = '" + data.CreateBy + "' ");                    //CreateBy Lead
//            whr += (string.IsNullOrEmpty(data.StatusList) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Status IN (" + data.StatusList + ") ");

//            //COC
//            whr += (cocAssignedDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.coc_AssignedDate) = '" + cocAssignedDate + "' ");
//            whr += (string.IsNullOrEmpty(data.MarketingOwner) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_MarketingOwner = '" + data.MarketingOwner + "' ");
//            whr += (string.IsNullOrEmpty(data.CocStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_Status = '" + data.CocStatus + "' ");
//            whr += (string.IsNullOrEmpty(data.CocSubStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_SubStatus = '" + data.CocSubStatus + "' ");

//            if (whr != "")
//                sql = sql + " AND " + whr;

//            return sql;
//        }

//        private static string GetSearchNoLastOwnerQuery(SearchLeadCondition data, string username, string teamRecursiveList)
//        {
//            string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
//            string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;
//            string cocAssignedDate = data.CocAssignedDate.Year != 1 ? data.CocAssignedDate.Year + data.CocAssignedDate.ToString("-MM-dd") : string.Empty;

//            string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
//                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc, 
//                            staff.slm_PositionName +' - '+staff.slm_StaffNameTH AS OwnerName, lead.slm_CreatedDate AS CreatedDate, 
//                            lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag, staff.slm_StaffId,
//                            delegate.slm_PositionName + ' - ' + delegate.slm_StaffNameTH AS DelegateName,ownerbranch.slm_BranchName AS OwnerBranchName,
//	                        Delegatebranch.slm_BranchName AS DelegateBranchName, ISNULL(createby.slm_PositionName + ' - ' + createby.slm_StaffNameTH, LEAD.slm_CreatedBy) AS CreateName,
//	                        CreateBybranch.slm_BranchName AS BranchCreateBranchName, lead.slm_Product_Name AS ProductName, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
//                            , lead.slm_CampaignId AS CampaignId, prodinfo.slm_LicenseNo AS LicenseNo, lead.slm_TelNo_1 AS TelNo1, prodinfo.slm_ProvinceRegis AS ProvinceRegis
//                            , campaign.slm_Url AS CalculatorUrl, lead.slm_Product_Group_Id AS ProductGroupId, lead.slm_Product_Id AS ProductId
//                            , lead.coc_AssignedDate AS CocAssignedDate, ISNULL(mktowner.slm_PositionName + ' - ' + mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) AS MarketingOwnerName, lead.coc_LastOwner AS LastOwner, ISNULL(lastowner.slm_PositionName + ' - ' + lastowner.slm_StaffNameTH, lead.coc_LastOwner) AS LastOwnerName
//                            , lead.coc_CurrentTeam AS CocTeam, cocstatus.slm_OptionDesc AS CocStatusDesc, lead.coc_Counting AS CocCounting, lead.coc_FlowType AS FlowType
//                            , lead.coc_Appno AS AppNo
//                        FROM " + SLMDBName + @".DBO.kkslm_tr_lead lead 
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' AND opt.is_Deleted = 0 
//                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff delegate on delegate.slm_UserName = lead.slm_Delegate
//					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch ownerbranch on lead.slm_Owner_Branch = ownerbranch.slm_BranchCode 
//					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on lead.slm_Delegate_Branch = Delegatebranch.slm_BranchCode 
//					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
//					    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch CreateBybranch on lead.slm_CreatedBy_Branch = CreateBybranch.slm_BranchCode 
//                        LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
//                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status' AND cocstatus.is_Deleted = 0 
//                        WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1 AND lead.coc_LastOwner IS NULL AND lead.coc_CurrentTeam IN (" + teamRecursiveList + @") ";

//            string whr = "";

//            whr += (string.IsNullOrEmpty(data.TicketId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ticketId LIKE '%" + data.TicketId + "%' ");
//            whr += (string.IsNullOrEmpty(data.Firstname) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Name LIKE @firstname ");
//            whr += (string.IsNullOrEmpty(data.Lastname) ? "" : (whr == "" ? "" : " AND ") + " info.slm_LastName LIKE @lastname ");
//            whr += (string.IsNullOrEmpty(data.CitizenId) ? "" : (whr == "" ? "" : " AND ") + " info.slm_CitizenId LIKE '%" + data.CitizenId + "%' ");
//            whr += (string.IsNullOrEmpty(data.CampaignId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CampaignId = '" + data.CampaignId + "' ");
//            whr += (string.IsNullOrEmpty(data.ChannelId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ChannelId = '" + data.ChannelId + "' ");
//            whr += (string.IsNullOrEmpty(data.OwnerUsername) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner = '" + data.OwnerUsername + "' ");      //Owner Lead
//            whr += (createDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_CreatedDate) = '" + createDate + "' ");
//            whr += (assignDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_AssignedDate) = '" + assignDate + "' ");
//            whr += (string.IsNullOrEmpty(data.OwnerBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner_Branch = '" + data.OwnerBranch + "' ");           //Owner Branch
//            whr += (string.IsNullOrEmpty(data.DelegateBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate_Branch = '" + data.DelegateBranch + "' ");  //Delegate Branch
//            whr += (string.IsNullOrEmpty(data.DelegateLead) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate = '" + data.DelegateLead + "' ");             //Delegate Lead
//            whr += (string.IsNullOrEmpty(data.CreateByBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy_Branch = '" + data.CreateByBranch + "' "); //CreateBy Branch
//            whr += (string.IsNullOrEmpty(data.CreateBy) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy = '" + data.CreateBy + "' ");                    //CreateBy Lead
//            whr += (string.IsNullOrEmpty(data.StatusList) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Status IN (" + data.StatusList + ") ");

//            //COC
//            whr += (cocAssignedDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.coc_AssignedDate) = '" + cocAssignedDate + "' ");
//            whr += (string.IsNullOrEmpty(data.MarketingOwner) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_MarketingOwner = '" + data.MarketingOwner + "' ");
//            whr += (string.IsNullOrEmpty(data.CocStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_Status = '" + data.CocStatus + "' ");
//            whr += (string.IsNullOrEmpty(data.CocSubStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_SubStatus = '" + data.CocSubStatus + "' ");

//            if (whr != "")
//                sql = sql + " AND " + whr;

//            return sql;
//        }

        #endregion

        public static List<SearchLeadData> SearchLeadData(SearchLeadCondition data, string username, string recursiveList, string teamRecursiveList, string staffTypeId)
        {
            if (staffTypeId == COCConstant.StaffType.ManagerOper.ToString() || staffTypeId == COCConstant.StaffType.SupervisorOper.ToString())
                return DoSearchLeadDataByManagerOper(data);
            else
                return DoSearchLeadData(data, recursiveList);
        }

        private static List<SearchLeadData> DoSearchLeadData(SearchLeadCondition data, string recursiveList)
        {
            string sql = "";
            SLMDBEntities slmdb = new SLMDBEntities();

            string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;
            string cocAssignedDate = data.CocAssignedDate.Year != 1 ? data.CocAssignedDate.Year + data.CocAssignedDate.ToString("-MM-dd") : string.Empty;

            sql = @"SELECT A.* FROM (
                        SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc
                            , CASE WHEN poOwner.slm_PositionNameABB IS NULL THEN staff.slm_StaffNameTH
	                               ELSE poOwner.slm_PositionNameABB + ' - ' + staff.slm_StaffNameTH  END OwnerName
                            , CASE WHEN poDelegate.slm_PositionNameABB IS NULL THEN delegate.slm_StaffNameTH 
		                           ELSE poDelegate.slm_PositionNameABB + ' - ' + delegate.slm_StaffNameTH  END DelegateName
                            , CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                                   WHEN poMktOwner.slm_PositionNameABB IS NULL THEN mktowner.slm_StaffNameTH 
	                               ELSE poMktOwner.slm_PositionNameABB + ' - ' + mktowner.slm_StaffNameTH END MarketingOwnerName
                            , CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
                                   WHEN poLastOwner.slm_PositionNameABB IS NULL THEN lastowner.slm_StaffNameTH 
	                               ELSE poLastOwner.slm_PositionNameABB + ' - ' + lastowner.slm_StaffNameTH END LastOwnerName
                            , CASE WHEN ISNULL(createby.slm_StaffNameTH, LEAD.slm_CreatedBy) = LEAD.slm_CreatedBy THEN LEAD.slm_CreatedBy
                                   WHEN poCreateby.slm_PositionNameABB IS NULL THEN createby.slm_StaffNameTH 
	                               ELSE poCreateby.slm_PositionNameABB + ' - ' + createby.slm_StaffNameTH END CreateName
                            , lead.slm_CreatedDate AS CreatedDate
                            , lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag, staff.slm_StaffId
                            , ownerbranch.slm_BranchName AS OwnerBranchName
        	                , Delegatebranch.slm_BranchName AS DelegateBranchName
        	                , CreateBybranch.slm_BranchName AS BranchCreateBranchName, lead.slm_Product_Name AS ProductName, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
                            , lead.slm_CampaignId AS CampaignId, prodinfo.slm_LicenseNo AS LicenseNo, lead.slm_TelNo_1 AS TelNo1, prodinfo.slm_ProvinceRegis AS ProvinceRegis
                            , campaign.slm_Url AS CalculatorUrl, lead.slm_Product_Group_Id AS ProductGroupId, lead.slm_Product_Id AS ProductId
                            , lead.coc_AssignedDate AS CocAssignedDate, lead.coc_LastOwner AS LastOwner
                            , lead.coc_CurrentTeam AS CocTeam, cocstatus.slm_OptionDesc AS CocStatusDesc, lead.coc_Counting AS CocCounting, lead.coc_FlowType AS FlowType
                            , lead.coc_Appno AS AppNo
                        FROM " + SLMDBName + @".DBO.kkslm_tr_lead lead 
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id 
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' AND opt.is_Deleted = 0 
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff delegate on delegate.slm_UserName = lead.slm_Delegate
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id 
        			    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch ownerbranch on lead.slm_Owner_Branch = ownerbranch.slm_BranchCode 
        			    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on lead.slm_Delegate_Branch = Delegatebranch.slm_BranchCode 
        			    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poCreateby on lead.slm_CreatedBy_Position = poCreateby.slm_Position_id 
        			    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch CreateBybranch on lead.slm_CreatedBy_Branch = CreateBybranch.slm_BranchCode 
                        LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poMktOwner ON lead.coc_MarketingOwner_Position = poMktOwner.slm_Position_id
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poLastOwner ON lead.coc_LastOwner_Position = poLastOwner.slm_Position_id
                        LEFT JOIN (
				                    SELECT DISTINCT slm_OptionCode, slm_OptionSubCode, slm_OptionType, slm_OptionDesc
				                    FROM " + SLMDBName + @".dbo.kkslm_ms_option ) cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status'   
                        WHERE lead.is_Deleted = 0 AND lead.coc_IsCOC = 1 AND lead.coc_LastOwner IS NOT NULL {0} ) A 
                        {1}
                        ORDER BY CASE WHEN A.FlowType IS NULL THEN '0' 
                                        WHEN A.FlowType = 'F' THEN '0'
                                        WHEN A.FlowType = 'R' THEN '1' END DESC, A.CreatedDate DESC ";

            string whr = "";

            whr += (string.IsNullOrEmpty(data.TicketId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ticketId LIKE '%" + data.TicketId + "%' ");
            whr += (string.IsNullOrEmpty(data.Firstname) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Name LIKE @firstname ");
            whr += (string.IsNullOrEmpty(data.Lastname) ? "" : (whr == "" ? "" : " AND ") + " info.slm_LastName LIKE @lastname ");
            whr += (string.IsNullOrEmpty(data.CitizenId) ? "" : (whr == "" ? "" : " AND ") + " info.slm_CitizenId LIKE '%" + data.CitizenId + "%' ");
            whr += (string.IsNullOrEmpty(data.CampaignId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CampaignId = '" + data.CampaignId + "' ");
            whr += (string.IsNullOrEmpty(data.ChannelId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ChannelId = '" + data.ChannelId + "' ");
            whr += (string.IsNullOrEmpty(data.OwnerUsername) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner = '" + data.OwnerUsername + "' ");      //Owner Lead
            whr += (createDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_CreatedDate) = '" + createDate + "' ");
            whr += (assignDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_AssignedDate) = '" + assignDate + "' ");
            whr += (string.IsNullOrEmpty(data.OwnerBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner_Branch = '" + data.OwnerBranch + "' ");           //Owner Branch
            whr += (string.IsNullOrEmpty(data.DelegateBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate_Branch = '" + data.DelegateBranch + "' ");  //Delegate Branch
            whr += (string.IsNullOrEmpty(data.DelegateLead) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate = '" + data.DelegateLead + "' ");             //Delegate Lead
            whr += (string.IsNullOrEmpty(data.CreateByBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy_Branch = '" + data.CreateByBranch + "' "); //CreateBy Branch
            whr += (string.IsNullOrEmpty(data.CreateBy) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy = '" + data.CreateBy + "' ");                    //CreateBy Lead
            whr += (string.IsNullOrEmpty(data.StatusList) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Status IN (" + data.StatusList + ") ");

            //COC
            whr += (cocAssignedDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.coc_AssignedDate) = '" + cocAssignedDate + "' ");
            whr += (string.IsNullOrEmpty(data.MarketingOwner) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_MarketingOwner = '" + data.MarketingOwner + "' ");
            whr += (recursiveList == "" ? "" : (whr == "" ? "" : " AND ") + " lead.coc_LastOwner IN (" + recursiveList + ") ");
            whr += (string.IsNullOrEmpty(data.CocTeam) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_CurrentTeam = '" + data.CocTeam + "' ");
            whr += (string.IsNullOrEmpty(data.CocStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_Status = '" + data.CocStatus + "' ");
            whr += (string.IsNullOrEmpty(data.CocSubStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_SubStatus = '" + data.CocSubStatus + "' ");

            whr = whr == "" ? "" : " AND " + whr;

            //Where ในตำแหน่ง {1}
            string whr1 = "";
            if (!string.IsNullOrEmpty(data.LastOwner)) whr1 = " WHERE A.LastOwner = '" + data.LastOwner + "' ";

            sql = string.Format(sql, whr, whr1);

            object[] param = new object[] 
            { 
                new SqlParameter("@firstname", "%" + data.Firstname + "%"),
                new SqlParameter("@lastname", "%" + data.Lastname + "%")
            };

            return slmdb.ExecuteStoreQuery<SearchLeadData>(sql, param).ToList();
        }

        private static List<SearchLeadData> DoSearchLeadDataByManagerOper(SearchLeadCondition data)
        {
            string sql = "";
            SLMDBEntities slmdb = new SLMDBEntities();

            string createDate = data.CreatedDate.Year != 1 ? data.CreatedDate.Year + data.CreatedDate.ToString("-MM-dd") : string.Empty;
            string assignDate = data.AssignedDate.Year != 1 ? data.AssignedDate.Year + data.AssignedDate.ToString("-MM-dd") : string.Empty;
            string cocAssignedDate = data.CocAssignedDate.Year != 1 ? data.CocAssignedDate.Year + data.CocAssignedDate.ToString("-MM-dd") : string.Empty;

            sql = @"SELECT A.* FROM (
                        SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc
                            , CASE WHEN poOwner.slm_PositionNameABB IS NULL THEN staff.slm_StaffNameTH
	                               ELSE poOwner.slm_PositionNameABB + ' - ' + staff.slm_StaffNameTH  END OwnerName
                            , CASE WHEN poDelegate.slm_PositionNameABB IS NULL THEN delegate.slm_StaffNameTH 
		                           ELSE poDelegate.slm_PositionNameABB + ' - ' + delegate.slm_StaffNameTH  END DelegateName
                            , CASE WHEN ISNULL(createby.slm_StaffNameTH, LEAD.slm_CreatedBy) = LEAD.slm_CreatedBy THEN LEAD.slm_CreatedBy
                                   WHEN poCreateby.slm_PositionNameABB IS NULL THEN createby.slm_StaffNameTH 
	                               ELSE poCreateby.slm_PositionNameABB + ' - ' + createby.slm_StaffNameTH END CreateName
                            , CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                                   WHEN poMktOwner.slm_PositionNameABB IS NULL THEN mktowner.slm_StaffNameTH 
	                               ELSE poMktOwner.slm_PositionNameABB + ' - ' + mktowner.slm_StaffNameTH END MarketingOwnerName
                            , CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
                                   WHEN poLastOwner.slm_PositionNameABB IS NULL THEN lastowner.slm_StaffNameTH 
	                               ELSE poLastOwner.slm_PositionNameABB + ' - ' + lastowner.slm_StaffNameTH END LastOwnerName
                            , lead.slm_CreatedDate AS CreatedDate
                            , lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag, staff.slm_StaffId
                            , ownerbranch.slm_BranchName AS OwnerBranchName
        	                , Delegatebranch.slm_BranchName AS DelegateBranchName
        	                , CreateBybranch.slm_BranchName AS BranchCreateBranchName, lead.slm_Product_Name AS ProductName, ISNULL(MP.HasADAMUrl, 0) AS HasAdamUrl
                            , lead.slm_CampaignId AS CampaignId, prodinfo.slm_LicenseNo AS LicenseNo, lead.slm_TelNo_1 AS TelNo1, prodinfo.slm_ProvinceRegis AS ProvinceRegis
                            , campaign.slm_Url AS CalculatorUrl, lead.slm_Product_Group_Id AS ProductGroupId, lead.slm_Product_Id AS ProductId
                            , lead.coc_AssignedDate AS CocAssignedDate, lead.coc_LastOwner AS LastOwner
                            , lead.coc_CurrentTeam AS CocTeam, cocstatus.slm_OptionDesc AS CocStatusDesc, lead.coc_Counting AS CocCounting, lead.coc_FlowType AS FlowType
                            , lead.coc_Appno AS AppNo
                        FROM " + SLMDBName + @".DBO.kkslm_tr_lead lead 
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id 
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' AND opt.is_Deleted = 0 
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff delegate on delegate.slm_UserName = lead.slm_Delegate
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id 
        			    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch ownerbranch on lead.slm_Owner_Branch = ownerbranch.slm_BranchCode 
        			    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on lead.slm_Delegate_Branch = Delegatebranch.slm_BranchCode 
        			    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poCreateby on lead.slm_CreatedBy_Position = poCreateby.slm_Position_id 
        			    LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch CreateBybranch on lead.slm_CreatedBy_Branch = CreateBybranch.slm_BranchCode 
                        LEFT JOIN " + SLMDBName + @".dbo.CMT_MAPPING_PRODUCT MP ON lead.slm_Product_Id = MP.sub_product_id
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prodinfo ON lead.slm_ticketId = prodinfo.slm_TicketId
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poMktOwner ON lead.coc_MarketingOwner_Position = poMktOwner.slm_Position_id
                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poLastOwner ON lead.coc_LastOwner_Position = poLastOwner.slm_Position_id
                        LEFT JOIN (
		                            SELECT DISTINCT slm_OptionCode, slm_OptionSubCode, slm_OptionType, slm_OptionDesc
		                            FROM " + SLMDBName + @".dbo.kkslm_ms_option ) cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status' 
                        WHERE lead.is_Deleted = 0 {0} ) A 
                        ORDER BY CASE WHEN A.FlowType IS NULL THEN '0' 
                                        WHEN A.FlowType = 'F' THEN '0'
                                        WHEN A.FlowType = 'R' THEN '1' END DESC, A.CreatedDate DESC ";

            string whr = "";

            whr += (string.IsNullOrEmpty(data.TicketId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ticketId LIKE '%" + data.TicketId + "%' ");
            whr += (string.IsNullOrEmpty(data.Firstname) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Name LIKE @firstname ");
            whr += (string.IsNullOrEmpty(data.Lastname) ? "" : (whr == "" ? "" : " AND ") + " info.slm_LastName LIKE @lastname ");
            whr += (string.IsNullOrEmpty(data.CitizenId) ? "" : (whr == "" ? "" : " AND ") + " info.slm_CitizenId LIKE '%" + data.CitizenId + "%' ");
            whr += (string.IsNullOrEmpty(data.CampaignId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CampaignId = '" + data.CampaignId + "' ");
            whr += (string.IsNullOrEmpty(data.ChannelId) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ChannelId = '" + data.ChannelId + "' ");
            whr += (string.IsNullOrEmpty(data.OwnerUsername) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner = '" + data.OwnerUsername + "' ");      //Owner Lead
            whr += (createDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_CreatedDate) = '" + createDate + "' ");
            whr += (assignDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.slm_AssignedDate) = '" + assignDate + "' ");
            whr += (string.IsNullOrEmpty(data.OwnerBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Owner_Branch = '" + data.OwnerBranch + "' ");           //Owner Branch
            whr += (string.IsNullOrEmpty(data.DelegateBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate_Branch = '" + data.DelegateBranch + "' ");  //Delegate Branch
            whr += (string.IsNullOrEmpty(data.DelegateLead) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Delegate = '" + data.DelegateLead + "' ");             //Delegate Lead
            whr += (string.IsNullOrEmpty(data.CreateByBranch) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy_Branch = '" + data.CreateByBranch + "' "); //CreateBy Branch
            whr += (string.IsNullOrEmpty(data.CreateBy) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_CreatedBy = '" + data.CreateBy + "' ");                    //CreateBy Lead
            whr += (string.IsNullOrEmpty(data.StatusList) ? "" : (whr == "" ? "" : " AND ") + " lead.slm_Status IN (" + data.StatusList + ") ");

            //COC
            whr += (cocAssignedDate == "" ? "" : (whr == "" ? "" : " AND ") + " CONVERT(DATE, lead.coc_AssignedDate) = '" + cocAssignedDate + "' ");
            whr += (string.IsNullOrEmpty(data.MarketingOwner) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_MarketingOwner = '" + data.MarketingOwner + "' ");
            whr += (string.IsNullOrEmpty(data.LastOwner) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_LastOwner = '" + data.LastOwner + "' ");
            whr += (string.IsNullOrEmpty(data.CocTeam) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_CurrentTeam = '" + data.CocTeam + "' ");
            whr += (string.IsNullOrEmpty(data.CocStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_Status = '" + data.CocStatus + "' ");
            whr += (string.IsNullOrEmpty(data.CocSubStatus) ? "" : (whr == "" ? "" : " AND ") + " lead.coc_SubStatus = '" + data.CocSubStatus + "' ");

            whr = whr == "" ? "" : " AND " + whr;

            sql = string.Format(sql, whr);

            object[] param = new object[] 
            { 
                new SqlParameter("@firstname", "%" + data.Firstname + "%"),
                new SqlParameter("@lastname", "%" + data.Lastname + "%")
            };

            return slmdb.ExecuteStoreQuery<SearchLeadData>(sql, param).ToList();
        }

        public static LeadData GetLeadInfo(string ticketId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT lead.slm_ticketId AS TicketId,lead.slm_Name AS name, info.slm_LastName AS LastName,campaign.slm_CampaignName AS CampaignName
	                        ,channel.slm_ChannelDesc AS ChannelDesc,lead.slm_TelNo_1 AS TelNo_1,info.slm_TelNo_2 AS TelNo_2,info.slm_TelNo_3 AS TelNo_3
	                        ,lead.slm_Ext_1 AS Ext_1,info.slm_Ext_2 AS Ext_2, info.slm_Ext_3 AS Ext_3
	                        ,info.slm_Birthdate AS Birthdate,info.slm_CitizenId AS CitizenID,info.slm_Email AS Email,info.slm_Topic AS Topic
	                        ,info.slm_Detail AS Detail,lead.slm_AvailableTime AS AvailableTime,branch.slm_BranchName AS BranchName
                            ,CASE WHEN poOwner.slm_PositionNameABB IS NULL THEN staff.slm_StaffNameTH
	                              ELSE poOwner.slm_PositionNameABB + ' - ' + staff.slm_StaffNameTH  END OwnerName
                            ,CASE WHEN poDelegate.slm_PositionNameABB IS NULL THEN delegate.slm_StaffNameTH 
	                              ELSE poDelegate.slm_PositionNameABB + ' - ' + delegate.slm_StaffNameTH  END DelegateName
                            ,CASE WHEN ISNULL(mktowner.slm_StaffNameTH, lead.coc_MarketingOwner) = lead.coc_MarketingOwner THEN lead.coc_MarketingOwner
                                  WHEN poMktOwner.slm_PositionNameABB IS NULL THEN mktowner.slm_StaffNameTH 
                                  ELSE poMktOwner.slm_PositionNameABB + ' - ' + mktowner.slm_StaffNameTH END MarketingOwnerName
                            ,CASE WHEN ISNULL(lastowner.slm_StaffNameTH, lead.coc_LastOwner) = lead.coc_LastOwner THEN lead.coc_LastOwner
                                  WHEN poLastOwner.slm_PositionNameABB IS NULL THEN lastowner.slm_StaffNameTH 
                                  ELSE poLastOwner.slm_PositionNameABB + ' - ' + lastowner.slm_StaffNameTH END LastOwnerName
                            ,CASE WHEN ISNULL(createby.slm_StaffNameTH, LEAD.slm_CreatedBy) = LEAD.slm_CreatedBy THEN LEAD.slm_CreatedBy
                                   WHEN poCreateby.slm_PositionNameABB IS NULL THEN createby.slm_StaffNameTH 
	                               ELSE poCreateby.slm_PositionNameABB + ' - ' + createby.slm_StaffNameTH END LeadCreateBy
	                        ,lead.slm_CreatedDate AS CreatedDateView
	                        ,lead.slm_AssignedDate AS AssignedDateView,info.slm_AddressNo AS AddressNo,info.slm_BuildingName AS BuildingName,info.slm_Floor AS [Floor]
	                        ,info.slm_Soi AS Soi,info.slm_Street AS Street,tambol.slm_TambolNameTH AS TambolName,amphur.slm_AmphurNameTH AS AmphurName
	                        ,province.slm_ProvinceNameTH AS ProvinceName,info.slm_PostalCode AS PostalCode,info.slm_IsCustomer AS IsCustomer,info.slm_CusCode AS CusCode
	                        ,occ.slm_OccupationNameTH AS OccupationName,info.slm_BaseSalary AS BaseSalary,prod.slm_InterestedProd AS InterestedProd
	                        ,prod.slm_LicenseNo  AS LicenseNo,prod.slm_YearOfCar AS YearOfCar,prod.slm_YearOfCarRegis AS YearOfCarRegis
	                        ,brand.slm_BrandName AS BrandName,prod.slm_CarPrice AS CarPrice,model.slm_FamilyDesc AS ModelName,submodel.slm_SubModel+' : '+ submodel.slm_Description AS SubModelName
	                        ,prod.slm_DownPayment AS DownPayment,prod.slm_DownPercent AS DownPercent,prod.slm_FinanceAmt AS FinanceAmt,prod.slm_PaymentTerm AS PaymentTerm
	                        ,prod.slm_PaymentType AS PaymentType,prod.slm_BalloonAmt AS BalloonAmt,prod.slm_BalloonPercent AS BalloonPercent
	                        ,provinceregis.slm_ProvinceNameTH AS ProvinceRegisName,prod.slm_CoverageDate AS CoverageDate,prod.slm_PlanType AS PlanType
	                        ,module.slm_ModuleNameTH AS AccTypeName,promotion.slm_PromotionName AS PromotionName,prod.slm_AccTerm AS AccTerm,prod.slm_Interest As Interest
	                        ,prod.slm_Invest AS Invest,prod.slm_LoanOd AS LoanOd,prod.slm_LoanOdTerm AS LoanOdTerm,prod.slm_Ebank AS Ebank,prod.slm_Atm AS Atm
	                        ,channelinfo.slm_Company AS Company,info.slm_PathLink AS PathLink,opt.slm_OptionDesc AS StatusName
	                        ,phone1.slm_CreateDate AS ContactLatestDate,phone2.slm_CreateDate AS ContactFirstDate,branchprod.slm_BranchName as Branchprod
                            ,lead.slm_CampaignId AS CampaignId ,lead.slm_Status AS [Status],lead.slm_Owner AS [Owner] ,lead.slm_Delegate As Delegate 
                            ,lead.slm_ChannelId As ChannelId, lead.slm_CreatedDate As LeadCreateDate, channelinfo.slm_branch As Branch 
                            ,info.slm_Occupation As Occupation, info.slm_ContactBranch As ContactBranch, info.slm_Province As Province  
                            ,info.slm_Amphur As Amphur, info.slm_Tambon As Tambon, prod.slm_ProvinceRegis As ProvinceRegis,prod.slm_Brand As Brand
                            ,prod.slm_Model As Model, prod.slm_Submodel As SubModel,prod.slm_AccType As AccType,prod.slm_AccPromotion As AccPromotion 
                            ,prod.slm_CarType As CarType,province.slm_ProvinceCode AS ProvinceCode,tambol.slm_TambolCode As TambolCode
                            ,amphur.slm_AmphurCode As AmphurCode,provinceregis.slm_ProvinceCode As ProvinceRegisCode
                            ,brand.slm_BrandCode AS BrandCode,model.slm_Family As Family,Delegatebranch.slm_BranchName As DelegatebranchName, lead.slm_NoteFlag AS NoteFlag
                            ,PaymentType.slm_PaymentNameTH As PaymentName,PlanBanc.slm_Plan_Banc_T_Desc As PlanBancName, lead.slm_AssignedFlag AS AssignedFlag  
                            ,model.slm_Family AS ModelFamily,CONVERT(VARCHAR, submodel.slm_RedBookNo) AS SubModelCode, PaymentType.slm_PaymentCode AS PaymentTypeCode
                            ,CONVERT(VARCHAR, module.slm_ModuleCode) AS AccTypeCode, slm_PromotionCode AS AccPromotionCode, occ.slm_OccupationCode AS OccupationCode
                            ,OwnerBranch.slm_BranchName AS OwnerBranchName,lead.slm_Owner_Branch AS Owner_Branch
                            ,lead.slm_Delegate_Branch AS Delegate_Branch,lead.slm_CreatedBy_Branch AS CreatedBy_Branch, lead.slm_DealerCode AS DealerCode, lead.slm_DealerName AS DealerName 
                            ,lead.coc_CurrentTeam AS CocTeam, cocstatus.slm_OptionDesc AS CocStatusDesc, lead.coc_AssignedDate AS CocAssignedDate
                        FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff createby on createby.slm_UserName = lead.slm_CreatedBy
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poCreateby on lead.slm_CreatedBy_Position = poCreateby.slm_Position_id 
	                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo prod ON prod.slm_TicketId = lead.slm_ticketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poOwner on lead.slm_Owner_Position = poOwner.slm_Position_id 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' AND opt.is_Deleted = '0' 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch branch ON branch.slm_BranchCode = info.slm_ContactBranch 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff delegate ON lead.slm_Delegate = delegate.slm_UserName  
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poDelegate on lead.slm_Delegate_Position = poDelegate.slm_Position_id 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_tambol tambol ON tambol.slm_TambolId = info.slm_Tambon 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_amphur amphur ON amphur.slm_AmphurId = info.slm_Amphur
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_province province ON province.slm_ProvinceId = info.slm_Province 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_occupation occ ON occ.slm_OccupationId = info.slm_Occupation 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_brand brand ON brand.slm_BrandId = prod.slm_Brand 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_model model ON model.slm_ModelId = prod.slm_Model 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_submodel submodel ON submodel.slm_SubModelId = prod.slm_Submodel 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_province provinceregis ON provinceregis.slm_ProvinceId = prod.slm_ProvinceRegis 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_module module ON module.slm_ModuleId = prod.slm_AccType 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_promotion promotion ON promotion.slm_PromotionId = prod.slm_AccPromotion 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_channelinfo channelinfo on channelinfo.slm_TicketId = lead.slm_ticketId 
                                LEFT JOIN (SELECT TOP 1 slm_CreateDate,slm_TicketId FROM " + SLMDBName + @".DBO.kkslm_phone_call WHERE slm_TicketId = '" + ticketId + @"' ORDER BY slm_CreateDate DESC) AS phone1 
		                        ON phone1.slm_TicketId = LEAD.slm_ticketId 
	                        LEFT JOIN (SELECT TOP 1 slm_CreateDate,slm_TicketId FROM " + SLMDBName + @".DBO.kkslm_phone_call WHERE slm_TicketId = '" + ticketId + @"' ORDER BY slm_CreateDate ASC) AS phone2
		                        ON phone2.slm_TicketId = LEAD.slm_ticketId 
					        LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch branchProd on branchProd.slm_BranchCode = channelinfo.slm_branch 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch Delegatebranch on Delegatebranch.slm_BranchCode = lead.slm_Delegate_Branch 
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_paymenttype PaymentType on PaymentType.slm_PaymentId = prod.slm_PaymentType
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_plan_banc PlanBanc on PlanBanc.slm_Plan_Banc_Code = prod.slm_PlanType   
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_branch OwnerBranch on OwnerBranch.slm_BranchCode = lead.slm_Owner_Branch
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff mktowner ON lead.coc_MarketingOwner = mktowner.slm_EmpCode
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poMktOwner ON lead.coc_MarketingOwner_Position = poMktOwner.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff lastowner ON lead.coc_LastOwner = lastowner.slm_EmpCode
                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_position poLastOwner ON lead.coc_LastOwner_Position = poLastOwner.slm_Position_id
                            LEFT JOIN (
			                            SELECT DISTINCT slm_OptionCode, slm_OptionSubCode, slm_OptionType, slm_OptionDesc
			                            FROM " + SLMDBName + @".dbo.kkslm_ms_option ) cocstatus ON lead.coc_Status = cocstatus.slm_OptionCode AND ISNULL(lead.coc_SubStatus, '0123456789') = ISNULL(cocstatus.slm_OptionSubCode, '0123456789') AND cocstatus.slm_OptionType = 'coc_status' ";
            
            string whr = " lead.is_Deleted = 0 ";

            whr += (ticketId == "" ? "" : (whr == "" ? "" : " AND ") + " lead.slm_ticketId = '" + ticketId + "' ");

            sql += (whr == "" ? "" : " WHERE " + whr);
            sql += " ORDER BY lead.slm_ticketId";

            return slmdb.ExecuteStoreQuery<LeadData>(sql).FirstOrDefault();
        }

        //Reference: Tab004
        public static List<CampaignWSData> GetCampaignFinalList(string ticketId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT FINAL.slm_CampaignId AS CampaignId, FINAL.slm_CampaignName AS CampaignName, CAM.slm_Offer + ' : ' + CAM.slm_Criteria AS CampaignDetail,
	                            FINAL.slm_CreatedDate AS CreatedDate,
                                CASE WHEN STAFF.slm_StaffNameTH IS NULL THEN FINAL.slm_CreatedBy 
		                             WHEN POS.slm_PositionNameAbb IS NULL THEN STAFF.slm_StaffNameTH
		                             ELSE POS.slm_PositionNameAbb + ' - ' + STAFF.slm_StaffNameTH END CreatedByName,
	                            FINAL.slm_CampaignFinalId AS CampaignFinalId, FINAL.slm_TicketId
                            FROM " + SLMDBName + @".dbo.kkslm_tr_campaignfinal AS FINAL
	                        LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff AS STAFF ON STAFF.slm_UserName = FINAL.slm_CreatedBy 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position POS ON FINAL.slm_CreatedBy_Position = POS.slm_Position_id 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign CAM ON FINAL.slm_CampaignId = CAM.slm_CampaignId
                            WHERE FINAL.slm_TicketId = '" + ticketId + @"'
                            ORDER BY FINAL.slm_CreatedDate ASC";

            return slmdb.ExecuteStoreQuery<CampaignWSData>(sql).ToList();
        }

        //Reference: Tab005
        public static List<SearchLeadData> SearchExistingLead(string citizenId, string telNo1)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT lead.slm_ticketId AS TicketId, lead.slm_Counting AS Counting, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, campaign.slm_CampaignName AS CampaignName, channel.slm_ChannelDesc AS ChannelDesc, 
                            CASE WHEN POS.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
                                 ELSE POS.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS OwnerName, 
                            lead.slm_CreatedDate AS CreatedDate, lead.slm_AssignedDate AS AssignedDate, lead.slm_NoteFlag AS NoteFlag
                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign campaign ON lead.slm_CampaignId = campaign.slm_CampaignId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel channel ON lead.slm_ChannelId = channel.slm_ChannelId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON lead.slm_Owner = staff.slm_UserName  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON lead.slm_Owner_Position = pos.slm_Position_id
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON lead.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' ";

            if (!string.IsNullOrEmpty(citizenId))
                sql += " WHERE (info.slm_CitizenId = '" + citizenId + "' OR lead.slm_TelNo_1 = '" + telNo1 + "') AND lead.is_Deleted = 0 ";
            else
                sql += " WHERE lead.slm_TelNo_1 = '" + telNo1 + "' AND lead.is_Deleted = 0 ";

            sql += " ORDER BY lead.slm_CreatedDate DESC ";

            return slmdb.ExecuteStoreQuery<SearchLeadData>(sql).ToList();
        }

        //Reference : Tab006
        public static List<ExistingProductData> SearchExistingProduct(string citizenId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            if (!string.IsNullOrEmpty(citizenId))
            {
                return slmdb.kkslm_existing_product.Where(p => p.slm_CitizenId == citizenId).AsEnumerable().Select((p, index) => new ExistingProductData
                {
                    No = (index + 1).ToString(),
                    CitizenId = p.slm_CitizenId,
                    ProductGroup = p.slm_ProductGroup,
                    ProductName = p.slm_ProductName,
                    Grade = p.slm_Grade,
                    ContactNo = p.slm_ContactNo,
                    StartDate = p.slm_StartDate,
                    EndDate = p.slm_EndDate,
                    PaymentTerm = p.slm_PaymentTerm,
                    Status = p.slm_Status
                }).ToList();
            }
            else
                return new List<ExistingProductData>();
        }

        //Reference : Tab007
        public static List<OwnerLoggingData> SearchOwnerLogging(string ticketId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
//            string sql = @" SELECT act.slm_CreatedDate AS CreatedDate, lead.slm_ticketId AS TicketId, lead.slm_Name AS Firstname, info.slm_CitizenId AS CitizenId, 
//                            info.slm_LastName AS LastName, optOld.slm_OptionDesc AS OldStatusDesc, staffOld.slm_StaffNameTH AS OldOwnerName,
//                            optNew.slm_OptionDesc AS NewStatusDesc, staffNew.slm_StaffNameTH AS NewOwnerName,DelegateOld.slm_StaffNameTH AS OldDelegateName,
//                            DelegateNew.slm_StaffNameTH AS NewDelegateName,
//                            CASE WHEN ACT.slm_Type = '01' THEN 'System Assign' 
//                                 WHEN ACT.slm_Type = '02' THEN 'Change Status'
//                                 WHEN ACT.slm_Type = '03' THEN 'Delegate' 
//                                 WHEN ACT.slm_Type = '04' THEN 'Transfer' 
//                                 WHEN ACT.slm_Type = '05' THEN 'User Assign' 
//                                 WHEN ACT.slm_Type = '06' THEN 'Consulidate' 
//                                 WHEN ACT.slm_Type = '07' THEN 'Reset Owner' 
//                                 WHEN ACT.slm_Type = '08' THEN 'Update Owner' ELSE '' END Action ,
//	                        ISNULL(CreateBy.slm_StaffNameTH, act.slm_CreatedBy) as CreateBy  
//                            FROM " + SLMDBName + @".dbo.kkslm_tr_activity act
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option optOld ON act.slm_OldStatus = optOld.slm_OptionCode AND optOld.slm_OptionType = 'lead status' AND optOld.is_Deleted = '0'
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option optNew ON act.slm_NewStatus = optNew.slm_OptionCode AND optNew.slm_OptionType = 'lead status' AND optNew.is_Deleted = '0'
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffOld ON act.slm_OldOwner = staffOld.slm_UserName
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffNew ON act.slm_NewOwner = staffNew.slm_UserName
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_Lead lead ON lead.slm_ticketId = act.slm_TicketId
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateOld ON act.slm_OldDelegate = DelegateOld.slm_UserName
//                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateNew ON act.slm_NewDelegate = DelegateNew.slm_UserName
//                            LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff CreateBy on CreateBy.slm_UserName = act.slm_CreatedBy 
//                            WHERE lead.slm_TicketId = '" + ticketId + @"' AND act.is_Deleted = '0' AND act.slm_WorkId IS NULL 
//                            ORDER BY act.slm_CreatedDate DESC ";
                        string sql = @"SELECT Z.* FROM (

                                SELECT act.slm_CreatedDate AS CreatedDate, lead.slm_ticketId AS TicketId, lead.slm_Name AS Firstname, info.slm_CitizenId AS CitizenId, 
                                    info.slm_LastName AS LastName, optOld.slm_OptionDesc AS OldStatusDesc 
                                    ,CASE WHEN posoldowner.slm_PositionNameAbb IS NULL THEN staffOld.slm_StaffNameTH
                                            ELSE posoldowner.slm_PositionNameAbb + ' - ' + staffOld.slm_StaffNameTH END AS OldOwnerName
                                    ,CASE WHEN posnewowner.slm_PositionNameAbb IS NULL THEN staffNew.slm_StaffNameTH
                                            ELSE posnewowner.slm_PositionNameAbb + ' - ' + staffNew.slm_StaffNameTH END AS NewOwnerName
                                    ,CASE WHEN posolddelegate.slm_PositionNameAbb IS NULL THEN DelegateOld.slm_StaffNameTH
                                            ELSE posolddelegate.slm_PositionNameAbb + ' - ' + DelegateOld.slm_StaffNameTH END AS OldDelegateName
                                    ,CASE WHEN posnewdelegate.slm_PositionNameAbb IS NULL THEN DelegateNew.slm_StaffNameTH
                                            ELSE posnewdelegate.slm_PositionNameAbb + ' - ' + DelegateNew.slm_StaffNameTH END AS NewDelegateName
                                    ,CASE WHEN ISNULL(CreateBy.slm_StaffNameTH, act.slm_CreatedBy) = act.slm_CreatedBy THEN act.slm_CreatedBy
                                            WHEN poscreateby.slm_PositionNameAbb IS NULL THEN CreateBy.slm_StaffNameTH
                                            ELSE poscreateby.slm_PositionNameAbb + ' - ' + CreateBy.slm_StaffNameTH END AS CreateBy
                                    ,optNew.slm_OptionDesc AS NewStatusDesc 
                                    ,CASE WHEN ACT.slm_Type = '01' THEN 'System Assign' 
                                            WHEN ACT.slm_Type = '02' THEN 'Change Status'
                                            WHEN ACT.slm_Type = '03' THEN 'Delegate' 
                                            WHEN ACT.slm_Type = '04' THEN 'Transfer' 
                                            WHEN ACT.slm_Type = '05' THEN 'User Assign' 
                                            WHEN ACT.slm_Type = '06' THEN 'Consulidate' 
                                            WHEN ACT.slm_Type = '07' THEN 'Reset Owner' 
                                            WHEN ACT.slm_Type = '08' THEN 'Update Owner' 
                                            WHEN ACT.slm_Type = '09' THEN 'EOD Update Current' 
                                            WHEN ACT.slm_Type = '10' THEN 'Change Owner' 
                                            WHEN ACT.slm_Type = '11' THEN 'EOD History Logs' 
                                            WHEN ACT.slm_Type = '12' THEN 'Error Assign' 
                                            ELSE '' END Action
	                                ,ISNULL(ACT.slm_SystemAction, 'SLM') AS SystemAction
                                FROM " + SLMDBName + @".dbo.kkslm_tr_activity act
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option optOld ON act.slm_OldStatus = optOld.slm_OptionCode AND optOld.slm_OptionType = 'lead status'
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option optNew ON act.slm_NewStatus = optNew.slm_OptionCode AND optNew.slm_OptionType = 'lead status'
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffOld ON act.slm_OldOwner = staffOld.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posoldowner ON act.slm_OldOwner_Position = posoldowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffNew ON act.slm_NewOwner = staffNew.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posnewowner ON act.slm_NewOwner_Position = posnewowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_Lead lead ON lead.slm_ticketId = act.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateOld ON act.slm_OldDelegate = DelegateOld.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posolddelegate ON act.slm_OldDelegate_Position = posolddelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateNew ON act.slm_NewDelegate = DelegateNew.slm_UserName
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posnewdelegate ON act.slm_NewDelegate_Position = posnewdelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff CreateBy on CreateBy.slm_UserName = act.slm_CreatedBy 
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poscreateby ON act.slm_CreatedBy_Position = poscreateby.slm_Position_id
                                WHERE lead.slm_TicketId = '" + ticketId + @"' AND act.is_Deleted = '0' AND act.slm_WorkId IS NULL   AND ACT.coc_Team IS NULL
                                UNION ALL
                                SELECT act.slm_CreatedDate AS CreatedDate, lead.slm_ticketId AS TicketId, lead.slm_Name AS Firstname, info.slm_CitizenId AS CitizenId, 
                                    info.slm_LastName AS LastName, optOld.slm_OptionDesc AS OldStatusDesc
                                    ,CASE WHEN posoldowner.slm_PositionNameAbb IS NULL THEN staffOld.slm_StaffNameTH
                                          ELSE posoldowner.slm_PositionNameAbb + ' - ' + staffOld.slm_StaffNameTH END AS OldOwnerName
                                    ,CASE WHEN posnewowner.slm_PositionNameAbb IS NULL THEN staffNew.slm_StaffNameTH
                                          ELSE posnewowner.slm_PositionNameAbb + ' - ' + staffNew.slm_StaffNameTH END AS NewOwnerName
                                    ,CASE WHEN posolddelegate.slm_PositionNameAbb IS NULL THEN DelegateOld.slm_StaffNameTH
                                          ELSE posolddelegate.slm_PositionNameAbb + ' - ' + DelegateOld.slm_StaffNameTH END AS OldDelegateName
                                    ,CASE WHEN posnewdelegate.slm_PositionNameAbb IS NULL THEN DelegateNew.slm_StaffNameTH
                                          ELSE posnewdelegate.slm_PositionNameAbb + ' - ' + DelegateNew.slm_StaffNameTH END AS NewDelegateName
                                    ,CASE WHEN ISNULL(CreateBy.slm_StaffNameTH, act.slm_CreatedBy) = act.slm_CreatedBy THEN act.slm_CreatedBy
                                          WHEN poscreateby.slm_PositionNameAbb IS NULL THEN CreateBy.slm_StaffNameTH
                                          ELSE poscreateby.slm_PositionNameAbb + ' - ' + CreateBy.slm_StaffNameTH END AS CreateBy
                                    ,optNew.slm_OptionDesc AS NewStatusDesc 
                                    ,CASE WHEN ACT.slm_Type = '01' THEN 'System Assign' 
                                          WHEN ACT.slm_Type = '02' THEN 'Change Status'
                                          WHEN ACT.slm_Type = '03' THEN 'Delegate' 
                                          WHEN ACT.slm_Type = '04' THEN 'Transfer' 
                                          WHEN ACT.slm_Type = '05' THEN 'User Assign' 
                                          WHEN ACT.slm_Type = '06' THEN 'Consulidate' 
                                          WHEN ACT.slm_Type = '07' THEN 'Reset Owner' 
                                          WHEN ACT.slm_Type = '08' THEN 'Update Owner' 
                                          WHEN ACT.slm_Type = '09' THEN 'EOD Update Current' 
                                          WHEN ACT.slm_Type = '10' THEN 'Change Owner' 
                                          WHEN ACT.slm_Type = '11' THEN 'EOD History Logs' 
                                          WHEN ACT.slm_Type = '12' THEN 'Error Assign'  
                                          ELSE '' END Action
	                                ,ISNULL(ACT.slm_SystemAction, 'SLM') AS SystemAction   
                                FROM " + SLMDBName + @".dbo.kkslm_tr_activity act
                                LEFT JOIN (
		                                    SELECT DISTINCT slm_OptionCode, slm_OptionSubCode, slm_OptionType, slm_OptionDesc
		                                    FROM " + SLMDBName + @".dbo.kkslm_ms_option ) optOld ON act.slm_OldStatus = optOld.slm_OptionCode AND ISNULL(ACT.slm_OldSubStatus,'0123456789') = ISNULL(optOld.slm_OptionSubCode,'0123456789') AND optOld.slm_OptionType = 'coc_status' 
                                LEFT JOIN (
		                                    SELECT DISTINCT slm_OptionCode, slm_OptionSubCode, slm_OptionType, slm_OptionDesc
		                                    FROM " + SLMDBName + @".dbo.kkslm_ms_option ) optNew ON act.slm_NewStatus = optNew.slm_OptionCode AND ISNULL(ACT.slm_NewSubStatus,'0123456789') = ISNULL(optNew.slm_OptionSubCode,'0123456789') AND optNew.slm_OptionType = 'coc_status'
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffOld ON act.slm_OldOwner = staffOld.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posoldowner ON act.slm_OldOwner_Position = posoldowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staffNew ON act.slm_NewOwner = staffNew.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posnewowner ON act.slm_NewOwner_Position = posnewowner.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_Lead lead ON lead.slm_ticketId = act.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateOld ON act.slm_OldDelegate = DelegateOld.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posolddelegate ON act.slm_OldDelegate_Position = posolddelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff DelegateNew ON act.slm_NewDelegate = DelegateNew.slm_EmpCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posnewdelegate ON act.slm_NewDelegate_Position = posnewdelegate.slm_Position_id
                                LEFT JOIN " + SLMDBName + @".DBO.kkslm_ms_staff CreateBy on CreateBy.slm_UserName = act.slm_CreatedBy 
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poscreateby ON act.slm_CreatedBy_Position = poscreateby.slm_Position_id 
                                WHERE lead.slm_TicketId = '" + ticketId + @"' AND act.is_Deleted = '0' AND act.slm_WorkId IS NULL AND ACT.coc_Team IS NOT NULL) AS Z
                                ORDER BY Z.CreatedDate DESC
		                            ";
            return slmdb.ExecuteStoreQuery<OwnerLoggingData>(sql).ToList();
        }

        //Reference : Tab008
        public static List<PhoneCallHistoryData> SearchPhoneCallHistory(string citizenId, string telNo1)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT phone.slm_CreateDate AS CreatedDate, lead.slm_ticketId AS TicketId, lead.slm_Name AS Firstname, opt.slm_OptionDesc AS StatusDesc, 
                            info.slm_CitizenId AS CitizenId, info.slm_LastName AS LastName, phone.slm_ContactPhone AS ContactPhone, phone.slm_ContactDetail AS ContactDetail
                            ,CASE WHEN posowner.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
	                              ELSE posowner.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS OwnerName
                            ,CASE WHEN ISNULL(staff2.slm_StaffNameTH, phone.slm_CreateBy) = phone.slm_CreateBy THEN phone.slm_CreateBy
	                              WHEN poscreateby.slm_PositionNameAbb IS NULL THEN staff2.slm_StaffNameTH
	                              ELSE poscreateby.slm_PositionNameAbb + ' - ' + staff2.slm_StaffNameTH END AS CreatedName
                            , cam.slm_CampaignName AS CampaignName
                            FROM " + SLMDBName + @".dbo.kkslm_phone_call phone
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_tr_lead lead on phone.slm_TicketId = lead.slm_ticketId
                            INNER JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo info ON lead.slm_ticketId = info.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON phone.slm_Owner = staff.slm_UserName  
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position posowner ON phone.slm_Owner_Position = posowner.slm_Position_id 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option opt ON phone.slm_Status = opt.slm_OptionCode AND opt.slm_OptionType = 'lead status' 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_campaign cam ON lead.slm_CampaignId = cam.slm_CampaignId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff2 ON phone.slm_CreateBy = staff2.slm_UserName 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position poscreateby ON phone.slm_CreatedBy_Position = poscreateby.slm_Position_id ";

            if (!string.IsNullOrEmpty(citizenId))
                sql += " WHERE (info.slm_CitizenId = '" + citizenId + "' OR lead.slm_TelNo_1 = '" + telNo1 + @"') AND phone.is_Deleted = '0' ";
            else
                sql += " WHERE lead.slm_TelNo_1 = '" + telNo1 + @"' AND phone.is_Deleted = '0' ";

            sql += " ORDER BY phone.slm_CreateDate DESC";

            return slmdb.ExecuteStoreQuery<PhoneCallHistoryData>(sql).ToList();
        }

        //Reference : Tab009
        public static List<NoteHistoryData> SearchNoteHistory(string ticketId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT note.slm_CreateDate AS CreatedDate, note.slm_TicketId AS TicketId
                            ,CASE WHEN ISNULL(staff.slm_StaffNameTH, NOTE.slm_CreateBy) = NOTE.slm_CreateBy THEN NOTE.slm_CreateBy
	                              WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
                                  ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS CreateBy
                            ,note.slm_NoteDetail AS NoteDetail,note.slm_EmailSubject AS EmailSubject, note.slm_SendEmailFlag AS SendEmailFlag
                            FROM " + SLMDBName + @".dbo.kkslm_note note
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff staff ON note.slm_CreateBy = staff.slm_UserName
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON note.slm_CreatedBy_Position = pos.slm_Position_id
                            WHERE note.slm_TicketId = '" + ticketId + @"'
                            ORDER BY note.slm_CreateDate DESC";

            return slmdb.ExecuteStoreQuery<NoteHistoryData>(sql).ToList();
        }

        //Reference : SlmScr015Biz
        public static string GetNumOfUnassignLead(string username)
        {
            //string[] statusCode = { "00", "01" };   //สนใจ, ติดต่อไม่ได้
            //return slmdb.kkslm_tr_lead.Where(p => p.slm_Owner == null && p.is_Deleted == 0 && statusCode.Contains(p.slm_Status)).Count().ToString("#,##0");

            SLMDBEntities slmdb = new SLMDBEntities();

            string sql = @"SELECT COUNT(LEAD.slm_ticketId) AS UnAssignCount
                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead INNER JOIN 
                            (
	                            SELECT DISTINCT SLM_CAMPAIGNID 
	                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff_group 
	                            WHERE is_Deleted = 0 AND slm_StaffId IN (
		                            SELECT slm_StaffId
		                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
		                            WHERE staff.slm_UserName = '" + username + @"')
		                      ) AS Z ON Z.slm_CampaignId = LEAD.slm_CampaignId 
                              WHERE slm_Status IN ('" + COCConstant.StatusCode.Interest + @"','" + COCConstant.StatusCode.NoContact + @"') 
                              AND lead.slm_Owner IS NULL AND LEAD.is_Deleted = 0 ";
            //Back up 24/3/2014
            //            string sql = @"SELECT COUNT(LEAD.slm_ticketId) AS UnAssignCount
            //                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead lead INNER JOIN 
            //                            (
            //		                            SELECT DISTINCT B.SLM_CAMPAIGNID
            //		                            FROM
            //		                            (
            //		                            SELECT SLM_CAMPAIGNID
            //		                            FROM " + SLMDBName + @".DBO.kkslm_ms_group [GROUP] 
            //		                            WHERE slm_GroupId IN (
            //				                            SELECT slm_GroupId 
            //				                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff_group 
            //				                            WHERE is_Deleted = 0 AND slm_StaffId IN (
            //					                            SELECT slm_StaffId
            //					                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
            //					                            WHERE staff.slm_UserName = '" + username + @"'))
            //		                            UNION ALL
            //		                            SELECT SLM_CAMPAIGNID
            //		                            FROM " + SLMDBName + @".DBO.kkslm_ms_group [GROUP] 
            //		                            WHERE slm_StaffId IN (
            //			                            SELECT slm_StaffId
            //			                            FROM " + SLMDBName + @".DBO.kkslm_ms_staff staff 
            //			                            WHERE staff.slm_UserName = '" + username + @"')
            //		                            ) AS B) AS Z ON Z.slm_CampaignId = LEAD.slm_CampaignId 
            //                              WHERE slm_Status IN ('" + SLMConstant.StatusCode.Interest + @"','" + SLMConstant.StatusCode.NoContact + @"') AND lead.slm_Owner IS NULL AND LEAD.is_Deleted = 0 
            //                            ";
            return slmdb.ExecuteStoreQuery<int>(sql).Select(p => p.ToString("#,##0")).FirstOrDefault();
        }

        public LeadDataForAdam GetLeadDataForAdam(string ticketId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT LEAD.slm_ticketId AS TicketId, LEAD.slm_Name AS Firstname, LEAD.slm_TelNo_1 AS TelNo1, LEAD.slm_CampaignId AS Campaign, LEAD.slm_Ext_1 AS ExtNo1, LEAD.slm_AvailableTime AS AvailableTime, LEAD.slm_Owner AS TelesaleName, LEAD.slm_Status AS Status
                            , LEAD.slm_Product_Group_Id AS ProductGroupId, LEAD.slm_Product_Id AS ProductId
                            , CUS.slm_LastName AS Lastname, CUS.slm_Email AS Email, CUS.slm_TelNo_2 AS TelNo2, CUS.slm_TelNo_3 AS TelNo3, CUS.slm_Ext_2 AS ExtNo2
                            , CUS.slm_Ext_3 AS ExtNo3, CUS.slm_BuildingName AS BuildingName, CUS.slm_AddressNo AS AddrNo, CUS.slm_Floor AS Floor, CUS.slm_Soi AS Soi
                            , CUS.slm_Street AS Street, CUS.slm_Tambon AS Tambol, TAM.slm_TambolCode AS TambolCode, CUS.slm_Amphur AS Amphur, AM.slm_AmphurCode AS AmphurCode, CUS.slm_Province AS Province, PRO.slm_ProvinceCode AS ProvinceCode, CUS.slm_PostalCode AS PostalCode
                            , CUS.slm_Occupation AS Occupation, OCC.slm_OccupationCode AS OccupationCode, CUS.slm_BaseSalary AS BaseSalary, CUS.slm_IsCustomer AS IsCustomer, CUS.slm_CusCode AS CustomerCode
                            , CUS.slm_Birthdate AS DateOfBirth, CUS.slm_CitizenId AS Cid, CUS.slm_Topic AS Topic, CUS.slm_Detail AS Detail, CUS.slm_PathLink AS PathLink
                            , CUS.slm_ContactBranch AS ContactBranch
                            , PROD.slm_InterestedProd AS InterestedProdAndType, PROD.slm_LicenseNo AS LicenseNo, PROD.slm_YearOfCar AS YearOfCar, PROD.slm_YearOfCarRegis AS YearOfCarRegis
                            , PROD.slm_ProvinceRegis AS RegisterProvince, PRO2.slm_ProvinceCode AS RegisterProvinceCode, PROD.slm_Brand AS Brand, PROD.slm_Model AS Model, PROD.slm_Submodel AS Submodel, PROD.slm_DownPayment AS DownPayment
                            , PROD.slm_DownPercent AS DownPercent, PROD.slm_CarPrice AS CarPrice, PROD.slm_CarType AS CarType, PROD.slm_FinanceAmt AS FinanceAmt, PROD.slm_PaymentTerm AS Term
                            , PROD.slm_PaymentType AS PaymentType, PROD.slm_BalloonAmt AS BalloonAmt, PROD.slm_BalloonPercent AS BalloonPercent, PROD.slm_PlanType AS PlanType
                            , PROD.slm_CoverageDate AS CoverageDate, PROD.slm_AccType AS AccType, PROD.slm_AccPromotion AS AccPromotion, PROD.slm_AccTerm AS AccTerm, PROD.slm_Interest AS Interest
                            , PROD.slm_Invest AS Invest, PROD.slm_LoanOd AS LoanOd, PROD.slm_LoanOdTerm AS LoanOdTerm, PROD.slm_Ebank AS SlmBank, PROD.slm_Atm AS SlmAtm
                            , PROD.slm_OtherDetail_1 AS OtherDetail1, PROD.slm_OtherDetail_2 AS OtherDetail2, PROD.slm_OtherDetail_3 AS OtherDetail3, PROD.slm_OtherDetail_4 AS OtherDetail4
                            , CHAN.slm_ChannelId AS ChannelId, CHAN.slm_RequestDate AS RequestDate, CHAN.slm_RequestBy AS CreateUser, CHAN.slm_IPAddress AS Ipaddress, CHAN.slm_Company AS Company
                            , CHAN.slm_Branch AS Branch, CHAN.slm_BranchNo AS BranchNo, CHAN.slm_MachineNo AS MachineNo, CHAN.slm_ClientServiceType AS ClientServiceType
                            , CHAN.slm_DocumentNo AS DocumentNo, CHAN.slm_CommPaidCode AS CommPaidCode, CHAN.slm_Zone AS Zone, CHAN.slm_TransId AS TransId
                            , BRAND.slm_BrandCode AS BrandCode, MODEL.slm_Family AS ModelFamily, CONVERT(VARCHAR, SUBMODEL.slm_RedBookNo) AS SubmodelRedBookNo, PAYTYPE.slm_PaymentCode AS PaymentCode
                            , CONVERT(VARCHAR, ACCTYPE.slm_ModuleCode) AS AccTypeCode, PROMOTE.slm_PromotionCode AS AccPromotionCode, OPT.slm_OptionDesc AS StatusDesc
                            FROM " + SLMDBName + @".dbo.kkslm_tr_lead LEAD
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_cusinfo CUS ON LEAD.slm_ticketId = CUS.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_productinfo PROD ON LEAD.slm_ticketId = PROD.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_tr_channelinfo CHAN ON LEAD.slm_ticketId = CHAN.slm_TicketId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_tambol TAM ON CUS.slm_Tambon = TAM.slm_TambolId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_amphur AM ON CUS.slm_Amphur = AM.slm_AmphurId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_province PRO ON CUS.slm_Province = PRO.slm_ProvinceId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_occupation OCC ON CUS.slm_Occupation = OCC.slm_OccupationId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_province PRO2 ON PROD.slm_ProvinceRegis = PRO2.slm_ProvinceId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_brand BRAND ON PROD.slm_Brand = BRAND.slm_BrandId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_model MODEL ON PROD.slm_Model = MODEL.slm_ModelId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_submodel SUBMODEL ON PROD.slm_Submodel = SUBMODEL.slm_SubModelId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_paymenttype PAYTYPE ON PROD.slm_PaymentType = PAYTYPE.slm_PaymentId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_module ACCTYPE ON PROD.slm_AccType = ACCTYPE.slm_ModuleId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_promotion PROMOTE ON PROD.slm_AccPromotion = PROMOTE.slm_PromotionId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_option OPT ON OPT.slm_OptionCode = LEAD.slm_Status AND OPT.slm_OptionType = 'lead status'
                            WHERE LEAD.slm_ticketId = '" + ticketId + "'";

            return slmdb.ExecuteStoreQuery<LeadDataForAdam>(sql).FirstOrDefault();
        }
    }
}
