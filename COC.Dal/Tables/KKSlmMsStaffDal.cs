using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using COC.Resource;
using COC.Resource.Data;

namespace COC.Dal.Tables
{
    public class KKSlmMsStaffDal
    {
        private static string SLMDBName = COCConstant.SLMDBName;

        public static LoginUserData GetLoginUserData(string username)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            var data = (from staff in slmdb.kkslm_ms_staff
                        join branch in slmdb.kkslm_ms_branch on staff.slm_BranchCode equals branch.slm_BranchCode into branchjoin
                        from branch1 in branchjoin.DefaultIfEmpty()
                        where staff.slm_UserName == username && staff.is_Deleted == 0
                        select new LoginUserData { StaffNameTH = staff.slm_StaffNameTH, BranchName = branch1.slm_BranchName, BranchCode = branch1.slm_BranchCode, StaffId = staff.slm_StaffId }).FirstOrDefault();

            return data;
        }

        public static List<StaffData> GetStaffList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_staff.Where(p => p.is_Deleted == 0).Select(p => new StaffData { UserName = p.slm_UserName, StaffId = p.slm_StaffId, HeadStaffId = p.slm_HeadStaffId, EmpCode = p.slm_EmpCode, StaffTypeId = p.slm_StaffTypeId, CocTeam = p.coc_Team }).ToList();
        }

        public static StaffData GetStaff(string username)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT staff.slm_EmpCode AS EmpCode, staff.slm_StaffTypeId AS StaffTypeId, stafftype.slm_StaffTypeDesc AS StaffTypeDesc
                                    FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                                    LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type stafftype ON staff.slm_StaffTypeId = stafftype.slm_StaffTypeId
                                    WHERE staff.slm_UserName = '" + username + "'";

            return slmdb.ExecuteStoreQuery<StaffData>(sql).FirstOrDefault();
        }

        public static List<ControlListData> GetStaffList(string branchCode)
        {
            //SLMDBEntities slmdb = new SLMDBEntities();
            //decimal[] dec = { COCConstant.StaffType.ITAdministrator };
            //return slmdb.kkslm_ms_staff.Where(p => p.slm_BranchCode == branch && p.is_Deleted == 0 && dec.Contains(p.slm_StaffTypeId) == false).OrderBy(p => p.slm_StaffNameTH).Select(p => new ControlListData { TextField = p.slm_PositionName + " - " + p.slm_StaffNameTH, ValueField = p.slm_UserName }).ToList();

            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
			                            ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS TextField, staff.slm_UserName AS ValueField
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id
                            WHERE staff.slm_BranchCode = '" + branchCode + @"' AND staff.is_Deleted = '0'
                            AND staff.slm_StaffTypeId <> '" + COCConstant.StaffType.ITAdministrator + @"'
                            ORDER BY staff.slm_StaffNameTH ";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public static List<ControlListData> GetStaffListByStaffTypeId(decimal staffTypeId, string recursiveList)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
		                          ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS TextField, staff.slm_EmpCode AS ValueField
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id 
                            WHERE staff.slm_StaffTypeId = '" + staffTypeId + "' AND staff.is_Deleted = 0 ";

            if (recursiveList != "")
                sql += " AND staff.slm_EmpCode IN (" + recursiveList + ") ";

            sql += " ORDER BY staff.slm_StaffNameTH ";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public static List<ControlListData> GetStaffHeadList(string branchCode)
        {
            //SLMDBEntities slmdb = new SLMDBEntities();
            //decimal[] dec = { COCConstant.StaffType.ITAdministrator };
            //return slmdb.kkslm_ms_staff.Where(p => p.slm_BranchCode == branchCode && p.is_Deleted == 0 && dec.Contains(p.slm_StaffTypeId) == false).OrderBy(p => p.slm_StaffNameTH).AsEnumerable().Select(p => new ControlListData { TextField = p.slm_PositionName + " - " + p.slm_StaffNameTH, ValueField = p.slm_StaffId.ToString() }).ToList();

            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
			                           ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS TextField, CONVERT(VARCHAR, staff.slm_StaffId) AS ValueField
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id
                            WHERE staff.slm_BranchCode = '" + branchCode + @"' AND staff.is_Deleted = '0'
                            AND staff.slm_StaffTypeId <> '" + COCConstant.StaffType.ITAdministrator + @"'
                            ORDER BY staff.slm_StaffNameTH ";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public static List<ControlListData> GetTeamList()
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkcoc_ms_team.Where(p => p.is_Deleted == 0 && p.is_ViewMonitoring == true).OrderBy(p => p.coc_TeamId).Select(p => new ControlListData { TextField = p.coc_TeamId, ValueField = p.coc_TeamId }).ToList();
            
            //return slmdb.kkslm_ms_staff.Where(p => p.is_Deleted == 0 && p.coc_Team != null && p.coc_Team != "").Select(p => new ControlListData { TextField = p.coc_Team, ValueField = p.coc_Team }).Distinct().ToList();
        }

        public static List<ControlListData> GetStaffByTeam(string cocTeam)
        {
            //SLMDBEntities slmdb = new SLMDBEntities();
            //if (!string.IsNullOrEmpty(cocTeam))
            //    return slmdb.kkslm_ms_staff.Where(p => p.coc_Team == cocTeam && p.is_Deleted == 0).OrderBy(p => p.slm_StaffNameTH).Select(p => new ControlListData { TextField = p.slm_PositionName + " - " + p.slm_StaffNameTH, ValueField = p.slm_EmpCode }).ToList();
            //else
            //    return new List<ControlListData>();

            //แก้ไข Position แล้ว
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
			                           ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS TextField, staff.slm_EmpCode AS ValueField
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id
                            WHERE staff.coc_Team = '" + cocTeam + @"' AND staff.is_Deleted = '0'
                            ORDER BY staff.slm_StaffNameTH ";

            return slmdb.ExecuteStoreQuery<ControlListData>(sql).ToList();
        }

        public static string GetCurrentStatus(string username)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            var staff = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && (p.slm_StaffTypeId == COCConstant.StaffType.Supervisor || p.slm_StaffTypeId == COCConstant.StaffType.Telesales) && p.is_Deleted == 0).FirstOrDefault();

            if (staff != null)
                return staff.slm_IsActive.ToString();
            else
                return string.Empty;
        }

        public static void SetCurrentStatus(string username, int status)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            var staff = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && (p.slm_StaffTypeId == COCConstant.StaffType.Supervisor || p.slm_StaffTypeId == COCConstant.StaffType.Telesales) && p.is_Deleted == 0).FirstOrDefault();
            if (staff != null)
            {
                staff.slm_IsActive = status;
                slmdb.SaveChanges();
            }
        }

        public static decimal? GetStaffType(string username)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            var staffdata = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_StaffTypeId;
            else
                return null;
        }

        public static int? GetDepartmentId(string username)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            var staffdata = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_DepartmentId;
            else
                return null;
        }

        public static string InsertStaff(StaffDataManagement data, string username)
        {
            try
            {
                SLMDBEntities slmdb = new SLMDBEntities();

                kkslm_ms_staff staff = new kkslm_ms_staff();
                staff.slm_UserName = data.Username;
                staff.slm_EmpCode = data.EmpCode;

                if (!string.IsNullOrEmpty(data.MarketingCode))
                    staff.slm_MarketingCode = data.MarketingCode;

                staff.slm_StaffNameTH = data.StaffNameTH;

                if (!string.IsNullOrEmpty(data.TelNo))
                    staff.slm_TellNo = data.TelNo;

                staff.slm_StaffEmail = data.StaffEmail;

                if (!string.IsNullOrEmpty(data.PositionId))
                    staff.slm_Position_id = int.Parse(data.PositionId);

                if (!string.IsNullOrEmpty(data.Team))
                    staff.slm_Team = data.Team;
                if (data.StaffTypeId != null)
                    staff.slm_StaffTypeId = data.StaffTypeId.Value;
                if (!string.IsNullOrEmpty(data.BranchCode))
                    staff.slm_BranchCode = data.BranchCode;
                if (data.HeadStaffId != null)
                    staff.slm_HeadStaffId = data.HeadStaffId.Value;
                if (data.DepartmentId != null)
                    staff.slm_DepartmentId = data.DepartmentId.Value;

                staff.coc_Team = data.CocTeam;
                staff.slm_CreatedBy = username;
                staff.slm_CreatedDate = DateTime.Now;
                staff.is_Deleted = 0;
                staff.slm_IsActive = 0;
                staff.slm_IsLocked = 0;
                staff.slm_UpdateStatusDate = DateTime.Now;
                staff.slm_UpdateStatusBy = username;

                slmdb.kkslm_ms_staff.AddObject(staff);
                slmdb.SaveChanges();
                return staff.slm_StaffId.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string UpdateStaff(StaffDataManagement data, string username, int flag)
        {
            SLMDBEntities slmdb = new SLMDBEntities();

            var staff = slmdb.kkslm_ms_staff.Where(p => p.slm_StaffId == data.StaffId).FirstOrDefault();
            if (staff != null)
            {
                try
                {
                    staff.slm_UserName = data.Username;
                    staff.slm_EmpCode = data.EmpCode;
                    staff.slm_MarketingCode = data.MarketingCode;
                    staff.slm_StaffNameTH = data.StaffNameTH;
                    staff.slm_TellNo = data.TelNo;
                    staff.slm_StaffEmail = data.StaffEmail;

                    if (!string.IsNullOrEmpty(data.PositionId))
                        staff.slm_Position_id = int.Parse(data.PositionId);
                    else
                        staff.slm_Position_id = null;

                    staff.slm_Team = string.IsNullOrEmpty(data.Team) ? null : data.Team;

                    if (data.StaffTypeId != null)
                        staff.slm_StaffTypeId = data.StaffTypeId.Value;
                    if (!string.IsNullOrEmpty(data.BranchCode))
                        staff.slm_BranchCode = data.BranchCode;
                    if (data.DepartmentId != null)
                        staff.slm_DepartmentId = data.DepartmentId.Value;

                    staff.slm_HeadStaffId = data.HeadStaffId;
                    staff.coc_Team = string.IsNullOrEmpty(data.CocTeam) ? null : data.CocTeam;
                    staff.slm_UpdatedBy = username;
                    staff.slm_UpdatedDate = DateTime.Now;
                    staff.is_Deleted = data.Is_Deleted.Value;
                    if (flag == 1)
                    {
                        staff.slm_UpdateStatusDate = DateTime.Now;
                        staff.slm_UpdateStatusBy = username;
                    }

                    slmdb.SaveChanges();
                    return data.StaffId.ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
                return data.StaffId.ToString();

        }

        public static bool CheckUsernameExist(string username, int? staffid)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            if (staffid == null)
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
                return user != null ? true : false;
            }
            else
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0 && p.slm_StaffId != staffid).FirstOrDefault();
                return user != null ? true : false;
            }
        }

        public static bool CheckEmpCodeExist(string empCode, int? staffid)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            if (staffid == null)
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_EmpCode == empCode && p.is_Deleted == 0).FirstOrDefault();
                return user != null ? true : false;
            }
            else
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_EmpCode == empCode && p.is_Deleted == 0 && p.slm_StaffId != staffid).FirstOrDefault();
                return user != null ? true : false;
            }
        }

        public static bool CheckMarketingCodeExist(string marketingCode, int? staffid)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            if (staffid == null)
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_MarketingCode == marketingCode && p.is_Deleted == 0).FirstOrDefault();
                return user != null ? true : false;
            }
            else
            {
                var user = slmdb.kkslm_ms_staff.Where(p => p.slm_MarketingCode == marketingCode && p.is_Deleted == 0 && p.slm_StaffId != staffid).FirstOrDefault();
                return user != null ? true : false;
            }
        }

        public static StaffDataManagement GetStaff(int staffId)
        {
            SLMDBEntities slmdb = new SLMDBEntities();

            return slmdb.kkslm_ms_staff.Where(p => p.slm_StaffId == staffId).AsEnumerable().Select(p =>
                new StaffDataManagement
                {
                    StaffId = p.slm_StaffId,
                    Username = p.slm_UserName,
                    EmpCode = p.slm_EmpCode,
                    MarketingCode = p.slm_MarketingCode,
                    StaffNameTH = p.slm_StaffNameTH,
                    TelNo = p.slm_TellNo,
                    StaffEmail = p.slm_StaffEmail,
                    StaffTypeId = p.slm_StaffTypeId,
                    Team = p.slm_Team,
                    BranchCode = p.slm_BranchCode,
                    HeadStaffId = p.slm_HeadStaffId,
                    PositionId = p.slm_Position_id.ToString(),
                    PositionName = p.slm_PositionName,
                    DepartmentId = p.slm_DepartmentId,
                    CocTeam = p.coc_Team,
                    Is_Deleted = p.is_Deleted
                }).FirstOrDefault();
        }

        public static StaffData GetStaffInfo(string username)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT staff.slm_UserName AS UserName, staff.slm_StaffNameTH AS StaffNameTH, chan.slm_ChannelId AS ChannelId, branch.slm_BranchName AS BranchName,chan.slm_ChannelDesc AS ChannelDesc 
                                FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type st ON staff.slm_StaffTypeId = st.slm_StaffTypeId AND st.is_Deleted = 0
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch branch ON staff.slm_BranchCode = branch.slm_BranchCode
                                LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_channel chan ON st.slm_ChannelId = chan.slm_ChannelId
                                WHERE staff.slm_UserName = '" + username + "' AND staff.is_Deleted = 0 ";

            return slmdb.ExecuteStoreQuery<StaffData>(sql).FirstOrDefault();
        }

        public static List<StaffDataManagement> SearchStaffList(string username, string branchCode, string empCode, string marketingCode, string staffNameTH, string positionId, string staffTypeId, string team, string departmentId, string cocTeam)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT staff.slm_StaffId AS StaffId, staff.slm_EmpCode AS EmpCode, staff.slm_MarketingCode AS MarketingCode, staff.slm_UserName AS Username, staff.slm_StaffNameTH AS StaffNameTH
                            , pos.slm_PositionNameTH AS PositionName, staff.slm_StaffTypeId AS StaffTypeId, st.slm_StaffTypeDesc AS StaffTypeDesc, staff.slm_Team AS Team, staff.slm_BranchCode AS BranchCode, branch.slm_BranchName AS BranchName
                            , staff.slm_DepartmentId AS DepartmentId, dep.slm_DepartmentName AS DepartmentName, staff.is_Deleted AS Is_Deleted,staff.slm_UpdateStatusDate AS UpdateStatusDate 
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_staff_type st ON staff.slm_StaffTypeId = st.slm_StaffTypeId
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_branch branch ON staff.slm_BranchCode = branch.slm_BranchCode
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_department dep ON staff.slm_DepartmentId = dep.slm_DepartmentId 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id ";

            string whr = "";
            whr += (username == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_UserName LIKE @UserName ");
            whr += (branchCode == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_BranchCode = '" + branchCode + "' ");
            whr += (empCode == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_EmpCode LIKE @EmpCode ");
            whr += (marketingCode == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_MarketingCode LIKE @MarketingCode ");
            whr += (staffNameTH == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_StaffNameTH LIKE @FullName ");
            whr += (positionId == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_Position_id = '" + positionId + "' ");
            whr += (staffTypeId == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_StaffTypeId = '" + staffTypeId + "' ");
            whr += (team == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_Team LIKE @Team ");
            whr += (departmentId == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_DepartmentId = '" + departmentId + "' ");
            whr += (cocTeam == "" ? "" : (whr == "" ? "" : " AND ") + " staff.coc_Team = '" + cocTeam + "' ");

            sql += (whr == "" ? "" : " WHERE " + whr);
            sql += " ORDER BY staff.slm_EmpCode ";

            object[] param = new object[] 
            { 
                new SqlParameter("@UserName", (username != null ? "%" + username + "%" : "")),
                new SqlParameter("@EmpCode", (empCode != null ? "%" + empCode + "%" : "")),
                new SqlParameter("@MarketingCode", (marketingCode != null ? "%" + marketingCode + "%" : "")),
                new SqlParameter("@FullName", (staffNameTH != null ? "%" + staffNameTH + "%" : "")),
                new SqlParameter("@Team", (team != null ? "%" + team + "%" : ""))
            };

            return slmdb.ExecuteStoreQuery<StaffDataManagement>(sql, param).ToList();
        }

        public static string GetEmpCode(string username)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            var staffdata = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username && p.is_Deleted == 0).FirstOrDefault();
            if (staffdata != null)
                return staffdata.slm_EmpCode != null ? staffdata.slm_EmpCode.Trim() : "";
            else
                return "";
        }

        public static List<ForecastReportData> GetDataForForecastReport(string teamList)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT team.coc_TeamId AS Team, ISNULL(team.coc_SLA_Minute_Forcast, 0) AS Sla, ISNULL(A.AmountAvailable, 0) AS AmountOfAvailableStaff 
                            FROM " + SLMDBName + @".dbo.kkcoc_ms_team team
                            LEFT JOIN(
	                            SELECT coc_Team AS Team, COUNT(*) AS AmountAvailable
	                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff 
	                            WHERE is_Deleted = 0 AND slm_IsActive = '1' AND coc_Team IS NOT NULL 
	                            GROUP BY coc_Team) A ON team.coc_TeamId = A.Team
                            WHERE team.is_Deleted = 0 AND team.is_ViewMonitoring = 1
                            AND team.coc_TeamId IN (" + teamList + @")
                            ORDER BY coc_Seq";

            return slmdb.ExecuteStoreQuery<ForecastReportData>(sql).ToList();
        }

        public static List<UserMonitoringData> GetUserMonotoringStaffList(string available, string cocTeam, string teamList)
        {
            //แก้ไข Position แล้ว
            SLMDBEntities slmdb = new SLMDBEntities();
            string sql = @"SELECT staff.coc_Team AS Team
                            , CASE WHEN pos.slm_PositionNameAbb IS NULL THEN staff.slm_StaffNameTH
                                   ELSE pos.slm_PositionNameAbb + ' - ' + staff.slm_StaffNameTH END AS StaffFullname
                            , staff.slm_IsActive AS Active, staff.slm_EmpCode AS EmpCode
                            , 0 AS AmountNewJobNew, 0 AS AmountNewJobOnHand, 0 AS AmountNewJobAll
                            , 0 AS AmountDoneJobForward, 0 AS AmountDoneJobRouteBackCoc, 0 AS AmountDoneJobRouteBackMkt, 0 AS AmountDoneJobAll, 0 AS AmountAllJob
                            FROM " + SLMDBName + @".dbo.kkslm_ms_staff staff 
                            LEFT JOIN " + SLMDBName + @".dbo.kkslm_ms_position pos ON staff.slm_Position_id = pos.slm_Position_id 
                            WHERE staff.is_Deleted = 0 AND staff.coc_Team IS NOT NUll AND RTRIM(LTRIM(staff.coc_Team)) <> '' AND staff.coc_Team <> 'MARKETING' 
                            AND staff.coc_Team IN (" + teamList + ") ";

            string whr = "";
            whr += (available == "" ? "" : (whr == "" ? "" : " AND ") + " staff.slm_IsActive = '" + available + "' ");
            whr += (cocTeam == "" ? "" : (whr == "" ? "" : " AND ") + " staff.coc_Team = '" + cocTeam + "' ");

            sql += (whr == "" ? "" : " AND " + whr);
            sql += " ORDER BY staff.coc_Team, staff.slm_StaffNameTH ";

            return slmdb.ExecuteStoreQuery<UserMonitoringData>(sql).ToList();

        }

        public static void SetStaffStatus(string empCode, int status, string loginUsername)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            var staff = slmdb.kkslm_ms_staff.Where(p => p.slm_EmpCode == empCode).FirstOrDefault();
            if (staff != null)
            {
                DateTime date = DateTime.Now;
                staff.slm_IsActive = status;
                staff.slm_UpdatedDate = date;
                staff.slm_UpdatedBy = loginUsername;

                kkcoc_tr_log_working logwk = new kkcoc_tr_log_working();
                logwk.coc_Actionby = loginUsername;
                logwk.coc_ActionDate = date;
                logwk.coc_UserName = staff.slm_UserName;
                logwk.coc_EmpCode = empCode;
                logwk.coc_EmpName = staff.slm_StaffNameTH;
                logwk.coc_AvailableFlag = (status == 1 ? "Y" : "N");
                slmdb.kkcoc_tr_log_working.AddObject(logwk);

                slmdb.SaveChanges();
            }
        }

        public static decimal? GetStaffStatus(string username)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username).Select(p => p.slm_IsActive).FirstOrDefault();
        }

        public static int? GetPositionId(string username)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == username).Select(p => p.slm_Position_id).FirstOrDefault();
        }

        public static int? GetPositionIdByEmpCode(string empCode)
        {
            SLMDBEntities slmdb = new SLMDBEntities();
            return slmdb.kkslm_ms_staff.Where(p => p.slm_EmpCode == empCode).Select(p => p.slm_Position_id).FirstOrDefault();
        }
    }
}
