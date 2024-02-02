/// <summary>
/// Eroor Log Repository for All Method
/// Create by : Satish Patel
/// Create Date : 05/05/2014
/// Update by : 
/// Update Date :
/// </summary>
using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Collections.Generic;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IErrorLogRepository
    {
        List<ErrorLogViewModel> GetErrorLog();
        List<ErrorLogViewModel> GetErrorLogLast50();
        List<ErrorLogViewModel> GetErrorLogCurrentDate();
        ErrorLogViewModel GetErrorLogByID(Int64 ErrorLogID);
        List<ErrorLogViewModel> GetErrorLogByEmail(string UserEmail);
        void InsertErrorLog(ErrorLogViewModel errorlog);
        void Save();
    }
}