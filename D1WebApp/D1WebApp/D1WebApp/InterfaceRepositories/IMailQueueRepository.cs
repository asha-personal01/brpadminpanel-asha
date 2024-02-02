using System;
using System.Collections.Generic;
using D1WebApp.BusinessLogicLayer.ViewModels;
namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IMailQueueRepository : IDisposable
    {
        List<MailQueueViewModel> GetMailQueue();
        List<MailQueueViewModel> GetMailRowQueue();
        MailQueueViewModel GetMailQueueDetailByID(Int64 MailQueueId);
        void InsertMailQueue(MailQueueViewModel mailqueue);
        void DeleteMailQueue(Int64 MailQueueID);
        void UpdateMailQueue(MailQueueViewModel mailqueue);
        void Save();
        
    }
}