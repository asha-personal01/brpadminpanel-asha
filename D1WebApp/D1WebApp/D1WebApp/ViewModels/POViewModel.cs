using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.ViewModels
{
    public class POViewModel
    {
        public long UserID { get; set; }
        public string RecType { get; set; }
        public string PO { get; set; }
        public string line_add { get; set; }
        public string wanted_date { get; set; }
        public string price { get; set; }
    }
}