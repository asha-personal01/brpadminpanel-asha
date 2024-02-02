using D1WebApp.Models;
using System.Collections.Generic;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class UserLogViewModel
    {
        public UserLogViewModel()
        {

        }
        public UserLogViewModel(UserLog userlog)
        {
            this.UserLogID = userlog.UserLogID;
            this.UserLogTimestamp = userlog.UserLogTimestamp;
            this.UserID = userlog.UserID; ;
            this.UserAction = userlog.UserAction;
            this.UserActionDescription = userlog.UserActionDescription;
            this.IPAddress = userlog.IPAddress;
            this.Browser = userlog.Browser;
            this.OperatingSystem = userlog.OperatingSystem;
            this.ScreenResolution = userlog.ScreenResolution;
            this.Remarks = userlog.Remarks;
         }
        public static UserLog ConvertToModel(UserLogViewModel userlogvm)
         {
             UserLog userlog = new UserLog();
             userlog.UserLogID = userlogvm.UserLogID;
             userlog.UserLogTimestamp = userlogvm.UserLogTimestamp;
             userlog.UserID = userlogvm.UserID;
             userlog.UserAction = userlogvm.UserAction;
             userlog.UserActionDescription = userlogvm.UserActionDescription;
             userlog.IPAddress= userlogvm.IPAddress;
             userlog.Browser= userlogvm.Browser;
             userlog.OperatingSystem= userlogvm.OperatingSystem;
             userlog.ScreenResolution= userlogvm.ScreenResolution;
             userlog.Remarks = userlogvm.Remarks;
             userlog.isSuccessful = true; 
             userlog.IsAdmin = false;
             return userlog;
         }

        public List<UserLogViewModel> ConvertToViewModelList(IEnumerable<UserLog> userlogs)
         {
             List<UserLogViewModel> userloglist = new List<UserLogViewModel>();
             foreach (UserLog userlog in userlogs)
             {
                 userloglist.Add(new UserLogViewModel(userlog));
             }
             return userloglist;
         }
        public long UserLogID { get; set; }
        public System.DateTime UserLogTimestamp { get; set; }
        public long UserID { get; set; }
        public string UserAction { get; set; }
        public string UserActionDescription { get; set; }
        public string IPAddress { get; set; }
        public string Browser { get; set; }
        public string OperatingSystem { get; set; }
        public string ScreenResolution { get; set; }
        public string Remarks { get; set; }
    }
}