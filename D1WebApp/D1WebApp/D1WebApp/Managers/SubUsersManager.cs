//Developed by Nayan

using System.Web;
using System;
using D1WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.ViewModels;

namespace D1WebApp.Utilities
{
    public class SubUsersManager
    {
        private SubUsersRepository subUsersRepository = new SubUsersRepository(new D1WebAppEntities());

        public SubUsersManager()
        {
        }

        public dynamic GetSubusersList(long userParent,string memRefNo, string filterQuery, int pageno)
        {
            return subUsersRepository.GetSubusersList(userParent,memRefNo, filterQuery,pageno);
        }
        public long AddUpdateSubUser(UserInsertViewModel createnbewuser, D1WebAppEntities Context1)
        {
            return subUsersRepository.AddUpdateSubUser(createnbewuser, Context1);
        }
    }
}