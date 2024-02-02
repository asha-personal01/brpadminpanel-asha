//Developed by Nayan

using D1WebApp.Models;
using D1WebApp.Utilities;
using System;
using System.Collections.Generic;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{

    public class countviewmodel {
        public long count { get; set; }
    }
    public class UserViewModel
    {
        public UserViewModel() { }
        public UserViewModel(User user)
        {
            this.UserID = user.UserID;
            this.UserName = string.Concat(user.FirstName, " ", user.LastName);
            this.Email = user.Email;
            this.Password = user.Password;
            this.HostName = user.HostName;

            

        }

        public static List<UserViewModel> ConvertToViewModelList(List<User> user)
        {
            List<UserViewModel> userlist = new List<UserViewModel>();
            foreach (User userList in user)
            {
                userlist.Add(new UserViewModel(userList));
            }
            return userlist;
        }
        public string MemberRefNo { get; set; }
        public long RoleID { get; set; }
        public long UserID { get; set; }        
        public decimal GUIDCode { get; set; }
        public string UserName{ get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string HostName { get; set; }

    }

    public class UserDetialViewModel
    {
        public string MemberRefNo { get; set; }
        public string FullName { get; set; }
        public string uuid { get; set; }
        public List<long> UserMemRefNo { get; set; }
    }
}