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
    
    public partial class EndUser
    {
        public long UsersID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string password { get; set; }
        public string Email { get; set; }
        public Nullable<bool> Usertype { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> Lastlogindate { get; set; }
    }
}
