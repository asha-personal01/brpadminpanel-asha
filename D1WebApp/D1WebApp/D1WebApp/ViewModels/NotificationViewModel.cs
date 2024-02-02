// Created by sunil
// Date : 7/6/2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D1WebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class NotificationViewModel
    {
        [DisplayName("Notification Type")]
        public string NotificationType { get; set; }
        public string Remarks { get; set; }
        [DisplayName("From")]
        public long FromUserID { get; set; }
        public long ToUserID { get; set; }
        public string NotificationStyle { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Parameters { get; set; }
        [DisplayName("TimeStamp")]
        public DateTime NotificationTimeStamp { get; set; }

        public NotificationViewModel()
        {

        }

        public NotificationViewModel(Notification notification)
        {
            this.NotificationType = notification.NotificationType;
            this.Remarks = notification.Remarks;
            this.FromUserID = notification.FromUserID;
            this.ToUserID = notification.ToUserID;
            this.NotificationStyle = notification.NotificationStyle;
            this.Controller = notification.Controller;
            this.Action = notification.Action;
            this.Parameters = notification.Parameters;
            this.NotificationTimeStamp = notification.NotificationTimeStamp;
        }

        public static Notification ConvertViewModelToModel(NotificationViewModel notificationviewmodel)
        {
            Notification notification = new Notification();
            notification.NotificationType = notificationviewmodel.NotificationType;
            notification.Remarks = notificationviewmodel.Remarks;
            notification.NotificationTimeStamp = D1WebApp.Utilities.Miscellaneous.CurrentDateTime();
            notification.FromUserID = notificationviewmodel.FromUserID;
            notification.ToUserID = notificationviewmodel.ToUserID;
            notification.NotificationStyle = notificationviewmodel.NotificationStyle;
            notification.Controller = notificationviewmodel.Controller;
            notification.Action = notificationviewmodel.Action;
            notification.Parameters = notificationviewmodel.Parameters;

            return notification;
        }

        public static List<NotificationViewModel> ConvertModelToViewModel(List<Notification> Notifications)
        {
            List<NotificationViewModel> notificationviewmodel = new List<NotificationViewModel>();

            foreach (Notification notification in Notifications)
            {
                notificationviewmodel.Add(new NotificationViewModel(notification));
            }

            return notificationviewmodel;
        }

    }
}