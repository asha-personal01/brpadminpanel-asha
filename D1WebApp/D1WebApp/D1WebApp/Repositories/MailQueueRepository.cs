/// <summary>
/// Mail Queue Repository for All Method
/// Create by : Satish Patel
/// Create Date : 05/05/2014
/// Update by : 
/// Update Date :
/// </summary>

using D1WebApp.Models;
using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class MailQueueRepository : IMailQueueRepository, IDisposable
    {
        private D1WebAppEntities context;
        /// <summary>
        /// Define Context
        /// </summary>
        /// <param name="context"></param>
        public MailQueueRepository(D1WebAppEntities context)
        {
            this.context = context;
        }

        ///// <summary>
        ///// Get Mail Queue Data
        ///// </summary>
        ///// <returns></returns>

        //public IEnumerable<MailQueueViewModel> GetMailQueue()
        //{
        //    MailQueueViewModel objmailqueueviewmodel = new MailQueueViewModel();
        //    return objmailqueueviewmodel.ConvertToViewModelList(context.MailQueues.ToList());
        //}

        /// <summary>
        /// Auther:asha
        /// purpose: this function will get the mail queue detail which has null value in MailSValidTo.
        /// </summary>
        /// <returns></returns>
        public List<MailQueueViewModel> GetMailQueue()
        {
            MailQueueViewModel obj = new MailQueueViewModel();
            List<MailQueue> mailqueues = new List<MailQueue>();
            mailqueues = context.MailQueues.Where(mq => mq.IsProccessed == false).OrderBy(cp => cp.MailQueueID).ToList();
            return obj.ConvertToViewModelList(mailqueues);
        }

        /// <summary>
        /// Get Mail Queue Row Data for Process Mail
        /// </summary>
        /// <returns></returns>
        public List<MailQueueViewModel> GetMailRowQueue()
        {
            MailQueueViewModel objmailqueueviewmodel = new MailQueueViewModel();
            //var mailqueuerow = from k in context.MailQueues 
            //                   where (k.MailSValidTo == null || k.MailSValidTo == System.D1WebApp.Utilities.Miscellaneous.CurrentDateTime()) && k.IsProccessed == false 
            //                   select k;
            List<MailQueue> mailqueues = new List<MailQueue>();
            mailqueues = context.MailQueues.Where(mq => mq.IsProccessed == false && mq.MailSendDate == null).ToList();
            return objmailqueueviewmodel.ConvertToViewModelList(mailqueues);
        }


        /// <summary>
        /// Auther:asha
        /// purpose: this function will get the mail queue detail by MailQueueId
        /// </summary>
        /// <returns></returns>
        public MailQueueViewModel GetMailQueueDetailByID(Int64 MailQueueId)
        {
            return new MailQueueViewModel(context.MailQueues.Where(mq => mq.MailQueueID == MailQueueId).SingleOrDefault());
        }

        /// <summary>
        /// Insert Mail Queue 
        /// </summary>
        /// <param name="mailqueueviewmodel"></param>

        public void InsertMailQueue(MailQueueViewModel mailqueueviewmodel)
        {
            context.MailQueues.Add(MailQueueViewModel.ConvertToModel(mailqueueviewmodel));
        }

        /// <summary>
        /// Delete Mail Queue
        /// </summary>
        /// <param name="MailQueueID"></param>

        public void DeleteMailQueue(Int64 MailQueueID)
        {
            MailQueue mailqueue = context.MailQueues.Find(MailQueueID);
            context.MailQueues.Remove(mailqueue);
        }

        /// <summary>
        ///  Update Mail Queue
        /// </summary>
        /// <param name="mailqueueviewmodel"></param>

        public void UpdateMailQueue(MailQueueViewModel mailqueueviewmodel)
        {
            // context.MailQueues.Attach(MailQueueViewModel.ConvertToModel(mailqueueviewmodel));
            MailQueue mailqueue = context.MailQueues.Single(mq => mq.MailQueueID == mailqueueviewmodel.MailQueueID);
            context.Entry(MailQueueViewModel.ConvertToModel(mailqueueviewmodel, mailqueue)).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Finaly Save Method Database Update.
        /// </summary>

        public void Save()
        {
            context.SaveChanges();
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