//Developed by Nayan

using D1WebApp.BusinessLogicLayer.ViewModels;

using D1WebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Web;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IUserMailBoxRepository : IDisposable
    {
        
        bool InsertUserMailBox(UserMailBoxViewModel NewUserMailBox);
        dynamic GetDataByUserID(string memRefNo);
        bool UpdateUserMailBox(UserMailBoxViewModel NewUserMailBox);
        bool DeleteUserMailBox(string memRefNo,string company);

    }
}