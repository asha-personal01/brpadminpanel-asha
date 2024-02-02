
/// <summary>
/// Eroor Log Repository for All Method
/// Create by : Satish Patel
/// Create Date : 05/05/2014
/// Update by : 
/// Update Date :
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using D1WebApp.Models;
using D1WebApp.BusinessLogicLayer.ViewModels;
using System.Data.Entity;
using D1WebApp.Utilities;
using System.Data.Entity.Validation;
namespace D1WebApp.DataAccessLayer.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository, IDisposable
    {
        private D1WebAppEntities context;
        /// <summary>
        /// Define Context
        /// </summary>
        /// <param name="context"></param>
        public ErrorLogRepository(D1WebAppEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get Error Log Data
        /// </summary>
        /// <returns></returns>

        public List<ErrorLogViewModel> GetErrorLog()
        {
            ErrorLogViewModel objerrorlogviewmodel = new ErrorLogViewModel();
            return objerrorlogviewmodel.ConvertToViewModelList(context.ErrorLogs.ToList());
        }

        public List<ErrorLogViewModel> GetErrorLogLast50()
        {
            ErrorLogViewModel objerrorlogviewmodel = new ErrorLogViewModel();
            return objerrorlogviewmodel.ConvertToViewModelList(context.ErrorLogs.OrderByDescending(e => e.ErrorLogID).Take(50).ToList());
        }


        public List<ErrorLogViewModel> GetErrorLogCurrentDate()
        {
            ErrorLogViewModel objerrorlogviewmodel = new ErrorLogViewModel();
            DateTime currentdate = Miscellaneous.CurrentDateTime();
            return objerrorlogviewmodel.ConvertToViewModelList(context.ErrorLogs.Where(e => DbFunctions.DiffDays(e.ErrorLogTimestamp, currentdate) == 0).OrderByDescending(e => e.ErrorLogID).ToList());
        }
        /// <summary>
        /// Get Error Log Row By ID
        /// </summary>
        /// <param name="MailQueueID"></param>
        /// <returns></returns>

        public ErrorLogViewModel GetErrorLogByID(Int64 ErrorLogID)
        {
            return new ErrorLogViewModel(context.ErrorLogs.Find(ErrorLogID));
        }

        /// <summary>
        /// Insert Error Log 
        /// </summary>
        /// <param name="mailqueueviewmodel"></param>

        public void InsertErrorLog(ErrorLogViewModel errorlogviewmodel)
        {
            errorlogviewmodel.UserName = errorlogviewmodel.UserName == null ? "Unknwon" : errorlogviewmodel.UserName;
            context.ErrorLogs.Add(ErrorLogViewModel.ConvertToModel(errorlogviewmodel));
        }


        /// <summary>
        /// Finaly Save Method Database Update.
        /// </summary>

        public void Save()
        {
            try
            {
                context.SaveChanges();
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
                ErrorLogs.ErrorLog(0, "Guest", Miscellaneous.CurrentDateTime(), "", "DB Exception" + exceptionMessage, "Error : Error log msg", 1);
            }

        }

        public List<ErrorLogViewModel> GetErrorLogByEmail(string UserEmail)
        {
            ErrorLogViewModel objErrorVm = new ErrorLogViewModel();
            return (objErrorVm.ConvertToViewModelList(context.ErrorLogs.Where(e => e.UserName.Equals(UserEmail)).ToList()));
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