//Created by Sunil
//Date : 07/06/2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using D1WebApp.Models;
using D1WebApp.BusinessLogicLayer.ViewModels;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface INotificationRepository : IDisposable
    {
      
        //bool UpdateLastNotifiedTImeStamp(DateTime NewDate);
        int ShowNotification();
        List<NotificationToastViewModel> ShowToast();
        void ClearNotification();
        void InsertNotification(NotificationViewModel notification);
        List<NotificationDisplayViewModel> GetAll(string NoOfRecords);
        void Save();
       
    }
}