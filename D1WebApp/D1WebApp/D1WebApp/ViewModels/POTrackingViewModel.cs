using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.ViewModels
{
    public class POTrackingViewModel
    {
        public string UserID { get; set; }
        public string Company { get; set; }
        public string Manifest_id { get; set; }
        public string Order { get; set; }
        public string rec_type { get; set; }
        public string tracking_no { get; set; }
        public string manifest_carrier { get; set; }
        public string service_type { get; set; }
        public string ship_via_code { get; set; }
        public string ship_date { get; set; }
        public string pkg_wgt { get; set; }
        public string pkg_no { get; set; }
        public string pack_type { get; set; }
        public string descr { get; set; }
    }
}