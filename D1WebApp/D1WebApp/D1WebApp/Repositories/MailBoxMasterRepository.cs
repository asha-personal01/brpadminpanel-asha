using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class MailBoxMasterRepository : IMailBoxMasterRepository, IDisposable
    {
        private D1WebAppEntities context;

        public MailBoxMasterRepository()
        { }

        public MailBoxMasterRepository(D1WebAppEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get MenuGroup list
        /// </summary>
        /// <returns>List of MenuGroup viewmodel</returns>
        public List<MailBoxMasterViewModel> GetMailBoxList()
        {
            MailBoxMasterViewModel MailBoxView = new MailBoxMasterViewModel();
            List<MailBox> MailBoxList = context.MailBoxes.OrderByDescending(mg => mg.MailBoxID).ToList();
            return MailBoxMasterViewModel.ConvertToviewList(MailBoxList);
        }

        /// <summary>
        /// Insert MenuGroup in database
        /// </summary>
        /// <param name="MenuGroup">ViewModel of MenuGroup</param>
        public bool InsertMailBox(MailBoxMasterViewModel MailBoxView)
        {
            try
            {
                context.MailBoxes.Add(MailBoxMasterViewModel.ConvertToModel(MailBoxView));
                Save();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogs.ErrorLog(0, "Anonymous", D1WebApp.Utilities.Miscellaneous.CurrentDateTime(), "Error in Mail Box insert", ex.Message, " Mail Box/Create", 1);
                return false;
            }
        }

        /// <summary>
        /// Update MenuGroup data in database
        /// </summary>
        /// <param name="MenuGroup">ViewModel of MenuGroup</param>
        public bool UpdateMailBox(MailBoxMasterViewModel MailBoxView)
        {
            try
            {
                MailBox MailBoxModel = context.MailBoxes.Find(MailBoxView.MailBoxID);
                MailBox MailBoxModel1 = MailBoxMasterViewModel.ConvertToModel(MailBoxView);
                context.Entry(MailBoxModel).CurrentValues.SetValues(MailBoxModel1);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogs.ErrorLog(0, "Anonymous", D1WebApp.Utilities.Miscellaneous.CurrentDateTime(), "Error in  Mail Box update", ex.Message, " Mail Box/Edit", 1);
                return false;
            }
        }

        /// <summary>
        /// Find MenuGroup data by ID
        /// </summary>
        /// <param name="MenuGroupID">MenuGroup ID</param>
        /// <returns>ViewModel of MenuGroup from Database</returns>
        public MailBoxMasterViewModel GetDataByID(long MailBoxID)
        {
        
            try{
        return new MailBoxMasterViewModel(context.MailBoxes.Find(MailBoxID));
    }
        catch (Exception ex)
            {
                ErrorLogs.ErrorLog(0, "Anonymous", D1WebApp.Utilities.Miscellaneous.CurrentDateTime(), "Error in Mail Box insert", ex.Message, " Mail Box/Create",1);
                return new MailBoxMasterViewModel();
            }
}

        /// <summary>
        /// Delete MenuGroup from database
        /// </summary>
        /// <param name="MenuGroupID">MenuGroup ID</param>
        public bool DeleteMailBox(long MailBoxID)
        {
            try
            {
                MailBox MailBoxModel = context.MailBoxes.Find(MailBoxID);
                context.MailBoxes.Remove(MailBoxModel);
                Save();
                return true;
            }
            catch(Exception ex)
            {
                ErrorLogs.ErrorLog(0, "Anonymous", D1WebApp.Utilities.Miscellaneous.CurrentDateTime(), "Error in  Mail Box delete", ex.Message, "Mail Box/Delete",1);
                return false;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Dispose Process
        /// </summary>
         private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
