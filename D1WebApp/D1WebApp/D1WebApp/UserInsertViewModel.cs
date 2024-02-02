//Developed by Nayan

using D1WebApp.Models;
using D1WebApp.Utilities;
using System;
using System.Collections.Generic;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class UserInsertViewModel
    {
        public UserInsertViewModel() { }
        public UserInsertViewModel(User user)
        {
            
            this.UserID = user.UserID;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.Mobile = user.Mobile;
            this.Password = user.Password;
            this.IsActive = user.IsActive;
            this.IsLocked = user.IsLocked;
            this.IsPasswordReset = user.IsPasswordReset;
            this.FailedAttemptCount = user.FailedAttemptCount;
            this.LockedOn = user.LockedOn;
            this.LastLoggedOn = user.LastLoggedOn;
            this.LastUpdatedOn = user.LastUpdatedOn;
            this.LastUpdatedBy = user.LastUpdatedBy;            
            this.GUIDCode = user.GUIDCode.Value;            
            this.AuthenticationToken = user.AuthenticationToken.ToString();
            this.TokenExpirsOn = user.TokenExpirsOn;
            this.MemberRefNo = user.MemberRefNo;
            this.IsRemember = user.IsRemember.Value;
            this.SessionCount = user.SessionCount.Value;
            this.HostName = user.HostName;
            this.ApiPassword = user.ApiPwd;
            this.ApiUserName = user.ApiUserName;
            this.CompanyID = user.CompanyID;
            this.NotificationEmails = user.NotificationEmails;
            this.ApiEndPoint = user.ApiEndPoint;
        }
        public User GetNewUser(UserInsertViewModel UserInsertView)
        {
            User newuser = new User();
            if (UserInsertView.UserID != null && UserInsertView.UserID > 0)
            {
                newuser.UserID = UserInsertView.UserID;
            }
            newuser.FirstName = UserInsertView.FirstName;
            newuser.LastName = UserInsertView.LastName;
            newuser.Email = UserInsertView.Email;
            newuser.Mobile = UserInsertView.Mobile;
            if (!string.IsNullOrEmpty(UserInsertView.Password))
            {
                newuser.Password = UserInsertView.Password;
            }
            newuser.IsActive = UserInsertView.IsActive;
            newuser.IsLocked = UserInsertView.IsLocked;
            newuser.IsPasswordReset = UserInsertView.IsPasswordReset;
            newuser.FailedAttemptCount = UserInsertView.FailedAttemptCount;
            newuser.LockedOn = UserInsertView.LockedOn;
            newuser.LastLoggedOn = UserInsertView.LastLoggedOn;
            newuser.LastUpdatedOn = UserInsertView.LastUpdatedOn;
            newuser.LastUpdatedBy = UserInsertView.LastUpdatedBy;            
            newuser.GUIDCode = UserInsertView.GUIDCode;            
            if (string.IsNullOrEmpty(UserInsertView.AuthenticationToken))
            {
                newuser.AuthenticationToken = Guid.NewGuid();
            }
            newuser.TokenExpirsOn = UserInsertView.TokenExpirsOn;
            newuser.MemberRefNo = UserInsertView.MemberRefNo;
            newuser.IsRemember = UserInsertView.IsRemember;
            newuser.SessionCount = UserInsertView.SessionCount;
            newuser.HostName = UserInsertView.HostName;
            newuser.ApiUserName = UserInsertView.ApiUserName;
            newuser.ApiPwd = UserInsertView.ApiPassword;
            newuser.CompanyID = UserInsertView.CompanyID;
            newuser.NotificationEmails = UserInsertView.NotificationEmails;
            newuser.ApiEndPoint = UserInsertView.ApiEndPoint;
            return newuser;
        }
        public static List<UserInsertViewModel> ConvertToViewModelList(List<User> user)
        {
            List<UserInsertViewModel> userlist = new List<UserInsertViewModel>();
            foreach (User userList in user)
            {
                userlist.Add(new UserInsertViewModel(userList));
            }
            return userlist;
        }
        public string ApiEndPoint { get; set; }
        public string NotificationEmails { get; set; }
        public string CompanyID { get; set; }
        public int ProjectTypeID { get; set; }
        public string ApiUserName { get; set; }
        public string ApiPassword { get; set; }
        public string HostName { get; set; }

        public long UserID { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal Mobile { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsPasswordReset { get; set; }
        public int FailedAttemptCount { get; set; }
        public DateTime? LockedOn { get; set; }
        public DateTime? LastLoggedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public long LastUpdatedBy { get; set; }
        
        public decimal GUIDCode { get; set; }
        public string type { get; set; }
        public string AuthenticationToken { get; set; }
        public DateTime? TokenExpirsOn { get; set; }
        public string MemberRefNo { get; set; }
        public bool IsRemember { get; set; }
        public byte? SessionCount { get; set; }
        public long RoleID { get; set; }
        public long UserParent { get; set; }
        public string TabAccess { get; set; }

    }

    public class UserListViewModel
    {
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public decimal Mobile { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string MemberRefNo { get; set; }
        public int TotalPage { get; set; }
    }

}