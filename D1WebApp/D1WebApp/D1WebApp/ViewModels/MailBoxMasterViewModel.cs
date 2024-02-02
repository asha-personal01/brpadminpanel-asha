using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D1WebApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class MailBoxMasterViewModel
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
        
        [AllowHtml]
        public string MailFrom { get; set; }
        [Required(ErrorMessage = "ReplyTo is required")]
        [Display(Name = "Reply To")]
        public string ReplyTo { get; set; }
       
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }




        public MailBoxMasterViewModel()
        { }
        public MailBoxMasterViewModel ConvertToViewModel(MailBox MailBoxModel)
        {
            this.IsActive = MailBoxModel.IsActive;
            this.IsSSL = MailBoxModel.IsSSL;
            this.MailBoxID = MailBoxModel.MailBoxID;
            this.MailBoxName = MailBoxModel.MailBoxName;
            this.MailFrom = MailBoxModel.MailFrom;
            this.Password = MailBoxModel.Password;
            this.ReplyTo = MailBoxModel.ReplyTo;
            this.SMTPPort = Convert.ToString(MailBoxModel.SMTPPort);
            this.SMTPServer = MailBoxModel.SMTPServer;
            this.UserName = MailBoxModel.UserName;
            return this;
        }
        public MailBoxMasterViewModel(MailBox MailBoxModel)
        {
            this.IsActive = MailBoxModel.IsActive;
            this.IsSSL = MailBoxModel.IsSSL;
            this.MailBoxID = MailBoxModel.MailBoxID;
            this.MailBoxName = MailBoxModel.MailBoxName;
            this.MailFrom = MailBoxModel.MailFrom;
            this.Password = MailBoxModel.Password;
            this.ReplyTo = MailBoxModel.ReplyTo;
            this.SMTPPort = Convert.ToString(MailBoxModel.SMTPPort);
            this.SMTPServer = MailBoxModel.SMTPServer;
            this.UserName = MailBoxModel.UserName;

        }
        public static MailBox ConvertToModel(MailBoxMasterViewModel MailMailBoxView)
        {
            MailBox MailBoxModel = new MailBox();
            MailBoxModel.IsActive = MailMailBoxView.IsActive;
            MailBoxModel.IsSSL = MailMailBoxView.IsSSL;
            MailBoxModel.MailBoxID = MailMailBoxView.MailBoxID;
            MailBoxModel.MailBoxName = MailMailBoxView.MailBoxName;
            MailBoxModel.MailFrom = MailMailBoxView.MailFrom;
            MailBoxModel.Password = MailMailBoxView.Password;
            MailBoxModel.ReplyTo = MailMailBoxView.ReplyTo;
            MailBoxModel.SMTPPort = Convert.ToInt16(MailMailBoxView.SMTPPort);
            MailBoxModel.SMTPServer = MailMailBoxView.SMTPServer;
            MailBoxModel.UserName = MailMailBoxView.UserName;

            return MailBoxModel;
        }
        public static List<MailBoxMasterViewModel> ConvertToviewList(List<MailBox> MailBoxList)
        {
            List<MailBoxMasterViewModel> ViewModelList = new List<MailBoxMasterViewModel>();
            foreach (MailBox MailBox in MailBoxList.ToList())
            {
                ViewModelList.Add(new MailBoxMasterViewModel(MailBox));
            }
            return ViewModelList;
        }
    }
}