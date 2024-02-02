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
using D1WebApp.ClientModel;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class UserMailBoxRepository : IUserMailBoxRepository, IDisposable
    {
        
        public UserMailBoxRepository()
        {
            
        }

        public bool DeleteUserMailBox(string memRefNo,string company)
        {
            var Context2 = new D1WebAppEntities();
            var getuser = Context2.Users.Where(cp => cp.MemberRefNo.Equals(memRefNo)).First();
            bool flag = false;
            
            UserMailBox getoldUserMailBox = Context2.UserMailBoxes.Where(cp => cp.UserID == getuser.UserID && cp.Company.Equals(company)).FirstOrDefault();
            if (getoldUserMailBox != null && getoldUserMailBox.MailBoxID > 0)
            {
                Context2.UserMailBoxes.Remove(getoldUserMailBox);
                Context2.SaveChanges();
                Context2.Dispose();
            }
            try
            {
                var cont = new ClientEntities(getuser.MemberRefNo);
                var getrec = cont.MailConfigs.FirstOrDefault();
                if (getrec != null)
                {
                    cont.MailConfigs.Remove(getrec);
                    cont.SaveChanges();
                }

            }catch(Exception ed) { }
            flag = true;
            return flag;
        }

        public bool UpdateUserMailBox(UserMailBoxViewModel NewUserMailBox)
        {
            bool flag = false;
            var Context2 = new D1WebAppEntities();
            var getuser = Context2.Users.Where(cp => cp.MemberRefNo.Equals(NewUserMailBox.memRefNo)).First();
            NewUserMailBox.UserID = getuser.UserID;
            UserMailBox getooldUserMailBox = Context2.UserMailBoxes.Where(cp => cp.UserID== getuser.UserID && cp.Company.Equals(NewUserMailBox.Company)).FirstOrDefault();
            UserMailBox newUserMailBox = UserMailBoxViewModel.ConvertToModel(NewUserMailBox);
            Context2.Entry(getooldUserMailBox).CurrentValues.SetValues(newUserMailBox);
            Context2.SaveChanges();
            Context2.Dispose();
            try
            {
                var context = new ClientEntities(getuser.MemberRefNo);
                var getrec = context.MailConfigs.FirstOrDefault();
                if (getrec != null)
                {
                    ClientModel.MailConfig cd1 = new MailConfig();
                    cd1.IsActive = NewUserMailBox.IsActive;
                    cd1.IsSSL = NewUserMailBox.IsSSL;
                    cd1.MailBoxID = getrec.MailBoxID;
                    cd1.MailBoxName = NewUserMailBox.MailBoxName;
                    cd1.MailFrom = NewUserMailBox.MailFrom;
                    cd1.Password = NewUserMailBox.Password;
                    cd1.ReplyTo= NewUserMailBox.ReplyTo;
                    cd1.SMTPPort= NewUserMailBox.SMTPPort;
                    cd1.SMTPServer= NewUserMailBox.SMTPServer;
                    cd1.UserName = NewUserMailBox.UserName;
                    cd1.DisplayName = NewUserMailBox.DisplayName;
                    cd1.Company = NewUserMailBox.Company;
                    context.Entry(getrec).CurrentValues.SetValues(cd1);
                    context.SaveChanges();
                }
                else
                {
                    ClientModel.MailConfig cd1 = new MailConfig();
                    cd1.IsActive = NewUserMailBox.IsActive;
                    cd1.IsSSL = NewUserMailBox.IsSSL;                    
                    cd1.MailBoxName = NewUserMailBox.MailBoxName;
                    cd1.MailFrom = NewUserMailBox.MailFrom;
                    cd1.Password = NewUserMailBox.Password;
                    cd1.ReplyTo = NewUserMailBox.ReplyTo;
                    cd1.SMTPPort = NewUserMailBox.SMTPPort;
                    cd1.SMTPServer = NewUserMailBox.SMTPServer;
                    cd1.UserName = NewUserMailBox.UserName;
                    cd1.DisplayName = NewUserMailBox.DisplayName;
                    cd1.Company = NewUserMailBox.Company;
                    context.MailConfigs.Add(cd1);
                    context.SaveChanges();
                }

                }catch(Exception ed)
            {

            }
            flag = true;
            return flag;
        }
        public dynamic GetDataByUserID(string memRefNo)
        {
            var Context2 = new D1WebAppEntities();
            var getuser = Context2.Users.Where(cp => cp.MemberRefNo.Equals(memRefNo)).First();
            var getUserMailBox = Context2.UserMailBoxes.Where(cp => cp.UserID == getuser.UserID).ToList();
            if (getUserMailBox != null && getUserMailBox.Count > 0)
            {
                return UserMailBoxViewModel.ConvertToViewModelListNew(getUserMailBox);
            }
            else
            {
                return new UserMailBoxViewModel();
            }
        }
        public bool InsertUserMailBox(UserMailBoxViewModel NewUserMailBox)
        {
            bool flag = false;
            var Context2 = new D1WebAppEntities();
            var getuser = Context2.Users.Where(cp => cp.MemberRefNo.Equals(NewUserMailBox.memRefNo)).First();
            NewUserMailBox.UserID = getuser.UserID;
            UserMailBox oldUserMailBox = Context2.UserMailBoxes.Where(cp => cp.UserID == NewUserMailBox.UserID && cp.Company.Equals(NewUserMailBox.Company)).FirstOrDefault();
            if (oldUserMailBox != null)
            {
                NewUserMailBox.MailBoxID = oldUserMailBox.MailBoxID;
            }
            if (NewUserMailBox.MailBoxID != null && NewUserMailBox.MailBoxID > 0)
            {
                
                
                UserMailBox nUserMailBox = UserMailBoxViewModel.ConvertToModel(NewUserMailBox);
                Context2.Entry(oldUserMailBox).CurrentValues.SetValues(nUserMailBox);
                Context2.SaveChanges();
                Context2.Dispose();
                flag = true;
            }
            else
            {
                UserMailBox nUserMailBox = UserMailBoxViewModel.ConvertToModel(NewUserMailBox);
                Context2.UserMailBoxes.Add(nUserMailBox);
                Context2.SaveChanges();
                Context2.Dispose();
                flag = true;
            }
            try
            {
                var context = new ClientEntities(getuser.MemberRefNo);
                var getrec = context.MailConfigs.FirstOrDefault();
                if (getrec != null)
                {
                    ClientModel.MailConfig cd1 = new MailConfig();
                    cd1.IsActive = NewUserMailBox.IsActive;
                    cd1.IsSSL = NewUserMailBox.IsSSL;
                    cd1.MailBoxID = getrec.MailBoxID;
                    cd1.MailBoxName = NewUserMailBox.MailBoxName;
                    cd1.MailFrom = NewUserMailBox.MailFrom;
                    cd1.Password = NewUserMailBox.Password;
                    cd1.ReplyTo = NewUserMailBox.ReplyTo;
                    cd1.SMTPPort = NewUserMailBox.SMTPPort;
                    cd1.SMTPServer = NewUserMailBox.SMTPServer;
                    cd1.UserName = NewUserMailBox.UserName;
                    cd1.DisplayName = NewUserMailBox.DisplayName;
                    cd1.Company = NewUserMailBox.Company;
                    context.Entry(getrec).CurrentValues.SetValues(cd1);
                    context.SaveChanges();
                }
                else
                {
                    ClientModel.MailConfig cd1 = new MailConfig();
                    cd1.IsActive = NewUserMailBox.IsActive;
                    cd1.IsSSL = NewUserMailBox.IsSSL;
                    cd1.MailBoxName = NewUserMailBox.MailBoxName;
                    cd1.MailFrom = NewUserMailBox.MailFrom;
                    cd1.Password = NewUserMailBox.Password;
                    cd1.ReplyTo = NewUserMailBox.ReplyTo;
                    cd1.SMTPPort = NewUserMailBox.SMTPPort;
                    cd1.SMTPServer = NewUserMailBox.SMTPServer;
                    cd1.UserName = NewUserMailBox.UserName;
                    cd1.DisplayName = NewUserMailBox.DisplayName;
                    cd1.Company = NewUserMailBox.Company;
                    context.MailConfigs.Add(cd1);
                    context.SaveChanges();
                }

            }
            catch (Exception ed)
            {

            }
            return flag;
        }
        public void Dispose()
        {         
            GC.SuppressFinalize(this);
        }
    }
}