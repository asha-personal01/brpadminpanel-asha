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
    
    public partial class tblOrderProduct
    {
        public long OrderId { get; set; }
        public string order_ext { get; set; }
        public string notes { get; set; }
        public string customer { get; set; }
        public string company_oe { get; set; }
        public string company_cu { get; set; }
        public string company_it { get; set; }
        public string warehouse { get; set; }
        public string line { get; set; }
        public string item { get; set; }
        public string qty_ord { get; set; }
        public string unit_price { get; set; }
        public string um_o { get; set; }
        public Nullable<bool> IsProcessed { get; set; }
    }
}
