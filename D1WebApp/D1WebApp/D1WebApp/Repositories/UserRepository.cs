//Developed by Nayan

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

using D1WebApp.ViewModels;
using System.IO;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private D1WebAppEntities context;
        public UserRepository(D1WebAppEntities context)
        {
            this.context = context;
        }
        public bool CheckUserForMemRefNo(string MemRefno, long UserID)
        {
            bool flag = true;
            User getuser = new User();
            if (UserID > 0)
            {
                getuser = context.Users.Where(cp => cp.MemberRefNo.Equals(MemRefno) && cp.UserID != UserID).FirstOrDefault();
            }
            else
            {
                getuser = context.Users.Where(cp => cp.MemberRefNo.Equals(MemRefno)).FirstOrDefault();
            }
            if (getuser != null && getuser.UserID > 0)
            {
                flag = false;
            }
            return flag;
        }

        public bool DeleteUser(long UserID, bool active)
        {
            bool flag = false;
            try
            {
                var userrole = context.UserRoles.Where(cp => cp.UserID == UserID).First();
                context.UserRoles.Remove(userrole);
                var varlogin = context.VendorUserLoginDetails.Where(cp => cp.UserID == UserID).ToList();
                if (varlogin != null && varlogin.Count() > 0)
                {
                    context.VendorUserLoginDetails.RemoveRange(varlogin);
                }
                var audits = context.AuditTrails.Where(cp => cp.UserID == UserID).ToList();
                if (audits != null && audits.Count() > 0)
                {
                    context.AuditTrails.RemoveRange(audits);
                }
                var error = context.ErrorLogs.Where(cp => cp.UserID == UserID).ToList();
                if (error != null && error.Count() > 0)
                {
                    context.ErrorLogs.RemoveRange(error);
                }
                var mails = context.UserMailBoxes.Where(cp => cp.UserID == UserID).ToList();
                if (mails != null && mails.Count() > 0)
                {
                    context.UserMailBoxes.RemoveRange(mails);
                }
                var userlogs = context.UserLogs.Where(cp => cp.UserID == UserID).ToList();
                if (userlogs != null && userlogs.Count() > 0)
                {
                    context.UserLogs.RemoveRange(userlogs);
                }
                var uesrproce = context.UserProcessTimes.Where(cp => cp.UserID == UserID).ToList();
                if (uesrproce != null && uesrproce.Count() > 0)
                {
                    context.UserProcessTimes.RemoveRange(uesrproce);
                }
                var userproj = context.UserProjectMappings.Where(cp => cp.UserID == UserID).ToList();
                if (userproj != null && userproj.Count() > 0)
                {
                    context.UserProjectMappings.RemoveRange(userproj);
                }
                var usersess = context.UserSessions.Where(cp => cp.UserID == UserID).ToList();
                if (usersess != null && usersess.Count() > 0)
                {
                    context.UserSessions.RemoveRange(usersess);
                }

                var getisers = context.Users.Find(UserID);
                context.Users.Remove(getisers);
                context.SaveChanges();
                flag = true;
            }
            catch (Exception ed) { flag = false; }
            return flag;
        }


        public List<RoleViewModel> GetRolesList()
        {
            List<Role> getrolelist = new List<Role>();
            getrolelist = context.Roles.Where(cp => cp.IsActive == true).ToList();
            List<RoleViewModel> GetRolesList = RoleViewModel.ConvertModelToViewModel(getrolelist);
            return GetRolesList;
        }

        public long GetRoleByType(string type)
        {
            Role getrole = new Role();
            if (!string.IsNullOrEmpty(type))
            {
                getrole = context.Roles.Where(cp => cp.RoleName.Equals(type)).First();
            }

            return getrole.RoleID;
        }

        public UserAccessTab GetRoleByUserId(long userId)
        {
            UserAccessTab role = new UserAccessTab();
            try
            {
                string roleType = (from ur in context.UserRoles
                                   join r in context.Roles on ur.RoleID equals r.RoleID
                                   where ur.UserID == userId
                                   select r.RoleName).FirstOrDefault();

                if(roleType == "Subuser")
                {
                    role = (from ur in context.UserRoles
                            join u in context.Users on ur.UserParent equals u.UserID 
                            //into pusr from p in pusr.DefaultIfEmpty()
                            join r in context.Roles on ur.RoleID equals r.RoleID
                            join pu in context.Users on ur.UserParent equals pu.UserID
                            where ur.UserID == userId
                            select new UserAccessTab
                            {
                                RoleName = roleType,
                                TabAccess = ur.TabAccess != null ? ur.TabAccess : "",
                                UserParent = (long)ur.UserParent,
                                UserParentMemberRefNo = pu.MemberRefNo,
                            }).FirstOrDefault();
                }
                else
                {
                    role = (from ur in context.UserRoles
                            join r in context.Roles on ur.RoleID equals r.RoleID
                            join u in context.Users on ur.UserID equals u.UserID 
                            where ur.UserID == userId
                            select new UserAccessTab
                            {
                                RoleName = roleType,
                                TabAccess = "",
                                UserParent = 0,
                                UserParentMemberRefNo = u.MemberRefNo,
                            }).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {

            }
         
            //role = (from ur in context.UserRoles
            //               join r in context.Roles on ur.RoleID equals r.RoleID
            //               join u in context.Users on ur.UserParent equals u.UserID
            //               where ur.UserID == userId
            //               select new UserAccessTab
            //               {
            //                   RoleName = r.RoleName,
            //                   TabAccess = ur.TabAccess,
            //                   UserParent = (long)ur.UserParent,
            //                   UserParentMemberRefNo = u.MemberRefNo,
            //               }).First();
            return role;
        }

        public UserInsertViewModel GetUserByMemRefNoForEdit(long UserID)
        {
            UserInsertViewModel GetUserByMem = new UserInsertViewModel();
            GetUserByMem = (from ur in context.Users
                            join urol in context.UserRoles on ur.UserID equals urol.UserID
                            join pt in context.UserProjectMappings on ur.UserID equals pt.UserID into pts
                            from ptypes in pts.DefaultIfEmpty()
                            join rr in context.Roles on urol.RoleID equals rr.RoleID
                            where ur.UserID == UserID
                            select new UserInsertViewModel
                            {
                                AuthenticationToken = ur.AuthenticationToken.ToString(),
                                Email = ur.Email,
                                FailedAttemptCount = ur.FailedAttemptCount,
                                FirstName = ur.FirstName,
                                GUIDCode = ur.GUIDCode.Value,
                                HostName = ur.HostName,
                                IsActive = ur.IsActive,
                                IsLocked = ur.IsLocked,
                                IsPasswordReset = ur.IsPasswordReset,
                                IsRemember = ur.IsRemember.Value,
                                LastLoggedOn = ur.LastLoggedOn,
                                LastName = ur.LastName,
                                LastUpdatedBy = ur.LastUpdatedBy,
                                LastUpdatedOn = ur.LastUpdatedOn,
                                LockedOn = ur.LockedOn,
                                MemberRefNo = ur.MemberRefNo,
                                Mobile = ur.Mobile,
                                Password = ur.Password,
                                RoleID = rr.RoleID,
                                SessionCount = ur.SessionCount,
                                TokenExpirsOn = ur.TokenExpirsOn,
                                UserID = ur.UserID,
                                ApiPassword = ur.ApiPwd,
                                ApiUserName = ur.ApiUserName,
                                ProjectTypeID = (ptypes != null ? ptypes.ProjectTypeID : 0),
                                RoleName = rr.RoleName,
                                CompanyID = ur.CompanyID,
                                ApiEndPoint = ur.ApiEndPoint,
                                NotificationEmails = ur.NotificationEmails,
                                UserParent = (long)urol.UserParent,
                                TabAccess = urol.TabAccess,
                            }).FirstOrDefault();
            return GetUserByMem;
        }



        public long insertNewUser(UserInsertViewModel createnbewuser, D1WebAppEntities Context1)
        {
            if (createnbewuser.UserID != null && createnbewuser.UserID > 0)
            {
                User newuser = createnbewuser.GetNewUser(createnbewuser);
                User olduser = Context1.Users.Find(createnbewuser.UserID);
                newuser.AuthenticationToken = olduser.AuthenticationToken;
                newuser.FailedAttemptCount = olduser.FailedAttemptCount;
                if (!string.IsNullOrEmpty(createnbewuser.Password))
                {
                    newuser.Password = createnbewuser.Password;
                }
                else
                {
                    newuser.Password = olduser.Password;
                }
                newuser.GUIDCode = olduser.GUIDCode;
                newuser.CompanyID = olduser.CompanyID;
                newuser.HostName = olduser.HostName;
                newuser.ApiEndPoint = olduser.ApiEndPoint;
                newuser.ApiUserName = olduser.ApiUserName;
                newuser.ApiPwd = olduser.ApiPwd;
                newuser.IsPasswordReset = olduser.IsPasswordReset;
                newuser.IsRemember = olduser.IsRemember;
                newuser.LastLoggedOn = olduser.LastLoggedOn;
                newuser.LastUpdatedOn = DateTime.Now;
                newuser.LockedOn = olduser.LockedOn;
                newuser.MemberRefNo = olduser.MemberRefNo;
                newuser.SessionCount = olduser.SessionCount;
                newuser.TokenExpirsOn = olduser.TokenExpirsOn;

                Context1.Entry(olduser).CurrentValues.SetValues(newuser);
                Context1.SaveChanges();
                return newuser.UserID;
            }
            else
            {
                User newuser = createnbewuser.GetNewUser(createnbewuser);
                Context1.Users.Add(newuser);
                Context1.SaveChanges();
                return newuser.UserID;
            }
        }

        public List<UserListViewModel> GetUsersList(string type, string Email, long Mobile, string MemberRefNo, string HostName, int pageno)
        {
            List<UserListViewModel> GetUsersList = new List<UserListViewModel>();
            if (!string.IsNullOrEmpty(Email) || Mobile > 0 || !string.IsNullOrEmpty(MemberRefNo) || !string.IsNullOrEmpty(HostName))
            {
                var getusers = context.Users.AsQueryable();
                if (!string.IsNullOrEmpty(Email))
                {
                    getusers = getusers.Where(cp => cp.Email.Equals(Email)).AsQueryable();
                }
                if (Mobile > 0)
                {
                    getusers = getusers.Where(cp => cp.Mobile == Mobile).AsQueryable();
                }
                if (!string.IsNullOrEmpty(MemberRefNo))
                {
                    getusers = getusers.Where(cp => cp.MemberRefNo.Equals(MemberRefNo)).AsQueryable();
                }
                if (!string.IsNullOrEmpty(HostName))
                {
                    getusers = getusers.Where(cp => cp.HostName.Equals(HostName)).AsQueryable();
                }

                int counts = (from ur in getusers
                              join urr in context.UserRoles on ur.UserID equals urr.UserID
                              join ro in context.Roles on urr.RoleID equals ro.RoleID
                              where ro.RoleName.Equals(type)
                              select ur).Distinct().Count();
                GetUsersList = (from ur in getusers
                                join urr in context.UserRoles on ur.UserID equals urr.UserID
                                join ro in context.Roles on urr.RoleID equals ro.RoleID
                                where ro.RoleName.Equals(type)
                                select new UserListViewModel
                                {
                                    TotalPage = counts,
                                    Email = ur.Email,
                                    FirstName = ur.FirstName,
                                    IsActive = ur.IsActive,
                                    IsLocked = ur.IsLocked,
                                    LastName = ur.LastName,
                                    MemberRefNo = ur.MemberRefNo,
                                    Mobile = ur.Mobile,
                                    RoleName = ro.RoleName,
                                    UserID = ur.UserID,
                                }).Take(50).ToList();
            }
            else
            {
                if (pageno == 0)
                {
                    pageno = 1;
                }
                int counts = (from ur in context.Users
                              join urr in context.UserRoles on ur.UserID equals urr.UserID
                              join ro in context.Roles on urr.RoleID equals ro.RoleID
                              where ro.RoleName.Equals(type)
                              select ur).Distinct().Count();
                GetUsersList = (from ur in context.Users
                                join urr in context.UserRoles on ur.UserID equals urr.UserID
                                join ro in context.Roles on urr.RoleID equals ro.RoleID
                                where ro.RoleName.Equals(type)
                                select new UserListViewModel
                                {
                                    TotalPage = counts,
                                    Email = ur.Email,
                                    FirstName = ur.FirstName,
                                    IsActive = ur.IsActive,
                                    IsLocked = ur.IsLocked,
                                    LastName = ur.LastName,
                                    MemberRefNo = ur.MemberRefNo,
                                    Mobile = ur.Mobile,
                                    RoleName = ro.RoleName,
                                    UserID = ur.UserID,
                                }).OrderByDescending(c => c.UserID).Skip((pageno - 1) * 15).Take(15).ToList();
            }

            return GetUsersList;
        }

        public bool InsertUserProject(long UserID, int ProjectTypeID, D1WebAppEntities Context1)
        {
            UserProjectMapping userproj = new UserProjectMapping();
            userproj = Context1.UserProjectMappings.Where(cp => cp.UserID == UserID).FirstOrDefault();
            if (userproj != null)
            {
                userproj.ProjectTypeID = ProjectTypeID;
                Context1.Entry(userproj).State = System.Data.Entity.EntityState.Modified;
                Context1.SaveChanges();
                return true;
            }
            else
            {
                userproj = new UserProjectMapping();
                userproj.ProjectTypeID = ProjectTypeID;
                userproj.UserID = UserID;
                Context1.UserProjectMappings.Add(userproj);
                Context1.SaveChanges();
                return true;

            }
        }

        public List<ProjectTypeViewModel> GetProjectTypelist()
        {
            List<ProjectTypeViewModel> GetProjectTypelist = new List<ProjectTypeViewModel>();
            GetProjectTypelist = (from pt in context.ProjectTypes
                                  select new ProjectTypeViewModel
                                  {
                                      ProjectTypeID = pt.ProjectID,
                                      ProjectTypeName = pt.ProjectType1,
                                  }).ToList();

            return GetProjectTypelist;
        }
        public bool insertUserRoles(long Userid, long RoleID,long UserParent ,string TabAccess, D1WebAppEntities Context1)
        {
            UserRole getuserrole = new UserRole();
            getuserrole = Context1.UserRoles.Where(cp => cp.UserID == Userid).FirstOrDefault();
            if (getuserrole != null)
            {
                getuserrole.RoleID = RoleID;
                getuserrole.TabAccess = TabAccess;
                getuserrole.ModifiedOn = DateTime.Now;
                Context1.Entry(getuserrole).State = System.Data.Entity.EntityState.Modified;
                Context1.SaveChanges();
                return true;
            }
            else
            {
                getuserrole = new UserRole();
                getuserrole.RoleID = RoleID;
                getuserrole.UserID = Userid;
                getuserrole.TabAccess = TabAccess;
                getuserrole.CreatedOn = DateTime.Now;
                getuserrole.ModifiedOn = DateTime.Now;
                getuserrole.UserParent = UserParent;
                Context1.UserRoles.Add(getuserrole);
                Context1.SaveChanges();
                return true;
            }
        }
        public User GetExternalUserDataByUsername(string username)
        {
            User getuser = new User();
            getuser = context.Users.Where(c => c.MemberRefNo.Equals(username)).FirstOrDefault();
            return getuser;
        }

        public string VendorUserLogin(string username, string password, long UserID)
        {
            string status = string.Empty;
            try
            {
                var getuser = context.VendorUserLoginDetails.Where(cp => cp.vendor.Equals(username) && cp.web_passwd.ToUpper().Equals(password.ToUpper()) && cp.UserID == UserID).FirstOrDefault();
                if (getuser != null && getuser.UserID > 0)
                {
                    status = getuser.vendor;
                }
                else
                {
                    status = "Failed";
                }

            }
            catch (Exception ed)
            {
                status = "Failed";
            }
            return status;
        }

        public List<UserViewModel> GetActiveUsersByInternalExternal(byte typeofuser)
        {
            List<User> getall = context.Users.Where(ur => ur.IsActive == true).OrderByDescending(u => u.UserID).ToList();
            return (UserViewModel.ConvertToViewModelList(getall));
        }



        public User GetUserByUserID(long UserID)
        {
            User NewUser = new User();
            NewUser = context.Users.Find(UserID);
            return NewUser;
        }
        public long RegisterUser(User userdata)
        {
            context.Users.Add(userdata);
            context.SaveChanges();

            return userdata.UserID;
        }


        public void UpdateRegisterUser(User userdata)
        {
            context.Entry(userdata).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }
        public void UpdateRegisterVendorUser(VendorUserLoginDetail userdata)
        {
            context.Entry(userdata).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public long GetUserIDByMemRefNo(string MemRefNo)
        {
            return context.Users.SingleOrDefault(a => a.MemberRefNo.Equals(MemRefNo)).UserID;
        }



        public User GetUserDataByMemfernoGuid(string memrefno, decimal guidcode)
        {
            User user = context.Users.SingleOrDefault(a => a.MemberRefNo.Equals(memrefno) && a.GUIDCode == guidcode && a.IsActive == false);
            return user;
        }

        public List<UserSession> GetUserSessionCurrentListByUserID(long userid)
        {
            List<UserSession> listofUserSession = context.UserSessions.Where(ussesn => ussesn.UserID == userid && ussesn.IsExpired == false).ToList();
            return listofUserSession;
        }

        public UserSession GetUserSessionDataByUseridNtoken(long userid, System.Guid authenticationtoken)
        {
            UserSession usersession = context.UserSessions.Where(urs => urs.UserID == userid && urs.AuthenticationToken == authenticationtoken).FirstOrDefault();
            return usersession;
        }







        public bool GetInternalUsertokenListByUserID(long userid)
        {
            User tokenlist = new User();
            tokenlist = context.Users.Where(us => us.UserID == userid).FirstOrDefault();
            if (tokenlist != null && tokenlist.UserID > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InsertUserSessionData(UserSession usersessiondata)
        {
            context.UserSessions.Add(usersessiondata);
            Save();
        }

        public void UpdateUserSessionData(UserSession usersessiondata)
        {
            context.Entry(usersessiondata).State = System.Data.Entity.EntityState.Modified;
            Save();
        }






        public dynamic GetProjectByUserID(long UserID)
        {
            return context.UserProjectMappings.Where(cp => cp.UserID == UserID).FirstOrDefault();
        }
        public User GetUserFullDataByMemrefNo(string memrefno)
        {
            User userdata = context.Users.AsNoTracking().FirstOrDefault(a => a.MemberRefNo.Equals(memrefno));
            return userdata;
        }
        public VendorUserLoginDetail GetUserFullDataByVendor(long UserID, string Vendor)
        {
            VendorUserLoginDetail userdata = context.VendorUserLoginDetails.AsNoTracking().FirstOrDefault(a => a.UserID == UserID && a.vendor.Equals(Vendor));
            return userdata;
        }


        public bool GetPublishAllwebsites()
        {
            try
            {
                var getuserlist = (from ur in context.Users.AsNoTracking()
                                   join pm in context.UserProjectMappings.AsNoTracking() on ur.UserID equals pm.UserID
                                   where pm.ProjectTypeID != 2 && pm.ProjectTypeID != 3
                                   select ur).Distinct().ToList();
                foreach (var useritem in getuserlist)
                {
                    var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirEcom");
                    string targetDirectory = targetURL.ConfigurationValue + useritem.MemberRefNo.ToString();
                    string UIpath = targetDirectory + "\\" + useritem.MemberRefNo.ToString() + "UI";
                    var myFiles = Directory.GetFiles(UIpath, "*.*");
                    foreach (var item in myFiles)
                    {
                        if (item.Contains(".js") || item.Contains(".css") || item.Contains(".html"))
                        {
                            File.Delete(item);
                        }
                    }
                    var sourceURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("sourceDirEcom");
                    var EcomUI = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("EcomUIName");
                    string sourcepath = sourceURL.ConfigurationValue + EcomUI.ConfigurationValue;
                    var myFiles1 = Directory.GetFiles(sourcepath, "*.*");
                    foreach (var item in myFiles1)
                    {
                        if (item.Contains(".js") || item.Contains(".css") || item.Contains(".html"))
                        {
                            string[] str = item.Split('\\');
                            string newp = UIpath + "\\"+ str[str.Length - 1];
                            File.Copy(item, newp);
                        }
                    }                    
                    var mainfile = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("EcommerceMainFile");
                    string text1 = File.ReadAllText(UIpath + "\\" + mainfile.ConfigurationValue);
                    var AdminURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UserApiPath");
                    text1 = text1.Replace("{{AdminURL}}", AdminURL.ConfigurationValue + useritem.MemberRefNo.ToString() + "API").Replace("{{cookname}}", useritem.MemberRefNo.ToString());
                    File.WriteAllText(UIpath + "\\" + mainfile.ConfigurationValue, text1);

                    var EcomApiName = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("EcomApiName");
                    targetDirectory = targetURL.ConfigurationValue + useritem.MemberRefNo.ToString();
                    
                    string Apinewpath = targetDirectory + "\\" + useritem.MemberRefNo.ToString() + "API" + "\\bin";
                    Directory.Delete(Apinewpath, true);
                    CopyDir.Copy(sourceURL.ConfigurationValue + EcomApiName.ConfigurationValue + "\\bin", Apinewpath);

                    string Apinewpath1 = targetDirectory + "\\" + useritem.MemberRefNo.ToString() + "API" + "\\Models";
                    Directory.Delete(Apinewpath1, true);
                    CopyDir.Copy(sourceURL.ConfigurationValue + EcomApiName.ConfigurationValue + "\\Models", Apinewpath1);

                    //var Sqlserver = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("SqlServer");
                    //string text = File.ReadAllText(Apinewpath + "\\web.config");
                    //string newNameFullPath = targetDirectory + "\\" + useritem.MemberRefNo.ToString() + ".mdf";
                    //text = text.Replace("{{DBName}}", newNameFullPath).Replace("{{Sqlserver}}", Sqlserver.ConfigurationValue);
                    //File.WriteAllText(Apinewpath + "\\web.config", text);


                }
                return true;
            }catch(Exception ed)
            {
                return false;
            }
        }

        public User GetExternalUserDataByEmailID(string email)
        {
            int UserType = (byte)Miscellaneous.UserType.Client;
            User userdata = context.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
            return userdata;
        }

        public User GetExternalUserDataByMobileNo(decimal MObileno)
        {
            int UserType = (byte)Miscellaneous.UserType.Client;
            User userdata = context.Users.Where(u => u.Mobile == MObileno).FirstOrDefault();
            return userdata;
        }

        public bool InsertVendorUserLoginDetails(List<VendorUserLoginDetail> VendorUserLoginList, D1WebAppEntities Contaxt1)
        {
            bool flag = false;
            try
            {
                Contaxt1.VendorUserLoginDetails.AddRange(VendorUserLoginList);
                Contaxt1.SaveChanges();
                flag = true;
            }
            catch (Exception ed)
            {

            }
            return flag;
        }

        public bool UpdateVendorUserLoginDetails(List<VendorUserLoginDetail> VendorUserLoginList)
        {
            bool flag = false;
            try
            {
                if (VendorUserLoginList != null && VendorUserLoginList.Count() > 0)
                {
                    var oldverndorlist = context.VendorUserLoginDetails.ToList();
                    if (oldverndorlist != null && oldverndorlist.Count() > 0)
                    {
                        context.VendorUserLoginDetails.RemoveRange(oldverndorlist);
                        context.SaveChanges();
                    }
                    context.VendorUserLoginDetails.AddRange(VendorUserLoginList);
                    context.SaveChanges();
                }
                flag = true;

            }
            catch (Exception ed)
            {

            }
            return flag;
        }

        public bool InsertVendorUserLoginDetailsForSync(List<VendorUserLoginDetail> VendorUserLoginList)
        {
            bool flag = false;
            try
            {
                foreach (var item in VendorUserLoginList)
                {
                    var getchek = context.VendorUserLoginDetails.Where(cp => cp.CompanyID == item.CompanyID && cp.UserID == item.UserID && cp.vendor == item.vendor).FirstOrDefault();
                    if (getchek == null)
                    {
                        context.VendorUserLoginDetails.Add(item);
                        Save();
                    }
                }
                flag = true;
            }
            catch (Exception ed)
            {

            }
            return flag;
        }

        public bool InsertNewTokenToUser(long UserID, string Atoken)
        {
            bool flag = false;
            try
            {
                var getolduser = context.Users.Find(UserID);
                var getnewuser = context.Users.Find(UserID);
                getnewuser.ApiToken = Atoken;
                context.Entry(getolduser).CurrentValues.SetValues(getnewuser);
                Save();
                flag = true;
            }
            catch (Exception ed)
            {
                flag = false;
            }
            return flag;
        }
        public bool InsertNewTokenToUserForInsert(long UserID, string Atoken, D1WebAppEntities context1)
        {
            bool flag = false;
            try
            {
                var getolduser = context1.Users.Find(UserID);
                var getnewuser = context1.Users.Find(UserID);
                getnewuser.ApiToken = Atoken;
                context1.Entry(getolduser).CurrentValues.SetValues(getnewuser);
                Save();
                flag = true;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "InsertNewTokenToUserForInsert", DateTime.Now, "InsertNewTokenToUserForInsert", ed.ToString(), "InsertNewTokenToUserForInsert", 2);
                flag = false;
            }
            return flag;
        }


        public User GetExternalUserDataByUserNameandMobileNo(string username, decimal mobileno)
        {
            int UserType = (byte)Miscellaneous.UserType.Client;
            User userdata = context.Users.Where(u => (u.Mobile == mobileno || (u.Email == null ? u.Mobile == mobileno : u.Email.Equals(username)))).FirstOrDefault();
            return userdata;
        }

        public long GetMemberRefNo()
        {
            long memrefno = 0;
            while (true)
            {
                memrefno = Miscellaneous.GetRandomCodeNo(5, 5);
                var chkuser = context.Users.FirstOrDefault(a => a.MemberRefNo.Equals(memrefno));
                if (chkuser == null)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            return memrefno;
        }










        public UserDetialViewModel GetUserDtlByMemRefNo(string MemRefNo)
        {
            UserDetialViewModel userDtl = context.Users.Where(u => u.MemberRefNo.Equals(MemRefNo)).Select(u => new UserDetialViewModel { MemberRefNo = u.MemberRefNo, FullName = u.FirstName + " " + u.LastName, uuid = u.MemberRefNo + "_" + u.FirstName + " " + u.LastName }).FirstOrDefault();
            return userDtl;
        }

        public List<UserDetialViewModel> GetUserDtlListByMemRefNo(UserDetialViewModel userDtl)
        {
            List<UserDetialViewModel> userDtlList = context.Users.Where(u => u.MemberRefNo.Contains(u.MemberRefNo)).Select(u => new UserDetialViewModel { MemberRefNo = u.MemberRefNo, FullName = u.FirstName + " " + u.LastName, uuid = u.MemberRefNo + "_" + u.FirstName + " " + u.LastName }).ToList();
            return userDtlList;
        }



        public DateTime GetLastLogoutLoginTime(long userid)
        {
            DateTime maxTime;
            string nullabletime = "01-01-0001 00:00:00";
            DateTime logouttime = context.UserLogs.Where(wh => wh.UserID == userid && wh.UserAction.Equals("Logout Successful")).OrderByDescending(obd => obd.UserLogID).Select(sl => sl.UserLogTimestamp).FirstOrDefault();
            DateTime logintime = context.UserLogs.Where(wh => wh.UserID == userid && wh.UserAction.Equals("Login Successful")).OrderByDescending(obd => obd.UserLogID).Select(sl => sl.UserLogTimestamp).FirstOrDefault();

            if (logouttime == Convert.ToDateTime(nullabletime) && logintime == Convert.ToDateTime(nullabletime))
            {
                //This block execute when first time user login
                string setTime = "2014-01-01 00:00:00";
                maxTime = Convert.ToDateTime(setTime);
            }
            else if (logouttime == Convert.ToDateTime(nullabletime))
            {
                //This block execute when first time user login and without logout it close window
                //After some time when again login then return last login time
                maxTime = logintime;
            }
            else
            {
                //This block execute when user normally follow login logout procedure
                //If logout time is hign then return logout time other wise return login time
                if (logouttime > logintime)
                    maxTime = logouttime;
                else
                    maxTime = logintime;
            }
            return maxTime;
        }



        public void InsertUserVisitUrlTableData(UserVisit uservisitdata)
        {
            try
            {
                if (string.IsNullOrEmpty(uservisitdata.UniqueID))
                {

                    ErrorLogs.ErrorLog(0, "Unknwon", Miscellaneous.CurrentDateTime(), "", "Data : " + uservisitdata.ToString(), "Log : Uniqueid is null", 1);
                }
                else
                {
                    context.UserVisits.Add(uservisitdata);
                    Save();
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                ErrorLogs.ErrorLog(0, "Unknwon", Miscellaneous.CurrentDateTime(), "", "DB Exception" + exceptionMessage, "UserVisit url data insert", 1);
            }
        }

        public List<UniqueSession> GetUniqueSessionData(string uniqueid)
        {
            List<UniqueSession> uniquesessiondatalist = context.UniqueSessions.Where(wh => wh.UniqueID == uniqueid).ToList();
            return uniquesessiondatalist;
        }

        public UniqueSession GetUniqueSessionsingleData(string uniqueid)
        {
            UniqueSession uniquesessiondata = context.UniqueSessions.Where(wh => wh.UniqueID == uniqueid).FirstOrDefault();
            return uniquesessiondata;
        }

        public void InsertUniqueSessionData(UniqueSession uniquesessiondata)
        {
            try
            {
                context.UniqueSessions.Add(uniquesessiondata);
                Save();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                ErrorLogs.ErrorLog(0, "Unknwon", Miscellaneous.CurrentDateTime(), "", "DB Exception" + exceptionMessage, "Unique session data insert", 1);
            }
        }

        public void UpdateUniqueSessionData(UniqueSession olduniquesessiondata, UniqueSession newuniquesessiondata)
        {
            try
            {
                context.Entry(olduniquesessiondata).CurrentValues.SetValues(newuniquesessiondata);
                Save();
            }
            catch (Exception ex)
            {
                ErrorLogs.ErrorLog(0, "Unknwon", Miscellaneous.CurrentDateTime(), "Unique Session data Update", ex.InnerException == null ? ex.Message.ToString() : ex.InnerException.ToString(), "UniqueSession", 1);
            }
        }

        public UserSession GetUserSessionData(long userid)
        {
            UserSession usersessiondata = context.UserSessions.Where(us => us.UserID == userid && us.IsExpired == false).OrderByDescending(od => od.TimeStamp).FirstOrDefault();
            return usersessiondata;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}