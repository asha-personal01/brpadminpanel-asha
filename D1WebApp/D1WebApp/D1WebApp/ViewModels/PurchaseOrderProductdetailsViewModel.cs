using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.ViewModels
{
    public class PurchaseOrderProductdetailsViewModel
    {
        public string line { get; set; }
        public string line_add { get; set; }
        public string item { get; set; }
        public string upc1 { get; set; }
        public string descr1 { get; set; }
        public string descr2 { get; set; }
        public string q_ord_d { get; set; }
        public string um_o { get; set; }
        public string req_date { get; set; }
        public string unitcost { get; set; }
        public string orderedext { get; set; }
        public string rohs { get; set; }
        public string condition { get; set; }
        public string packaging { get; set; }
        public string delivery { get; set; }
        public string packqty { get; set; }
        public string minqty { get; set; }
        public string instock { get; set; }
        public string expdate { get; set; }
        public string Note { get; set; }
    }
}