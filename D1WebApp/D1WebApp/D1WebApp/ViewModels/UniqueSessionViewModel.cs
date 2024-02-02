using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class UniqueSessionViewModel
    {
        public string UniqueID { get; set; }
        public long MemRefNo { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
    }
}