//Developed by Nayan

using System.Web;
using System;
using D1WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.ViewModels;

namespace D1WebApp.Utilities
{
    public class UserManager
    {
        private UserRepository userrepository = new UserRepository(new D1WebAppEntities());
        private static Configuration _configUserLogOutDuration = GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UIUserLogoutTime");

        public UserManager()
        {
        }
        public bool DeleteUser(long UserID, bool active)
        {
            return userrepository.DeleteUser(UserID, active);
        }
        public long GetRoleByType(string type)
        {
            return userrepository.GetRoleByType(type);
        }
        public UserAccessTab GetRoleByUserId(long userId)
        {
            return userrepository.GetRoleByUserId(userId);
        }
        public UserInsertViewModel GetUserByMemRefNoForEdit(long UserID)
        {
            return userrepository.GetUserByMemRefNoForEdit(UserID);
        }
        public bool InsertVendorUserLoginDetails(List<VendorUserLoginDetail> VendorUserLoginList, D1WebAppEntities Contaxt1)
        {
            return userrepository.InsertVendorUserLoginDetails(VendorUserLoginList, Contaxt1);
        }
        public bool UpdateVendorUserLoginDetails(List<VendorUserLoginDetail> VendorUserLoginList)
        {
            return userrepository.UpdateVendorUserLoginDetails(VendorUserLoginList);
        }
        public bool InsertVendorUserLoginDetailsForSync(List<VendorUserLoginDetail> VendorUserLoginList)
        {
            return userrepository.InsertVendorUserLoginDetailsForSync(VendorUserLoginList);
        }
        public bool InsertNewTokenToUser(long UserID, string Atoken)
        {
            return userrepository.InsertNewTokenToUser(UserID, Atoken);
        }
        public bool InsertNewTokenToUserForInsert(long UserID, string Atoken, D1WebAppEntities Contaxt1)
        {
            return userrepository.InsertNewTokenToUserForInsert(UserID, Atoken, Contaxt1);
        }


        public List<UserListViewModel> GetUsersList(string type, string Email, long Mobile, string MemberRefNo, string HostName, int pageno)
        {
            return userrepository.GetUsersList(type, Email, Mobile, MemberRefNo, HostName, pageno);
        }
        //public bool InsertUserHomeData(string h1,string dbname)
        //{
        //    return userrepository.InsertUserHomeData(h1, dbname);
        //}
        //public string Getuserwebdata(string dbname)
        //{
        //    return userrepository.Getuserwebdata(dbname);
        //}
        public UserDetialViewModel GetUserDetail(string memrefno)
        {
            UserDetialViewModel getuser = new UserDetialViewModel();
            getuser = userrepository.GetUserDtlByMemRefNo(memrefno);
            return getuser;
        }

        public List<RoleViewModel> GetRolesList()
        {
            return userrepository.GetRolesList();
        }

        public long insertNewUser(UserInsertViewModel createnbewuser, D1WebAppEntities Context1)
        {
            return userrepository.insertNewUser(createnbewuser, Context1);
        }
        public bool InsertUserProject(long UserID, int ProjectTypeID, D1WebAppEntities Context1)
        {
            return userrepository.InsertUserProject(UserID, ProjectTypeID, Context1);
        }
        public bool inseruserroles(long userid, long roleid, long UserParent,string TabAccess, D1WebAppEntities Context1)
        {
            return userrepository.insertUserRoles(userid, roleid, UserParent, TabAccess, Context1);
        }
        public List<ProjectTypeViewModel> GetProjectTypelist()
        {
            return userrepository.GetProjectTypelist();
        }

        public bool ValidateToken(string memrefno, string token)
        {
            bool result = false;
            User getUser = IsSessionValid(memrefno, new Guid(token));
            if (getUser != null)
            {
                result = true;
            }

            return result;
        }
        public bool ValidateTokenVendor(long UserID, string Vendor)
        {
            bool result = false;
            VendorUserLoginDetail getUser = userrepository.GetUserFullDataByVendor(UserID, Vendor);
            if (getUser != null)
            {
                result = true;
            }

            return result;
        }
        public bool CHeckUserForMemRefNo(string MemRefNo, long UserID)
        {
            return userrepository.CheckUserForMemRefNo(MemRefNo, UserID);
        }

        public User GetUserToken(HttpRequest Request)
        {
            User user = null;

            string headercontent = Request.Headers["Authorization"];
            if (headercontent != null)
            {
                string[] tokenuser = null;
                string memrefno = "0";
                try
                {
                    string[] credentials = headercontent.Split(' ');
                    tokenuser = credentials[1].Split('/');
                    string token = tokenuser[0];
                    memrefno = tokenuser[1];
                    user = IsSessionValid(memrefno, new Guid(token));
                }
                catch (Exception ex)
                {
                    ErrorLogs.ErrorLog(user.UserID, "", Miscellaneous.CurrentDateTime(), "error in GetuserToken", ex.InnerException == null ? ex.Message.ToString() : ex.InnerException.ToString(), "Token " + tokenuser[1], 1);
                }
            }
            return user;

        }
        public bool CheckUserForMemRefNo(string MemRefno, long UserID)
        {
            return userrepository.CheckUserForMemRefNo(MemRefno, UserID);
        }

        public bool ActivateUser(UserViewModel userviewmodel)
        {
            bool result = false;
            try
            {
                User userdata = userrepository.GetUserDataByMemfernoGuid(userviewmodel.MemberRefNo, userviewmodel.GUIDCode);

                if (userdata != null)
                {
                    userdata.FailedAttemptCount = 0;
                    userdata.LastLoggedOn = Miscellaneous.CurrentDateTime();
                    Guid? token = userdata.AuthenticationToken;
                    if (token == null)
                        userdata.AuthenticationToken = Guid.NewGuid();
                    else
                        userdata.AuthenticationToken = token;

                    Configuration defaulttokentime = GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UIDefaultTokenExpireDurationMinutes");
                    userdata.TokenExpirsOn = Miscellaneous.CurrentDateTime().AddMinutes(Convert.ToDouble(defaulttokentime.ConfigurationValue));

                    userdata.IsActive = true;


                    userrepository.UpdateRegisterUser(userdata);
                    InsertUserSessionData(userdata);

                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void PasswordReset(ForgotPasswordViewModel forgotpasswordviewModel)
        {
            try
            {
                User userdata = userrepository.GetExternalUserDataByUsername(forgotpasswordviewModel.UserName);
                if (userdata != null)
                {
                    userdata.Password = forgotpasswordviewModel.Password;
                    userdata.IsPasswordReset = true;
                    userdata.IsLocked = false;
                    userdata.IsActive = true;
                    userrepository.UpdateRegisterUser(userdata);
                }
                else
                {
                    throw new Exception("Member not found.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string VendorUserLogin(string username, string password, long UserID)
        {
            return userrepository.VendorUserLogin(username, password, UserID);
        }

        public string[] UserLogin(LoginViewModel loginviewmodel)
        {
            int myResult = 0;
            string[] retVal = new string[] { "", "" };
            try
            {
                User userdata = userrepository.GetExternalUserDataByUsername(loginviewmodel.UserName);
                if (userdata != null)
                {
                    if (userdata.IsActive == true)
                    {
                        if (userdata.IsLocked == false)
                        {
                            if (userdata.Password.ToUpper().Equals(loginviewmodel.Password.ToUpper()))
                            {
                                userdata.FailedAttemptCount = 0;
                                userdata.LastLoggedOn = Miscellaneous.CurrentDateTime();
                                userdata.AuthenticationToken = Guid.NewGuid();
                                if (loginviewmodel.IsRememberPassword == true)
                                {
                                    Configuration remembertokentime = GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UIRememberTokenExpireDurationMinutes");
                                    userdata.TokenExpirsOn = Miscellaneous.CurrentDateTime().AddMinutes(Convert.ToDouble(remembertokentime.ConfigurationValue));
                                }
                                else
                                {
                                    Configuration defaulttokentime = GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UIDefaultTokenExpireDurationMinutes");
                                    userdata.TokenExpirsOn = Miscellaneous.CurrentDateTime().AddMinutes(Convert.ToDouble(defaulttokentime.ConfigurationValue));
                                }

                                if (IsUserLoginProvideAccess(userdata))
                                {

                                    userrepository.UpdateRegisterUser(userdata);
                                    myResult = 1;
                                }
                                else
                                {
                                    myResult = 2;
                                }
                            }
                            else
                            {
                                if (userdata.FailedAttemptCount >= 10)
                                {
                                    userdata.IsLocked = true;
                                    userrepository.UpdateRegisterUser(userdata);
                                    throw new Exception("Your account is locked.");
                                }
                                else
                                {
                                    userdata.FailedAttemptCount++;
                                    userrepository.UpdateRegisterUser(userdata);
                                    throw new Exception("Invalid username or password");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Your account is locked.");
                        }
                    }
                    else
                    {
                        //throw new Exception("User inactive.");
                    }
                }
                else
                {
                    throw new Exception("Invalid username or password");
                }
                retVal[0] = myResult.ToString();
                retVal[1] = userdata.AuthenticationToken.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }

        public bool IsUserLoginProvideAccess(User userdata)
        {
            bool result = false;

            Configuration configurations = new Configuration();
            configurations = GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UIUserLogoutTime");

            int ValidCount = 0;
            string value = configurations.ConfigurationValue;

            List<UserSession> listofUserSession = userrepository.GetUserSessionCurrentListByUserID(userdata.UserID);
            if (listofUserSession.Count > 0)
            {
                foreach (UserSession item in listofUserSession)
                {
                    DateTime chkvalidDate = item.TimeStamp.AddMinutes(Convert.ToDouble(value));
                    DateTime currentdatetime = Miscellaneous.CurrentDateTime();

                    if (chkvalidDate > currentdatetime)
                    {
                        ValidCount++;
                    }
                    else
                    {
                        //if record old then config value then set IsExpired flag to true
                        UpdateUserSessionData(userdata.UserID, item.AuthenticationToken, (int)Miscellaneous.UserSessionValidate.Expired_Session);
                    }
                }
            }

            if (ValidCount < userdata.SessionCount)
            {
                result = true;
                InsertUserSessionData(userdata);
            }
            return result;
        }

        public bool ChangePassword(ChangePasswordViewModel changepasswordviewmodel)
        {
            bool result = false;

            string memrefno = changepasswordviewmodel.UserName;

            User userdata = GetUserAlldataByMemrefno(memrefno);
            if (userdata != null)
            {
                if (userdata.Password.ToUpper().Equals(changepasswordviewmodel.CurrentPassword.ToUpper()))
                {
                    userdata.IsPasswordReset = false;
                    userdata.Password = changepasswordviewmodel.NewPassword;

                    userrepository.UpdateRegisterUser(userdata);

                    result = true;
                }
            }
            return result;
        }

        public bool ChangePasswordForVendor(ChangePasswordViewModel changepasswordviewmodel, long UserID, string Vendor)
        {
            bool result = false;

            string memrefno = changepasswordviewmodel.UserName;

            VendorUserLoginDetail userdata = userrepository.GetUserFullDataByVendor(UserID, Vendor);
            if (userdata != null)
            {
                if (userdata.web_passwd.ToUpper().Equals(changepasswordviewmodel.CurrentPassword.ToUpper()))
                {

                    userdata.web_passwd = changepasswordviewmodel.NewPassword;

                    userrepository.UpdateRegisterVendorUser(userdata);

                    result = true;
                }
            }
            return result;
        }

        public UserSystemDetails GetUserSystemConfiguration()
        {
            UserSystemDetails systemdetails = new UserSystemDetails();
            systemdetails.IpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            systemdetails.Browser = HttpContext.Current.Request.Browser.Browser;
            systemdetails.OperatingSystem = HttpContext.Current.Request.Browser.Platform;
            return systemdetails;
        }

        public User IsSessionValid(string memrefno, System.Guid authenticationtoken)
        {
            bool result = false;

            User userdata = GetUserAlldataByMemrefno(memrefno);
            if (userdata != null)
            {
                long userid = userdata.UserID;

                string value = _configUserLogOutDuration.ConfigurationValue;

                UserSession usersession = userrepository.GetUserSessionDataByUseridNtoken(userid, authenticationtoken);

                if (usersession == null)
                    result = false;
                else
                {
                    if (!usersession.IsExpired)
                    {
                        DateTime chkvalidDate = usersession.TimeStamp.AddMinutes(Convert.ToDouble(value));
                        DateTime currentdatetime = Miscellaneous.CurrentDateTime();

                        if (chkvalidDate > currentdatetime)
                        {
                            result = true;
                            UpdateUserSessionData(userid, authenticationtoken, (int)Miscellaneous.UserSessionValidate.Extend_Session);
                        }
                        else
                            UpdateUserSessionData(userid, authenticationtoken, (int)Miscellaneous.UserSessionValidate.Expired_Session);
                    }
                }
            }
            if (!result)
                userdata = null;
            return userdata;
        }

        public void LogOut(string memrefno, System.Guid authenticationtoken)
        {
            User userdata = GetUserAlldataByMemrefno(memrefno);
            if (userdata != null)
            {
                UpdateUserSessionData(userdata.UserID, authenticationtoken, (int)Miscellaneous.UserSessionValidate.Expired_Session);

                userdata.AuthenticationToken = null;
                userdata.TokenExpirsOn = null;

                userrepository.UpdateRegisterUser(userdata);
            }
        }



        public bool IsUserTokenValidate(string reqToken)
        {
            bool flag = false;
            flag = userrepository.GetInternalUsertokenListByUserID(Convert.ToInt64(reqToken));

            if (!flag)
                ErrorLogs.ErrorLog(0, "Cache key mismatch", Miscellaneous.CurrentDateTime(), "Log : token API List", reqToken, "Caching Delete Log", 1);

            return flag;
        }

        public void InsertUserSessionData(User userdata)
        {
            UserSession usersession = new UserSession();
            usersession.UserID = userdata.UserID;
            usersession.AuthenticationToken = userdata.AuthenticationToken.Value;
            usersession.TimeStamp = Miscellaneous.CurrentDateTime();
            usersession.IsExpired = false;
            usersession.IpAddresss = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            usersession.Browser = HttpContext.Current.Request.Browser.Browser;
            userrepository.InsertUserSessionData(usersession);
        }

        //type 1 : for Logout , type 2 : for validate token - change date
        public void UpdateUserSessionData(long userid, System.Guid authenticationtoken, int type)
        {
            UserSession oldusersession = userrepository.GetUserSessionDataByUseridNtoken(userid, authenticationtoken);
            if (oldusersession != null)
            {
                UserSession newusersession = new UserSession();
                newusersession = oldusersession;
                if (type == (int)Miscellaneous.UserSessionValidate.Expired_Session)
                    newusersession.IsExpired = true;
                else if (type == (int)Miscellaneous.UserSessionValidate.Extend_Session)
                {
                    newusersession.IsExpired = false;
                    newusersession.TimeStamp = Miscellaneous.CurrentDateTime();
                }
                userrepository.UpdateUserSessionData(newusersession);
            }
        }

        public bool GetCheckMobileNo(decimal mobileno)
        {
            User user = userrepository.GetExternalUserDataByMobileNo(mobileno);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool GetPublishAllwebsites()
        {
            return userrepository.GetPublishAllwebsites();
        }



        public bool GetCheckEmailID(string emailid)
        {
            User user = userrepository.GetExternalUserDataByEmailID(emailid);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public dynamic GetUserByID(long UserID)
        {
            User user = userrepository.GetUserByUserID(UserID);
            if (user != null)
            {
                return user;

            }
            else
            {
                return null;
            }

        }

        public dynamic GetUserByEmail(string emailid)
        {
            User user = userrepository.GetExternalUserDataByEmailID(emailid);
            if (user != null)
            {
                return new UserViewModel(user);

            }
            else
            {
                return null;
            }

        }

        public User GetUserAlldataByMemrefno(string memrefno)
        {
            User getUserFulldata = userrepository.GetUserFullDataByMemrefNo(memrefno);
            return getUserFulldata;
        }

        public dynamic GetProjectbyUserid(long Userid)
        {
            return userrepository.GetProjectByUserID(Userid);
        }

        public VendorUserLoginDetail GetUserAlldataByVendor(long UserID, string Vendor)
        {
            VendorUserLoginDetail getUserFulldata = userrepository.GetUserFullDataByVendor(UserID, Vendor);
            return getUserFulldata;
        }



        public User GetExternalUserDataByUsername(string username)
        {
            User userdata = userrepository.GetExternalUserDataByUsername(username);
            return userdata;
        }

        public bool GetCheckUser(string username, decimal mobileno)
        {
            User user = new User();
            if (username != "")
            {
                user = userrepository.GetExternalUserDataByUserNameandMobileNo(username, mobileno);
            }
            else
            {
                user = userrepository.GetExternalUserDataByMobileNo(mobileno);
            }
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public long GetMemberRefNo()
        {
            long memrefno = userrepository.GetMemberRefNo();
            return memrefno;
        }









        public UserDetialViewModel GetUserDtlByMemRefNo(string MemRefNo)
        {
            return userrepository.GetUserDtlByMemRefNo(MemRefNo);
        }

        public List<UserDetialViewModel> GetUserDtlListByMemRefNo(UserDetialViewModel userDtl)
        {
            return userrepository.GetUserDtlListByMemRefNo(userDtl);
        }



        public DateTime GetLastLogoutLoginTime(User userdata)
        {
            return userrepository.GetLastLogoutLoginTime(userdata.UserID);
        }



        public void InsertUserVisitUrldata(UserVisitViewModel uservisitvm)
        {
            UserVisit uservisitdata = new UserVisit();
            uservisitdata.UniqueID = uservisitvm.UniqueID;
            uservisitdata.UrlName = uservisitvm.UrlName;
            uservisitdata.MemRefNo = uservisitvm.MemRefNo;
            uservisitdata.IpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            uservisitdata.Browser = HttpContext.Current.Request.Browser.Browser;
            uservisitdata.VisitTime = Miscellaneous.CurrentDateTime();
            Task.Run(() => userrepository.InsertUserVisitUrlTableData(uservisitdata));
        }

        public void InsertUniqueSessiondata(UniqueSessionViewModel uniquesessionvmdata)
        {
            bool flag = false;
            UniqueSession setuniquesessiondata = new UniqueSession();

            List<UniqueSession> olduniquesessiondatalist = userrepository.GetUniqueSessionData(uniquesessionvmdata.UniqueID);
            if (olduniquesessiondatalist.Count > 0)
            {
                foreach (UniqueSession item in olduniquesessiondatalist)
                {
                    if (item.MemRefNo == uniquesessionvmdata.MemRefNo)
                    {
                        flag = false;
                        break;
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            else
            {
                flag = true;
            }
            if (flag)
            {
                setuniquesessiondata.UniqueID = uniquesessionvmdata.UniqueID;
                setuniquesessiondata.MemRefNo = uniquesessionvmdata.MemRefNo;
                setuniquesessiondata.CreatedOn = Miscellaneous.CurrentDateTime();
                setuniquesessiondata.IpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                setuniquesessiondata.Browser = HttpContext.Current.Request.Browser.Browser;

                userrepository.InsertUniqueSessionData(setuniquesessiondata);
            }
        }

        public void UpdateUniqueSessiondata(UniqueSessionViewModel uniquesessionvmdata)
        {
            UniqueSession olduniquesessiondata = userrepository.GetUniqueSessionsingleData(uniquesessionvmdata.UniqueID);
            if (olduniquesessiondata != null)
            {
                UniqueSession newuniquesessiondata = olduniquesessiondata;
                newuniquesessiondata.MemRefNo = uniquesessionvmdata.MemRefNo;

                userrepository.UpdateUniqueSessionData(olduniquesessiondata, newuniquesessiondata);
            }
        }

        public UserSession GetAuthenticationTokenData(long userid)
        {
            return userrepository.GetUserSessionData(userid);
        }

        public void UpdateRegisterUser(User userdata)
        {
            userrepository.UpdateRegisterUser(userdata);
        }

        //This method used to expire all session data for perticular user : At Change Password time
        public void GetActiveUserSessionData(long userid)
        {
            List<UserSession> activeusersessionlist = userrepository.GetUserSessionCurrentListByUserID(userid);
            if (activeusersessionlist.Count > 0)
            {
                foreach (UserSession oldusersession in activeusersessionlist)
                {
                    UserSession newusersession = new UserSession();
                    newusersession = oldusersession;
                    newusersession.IsExpired = true;
                    userrepository.UpdateUserSessionData(newusersession);
                }
            }
        }
    }
}