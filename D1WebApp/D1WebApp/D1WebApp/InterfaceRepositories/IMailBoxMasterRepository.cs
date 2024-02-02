using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IMailBoxMasterRepository : IDisposable
    {
        List<MailBoxMasterViewModel> GetMailBoxList();
        bool InsertMailBox(MailBoxMasterViewModel MailBoxView);
        bool UpdateMailBox(MailBoxMasterViewModel MailBoxView);
        MailBoxMasterViewModel GetDataByID(long MailBoxID);
        bool DeleteMailBox(long MailBoxID);
    }
}