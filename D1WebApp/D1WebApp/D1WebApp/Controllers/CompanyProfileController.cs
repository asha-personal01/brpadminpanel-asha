//Developed by Nayan
using D1WebApp.Models;
using System;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Utilities.ApiResponse;
using D1WebApp.Utilities;
using D1WebApp.Utilities.GeneralConfiguration;
using System.IO;
using System.Web.Hosting;
using System.Data.Entity.Core.EntityClient;
using D1WebApp.Controllers;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using System.Web.UI.WebControls;
using D1WebApp.ClientModel;
using System.Globalization;

namespace D1WebApp.BussinessLogicLayer.Controllers
{
#if !DEBUG
      [System.Web.Mvc.RequireHttps]
#endif


    public class CompanyProfileController : ApiController
    {

        public CompanyProfileManager CompanyProfilemanager = new CompanyProfileManager();
        public UserManager usermanager = new UserManager();

        public CompanyProfileController()
        {

        }

        [HttpPost]
        public dynamic GetActivitylogsearch(activitylogviewmodel activitylogview)
        {
            return CompanyProfilemanager.GetActivitylogsearch(activitylogview);
        }

        [HttpGet]
        public dynamic Setconfiguration()
        {

            return CompanyProfilemanager.AddConfiguations("Hello", "hello", "hello");
        }

        [HttpGet]
        public dynamic GetHeaderlinklist(string memRefNo)
        {
            return CompanyProfilemanager.GetHeaderlinklist(memRefNo);
        }
        [HttpGet]
        public dynamic GetHeaderlinkByID(string memRefNo, int linkid)
        {
            return CompanyProfilemanager.GetHeaderlinkByID(memRefNo, linkid);
        }
        [HttpGet]
        public dynamic DeleteHeaderlinkByID(string memRefNo, int linkid)
        {
            return CompanyProfilemanager.DeleteHeaderlinkByID(memRefNo, linkid);
        }
        [HttpGet]
        public dynamic DeletesafiltersortByID(string memRefNo, int sortid)
        {
            return CompanyProfilemanager.DeletesafiltersortByID(memRefNo, sortid);
        }
        [HttpPost]
        public dynamic UpdateHeaderlinkByID(headerlinkViewModel headerlinkView)
        {
            return CompanyProfilemanager.UpdateHeaderlinkByID(headerlinkView.memRefNo, headerlinkViewModel.ConvertToModel(headerlinkView));
        }

        [HttpGet]
        public dynamic GetConfigsList(string UserMemRefNo)
        {
            return CompanyProfilemanager.GetWebConfigsList(UserMemRefNo);
        }
        [HttpGet]
        public dynamic GetsafiltersortList(string UserMemRefNo)
        {
            return CompanyProfilemanager.GetsafiltersortList(UserMemRefNo);
        }
        [HttpGet]
        public dynamic ColorGetConfigsList(string UserMemRefNo)
        {
            return CompanyProfilemanager.GetWebColorConfigsList(UserMemRefNo);
        }

        [HttpGet]
        public dynamic GetMailTemplateList(string UserMemRefNo)
        {
            return CompanyProfilemanager.GetMailtemplateList(UserMemRefNo);
        }
        [HttpGet]
        public dynamic Getdynamicpagelist(string UserMemRefNo)
        {
            return CompanyProfilemanager.Getdynamicpagelist(UserMemRefNo);
        }

        [HttpGet]
        public dynamic Getproductlist(string UserMemRefNo,int pageno)
        {
            return CompanyProfilemanager.Getproductlist(UserMemRefNo, pageno);
        }
        [HttpGet]
        public dynamic GetFilteredproductlist(string UserMemRefNo, string filterQuery,int activeFlag, int pageno)
        {
            return CompanyProfilemanager.GetFilteredproductlist(UserMemRefNo, filterQuery, activeFlag, pageno);
        }
        [HttpPost]
        public dynamic GetManufracturerItemDocList(ManufracturerParamViewModel manufracturerParamViewModel)
        {
            return CompanyProfilemanager.GetManufracturerItemDocList(manufracturerParamViewModel);
        }

        [HttpPost]
        public dynamic GetUserCartItemDetails(CartReportParam cartReportParam)
        {
            return CompanyProfilemanager.GetUserCartItemDetails(cartReportParam);
        }

        [HttpPost]
        public dynamic GetFilteredManufracturerItemDocList(ManufracturerParamViewModel manufracturerParamViewModel)
        {
            return CompanyProfilemanager.GetFilteredManufracturerItemDocList(manufracturerParamViewModel);
        }

        [HttpGet]
        public dynamic UpdateWebConfigs(string memRefNo, int configid, string configkey, string configvalue)
        {
            return CompanyProfilemanager.UpdateWebConfigsList(memRefNo, configid, configkey, configvalue);
        }
        [HttpGet]
        public dynamic Updatesafilterorder(string memRefNo, int configid, string configkey, int configvalue, int sorder)
        {
            return CompanyProfilemanager.Updatesafilterorder(memRefNo, configid, configkey, configvalue,sorder);
        }
        [HttpPost]
        public dynamic UpdateColorConfigs(ColorConfigViewModel ColorConfigView)
        {
            return CompanyProfilemanager.UpdateColorConfigsList(ColorConfigView.memRefNo, ColorConfigView.configid, ColorConfigView.configkey, ColorConfigView.configvalue, ColorConfigView.OldValue);
        }
        [HttpPost]
        public dynamic UpdateMailTemplate(MailTemplateViewModelNew MailTemplateView)
        {
            return CompanyProfilemanager.UpdateMailtemplate(MailTemplateView);
        }
        [HttpPost, ValidateInput(false)]
        public dynamic Updatedynamicpage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                dynamicpageviewmodel MailTemplateView = new dynamicpageviewmodel();
                string getid = httpRequest.Form["PageID"];
                if (!getid.Equals("undefined") && !string.IsNullOrEmpty(getid) && !getid.Equals("null"))
                {
                    MailTemplateView.PageID = Convert.ToInt16(getid);
                }
                MailTemplateView.PageName = httpRequest.Form["PageName"];
                MailTemplateView.PageTitle = httpRequest.Form["PageTitle"];
                MailTemplateView.PageDescription = httpRequest.Form["PageDescription"];
                MailTemplateView.PageKeywords = httpRequest.Form["PageKeywords"];
                MailTemplateView.PageContent = httpRequest.Form["PageContent"];
                MailTemplateView.PageType = httpRequest.Form["PageType"];
                MailTemplateView.Sequence = Convert.ToInt16(httpRequest.Form["Sequence"]);
                MailTemplateView.IsActive =Convert.ToBoolean(httpRequest.Form["IsActive"]);
                string memRefNo = httpRequest.Form["memRefNo"];
                MailTemplateView.memRefNo = memRefNo;
                MailTemplateView.ImageUrl = httpRequest.Form["ImageUrl"];
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                       
                        string FileName = httpRequest.Form["FileName"];
                        int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 5 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirEcom");

                            string targetDirectory = targetURL.ConfigurationValue + memRefNo;
                            string UIpath = targetDirectory + "\\" + memRefNo + "Api";
                            UIpath = UIpath + "\\Content\\DynamicPages\\";

                            var getpath = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("domainpath");
                            string getimagepath = getpath.ConfigurationValue + "/" + memRefNo + "Api" + "/Content/DynamicPages/" + FileName.Replace(" ", "") + "?v=" + DateTime.Now.ToShortDateString();
                            //  where you want to attach your imageurl
                            //if needed write the code to update the table
                            // var filePath = HttpContext.Current.Server.MapPath(UIpath);
                            //Userimage myfolder name where i want to save my image
                            if (!string.IsNullOrEmpty(UIpath))
                            {
                                MailTemplateView.ImageUrl = getimagepath;
                            }
                            if (!Directory.Exists(UIpath))
                            {
                                Directory.CreateDirectory(UIpath);
                            }
                            postedFile.SaveAs(Path.Combine(UIpath, FileName.Replace(" ", "")));

                        }
                    }
                }
                return CompanyProfilemanager.Updatesynamicpage(MailTemplateView);
            }
            catch (Exception ex)
            {
                dict.Add("error", ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            
        }
        [HttpGet]
        public dynamic GetConfigByID(string memRefNo, int configid)
        {
            return CompanyProfilemanager.GetConfigByID(memRefNo, configid);
        }
        [HttpGet]
        public dynamic Deletedynamicpage(string memRefNo, int configid)
        {
            return CompanyProfilemanager.Deletedynamic(memRefNo, configid);
        }
        [HttpGet]
        public dynamic DeleteItemDocByID(string memRefNo, int itemDocId)
        {
            return CompanyProfilemanager.DeleteItemDocByID(memRefNo, itemDocId);
        }
        [HttpGet]
        public dynamic UpdateItemPrice(string memRefNo, string item,decimal price,bool isItemActive)
        {
            return CompanyProfilemanager.UpdateItemPrice(memRefNo,item, price, isItemActive);
        }
        [HttpPost]
        public dynamic UpdateBulkActiveInActive(UpdateActiveInActiveViewModel updateActiveInActiveViewModel)
        {
            return CompanyProfilemanager.UpdateBulkActiveInActive(updateActiveInActiveViewModel);
        }
       
        [HttpGet]
        public dynamic Getsafiltersort(string memRefNo, int configid)
        {
            return CompanyProfilemanager.Getsafiltersort(memRefNo, configid);
        }
        [HttpGet]
        public dynamic GetColorConfigByID(string memRefNo, int configid)
        {
            var getconfig =CompanyProfilemanager.GetColorConfigByID(memRefNo, configid);
            if (getconfig.ColorConfigKey.Equals("Logo") && (getconfig.ColorValue.Equals("path") || string.IsNullOrEmpty(getconfig.ColorValue)))
            {
                var getcon = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("domainpath");
                getconfig.ColorValue = getcon.ConfigurationValue + "/" + memRefNo + "/assets/images/logo.png";
            }
                return getconfig;
        }
        [HttpGet]
        public dynamic GetMailtemplateByID(string memRefNo, int configid)
        {
            return CompanyProfilemanager.GetMailtemplateByID(memRefNo, configid);
        }
        [HttpGet]
        public dynamic GetdynamicpageByID(string memRefNo, int pageid)
        {
            return CompanyProfilemanager.GetdynamicpageByID(memRefNo, pageid);
        }
        [HttpGet]
        public dynamic GetItemDocByID(string memRefNo, string item)
        {
            return CompanyProfilemanager.GetItemDocByID(memRefNo, item);
        }
     
        [HttpGet]
        public dynamic GetItemPriceByItem(string memRefNo, string item)
        {
            return CompanyProfilemanager.GetItemPriceByItem(memRefNo, item);
        }
        [HttpGet]
        public dynamic GetDocumentTypeConfigValue(string memRefNo)
        {
            return CompanyProfilemanager.GetDocumentTypeConfigValue(memRefNo);
        }
        [HttpPost]
        public dynamic UpdateItemPriceList()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                List<ItemPriceListModel> priceList = new List<ItemPriceListModel>();

                var memRefNo = httpRequest.Form["memRefNo"];

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string FileName = httpRequest.Form["FileName"];

                        IList<string> AllowedFileExtensions = new List<string> { ".csv"};
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload csv file");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            priceList = ReadCsvDataForItemPrice(postedFile.InputStream);
                        }
                    }
                }
                return CompanyProfilemanager.UpdateItemPriceBulk(memRefNo, priceList);
            }
            catch (Exception ex)
            {
                //var res = string.Format("some Message");
                dict.Add("error", ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
        private List<ItemPriceListModel> ReadCsvDataForItemPrice(Stream inputStream)
        {
            var records = new List<ItemPriceListModel>();

            using (var reader = new StreamReader(inputStream))
            {
                bool isFirstLine = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue; // Skip the header line
                    }

                    var values = line.Split(',');
                    if (values.Length >= 2)
                    {
                        var itemPriceListModel = new ItemPriceListModel
                        {
                            Item = values[0],
                            Price = (double)decimal.Parse(values[1])
                        };
                        records.Add(itemPriceListModel);
                    }
                }
            }
            return records;
        }

        [HttpPost]
        public dynamic UpdateItemDocumentList()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                List<ItemDetailsViewModel> documentList = new List<ItemDetailsViewModel>();

                var memRefNo = httpRequest.Form["memRefNo"];
                var IMType = Convert.ToBoolean(httpRequest.Form["IMType"]);
                
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string FileName = httpRequest.Form["FileName"];

                        IList<string> AllowedFileExtensions = new List<string> { ".csv" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload csv file");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            documentList = ReadCsvDataForItemDocument(postedFile.InputStream);
                        }
                    }
                }
                return CompanyProfilemanager.UpdateItemDocumentBulk (memRefNo, documentList, IMType);
            }
            catch (Exception ex)
            {
                //var res = string.Format("some Message");
                dict.Add("error", ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
        private List<ItemDetailsViewModel> ReadCsvDataForItemDocument(Stream inputStream)
        {
            var records = new List<ItemDetailsViewModel>();

            using (var reader = new StreamReader(inputStream))
            {
                bool isFirstLine = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue; // Skip the header line
                    }

                    var values = line.Split(',');
                    if (values.Length >= 2)
                    {
                        var ItemDocDetailsViewModel = new ItemDetailsViewModel
                        {
                            Item = values[0],
                            DocType = values[1],
                            DocName = values[2],
                            DocDetailsUrl = values[3],
                            Sequence = Convert.ToInt32(values[4]),
                        };
                        records.Add(ItemDocDetailsViewModel);
                    }
                }
            }
            return records;
        }
        [HttpPost]
        public dynamic UpdateItemDocument()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                 
                ItemDetailsViewModel ItemDetailsViewModel = new ItemDetailsViewModel();

                string getid = httpRequest.Form["ItemDocId"];
                if (!string.IsNullOrEmpty(getid) && !getid.Equals("null"))
                {
                    ItemDetailsViewModel.ItemDocId = Convert.ToInt32(getid);
                }
                ItemDetailsViewModel.DocType = httpRequest.Form["DocType"];
                ItemDetailsViewModel.DocName = httpRequest.Form["DocName"];
                ItemDetailsViewModel.Item = httpRequest.Form["Item"];
                ItemDetailsViewModel.memRefNo = httpRequest.Form["memRefNo"];
                ItemDetailsViewModel.Sequence = Convert.ToInt16(httpRequest.Form["Sequence"]);
                ItemDetailsViewModel.IMType = Convert.ToBoolean(httpRequest.Form["IMType"]);
                if (ItemDetailsViewModel.ItemDocId > 0 && (ItemDetailsViewModel.DocType == "doc" || ItemDetailsViewModel.DocType == "image"))
                {
                    ItemDetailsViewModel.DocDetailsUrl = httpRequest.Form["DocDetailsUrl"];
                }
                if (ItemDetailsViewModel.DocType == "text" || ItemDetailsViewModel.DocType == "video")
                {
                    ItemDetailsViewModel.DocDetailsUrl = httpRequest.Form["DocDetailsUrl"];
                }
                else { 
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            string FileName = httpRequest.Form["FileName"];
                            int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB

                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png",".pdf", ".webp", ".stp" };
                            var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                var message = string.Format("Allowed file types are .jpg,.gif,.png.,.pdf,.stp");

                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            }
                            else if (postedFile.ContentLength > MaxContentLength)
                            {
                                var message = string.Format("Please Upload a file upto 5 mb.");

                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            }
                            else
                            {
                                var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirEcom");

                                string targetDirectory = targetURL.ConfigurationValue + ItemDetailsViewModel.memRefNo;
                                string UIpath = targetDirectory + "\\" + ItemDetailsViewModel.memRefNo + "Api";
                                UIpath = UIpath + "\\Content\\ItemDetails\\";

                                var getpath = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("domainpath");
                                string getimagepath = getpath.ConfigurationValue + "/" + ItemDetailsViewModel.memRefNo + "Api" + "/Content/ItemDetails/" + FileName.Replace(" ", "") + "?v=" + DateTime.Now.ToShortDateString();
                                //  where you want to attach your imageurl
                                //if needed write the code to update the table
                                // var filePath = HttpContext.Current.Server.MapPath(UIpath);
                                //Userimage myfolder name where i want to save my image
                                if (!string.IsNullOrEmpty(UIpath))
                                {
                                    ItemDetailsViewModel.DocDetailsUrl = getimagepath;
                                }
                                if (!Directory.Exists(UIpath))
                                {
                                    Directory.CreateDirectory(UIpath);
                                }
                                postedFile.SaveAs(Path.Combine(UIpath, FileName.Replace(" ", "")));
                            }
                        }
                    }
                }
               return CompanyProfilemanager.AddUpdateItemDocument(ItemDetailsViewModel);
            }
            catch (Exception ex)
            {
                //var res = string.Format("some Message");
                dict.Add("error", ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }
    }
}
