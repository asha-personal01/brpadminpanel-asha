//Created by Sunil
//Date : 07/06/2014

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.IO.MemoryMappedFiles;



using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.Utilities.GeneralConfiguration;
using D1WebApp.Utilities;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Globalization;


namespace D1WebApp.DataAccessLayer.Repositories
{
    public class NotificationRepository : INotificationRepository, IDisposable
    {
        private D1WebAppEntities context;

        UserRepository userrepository = new UserRepository(new D1WebAppEntities());

        public NotificationRepository(D1WebAppEntities context)
        {
            this.context = context;
        }
        


     

        public int ShowNotification()
        {
            DateTime GetTimeStamp = D1WebApp.Utilities.Security.GetLastNotified();
            int count = 0;            
            return count;
        }

        public List<NotificationToastViewModel> ShowToast()
        {
            DateTime GetTimeStamp = D1WebApp.Utilities.Security.GetLastNotified();
            List<NotificationToastViewModel> notificationList = new List<NotificationToastViewModel>();
            List<NotificationToastViewModel> notificationList1 = new List<NotificationToastViewModel>();
            if (GetTimeStamp != null)
            {
               //Query goes here
                //notificationList = (from buyinquiry in context.BuyEnquiries
                //                    where buyinquiry.EnquiryDate > GetTimeStamp
                //                    select new NotificationToastViewModel
                //                    {
                //                        NotificationStyle = "Success",
                //                        NotificationType = "Inquiry Created",
                //                        Remarks = "New Inquiry Created",
                //                    }).Take(1).ToList();

                //notificationList1 = (from buyinquiry in context.BuyEnqPartyResponses
                //                     where buyinquiry.ResponseDate > GetTimeStamp
                //                     select new NotificationToastViewModel
                //                     {
                //                         NotificationStyle = "Info",
                //                         NotificationType = "New Response Received",
                //                         Remarks = "New Response Received",
                //                     }).Take(1).ToList();
            }
            return notificationList.Union(notificationList1).ToList();
        }

        public void ClearNotification()
        {
            D1WebApp.Utilities.Security.UpdateLastNotifiedTimeStamp();
        }

        public void InsertNotification(NotificationViewModel notification)
        {
            
            context.Notifications.Add(NotificationViewModel.ConvertViewModelToModel(notification));
            context.SaveChanges();
            
        }
          

        public List<NotificationDisplayViewModel> GetAll(string NoOfRecords)
        {
            DateTime LastNotifiedTime = D1WebApp.Utilities.Security.GetLastNotified();
            List<NotificationDisplayViewModel> notificationviewmodellist = new List<NotificationDisplayViewModel>();
            List<NotificationDisplayViewModel> notificationviewmodellist1 = new List<NotificationDisplayViewModel>();
            if (string.IsNullOrEmpty(NoOfRecords))
            {
            //query
            }
                        return notificationviewmodellist.Union(notificationviewmodellist1).ToList();
        }







        public void Save()
        {
            context.SaveChanges();
        }

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