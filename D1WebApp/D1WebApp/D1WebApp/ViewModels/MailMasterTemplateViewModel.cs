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
    public class MailMasterTemplateViewModel
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

      

        public MailMasterTemplateViewModel()
        { }

        public MailMasterTemplateViewModel(D1WebApp.Models.MailTemplate MailTemplateModel)
        {
            this.MailTemplateID = MailTemplateModel.MailTemplateID;
            this.MailTemplateName = MailTemplateModel.MailTemplateName;
            this.MailTemplateSubject = MailTemplateModel.MailTemplateSubject;
            this.MailTemplateContent = MailTemplateModel.MailTemplateContent;
            this.IsActive = MailTemplateModel.IsActive;
           
           
        }

        /// <summary>
        /// Convert Model to ViewModel
        /// </summary>
        /// <param name="marketverticals">marketvertical list from model</param>
        /// <returns>marketvertical list in ViewModel</returns>
        public List<MailMasterTemplateViewModel> ConvertModelToViewModel(List<D1WebApp.Models.MailTemplate> MailTemplateList)
        {
            List<MailMasterTemplateViewModel> MailTemplateViewModellist = new List<MailMasterTemplateViewModel>();
            foreach (D1WebApp.Models.MailTemplate list in MailTemplateList.ToList())
            {
                MailTemplateViewModellist.Add(new MailMasterTemplateViewModel(list));
            }
            return MailTemplateViewModellist.ToList();
        }

        /// <summary>
        /// Convert ViewModel To Model
        /// </summary>
        /// <param name="marketvertical">Viewmodel of MarketVertical</param>
        /// <returns>Model of MarketVertical</returns>
        public D1WebApp.Models.MailTemplate ConvertViewModelToModel(MailMasterTemplateViewModel MailTemplateviewmodel)
        {
            D1WebApp.Models.MailTemplate MailTemplateModel = new D1WebApp.Models.MailTemplate();
            MailTemplateModel.MailTemplateID = MailTemplateviewmodel.MailTemplateID;
            MailTemplateModel.MailTemplateName = MailTemplateviewmodel.MailTemplateName;
            MailTemplateModel.MailTemplateSubject = MailTemplateviewmodel.MailTemplateSubject;
            MailTemplateModel.MailTemplateContent = MailTemplateviewmodel.MailTemplateContent;
            MailTemplateModel.IsActive = MailTemplateviewmodel.IsActive;

            return MailTemplateModel;
        }

        /// <summary>
        /// Update MarketVertical
        /// </summary>
        /// <param name="marketvertical">Object of MarketVertical ViewModel</param>
        /// <param name="marketverticals">Object of MarketVertical Model</param>
        /// <returns>MarketVertical Model Updated</returns>
        public static D1WebApp.Models.MailTemplate UpdateMailTemplate(MailMasterTemplateViewModel MailTemplateviewmodel, MailTemplate MailTemplateModel)
        {
            
            MailTemplateModel.MailTemplateName = MailTemplateviewmodel.MailTemplateName;
            MailTemplateModel.MailTemplateSubject = MailTemplateviewmodel.MailTemplateSubject;
            MailTemplateModel.MailTemplateContent = MailTemplateviewmodel.MailTemplateContent;
            MailTemplateModel.IsActive = MailTemplateviewmodel.IsActive;

            return MailTemplateModel;
        }
    }
}
