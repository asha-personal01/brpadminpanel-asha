using D1WebApp.Models;
using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.ViewModels;
using System.Text;

namespace D1WebApp.Utilities
{
    public class Mail
    {
        public enum MailBoxName
        {
            Default,
            Support,
            Sales
        }

        public static bool SendOfferDetailsMail(PurchaseOrderDetailsViewModel PurchaseOrderDetails)
        {
            //PurchaseOrderDetails = new PurchaseOrderDetailsViewModel();
            //PurchaseOrderDetails.product_line = new List<PurchaseOrderProductdetailsViewModel>();
            //PurchaseOrderDetails.MailReceipient = "npanchal108@gmail.com";
            //PurchaseOrderDetails.adr = "abc";
            //PurchaseOrderDetails.country_code = "abc";
            //PurchaseOrderDetails.currency_code = "abc";
            //PurchaseOrderDetails.Delivey_terms = "abc";
            //PurchaseOrderDetails.email = "abc";
            //PurchaseOrderDetails.enter_by = "abc";
            //PurchaseOrderDetails.exchange_rate = "abc";
            //PurchaseOrderDetails.fax = "abc";
            //PurchaseOrderDetails.follow_up_code = "abc";
            //PurchaseOrderDetails.follow_up_date = "abc";
            //PurchaseOrderDetails.name = "abc";
            //PurchaseOrderDetails.ord_date = "abc";
            //PurchaseOrderDetails.phone = "abc";
            //PurchaseOrderDetails.phone_ext = "abc";
            //PurchaseOrderDetails.PO = "abc";
            //PurchaseOrderDetails.postal_code = "abc";
            //PurchaseOrderDetails.ship_atn = "abc";
            //PurchaseOrderDetails.ship_ID = "abc";
            //PurchaseOrderDetails.ship_via_code = "abc";
            //PurchaseOrderDetails.state = "abc";
            //PurchaseOrderDetails.vendor = "abc";
            //PurchaseOrderDetails.wanted_date = "abc";
            //PurchaseOrderProductdetailsViewModel prod1 = new PurchaseOrderProductdetailsViewModel();
            //prod1.condition = "abc";
            //prod1.delivery = "abc";
            //prod1.descr1 = "abc";
            //prod1.descr2 = "abc";
            //prod1.expdate = "abc";
            //prod1.instock = "abc";
            //prod1.item = "abc";
            //prod1.line = "abc";
            //prod1.line_add = "abc";
            //prod1.minqty = "abc";
            //prod1.Note = "abc";
            //prod1.orderedext = "abc";
            //prod1.packaging = "abc";
            //prod1.packqty = "abc";
            //prod1.q_ord_d = "abc";
            //prod1.req_date = "abc";
            //prod1.rohs = "abc";
            //prod1.um_o = "abc";
            //prod1.unitcost = "abc";
            //prod1.upc1 = "abc";
            //PurchaseOrderDetails.product_line.Add(prod1);
            //PurchaseOrderProductdetailsViewModel prod2 = new PurchaseOrderProductdetailsViewModel();
            //prod2.condition = "abc";
            //prod2.delivery = "abc";
            //prod2.descr1 = "abc";
            //prod2.descr2 = "abc";
            //prod2.expdate = "abc";
            //prod2.instock = "abc";
            //prod2.item = "abc";
            //prod2.line = "abc";
            //prod2.line_add = "abc";
            //prod2.minqty = "abc";
            //prod2.Note = "abc";
            //prod2.orderedext = "abc";
            //prod2.packaging = "abc";
            //prod2.packqty = "abc";
            //prod2.q_ord_d = "abc";
            //prod2.req_date = "abc";
            //prod2.rohs = "abc";
            //prod2.um_o = "abc";
            //prod2.unitcost = "abc";
            //prod2.upc1 = "abc";
            //PurchaseOrderDetails.product_line.Add(prod2);

            bool flag = false;
            MailTempleteRepository mailtemp = new MailTempleteRepository(new D1WebAppEntities());
            var offerdetails = mailtemp.GetMailTemplete("OfferDetails");
            var Productdetails = mailtemp.GetMailTemplete("ProductDetails");
            StringBuilder TableRows = new StringBuilder();
            
            UserRepository Userreppo = new UserRepository(new D1WebAppEntities());
            var getuser = Userreppo.GetUserByUserID(PurchaseOrderDetails.UserID);
            MailBoxRepository mailbox = new MailBoxRepository(new D1WebAppEntities());
            var mailb = mailbox.GetMailBoxDetailsByUserID(getuser.UserID);
            if(mailb==null || string.IsNullOrEmpty(mailb.MailBoxName))
            {
                mailb = mailbox.GetMailBoxDetailByMailBoxID(1);
            }

            foreach (var item in PurchaseOrderDetails.product_line)
            {
                TableRows.Append(Productdetails.MailTemplateContent.Replace("{{line}}", item.line)
                                .Replace("{{line_add}}", item.line_add)
                                .Replace("{{item}}", item.item)
                                .Replace("{{upc1}}", item.upc1)
                                .Replace("{{descr1}}", item.descr1)
                                .Replace("{{descr2}}", item.descr2)
                                .Replace("{{q_ord_d}}", item.q_ord_d)
                                .Replace("{{um_o}}", item.um_o)                                
                                .Replace("{{req_date}}", item.req_date)
                                .Replace("{{unitcost}}", item.unitcost)
                                .Replace("{{orderedext}}", item.orderedext)
                                .Replace("{{rohs}}", item.rohs)
                                .Replace("{{condition}}", item.condition)
                                .Replace("{{packaging}}", item.packaging)
                                .Replace("{{delivery}}", item.delivery)
                                .Replace("{{packqty}}", item.packqty)
                                .Replace("{{minqty}}", item.minqty)
                                .Replace("{{instock}}", item.instock)
                                .Replace("{{expdate}}", item.expdate)
                                .Replace("{{Note}}", item.Note)
                                );
            }

            string message = offerdetails.MailTemplateContent.Replace("{{vendor}}", PurchaseOrderDetails.vendor)
                .Replace("{{ship_ID}}", PurchaseOrderDetails.ship_ID)
                .Replace("{{ship_via_code}}", PurchaseOrderDetails.ship_via_code)
                .Replace("{{name}}", PurchaseOrderDetails.name)
                .Replace("{{ship_atn}}", PurchaseOrderDetails.ship_atn)
                .Replace("{{adr}}", PurchaseOrderDetails.adr)
                .Replace("{{state}}", PurchaseOrderDetails.state)
                .Replace("{{postal_code}}", PurchaseOrderDetails.postal_code)
                .Replace("{{country_code}}", PurchaseOrderDetails.country_code)
                .Replace("{{phone}}", PurchaseOrderDetails.phone)
                .Replace("{{phone_ext}}", PurchaseOrderDetails.phone_ext)
                .Replace("{{fax}}", PurchaseOrderDetails.fax)
                .Replace("{{email}}", PurchaseOrderDetails.email)
                .Replace("{{PO}}", PurchaseOrderDetails.PO)
                .Replace("{{ord_date}}", PurchaseOrderDetails.ord_date)
                .Replace("{{wanted_date}}", PurchaseOrderDetails.wanted_date)
                .Replace("{{follow_up_date}}", PurchaseOrderDetails.follow_up_date)
                .Replace("{{follow_up_code}}", PurchaseOrderDetails.follow_up_code)
                .Replace("{{currency_code}}", PurchaseOrderDetails.currency_code)
                .Replace("{{exchange_rate}}", PurchaseOrderDetails.exchange_rate)
                .Replace("{{Delivey_terms}}", PurchaseOrderDetails.Delivey_terms)
                .Replace("{{enter_by}}", PurchaseOrderDetails.enter_by)
                .Replace("{{ProductDetailsLines}}", TableRows.ToString())
                ;

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(mailb.SMTPServer);

            mail.From = new MailAddress(mailb.MailFrom);
            mail.To.Add(getuser.NotificationEmails);
            mail.Subject = offerdetails.MailTemplateSubject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            SmtpServer.Port = mailb.SMTPPort;
            SmtpServer.Credentials = new System.Net.NetworkCredential(mailb.UserName, mailb.Password);
            SmtpServer.EnableSsl = mailb.IsSSL;

            SmtpServer.Send(mail);
            flag = true;

            return flag;
        }

        public static void SendMail(String MailRecipient, String MailSubject, String MailMessage, MailBoxName? mailboxname, Nullable<System.DateTime> SValidTo = null, string ReplyTo = null)
        {
            Configuration UrlPrefixs = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UrlPrefix");
            string UrlPrefix = UrlPrefixs.ConfigurationValue;
            MailMessage = MailMessage.Replace("{{UrlPrefix}}", UrlPrefix);
            Configuration LogoDisplaypath = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("LogoDisplayPath");
            string Logopath = LogoDisplaypath.ConfigurationValue;
            string GetPath = "##https://D1WebApp.blob.core.windows.net##";
            MailMessage = MailMessage.Replace(GetPath, Logopath);
            String mailboxnames = "";
            if (mailboxname.ToString() == "")
            {
                mailboxnames = "Default";
            }
            else
            {
                mailboxnames = mailboxname.ToString();
            }
            MailQueueViewModel mailqueuevm = new MailQueueViewModel();
            if (string.IsNullOrEmpty(MailRecipient) || string.IsNullOrWhiteSpace(MailRecipient) || MailRecipient.Equals("N/A"))
            {
                Configuration configurationsEmail = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("DefaultEmail");
                mailqueuevm.MailRecipient = configurationsEmail.ConfigurationValue;
            }
            else
            {
                mailqueuevm.MailRecipient = MailRecipient;
            }
            mailqueuevm.MailSubject = MailSubject;
            mailqueuevm.MailMessage = MailMessage;
            mailqueuevm.MailSValidTo = SValidTo;
            mailqueuevm.IsProccessed = false;
            mailqueuevm.IsDelivered = false;
            mailqueuevm.CreatedOn = D1WebApp.Utilities.Miscellaneous.CurrentDateTime();
            mailqueuevm.ProccessedOn = null;
            mailqueuevm.SentOn = null;
            mailqueuevm.ReplyTo = ReplyTo;
            MailBoxRepository mailboxrep = new MailBoxRepository(new D1WebAppEntities());
            Int64 MBID = mailboxrep.GetMailBoxID(mailboxnames);
            mailqueuevm.MailBoxID = MBID;
            if (MBID != 0)
            {
                MailQueueRepository mailqueuerep = new MailQueueRepository(new D1WebAppEntities());
                mailqueuerep.InsertMailQueue(mailqueuevm);
                mailqueuerep.Save();
            }
            else
            {
                String strMsg = "MailBoxID : " + mailqueuevm.MailBoxID + "MailBox Not Found";
                ErrorLogs.ErrorLog(0, "Unknwon", Miscellaneous.CurrentDateTime(), "Mail box", strMsg, "Mail box",1);
            }
        }
        public static void ProccessMailQueue()
        {
            GetMailQueueDetails();
            GetMailQueueDetailsOfSValidTo();
        }
        public static void GetMailQueueDetails()
        {
            MailQueueRepository objMailRepository = new MailQueueRepository(new D1WebAppEntities());
            List<MailQueueViewModel> MailQueueList = objMailRepository.GetMailQueue();
            MailBoxRepository objMailBoxRepository = new MailBoxRepository(new D1WebAppEntities());
            foreach (MailQueueViewModel Mail in MailQueueList)
            {
                bool flag = false;
                MailQueueViewModel VMMailQueue = objMailRepository.GetMailQueueDetailByID(Mail.MailQueueID);
                VMMailQueue.IsProccessed = true;
                VMMailQueue.ProccessedOn = D1WebApp.Utilities.Miscellaneous.CurrentDateTime();
                objMailRepository.UpdateMailQueue(VMMailQueue);
                objMailRepository.Save();
                MailBoxViewModel MailBoxDetail = objMailBoxRepository.GetMailBoxDetailByMailBoxID(Mail.MailBoxID);
                flag=SendEmail(Mail.MailQueueID, Mail.MailRecipient, MailBoxDetail.MailFrom, Mail.MailSubject, Mail.MailMessage, MailBoxDetail.SMTPServer, MailBoxDetail.SMTPPort, MailBoxDetail.UserName, MailBoxDetail.Password, MailBoxDetail.IsSSL, Mail.ReplyTo);
                //D1WebApp.Utilities.Miscellaneous.InsertTrackRecordsForJobs(Mail.MailQueueID, flag, 4);
            }
        }
        public static void GetMailQueueDetailsOfSValidTo()
        {
            MailQueueRepository objMailRepository = new MailQueueRepository(new D1WebAppEntities());
            List<MailQueueViewModel> MailQueueList = objMailRepository.GetMailQueueBySValidTo();
            MailBoxRepository objMailBoxRepository = new MailBoxRepository(new D1WebAppEntities());
            foreach (MailQueueViewModel Mail in MailQueueList)
            {
                bool flag = false;
                MailQueueViewModel VMMailQueue = objMailRepository.GetMailQueueDetailByID(Mail.MailQueueID);
                VMMailQueue.IsProccessed = true;
                VMMailQueue.ProccessedOn = D1WebApp.Utilities.Miscellaneous.CurrentDateTime();
                objMailRepository.UpdateMailQueue(VMMailQueue);
                objMailRepository.Save();
                MailBoxViewModel MailBoxDetail = objMailBoxRepository.GetMailBoxDetailByMailBoxID(Mail.MailBoxID);
                flag=SendEmail(Mail.MailQueueID, Mail.MailRecipient, MailBoxDetail.MailFrom, Mail.MailSubject, Mail.MailMessage, MailBoxDetail.SMTPServer, MailBoxDetail.SMTPPort, MailBoxDetail.UserName, MailBoxDetail.Password, MailBoxDetail.IsSSL,Mail.ReplyTo);
                //D1WebApp.Utilities.Miscellaneous.InsertTrackRecordsForJobs(Mail.MailQueueID, flag, 4);
            }
        }
        public static bool SendEmail(Int64 MailQueueID, string _MailRecipient, string _MailFrom, string _MailSubject, string _MailMessage, string _SMTPServer, int _SMTPPort, string _UserName, string _Password, bool _IsSSL,string _ReplyTo = null)
        {
            MailQueueRepository objMailRepository = new MailQueueRepository(new D1WebAppEntities());
            MailQueueViewModel objMailQueue = new MailQueueViewModel();
            bool msgsend = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_MailRecipient);
                mail.From = new MailAddress(_MailFrom);
                Configuration configurationssupportmail = GeneralConfiguration.GeneralConfiguration.CheckConfiguration("BccMailAddress");
                mail.Bcc.Add(configurationssupportmail.ConfigurationValue);
                if (_ReplyTo == null)
                {
                    Configuration configurationsReplyEmail = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("DefaultEmail");
                    _ReplyTo = configurationsReplyEmail.ConfigurationValue;
                    mail.ReplyToList.Add(_ReplyTo);
                }
                else
                {
                    mail.ReplyToList.Add(_ReplyTo);
                }
                mail.Subject = _MailSubject;
                string Body = _MailMessage;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = _SMTPServer;
                smtp.Port = _SMTPPort;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(_UserName, _Password);// Enter seders User name and password
                smtp.EnableSsl = _IsSSL;
                smtp.Send(mail);
                msgsend = true;
                MailQueueViewModel VMMailQueue = objMailRepository.GetMailQueueDetailByID(MailQueueID);
                VMMailQueue.IsDelivered = true;
                VMMailQueue.SentOn = D1WebApp.Utilities.Miscellaneous.CurrentDateTime();
                objMailRepository.UpdateMailQueue(VMMailQueue);
                objMailRepository.Save();
            }
            catch (Exception ex)
            {
                msgsend = false;
                ErrorLogs.ErrorLog(0, "Unknwon", Miscellaneous.CurrentDateTime(), "Sendmail not working", ex.Message.ToString(), "Mail",1);
            }
            return msgsend;
        }
        public static void WriteToLogFile(string strMessage, string outputFile)
        {

            string line = D1WebApp.Utilities.Miscellaneous.CurrentDateTime().ToString() + " | ";
            line += strMessage;
            FileStream fs = new FileStream(outputFile, FileMode.Append, FileAccess.Write, FileShare.None);
            StreamWriter swFromFileStream = new StreamWriter(fs);
            swFromFileStream.WriteLine(line);
            swFromFileStream.Flush();
            swFromFileStream.Close();
        }
    }
}