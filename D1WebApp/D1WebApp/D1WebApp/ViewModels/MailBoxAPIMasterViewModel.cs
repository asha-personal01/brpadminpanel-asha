using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D1WebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class MailBoxAPIMasterViewModel
    {
        public long MailBoxID { get; set; }
        [Required(ErrorMessage = "Mail Box Name is required")]
        [Display(Name = "Mail Box Name")]
        public string MailBoxName { get; set; }
        [Required(ErrorMessage = "SMTP Server is required")]
        [Display(Name = "SMTP Server")]
        public string SMTPServer { get; set; }
        [Required(ErrorMessage = "SMTP Port is required")]
        [Display(Name = "SMTP Port")]
        public string SMTPPort { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Display(Name = "IsSSL")]
        public bool IsSSL { get; set; }
        [Required(ErrorMessage = "News Letter From is required")]
        [Display(Name = "Mail From")]
        public string MailFrom { get; set; }
        [Required(ErrorMessage = "ReplyTo is required")]
        [Display(Name = "Reply To")]
        public string ReplyTo { get; set; }
       
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
        public string ConfigurationValue { get; set; }
    }
}