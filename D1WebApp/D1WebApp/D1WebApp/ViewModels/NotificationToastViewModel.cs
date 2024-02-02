// Created by sunil
// Date : 7/6/2014

using D1WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{


    public class SellerProfileDetailsViewModel
    {
        public string MobileNo { get; set; }
        public string ProfileName { get; set; }
        public string ValidToDate { get; set; }
    }


    public class MembershipExpiryNotification
    {
        public string MobileNo { get; set; }
        public string MembershipName { get; set; }
        public string ValidTo { get; set; }
    }

    public class NotificationToastViewModel
    {
        public string NotificationType { get; set; }
        public string Remarks { get; set; }
        public string NotificationStyle { get; set; }

        public NotificationToastViewModel()
        {

        }

        public NotificationToastViewModel(Notification notification)
        {
            this.NotificationType = notification.NotificationType;
            this.Remarks = notification.Remarks;
            this.NotificationStyle = notification.NotificationStyle;
        }

        public static List<NotificationToastViewModel> ConvertModelToViewModel(List<Notification> notifications)
        {
            List<NotificationToastViewModel> list = new List<NotificationToastViewModel>();

            foreach (Notification notification in notifications)
            {
                list.Add(new NotificationToastViewModel(notification));
            }

            return list;
        }
    }
}