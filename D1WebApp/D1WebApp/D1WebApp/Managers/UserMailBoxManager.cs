//Developed by Nayan

using System.Web;
using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.BusinessLogicLayer.ViewModels;

namespace D1WebApp.Utilities
{
    public class UserMailBoxManager
    {
        private UserMailBoxRepository UserMailBoxrepository= new UserMailBoxRepository();        

        public UserMailBoxManager()
        {
        }
        public bool InsertUserMailBox(UserMailBoxViewModel NewUserMailBox)
        {
            return UserMailBoxrepository.InsertUserMailBox(NewUserMailBox);
        }
        public dynamic GetDataByUserID(string UserID)
        {
            return UserMailBoxrepository.GetDataByUserID(UserID);
        }
        public bool UpdateUserMailBox(UserMailBoxViewModel NewUserMailBox)
        {
            return UserMailBoxrepository.UpdateUserMailBox(NewUserMailBox);
        }
        public bool DeleteUserMailBox(string UserMailBoxID,string Company)
        {
            return UserMailBoxrepository.DeleteUserMailBox(UserMailBoxID, Company);
        }

    }
}