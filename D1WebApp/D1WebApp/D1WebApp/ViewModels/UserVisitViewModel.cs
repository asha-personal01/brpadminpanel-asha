using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class UserVisitViewModel
    {
        public long UserVisitID { get; set; }
        public string UniqueID { get; set; }
        public string UrlName { get; set; }
        public long MemRefNo { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public System.DateTime VisitTime { get; set; }
        public string HostName { get; set; }
    }
}