//Created by Sunil
//Date : 07/06/2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class NotificationDisplayViewModel
    {
        public string NotificationTimeStamp { get; set; }
        public string From { get; set; }
        public string NotificationType { get; set; }
        public string Remarks { get; set; }
        public string RedirectLink { get; set; }
        public string NotificationStyle { get; set; }
    }
}