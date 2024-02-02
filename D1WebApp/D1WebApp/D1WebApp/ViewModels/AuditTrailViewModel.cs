using D1WebApp.ClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace D1WebApp.BusinessLogicLayer.ViewModels
{

    public class SchedulerConfigViewModels
    {
        public int? SchedulerConfigId { get; set; }
        public string SchedulerName { get; set; }
        public string SchedulerTime { get; set; }
        public string SchedulerTables { get; set; }
        public long? UserId { get; set; }
        public bool? Active { get; set; }

    }

    public class activitylogviewmodel
    {
        public string memRefNo { get; set; }
        public long ActivityLogId { get; set; }
        public string LogType { get; set; }
        public string Description { get; set; }
        public string SearchKeyword { get; set; }
        public string CustID { get; set; }
        public string UserId { get; set; }
        public string ClientIP { get; set; }
        public DateTime? LogDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ClientBannerViewModel
    {
        public int BannerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsActive { get; set; }
        public string memRefNo { get; set; }
        public string targeturl { get; set; }
        public string types { get; set; }
        public string linkname { get; set; }


        public static tblBanner ConvertToModel(ClientBannerViewModel ClientBannerView)
        {
            tblBanner ntblBanner = new tblBanner();
            ntblBanner.BannerId = ClientBannerView.BannerId;
            ntblBanner.Description = ClientBannerView.Description;
            ntblBanner.ImageUrl = ClientBannerView.ImageUrl;
            ntblBanner.IsActive = ClientBannerView.IsActive;
            ntblBanner.Title = ClientBannerView.Title;
            ntblBanner.targeturl = ClientBannerView.targeturl;
            ntblBanner.types = ClientBannerView.types;
            ntblBanner.linkname = ClientBannerView.linkname;
            return ntblBanner;
        }
        public static ClientBannerViewModel ConvertToViewModel(tblBanner ClientBannerView)
        {
            ClientBannerViewModel ntblBanner = new ClientBannerViewModel();
            ntblBanner.BannerId = ClientBannerView.BannerId;
            ntblBanner.Description = ClientBannerView.Description;
            ntblBanner.ImageUrl = ClientBannerView.ImageUrl;
            ntblBanner.IsActive = ClientBannerView.IsActive;
            ntblBanner.Title = ClientBannerView.Title;
            ntblBanner.targeturl = ClientBannerView.targeturl;
            ntblBanner.types = ClientBannerView.types;
            ntblBanner.linkname = ClientBannerView.linkname;
            return ntblBanner;
        }
        public static List<ClientBannerViewModel> ConvertToViewModelList(List<tblBanner> tblBannerList)
        {
            List<ClientBannerViewModel> ClientBannerViewModelList = new List<ClientBannerViewModel>();
            foreach (var item in tblBannerList)
            {
                ClientBannerViewModelList.Add(ClientBannerViewModel.ConvertToViewModel(item));
            }
            return ClientBannerViewModelList;
        }
    }

    public class dynamicpageviewmodel {
        public string memRefNo { get; set; }
        public int PageID { get; set; }
        public string PageName { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }
        public string PageContent { get; set; }
        public string PageType { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
    }
    public class MailTemplateViewModelNew
    {
        public string memRefNo { get; set; }
        public int MailTemplateID { get; set; }
        public string MailTemplateContent { get; set; }
        public string MailTemplateName { get; set; }
        public string MailTemplateSubject { get; set; }
        public bool IsActive { get; set; }
        public string MailType { get; set; }

    }
    

    public class UpdateActiveInActiveViewModel
    {
        public string memRefNo { get; set; }
        public List<string> items { get; set; }
        public bool isItemActive { get; set; }
    }

    public class headerlinkViewModel
    {
        public int linkid { get; set; }
        public string linkname { get; set; }
        public string linkurl { get; set; }
        public Nullable<int> seq { get; set; }
        public Nullable<bool> popover { get; set; }
        public string memRefNo { get; set; }
        public string types { get; set; }
        public Nullable<int> parentseq { get; set; }
        public Nullable<int> ismenu { get; set; }

        public static headerlink ConvertToModel(headerlinkViewModel headerlinkView)
        {
            headerlink newheaderlink = new headerlink();
            newheaderlink.linkid = headerlinkView.linkid;
            newheaderlink.linkname = headerlinkView.linkname;
            newheaderlink.linkurl = headerlinkView.linkurl;
            newheaderlink.popover = headerlinkView.popover;
            newheaderlink.seq = headerlinkView.seq;
            newheaderlink.types = headerlinkView.types;
            newheaderlink.parent_seq = headerlinkView.parentseq;
            newheaderlink.ismenu = headerlinkView.ismenu;
            return newheaderlink;
        }
    }

    public class ColorConfigViewModel
    {
        public string memRefNo { get; set; }
        public int configid { get; set; }
        public string configkey { get; set; }
        public string configvalue { get; set; }
        public string OldValue { get; set; }
    }

    public class AuditTrailViewModel
    {
        public long AuditTrailID { get; set; }
        public long RecordID { get; set; }
        public long? UserID { get; set; }
        public int ActionType { get; set; }
        public DateTime ActionDate { get; set; }
        public int? RecordType { get; set; }
    }
}