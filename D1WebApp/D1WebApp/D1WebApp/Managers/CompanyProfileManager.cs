//Developed by Nayan

using System.Web;
using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.ClientModel;
using System.Web.UI;

namespace D1WebApp.Utilities
{
    public class CompanyProfileManager
    {
        private CompanyProfileRepository CompanyProfilerepository= new CompanyProfileRepository();

        public CompanyProfileManager()
        {
        }
        public dynamic GetActivitylogsearch(activitylogviewmodel activitylogview)
        {
            return CompanyProfilerepository.GetActivitylogsearch(activitylogview);
        }
        public bool AddConfigurationforapi(string UserMemRefNo, string ApiEndPoint, string AuthonticationToken, string client, string company, string username, string password)
        {
            return CompanyProfilerepository.AddConfigurationforapi(UserMemRefNo, ApiEndPoint, AuthonticationToken, client, company, username, password);
        }
        public dynamic GetBannerDetailsByID(string memRefNo, int bannerID)
        {
            return CompanyProfilerepository.GetBannerDetailsByID(memRefNo, bannerID);
        }
        public dynamic GetBannerListforClient(string memRefNo)
        {
            return CompanyProfilerepository.GetBannerListforClient(memRefNo);
        }
        public dynamic AddAndUpdateBanners(ClientBannerViewModel ClientBannerView)
        {
            return CompanyProfilerepository.AddAndUpdateBanners(ClientBannerView);
        }
        public dynamic AddUpdateItemDocument(ItemDetailsViewModel ItemDetailsViewModel)
        {
            return CompanyProfilerepository.AddUpdateItemDocument(ItemDetailsViewModel);
        }
        public dynamic UpdateItemPriceBulk(string memRefNo, List<ItemPriceListModel> itemDetailsViewModel)
        {
            return CompanyProfilerepository.UpdateItemPriceBulk(memRefNo,itemDetailsViewModel);
        }

        public dynamic GetExternalItemDocFiles(string memRefNo)
        {
            return CompanyProfilerepository.GetExternalItemDocFiles(memRefNo);
        }
        public dynamic UpdateItemDocumentBulk(string memRefNo, List<ItemDetailsViewModel> itemDocuemntViewModel,bool IMType)
        {
            return CompanyProfilerepository.UpdateItemDocumentBulk(memRefNo, itemDocuemntViewModel, IMType);
        }
        public dynamic UpdateExternalItemDocument(string memRefNo, List<ItemDetailsViewModel> itemDocuemntViewModel, bool IMType,string FileName)
        {
            return CompanyProfilerepository.UpdateExternalItemDocument(memRefNo, itemDocuemntViewModel, IMType, FileName);
        }
        public bool AddConfiguations(string UserMemRefNo, string Key, string keyvalue)
        {
            return CompanyProfilerepository.AddConfiguations(UserMemRefNo, Key, keyvalue);
        }
        public dynamic DataMigration(string UserMemRefNo, string Api, string AuthonticationToken, string company) {
            return CompanyProfilerepository.DataMigration(UserMemRefNo, Api, AuthonticationToken, company);
        }
        public dynamic GetHeaderlinklist(string memRefNo)
        {
            return CompanyProfilerepository.GetHeaderlinklist(memRefNo);
        }
        public dynamic GetHeaderlinkByID(string memRefNo, int linkid)
        {
            return CompanyProfilerepository.GetHeaderlinkByID(memRefNo, linkid);
        }
        public dynamic DeleteHeaderlinkByID(string memRefNo, int linkid)
        {
            return CompanyProfilerepository.DeleteHeaderlinkByID(memRefNo, linkid);
        }
        public dynamic DeletesafiltersortByID(string memRefNo, int linkid)
        {
            return CompanyProfilerepository.DeletesafiltersortByID(memRefNo, linkid);
        }
        public dynamic UpdateHeaderlinkByID(string memRefNo, headerlink hlink)
        {
            return CompanyProfilerepository.UpdateHeaderlinkByID(memRefNo, hlink);
        }

        public dynamic GetWebConfigsList(string memRefNo)
        {
            return CompanyProfilerepository.GetWebConfigsList(memRefNo);
        }

        public dynamic GetsafiltersortList(string memRefNo)
        {
            return CompanyProfilerepository.GetsafiltersortList(memRefNo);
        }
        public dynamic GetWebColorConfigsList(string memRefNo)
        {
            return CompanyProfilerepository.GetWebColorConfigsList(memRefNo);
        }
        public dynamic GetMailtemplateList(string memRefNo)
        {
            return CompanyProfilerepository.GetMailTemplateList(memRefNo);
        }
        public dynamic Getdynamicpagelist(string memRefNo)
        {
            return CompanyProfilerepository.Getdynamicpagelist(memRefNo);
        }
        public dynamic Getproductlist(string memRefNo, int pageno)
        {
            return CompanyProfilerepository.Getproductlist(memRefNo, pageno);
        }
        public dynamic GetFilteredproductlist(string memRefNo, string filterQuery, int activeFlag, int pageno)
        {
            return CompanyProfilerepository.GetFilteredproductlist(memRefNo, filterQuery, activeFlag, pageno);
        }

        public dynamic UpdateWebConfigsList(string memRefNo, int configid, string configkey, string configvalue)
        {
            return CompanyProfilerepository.UpdateWebConfigsList(memRefNo, configid, configkey, configvalue);
        }
        public dynamic Updatesafilterorder(string memRefNo, int configid, string configkey, int configvalue, int sorder)
        {
            return CompanyProfilerepository.Updatesafilterorder(memRefNo, configid, configkey, configvalue,sorder);
        }
        public dynamic UpdateColorConfigsList(string memRefNo, int configid, string configkey, string configvalue,string oldvalue)
        {
            return CompanyProfilerepository.UpdateColorConfigsList(memRefNo, configid, configkey, configvalue, oldvalue);
        }
        public dynamic UpdateMailtemplate(MailTemplateViewModelNew MailTemplateView)
        {
            return CompanyProfilerepository.UpdateMailTemplate(MailTemplateView);
        }
        public dynamic Updatesynamicpage(dynamicpageviewmodel MailTemplateView)
        {
            return CompanyProfilerepository.Updatesynamicpage(MailTemplateView);
        }
        public dynamic Deletedynamic(string memRefNo, int pageID)
        {
            return CompanyProfilerepository.Deletedynamicpage(memRefNo, pageID);
        }
        public dynamic DeleteItemDocByID(string memRefNo, int itemDocId)
        {
            return CompanyProfilerepository.DeleteItemDocByID(memRefNo, itemDocId);
        }
        public dynamic UpdateItemPrice(string memRefNo, string item, decimal price, bool isItemActive)
        {
            return CompanyProfilerepository.UpdateItemPrice(memRefNo, item, price, isItemActive);
        }
        public dynamic UpdateBulkActiveInActive(UpdateActiveInActiveViewModel updateActiveInActiveViewModel)
        {
            return CompanyProfilerepository.UpdateBulkActiveInActive(updateActiveInActiveViewModel);
        }
        public dynamic GetConfigByID(string memRefNo, int configid)
        {
            return CompanyProfilerepository.GetConfigByID(memRefNo, configid);
        }
        public dynamic Getsafiltersort(string memRefNo, int configid)
        {
            return CompanyProfilerepository.Getsafiltersort(memRefNo, configid);
        }
        public ColorConfiguration GetColorConfigByID(string memRefNo, int configid)
        {
            return CompanyProfilerepository.GetColorConfigByID(memRefNo, configid);
        }
        public dynamic GetMailtemplateByID(string memRefNo, int configid)
        {
            return CompanyProfilerepository.GetMailTemplateByID(memRefNo, configid);
        }
        public dynamic GetdynamicpageByID(string memRefNo, int pageid)
        {
            return CompanyProfilerepository.GetdynamicpageByID(memRefNo, pageid);
        }
        public dynamic GetItemDocByID(string memRefNo, string item)
        {
            return CompanyProfilerepository.GetItemDocByID(memRefNo, item);
        }
        public dynamic GetManufracturerItemDocList(ManufracturerParamViewModel manufracturerParamViewModel)
        {
            return CompanyProfilerepository.GetManufracturerItemDocList(manufracturerParamViewModel);
        }
        public dynamic GetUserCartItemDetails(CartReportParam cartReportParam)
        {
            return CompanyProfilerepository.GetUserCartItemDetails(cartReportParam);
        }
        public dynamic GetFilteredManufracturerItemDocList(ManufracturerParamViewModel manufracturerParamViewModel)
        {
            return CompanyProfilerepository.GetFilteredManufracturerItemDocList(manufracturerParamViewModel);
        }
        
        public dynamic GetItemPriceByItem(string memRefNo, string item)
        {
            return CompanyProfilerepository.GetItemPriceByItem(memRefNo, item);
        }
        public dynamic GetDocumentTypeConfigValue(string memRefNo)
        {
            return CompanyProfilerepository.GetDocumentTypeConfigValue(memRefNo);
        }
        //public List<CompanyProfileViewModel> GetAll(string UserMemRefNo)
        //{
        //    return CompanyProfilerepository.GetAll(UserMemRefNo);
        //}
        //public List<CompanyProfileViewModel> GetCompanyProfileListByPage(int page, int pagenumber, string UserMemRefNo)
        //{
        //    return CompanyProfilerepository.GetCompanyProfileListByPage(page, pagenumber, UserMemRefNo);
        //}
        //public bool InsertCompanyProfile(CompanyProfileViewModel NewCompanyProfile)
        //{
        //    return CompanyProfilerepository.InsertCompanyProfile(NewCompanyProfile);
        //}
        //public CompanyProfileViewModel GetDataByID(string UserMemRefNo)
        //{
        //    return CompanyProfilerepository.GetDataByID(UserMemRefNo);
        //}
        //public bool UpdateCompanyProfile(CompanyProfileViewModel NewCompanyProfile)
        //{
        //    return CompanyProfilerepository.UpdateCompanyProfile(NewCompanyProfile);
        //}
        //public bool DeleteCompanyProfile(string UserMemRefNo)
        //{
        //    return CompanyProfilerepository.DeleteCompanyProfile(UserMemRefNo);
        //}

    }
}