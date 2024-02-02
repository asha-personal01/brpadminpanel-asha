using System.Collections.Generic;
using D1WebApp.Models;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class UserMailBoxViewModel
    {
        public UserMailBoxViewModel()
        {
        }
        public UserMailBoxViewModel(UserMailBox mailbox)
        {
            this.MailBoxID = mailbox.MailBoxID;
            this.MailBoxName = mailbox.MailBoxName;
            this.SMTPServer = mailbox.SMTPServer;
            this.SMTPPort = mailbox.SMTPPort;
            this.UserName = mailbox.UserName;
            this.Password = mailbox.Password;
            this.IsSSL = mailbox.IsSSL; ;
            this.MailFrom = mailbox.MailFrom;
            this.ReplyTo = mailbox.ReplyTo;
            this.IsActive = mailbox.IsActive;
            this.UserID = mailbox.UserID;
            this.DisplayName = mailbox.DisplayName;
            this.Company = mailbox.Company;
        }
        public static UserMailBox ConvertToModel(UserMailBoxViewModel UserMailBoxView)
        {
            UserMailBox NewUserMailBox = new UserMailBox();
            if (UserMailBoxView.MailBoxID != null && UserMailBoxView.MailBoxID > 0)
            {
                NewUserMailBox.MailBoxID = UserMailBoxView.MailBoxID;
            }
            NewUserMailBox.MailBoxName = UserMailBoxView.MailBoxName;
            NewUserMailBox.SMTPServer = UserMailBoxView.SMTPServer;
            NewUserMailBox.SMTPPort = UserMailBoxView.SMTPPort;
            NewUserMailBox.UserName = UserMailBoxView.UserName;
            NewUserMailBox.Password = UserMailBoxView.Password;
            NewUserMailBox.IsSSL = UserMailBoxView.IsSSL; ;
            NewUserMailBox.MailFrom = UserMailBoxView.MailFrom;
            NewUserMailBox.ReplyTo = UserMailBoxView.ReplyTo;
            NewUserMailBox.IsActive = UserMailBoxView.IsActive;
            NewUserMailBox.UserID = UserMailBoxView.UserID;
            NewUserMailBox.DisplayName = UserMailBoxView.DisplayName;
            NewUserMailBox.Company = UserMailBoxView.Company;
            return NewUserMailBox;
        }

        public static List<MailBoxViewModel> ConvertToViewModelList(List<MailBox> mailboxes)
        {
            List<MailBoxViewModel> mailboxeslist = new List<MailBoxViewModel>();
            foreach (MailBox mailbox in mailboxes)
            {
                mailboxeslist.Add(new MailBoxViewModel(mailbox));
            }
            return mailboxeslist;
        }
        public static List<UserMailBoxViewModel> ConvertToViewModelListNew(List<UserMailBox> mailboxes)
        {
            List<UserMailBoxViewModel> mailboxeslist = new List<UserMailBoxViewModel>();
            foreach (UserMailBox mailbox in mailboxes)
            {
                mailboxeslist.Add(new UserMailBoxViewModel(mailbox));
            }
            return mailboxeslist;
        }
        public string memRefNo { get; set; }
        public long UserID { get; set; }
        public long MailBoxID { get; set; }
        public string MailBoxName { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsSSL { get; set; }
        public string MailFrom { get; set; }
        public string ReplyTo { get; set; }
        public bool IsActive { get; set; }
        public string DisplayName { get; set; }
        public string Company { get; set; }

        public virtual ICollection<MailQueueViewModel> MailQueues { get; set; }
    }


    public class MailBoxViewModel
    {
        public MailBoxViewModel()
        {
        }
        public MailBoxViewModel(MailBox mailbox)
        {
            this.MailBoxID = mailbox.MailBoxID;
            this.MailBoxName = mailbox.MailBoxName;
            this.SMTPServer = mailbox.SMTPServer;
            this.SMTPPort = mailbox.SMTPPort;
            this.UserName = mailbox.UserName;
            this.Password = mailbox.Password;
            this.IsSSL = mailbox.IsSSL; ;
            this.MailFrom = mailbox.MailFrom;
            this.ReplyTo = mailbox.ReplyTo;
            this.IsActive = mailbox.IsActive;
        }
        public MailBoxViewModel(UserMailBox mailbox)
        {
            this.MailBoxID = mailbox.MailBoxID;
            this.MailBoxName = mailbox.MailBoxName;
            this.SMTPServer = mailbox.SMTPServer;
            this.SMTPPort = mailbox.SMTPPort;
            this.UserName = mailbox.UserName;
            this.Password = mailbox.Password;
            this.IsSSL = mailbox.IsSSL; ;
            this.MailFrom = mailbox.MailFrom;
            this.ReplyTo = mailbox.ReplyTo;
            this.IsActive = mailbox.IsActive;
        }
        public List<MailBoxViewModel> ConvertToViewModelList(List<MailBox> mailboxes)
        {
            List<MailBoxViewModel> mailboxeslist = new List<MailBoxViewModel>();
            foreach (MailBox mailbox in mailboxes)
            {
                mailboxeslist.Add(new MailBoxViewModel(mailbox));
            }
            return mailboxeslist;
        }

        public long MailBoxID { get; set; }
        public string MailBoxName { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsSSL { get; set; }
        public string MailFrom { get; set; }
        public string ReplyTo { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<MailQueueViewModel> MailQueues { get; set; }
    }
}