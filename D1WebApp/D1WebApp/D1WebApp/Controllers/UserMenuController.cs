//Developed by Nayan
using D1WebApp.Models;
using System;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Utilities.ApiResponse;
using D1WebApp.Utilities;
using D1WebApp.Utilities.GeneralConfiguration;
using System.IO;
using System.Web.Hosting;
using System.Data.Entity.Core.EntityClient;

using D1WebApp.Controllers;

namespace D1WebApp.BussinessLogicLayer.Controllers
{
#if !DEBUG
      [System.Web.Mvc.RequireHttps]
#endif


    public class UserMenuController : ApiController
    {

        public UserMenuManager UserMenumanager = new UserMenuManager();
        public UserManager usermanager = new UserManager();

        public UserMenuController()
        {

        }

        //[HttpGet]
        //public bool DeleteUserMenu(long UserMenuID, string UserMemRefNo)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            bool flag = false;
                    
        //                flag = UserMenumanager.DeleteUserMenu(UserMenuID, UserMemRefNo);
                    
        //            return flag;
                   
        //        }
        //        else
                    
        //            return false;
        //    }
        //    else
                
        //        return false;
        //}

        //[HttpPost]
        //public HttpPostResponse UpdateUserMenu(UserMenuViewModel NewUserMenu)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            bool flag = false;
        //            flag = UserMenumanager.UpdateUserMenu(NewUserMenu);
        //            if (flag == true)
        //            {
        //                return (new HttpPostResponse { }.CreateResponse("Success", "UserMenu Updated", ""));
        //            }
        //            else
        //            {
        //                return (new HttpPostResponse { }.CreateResponse("Failed", "Please try again letter", ""));
        //            }
        //        }
        //        else
        //            return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //    }
        //    else
        //        return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //}

        //[HttpGet]
        //public dynamic GetUserMenuDataByID(long UserMenuID, string UserMemRefNo)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            return UserMenumanager.GetDataByID(UserMenuID, UserMemRefNo);                    
        //        }
        //        else
        //            return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //    }
        //    else
        //        return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //}

        //[HttpPost]
        //public HttpPostResponse InsertUserMenu(UserMenuViewModel NewUserMenu)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            bool flag = false;
        //            flag = UserMenumanager.InsertUserMenu(NewUserMenu);
        //            if (flag == true)
        //            {
        //                return (new HttpPostResponse { }.CreateResponse("Success", "UserMenu Inserted", ""));
        //            }
        //            else
        //            {
        //                return (new HttpPostResponse { }.CreateResponse("Failed", "Please try again letter", ""));
        //            }
        //        }
        //        else
        //            return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //    }
        //    else
        //        return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //}

        //[HttpGet]
        //public dynamic GetAllUserMenu(string UserMemRefNo)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            return UserMenumanager.GetAll(UserMemRefNo);
        //        }
        //        else
        //            return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //    }
        //    else
        //        return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //}

        //[HttpGet]
        //public dynamic GetUserMenuListForUI(string UserMemRefNo)
        //{
        //    List<UserMenuViewModel> Getmenulist = new List<UserMenuViewModel>();
        //    int typeweb=1;
        //    Random rand = new Random();
        //    if (rand.Next(1, 2) == 1)
        //    {
        //        typeweb = 1;
        //    }
        //    else
        //    {
        //        typeweb = 2;
        //    }
        //    typeweb = 2;
        //        Getmenulist = UserMenumanager.GetUserMenuListForUI(UserMemRefNo,typeweb);
            

        //    return Getmenulist;
        //}

        //[HttpGet]
        //public dynamic GetUserMenuListByPage(int page, int pagenumber, string UserMemRefNo)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            return UserMenumanager.GetUserMenuListByPage(page,pagenumber,UserMemRefNo);
        //        }
        //        else
        //            return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //    }
        //    else
        //        return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //}

    }
}
