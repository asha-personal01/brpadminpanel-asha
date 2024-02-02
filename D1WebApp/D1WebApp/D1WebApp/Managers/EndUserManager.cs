//Developed by Nayan

using System.Web;
using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.BusinessLogicLayer.ViewModels;

namespace D1WebApp.Utilities
{
    public class EndUserManager
    {
        private EndUserRepository EndUserrepository= new EndUserRepository();        

        public EndUserManager()
        {
        }
        //public List<EndUserViewModel> GetAll(string UserMemRefNo)
        //{
        //    return EndUserrepository.GetAll(UserMemRefNo);
        //}
        //public List<EndUserViewModel> GetEndUserListByPage(int page, int pagenumber, string UserMemRefNo)
        //{
        //    return EndUserrepository.GetEndUserListByPage(page, pagenumber, UserMemRefNo);
        //}
        //public bool InsertEndUser(EndUserViewModel NewEndUser)
        //{
        //    return EndUserrepository.InsertEndUser(NewEndUser);
        //}
        //public EndUserViewModel GetDataByID(long EndUserID, string UserMemRefNo)
        //{
        //    return EndUserrepository.GetDataByID(EndUserID, UserMemRefNo);
        //}
        //public bool UpdateEndUser(EndUserViewModel NewEndUser)
        //{
        //    return EndUserrepository.UpdateEndUser(NewEndUser);
        //}
        //public bool DeleteEndUser(long EndUserID, string UserMemRefNo)
        //{
        //    return EndUserrepository.DeleteEndUser(EndUserID, UserMemRefNo);
        //}

    }
}