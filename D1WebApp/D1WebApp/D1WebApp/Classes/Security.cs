using D1WebApp.Models;
using D1WebApp.DataAccessLayer.Repositories;
using System;
using System.Linq;
using System.Web;
namespace D1WebApp.Utilities
{
    public class Security
    {
       

       

        public static DateTime GetLastNotified()
        {
            if (System.Web.HttpContext.Current.Session["LastNotifiedOn"] != null)
            {
                return Convert.ToDateTime(System.Web.HttpContext.Current.Session["LastNotifiedOn"]);
            }
            else
            {
                UpdateLastNotifiedTimeStamp();
                return Convert.ToDateTime(System.Web.HttpContext.Current.Session["LastNotifiedOn"]);
            }
        }


        public static bool UpdateLastNotifiedTimeStamp()
        {
            bool flag = true;
            long UserID = GetCurrentUserID();
            if (UserID > 0)
            {
                if (System.Web.HttpContext.Current.Session["LastNotifiedOn"] != null)
                {
                    System.Web.HttpContext.Current.Session["LastNotifiedOn"] = D1WebApp.Utilities.Miscellaneous.CurrentDateTime();
                    flag = true;
                }
                else
                {
                    UserLogRepository GetUserLog = new UserLogRepository(new D1WebAppEntities());
                    var Timestamp = GetUserLog.GetLogOutTimeStamp(UserID);
                    System.Web.HttpContext.Current.Session["LastNotifiedOn"] = Timestamp;
                    flag = true;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public static long GetCurrentUserID()
        {
            if (System.Web.HttpContext.Current.Session["UserID"] != null)
            {
                return Convert.ToInt64(HttpContext.Current.Session["UserID"]);
            }
            else if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserLogRepository userrepository = new UserLogRepository(new D1WebAppEntities());
                HttpContext.Current.Session["UserID"] = userrepository.GetUserByUserName(HttpContext.Current.User.Identity.Name).UserID;
                return Convert.ToInt64(HttpContext.Current.Session["UserID"]);
            }
            else
            {
                return 0;
            }
        }
        public static string GetCurrentUserName()
        {
            if (HttpContext.Current.Session["UserName"] != null)
            {

                return Convert.ToString(HttpContext.Current.Session["UserName"]);
            }
            else if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserLogRepository userrepository = new UserLogRepository(new D1WebAppEntities());
                var GetUser = userrepository.GetUserByUserName(HttpContext.Current.User.Identity.Name);
                return string.Concat(GetUser.FirstName, " ", GetUser.LastName);
            }
            else
            {
                return " ";
            }
        }
        public static string GetCurrentUserRoleName()
        {
            if (HttpContext.Current.Session["UserRoleName"] != null)
            {

                return Convert.ToString(HttpContext.Current.Session["UserRoleName"]);
            }
            else if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserLogRepository userrepository = new UserLogRepository(new D1WebAppEntities());
                var GetUser = userrepository.GetUserByUserName(HttpContext.Current.User.Identity.Name);
                if (GetUser.UserRoles.Count() != 0)
                {
                    return GetUser.UserRoles.FirstOrDefault().Role.RoleName;
                }
                return " ";
            }
            else
            {
                return " ";
            }
        }
        public static int GetCurrentUserType()
        {
            if (HttpContext.Current.Session["UserType"] != null)
            {

                return Convert.ToInt16(HttpContext.Current.Session["UserType"]);
            }
            else if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserLogRepository userrepository = new UserLogRepository(new D1WebAppEntities());
                var GetUser = userrepository.GetUserByUserName(HttpContext.Current.User.Identity.Name);
                return (int)GetUser.UserRoles.First().RoleID;
            }
            else
            {
                return 0;
            }
        }
        public static long GetCurrentUserBranchID()
        {
            if (HttpContext.Current.Session["BranchID"] != null)
            {

                return Convert.ToInt64(HttpContext.Current.Session["BranchID"]);
            }
            else if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserLogRepository userrepository = new UserLogRepository(new D1WebAppEntities());
                var GetUser = userrepository.GetUserByUserName(HttpContext.Current.User.Identity.Name);              
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static int GetCurrentRolemembershipID()
        {
            if (HttpContext.Current.Session["UserRole"] != null)
            {
                return Convert.ToInt16(HttpContext.Current.Session["UserRole"]);
            }
            else if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserLogRepository userrepository = new UserLogRepository(new D1WebAppEntities());
                long UserID = userrepository.GetUserByUserName(HttpContext.Current.User.Identity.Name).UserID;
                var GetuserByID = userrepository.GetbyId(UserID);
                
                    HttpContext.Current.Session["UserRole"] = (int)GetuserByID.RoleID;
                    return (int)GetuserByID.RoleID;
                
                

            }
            else
            {
                return 0;
            }
        }

        public static string GetCurrentLoginUserToken()
        {
            if (HttpContext.Current.Session["LoginToken"] != null)
            {
                return (HttpContext.Current.Session["LoginToken"]).ToString();
            }
            else if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserLogRepository userrepository = new UserLogRepository(new D1WebAppEntities());
                HttpContext.Current.Session["LoginToken"] = userrepository.GetUserByUserName(HttpContext.Current.User.Identity.Name).AuthenticationToken;
                return (HttpContext.Current.Session["LoginToken"]).ToString();
            }
            else
                return null;
        }
    }
}