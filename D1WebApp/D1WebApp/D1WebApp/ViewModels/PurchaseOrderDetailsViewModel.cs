using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.ViewModels
{
    public class PurchaseOrderDetailsViewModel
    {
        public long UserID { get; set; }
        public string MailReceipient { get; set; }
        public string vendor { get; set; }
        public string ship_ID { get; set; }
        public string ship_via_code { get; set; }
        public string name { get; set; }
        public string ship_atn { get; set; }
        public string adr { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string phone { get; set; }
        public string phone_ext { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string PO { get; set; }
        public string ord_date { get; set; }
        public string wanted_date { get; set; }
        public string follow_up_date { get; set; }
        public string follow_up_code { get; set; }
        public string currency_code { get; set; }
        public string exchange_rate { get; set; }
        public string Delivey_terms { get; set; }
        public string enter_by { get; set; }
        public List<PurchaseOrderProductdetailsViewModel> product_line { get; set; }
    }
}