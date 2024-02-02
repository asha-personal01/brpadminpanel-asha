using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class UserLogRepository : IUserLogRepository, IDisposable
    {
        private D1WebAppEntities context;
        /// <summary>
        /// Define Context
        /// </summary>
        /// <param name="context"></param>
        public UserLogRepository(D1WebAppEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get User Log all Data
        /// </summary>
        /// <returns></returns>
        public DateTime GetLogOutTimeStamp(long UserID)
        {
            var usrelog = context.UserLogs.Where(cp => cp.UserID == UserID && (cp.UserAction.Contains("Logout") || cp.UserAction.Contains("LogOff"))).OrderByDescending(cp => cp.UserLogTimestamp).FirstOrDefault();
            if (usrelog != null)
            {
                return usrelog.UserLogTimestamp;
            }
            else
            {
                return D1WebApp.Utilities.Miscellaneous.CurrentDateTime();
            }
        }
        public List<UserLogViewModel> GetUserLog()
        {
            UserLogViewModel objuserlogviewmodel = new UserLogViewModel();
            return objuserlogviewmodel.ConvertToViewModelList(context.UserLogs.ToList());
        }
        public User GetUserByUserName(string username)
        {
            try
            {
                return context.Users.FirstOrDefault(u => u.Mobile.ToString().Equals(username) || (u.Email == null ? u.Mobile.ToString().Equals(username) : u.Email.Equals(username)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserViewModel GetbyId(long id)
        {
            User user = context.Users.Find(id);
            UserViewModel viewmodel = new UserViewModel(user);
            viewmodel = (from ur in context.Users
                         join rol in context.UserRoles on user.UserID equals rol.UserID
                         select new UserViewModel {
                             Email=ur.Email,                             
                             GUIDCode=ur.GUIDCode.Value,
                             Password=ur.Password,
                             RoleID=rol.RoleID,
                             UserID=ur.UserID,
                             UserName=ur.FirstName + " " + ur.LastName,
                         }).First();

            return viewmodel;
        }
        /// <summary>
        /// Get User Log By User ID
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>

        public List<UserLogViewModel> GetUserLogByUserID(Int64 UserID)
        {
            UserLogViewModel objuserlogviewmodel = new UserLogViewModel();
            
            return objuserlogviewmodel.ConvertToViewModelList(context.UserLogs.Where(ul => ul.UserID == UserID));
        }

        /// <summary>
        /// Insert User Log 
        /// </summary>
        /// <param name="userlogviewmodel"></param>

        public void InsertUserLog(UserLogViewModel userlogviewmodel)
        {
            context.UserLogs.Add(UserLogViewModel.ConvertToModel(userlogviewmodel));
        }


        /// <summary>
        /// Finaly Save Method Database Update.
        /// </summary>

        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        ///  Dispose Variable and Method
        /// </summary>

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // End  Dispose Variable and Method
    }
}