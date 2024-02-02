
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace D1WebApp.BusinessLogicLayer.ViewModels
//{
//    public class EndUserViewModel
//    {
//        public string UserMemRefNo { get; set; }
//        public long UsersID { get; set; }
//        public string FirstName { get; set; }
//        public string LastName { get; set; }
//        public string Mobile { get; set; }
//        public string password { get; set; }
//        public string Email { get; set; }
//        public Nullable<bool> Usertype { get; set; }
//        public Nullable<bool> Active { get; set; }
//        public Nullable<System.DateTime> Lastlogindate { get; set; }
//        public EndUserViewModel()
//        { }

//        public EndUserViewModel(EndUser EndUsers)
//        {
//            this.Active = EndUsers.Active;
//            this.Email = EndUsers.Email;
//            this.FirstName = EndUsers.FirstName;
//            this.Lastlogindate = EndUsers.Lastlogindate;
//            this.LastName = EndUsers.LastName;
//            this.Mobile = EndUsers.Mobile;
//            this.password = EndUsers.password;
//            this.UsersID = EndUsers.UsersID;
//            this.Usertype = EndUsers.Usertype;

//        }

     
//        public static List<EndUserViewModel> ConvertModelToViewModel(List<EndUser> EndUsers)
//        {
//            List<EndUserViewModel> EndUserList = new List<EndUserViewModel>();
//            foreach (EndUser list in EndUsers)
//            {
//                EndUserList.Add(new EndUserViewModel(list));
//            }
//            return EndUserList;
//        }

     
//        public static EndUser ConvertViewModelToModel(EndUserViewModel EndUser)
//        {
//            EndUser EndUsers = new EndUser();
//            if (EndUser.UsersID != null && EndUser.UsersID > 0)
//            {
//                EndUsers.UsersID = EndUser.UsersID;
//            }
//            EndUsers.Active = EndUser.Active;
//            EndUsers.Email = EndUser.Email;
//            EndUsers.FirstName = EndUser.FirstName;
//            EndUsers.Lastlogindate = EndUser.Lastlogindate;
//            EndUsers.LastName = EndUser.LastName;
//            EndUsers.Mobile = EndUser.Mobile;
//            EndUsers.password = EndUser.password;
//            EndUsers.Usertype = EndUser.Usertype;
//            return EndUsers;
//        }
//    }
//}