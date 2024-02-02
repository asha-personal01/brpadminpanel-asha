using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.Utilities.GeneralConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class MailMasterTemplateRepository : IMailMasterTemplateRepository, IDisposable
    {
        private D1WebAppEntities context;

        public MailMasterTemplateRepository()
        { }

        public MailMasterTemplateRepository(D1WebAppEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// GetMailMasterTemplate list
        /// </summary>
        /// <returns>List ofMailMasterTemplate viewmodel</returns>
        public List<MailMasterTemplateViewModel> GetMailMasterTemplateList()
        {
           MailMasterTemplateViewModel MailMasterTemplate = new MailMasterTemplateViewModel();
            List<D1WebApp.Models.MailTemplate> MailMasterTemplatelist = context.MailTemplates.OrderByDescending(mv => mv.MailTemplateID).ToList();
            return MailMasterTemplate.ConvertModelToViewModel(MailMasterTemplatelist.ToList());
        }

        /// <summary>
        /// InsertMailMasterTemplate in database
        /// </summary>
        /// <param name="NewsLetterTemplate">ViewModel ofMailMasterTemplate</param>
        public int InsertMailMasterTemplate(MailMasterTemplateViewModel MailMasterTemplate)
        {
            int status = 0;
            D1WebApp.Models.MailTemplate newMailTemplate = MailMasterTemplate.ConvertViewModelToModel(MailMasterTemplate);
            context.MailTemplates.Add(newMailTemplate);
            context.SaveChanges();

            status = 1;
          
            return status;
        }
    
        /// <summary>
        /// Get Market Vertical Data for API Calling Process
        /// </summary>
        /// <param name="NewsLetterTemplateID">Market Vertical ID</param>
        /// <returns>ViewModel ofMailMasterTemplate for API</returns>
      

        /// <summary>
        /// FindMailMasterTemplate data by ID
        /// </summary>
        /// <param name="NewsLetterTemplateID">NewsLetterTemplate ID</param>
        /// <returns>ViewModel ofMailMasterTemplate from Database</returns>
        public MailMasterTemplateViewModel GetDataByID(long MailMasterTemplateID)
        {
            return new MailMasterTemplateViewModel(context.MailTemplates.Find(MailMasterTemplateID));
        }

        /// <summary>
        /// UpdateMailMasterTemplate data in database
        /// </summary>
        /// <param name="NewsLetterTemplate">ViewModel ofMailMasterTemplate</param>
        public bool UpdateMailMasterTemplate(MailMasterTemplateViewModel MailMasterTemplateviewmodel)
        {
            bool result=false;

            D1WebApp.Models.MailTemplate MailMasterTemplatemodel = context.MailTemplates.Find(MailMasterTemplateviewmodel.MailTemplateID);
            D1WebApp.Models.MailTemplate MailMasterTemplatemodel1 = MailMasterTemplateviewmodel.ConvertViewModelToModel(MailMasterTemplateviewmodel);
            context.Entry(MailMasterTemplatemodel).CurrentValues.SetValues(MailMasterTemplatemodel1);
                context.SaveChanges();
                result = true;
            return result;
        }

        /// <summary>
        /// set IsActive value for Asset, if true then Publish in business directory
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public void IsActiveFromID(long id, bool status)
        {
            D1WebApp.Models.MailTemplate OldModel = context.MailTemplates.Find(id);
            D1WebApp.Models.MailTemplate NewModel = OldModel;
            NewModel.IsActive = status;
            context.Entry(OldModel).CurrentValues.SetValues(NewModel);
            context.SaveChanges();
        }

       
        public bool DeleteMailMasterTemplate(long MailMasterTemplateID)
        {
            bool result = false;

            D1WebApp.Models.MailTemplate MailMasterTemplatemodel = context.MailTemplates.Find(MailMasterTemplateID);
                context.MailTemplates.Remove(MailMasterTemplatemodel);
                context.SaveChanges();
                result = true;
          
            return result;
        }

        /// <summary>
        /// DeactivateMailMasterTemplate
        /// </summary>
        /// <param name="NewsLetterTemplate">Model ofMailMasterTemplate</param>
        public void DeActivate(MailMasterTemplateViewModel MailTempleteView)
        {
            MailTempleteView.IsActive = false;
        }

        /// <summary>
        /// Save Data
        /// </summary>
        //public void Save()
        //{
        //    context.SaveChanges();
        //}

        /// <summary>
        /// Dispose Process
        /// </summary>
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
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