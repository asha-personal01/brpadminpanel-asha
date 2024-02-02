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
using D1WebApp.ClientModel;
using System.Reflection;
using System.Web.Helpers;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class SubUsersRepository : ISubUsersRepository, IDisposable
    {
        private D1WebAppEntities context;
        public SubUsersRepository(D1WebAppEntities context)
        {
            this.context = context;
        }


        public dynamic GetSubusersList(long userParent,string memRefNo, string filterQuery, int pageno)
        {
            List<SubusersViewModel> subUsersList = new List<SubusersViewModel>();
            string type = "Subuser";
            try
            {
                if (pageno == 0)
                {
                    pageno = 1;
                }
                int counts = (from ur in context.Users
                              join urr in context.UserRoles on ur.UserID equals urr.UserID
                              join ro in context.Roles on urr.RoleID equals ro.RoleID
                              where ro.RoleName.Equals(type.ToString())
                              select ur).Distinct().Count();
                subUsersList = (from ur in context.Users
                                join urr in context.UserRoles on ur.UserID equals urr.UserID
                                join ro in context.Roles on urr.RoleID equals ro.RoleID
                                where ro.RoleName.Equals(type.ToString()) && urr.UserParent == userParent
                                select new SubusersViewModel
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
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetSubusersList", DateTime.Now, "GetSubusersList", ed.ToString(), "GetSubusersList", 2);
            }
            return subUsersList;
        }
        
        public dynamic AddUpdateSubUser(UserInsertViewModel createNewSubUser, D1WebAppEntities Context1)
        {
            try
            {
                if (createNewSubUser.UserID > 0)
                {
                    User newuser = createNewSubUser.GetNewUser(createNewSubUser);
                    User olduser = Context1.Users.Find(createNewSubUser.UserID);
                    newuser.AuthenticationToken = olduser.AuthenticationToken;
                    newuser.FailedAttemptCount = olduser.FailedAttemptCount;
                    if (!string.IsNullOrEmpty(createNewSubUser.Password))
                    {
                        newuser.Password = createNewSubUser.Password;
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
                    User newuser = createNewSubUser.GetNewUser(createNewSubUser);
                    Context1.Users.Add(newuser);
                    Context1.SaveChanges();
                    return newuser.UserID;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateWebConfigsList", DateTime.Now, "UpdateWebConfigsList", ed.ToString(), "UpdateWebConfigsList", 2);
                return ed.InnerException.ToString();
            }

        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}