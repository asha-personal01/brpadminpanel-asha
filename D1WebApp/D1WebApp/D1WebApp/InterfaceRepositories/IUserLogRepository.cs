/// <summary>
/// User Log Repository for All Method
/// Create by : Satish Patel
/// Create Date : 06/05/2014
/// Update by : 
/// Update Date :
/// </summary>
using System;
using System.Collections.Generic;
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IUserLogRepository : IDisposable
    {
        UserViewModel GetbyId(long id);
        User GetUserByUserName(string username);
        DateTime GetLogOutTimeStamp(long UserID);
        List<UserLogViewModel> GetUserLog();
        List<UserLogViewModel> GetUserLogByUserID(Int64 UserID);
        void InsertUserLog(UserLogViewModel errorlog);
        void Save();
    }
}