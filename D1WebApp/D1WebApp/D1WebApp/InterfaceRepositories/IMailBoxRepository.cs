using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Collections.Generic;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IMailBoxRepository
    {
        Int64 GetMailBoxID(String mailboxname);
        //MailBoxViewModel GetMailBox(Int64 MailBoxID);
        MailBoxViewModel GetMailBoxDetailByMailBoxID(Int64 MailBoxId);
        List<MailQueueViewModel> GetMailQueueBySValidTo();
    }
}