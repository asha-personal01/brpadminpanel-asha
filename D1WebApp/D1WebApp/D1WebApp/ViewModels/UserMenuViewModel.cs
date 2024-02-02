
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace D1WebApp.BusinessLogicLayer.ViewModels
//{
//    public class UserMenuViewModel
//    {
//        public int webtypes { get; set; }
//        public string UserMemRefNo { get; set; }
//        public long MenuID { get; set; }
//        public string Title { get; set; }
//        public string Linkurl { get; set; }
//        public int Rank { get; set; }
//        public bool Active { get; set; }
//        public long? ParentID { get; set; }

//        public UserMenuViewModel()
//        { }

//        public UserMenuViewModel(Menu UserMenus)
//        {
//            this.ParentID = UserMenus.ParentID;
//            this.MenuID = UserMenus.MenuID;
//            this.Title = UserMenus.Title;
//            this.Linkurl = UserMenus.Linkurl;
//            this.Rank = UserMenus.Rank;
//            this.Active = UserMenus.Active;

//        }

     
//        public static List<UserMenuViewModel> ConvertModelToViewModel(List<Menu> UserMenus)
//        {
//            List<UserMenuViewModel> UserMenuList = new List<UserMenuViewModel>();
//            foreach (Menu list in UserMenus)
//            {
//                UserMenuList.Add(new UserMenuViewModel(list));
//            }
//            return UserMenuList;
//        }

     
//        public static Menu ConvertViewModelToModel(UserMenuViewModel UserMenu)
//        {
//            Menu UserMenus = new Menu();
//            if (UserMenu.MenuID != null && UserMenu.MenuID > 0)
//            {
//                UserMenus.MenuID = UserMenu.MenuID;
//            }
//            UserMenus.ParentID = UserMenu.ParentID;
//            UserMenus.Title = UserMenu.Title;
//           UserMenus.Linkurl = UserMenu.Linkurl;
//           UserMenus.Rank = UserMenu.Rank;
//            UserMenus.Active = UserMenu.Active;
//            return UserMenus;
//        }

        
      
//    }
//}