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
using D1WebApp.Utilities.Classes;
using D1WebApp.ViewModels;
using System.Data.SqlClient;

namespace D1WebApp.BussinessLogicLayer.Controllers
{
#if !DEBUG
      [System.Web.Mvc.RequireHttps]
#endif


    public class SubUsersController : ApiController
    {
        public SubUsersManager subUserManager = new SubUsersManager();
        public UserManager usermanager = new UserManager();

        public SubUsersController()
        {

        }

        [HttpGet]
        public dynamic GetSubusersList(long userParent, string memRefNo, string filterQuery, int pageno)
        {
              return subUserManager.GetSubusersList(userParent, memRefNo, filterQuery,pageno);
        }

        [HttpPost]
        public HttpPostResponse AddUpdateSubUser(UserInsertViewModel NewUserinsert)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);
            bool flag = false;
            long userid = 0;
            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    if (usermanager.CheckUserForMemRefNo(NewUserinsert.MemberRefNo, NewUserinsert.UserID))
                    {
                        using (var context = new D1WebAppEntities())
                        {
                            using (var dbcxtransaction = context.Database.BeginTransaction())
                            {
                                try
                                {
                                    if (NewUserinsert.UserID == 0)
                                    {
                                        NewUserinsert.IsPasswordReset = false;
                                        NewUserinsert.FailedAttemptCount = 0;
                                        NewUserinsert.LockedOn = null;
                                        NewUserinsert.LastLoggedOn = null;
                                        NewUserinsert.LastUpdatedOn = DateTime.Now;
                                        NewUserinsert.GUIDCode = Miscellaneous.GetRandomCode(4);
                                        D1WebApp.Models.Configuration defaulttokentime = GeneralConfiguration.CheckConfiguration("UIDefaultTokenExpireDurationMinutes");
                                        NewUserinsert.TokenExpirsOn = Miscellaneous.CurrentDateTime().AddMinutes(Convert.ToDouble(defaulttokentime.ConfigurationValue));
                                        NewUserinsert.IsRemember = false;
                                        string sessionvalue = GeneralConfiguration.CheckConfiguration("UserSessionCount").ConfigurationValue;
                                        NewUserinsert.SessionCount = Convert.ToByte(sessionvalue);
                                    }
                                    NewUserinsert.RoleID = usermanager.GetRoleByType(NewUserinsert.type);
                                    userid = subUserManager.AddUpdateSubUser(NewUserinsert, context);
                                    flag = usermanager.inseruserroles(userid, NewUserinsert.RoleID, NewUserinsert.UserParent, NewUserinsert.TabAccess, context);
                                    context.SaveChanges();
                                    dbcxtransaction.Commit();
                                }
                                catch (Exception ed)
                                {
                                    ErrorLogs.ErrorLog(0, "InsertNewUser", DateTime.Now, "InsertNewUser", ed.ToString(), "Account", 2);
                                    dbcxtransaction.Rollback();
                                    flag = false;
                                }
                            }
                        }
                        if (flag == true)
                        {
                            if (NewUserinsert.UserID > 0)
                            {
                                return (new HttpPostResponse { }.CreateResponse("Success", "User Updated Successfully", NewUserinsert.UserID.ToString() + "," + NewUserinsert.MemberRefNo));
                            }
                            else
                            {
                                return (new HttpPostResponse { }.CreateResponse("Success", "User Created Successfully", userid.ToString() + "," + NewUserinsert.MemberRefNo));
                            }
                        }
                        else
                        {
                            return (new HttpPostResponse { }.CreateResponse("Failed", "Please try again letter", ""));
                        }
                    }
                    else
                    {
                        return (new HttpPostResponse { }.CreateResponse("Redirect", "Please Insert Unique ID", "Login"));
                    }
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
            }
        }
    }
}
