using System;
using System.Collections.Generic;
using D1WebApp.Models;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class ErrorLogViewModel
    {
        public ErrorLogViewModel()
        {

        }
        public ErrorLogViewModel(ErrorLog errorlog)
        {
            this.ErrorLogID = errorlog.ErrorLogID;
            this.ErrorLogTimestamp = errorlog.ErrorLogTimestamp;
            this.UserID = errorlog.UserID;
            this.UserName = errorlog.UserName;
            this.ErrorType = errorlog.ErrorType;
            this.ErrorMessage = errorlog.ErrorMessage;
            this.Module = errorlog.Module;

        }
        public static ErrorLog ConvertToModel(ErrorLogViewModel errorlogvm)
        {
            ErrorLog errorlog = new ErrorLog();
            errorlog.ErrorLogID = errorlogvm.ErrorLogID;
            errorlog.ErrorLogTimestamp = errorlogvm.ErrorLogTimestamp;
            errorlog.UserID = errorlogvm.UserID;
            errorlog.UserName = errorlogvm.UserName;
            errorlog.ErrorType = errorlogvm.ErrorType;
            errorlog.ErrorMessage = errorlogvm.ErrorMessage;
            errorlog.Module = errorlogvm.Module;
            errorlog.IsAdmin = false;
            errorlog.IsError = errorlogvm.IsError;
            return errorlog;
        }

        public List<ErrorLogViewModel> ConvertToViewModelList(List<ErrorLog> errorlogs)
        {
            List<ErrorLogViewModel> errorloglist = new List<ErrorLogViewModel>();
            foreach (ErrorLog errorlog in errorlogs)
            {
                errorloglist.Add(new ErrorLogViewModel(errorlog));
            }
            return errorloglist;
        }
        public int? IsError { get; set; }
        public long ErrorLogID { get; set; }
        public DateTime ErrorLogTimestamp { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public string Module { get; set; }
    }
}