//Developed by Nayan

using System.Web;
using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.BusinessLogicLayer.ViewModels;

namespace D1WebApp.Utilities
{
    public class UserMenuManager
    {
        private UserMenuRepository UserMenusrepository = new UserMenuRepository();        

        public UserMenuManager()
        {
        }
        //public List<UserMenuViewModel> GetAll(string UserMemRefNo)
        //{
        //    return UserMenusrepository.GetAll(UserMemRefNo);
        //}
        //public List<UserMenuViewModel> GetUserMenuListForUI(string UserMemRefNo,int typeweb)
        //{
        //    return UserMenusrepository.GetUserMenuListForUI(UserMemRefNo, typeweb);
        //}
        //public List<UserMenuViewModel> GetUserMenuListByPage(int page, int pagenumber, string UserMemRefNo)
        //{
        //    return UserMenusrepository.GetMenuListByPage(page, pagenumber, UserMemRefNo);
        //}
        //public bool InsertUserMenu(UserMenuViewModel NewUserMenu)
        //{
        //    return UserMenusrepository.InsertMenu(NewUserMenu);
        //}
        //public UserMenuViewModel GetDataByID(long UserMenuID, string UserMemRefNo)
        //{
        //    return UserMenusrepository.GetDataByID(UserMenuID, UserMemRefNo);
        //}
        //public bool UpdateUserMenu(UserMenuViewModel NewUserMenu)
        //{
        //    return UserMenusrepository.UpdateMenu(NewUserMenu);
        //}
        //public bool DeleteUserMenu(long UserMenuID, string UserMemRefNo)
        //{
        //    return UserMenusrepository.DeleteMenu(UserMenuID, UserMemRefNo);
        //}


            

    }
}