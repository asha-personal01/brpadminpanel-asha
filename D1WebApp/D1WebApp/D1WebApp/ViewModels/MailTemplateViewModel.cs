using D1WebApp.Models;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class MailTemplateViewModel
    {

        public MailTemplateViewModel()
        {
        }
        public MailTemplateViewModel(D1WebApp.Models.MailTemplate mailtemplete)
        {
            this.MailTemplateID = mailtemplete.MailTemplateID;
            this.MailTemplateName = mailtemplete.MailTemplateName;
            this.MailTemplateSubject = mailtemplete.MailTemplateSubject;
            this.MailTemplateContent = mailtemplete.MailTemplateContent;
            this.IsActive = mailtemplete.IsActive;
        }
        public long MailTemplateID { get; set; }
        public string MailTemplateName { get; set; }
        public string MailTemplateSubject { get; set; }
        public string MailTemplateContent { get; set; }
        public bool IsActive { get; set; }
    }
}