//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace D1WebApp.ClientModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class order
    {
        public int index { get; set; }
        public string ordernumber { get; set; }
        public string custid { get; set; }
        public string shipmethod { get; set; }
        public string shipviacode { get; set; }
        public short shipComplete { get; set; }
        public string shipcost { get; set; }
        public Nullable<decimal> shipDiscount { get; set; }
        public string subtotal { get; set; }
        public string tax { get; set; }
        public string total { get; set; }
        public string status { get; set; }
        public string odate { get; set; }
        public string otime { get; set; }
        public string sdate { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public string emailCC { get; set; }
        public string emailTemplate { get; set; }
        public string shipnum { get; set; }
        public string wantedDate { get; set; }
        public string cancelDate { get; set; }
        public Nullable<int> billadr { get; set; }
        public Nullable<int> shipadr { get; set; }
        public string name { get; set; }
        public string cname { get; set; }
        public string address { get; set; }
        public string state { get; set; }
        public string postal { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string phext { get; set; }
        public string fax { get; set; }
        public short residential { get; set; }
        public string cartName { get; set; }
        public string ffContact { get; set; }
        public string ffPhone { get; set; }
        public string ffInstr { get; set; }
        public Nullable<int> checkoutPage { get; set; }
        public string discountAmt { get; set; }
        public string discountCd { get; set; }
        public string discountDescr { get; set; }
        public string ordInstr { get; set; }
        public Nullable<decimal> frVolDisCustTotal { get; set; }
        public string promoCode { get; set; }
        public string promoCodeAmt { get; set; }
        public string terms_code { get; set; }
        public string cu_po { get; set; }
        public string pay_code { get; set; }
        public string pay_code_card { get; set; }
        public string warehouse { get; set; }
        public string ord_class { get; set; }
        public string wanted_date_range { get; set; }
        public string punchout_id { get; set; }
        public Nullable<short> resubmit { get; set; }
        public string job_rel { get; set; }
        public Nullable<int> v2_order { get; set; }
        public string cell_phone { get; set; }
        public string ship_fob { get; set; }
        public string ship_fob_carrier { get; set; }
        public string ship_fob_account { get; set; }
        public Nullable<decimal> total_weight { get; set; }
    }
}
