using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COC.Resource;
using COC.Resource.Data;
using COC.Dal.Tables;

namespace COC.Biz
{
    public class StaffBiz
    {
        public static LoginUserData GetLoginUserData(string username)
        {
            return KKSlmMsStaffDal.GetLoginUserData(username);
        }

        public static List<StaffData> GetStaffList()
        {
            return KKSlmMsStaffDal.GetStaffList();
        }

        public static StaffData GetStaff(string username)
        {
            return KKSlmMsStaffDal.GetStaff(username);
        }

        public static List<ControlListData> GetStaffList(string branch)
        {
            return KKSlmMsStaffDal.GetStaffList(branch);
        }

        public static List<ControlListData> GetStaffListByStaffTypeId(decimal staffTypeId, string recursiveList)
        {
            return KKSlmMsStaffDal.GetStaffListByStaffTypeId(staffTypeId, recursiveList);
        }

        public static List<ControlListData> GetStaffTyeList()
        {
            return KKSlmMsStaffTypeDal.GetStaffTyeList();
        }

        public static List<ControlListData> GetTeamList()
        {
            return KKSlmMsStaffDal.GetTeamList();
        }

        public static List<ControlListData> GetStaffByTeam(string cocTeam)
        {
            return KKSlmMsStaffDal.GetStaffByTeam(cocTeam);
        }

        public static string GetCurrentStatus(string username)
        {
            return KKSlmMsStaffDal.GetCurrentStatus(username);
        }

        public static void SetCurrentStatus(string username, int status)
        {
            KKSlmMsStaffDal.SetCurrentStatus(username, status);
        }

        public static decimal? GetStaffType(string username)
        {
            return KKSlmMsStaffDal.GetStaffType(username);
        }

        public static StaffData GetStaffInfo(string username)
        {
            return KKSlmMsStaffDal.GetStaffInfo(username);
        }

        public static List<StaffDataManagement> SearchStaffList(string username, string branchCode, string empCode, string marketingCode, string staffNameTH, string positionId
            , string staffTypeId, string team, string departmentId, string cocTeam)
        {
            return KKSlmMsStaffDal.SearchStaffList(username, branchCode, empCode, marketingCode, staffNameTH, positionId, staffTypeId, team, departmentId, cocTeam);
        }

        public static int? GetDepartmentId(string username)
        {
            return KKSlmMsStaffDal.GetDepartmentId(username);
        }

        public static string InsertStaff(StaffDataManagement data, string username)
        {
            return KKSlmMsStaffDal.InsertStaff(data, username);
        }

        public static string UpdateStaff(StaffDataManagement data, string username, int flag)
        {
            return KKSlmMsStaffDal.UpdateStaff(data, username, flag);
        }

        public static bool CheckUsernameExist(string username, int? staffid)
        {
            return KKSlmMsStaffDal.CheckUsernameExist(username, staffid);
        }

        public static bool CheckEmpCodeExist(string empCode, int? staffid)
        {
            return KKSlmMsStaffDal.CheckEmpCodeExist(empCode, staffid);
        }

        public static bool CheckMarketingCodeExist(string marketingCode, int? staffid)
        {
            return KKSlmMsStaffDal.CheckMarketingCodeExist(marketingCode, staffid);
        }

        public static List<ControlListData> GetStaffHeadList(string branch)
        {
            return KKSlmMsStaffDal.GetStaffHeadList(branch);
        }

        public static StaffDataManagement GetStaff(int staffId)
        {
            return KKSlmMsStaffDal.GetStaff(staffId);
        }

        public static string GetPrivilegeNCB(string productId, decimal staffTypeId)
        {
            return KKCocMsAolDal.GetPrivilegeNCB(productId, staffTypeId);
        }

        public static string GetEmpCode(string username)
        {
            return KKSlmMsStaffDal.GetEmpCode(username);
        }

        public static List<ForecastReportData> GetDataForForecastReport(string teamList)
        {
            return KKSlmMsStaffDal.GetDataForForecastReport(teamList);
        }

        public static List<UserMonitoringData> GetUserMonotoringStaffList(string available, string cocTeam, string teamList)
        {
            return KKSlmMsStaffDal.GetUserMonotoringStaffList(available, cocTeam, teamList);
        }

        public static void SetStaffStatus(string empCode, int status, string loginUsername)
        {
            KKSlmMsStaffDal.SetStaffStatus(empCode, status, loginUsername);
        }

        public static decimal? GetStaffStatus(string username)
        {
            return KKSlmMsStaffDal.GetStaffStatus(username);
        }
    }
}
