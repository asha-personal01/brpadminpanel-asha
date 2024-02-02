using D1WebApp.BusinessLogicLayer.ViewModels;
using System;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IMailTempleteRepository : IDisposable
    {
        MailTemplateViewModel GetMailTemplete(String mailtempletename);
    }
}