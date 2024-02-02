using D1WebApp.Models;
using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Linq;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class MailTempleteRepository : IMailTempleteRepository, IDisposable
    {
         private D1WebAppEntities context;
        /// <summary>
        /// Define Context
        /// </summary>
        /// <param name="context"></param>
         public MailTempleteRepository(D1WebAppEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// For return MailTemplete Data
        /// </summary>
        /// <param name="mailboxname"></param>
        /// <returns></returns>
         public MailTemplateViewModel GetMailTemplete(String mailtempletname)
        {
            var mailtempletes = context.MailTemplates.Where(mt => mt.MailTemplateName.Equals(mailtempletname) && mt.IsActive == true).ToList();
            if (mailtempletes.Count() > 0)
            {
                D1WebApp.Models.MailTemplate mailtemplete = mailtempletes.First();
                return new MailTemplateViewModel(mailtemplete);
            }
            else
                return new MailTemplateViewModel();
        }
         /// <summary>
         ///  Dispose Variable and Method
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
        // End  Dispose Variable and Method

    }
}