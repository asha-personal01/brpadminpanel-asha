using D1WebApp.Models;
using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class MailBoxRepository : IMailBoxRepository, IDisposable
    {
        private D1WebAppEntities context;
        /// <summary>
        /// Define Context
        /// </summary>
        /// <param name="context"></param>
        public MailBoxRepository(D1WebAppEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// For return MailBoxID pass MailBoxName
        /// </summary>
        /// <param name="mailboxname"></param>
        /// <returns></returns>
        public Int64 GetMailBoxID(String mailboxname)
        {
            //var mailbox = (from k in context.MailBoxes
            //              where k.MailBoxName.Equals(mailboxname) && k.IsActive== true
            //              select k.MailBoxID).SingleOrDefault();
            var mailbox = context.MailBoxes.Where(k => k.MailBoxName.Equals(mailboxname) && k.IsActive == true).Select(k => k.MailBoxID).SingleOrDefault();
            return mailbox;
        }

        /// <summary>
        /// Auther:asha
        /// purpose: this function will get the mail queue detail which has null value in MailSValidTo.
        /// </summary>
        /// <returns></returns>
        public List<MailQueueViewModel> GetMailQueueBySValidTo()
        {
            DateTime Date = D1WebApp.Utilities.Miscellaneous.CurrentDateTime();
            Date = Date + TimeSpan.FromMinutes(15);
            MailQueueViewModel obj = new MailQueueViewModel();
            List<MailQueue> mailqueues = new List<MailQueue>();
            mailqueues = context.MailQueues.Where(mq => mq.IsProccessed == false && mq.MailSendDate <= Date).ToList();
            return obj.ConvertToViewModelList(mailqueues);
        }

        /// <summary>
        /// For return MailBox Data
        /// </summary>
        /// <param name="mailboxname"></param>
        /// <returns></returns>
        public MailBoxViewModel GetMailBoxDetailByMailBoxID(Int64 MailBoxID)
        {
            return new MailBoxViewModel(context.MailBoxes.Find(MailBoxID));
        }
        public MailBoxViewModel GetMailBoxDetailsByUserID(Int64 UserID)
        {
            var getmail = context.UserMailBoxes.Where(cp => cp.UserID == UserID).FirstOrDefault();
            if (getmail != null && getmail.UserID > 0)
            {
                return new MailBoxViewModel(getmail);
            }
            else
            {
                return new MailBoxViewModel();
            }
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