using D1WebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class MailTemplateAPIMasterViewModel
    {
        public long MailTemplateID { get; set; }
        [DisplayName("Mail Template Name")]
        [Required(ErrorMessage = "Mail Template Name is Required")]
        public string MailTemplateName { get; set; }
        [DisplayName("Mail Template Subject")]
        [Required(ErrorMessage = "Mail Template Subject is Required")]
        public string MailTemplateSubject { get; set; }
        [DisplayName("Mail Template Content")]
        [AllowHtml]
        [Required(ErrorMessage = "Mail Template Content is Required")]
        public string MailTemplateContent { get; set; }
        [DisplayName("Status")]
        public bool IsActive { get; set; }
        public string ConfigurationValue { get; set; }
    }
}