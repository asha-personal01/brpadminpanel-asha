//Developed by Nayan

using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Linq;
using System.Web;
using D1WebApp.Models;
using D1WebApp.Utilities;

using D1WebApp.Utilities.GeneralConfiguration;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Validation;


namespace D1WebApp.DataAccessLayer.Repositories
{
    public class EndUserRepository : IEndUserRepository, IDisposable
    {
        
        public EndUserRepository()
        {
            
        }

        //public bool DeleteEndUser(long EndUserID, string UserMemRefNo)
        //{
        //    bool flag = false;
        //    try { 
        //    var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //    EndUser getoldEndUser = Context2.EndUsers.Find(EndUserID);
        //    Context2.EndUsers.Remove(getoldEndUser);
        //    Context2.SaveChanges();
        //    Context2.Dispose();
        //    flag = true;
        //    }
        //    catch (Exception ed) { }
        //    return flag;
        //}

        //public bool UpdateEndUser(EndUserViewModel NewEndUser)
        //{
        //    bool flag = false;
        //    try { 
        //    var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(NewEndUser.UserMemRefNo.ToString()));
        //    EndUser getooldEndUser = Context2.EndUsers.Find(NewEndUser.UsersID);
        //    EndUser newEndUser = EndUserViewModel.ConvertViewModelToModel(NewEndUser);
        //    Context2.Entry(getooldEndUser).CurrentValues.SetValues(newEndUser);
        //    Context2.SaveChanges();
        //    Context2.Dispose();
        //    flag = true;
        //    }
        //    catch (Exception ed) { }
        //    return flag;
        //}

        //public EndUserViewModel GetDataByID(long EndUserID, string UserMemRefNo)
        //{
        //    EndUserViewModel enduse = new EndUserViewModel();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //        var getEndUser = Context2.EndUsers.Find(EndUserID);
        //        enduse = new EndUserViewModel(getEndUser);
        //    }
        //    catch (Exception ed) { }
        //    return enduse;
        //}

        //public bool InsertEndUser(EndUserViewModel NewEndUser)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(NewEndUser.UserMemRefNo.ToString()));
        //    if (NewEndUser.UsersID != null && NewEndUser.UsersID > 0)
        //    {
        //        EndUser olduser = Context2.EndUsers.Find(NewEndUser.UsersID);
        //        EndUser newusers = EndUserViewModel.ConvertViewModelToModel(NewEndUser);
        //        Context2.Entry(olduser).CurrentValues.SetValues(newusers);
        //        Context2.SaveChanges();
        //        Context2.Dispose();
        //        flag = true;
        //    }
        //    else
        //    {
        //        EndUser newusers = EndUserViewModel.ConvertViewModelToModel(NewEndUser);
        //        Context2.EndUsers.Add(newusers);
        //        Context2.SaveChanges();
        //        Context2.Dispose();
        //        flag = true;
        //    }
        //    }
        //    catch (Exception ed) { }
        //    return flag;
        //}
        //public List<EndUserViewModel> GetAll(string UserMemRefNo)
        //{
        //    List<EndUserViewModel> GetAll = new List<EndUserViewModel>();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //    GetAll = (from br in Context2.EndUsers
        //              select new EndUserViewModel
        //              {
        //                 Active=br.Active,
        //                 Email=br.Email,
        //                 FirstName=br.FirstName,
        //                 Lastlogindate=br.Lastlogindate,
        //                 LastName=br.LastName,
        //                 Mobile=br.Mobile,
        //                 password=br.password,
        //                 UsersID=br.UsersID,
        //                 Usertype=br.Usertype
        //              }).OrderBy(cp => cp.UsersID).ToList();
        //    Context2.Dispose();
        //    }
        //    catch (Exception ed) { }
        //    return GetAll;
        //}

        //public List<EndUserViewModel> GetEndUserListByPage(int page, int pagenumber, string UserMemRefNo)
        //{
        //    List<EndUserViewModel> GetAll = new List<EndUserViewModel>();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //    GetAll = (from br in Context2.EndUsers.OrderByDescending(c => c.UsersID).Skip((page - 1) * pagenumber).Take(pagenumber).ToList()
        //              select new EndUserViewModel
        //              {
        //                  Active = br.Active,
        //                  Email = br.Email,
        //                  FirstName = br.FirstName,
        //                  Lastlogindate = br.Lastlogindate,
        //                  LastName = br.LastName,
        //                  Mobile = br.Mobile,
        //                  password = br.password,
        //                  UsersID = br.UsersID,
        //                  Usertype = br.Usertype
        //              }).ToList();
        //    Context2.Dispose();
        //    }
        //    catch (Exception ed) { }
        //    return GetAll;
        //}

     

        public void Dispose()
        {
         
            GC.SuppressFinalize(this);
        }
    }
}