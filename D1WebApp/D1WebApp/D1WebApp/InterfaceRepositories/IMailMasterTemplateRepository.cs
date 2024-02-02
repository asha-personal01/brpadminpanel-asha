using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IMailMasterTemplateRepository : IDisposable
    {
        List<MailMasterTemplateViewModel> GetMailMasterTemplateList();
        int InsertMailMasterTemplate(MailMasterTemplateViewModel MailMasterTemplateViewModel);
        MailMasterTemplateViewModel GetDataByID(long MailMasterTemplateID);
        bool UpdateMailMasterTemplate(MailMasterTemplateViewModel MailMasterTemplate);
        bool DeleteMailMasterTemplate(long MailMasterTemplateID);
    }
}