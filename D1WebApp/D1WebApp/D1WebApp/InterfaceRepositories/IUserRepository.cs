//Developed by Nayan

using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.Utilities;
using D1WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IUserRepository : IDisposable
    {
        bool GetPublishAllwebsites();
        bool UpdateVendorUserLoginDetails(List<VendorUserLoginDetail> VendorUserLoginList);
        bool InsertVendorUserLoginDetailsForSync(List<VendorUserLoginDetail> VendorUserLoginList);
        bool InsertNewTokenToUserForInsert(long UserID, string Atoken, D1WebAppEntities context1);
        bool InsertNewTokenToUser(long UserID, string Atoken);
        string VendorUserLogin(string username, string password, long UserID);
        bool InsertVendorUserLoginDetails(List<VendorUserLoginDetail> VendorUserLoginList, D1WebAppEntities Contaxt1);
        List<ProjectTypeViewModel> GetProjectTypelist();
        bool InsertUserProject(long UserID, int ProjectTypeID, D1WebAppEntities Context1);
        bool DeleteUser(long UserID, bool active);
        bool CheckUserForMemRefNo(string MemRefno,long UserID);
        long GetRoleByType(string type);
        UserInsertViewModel GetUserByMemRefNoForEdit(long UserID);
        List<UserListViewModel> GetUsersList(string type, string Email, long Mobile, string MemberRefNo, string HostName, int pageno);
        //bool InsertUserHomeData(string h1, string dbname);
        //string Getuserwebdata(string dbname);
        List<RoleViewModel> GetRolesList();
        List<UserViewModel> GetActiveUsersByInternalExternal(byte typeofuser);
        bool GetInternalUsertokenListByUserID(long userid);
        long insertNewUser(UserInsertViewModel createnbewuser, D1WebAppEntities Context1);
        bool insertUserRoles(long Userid, long RoleID, long userParent, string TabAccess, D1WebAppEntities Context1);
        User GetUserByUserID(long UserID);
        long RegisterUser(User user);
        void UpdateRegisterUser(User user);
        void Save();
        long GetUserIDByMemRefNo(string MemRefNo);
        long GetMemberRefNo();
        
        User GetUserDataByMemfernoGuid(string memrefno, decimal guidcode);
        User GetExternalUserDataByUsername(string username);
        List<UserSession> GetUserSessionCurrentListByUserID(long userid);
        
        
        
        
        void InsertUserSessionData(UserSession usersessiondata);
        void UpdateUserSessionData(UserSession usersessiondata);
        User GetExternalUserDataByMobileNo(decimal mobileno);
        User GetExternalUserDataByEmailID(string emailid);
        User GetUserFullDataByMemrefNo(string memrefno);
        
        User GetExternalUserDataByUserNameandMobileNo(string username, decimal mobileno);
        
        
        
        
        UserDetialViewModel GetUserDtlByMemRefNo(string MemRefNo);
        
        List<UserDetialViewModel> GetUserDtlListByMemRefNo(UserDetialViewModel userDtl);
        DateTime GetLastLogoutLoginTime(long userid);
        
        void InsertUserVisitUrlTableData(UserVisit uservisitdata);
        List<UniqueSession> GetUniqueSessionData(string uniqueid);
        UniqueSession GetUniqueSessionsingleData(string uniqueid);
        void UpdateUniqueSessionData(UniqueSession olduniquesessiondata, UniqueSession newuniquesessiondata);
        UserSession GetUserSessionData(long userid);

        UserAccessTab GetRoleByUserId(long userId);
    }
}