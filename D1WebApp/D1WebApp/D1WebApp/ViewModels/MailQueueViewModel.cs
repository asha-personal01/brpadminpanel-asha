/// <summary>
/// MailQueue for ViewModel
/// Create by : Satish Patel
/// Create Date : 05/05/2014
/// Update by : 
/// Update Date :
/// </summary>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using D1WebApp.Models;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class MailQueueViewModel
    {

         public MailQueueViewModel()
        {
        }
         public MailQueueViewModel(MailQueue mailqueue)
        {
            this.MailQueueID = mailqueue.MailQueueID;
            this.MailRecipient = mailqueue.MailRecipient;
            this.MailSubject = mailqueue.MailSubject;
            this.MailMessage = mailqueue.MailMessage;
            this.MailSValidTo =mailqueue.MailSendDate; 	
            this.MailBoxID	=mailqueue.MailBoxID;
            this.IsProccessed = mailqueue.IsProccessed;;
            this.IsDelivered= mailqueue.IsDelivered;	
            this.CreatedOn	= mailqueue.CreatedOn;
            this.ProccessedOn = mailqueue.ProccessedOn;
            this.SentOn = mailqueue.SentOn;
            this.MailBox = mailqueue.MailBox;
            this.ReplyTo = mailqueue.ReplyTo;

        }
         public static MailQueue ConvertToModel(MailQueueViewModel mailqueuevm)
        {
            MailQueue mailqueue = new MailQueue();
            mailqueue.MailQueueID = mailqueuevm.MailQueueID;
            mailqueue.MailRecipient= mailqueuevm.MailRecipient;
            mailqueue.MailSubject= mailqueuevm.MailSubject;
            mailqueue.MailMessage= mailqueuevm.MailMessage;
            mailqueue.MailSendDate= mailqueuevm.MailSValidTo;
            mailqueue.MailBoxID= mailqueuevm.MailBoxID;
            mailqueue.IsProccessed= mailqueuevm.IsProccessed; 
            mailqueue.IsDelivered= mailqueuevm.IsDelivered;
            mailqueue.CreatedOn= mailqueuevm.CreatedOn;
            mailqueue.ProccessedOn= mailqueuevm.ProccessedOn;
            mailqueue.SentOn= mailqueuevm.SentOn;
            mailqueue.MailBox= mailqueuevm.MailBox;
            mailqueue.ReplyTo = mailqueuevm.ReplyTo;
            return mailqueue;
        }
         public static MailQueue ConvertToModel(MailQueueViewModel mailqueuevm,MailQueue mailqueue)
         {
           
             mailqueue.MailQueueID = mailqueuevm.MailQueueID;
             mailqueue.MailRecipient = mailqueuevm.MailRecipient;
             mailqueue.MailSubject = mailqueuevm.MailSubject;
             mailqueue.MailMessage = mailqueuevm.MailMessage;
             mailqueue.MailSendDate = mailqueuevm.MailSValidTo;
             mailqueue.MailBoxID = mailqueuevm.MailBoxID;
             mailqueue.IsProccessed = mailqueuevm.IsProccessed;
             mailqueue.IsDelivered = mailqueuevm.IsDelivered;
             mailqueue.CreatedOn = mailqueuevm.CreatedOn;
             mailqueue.ProccessedOn = mailqueuevm.ProccessedOn;
             mailqueue.SentOn = mailqueuevm.SentOn;
             mailqueue.MailBox = mailqueuevm.MailBox;
             mailqueue.ReplyTo = mailqueuevm.ReplyTo;
             return mailqueue;
         }
         public List<MailQueueViewModel> ConvertToViewModelList(List<MailQueue> mailqueues)
        {
            List<MailQueueViewModel> mailqueuelist = new List<MailQueueViewModel>();
            foreach (MailQueue mailqueue in mailqueues)
            {
                mailqueuelist.Add(new MailQueueViewModel(mailqueue));
            }
            return mailqueuelist;
        }



        public long MailQueueID { get; set; }
        [Required]
        public string MailRecipient { get; set; }
        [Required]
        public string MailSubject { get; set; }
        [Required]
        public string MailMessage { get; set; }
        public Nullable<System.DateTime> MailSValidTo { get; set; }
        public long MailBoxID { get; set; }
        public bool IsProccessed { get; set; }
        public bool IsDelivered { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ProccessedOn { get; set; }
        public Nullable<System.DateTime> SentOn { get; set; }
        public string ReplyTo { get; set; }
        public virtual MailBox MailBox { get; set; }
    }
}