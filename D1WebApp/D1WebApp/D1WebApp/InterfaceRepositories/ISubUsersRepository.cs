//Developed by Nayan

using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.Utilities;
using D1WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface ISubUsersRepository : IDisposable
    {
        dynamic GetSubusersList(long userParent, string memRefNo, string filterQuery, int pageno);
        dynamic AddUpdateSubUser(UserInsertViewModel createNewSubUser, D1WebAppEntities Context1);
    }
}