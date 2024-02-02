using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Linq;
using System.Web;
using D1WebApp.Models;
using D1WebApp.Utilities;
using D1WebApp.Utilities.GeneralConfiguration;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Validation;


namespace D1WebApp.DataAccessLayer.Repositories
{
    public class UserMenuRepository : IUserMenuRepository, IDisposable
    {
    
        public UserMenuRepository()
        {

        }

        //public bool DeleteMenu(long MenuID, string UserMemRefNo)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //        Menu getoldMenu = Context2.Menus.Find(MenuID);
        //        Context2.Menus.Remove(getoldMenu);
        //        Context2.SaveChanges();
        //        Context2.Dispose();
        //        flag = true;
        //    }catch(Exception ed) { }
        //    return flag;
        //}

        //public bool UpdateMenu(UserMenuViewModel NewUserMenu)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(NewUserMenu.UserMemRefNo.ToString()));
        //    Menu getooldUserMenu = Context2.Menus.Find(NewUserMenu.MenuID);
        //    Menu newUserMenu = UserMenuViewModel.ConvertViewModelToModel(NewUserMenu);
        //    Context2.Entry(getooldUserMenu).CurrentValues.SetValues(newUserMenu);
        //    Context2.SaveChanges();
        //    Context2.Dispose();
        //    flag = true;
        //    }
        //    catch (Exception ed) { }
        //    return flag;
        //}

        //public UserMenuViewModel GetDataByID(long UserMenuID, string UserMemRefNo)
        //{
        //    UserMenuViewModel userm = new UserMenuViewModel();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //        var getUserMenu = Context2.Menus.Find(UserMenuID);
        //        userm = new UserMenuViewModel(getUserMenu);
        //    }
        //    catch (Exception ed) { }
        //    return userm;
        //}

        //public bool InsertMenu(UserMenuViewModel NewUserMenu)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(NewUserMenu.UserMemRefNo.ToString()));
        //    if (NewUserMenu.MenuID != null && NewUserMenu.MenuID > 0)
        //    {
        //        Menu oldmenu = Context2.Menus.Find(NewUserMenu.MenuID);
        //        Menu newmenu = UserMenuViewModel.ConvertViewModelToModel(NewUserMenu);
        //        Context2.Entry(oldmenu).CurrentValues.SetValues(newmenu);
        //        Context2.SaveChanges();
        //        Context2.Dispose();
        //        flag = true;
        //    }
        //    else
        //    {
        //        Menu newmenu = UserMenuViewModel.ConvertViewModelToModel(NewUserMenu);
        //        Context2.Menus.Add(newmenu);
        //        Context2.SaveChanges();
        //        Context2.Dispose();
        //        flag = true;
        //    }
        //    }
        //    catch (Exception ed) { }
        //    return flag;
        //}
        //public List<UserMenuViewModel> GetAll(string UserMemRefNo)
        //{
        //    List<UserMenuViewModel> GetAll = new List<UserMenuViewModel>();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //    GetAll = (from br in Context2.Menus
        //              select new UserMenuViewModel
        //              {
        //                  Active = br.Active,
        //                  Linkurl = br.Linkurl,
        //                  MenuID = br.MenuID,
        //                  Rank = br.Rank,
        //                  Title = br.Title,
        //              }).OrderBy(cp => cp.Rank).ToList();
        //    Context2.Dispose();
        //    }
        //    catch (Exception ed) { }
        //    return GetAll;
        //}

        //public List<UserMenuViewModel> GetUserMenuListForUI(string UserMemRefNo,int typeweb)
        //{
        //    List<UserMenuViewModel> GetAll = new List<UserMenuViewModel>();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //    GetAll = (from br in Context2.Menus.Where(cp => cp.Active==true)
        //              select new UserMenuViewModel
        //              {
        //                  webtypes= typeweb,
        //                  Active = br.Active,
        //                  Linkurl = br.Linkurl,
        //                  MenuID = br.MenuID,
        //                  Rank = br.Rank,
        //                  Title = br.Title,
        //              }).ToList();
        //    Context2.Dispose();
        //    }
        //    catch (Exception ed) { }
        //    return GetAll;

        //}

        //public List<UserMenuViewModel> GetMenuListByPage(int page, int pagenumber, string UserMemRefNo)
        //{
        //    List<UserMenuViewModel> GetAll = new List<UserMenuViewModel>();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //    GetAll = (from br in Context2.Menus.OrderByDescending(c => c.MenuID).Skip((page - 1) * pagenumber).Take(pagenumber).ToList()
        //              select new UserMenuViewModel
        //              {
        //                  Active = br.Active,
        //                  Linkurl = br.Linkurl,
        //                  MenuID = br.MenuID,
        //                  Rank = br.Rank,
        //                  Title = br.Title,
        //              }).ToList();
        //    Context2.Dispose();
        //    }
        //    catch (Exception ed) { }
        //    return GetAll;
        //}



        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
    }
}