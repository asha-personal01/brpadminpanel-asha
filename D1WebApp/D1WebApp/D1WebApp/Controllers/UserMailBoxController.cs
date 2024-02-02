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
using D1WebApp.InterfaceRepositories;
using D1WebApp.Repositories;
using D1WebApp.ViewModels;
using D1WebApp.Controllers;

namespace D1WebApp.BussinessLogicLayer.Controllers
{
#if !DEBUG
      [System.Web.Mvc.RequireHttps]
#endif


    public class UserMailBoxController : ApiController
    {

        public UserMailBoxManager UserMailBoxmanager = new UserMailBoxManager();
        public UserManager usermanager = new UserManager();
        private IUserProcessTimesRepository UserProcessTimesRepository;

        public UserMailBoxController()
        {
            this.UserProcessTimesRepository = new UserProcessTimesRepository(new D1WebAppEntities());
        }

        [HttpGet]
        public HttpPostResponse DeleteUserMailBox(string memRefNo,string company)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNoq = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);
            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNoq))
            {
                if (usermanager.ValidateToken(memRefNoq, authenticationToken))
                {
                    bool flag = false;
                    flag = UserMailBoxmanager.DeleteUserMailBox(memRefNo, company);
                    if (flag == true)
                    {
                        return (new HttpPostResponse { }.CreateResponse("Success", "UserMailBox Deleted", ""));
                    }
                    else
                    {
                        return (new HttpPostResponse { }.CreateResponse("Failed", "Please try again letter", ""));
                    }
                }
                else
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
            }
            else
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        }

        [HttpPost]
        public HttpPostResponse UpdateUserMailBox(UserMailBoxViewModel NewUserMailBox)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    bool flag = false;
                    flag = UserMailBoxmanager.UpdateUserMailBox(NewUserMailBox);
                    if (flag == true)
                    {
                        return (new HttpPostResponse { }.CreateResponse("Success", "UserMailBox Updated", ""));
                    }
                    else
                    {
                        return (new HttpPostResponse { }.CreateResponse("Failed", "Please try again letter", ""));
                    }
                }
                else
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
            }
            else
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        }

        [HttpGet]
        public dynamic GetUserMailBoxDataByUserID(string memRefNo)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo1 = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo1))
            {
                if (usermanager.ValidateToken(memRefNo1, authenticationToken))
                {
                    return UserMailBoxmanager.GetDataByUserID(memRefNo);
                }
                else
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
            }
            else
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        }

        [HttpPost]
        public HttpPostResponse InsertUserMailBox(UserMailBoxViewModel NewUserMailBox)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    bool flag = false;
                    flag = UserMailBoxmanager.InsertUserMailBox(NewUserMailBox);
                    if (flag == true)
                    {
                        return (new HttpPostResponse { }.CreateResponse("Success", "UserMailBox Inserted", ""));
                    }
                    else
                    {
                        return (new HttpPostResponse { }.CreateResponse("Failed", "Please try again letter", ""));
                    }
                }
                else
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
            }
            else
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        }

        [HttpPost]
        public HttpPostResponse InsertUserProcessTimes(UserProcessTimesViewModel UserProcessTimeView)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    bool flag = false;
                    var getuser = usermanager.GetUserAlldataByMemrefno(UserProcessTimeView.MemRefNo);
                    UserProcessTimeView.UserID = getuser.UserID;
                    flag = UserProcessTimesRepository.InsertUserProcessTimes(UserProcessTimeView);
                    if (flag == true)
                    {
                        return (new HttpPostResponse { }.CreateResponse("Success", "UserProcessTimes Inserted", ""));
                    }
                    else
                    {
                        return (new HttpPostResponse { }.CreateResponse("Failed", "Please try again letter", ""));
                    }
                }
                else
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
            }
            else
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        }
        [HttpGet]
        public dynamic GetUserProcessTimesByUserID(string memRefNo)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo1 = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo1))
            {
                if (usermanager.ValidateToken(memRefNo1, authenticationToken))
                {
                    var getuser = usermanager.GetUserAlldataByMemrefno(memRefNo);
                    return UserProcessTimesRepository.GetUserProcessTimesByUserID(getuser.UserID);
                }
                else
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
            }
            else
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        }
        [HttpGet]
        public dynamic GetSchedulerConfigByUserID(string memRefNo)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo1 = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo1))
            {
                if (usermanager.ValidateToken(memRefNo1, authenticationToken))
                {
                    var getuser = usermanager.GetUserAlldataByMemrefno(memRefNo);
                    return UserProcessTimesRepository.GetSchedulerConfigByUserID(getuser.UserID);
                }
                else
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
            }
            else
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        }
        [HttpGet]
        public dynamic GetProcessTimesList()
        {
            return UserProcessTimesRepository.GetProcessTimesList();
        }
    }
}
