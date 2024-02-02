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


    public class EndUserController : ApiController
    {

        public EndUserManager EndUsermanager = new EndUserManager();
        public UserManager usermanager = new UserManager();

        public EndUserController()
        {

        }

        //[HttpPost]
        //public HttpPostResponse DeleteEndUser(long EndUserID, string UserMemRefNo)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            bool flag = false;
        //            flag = EndUsermanager.DeleteEndUser(EndUserID, UserMemRefNo);
        //            if (flag == true)
        //            {
        //                return (new HttpPostResponse { }.CreateResponse("Success", "EndUser Created", ""));
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

        //[HttpPost]
        //public HttpPostResponse UpdateEndUser(EndUserViewModel NewEndUser)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            bool flag = false;
        //            flag = EndUsermanager.UpdateEndUser(NewEndUser);
        //            if (flag == true)
        //            {
        //                return (new HttpPostResponse { }.CreateResponse("Success", "EndUser Updated", ""));
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
        //public dynamic GetEndUserDataByID(long EndUserID, string UserMemRefNo)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            return EndUsermanager.GetDataByID(EndUserID, UserMemRefNo);
        //        }
        //        else
        //            return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //    }
        //    else
        //        return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //}

        //[HttpPost]
        //public HttpPostResponse InsertEndUser(EndUserViewModel NewEndUser)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            bool flag = false;
        //            flag = EndUsermanager.InsertEndUser(NewEndUser);
        //            if (flag == true)
        //            {
        //                return (new HttpPostResponse { }.CreateResponse("Success", "EndUser Inserted", ""));
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
        //public dynamic GetAllEndUser(string UserMemRefNo)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            return EndUsermanager.GetAll(UserMemRefNo);
        //        }
        //        else
        //            return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //    }
        //    else
        //        return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //}

        //[HttpGet]
        //public dynamic GetEndUserListByPage(int page, int pagenumber, string UserMemRefNo)
        //{
        //    HttpRequest newrequest = HttpContext.Current.Request;
        //    string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
        //    string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

        //    if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
        //    {
        //        if (usermanager.ValidateToken(memRefNo, authenticationToken))
        //        {
        //            return EndUsermanager.GetEndUserListByPage(page, pagenumber, UserMemRefNo);
        //        }
        //        else
        //            return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //    }
        //    else
        //        return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        //}

    }
}
