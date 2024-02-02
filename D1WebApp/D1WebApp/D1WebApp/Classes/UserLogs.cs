using System;
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.DataAccessLayer.Repositories;


namespace D1WebApp.Utilities
{
    public class UserLogs
    {
        public static void UserLog(DateTime UserLogTimestamp, Int64 UserID, String UserAction, String UserActionDescription, String IPAddress, String Browser, String OperatingSystem, String ScreenResolution, String Remarks)
        {
            UserLogViewModel userlogvm = new UserLogViewModel();
            userlogvm.UserID = UserID;
            userlogvm.UserLogTimestamp = UserLogTimestamp;
            userlogvm.UserAction = UserAction;
            userlogvm.UserActionDescription = UserActionDescription;
            userlogvm.IPAddress = IPAddress;
            userlogvm.Browser = Browser;
            userlogvm.OperatingSystem = OperatingSystem;
            userlogvm.ScreenResolution = ScreenResolution;
            userlogvm.Remarks = Remarks;
            UserLogRepository userlogerep = new UserLogRepository(new D1WebAppEntities());
            userlogerep.InsertUserLog(userlogvm);
            userlogerep.Save();
        }
    }
}