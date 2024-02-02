//Developed by Nayan
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.Utilities;
using D1WebApp.Utilities.ApiResponse;
using D1WebApp.Utilities.Classes;
using D1WebApp.Utilities.GeneralConfiguration;
using D1WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Http;

namespace D1WebApp.BussinessLogicLayer.Controllers
{
#if !DEBUG
      [System.Web.Mvc.RequireHttps]
#endif


    public class AccountController : ApiController
    {

        public UserManager usermanager = new UserManager();
        public CompanyProfileManager CompanyProfileM = new CompanyProfileManager();

        public AccountController()
        {

        }
        [HttpGet]
        public bool GetCheckMobile(decimal MobileNo)
        {
            return usermanager.GetCheckMobileNo(MobileNo);
        }
        [HttpGet]
        public bool GetCheckEmailID(string emailid)
        {
            return usermanager.GetCheckEmailID(emailid);
        }


        public bool GetPublishedAllWEbsites()
        {
            return usermanager.GetPublishAllwebsites();
        }


        [HttpGet]
        public dynamic GetProjectTypelist()
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    return usermanager.GetProjectTypelist();
                }
                return (new HttpPostResponse { }.CreateResponse("Failed", "Invalid User", ""));
            }
            return (new HttpPostResponse { }.CreateResponse("Failed", "Invalid User", ""));
        }

        [HttpGet]
        public dynamic DeleteUser(long UserID, bool Active)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    var getuser = usermanager.GetUserByID(UserID);
                    try
                    {
                        var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirectory");
                        var targetURL1 = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirEcom");
                        string targetDirectory = targetURL.ConfigurationValue + getuser.MemberRefNo.ToString();
                        string targetDirectory1 = targetURL1.ConfigurationValue + getuser.MemberRefNo.ToString();
                        try
                        {
                            Directory.Delete(targetDirectory, true);
                        }
                        catch (Exception de) { }
                        try
                        {
                            Directory.Delete(targetDirectory1, true);
                        }
                        catch (Exception ed) { }
                        HostingCls.DeleteIISWebsite(getuser.MemberRefNo.ToString());
                        HostingCls.DeleteIISWebsite(getuser.MemberRefNo.ToString() + "API");
                        HostingCls.DeleteIISWebsite(getuser.MemberRefNo.ToString() + "UI");
                    }
                    catch (Exception ed) { }
                    return usermanager.DeleteUser(UserID, Active);
                }
                return (new HttpPostResponse { }.CreateResponse("Failed", "Invalid User", ""));
            }
            return (new HttpPostResponse { }.CreateResponse("Failed", "Invalid User", ""));
        }
        public dynamic GetUserDataForEdit(long UserID)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    return usermanager.GetUserByMemRefNoForEdit(UserID);
                }
                return (new HttpPostResponse { }.CreateResponse("Failed", "Invalid User", ""));
            }
            return (new HttpPostResponse { }.CreateResponse("Failed", "Invalid User", ""));

        }
        [HttpPost]
        public HttpPostResponse InsertNewUser(UserInsertViewModel NewUserinsert)
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
                                    if (NewUserinsert.UserID == null || NewUserinsert.UserID == 0)
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
                                    userid = usermanager.insertNewUser(NewUserinsert, context);
                                    flag = usermanager.inseruserroles(userid, NewUserinsert.RoleID,0,null, context);
                                    if (NewUserinsert.RoleID != 1 && NewUserinsert.UserID == 0)
                                    {
                                        flag = usermanager.InsertUserProject(userid, NewUserinsert.ProjectTypeID, context);
                                    }
                                    if (NewUserinsert.RoleID != 1 && NewUserinsert.UserID == 0)
                                    {
                                        if (NewUserinsert.ProjectTypeID == 1 || NewUserinsert.ProjectTypeID == 4 || NewUserinsert.ProjectTypeID == 5 || NewUserinsert.ProjectTypeID == 6 || NewUserinsert.ProjectTypeID == 7)
                                        {
                                            WebApiRequest w1 = new WebApiRequest();
                                            var getclient = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("userclient");
                                            string toaken = w1.GetAuthonticationToken(getclient.ConfigurationValue, NewUserinsert.CompanyID, NewUserinsert.ApiUserName, NewUserinsert.ApiPassword, NewUserinsert.ApiEndPoint);
                                            if (!string.IsNullOrEmpty(toaken))
                                            {
                                                var sourceURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("sourceDirEcom");
                                                var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirEcom");
                                                var DBName = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("DatabaseName");
                                                //database
                                                string targetDirectory = targetURL.ConfigurationValue + NewUserinsert.MemberRefNo.ToString();
                                                string oldNameFullPath = targetDirectory + "\\" + DBName.ConfigurationValue;
                                                string newNameFullPath = targetDirectory + "\\" + NewUserinsert.MemberRefNo.ToString() + ".mdf";
                                                if (!Directory.Exists(targetDirectory + "\\" + NewUserinsert.MemberRefNo.ToString()))
                                                {
                                                    Directory.CreateDirectory(targetDirectory + "\\" + NewUserinsert.MemberRefNo.ToString());
                                                }
                                                System.IO.File.Copy(sourceURL.ConfigurationValue + DBName.ConfigurationValue, newNameFullPath);

                                                //Api
                                                var EcomApiName = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("EcomApiName");
                                                targetDirectory = targetURL.ConfigurationValue + NewUserinsert.MemberRefNo.ToString();
                                                oldNameFullPath = targetDirectory + "\\" + EcomApiName.ConfigurationValue;
                                                string Apinewpath = targetDirectory + "\\" + NewUserinsert.MemberRefNo.ToString() + "API";
                                                CopyDir.Copy(sourceURL.ConfigurationValue + EcomApiName.ConfigurationValue, Apinewpath);


                                                //UI
                                                var EcomUI = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("EcomUIName");
                                                targetDirectory = targetURL.ConfigurationValue + NewUserinsert.MemberRefNo.ToString();
                                                oldNameFullPath = targetDirectory + "\\" + EcomUI.ConfigurationValue;
                                                string UIpath = targetDirectory + "\\" + NewUserinsert.MemberRefNo.ToString() + "UI";
                                                CopyDir.Copy(sourceURL.ConfigurationValue + EcomUI.ConfigurationValue, UIpath);

                                                //webconfig and database name
                                                var Sqlserver = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("SqlServer");
                                                string text = File.ReadAllText(Apinewpath + "\\web.config");
                                                text = text.Replace("{{DBName}}", newNameFullPath).Replace("{{Sqlserver}}", Sqlserver.ConfigurationValue);
                                                File.WriteAllText(Apinewpath + "\\web.config", text);



                                                var mainfile = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("EcommerceMainFile");
                                                string text1 = File.ReadAllText(UIpath + "\\" + mainfile.ConfigurationValue);
                                                var AdminURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UserApiPath");
                                                text1 = text1.Replace("{{AdminURL}}", AdminURL.ConfigurationValue + NewUserinsert.MemberRefNo.ToString() + "API").Replace("{{cookname}}", NewUserinsert.MemberRefNo.ToString());
                                                File.WriteAllText(UIpath + "\\" + mainfile.ConfigurationValue, text1);
                                                string text2 = File.ReadAllText(UIpath + "\\" + "index.html");                                                
                                                text2 = text2.Replace("{{cookname}}", NewUserinsert.MemberRefNo.ToString());
                                                File.WriteAllText(UIpath + "\\" + "index.html", text2);
                                                HostingCls.CreateIISWebsite(@"/" + NewUserinsert.MemberRefNo.ToString() + "API", NewUserinsert.MemberRefNo.ToString() + "API", Apinewpath, "DefaultAppPool");
                                                HostingCls.CreateIISWebsite(@"/" + NewUserinsert.MemberRefNo, NewUserinsert.MemberRefNo, UIpath, "DefaultAppPool");
                                                var usergetclient = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("userclient");
                                                var domainpath = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("domainpath");
                                                string userapipaths = domainpath.ConfigurationValue + @"/" + NewUserinsert.MemberRefNo.ToString() + "API/";
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "CUST_ID_PREFIX", NewUserinsert.MemberRefNo);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "ApiEndPoint", NewUserinsert.ApiEndPoint);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "AuthonticationToken", toaken);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "client", usergetclient.ConfigurationValue);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "company", NewUserinsert.CompanyID);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "username", NewUserinsert.ApiUserName);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "password", NewUserinsert.ApiPassword);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "AppPath", userapipaths);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "SY_COOKIE_NAME", NewUserinsert.MemberRefNo);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "businesstype", "B2C");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "websitetype", NewUserinsert.ProjectTypeID.ToString());
                                                if (NewUserinsert.ProjectTypeID == 6)
                                                {
                                                    CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "itemcount", "50000");
                                                }
                                                else if (NewUserinsert.ProjectTypeID == 5)
                                                {
                                                    CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "itemcount", "10000");
                                                }
                                                else if (NewUserinsert.ProjectTypeID == 4)
                                                {
                                                    CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "itemcount", "100");
                                                }
                                                else
                                                {
                                                    CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "itemcount", "0");
                                                }
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "orderprefix", NewUserinsert.MemberRefNo);
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "orderpostfix", "0");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "Socialfacebook", "https://www.facebook.com");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "Socialtwitter", "https://twitter.com");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "Socialgoogle", "https://plus.google.com");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "Sociallinkedin", "https://www.linkedin.com");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "PaymentMethod", "AuthorizeNet");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "PaymentType", "Capture");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "AddZeroValue", "0");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "FastAddtocart", "1");
                                                CompanyProfileM.AddConfiguations(NewUserinsert.MemberRefNo, "TreeNode", "1");
                                                //if (NewUserinsert.ProjectTypeID != 7)
                                                //{
                                                //    CompanyProfileM.DataMigration(NewUserinsert.MemberRefNo, NewUserinsert.ApiEndPoint, toaken, NewUserinsert.CompanyID);
                                                //}
                                                CompanyProfileM.AddConfigurationforapi(NewUserinsert.MemberRefNo, NewUserinsert.ApiEndPoint, toaken, usergetclient.ConfigurationValue, NewUserinsert.CompanyID, NewUserinsert.ApiUserName, NewUserinsert.ApiPassword);
                                                usermanager.InsertNewTokenToUserForInsert(userid, toaken, context);
                                                //w1.SetConfigurations(NewUserinsert.MemberRefNo, NewUserinsert.ApiEndPoint, toaken, usergetclient.ConfigurationValue, NewUserinsert.CompanyID, NewUserinsert.ApiUserName, NewUserinsert.ApiPassword, userapipaths);
                                                //w1.DataPullMehod(NewUserinsert.MemberRefNo);
                                            }
                                        }
                                        else if (NewUserinsert.ProjectTypeID == 2)
                                        {
                                            WebApiRequest w1 = new WebApiRequest();
                                            var getclient = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("client");
                                            string toaken = w1.GetAuthonticationToken(getclient.ConfigurationValue, NewUserinsert.CompanyID, NewUserinsert.ApiUserName, NewUserinsert.ApiPassword, NewUserinsert.ApiEndPoint);

                                            List<VendorUserLoginDetail> VendorUserLoginList = VendorLoginDetailsViewModel.ConvertToModelListForUser(w1.GetVendorLoginDetails(NewUserinsert.CompanyID, toaken, NewUserinsert.ApiEndPoint), userid, NewUserinsert.CompanyID);
                                            usermanager.InsertVendorUserLoginDetails(VendorUserLoginList, context);
                                            var sourceURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("sourceDirectory");
                                            string sourceDirectory = sourceURL.ConfigurationValue;
                                            var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirectory");
                                            string targetDirectory = targetURL.ConfigurationValue + NewUserinsert.MemberRefNo.ToString();
                                            CopyDir.Copy(sourceDirectory, targetDirectory);
                                            var mainfile = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("mainfile");
                                            string text = File.ReadAllText(targetDirectory + "\\" + mainfile.ConfigurationValue);
                                            var AdminURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("AdminURL");
                                            text = text.Replace("{{UserID}}", userid.ToString());
                                            text = text.Replace("{{AdminURL}}", AdminURL.ConfigurationValue);
                                            usermanager.InsertNewTokenToUserForInsert(userid, toaken, context);
                                            File.WriteAllText(targetDirectory + "\\" + mainfile.ConfigurationValue, text);
                                            HostingCls.CreateIISWebsite(@"/" + NewUserinsert.MemberRefNo, NewUserinsert.MemberRefNo, targetDirectory, "DefaultAppPool");
                                        }
                                        else if (NewUserinsert.ProjectTypeID == 3)
                                        {
                                            WebApiRequest w1 = new WebApiRequest();
                                            var getclient = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("Amazonclient");
                                            string toaken = w1.GetAuthonticationToken(getclient.ConfigurationValue, NewUserinsert.CompanyID, NewUserinsert.ApiUserName, NewUserinsert.ApiPassword, NewUserinsert.ApiEndPoint);
                                            var sourceURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("sourceDirectoryAmazon");
                                            var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirectoryAmazon");
                                            string sourceDirectory = sourceURL.ConfigurationValue;
                                            string targetDirectory = targetURL.ConfigurationValue + NewUserinsert.MemberRefNo.ToString();
                                            CopyDir.Copy(sourceDirectory, targetDirectory);

                                            string connectionString = System.Configuration.ConfigurationManager
                          .ConnectionStrings["ConnStringDb1"].ConnectionString;
                                            connectionString = connectionString.Replace("{{path}}", targetDirectory + "\\App_Data\\");
                                            SqlConnection connection = new SqlConnection(connectionString);
                                            using (SqlCommand command = connection.CreateCommand())
                                            {
                                                command.CommandText = "update Configuration set ConfigurationValue=@value where ConfigurationKey=@key";
                                                command.Parameters.AddWithValue("@value", NewUserinsert.ApiEndPoint);
                                                command.Parameters.AddWithValue("@key", "ApiEndPoint");
                                                connection.Open();
                                                command.ExecuteNonQuery();
                                                connection.Close();
                                            }
                                            using (SqlCommand command = connection.CreateCommand())
                                            {
                                                command.CommandText = "update Configuration set ConfigurationValue=@value where ConfigurationKey=@key";
                                                command.Parameters.AddWithValue("@value", toaken);
                                                command.Parameters.AddWithValue("@key", "AuthonticationToken");
                                                connection.Open();
                                                command.ExecuteNonQuery();
                                                connection.Close();
                                            }
                                            using (SqlCommand command = connection.CreateCommand())
                                            {
                                                command.CommandText = "update Configuration set ConfigurationValue=@value where ConfigurationKey=@key";
                                                command.Parameters.AddWithValue("@value", getclient.ConfigurationValue);
                                                command.Parameters.AddWithValue("@key", "client");
                                                connection.Open();
                                                command.ExecuteNonQuery();
                                                connection.Close();
                                            }
                                            using (SqlCommand command = connection.CreateCommand())
                                            {
                                                command.CommandText = "update Configuration set ConfigurationValue=@value where ConfigurationKey=@key";
                                                command.Parameters.AddWithValue("@value", NewUserinsert.CompanyID);
                                                command.Parameters.AddWithValue("@key", "company");
                                                connection.Open();
                                                command.ExecuteNonQuery();
                                                connection.Close();
                                            }
                                            using (SqlCommand command = connection.CreateCommand())
                                            {
                                                command.CommandText = "update Configuration set ConfigurationValue=@value where ConfigurationKey=@key";
                                                command.Parameters.AddWithValue("@value", NewUserinsert.ApiUserName);
                                                command.Parameters.AddWithValue("@key", "username");
                                                connection.Open();
                                                command.ExecuteNonQuery();
                                                connection.Close();
                                            }
                                            using (SqlCommand command = connection.CreateCommand())
                                            {
                                                command.CommandText = "update Configuration set ConfigurationValue=@value where ConfigurationKey=@key";
                                                command.Parameters.AddWithValue("@value", NewUserinsert.ApiPassword);
                                                command.Parameters.AddWithValue("@key", "password");
                                                connection.Open();
                                                command.ExecuteNonQuery();
                                                connection.Close();
                                            }
                                            usermanager.InsertNewTokenToUserForInsert(userid, toaken, context);
                                            HostingCls.CreateIISWebsite(@"/" + NewUserinsert.MemberRefNo, NewUserinsert.MemberRefNo, targetDirectory, "DefaultAppPool");
                                        }
                                    }
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
                            if (NewUserinsert.UserID != null && NewUserinsert.UserID > 0)
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

        [HttpGet]
        public dynamic CreateWebsite(string name)
        {
            name = "Nayan";
            HostingCls.CreateIISWebsite(name, name, @"D:\ApiTest", "DefaultAppPool");
            return true;
        }



        [HttpPost]
        public HttpPostResponse ChangePassword(ChangePasswordViewModel changepasswordviewmodel)
        {

            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    try
                    {
                        changepasswordviewmodel.CurrentPassword = EasyMD5.Hash(changepasswordviewmodel.CurrentPassword);
                        changepasswordviewmodel.NewPassword = EasyMD5.Hash(changepasswordviewmodel.NewPassword);

                        if (ModelState.IsValid)
                        {

                            bool chflag = usermanager.ChangePassword(changepasswordviewmodel);
                            if (chflag)
                            {
                                string userid = changepasswordviewmodel.UserName;
                                User user = usermanager.GetUserAlldataByMemrefno(userid);
                                //UIUserProfile userprofile = userprofilerepository.GetUserProfileByUserId(user.UserID);
                                usermanager.GetActiveUserSessionData(user.UserID);
                                UserSystemDetails systemdetails = usermanager.GetUserSystemConfiguration();
                                UserLogs.UserLog(Miscellaneous.CurrentDateTime(), user.UserID, "Change Password", "Success", systemdetails.IpAddress, systemdetails.Browser, systemdetails.OperatingSystem, systemdetails.ScreenResolution, "");
                                return (new HttpPostResponse { }.CreateResponse("Success", "Password changed", ""));
                            }
                            else
                            {
                                return (new HttpPostResponse { }.CreateResponse("Current", "Invalid current password", ""));
                            }
                        }
                        else
                        {
                            string pusername = "Guest";
                            if (!string.IsNullOrEmpty(changepasswordviewmodel.UserName))
                            {
                                pusername = changepasswordviewmodel.UserName;
                            }
                            string userid = changepasswordviewmodel.UserName;
                            User user = usermanager.GetUserAlldataByMemrefno(userid);
                            if (user != null)
                            {
                                ErrorLogs.ErrorLog(user.UserID, user.Email, Miscellaneous.CurrentDateTime(), "", "Password Change Failed", "Change Password", 2);
                            }
                            else
                            {
                                ErrorLogs.ErrorLog(0, pusername, Miscellaneous.CurrentDateTime(), "", "Password Change Failed", "Change Password", 2);
                            }
                            return (new HttpPostResponse { }.CreateResponse("Failed", "Password change failed", ""));
                        }
                    }
                    catch (Exception ex)
                    {
                        string userid = changepasswordviewmodel.UserName;
                        User user = usermanager.GetUserAlldataByMemrefno(userid);
                        if (user != null)
                        {
                            ErrorLogs.ErrorLog(user.UserID, user.Email, Miscellaneous.CurrentDateTime(), "", ex.Message, "Change Password", 1);
                        }
                        else
                        {
                            ErrorLogs.ErrorLog(0, "Guest", Miscellaneous.CurrentDateTime(), "", ex.Message, "Change Password", 1);
                        }
                        return (new HttpPostResponse { }.CreateResponse("Failed", ex.Message, ""));
                    }
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Change password failed", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Change password failed", "Login"));
            }
        }



        [HttpPost]
        public HttpPostResponse ChangePasswordVendor(ChangePasswordViewModel changepasswordviewmodel)
        {

            HttpRequest newrequest = HttpContext.Current.Request;
            long UserID = Convert.ToInt64(newrequest.Headers["UserID"]);
            string Vendor = Convert.ToString(newrequest.Headers["Vendor"]);
            if (UserID > 0 && !string.IsNullOrEmpty(Vendor))
            {
                if (usermanager.ValidateTokenVendor(UserID, Vendor))
                {
                    try
                    {
                        bool chflag = usermanager.ChangePasswordForVendor(changepasswordviewmodel, UserID, Vendor);
                        if (chflag)
                        {

                            UserSystemDetails systemdetails = usermanager.GetUserSystemConfiguration();
                            UserLogs.UserLog(Miscellaneous.CurrentDateTime(), UserID, "Vendor Change Password", "Success", systemdetails.IpAddress, systemdetails.Browser, systemdetails.OperatingSystem, systemdetails.ScreenResolution, "");
                            return (new HttpPostResponse { }.CreateResponse("Success", "Password changed", ""));
                        }
                        else
                        {
                            return (new HttpPostResponse { }.CreateResponse("Current", "Invalid current password", ""));
                        }

                    }
                    catch (Exception ex)
                    {

                        ErrorLogs.ErrorLog(0, "Guest", Miscellaneous.CurrentDateTime(), "", ex.Message, "Change Password", 1);
                        return (new HttpPostResponse { }.CreateResponse("Failed", ex.Message, ""));
                    }
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Change password failed", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Change password failed", "Login"));
            }
        }

        [HttpGet]
        public dynamic GetUserListToDisplay(string type, int pageno, string Email = null, long Mobile = 0, string MemberRefNo = null, string HostName = null)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    return usermanager.GetUsersList(type, Email, Mobile, MemberRefNo, HostName, pageno);
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
            }
        }



        public dynamic GetUserDetailByEmail(string EmailAddress)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    return usermanager.GetUserByEmail(EmailAddress);
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
            }
        }

        public dynamic GetUserRolesList()
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    return usermanager.GetRolesList();
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
            }
        }

        [HttpGet]
        public dynamic CheckUserForMemRefNo(string MemrefNo, long UserID)
        {
            return usermanager.CheckUserForMemRefNo(MemrefNo, UserID);
        }

        [AllowAnonymous]
        [HttpPost]
        public string VendorLogin(VendorLoginViewModel VendorLoginView)
        {
            return usermanager.VendorUserLogin(VendorLoginView.UserName, VendorLoginView.Password, VendorLoginView.UserID);
        }


        [AllowAnonymous]
        [HttpPost]
        public dynamic Login(LoginViewModel loginviewmodel)
        {
            string[] retvalArr;
            int result;
            string authentoken;
            D1WebApp.Models.Configuration validtokenminute = GeneralConfiguration.CheckConfiguration("UIUserLogoutTime");
            try
            {
                loginviewmodel.UserName = loginviewmodel.UserName.ToLower();
                UserAccessTab role = new UserAccessTab();
                retvalArr = usermanager.UserLogin(loginviewmodel);
                result = Convert.ToInt16(retvalArr[0]);
                authentoken = retvalArr[1];
                if (result == 2)
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "User Already Login", "Login"));
                }
                else if (result == 3)
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Company InActive", "Login"));
                }
                else
                {
                    User user = usermanager.GetExternalUserDataByUsername(loginviewmodel.UserName);
                    D1WebApp.Models.Configuration defaulttokentime = GeneralConfiguration.CheckConfiguration("UIDefaultTokenExpireDurationMinutes");

                    if (user != null)
                    {
                        role = usermanager.GetRoleByUserId(user.UserID);
                    }

                    if (user.IsActive == false)
                    {
                        return (new LoginResponse { }.CreateResponse("Activation", "User Activation Required", "Activation", "", user.MemberRefNo, user.FirstName + " " + user.LastName, user.TokenExpirsOn, validtokenminute.ConfigurationValue, user.UserID, null, null, role.RoleName,role.TabAccess,role.UserParent,role.UserParentMemberRefNo));
                    }
                    if (user.IsPasswordReset == true)
                    {
                        D1WebApp.Models.Configuration remembertokentime = GeneralConfiguration.CheckConfiguration("UIRememberTokenExpireDurationMinutes");
                        return (new LoginResponse { }.CreateResponse("Redirect", "Change password is required", "ChangePassword", authentoken, user.MemberRefNo, user.FirstName + " " + user.LastName, user.TokenExpirsOn, remembertokentime.ConfigurationValue, user.UserID, validtokenminute.ConfigurationValue, null, role.RoleName, role.TabAccess, role.UserParent, role.UserParentMemberRefNo));
                    }
                    else
                    {
                        bool utype = false;
                        if (user.HostName == null && user.ApiUserName == null && user.ApiPwd == null && user.CompanyID == null && user.ApiEndPoint == null && user.ApiToken == null)
                        {
                            utype = true;
                        }
                        else { utype = false; }
                        DateTime lastTime = usermanager.GetLastLogoutLoginTime(user);
                        UserSystemDetails systemdetails = usermanager.GetUserSystemConfiguration();
                        UserLogs.UserLog(Miscellaneous.CurrentDateTime(), user.UserID, "Login Successful", "Success", systemdetails.IpAddress, systemdetails.Browser, systemdetails.OperatingSystem, systemdetails.ScreenResolution, "");
                        return (new LoginResponse { }.CreateResponse("Success", "Login successful", "edit profile", authentoken, user.MemberRefNo, user.FirstName + " " + user.LastName, user.TokenExpirsOn, defaulttokentime.ConfigurationValue, user.UserID, validtokenminute.ConfigurationValue, utype.ToString(), lastTime, role.RoleName, role.TabAccess, role.UserParent, role.UserParentMemberRefNo));
                    }
                }
            }
            catch (Exception ex)
            {

                User user = usermanager.GetExternalUserDataByUsername(loginviewmodel.UserName);
                if (user != null)
                {
                    ErrorLogs.ErrorLog(user.UserID, loginviewmodel.UserName.ToString(), Miscellaneous.CurrentDateTime(), "", ex.Message, "User Login", 1);
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "Guest", Miscellaneous.CurrentDateTime(), "", ex.Message, "User Login", 1);
                }
                return (new LoginResponse { }.CreateResponse("Failed", ex.Message, "", "", "", "", null, "", 0, validtokenminute.ConfigurationValue, "", DateTime.Now,"","",0,""));
            }
        }

        [HttpPost]
        public HttpPostResponse ResetPassword(ForgotPasswordViewModel forgotpasswordviewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    forgotpasswordviewmodel.UserName = forgotpasswordviewmodel.UserName.ToLower();

                    Random random = new Random();
                    string password = random.Next(1, 1000000000).ToString().Substring(0, 6);
                    forgotpasswordviewmodel.Password = EasyMD5.Hash(password);
                    usermanager.PasswordReset(forgotpasswordviewmodel);

                    User user = usermanager.GetExternalUserDataByUsername(forgotpasswordviewmodel.UserName);
                    if (GeneralConfiguration.CheckConfiguration("UIIsMail").ConfigurationValue == "1")
                    {


                    }
                    // send for sms
                    string smssend = "-1";
                    if (GeneralConfiguration.CheckConfiguration("UIIsSMS").ConfigurationValue == "1")
                    {
                        string message = "OTP for your password request on www.D1WebApp.com is " + password + ". Do not share this OTP with anyone for security reasons. For queries, contact +91 79 4040 1234.";
                        if ((user.Mobile == null ? 0 : user.Mobile) != 0)
                        {

                        }
                        if (smssend == "-1")
                        {

                        }
                    }

                    UserSystemDetails systemdetails = usermanager.GetUserSystemConfiguration();
                    UserLogs.UserLog(Miscellaneous.CurrentDateTime(), user.UserID, "Password Reset Successful", "Success", systemdetails.IpAddress, systemdetails.Browser, systemdetails.OperatingSystem, systemdetails.ScreenResolution, "");
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Password Reset Successful", "ChangePassword"));
                }
                else
                {
                    string pusername = "Guest";
                    if (!string.IsNullOrEmpty(forgotpasswordviewmodel.UserName))
                    {
                        pusername = forgotpasswordviewmodel.UserName;
                    }

                    User user = usermanager.GetExternalUserDataByUsername(forgotpasswordviewmodel.UserName);
                    if (user != null)
                    {
                        ErrorLogs.ErrorLog(user.UserID, user.Email, Miscellaneous.CurrentDateTime(), "", "Password Reset Failed", "Reset Password", 2);
                    }
                    else
                    {
                        ErrorLogs.ErrorLog(0, pusername, Miscellaneous.CurrentDateTime(), "", "Password Reset Failed", "Reset Password", 2);
                    }
                    return (new HttpPostResponse { }.CreateResponse("Fail", "Password Reset Failed", ""));
                }

            }
            catch (Exception ex)
            {
                User user = usermanager.GetExternalUserDataByUsername(forgotpasswordviewmodel.UserName);
                if (user != null)
                {
                    ErrorLogs.ErrorLog(user.UserID, user.Email, Miscellaneous.CurrentDateTime(), "", ex.Message, "Reset Password", 1);
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "Guest", Miscellaneous.CurrentDateTime(), "", ex.Message, "Reset Password", 1);
                }
                return (new HttpPostResponse { }.CreateResponse("Fail", ex.Message, ""));
            }
        }

        [HttpGet]
        public HttpPostResponse ValidateToken()
        {
            string retStatus = "Redirect";
            string retMessage = "Token Invalid";
            string retRedirectUrl = "Login";

            try
            {
                HttpRequest newrequest = HttpContext.Current.Request;
                string headercontent = newrequest.Headers["Authorization"];

                if (headercontent != null)
                {
                    string[] credentials = headercontent.Split(' ');
                    string[] tokenuser = credentials[1].Split('/');

                    string token = tokenuser[0];
                    string memrefno = tokenuser[1];

                    if (usermanager.ValidateToken(memrefno, token))
                    {
                        retStatus = "Success";
                        retMessage = "Token Valid";
                        retRedirectUrl = "";
                    }
                }
                return (new HttpPostResponse { }.CreateResponse(retStatus, retMessage, retRedirectUrl));
            }
            catch (Exception ex)
            {
                return (new HttpPostResponse { }.CreateResponse(retStatus, retMessage, retRedirectUrl));
            }
        }

        [HttpGet]
        public HttpPostResponse LogOut()
        {
            string memrefno = "";
            try
            {
                HttpRequest newrequest = HttpContext.Current.Request;
                string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
                string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

                if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
                {
                    if (usermanager.ValidateToken(memRefNo, authenticationToken))
                    {
                        usermanager.LogOut(memRefNo, new Guid(authenticationToken));

                        User user = usermanager.GetUserAlldataByMemrefno(memRefNo);
                        UserSystemDetails systemdetails = usermanager.GetUserSystemConfiguration();
                        UserLogs.UserLog(Miscellaneous.CurrentDateTime(), user.UserID, "Logout Successful", "Success", systemdetails.IpAddress, systemdetails.Browser, systemdetails.OperatingSystem, systemdetails.ScreenResolution, "");
                        return (new HttpPostResponse { }.CreateResponse("Success", "Logout Successfully.", ""));
                    }
                    else
                    {
                        User user = usermanager.GetUserAlldataByMemrefno(memRefNo);
                        if (user != null)
                        {
                            ErrorLogs.ErrorLog(user.UserID, user.Email, Miscellaneous.CurrentDateTime(), "", "Logout Failed", "Logout", 2);
                        }
                        else
                        {
                            ErrorLogs.ErrorLog(0, "", Miscellaneous.CurrentDateTime(), "", "Logout Failed", "Logout", 2);
                        }
                        return (new HttpPostResponse { }.CreateResponse("Redirect", "Logout Failed", "Login"));
                    }
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Logout Failed", "Login"));
                }
            }
            catch (Exception ex)
            {
                User user = usermanager.GetUserAlldataByMemrefno(memrefno);
                if (user != null)
                {
                    ErrorLogs.ErrorLog(user.UserID, user.Email, Miscellaneous.CurrentDateTime(), "", ex.Message, "Logout", 1);
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "Guest", Miscellaneous.CurrentDateTime(), "", ex.Message, "Logout", 1);
                }
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Logout Failed", "Login"));
            }

        }

        [HttpGet]
        public HttpPostResponse SendMobilePin(string UserID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string messageresp = "";
                    User user = usermanager.GetUserAlldataByMemrefno(UserID);
                    user.GUIDCode = Miscellaneous.GetRandomCode(4);
                    usermanager.UpdateRegisterUser(user);
                    if (user != null)
                    {
                        if ((user.Mobile == null ? 0 : user.Mobile) != 0)
                        {
                            // send for sms
                            if (GeneralConfiguration.CheckConfiguration("UIIsSMS").ConfigurationValue == "1")
                            {
                                string message = "OTP for your password request on www.D1WebApp.com is " + user.GUIDCode + ". Do not share this OTP with anyone for security reasons. For queries, contact +91 79 4040 1234.";

                            }
                        }
                        if (!string.IsNullOrEmpty(user.Email))
                        {
                            // send for mail
                            if (GeneralConfiguration.CheckConfiguration("UIIsMail").ConfigurationValue == "1")
                            {

                            }
                        }
                        UserSystemDetails systemdetails = usermanager.GetUserSystemConfiguration();
                        UserLogs.UserLog(Miscellaneous.CurrentDateTime(), user.UserID, "Password Pin Successfully", "Success", systemdetails.IpAddress, systemdetails.Browser, systemdetails.OperatingSystem, systemdetails.ScreenResolution, "");
                        return (new HttpPostResponse { }.CreateResponse("Success", "Password Pin Successfully", "Pin Password " + messageresp));
                    }
                    else
                    {
                        string pusername = "Guest";
                        ErrorLogs.ErrorLog(0, pusername, Miscellaneous.CurrentDateTime(), "", "Password Pin Failed", "Pin Password", 2);
                        return (new HttpPostResponse { }.CreateResponse("Fail", "Password Pin Failed", ""));
                    }
                }
                else
                {
                    string pusername = "Guest";
                    User user = usermanager.GetUserAlldataByMemrefno(UserID);
                    if (user != null)
                    {
                        ErrorLogs.ErrorLog(user.UserID, user.Email, Miscellaneous.CurrentDateTime(), "", "Password Pin Failed", "Pin Password", 2);
                    }
                    else
                    {
                        ErrorLogs.ErrorLog(0, pusername, Miscellaneous.CurrentDateTime(), "", "Password Pin Failed", "Pin Password", 2);
                    }
                    return (new HttpPostResponse { }.CreateResponse("Fail", "Password Pin Failed", ""));
                }

            }
            catch (Exception ex)
            {
                User user = usermanager.GetUserAlldataByMemrefno(UserID);
                if (user != null)
                {
                    ErrorLogs.ErrorLog(user.UserID, user.Email, Miscellaneous.CurrentDateTime(), "", ex.Message, "Pin Password", 1);
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "Guest", Miscellaneous.CurrentDateTime(), "", ex.Message, "Pin Password", 1);
                }
                return (new HttpPostResponse { }.CreateResponse("Fail", ex.Message, ""));
            }
        }

        [HttpGet]
        public dynamic GetUserDetailsByMemrefno(long Memrefno)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    return usermanager.GetUserDetail(memRefNo);
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
            }
        }

        [HttpGet]
        public dynamic GetUserDtlByMemRefNo(long MemRefNo)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    return usermanager.GetUserDtlByMemRefNo(memRefNo);
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
            }
        }
        [HttpPost]
        public dynamic GetUserDtlListByMemRefNo(UserDetialViewModel userDtl)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    return usermanager.GetUserDtlListByMemRefNo(userDtl);
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
            }
        }
        [HttpPost]
        public dynamic InsertUserVisitTable(UserVisitViewModel uservisitvm)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    usermanager.InsertUserVisitUrldata(uservisitvm);
                    return (new HttpPostResponse { }.CreateResponse("", "Suncessful", ""));
                }
                else
                {
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
                }
            }
            else
            {
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid Token", "Login"));
            }
        }

        [HttpPost]
        public void InsertUniqueSessiondata(UniqueSessionViewModel uniquesessionvm)
        {
            usermanager.InsertUniqueSessiondata(uniquesessionvm);
        }

        [HttpPost]
        public void UpdateUniqueSessiondata(UniqueSessionViewModel uniquesessionvm)
        {
            usermanager.UpdateUniqueSessiondata(uniquesessionvm);
        }

    }
}
