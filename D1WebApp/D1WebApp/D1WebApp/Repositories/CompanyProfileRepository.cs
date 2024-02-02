//Developed by Nayan

using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Linq;
using System.Web;

using D1WebApp.Utilities;

using D1WebApp.Utilities.GeneralConfiguration;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using D1WebApp.ClientModel;
using RestSharp;
using Newtonsoft.Json;
using D1WebApp.Models;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Security.Policy;
using System.Web.Query.Dynamic;
using System.Linq.Expressions;
using Antlr.Runtime.Misc;
using System.Web.Http.Results;
using System.Xml.Linq;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class CompanyProfileRepository : ICompanyProfileRepository, IDisposable
    {
        public string Errorlog = HttpContext.Current.Server.MapPath("~/SQL").ToString() + "\\Errorlog_" + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";

        public CompanyProfileRepository()
        {

        }

        public dynamic GetsafiltersortList(string memRefNo)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.safilterorders.ToList();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetsafiltersortList", DateTime.Now, "GetsafiltersortList", ed.ToString(), "GetsafiltersortList", 2);
                return ed.ToString();
            }
        }

        public dynamic GetWebConfigsList(string memRefNo)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.Configs.ToList();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetWebConfigsListrepo", DateTime.Now, "GetWebConfigsListrepo", ed.ToString(), "GetWebConfigsListrepo", 2);
                return ed.ToString();
            }

        }
        public dynamic GetWebColorConfigsList(string memRefNo)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.ColorConfigurations.ToList();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetWebColorConfigsListrepo", DateTime.Now, "GetWebColorConfigsListrepo", ed.ToString(), "GetWebColorConfigsListrepo", 2);
                return ed.ToString();
            }

        }

        public dynamic GetMailTemplateList(string memRefNo)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.EMailTemplates.ToList();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetMailTemplateListrepo", DateTime.Now, "GetMailTemplateListrepo", ed.ToString(), "GetMailTemplateListrepo", 2);
                return ed.InnerException.ToString();
            }

        }
        public dynamic Getdynamicpagelist(string memRefNo)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                string companyDomain = context.Configs.Where(i => i.ConfigKey == "CompanyDomain").Select(i => i.ConfigValue).FirstOrDefault();
                //return context.Configs.ToList();

               
                var orderedDynamicPages = context.DynamicPages
                .OrderByDescending(page => page.PageID)
                .ToList();

                return orderedDynamicPages;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetMailTemplateListrepo", DateTime.Now, "GetMailTemplateListrepo", ed.ToString(), "GetMailTemplateListrepo", 2);
                return ed.InnerException.ToString();
            }

        }
        public dynamic Getproductlist(string memRefNo, int pageno)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (pageno == 0)
                {
                    pageno = 1;
                }
                int counts = (context.items.ToList().Count());
                var GetItemList = (from itm in context.items
                                  select new ItemListModel
                                  {
                                      TotalPage = counts,
                                      item1 = itm.item1,
                                      discontinued = itm.discontinued
                                  }).OrderByDescending(c => c.item1).Skip((pageno - 1) * 50).Take(50).ToList();
                return GetItemList;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetProductListrepo", DateTime.Now, "GetProductListrepo", ed.ToString(), "GetProductListrepo", 2);
                return ed.InnerException.ToString();
            }

        }
        public dynamic GetFilteredproductlist(string memRefNo, string filterQuery, int activeFlag, int pageno)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (pageno == 0)
                {
                    pageno = 1;
                }

                IQueryable<item> query = (IQueryable<item>)context.items;
                if (filterQuery != "undefined" && !string.IsNullOrEmpty(filterQuery))
                {
                    query = query.Where(c => c.item1.Contains(filterQuery));
                }

                if (activeFlag != -1)
                {
                    bool actFlag = Convert.ToBoolean(activeFlag);
                    query = query.Where(c => c.discontinued == actFlag);
                }

                int counts = query.ToList().Count();

                var GetItemList = query
                    .Select(itm => new ItemListModel
                    {
                        TotalPage = counts,
                        item1 = itm.item1,
                        discontinued = itm.discontinued
                    }).OrderByDescending(c => c.item1)
                    .Skip((pageno - 1) * 50)
                    .Take(50)
                    .ToList(); ;

                return GetItemList;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetProductListrepo", DateTime.Now, "GetProductListrepo", ed.ToString(), "GetProductListrepo", 2);
                return ed.InnerException.ToString();
            }
        }

        public void writeanylog(string filepath, string logmsg)
        {
            try
            {
                File.AppendAllText(filepath, logmsg);
            }
            catch (Exception ed)
            {

            }
        }
        public dynamic GetUserCartItemDetails(CartReportParam cartReportParam)
        {
            try 
            {
                writeanylog(Errorlog, "GetUserCartItemDetails => cartReportParam" + "-" + cartReportParam + "\n");
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(cartReportParam.memRefNo));
                writeanylog(Errorlog, "GetUserCartItemDetails => db connection done" + "\n");
                List<CartDetailsViewModel> GetProductDetails = new List<CartDetailsViewModel>();

                GetProductDetails = (from its in context.GetUserCartItemDetailReport(cartReportParam.ItemName, cartReportParam.UserId, cartReportParam.Customer, cartReportParam.FromDate,cartReportParam.ToDate)
                                     select new CartDetailsViewModel
                                     {
                                         Item = its.Item,
                                         Customer = its.Customer,
                                         UserName = its.UserName,
                                         FirstName = its.FirstName, 
                                         LastName = its.LastName,
                                         Quantity = its.Quantity,
                                         Price = its.Price,
                                         Date = its.DayDate
                                     }).ToList();

                return GetProductDetails;
            }
            catch (Exception ed)
            {
                writeanylog(Errorlog, "GetUserCartItemDetails" + "-" + ed.InnerException.ToString() + "\n");
                return ed.InnerException.ToString();
            }
        }
        public dynamic GetManufracturerItemDocList(ManufracturerParamViewModel manufracturerParamViewModel)
        {
            try
            {
                writeanylog(Errorlog, "GetItemDetailsListCounts" + "-" + "memRefNo >>>>>" + manufracturerParamViewModel.memRefNo + "\n");
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(manufracturerParamViewModel.memRefNo));
                writeanylog(Errorlog, "GetItemDetailsListCounts" + "-" + "db connection done" + "\n");
                if (manufracturerParamViewModel.pageno == 0)
                {
                    manufracturerParamViewModel.pageno = 1;
                }
                int counts = (context.itemdetails.Where(p => p.IMType == true).ToList().Count());

                writeanylog(Errorlog, "GetItemDetailsListCounts" + "-" + "got counts >>>>>" + counts.ToString() + "\n");

                var GetItemList = (from itm in context.itemdetails
                                   select new ManufItemDetailsViewModel
                                   {
                                       TotalPage = counts,
                                       ItemDocId = itm.id,
                                       Item = itm.item,
                                       DocType = itm.type,
                                       DocName = itm.name,
                                       DocDetailsUrl = itm.details_or_url,
                                       Sequence = itm.sequence,
                                       IMType = itm.IMType
                                   }).Where(p => p.IMType == true).OrderByDescending(c => c.ItemDocId).Skip((manufracturerParamViewModel.pageno - 1) * 50).Take(50).ToList();
                writeanylog(Errorlog, "GetItemDetailsListCounts" + "-" + "got Item Doc List" + "\n");
                return GetItemList;
            }
            catch (Exception ed)
            {
                writeanylog(Errorlog, "GetItemDetailsListCounts" + "-" + ed.InnerException.ToString() + "\n");
                return ed.InnerException.ToString();
            }

        }

        public dynamic GetFilteredManufracturerItemDocList(ManufracturerParamViewModel manufracturerParamViewModel)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(manufracturerParamViewModel.memRefNo));

                if (manufracturerParamViewModel.pageno == 0)
                {
                    manufracturerParamViewModel.pageno = 1;
                }

                IQueryable<itemdetail> query = (IQueryable<itemdetail>)context.itemdetails.Where(c => c.IMType == true);

                if (manufracturerParamViewModel.filterQuery != "undefined" && !string.IsNullOrEmpty(manufracturerParamViewModel.filterQuery))
                {
                    query = query.Where(c => c.item.Contains(manufracturerParamViewModel.filterQuery));
                }

                int counts = query.ToList().Count();

                var GetItemList = query
                    .Select(itm => new ManufItemDetailsViewModel
                    {
                        TotalPage = counts,
                        ItemDocId = itm.id,
                        Item= itm.item,
                        DocType = itm.type,
                        DocName = itm.name,
                        DocDetailsUrl = itm.details_or_url,
                        Sequence = itm.sequence,
                        IMType = itm.IMType

                    }).Where(p => p.IMType == true).OrderByDescending(c => c.ItemDocId)
                    .Skip((manufracturerParamViewModel.pageno - 1) * 50)
                    .Take(50)
                    .ToList(); ;

                return GetItemList;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetFilteredManufracturerItemDocList", DateTime.Now, "GetFilteredManufracturerItemDocList", ed.ToString(), "GetFilteredManufracturerItemDocList", 2);
                return ed.InnerException.ToString();
            }
        }

        public dynamic GetHeaderlinklist(string memRefNo)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.headerlinks.ToList();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetHeaderlinklistrepo", DateTime.Now, "GetHeaderlinklistrepo", ed.ToString(), "GetHeaderlinklistrepo", 2);
                return ed.InnerException.ToString();
            }

        }

        public dynamic GetHeaderlinkByID(string memRefNo, int linkid)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.headerlinks.Where(cp => cp.linkid == linkid).First();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetHeaderlinkByIDrepo", DateTime.Now, "GetHeaderlinkByIDrepo", ed.ToString(), "GetHeaderlinkByIDrepo", 2);
                return ed.InnerException.ToString();
            }
        }

        public dynamic DeleteHeaderlinkByID(string memRefNo, int linkid)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                var getlink = context.headerlinks.Where(cp => cp.linkid == linkid).First();
                context.headerlinks.Remove(getlink);
                context.SaveChanges();
                return true;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "DeleteHeaderlinkByIDrepo", DateTime.Now, "DeleteHeaderlinkByIDrepo", ed.ToString(), "DeleteHeaderlinkByIDrepo", 2);
                return ed.InnerException.ToString();
            }
        }
        public dynamic DeletesafiltersortByID(string memRefNo, int linkid)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                var getlink = context.safilterorders.Where(cp => cp.id == linkid).First();
                context.safilterorders.Remove(getlink);
                context.SaveChanges();
                return true;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "DeletesafiltersortByID", DateTime.Now, "DeletesafiltersortByID", ed.ToString(), "DeletesafiltersortByID", 2);
                return ed.InnerException.ToString();
            }
        }
        public dynamic UpdateHeaderlinkByID(string memRefNo, headerlink hlink)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (hlink.linkid != null && hlink.linkid > 0)
                {
                    var getlink = context.headerlinks.Where(cp => cp.linkid == hlink.linkid).First();
                    context.Entry(getlink).CurrentValues.SetValues(hlink);
                }
                else
                {
                    context.headerlinks.Add(hlink);
                }
                context.SaveChanges();
                return true;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateHeaderlinkByIDrepo", DateTime.Now, "UpdateHeaderlinkByIDrepo", ed.ToString(), "UpdateHeaderlinkByIDrepo", 2);
                return ed.InnerException.ToString();
            }
        }

        public dynamic GetConfigByID(string memRefNo, int configid)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.Configs.Where(cp => cp.ConfigId == configid).First();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetConfigByIDrepo", DateTime.Now, "GetConfigByIDrepo", ed.ToString(), "GetConfigByIDrepo", 2);
                return ed.InnerException.ToString();
            }
        }
        public dynamic Getsafiltersort(string memRefNo, int configid)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.safilterorders.Where(cp => cp.id== configid).First();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Getsafiltersort", DateTime.Now, "Getsafiltersort", ed.ToString(), "Getsafiltersort", 2);
                return ed.InnerException.ToString();
            }
        }
        public dynamic GetColorConfigByID(string memRefNo, int configid)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.ColorConfigurations.Where(cp => cp.ColorConfigID == configid).First();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetColorConfigByIDrepo", DateTime.Now, "GetColorConfigByIDrepo", ed.ToString(), "GetColorConfigByIDrepo", 2);
                return ed.InnerException.ToString();
            }
        }

        public dynamic GetMailTemplateByID(string memRefNo, int MailTemplateID)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.EMailTemplates.Where(cp => cp.MailTemplateID == MailTemplateID).First();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetMailTemplateByIDrepo", DateTime.Now, "GetMailTemplateByIDrepo", ed.ToString(), "GetMailTemplateByIDrepo", 2);
                return ed.InnerException.ToString();
            }
        }
        public dynamic GetdynamicpageByID(string memRefNo, int pageID)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                return context.DynamicPages.Where(cp => cp.PageID == pageID).First();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetdynamicpageByID", DateTime.Now, "GetdynamicpageByID", ed.ToString(), "GetdynamicpageByID", 2);
                return ed.InnerException.ToString();
            }
        }
           
        public dynamic GetItemDocByID(string memRefNo, string item)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                var q = (from itm in context.items
                         join itmdtls in context.itemdetails on itm.item1 equals itmdtls.item
                         orderby itmdtls.id descending
                         where itmdtls.item == item
                         select new
                         {
                             Id = itmdtls.id,
                             Item = itmdtls.item,
                             DocType = itmdtls.type,
                             DocTypeName = itmdtls.name,
                             DocTypeTextUrl = itmdtls.details_or_url,
                             ItemIsActive = itm.discontinued,
                             Sequence = itmdtls.sequence,
                         }).ToList();
                return q;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetItemDocByID", DateTime.Now, "GetItemDocByID", ed.ToString(), "GetItemDocByID", 2);
                return ed.InnerException.ToString();
            }
        }
        
        public dynamic GetItemPriceByItem(string memRefNo, string item)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                var q = (from itm in context.items
                         join waitem in context.wa_item on itm.item1 equals waitem.wa_item_item
                         //orderby waitem.wa_item_item
                         where itm.item1 == item
                         select new
                         {
                             Item = waitem.wa_item_item,
                             ItemPrice = (decimal)waitem.wa_item_list_price,
                             ItemIsActive = itm.discontinued == true ? false : true,
                         }).FirstOrDefault();
                return q;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetItemPriceByItem", DateTime.Now, "GetItemPriceByItem", ed.ToString(), "GetItemPriceByItem", 2);
                return ed.InnerException.ToString();
            }
        }
        
        public dynamic DeleteItemDocByID(string memRefNo, int itemDocId)
        {
            bool flag = false;
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                var getold = context.itemdetails.Where(cp => cp.id == itemDocId).FirstOrDefault();
                if (getold != null)
                {
                    context.itemdetails.Remove(getold);
                    context.SaveChanges();
                }
                flag = true;
            }
            catch (Exception de)
            {
                flag = false;
            }
            return flag;
        }
        public dynamic UpdateItemPrice(string memRefNo, string item,decimal price,bool isItemActive) {
            bool flag = false;
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (!string.IsNullOrEmpty(item))
                {
                    var itemList = context.wa_item.Where(cp => cp.wa_item_item == item).ToList();
                    itemList.ForEach(c => c.wa_item_list_price = price);
                    context.SaveChanges();
                    flag = true;

                    var itemdetail = context.items.Where(cp => cp.item1 == item).FirstOrDefault();
                    if (itemdetail != null) {
                        itemdetail.discontinued = isItemActive == true ? false : true;
                    }
                    context.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception de)
            {
                flag = false;
            }
            return flag;
        }
        public dynamic UpdateBulkActiveInActive(UpdateActiveInActiveViewModel updateActiveInActiveViewModel)
        {
            bool flag = false;
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(updateActiveInActiveViewModel.memRefNo));
                if (updateActiveInActiveViewModel.items != null && updateActiveInActiveViewModel.items.Count > 0)
                {
                    var itemdetailsList = context.items.Where(cp => updateActiveInActiveViewModel.items.Contains(cp.item1)).ToList();
                    if (itemdetailsList != null && itemdetailsList.Count > 0)
                    {
                        itemdetailsList.ForEach(a => a.discontinued = updateActiveInActiveViewModel.isItemActive);
                    }
                    context.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        public dynamic UpdateWebConfigsList(string memRefNo, int configid, string configkey, string configvalue)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (configid > 0)
                {
                    var getold = context.Configs.Where(cp => cp.ConfigId == configid).FirstOrDefault();
                    var getNew = context.Configs.Where(cp => cp.ConfigId == configid).FirstOrDefault();
                    //getNew.ConfigKey = configkey;
                    getNew.ConfigValue = configvalue;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    var getold = context.Configs.Max(cp => cp.ConfigId);
                    Config f12 = new Config();
                    f12.ConfigId = getold + 1;
                    f12.ConfigKey = configkey;
                    f12.ConfigValue = configvalue;
                    context.Configs.Add(f12);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateWebConfigsList", DateTime.Now, "UpdateWebConfigsList", ed.ToString(), "UpdateWebConfigsList", 2);
                return ed.InnerException.ToString();
            }

        }

        public dynamic Updatesafilterorder(string memRefNo, int configid, string configkey, int configvalue,int sorder)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (configid > 0)
                {
                    //var getold = context.safilterorders.Where(cp => cp.id == configid).FirstOrDefault();
                    var getNew = context.safilterorders.Where(cp => cp.id == configid).FirstOrDefault();
                    //getNew.ConfigKey = configkey;
                    getNew.Name = configkey;
                    getNew.stype = configvalue;
                    getNew.sorder = sorder;
                    context.safilterorders.Attach(getNew);
                    context.Entry(getNew).State = EntityState.Modified;
                    //context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {

                    safilterorder f12 = new safilterorder();
                    f12.Name = configkey;
                    f12.stype = configvalue;
                    f12.sorder = sorder;
                    context.safilterorders.Add(f12);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Updatesafilterorder", DateTime.Now, "Updatesafilterorder", ed.ToString(), "Updatesafilterorder", 2);
                return ed.InnerException.ToString();
            }

        }

        public dynamic GetBannerDetailsByID(string memRefNo, int bannerID)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                var getdetails = context.tblBanners.Find(bannerID);
                return ClientBannerViewModel.ConvertToViewModel(getdetails);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateWebConfigsList", DateTime.Now, "UpdateWebConfigsList", ed.ToString(), "UpdateWebConfigsList", 2);
                return ed.InnerException.ToString();
            }

        }

        public dynamic GetBannerListforClient(string memRefNo)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                var getbannerlist = context.tblBanners.ToList();
                if (getbannerlist != null && getbannerlist.Count() > 0)
                {
                    return ClientBannerViewModel.ConvertToViewModelList(getbannerlist);
                }
                else
                {
                    return new List<ClientBannerViewModel>();
                }

            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetBannerListforClient", DateTime.Now, "GetBannerListforClient", ed.ToString(), "GetBannerListforClient", 2);
                return ed.InnerException.ToString();
            }
        }


        public dynamic GetActivitylogsearch(activitylogviewmodel activitylogview)
        {
            List<activitylogviewmodel> activityloglist = new List<activitylogviewmodel>();
            try {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(activitylogview.memRefNo));
                activityloglist = (from act in context.GetActivityLog(activitylogview.LogType, activitylogview.SearchKeyword, activitylogview.CustID, activitylogview.UserId, activitylogview.ClientIP, activitylogview.FromDate, activitylogview.ToDate).ToList()
                                    select new activitylogviewmodel
                                    {
                                        ActivityLogId = act.ActivityLogId,
                                        ClientIP = act.ClientIP,
                                        CustID = act.CustID,
                                        Description = act.Description,
                                        LogDate = act.LogDate,
                                        LogType = act.LogType,
                                        SearchKeyword = act.SearchKeyword,
                                        UserId = act.UserId
                                    }).ToList();
                return activityloglist;
            }
            catch(Exception ed)
            {
                return ed.ToString();
            }

        }

        public dynamic AddAndUpdateBanners(ClientBannerViewModel ClientBannerView)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(ClientBannerView.memRefNo));
                
                if (ClientBannerView.BannerId > 0)
                {
                    var getold = context.tblBanners.Where(cp => cp.BannerId == ClientBannerView.BannerId).FirstOrDefault();
                    var getNew = context.tblBanners.Where(cp => cp.BannerId == ClientBannerView.BannerId).FirstOrDefault();
                    //getNew.ConfigKey = configkey;
                    getNew.Description = ClientBannerView.Description;
                    getNew.ImageUrl = ClientBannerView.ImageUrl;
                    getNew.IsActive = ClientBannerView.IsActive;
                    getNew.Title = ClientBannerView.Title;
                    getNew.targeturl = ClientBannerView.targeturl;
                    getNew.types = ClientBannerView.types;
                    getNew.linkname = ClientBannerView.linkname;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {

                    tblBanner f12 = new tblBanner();
                    f12.Description = ClientBannerView.Description;
                    f12.ImageUrl = ClientBannerView.ImageUrl;
                    f12.IsActive = ClientBannerView.IsActive;
                    f12.Title = ClientBannerView.Title;
                    f12.types = ClientBannerView.types;
                    f12.linkname = ClientBannerView.linkname;
                    f12.targeturl = ClientBannerView.targeturl;
                    context.tblBanners.Add(f12);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "AddAndUpdateBanners", DateTime.Now, "AddAndUpdateBanners", ed.ToString(), "AddAndUpdateBanners", 2);
                return ed.InnerException.ToString();
            }

        }
        public dynamic AddUpdateItemDocument(ItemDetailsViewModel ItemDetailsViewModel)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(ItemDetailsViewModel.memRefNo));
                int sequence = 0;
                if (ItemDetailsViewModel.DocType == "text" || ItemDetailsViewModel.DocType == "video")
                {
                    string documentTypes = context.Configs.Where(p => p.ConfigKey == "DocumentSequence").Select(p => p.ConfigValue).FirstOrDefault();
                    List<KeyValuePair<string, string>> documentTypeList = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(documentTypes);
                    // Find the entry with the specified docType
                    var docTypeEntry = documentTypeList.FirstOrDefault(entry => entry.Key == ItemDetailsViewModel.DocType);
                    if (!string.IsNullOrEmpty(docTypeEntry.Value))
                    {
                        sequence = int.Parse(docTypeEntry.Value);
                    }
                }
                else
                {
                    sequence = (int)ItemDetailsViewModel.Sequence;
                }

                if (ItemDetailsViewModel.ItemDocId > 0)
                {
                    var getold = context.itemdetails.Where(cp => cp.id == ItemDetailsViewModel.ItemDocId).FirstOrDefault();
                    var getNew = context.itemdetails.Where(cp => cp.id == ItemDetailsViewModel.ItemDocId).FirstOrDefault();

                    getNew.item = ItemDetailsViewModel.Item;
                    getNew.type = ItemDetailsViewModel.DocType;
                    getNew.name = ItemDetailsViewModel.DocName;
                    getNew.details_or_url = ItemDetailsViewModel.DocDetailsUrl;
                    getNew.sequence = sequence;
                    getNew.IMType = ItemDetailsViewModel.IMType;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    itemdetail f12 = new itemdetail();
                    f12.item = ItemDetailsViewModel.Item;
                    f12.type = ItemDetailsViewModel.DocType;
                    f12.name = ItemDetailsViewModel.DocName;
                    f12.details_or_url = ItemDetailsViewModel.DocDetailsUrl;
                    f12.sequence = sequence;
                    f12.IMType = ItemDetailsViewModel.IMType;
                    context.itemdetails.Add(f12);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "AddUpdateItemDocument", DateTime.Now, "AddUpdateItemDocument", ed.ToString(), "AddUpdateItemDocument", 2);
                return ed.InnerException.ToString();
            }

        }
        public dynamic UpdateItemPriceBulk(string memRefNo, List<ItemPriceListModel> itemDetailsViewModel)
        {
            try 
            { 
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if(itemDetailsViewModel.Count > 0)
                {
                    foreach (var record in itemDetailsViewModel)
                    {
                        var existingItems = context.wa_item.Where(item => item.wa_item_item == record.Item).ToList();
                        if (existingItems != null && existingItems.Count > 0)
                        {
                            foreach (var existingItem in existingItems)
                            {
                                existingItem.wa_item_list_price = (decimal?)record.Price;
                            }
                        }
                    }
                    context.SaveChanges();
                    return true;
                }
                else 
                {
                    ErrorLogs.ErrorLog(0, "UpdateItemPriceBulk", DateTime.Now, "UpdateItemPriceBulk","File has no data", "UpdateItemPriceBulk", 2);
                    return false;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateItemPriceBulk", DateTime.Now, "UpdateItemPriceBulk", ed.ToString(), "UpdateItemPriceBulk", 2);
                return ed.InnerException.ToString();
            }
        }
        /// <summary>
        /// check if name of the document is valid or not in bulk upload csv file
        /// </summary>
        /// <param name="memRefNo"></param>
        /// <param name="docName"></param>
        /// <returns></returns>
        public bool CheckIfDocNameIsValid(string memRefNo, string docName)
        {
            bool IsValid = false;
            var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
            string documentTypes = context.Configs.Where(p => p.ConfigKey == "DocumentType").Select(p => p.ConfigValue).FirstOrDefault();
            List<KeyValuePair<string, string>> documentTypeList = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(documentTypes);
            IsValid = documentTypeList.Any(entry => entry.Key == docName);
            return IsValid;
        }
        public int GetSequenceForDocType(string memRefNo, string docType) //Pass value like TDS,Litrature, 2D Drawing etc
        {
            var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
            int sequence = 0;
            string documentTypes = context.Configs.Where(p => p.ConfigKey == "DocumentType").Select(p => p.ConfigValue).FirstOrDefault();

            List<KeyValuePair<string, string>> documentTypeList = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(documentTypes);

            // Find the entry with the specified docType
            var docTypeEntry = documentTypeList.FirstOrDefault(entry => entry.Key == docType);

            if (!string.IsNullOrEmpty(docTypeEntry.Value))
            {
                sequence = int.Parse(docTypeEntry.Value);
            }
            return sequence;
        }
        public dynamic GetDocumentTypeConfigValue(string memRefNo)
        {
            string documentTypes = "";
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                documentTypes = context.Configs.Where(p => p.ConfigKey == "DocumentType").Select(p => p.ConfigValue).FirstOrDefault();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetDocumentTypeConfigValue", DateTime.Now, "GetDocumentTypeConfigValue", ed.ToString(), "GetDocumentTypeConfigValue", 2);
                return ed.InnerException.ToString();
            }
            return documentTypes;
        }
        public dynamic UpdateItemDocumentBulk(string memRefNo, List<ItemDetailsViewModel> itemDocumentViewModel,bool IMType)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (itemDocumentViewModel.Count > 0)
                {
                    foreach (var record in itemDocumentViewModel)
                    {
                        int sequence = 0;
                        if (record.DocType.ToLower() == "doc")
                        {
                            bool isValidDocName = CheckIfDocNameIsValid(memRefNo, record.DocName);
                            if (!isValidDocName)
                            {
                                continue;
                            }
                            sequence = GetSequenceForDocType(memRefNo, record.DocName);
                        }
                        else if (record.DocType.ToLower() == "text" || record.DocType.ToLower() == "video")
                        {
                                string documentTypes = context.Configs.Where(p => p.ConfigKey == "DocumentSequence").Select(p => p.ConfigValue).FirstOrDefault();
                                List<KeyValuePair<string, string>> documentTypeList = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(documentTypes);
                                // Find the entry with the specified docType
                                var docTypeEntry = documentTypeList.FirstOrDefault(entry => entry.Key == record.DocType);
                                if (!string.IsNullOrEmpty(docTypeEntry.Value))
                                {
                                    sequence = int.Parse(docTypeEntry.Value);
                                }
                        }
                        else
                        {
                            sequence = Convert.ToInt32(record.Sequence);
                        }
                        var existingItems = context.itemdetails.Where(item => item.item == record.Item && item.type == record.DocType && item.name == record.DocName && item.IMType == IMType).ToList();
                        if (existingItems != null && existingItems.Count > 0)
                        {
                            foreach (var existingItem in existingItems)
                            {
                                existingItem.details_or_url = record.DocDetailsUrl;
                                existingItem.sequence = sequence;
                            }
                            context.SaveChanges();
                        }
                        else
                        {
                            itemdetail f12 = new itemdetail();
                            f12.item = record.Item; ;
                            f12.type= record.DocType; 
                            f12.name = record.DocName; 
                            f12.details_or_url = record.DocDetailsUrl;
                            f12.IMType = IMType;
                            f12.sequence = sequence;
                            context.itemdetails.Add(f12);
                            context.SaveChanges();
                        }
                    }
                    return true;
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "UpdateItemDocumentBulk", DateTime.Now, "UpdateItemDocumentBulk", "File has no data", "UpdateItemDocumentBulk", 2);
                    return false;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateItemDocumentBulk", DateTime.Now, "UpdateItemDocumentBulk", ed.ToString(), "UpdateItemDocumentBulk", 2);
                return ed.InnerException.ToString();
            }
        }
        
        public dynamic UpdateColorConfigsList(string memRefNo, int configid, string configkey, string configvalue, string OldValue)
        {
            try
            {
                if (!configkey.Equals("Logo"))
                {
                    try
                    {
                        var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirEcom");
                        string targetDirectory = targetURL.ConfigurationValue + memRefNo.ToString();
                        string UIpath = targetDirectory + "\\" + memRefNo.ToString() + "UI" + "\\assets\\css";
                        string text1 = File.ReadAllText(UIpath + "\\main.css");
                        var AdminURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("UserApiPath");
                        text1 = text1.Replace(OldValue.ToLower(), configvalue.ToLower()).Replace(OldValue.ToUpper(), configvalue.ToUpper());
                        File.WriteAllText(UIpath + "\\main.css", text1);
                        string text2 = File.ReadAllText(UIpath + "\\blue.css");
                        text2 = text2.Replace(OldValue.ToLower(), configvalue.ToLower()).Replace(OldValue.ToUpper(), configvalue.ToUpper());
                        File.WriteAllText(UIpath + "\\blue.css", text2);
                    }
                    catch (Exception ed)
                    {
                        ErrorLogs.ErrorLog(0, "UpdateColorConfigsListrepo", DateTime.Now, "UpdateColorConfigsListrepo", ed.ToString(), "UpdateColorConfigsListrepo", 2);
                    }
                }
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (configid > 0)
                {
                    var getold = context.ColorConfigurations.Where(cp => cp.ColorConfigID == configid).FirstOrDefault();
                    var getNew = context.ColorConfigurations.Where(cp => cp.ColorConfigID == configid).FirstOrDefault();
                    getNew.ColorConfigKey = configkey;
                    getNew.ColorValue = configvalue;
                    getNew.Description = configkey;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {

                    ColorConfiguration f12 = new ColorConfiguration();
                    f12.ColorConfigKey = configkey;
                    f12.ColorValue = configvalue;
                    f12.Description = configkey;
                    context.ColorConfigurations.Add(f12);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateColorConfigsListrepo", DateTime.Now, "UpdateColorConfigsListrepo", ed.ToString(), "UpdateColorConfigsListrepo", 2);
                return ed.InnerException.ToString();
            }

        }

        public dynamic UpdateMailTemplate(MailTemplateViewModelNew MailTemplateView)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(MailTemplateView.memRefNo));
                if (MailTemplateView.MailTemplateID > 0)
                {
                    var getold = context.EMailTemplates.Where(cp => cp.MailTemplateID == MailTemplateView.MailTemplateID).FirstOrDefault();
                    var getNew = context.EMailTemplates.Where(cp => cp.MailTemplateID == MailTemplateView.MailTemplateID).FirstOrDefault();
                    getNew.MailTemplateContent = MailTemplateView.MailTemplateContent;
                    getNew.MailTemplateSubject = MailTemplateView.MailTemplateSubject;
                    getNew.IsActive = MailTemplateView.IsActive;
                    getNew.MailType = MailTemplateView.MailType;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    var getold = context.EMailTemplates.Max(cp => cp.MailTemplateID);
                    EMailTemplate f12 = new EMailTemplate();
                    f12.MailTemplateID = getold + 1;
                    f12.MailTemplateContent = MailTemplateView.MailTemplateContent;
                    f12.MailTemplateName = MailTemplateView.MailTemplateName;
                    f12.MailTemplateSubject = MailTemplateView.MailTemplateSubject;
                    f12.IsActive = MailTemplateView.IsActive;
                    f12.MailType = MailTemplateView.MailType;
                    context.EMailTemplates.Add(f12);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateMailTemplaterepo", DateTime.Now, "UpdateMailTemplaterepo", ed.ToString(), "UpdateMailTemplaterepo", 2);
                return ed.InnerException.ToString();
            }

        }

        public dynamic Deletedynamicpage(string memRefNo, int pageID)
        {
            bool flag = false;
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                var getold = context.DynamicPages.Where(cp => cp.PageID == pageID).FirstOrDefault();
                if(getold!=null)
                {
                    context.DynamicPages.Remove(getold);
                    context.SaveChanges();
                }
                flag = true;
            }
            catch(Exception de)
            {
                flag = false;
            }
            return flag;
        }
        public dynamic Updatesynamicpage(dynamicpageviewmodel dynamicpage)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(dynamicpage.memRefNo));
                if (dynamicpage.PageID > 0)
                {
                    var getold = context.DynamicPages.Where(cp => cp.PageID == dynamicpage.PageID).FirstOrDefault();
                    var getNew = context.DynamicPages.Where(cp => cp.PageID == dynamicpage.PageID).FirstOrDefault();
                    getNew.PageContent = dynamicpage.PageContent;
                    getNew.PageDescription = dynamicpage.PageDescription;
                    getNew.PageID = dynamicpage.PageID;
                    getNew.PageKeywords = dynamicpage.PageKeywords;
                    getNew.PageName = dynamicpage.PageName;
                    getNew.PageTitle = dynamicpage.PageTitle;
                    getNew.ptype = dynamicpage.PageType;
                    getNew.imageurl = dynamicpage.ImageUrl;
                    getNew.Sequence = dynamicpage.Sequence;
                    getNew.IsActive = dynamicpage.IsActive;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    DynamicPage f12 = new DynamicPage();
                    f12.PageContent = dynamicpage.PageContent;
                    f12.PageDescription = dynamicpage.PageDescription;
                    f12.PageKeywords = dynamicpage.PageKeywords;
                    f12.PageName = dynamicpage.PageName;
                    f12.PageTitle = dynamicpage.PageTitle;
                    f12.ptype = dynamicpage.PageType;
                    f12.imageurl = dynamicpage.ImageUrl;
                    f12.CreatedDate = DateTime.Now;
                    f12.Sequence = dynamicpage.Sequence;
                    f12.IsActive = dynamicpage.IsActive;
                    context.DynamicPages.Add(f12);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Updatesynamicpage", DateTime.Now, "Updatesynamicpage", ed.ToString(), "Updatesynamicpage", 2);
                return ed.InnerException.ToString();
            }

        }




        public dynamic DataMigration(string UserMemRefNo, string Api, string AuthonticationToken, string company)
        {
            try
            {
                var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo));
                string websitetype = Context2.Configs.Where(cp => cp.ConfigKey.Equals("websitetype")).Select(cp => cp.ConfigValue).FirstOrDefault();



                var c16 = customercounts(Api, AuthonticationToken, company);
                long cc16 = c16.count / 500;
                for (int i = 1; i <= cc16 + 1; i++)
                {
                    AllData_customers(Context2, Convert.ToString(customer(Api, AuthonticationToken, company, i)));
                }
                var c5 = sy_paramcounts(Api, AuthonticationToken, company);
                long cc5 = c5.count / 500;
                for (int i = 1; i <= cc5 + 1; i++)
                {
                    AllData_sy_param(Context2, Convert.ToString(sy_param(Api, AuthonticationToken, company, i)));
                }
                AllData_sy_company(Context2, Convert.ToString(sy_company(Api, AuthonticationToken, company)));
                if (!websitetype.Equals("1"))
                {
                    var wcat = warehouseCount(Api, AuthonticationToken, company);
                    long cauts = wcat.count / 500;
                    for (int i = 1; i <= cauts + 1; i++)
                    {
                        AllData_warehouse(Context2, Convert.ToString(warehouse(Api, AuthonticationToken, company, i)));
                    }
                    var c17 = itemcounts(Api, AuthonticationToken, company);
                    var icount = c17.count;
                    int getconicount = Convert.ToInt32(Context2.Configs.Where(cp => cp.ConfigKey.Equals("itemcount")).Select(cp => cp.ConfigValue).FirstOrDefault());
                    if (icount < getconicount)
                    {
                        long cc17 = icount / 500;
                        for (int i = 1; i <= cc17 + 1; i++)
                        {
                            AllData_item(Context2, Convert.ToString(item(Api, AuthonticationToken, company, i, 500)), UserMemRefNo);
                        }
                    }
                    else
                    {
                        if (getconicount > 500)
                        {
                            long cc17 = getconicount / 500;
                            for (int i = 1; i <= cc17 + 1; i++)
                            {
                                AllData_item(Context2, Convert.ToString(item(Api, AuthonticationToken, company, i, 500)), UserMemRefNo);
                            }
                        }
                        else
                        {
                            AllData_item(Context2, Convert.ToString(item(Api, AuthonticationToken, company, 1, 500)), UserMemRefNo);
                        }
                    }
                    var c3 = sy_termscounts(Api, AuthonticationToken, company);
                    long cc3 = c3.count / 500;
                    for (int i = 1; i <= cc3 + 1; i++)
                    {
                        AllData_sy_terms(Context2, Convert.ToString(sy_terms(Api, AuthonticationToken, company, i)));
                    }
                    var d33 = sy_postal_codecounts(Api, AuthonticationToken, company);
                    long dc33 = d33.count / 500;
                    for (int i = 1; i <= dc33 + 1; i++)
                    {
                        AllData_sy_postal_code(Context2, Convert.ToString(sy_postal_code(Api, AuthonticationToken, company, i)));
                    }
                    var c15 = cu_shiptocounts(Api, AuthonticationToken, company);
                    long cc15 = c15.count / 500;
                    for (int i = 1; i <= cc15 + 1; i++)
                    {
                        AllData_cu_shipto(Context2, Convert.ToString(cu_shipto(Api, AuthonticationToken, company, i)));
                    }
                    var c21 = oe_totcodescounts(Api, AuthonticationToken, company);
                    long cc21 = c21.count / 500;
                    for (int i = 1; i <= cc21 + 1; i++)
                    {
                        AllData_oe_totcodes(Context2, Convert.ToString(oe_totcodes(Api, AuthonticationToken, company, i)));
                    }

                    var c101 = sy_contactcounts(Api, AuthonticationToken, company);
                    long cc101 = c101.count / 500;
                    for (int i = 1; i <= cc101 + 1; i++)
                    {
                        AllData_sy_contact(Context2, Convert.ToString(sy_contact(Api, AuthonticationToken, company, i)));
                    }


                    var c26 = salesmancounts(Api, AuthonticationToken, company);
                    long cc26 = c26.count / 500;
                    for (int i = 1; i <= cc26 + 1; i++)
                    {
                        AllData_SalesMan(Context2, Convert.ToString(salesman(Api, AuthonticationToken, company, i)));
                    }
                    var c27 = sy_codescounts(Api, AuthonticationToken, company);
                    long cc27 = c27.count / 500;
                    for (int i = 1; i <= cc27 + 1; i++)
                    {
                        AllData_sy_codes(Context2, Convert.ToString(sy_codes(Api, AuthonticationToken, company, i)));
                    }
                    AllData_sy_country(Context2, Convert.ToString(sy_country(Api, AuthonticationToken, company)));
                    AllData_sy_paycodes(Context2, Convert.ToString(sy_paycodes(Api, AuthonticationToken, company)));
                    AllData_sy_shipvia(Context2, Convert.ToString(sy_shipvia(Api, AuthonticationToken, company)));
                    AllData_sy_prof_label(Context2, Convert.ToString(sy_prof_label(Api, AuthonticationToken, company)));
                    AllData_sy_States(Context2, Convert.ToString(Sy_States(Api, AuthonticationToken)));
                    AllDatait_majcls(Context2, Convert.ToString(it_majcls(Api, AuthonticationToken, company)));
                    var c29 = vendorcounts(Api, AuthonticationToken, company);
                    long cc29 = c29.count / 500;
                    for (int i = 1; i <= cc29 + 1; i++)
                    {
                        AllData_vendor(Context2, Convert.ToString(vendor(Api, AuthonticationToken, company, i)));
                    }
                    var c30 = wa_itemcounts(Api, AuthonticationToken, company);
                    long cc30 = c30.count / 500;
                    for (int i = 1; i <= cc30 + 1; i++)
                    {
                        AllData_wa_item(Context2, Convert.ToString(wa_item(Api, AuthonticationToken, company, i)));
                    }
                    var c32 = it_tree_nodecounts(Api, AuthonticationToken, company);
                    long cc32 = c32.count / 500;
                    for (int i = 1; i <= cc32 + 1; i++)
                    {
                        AllData_it_tree_node(Context2, Convert.ToString(it_tree_node(Api, AuthonticationToken, company, i)));
                    }
                    var c33 = it_tree_linkcounts(Api, AuthonticationToken, company);
                    long cc33 = c33.count / 500;
                    for (int i = 1; i <= cc33 + 1; i++)
                    {
                        AllDatait_tree_link(Context2, Convert.ToString(it_tree_link(Api, AuthonticationToken, company, i)));
                    }
                    var c34 = it_prodlinecounts(Api, AuthonticationToken, company);
                    long cc34 = c34.count / 500;
                    for (int i = 1; i <= cc34 + 1; i++)
                    {
                        AllDatait_prodline(Context2, Convert.ToString(it_prodline(Api, AuthonticationToken, company, i)));
                    }

                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "DataMigration", DateTime.Now, "DataMigration", ed.ToString(), "DataMigration", 2);
                return ed.InnerException;
            }
            return true;
        }


        public bool AllData_sy_States(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_States_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_States", DateTime.Now, "AllData_sy_States", de.ToString(), "AllData_sy_States", 2);
            }
            return flag;
        }
        public bool AllData_sy_prof_label(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_prof_label_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_prof_label", DateTime.Now, "AllData_sy_prof_label", de.ToString(), "AllData_sy_prof_label", 2);
            }
            return flag;
        }


        public bool AllData_sy_contact(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_sy_contact_Data(jsonobj);
                context.SaveChanges();

            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_contact", DateTime.Now, "AllData_sy_contact", de.ToString(), "AllData_sy_contact", 2);
            }

            return flag;
        }

        public bool AllData_oe_totcodes(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_oe_totcodes_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_oe_totcodes", DateTime.Now, "AllData_oe_totcodes", de.ToString(), "AllData_oe_totcodes", 2);
            }
            return flag;
        }
        public bool AllData_price(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_price_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_price", DateTime.Now, "AllData_price", de.ToString(), "AllData_price", 2);
            }

            return flag;
        }

        public bool AllData_price_sale(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_price_sale_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_price_sale", DateTime.Now, "AllData_price_sale", de.ToString(), "AllData_price_sale", 2);
            }

            return flag;
        }
        public bool AllData_SalesMan(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_salesman_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_SalesMan", DateTime.Now, "AllData_SalesMan", de.ToString(), "AllData_SalesMan", 2);
            }

            return flag;
        }
        public bool AllData_sy_company(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_sy_company_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_company", DateTime.Now, "AllData_sy_company", de.ToString(), "AllData_sy_company", 2);
            }

            return flag;
        }
        public bool AllData_sy_country(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_country_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_country", DateTime.Now, "AllData_sy_country", de.ToString(), "AllData_sy_country", 2);
            }
            return flag;
        }
        public bool AllData_sy_paycodes(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_sy_paycodes_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_paycodes", DateTime.Now, "AllData_sy_paycodes", de.ToString(), "AllData_sy_paycodes", 2);
            }

            return flag;
        }
        public bool AllData_sy_shipvia(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_sy_shipvia_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_shipvia", DateTime.Now, "AllData_sy_shipvia", de.ToString(), "AllData_sy_shipvia", 2);
            }

            return flag;
        }
        public bool AllData_sy_trans_notes(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_sy_trans_notes_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_trans_notes", DateTime.Now, "AllData_sy_trans_notes", de.ToString(), "AllData_sy_trans_notes", 2);
            }

            return flag;
        }
        public bool AllData_vendor(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_vendor_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_vendor", DateTime.Now, "AllData_vendor", de.ToString(), "AllData_vendor", 2);
            }

            return flag;
        }
        public bool AllData_cu_contact(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insertcu_contactData(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_cu_contact", DateTime.Now, "AllData_cu_contact", de.ToString(), "AllData_cu_contact", 2);
            }

            return flag;
        }

        public bool AllData_cu_shipto(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insertcu_shiptoData(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_cu_shipto", DateTime.Now, "AllData_cu_shipto", de.ToString(), "AllData_cu_shipto", 2);
            }

            return flag;
        }
        public bool AllData_customers(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.InsertcustomerData(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_customers", DateTime.Now, "AllData_customers", de.ToString(), "AllData_customers", 2);
            }

            return flag;
        }
        public bool AllData_oe_class(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_oe_class_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_oe_class", DateTime.Now, "AllData_oe_class", de.ToString(), "AllData_oe_class", 2);
            }

            return flag;
        }


        public bool AllData_sy_codes(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_sy_codes_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_codes", DateTime.Now, "AllData_sy_codes", de.ToString(), "AllData_sy_codes", 2);
            }

            return flag;
        }
        public bool AllData_wa_item(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_wa_item_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_wa_item", DateTime.Now, "AllData_wa_item", de.ToString(), "AllData_wa_item", 2);
            }

            return flag;
        }
        public bool AllData_it_tree_node(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_it_tree_node_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_it_tree_node", DateTime.Now, "AllData_it_tree_node", de.ToString(), "AllData_it_tree_node", 2);
            }

            return flag;
        }

        public bool AllData_Sy_States(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_States_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_Sy_States", DateTime.Now, "AllData_Sy_States", de.ToString(), "AllData_Sy_States", 2);
            }

            return flag;
        }
        public bool AllData_cu_prospect(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_cu_prospect_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_cu_prospect", DateTime.Now, "AllData_cu_prospect", de.ToString(), "AllData_cu_prospect", 2);
            }

            return flag;
        }
        public bool AllData_it_compl(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_it_compl__Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_it_compl", DateTime.Now, "AllData_it_compl", de.ToString(), "AllData_it_compl", 2);
            }

            return flag;
        }
        public bool AllData_it_component(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_it_component_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_it_component", DateTime.Now, "AllData_it_component", de.ToString(), "AllData_it_component", 2);
            }

            return flag;
        }
        public bool AllData_sy_currency(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_currency_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_currency", DateTime.Now, "AllData_sy_currency", de.ToString(), "AllData_sy_currency", 2);
            }

            return flag;
        }
        public bool AllData_sy_default(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_default_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_default", DateTime.Now, "AllData_sy_default", de.ToString(), "AllData_sy_default", 2);
            }

            return flag;
        }
        public bool AllData_sy_numctrl(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_numctrl_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_numctrl", DateTime.Now, "AllData_sy_numctrl", de.ToString(), "AllData_sy_numctrl", 2);
            }

            return flag;
        }
        public bool AllData_sy_param(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_param_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_param", DateTime.Now, "AllData_sy_param", de.ToString(), "AllData_sy_param", 2);
            }

            return flag;
        }
        public bool AllData_sy_postal_code(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_postal_code_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_postal_code", DateTime.Now, "AllData_sy_postal_code", de.ToString(), "AllData_sy_postal_code", 2);
            }

            return flag;
        }
        public bool AllData_sy_terms(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_sy_terms_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_sy_terms", DateTime.Now, "AllData_sy_terms", de.ToString(), "AllData_sy_terms", 2);
            }

            return flag;
        }

        public bool AllData_ve_contact(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_ve_contact_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_ve_contact", DateTime.Now, "AllData_ve_contact", de.ToString(), "AllData_ve_contact", 2);
            }

            return flag;
        }
        public bool AllData_warehouse(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_warehouse_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_warehouse", DateTime.Now, "AllData_warehouse", de.ToString(), "AllData_warehouse", 2);
            }

            return flag;
        }

        public bool AllData_it_xref(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_it_xref_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_it_xref", DateTime.Now, "AllData_it_xref", de.ToString(), "AllData_it_xref", 2);
            }

            return flag;
        }
        public bool AllData_item(ClientEntities context, string jsonobj, string MemrefNo)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                var Sqlserver = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("SqlServer");
                var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirEcom");
                string dataname = targetURL.ConfigurationValue + MemrefNo + "\\" + MemrefNo + ".mdf";
                string cnnString = "Data Source = " + Sqlserver.ConfigurationValue + "; AttachDBFilename = " + dataname + "; Integrated Security = True;";
                SqlConnection cnn = new SqlConnection(cnnString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "InsertitemrData";
                cmd.Parameters.Add("@jsondata", SqlDbType.NVarChar).Value = jsonobj;
                cnn.Open();
                object o = cmd.ExecuteScalar();
                cnn.Close();
                //context.InsertitemrData(jsonobj);
                //context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllData_item", DateTime.Now, "AllData_item", de.ToString(), "AllData_item", 2);
            }

            return flag;
        }

        public bool AllDatait_tree_link(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_it_tree_link_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllDatait_tree_link", DateTime.Now, "AllDatait_tree_link", de.ToString(), "AllDatait_tree_link", 2);
            }

            return flag;
        }
        public bool AllDatait_tree_node(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;
            try
            {
                context.Insert_it_tree_node_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllDatait_tree_node", DateTime.Now, "AllDatait_tree_node", de.ToString(), "AllDatait_tree_node", 2);
            }

            return flag;
        }


        public bool AllDatait_majcls(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_it_majcls_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllDatait_majcls", DateTime.Now, "AllDatait_majcls", de.ToString(), "AllDatait_majcls", 2);
            }

            return flag;
        }

        public bool AllDatait_prodline(ClientEntities context, string jsonobj)
        {
            jsonobj = jsonobj.Replace("'", "");
            bool flag = false;

            try
            {
                context.Insert_it_prodline_Data(jsonobj);
                context.SaveChanges();
            }
            catch (Exception de)
            {
                ErrorLogs.ErrorLog(0, "AllDatait_prodline", DateTime.Now, "AllDatait_prodline", de.ToString(), "AllDatait_prodline", 2);
            }

            return flag;
        }


        public dynamic sy_contactcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "2509be07-7471-44b2-857f-884b0b2b561f");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_contact%20where%20company_sy%3D'" + company + "'&table=sy_contact&filter=FOR%20EACH%20sy_contact%20where%20company_sy%3D'" + company + "'&undefined=", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_contactcounts", DateTime.Now, "sy_contactcounts", ed.ToString(), "sy_contactcounts", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_contact(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = (pageno - 1) * take;
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_contact%20where%20company_sy=%27" + company + "%27&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "17fa26d4-5f22-48e6-bbff-7a7580c78d0c");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);

                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_contact", DateTime.Now, "sy_contact", ed.ToString(), "sy_contact", 2);
                return ed.ToString();
            }

        }

        public dynamic cu_itaxable(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20cu_itaxable%20where%20company_cu%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "c0f0160f-f9ed-f53e-ac3c-f696ca119a6e");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);

            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cu_itaxable", DateTime.Now, "cu_itaxable", ed.ToString(), "cu_itaxable", 2);
                return ed.ToString();
            }
        }
        public dynamic cu_itaxablecounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "fefbdc3f-d483-4fac-8929-55a09cc96d03");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20cu_itaxable%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20&table=cu_itaxable&filter=FOR%20EACH%20cu_itaxable%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);

            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cu_itaxablecounts", DateTime.Now, "cu_itaxablecounts", ed.ToString(), "cu_itaxablecounts", 2);
                return ed.ToString();
            }
        }
        public dynamic warehouse(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20warehouse%20where%20company_it=%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "f478009d-c26a-4357-97db-f3864b032690");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "warehouse", DateTime.Now, "warehouse", ed.ToString(), "warehouse", 2);
                return ed.ToString();
            }

        }
        public dynamic warehouseCount(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "debfe420-ecae-4ff5-9073-33578a3c9137");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20warehouse%20where%20company_it%3D%22" + company + "%22&table=warehouse&filter=where%20company_it%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "warehouseCount", DateTime.Now, "warehouseCount", ed.ToString(), "warehouseCount", 2);
                return ed.ToString();
            }

        }

        public dynamic ve_contact(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20ve_contact%20where%20company_ve=%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "cb876662-da5f-4796-aeb1-f0d3b86327f0");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "ve_contact", DateTime.Now, "ve_contact", ed.ToString(), "ve_contact", 2);
                return ed.ToString();
            }

        }
        public dynamic cu_prospectCount(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "fd29106f-acab-474b-b98d-3f25c885cfc1");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20cu_prospect%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22&table=cu_prospect&filter=FOR%20EACH%20cu_prospect%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "ve_contactcounts", DateTime.Now, "ve_contactcounts", ed.ToString(), "ve_contactcounts", 2);
                return ed.ToString();
            }

        }
        public dynamic ve_contactcounts(string Api, string Authorization, string company)
        {
            try
            {

                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "3b8b7740-6fcc-4b87-b95e-d54729da1e4e");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20ve_contact%20where%20company_ve%3D%22" + company + "%22&table=ve_contact&filter=where%20company_ve%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "ve_contactcounts", DateTime.Now, "ve_contactcounts", ed.ToString(), "ve_contactcounts", 2);
                return ed.ToString();
            }

        }

        //httppost
        public dynamic changetrackerfetch(string authorizationtoken, string Api, int PageNo, string Take)
        {
            try
            {
                string dates = "2017-11-04";
                string skip = ((PageNo - 1) * 20).ToString();

                var client = new RestClient(Api + "/distone/rest/service/change-tracker/fetch");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "2e59dcf0-f23e-48c4-9599-742191515f59");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", authorizationtoken);
                request.AddParameter("undefined", "table=item&since=" + dates + "&filter=&columns=item.item%2Citem.descr%2Clast_update&skip=" + skip + "&take=" + Take, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "changetrackerfetch", DateTime.Now, "changetrackerfetch", ed.ToString(), "changetrackerfetch", 2);
                return ed.ToString();
            }
        }

        //httpget
        public dynamic PriceBreaks(string authorizationtoken, string Api, string ski, string customer, string warehouse)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/price/breaks?item=ski-" + ski + "&customer=" + customer + "&warehouse=" + warehouse);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "3c2370ce-dfb4-4b09-b2b7-c856194a8cc7");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("authorization", authorizationtoken);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "PriceBreaks", DateTime.Now, "PriceBreaks", ed.ToString(), "PriceBreaks", 2);
                return ed.ToString();
            }
        }

        //httpget
        public dynamic EmailSentItems(string authorizationtoken, string Api, string keywords, string recipients, string mindate, string maxdate, string take, string skip)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/email/sent-items?status=&keywords=" + keywords + "&recipients=" + recipients + "&min-date=" + mindate + "&max-date=" + maxdate + "&take=" + take + "&skip=" + skip);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "a94ba243-1cde-4e1a-8f49-6905d76892f9");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("authorization", authorizationtoken);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "EmailSentItems", DateTime.Now, "EmailSentItems", ed.ToString(), "EmailSentItems", 2);
                return ed.ToString();
            }
        }

        //httppost
        public dynamic DataReadShow(string authorizationtoken, string Api, string keywords, string recipients, string mindate, string maxdate, string take, string skip)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read-rows");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "383c0e5a-0019-4e18-9946-7fcd27da644c");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", authorizationtoken);
                request.AddParameter("undefined", "table=item&columns=company_it%40Company%2Citem%40Item%2Cdescr%5B1%5D%40Description&rowids=0x00000000008aae11%2C0x0000000000190463%2C0x0000000000190480", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "DataReadShow", DateTime.Now, "DataReadShow", ed.ToString(), "DataReadShow", 2);
                return ed.ToString();
            }
        }

        //httpGet
        public dynamic AROpenOrderTotal(string authorizationtoken, string Api, string customer, string customer_po, string credit_check, string currency_code)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/accounts-receivable/open-order-total?customer=" + customer + "&customer_po=" + customer_po + "&credit_check=" + credit_check + "&currency_code=" + currency_code);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "db507298-eb76-437d-a599-51e40823c2c9");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", authorizationtoken);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "AROpenOrderTotal", DateTime.Now, "AROpenOrderTotal", ed.ToString(), "AROpenOrderTotal", 2);
                return ed.ToString();
            }
        }

        //httppost
        public dynamic imagethumbnails(string authorizationtoken, string Api)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/image/thumbnails");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "c7e0504b-713f-4a7f-8dc0-245dfcee9e56");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("authorization", authorizationtoken);
                request.AddParameter("undefined", "{\n  \"category\":\"item\",\n  \"size\":32,\n  \"images\":[\n    { \"key\":\"ski\" }\n  ]\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "imagethumbnails", DateTime.Now, "imagethumbnails", ed.ToString(), "imagethumbnails", 2);
                return ed.ToString();
            }
        }

        //httpGet
        public dynamic OrderFetch(string authorizationtoken, string Api, string order, string type, string seq)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/order/fetch?order=" + order + "&type=" + type + "&seq=" + seq);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "272c9b33-d554-4225-a510-b662e68b3fba");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", authorizationtoken);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "OrderFetch", DateTime.Now, "OrderFetch", ed.ToString(), "OrderFetch", 2);
                return ed.ToString();
            }
        }


        //httpGet
        public dynamic EmailStatus(string authorizationtoken, string Api, string rowid)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/email/status?rowid=" + rowid);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "f167fe94-1687-4282-bdd2-ab80bdce4351");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", authorizationtoken);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "EmailStatus", DateTime.Now, "EmailStatus", ed.ToString(), "EmailStatus", 2);
                return ed.ToString();
            }
        }
        //httpPost
        public dynamic EmailSend(string authorizationtoken, string Api, string rowid, string to, string subject, string message)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/email/send");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "b9682358-1a60-4180-91e7-e9119c81c1a9");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", authorizationtoken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("undefined", "{\n  \"to\":\"" + to + "\",\n  \"subject\":\"" + subject + "\",\n  \"message\":\"" + message + "\"\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "EmailSend", DateTime.Now, "EmailSend", ed.ToString(), "EmailSend", 2);
                return ed.ToString();
            }
        }

        //httpGet
        public dynamic ARTotal(string authorizationtoken, string Api, string customer, string customer_po, string all_companies, string age_date, string currency_code)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/accounts-receivable/ar-total?customer=" + customer + "&customer_po=" + customer_po + "&all_companies=" + all_companies + "&age_date=" + age_date + "&currency_code=" + currency_code);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "894033ba-eb32-4e59-be7e-147aa6ef214a");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", authorizationtoken);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "ARTotal", DateTime.Now, "ARTotal", ed.ToString(), "ARTotal", 2);
                return ed.ToString();
            }
        }

        //httpPost
        public dynamic itemavailability(string authorizationtoken, string Api, string items, string warehouses, string format)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/item/availability");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "e6dd3923-34dd-469f-a0e4-c3882f049521");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", authorizationtoken);
                request.AddParameter("undefined", "items=" + items + "&warehouses=" + warehouses + "&format=" + format, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "itemavailability", DateTime.Now, "itemavailability", ed.ToString(), "itemavailability", 2);
                return ed.ToString();
            }
        }
        //httpPost
        public dynamic reportgltrend(string authorizationtoken, string Api, string codes, string start, string end)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/report/gl-trend");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "bde5a5c1-7b0d-4033-ac02-7441d553ebe6");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", authorizationtoken);
                request.AddParameter("undefined", "codes=" + codes + "&start=" + start + "&end=" + end, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "reportgltrend", DateTime.Now, "reportgltrend", ed.ToString(), "reportgltrend", 2);
                return ed.ToString();
            }
        }


        //httpGet
        public dynamic clientsupdate(string authorizationtoken, string Api, string codes, string start, string end)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/report/gl-trend");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "bde5a5c1-7b0d-4033-ac02-7441d553ebe6");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", authorizationtoken);
                request.AddParameter("undefined", "codes=" + codes + "&start=" + start + "&end=" + end, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "clientsupdate", DateTime.Now, "clientsupdate", ed.ToString(), "clientsupdate", 2);
                return ed.ToString();
            }
        }


        //httpGet
        public dynamic imagefetch(string authorizationtoken, string Api, string category, string key, string max_width, string max_height)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/image/fetch?category=" + category + "&key=" + key + "&max_width=" + max_width + "&max_height=" + max_height);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "0b246955-9a13-4f02-8032-d872511aac0f");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("authorization", authorizationtoken);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "imagefetch", DateTime.Now, "imagefetch", ed.ToString(), "imagefetch", 2);
                return ed.ToString();
            }
        }

        //httpGet
        public dynamic changetrackerupdate(string authorizationtoken, string Api, string category, string key, string max_width, string max_height)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/change-tracker/update?table=item&begin=0");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "32670ef9-ecc4-470c-91ac-abf7944d7b4e");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("authorization", authorizationtoken);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "changetrackerupdate", DateTime.Now, "changetrackerupdate", ed.ToString(), "changetrackerupdate", 2);
                return ed.ToString();
            }
        }

        //httpPOST
        public dynamic signaturesubmit(string authorizationtoken, string Api, string manifest, string rec_type, string printed_name, string captured_on, string signature)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/signature/submit");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "e5aff21e-2ba0-44b6-a536-debd00cd1746");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("authorization", authorizationtoken);
                request.AddParameter("undefined", "{\n  \"manifest\":\"" + manifest + "\",\n  \"rec_type\":\"" + rec_type + "\",\n  \"printed_name\":\"" + printed_name + "\",\n  \"captured_on\":\"" + captured_on + "\",\n  \"signature\":" + signature, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "signaturesubmit", DateTime.Now, "signaturesubmit", ed.ToString(), "signaturesubmit", 2);
                return ed.ToString();
            }
        }




        //httpPOST
        public dynamic datahashbytes(string authorizationtoken, string Api, string item, string company_it, string rowid, string skip, string take)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/hashbytes");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "e3af4631-024f-4bd3-95a0-95617bf1a697");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", authorizationtoken);
                request.AddParameter("undefined", "table=" + item + "&filter=company_it%20%3D%20%22" + company_it + "%22&rowid=" + rowid + "&skip=" + skip + "&take=" + take, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "datahashbytes", DateTime.Now, "datahashbytes", ed.ToString(), "datahashbytes", 2);
                return ed.ToString();
            }
        }

        //httpPOST
        public dynamic datacount(string authorizationtoken, string Api, string company_cu, string ipad_extract)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "62c0981f-e80e-4296-814e-1a60daf9509f");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", authorizationtoken);
                request.AddParameter("undefined", "query=FOR%20EACH%20customer%20WHERE%20customer.company_cu%20%3D%20%22" + company_cu + "%22&table=item&filter=item.company_it%20%3D%20%22" + company_cu + "%22%20AND%20item.ipad_extract%20%3D%20" + ipad_extract, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "datacount", DateTime.Now, "datacount", ed.ToString(), "datacount", 2);
                return ed.ToString();
            }
        }

        //httpPOST
        public dynamic authorizegrant(string clients, string Api, string company, string username, string password)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/authorize/grant");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "d638a7e3-ad6d-4bfe-ad30-645f51cd3161");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("undefined", "client=" + clients + "&company=" + company + "&username=" + username + "&password=" + password, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "authorizegrant", DateTime.Now, "authorizegrant", ed.ToString(), "authorizegrant", 2);
                return ed.ToString();
            }
        }

        //httpGET
        public dynamic creditcheck(string Authorization, string Api, string customer, string customer_po, string force_totals)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/accounts-receivable/credit-check?customer=" + customer + "&customer_po=" + customer_po + "&force_totals=" + force_totals);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "699f4c82-b730-4e1c-90b4-46e00c138c60");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "creditcheck", DateTime.Now, "creditcheck", ed.ToString(), "creditcheck", 2);
                return ed.ToString();
            }
        }

        //httpGET
        public dynamic lastpaydate(string Authorization, string Api, string customer)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/accounts-receivable/last-pay-date?customer=" + customer);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "c24f3bf4-5552-4e91-a3ca-d3ad02ded27c");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "lastpaydate", DateTime.Now, "lastpaydate", ed.ToString(), "lastpaydate", 2);
                return ed.ToString();
            }
        }

        //httpGET
        public dynamic itemrestriction(string Authorization, string Api, string item, string customer, string ship_id, string ord_class, string warehouse, string type, string verbose)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/item/restriction?item=" + item + "&customer=" + customer + "&ship_id=" + ship_id + "&ord_class=" + ord_class + "&warehouse=" + warehouse + "&type=" + type + "&verbose=" + verbose);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "8bdeeb12-999a-49c4-9220-981312574edc");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "itemrestriction", DateTime.Now, "itemrestriction", ed.ToString(), "itemrestriction", 2);
                return ed.ToString();
            }
        }

        //httpPOST
        public dynamic vmiimport(string Authorization, string Api, string item, string customer, string ship_id, string ord_class, string warehouse, string type, string verbose)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/vmi/import");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "1b575131-1bcd-4240-b6e6-0277255abc58");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("undefined", "{\n  \"customer\":\"VLC\",\n  \"ship_id\":\"\",\n  \"warehouse\":\"1\",\n  \"initials\":\"MFK\",\n  \"created\":\"2017-07-10T15:10:00\",\n  \"lines\":[{\n    \"tag_no\":\"642\",\n    \"item\":\"\",\n    \"quantity\":10,\n    \"comment\":\"Samples\"\n  },{\n    \"tag_no\":\"644\",\n    \"item\":\"\",\n    \"quantity\":2,\n    \"comment\":\"An Extreme Ski\"\n  }]\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "vmiimport", DateTime.Now, "vmiimport", ed.ToString(), "vmiimport", 2);
                return ed.ToString();
            }
        }
        //httpGET
        public dynamic pricefetch(string Authorization, string Api, string item, string customer, string quantity, string warehouse)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/price/fetch?item=" + item + "&customer=" + customer + "&quantity=" + quantity + "&warehouse=" + warehouse);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "51d68949-b405-4ebc-82bb-e7bdd8e09ccd");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "pricefetch", DateTime.Now, "pricefetch", ed.ToString(), "pricefetch", 2);
                return ed.ToString();
            }
        }


        //POST
        public dynamic cp_head(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20cp_head%20where%20company_cu%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "f70bdd25-a4d1-6bf9-17ec-74bf2c402a6d");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cp_head", DateTime.Now, "cp_head", ed.ToString(), "cp_head", 2);
                return ed.ToString();
            }

        }
        public dynamic cp_headcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "03bf0741-5f7b-4c99-9066-4a8ec4754c84");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20cp_head%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20&table=cp_head&filter=FOR%20EACH%20cp_head%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cp_headcounts", DateTime.Now, "cp_headcounts", ed.ToString(), "cp_headcounts", 2);
                return ed.ToString();
            }

        }
        public dynamic cu_prospect(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20cu_prospect%20where%20company_cu%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "d304f844-bb4b-384a-4c72-96ddad697ffd");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cu_prospect", DateTime.Now, "cu_prospect", ed.ToString(), "cu_prospect", 2);
                return ed.ToString();
            }

        }
        public dynamic cu_prospectcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "fd29106f-acab-474b-b98d-3f25c885cfc1");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20cu_prospect%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22&table=cu_prospect&filter=FOR%20EACH%20cu_prospect%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cu_prospectcounts", DateTime.Now, "cu_prospectcounts", ed.ToString(), "cu_prospectcounts", 2);
                return ed.ToString();
            }

        }
        public dynamic it_compl(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20it_compl%20where%20company_it%3D%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "54b9ec7b-2718-cf13-dbdc-21d04b42322f");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_compl", DateTime.Now, "it_compl", ed.ToString(), "it_compl", 2);
                return ed.ToString();
            }

        }
        public dynamic it_complcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "4508f519-15c8-47cc-8c9d-a8405d6cf4ee");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20it_compl%20NO-LOCK%20where%20company_it%3D%22" + company + "%22%20&table=it_compl&filter=FOR%20EACH%20it_compl%20NO-LOCK%20where%20company_it%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_complcounts", DateTime.Now, "it_complcounts", ed.ToString(), "it_complcounts", 2);
                return ed.ToString();
            }

        }
        public dynamic it_component(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20it_component%20where%20company_it%3D%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "72cec23e-01fe-bf49-8aad-b1166595c4f7");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_component", DateTime.Now, "it_component", ed.ToString(), "it_component", 2);
                return ed.ToString();
            }

        }
        public dynamic it_componentcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "40ab86a1-0669-4088-ac28-ddabe89571ba");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20it_component%20NO-LOCK%20where%20company_it%3D%22" + company + "%22%20&table=it_component&filter=FOR%20EACH%20it_component%20NO-LOCK%20where%20company_it%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_componentcounts", DateTime.Now, "it_componentcounts", ed.ToString(), "it_componentcounts", 2);
                return ed.ToString();
            }

        }

        public dynamic sy_currency(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_currency%20where%20sy_currency.company_sy=%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "5051ff4a-4d31-4cb0-8664-e69ffb93a0f5");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_currency", DateTime.Now, "sy_currency", ed.ToString(), "sy_currency", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_currencycounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "a824ce08-6115-4005-8fb8-0341f8969770");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_currency%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20&table=sy_currency&filter=FOR%20EACH%20sy_currency%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_currencycounts", DateTime.Now, "sy_currencycounts", ed.ToString(), "sy_currencycounts", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_default(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_default%20%20where%20sy_default.company_sy=%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "9ae50a29-abb6-45cc-98b0-8e34d785337b");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_default", DateTime.Now, "sy_default", ed.ToString(), "sy_default", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_defaultcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "e7458d22-254e-4f1d-b506-dc7ee20e49b1");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_default%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20&table=sy_default&filter=FOR%20EACH%20sy_default%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_defaultcounts", DateTime.Now, "sy_defaultcounts", ed.ToString(), "sy_defaultcounts", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_numctrl(string Api, string Authorization, string company, int page)
        {
            try
            {
                int take = 500;
                int skip = ((page - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_numctrl%20where%20company_sy=%22" + company + "%22%20and%20type%3C%3E%22%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "39582e73-6c28-4eae-877c-11f6bd25f892");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_numctrl", DateTime.Now, "sy_numctrl", ed.ToString(), "sy_numctrl", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_numctrlcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "b937fe94-087a-4730-bea2-a7c9a46a1610");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_numctrl%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20&table=sy_numctrl&filter=FOR%20EACH%20sy_numctrl%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_numctrlcounts", DateTime.Now, "sy_numctrlcounts", ed.ToString(), "sy_numctrlcounts", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_param(string Api, string Authorization, string company, int page)
        {
            try
            {
                int take = 500;
                int skip = ((page - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_param%20where%20company_sy=%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "8d80f2e1-f9bd-4ace-ac9c-82ce3895ecb0");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_param", DateTime.Now, "sy_param", ed.ToString(), "sy_param", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_paramcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "51e1f9a2-c7d0-4885-bd87-efc047cc16c6");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_param%20where%20company_sy%3D%22" + company + "%22&table=sy_param&filter=where%20company_sy%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_paramcounts", DateTime.Now, "sy_paramcounts", ed.ToString(), "sy_paramcounts", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_postal_code(string Api, string Authorization, string company, int page)
        {
            try
            {
                int take = 500;
                int skip = ((page - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_postal_code&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "261d8130-2030-4603-9449-b65602648f8f");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_postal_code", DateTime.Now, "sy_postal_code", ed.ToString(), "sy_postal_code", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_postal_codecounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "933aa5a9-9857-4898-b922-79dbd9527226");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_postal_code&table=sy_postal_code&filter=", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_postal_codecounts", DateTime.Now, "sy_postal_codecounts", ed.ToString(), "sy_postal_codecounts", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_terms(string Api, string Authorization, string company, int page)
        {
            try
            {
                int take = 500;
                int skip = ((page - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_terms&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "43084e97-c12e-4631-b984-55a59d75d50b");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_terms", DateTime.Now, "sy_terms", ed.ToString(), "sy_terms", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_termscounts(string Api, string Authorization, string company)
        {
            try
            {

                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "6dd4f96b-1787-4459-8721-eede3f9efb01");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_terms&table=sy_terms&filter=", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_termscounts", DateTime.Now, "sy_termscounts", ed.ToString(), "sy_termscounts", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_user(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_user%20%20where%20company_sy=%22" + company + "%22%20and%20password%3C%3E%22%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "ef4503ff-d1ff-461c-9842-a8b900f6d66c");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_user", DateTime.Now, "sy_user", ed.ToString(), "sy_user", 2);
                return ed.ToString();
            }

        }
        public dynamic sy_usercounts(string Api, string Authorization, string company)
        {
            try
            {

                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "615258d4-6b3a-4aec-a032-959324a2ee90");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_user%20%20where%20company_sy%3D%22" + company + "%22and%20password%3C%3E%22%22&table=sy_user%20&filter=where%20company_sy%3D%22" + company + "%22and%20password%3C%3E%22%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_usercounts", DateTime.Now, "sy_usercounts", ed.ToString(), "sy_usercounts", 2);
                return ed.ToString();
            }

        }
        //POST
        public dynamic cu_contact(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20cu_contact%20where%20company_cu%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "06d5a11b-ac37-40b7-0f4a-dc0756510aa3");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cu_contact", DateTime.Now, "cu_contact", ed.ToString(), "cu_contact", 2);
                return ed.ToString();
            }

        }
        public dynamic cu_contactcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "a3c4bb03-a0f5-4b08-b22e-9a6dfc84f653");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20cu_contact%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20&table=cu_contact&filter=FOR%20EACH%20cu_contact%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cu_contactcounts", DateTime.Now, "cu_contactcounts", ed.ToString(), "cu_contactcounts", 2);
                return ed.ToString();
            }

        }

        //POST

        //POST
        public dynamic cu_shipto(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);

                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20cu_shipto%20where%20company_cu%3D%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "476cd1eb-36c9-7442-6408-93d0cc173903");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cu_shipto", DateTime.Now, "cu_shipto", ed.ToString(), "cu_shipto", 2);
                return ed.ToString();
            }

        }

        public dynamic cu_shiptocounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "e98a3286-938f-44f4-a231-5f2f79358069");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20cu_shipto%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22&table=cu_shipto&filter=FOR%20EACH%20cu_shipto%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "cu_shiptocounts", DateTime.Now, "cu_shiptocounts", ed.ToString(), "cu_shiptocounts", 2);
                return ed.ToString();
            }

        }
        public dynamic customer(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = (pageno - 1) * take;
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20customer%20where%20company_cu%3D%22" + company + "%22%20and%20Active%3Dtrue%20and%20is_deleted%3Dfalse%20and%20web_passwd%3C%3E%22%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "a945f5cd-51e1-9ab2-d2c8-6e0c5137ad64");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "customer", DateTime.Now, "customer", ed.ToString(), "customer", 2);
                return ed.ToString();
            }

        }
        public dynamic customercounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "f00da8df-33d1-4879-a2b0-703ecdfe28d3");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20customer%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20and%20Active%3Dtrue%20and%20is_deleted%3Dfalse%20and%20web_passwd%3C%3E%22%22&table=customer&filter=FOR%20EACH%20customer%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20and%20Active%3Dtrue%20and%20is_deleted%3Dfalse%20and%20web_passwd%3C%3E%22%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "customercounts", DateTime.Now, "customercounts", ed.ToString(), "customercounts", 2);
                return ed.ToString();
            }

        }

        public dynamic item(string Api, string Authorization, string company, int pageno, int take)
        {
            try
            {
                int skip = ((pageno - 1) * take);
                var client = new RestClient();
                client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20item%20where%20company_it=%22" + company + "%22%20and%20discontinued=false%20and%20is_deleted=false%20and%20web_item=true%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "02646865-2506-4dfe-83ae-27d0e954c170");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "item", DateTime.Now, "item", ed.ToString(), "item", 2);
                return ed.ToString();
            }
        }
        public dynamic itemcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "9f6e3432-35a8-451c-9e0d-00e0039c1c48");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20item%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20and%20web_item%3Dtrue%20and%20is_deleted%3Dfalse%20and%20discontinued%3Dfalse&table=item&filter=FOR%20EACH%20item%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20and%20web_item%3Dtrue%20and%20is_deleted%3Dfalse%20and%20discontinued%3Dfalse", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "itemcounts", DateTime.Now, "itemcounts", ed.ToString(), "itemcounts", 2);
                return ed.ToString();
            }
        }

        public dynamic oe_class(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_class%20where%20company_oe%3D%22" + company + "%22%20%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "71dbc96a-13e6-7606-017b-93d239b9d5b8");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "oe_class", DateTime.Now, "oe_class", ed.ToString(), "oe_class", 2);
                return ed.ToString();
            }
        }
        public dynamic oe_classcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "a4abc4e2-81b0-4124-a4ee-97dce9d98513");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20oe_class%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20&table=oe_class&filter=FOR%20EACH%20oe_class%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "oe_classcounts", DateTime.Now, "oe_classcounts", ed.ToString(), "oe_classcounts", 2);
                return ed.ToString();
            }
        }

        public dynamic oe_head(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20where%20company_oe%3D%22" + company + "%22%20%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "f98a6664-2ffb-5055-bf54-f988dcfee2c1");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "oe_head", DateTime.Now, "oe_head", ed.ToString(), "oe_head", 2);
                return ed.ToString();
            }
        }
        public dynamic oe_headcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "648b3733-1752-4df7-baf5-c65a278799e6");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20oe_head%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20&table=oe_head&filter=FOR%20EACH%20oe_head%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "oe_headcounts", DateTime.Now, "oe_headcounts", ed.ToString(), "oe_headcounts", 2);
                return ed.ToString();
            }
        }

        public dynamic oe_line(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_line%20where%20company_oe%3D%22" + company + "%22%20%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "eb33edb7-8770-f2a1-a95c-fa9a5d112b3f");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "oe_line", DateTime.Now, "oe_line", ed.ToString(), "oe_line", 2);
                return ed.ToString();
            }
        }
        public dynamic oe_linecounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "29ddabb3-e722-4b83-9a43-b2f3fed30b81");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20oe_line%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20&table=oe_line&filter=FOR%20EACH%20oe_line%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "oe_linecounts", DateTime.Now, "oe_linecounts", ed.ToString(), "oe_linecounts", 2);
                return ed.ToString();
            }
        }

        public dynamic oe_totcodes(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_totcodes%20where%20company_oe%3D%22" + company + "%22%20%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "507ab207-e488-6cef-3b53-1f0f9871502f");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "oe_totcodes", DateTime.Now, "oe_totcodes", ed.ToString(), "oe_totcodes", 2);
                return ed.ToString();
            }
        }
        public dynamic oe_totcodescounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "77e9c787-c787-4497-9365-219f18080625");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20oe_totcodes%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20&table=oe_totcodes&filter=FOR%20EACH%20oe_totcodes%20NO-LOCK%20where%20company_oe%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "oe_totcodescounts", DateTime.Now, "oe_totcodescounts", ed.ToString(), "oe_totcodescounts", 2);
                return ed.ToString();
            }
        }

        public dynamic price(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20price%20%20where%20company_ip%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "78dad94b-c9d8-6436-8615-38128ec9d335");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "price", DateTime.Now, "price", ed.ToString(), "price", 2);
                return ed.ToString();
            }
        }
        public dynamic pricecounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "5cbe551a-b9c3-4c62-889a-61294b2b4278");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20price%20NO-LOCK%20where%20company_ip%3D%22" + company + "%22%20&table=price&filter=FOR%20EACH%20price%20NO-LOCK%20where%20company_ip%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "pricecounts", DateTime.Now, "pricecounts", ed.ToString(), "pricecounts", 2);
                return ed.ToString();
            }
        }

        public dynamic price_def(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20price_def%20WHERE%20price_def.company_ip%20%3D%20%22" + company + "%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "797f4730-8cc0-595b-9328-96fb4d05e8e6");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "price_def", DateTime.Now, "price_def", ed.ToString(), "price_def", 2);
                return ed.ToString();
            }
        }

        public dynamic price_find(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20price_find%20%20where%20company_ip%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "0795182a-1033-4319-1a4b-c2eb1354de39");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "price_find", DateTime.Now, "price_find", ed.ToString(), "price_find", 2);
                return ed.ToString();
            }
        }
        public dynamic price_findcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "72fc0639-34f6-4c0b-a42c-3973713f00a9");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20price_find%20NO-LOCK%20where%20company_ip%3D%22" + company + "%22%20&table=price_find&filter=FOR%20EACH%20price_find%20NO-LOCK%20where%20company_ip%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "price_findcounts", DateTime.Now, "price_findcounts", ed.ToString(), "price_findcounts", 2);
                return ed.ToString();
            }
        }

        public dynamic price_rental(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20price_rental%20%20where%20company_it%3D%22" + company + "%22%20%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "3505c26a-064c-b431-09b8-da36146a9e34");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "price_rental", DateTime.Now, "price_rental", ed.ToString(), "price_rental", 2);
                return ed.ToString();
            }
        }
        public dynamic price_rentalcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "8e50f452-e562-427b-b2ff-c9d39e596bac");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20price_rental%20NO-LOCK%20where%20company_it%3D%22" + company + "%22%20&table=price_rental&filter=FOR%20EACH%20price_rental%20NO-LOCK%20where%20company_it%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "price_rentalcounts", DateTime.Now, "price_rentalcounts", ed.ToString(), "price_rentalcounts", 2);
                return ed.ToString();
            }
        }

        public dynamic price_sale(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20price_sale%20%20where%20company_it%3D%22" + company + "%22%20%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "1a69b6a1-1f8c-c1cb-ea7d-6b74a0846d8d");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "price_sale", DateTime.Now, "price_sale", ed.ToString(), "price_rental", 2);
                return ed.ToString();
            }
        }
        public dynamic price_salecounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "0f35e572-6b27-480f-ab1a-b523e8e0745a");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20price_sale%20NO-LOCK%20where%20company_it%3D%22" + company + "%22%20&table=price_sale&filter=FOR%20EACH%20price_sale%20NO-LOCK%20where%20company_it%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "price_salecounts", DateTime.Now, "price_salecounts", ed.ToString(), "price_salecounts", 2);
                return ed.ToString();
            }
        }

        public dynamic salesman(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20salesman%20where%20company_cu%3D%22" + company + "%22%20and%20web_passwd%3C%3E%22%22%20%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "8cce71f9-e767-f24a-31b7-a92dc9556f34");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "salesman", DateTime.Now, "salesman", ed.ToString(), "salesman", 2);
                return ed.ToString();
            }
        }
        public dynamic sy_states(string Api, string Authorization)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_codes%20NO-LOCK%20%20where%20code_type=%22sy_state%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "6c906d88-d5e5-4511-ab45-b3c22ce1cc28");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_states", DateTime.Now, "sy_states", ed.ToString(), "sy_states", 2);
                return ed.ToString();
            }
        }
        public dynamic salesmancounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "72926314-b756-4d4e-ac03-ada2f93513dd");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20salesman%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20&table=salesman&filter=FOR%20EACH%20salesman%20NO-LOCK%20where%20company_cu%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "salesmancounts", DateTime.Now, "salesmancounts", ed.ToString(), "salesmancounts", 2);
                return ed.ToString();
            }
        }

        public dynamic sy_codes(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_codes%20where%20company_sy%3D%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "a77230f9-3f47-bddd-48f1-7cf631aa1afb");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_codes", DateTime.Now, "sy_codes", ed.ToString(), "sy_codes", 2);
                return ed.ToString();
            }
        }
        public dynamic sy_codescounts(string Api, string Authorization, string company)
        {
            try
            {

                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "2afa145e-9a77-49ca-a5e0-0b1f2d8179d7");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_codes%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20&table=sy_codes&filter=FOR%20EACH%20sy_codes%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_codescounts", DateTime.Now, "sy_codescounts", ed.ToString(), "sy_codescounts", 2);
                return ed.ToString();
            }
        }

        public dynamic sy_company(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_company%20WHERE%20sy_company.company_sy%20%3D%20%22" + company + "%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "87a05835-8808-3df7-b6a0-ce05e0c4bc8c");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_company", DateTime.Now, "sy_company", ed.ToString(), "sy_company", 2);
                return ed.ToString();
            }
        }

        public dynamic sy_country(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_country");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "bfc7594e-4bf2-cd2e-a099-0cca7e10a6a5");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_company", DateTime.Now, "sy_company", ed.ToString(), "sy_company", 2);
                return ed.ToString();
            }
        }

        public dynamic sy_paycodes(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_paycodes%20WHERE%20sy_paycodes.company_sy%20%3D%20%22" + company + "%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "93c516f9-2d74-6084-2b8e-570cffa5c6b9");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_paycodes", DateTime.Now, "sy_paycodes", ed.ToString(), "sy_paycodes", 2);
                return ed.ToString();
            }
        }

        public dynamic sy_shipvia(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_shipvia");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "ceb8763f-5d07-159d-9345-0949c8a3729d");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_shipvia", DateTime.Now, "sy_shipvia", ed.ToString(), "sy_shipvia", 2);
                return ed.ToString();
            }
        }

        public dynamic sy_trans_notes(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_trans_notes%20where%20company_sy%3D%22" + company + "%22&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "afbb8c3a-f9b3-bd53-b1f0-cefceba4e2d5");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_trans_notes", DateTime.Now, "sy_trans_notes", ed.ToString(), "sy_trans_notes", 2);
                return ed.ToString();
            }
        }
        public dynamic sy_trans_notescounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "f0843635-45e7-4e44-b475-7c2558ded1db");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20sy_trans_notes%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20&table=sy_trans_notes&filter=FOR%20EACH%20sy_trans_notes%20NO-LOCK%20where%20company_sy%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_trans_notescounts", DateTime.Now, "sy_trans_notescounts", ed.ToString(), "sy_trans_notescounts", 2);
                return ed.ToString();
            }
        }

        public dynamic vendor(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20vendor%20where%20company_ve%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "279f4671-d2bb-8c89-7aef-d13d53a430a9");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "vendor", DateTime.Now, "vendor", ed.ToString(), "vendor", 2);
                return ed.ToString();
            }
        }
        public dynamic vendorcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "ea495730-4026-4409-b145-56ed375e1e44");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20vendor%20NO-LOCK%20where%20company_ve%3D%22" + company + "%22%20&table=vendor&filter=FOR%20EACH%20vendor%20NO-LOCK%20where%20company_ve%3D%22" + company + "%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "vendorcounts", DateTime.Now, "vendorcounts", ed.ToString(), "vendorcounts", 2);
                return ed.ToString();
            }
        }

        public dynamic wa_item(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20item%20WHERE%20item.company_it%20%3D%20%22" + company + "%22%20and%20item.web_item%20%3D%20%22yes%22%20and%20item.is_deleted%20%3D%20%22no%22%2C%20EACH%20wa_item%20WHERE%20wa_item.company_it%20%3D%20item.company_it%20AND%20wa_item.item%20%3D%20item.item&columns=wa_item.warehouse%2Cwa_item.item%2Cwa_item.list_price%2Cwa_item.list_price_per%2Cwa_item.um_alwd%2Cwa_item.qty_cmtd%2Cwa_item.qty_oh%2Citem.company_it&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "dccf622b-7d81-466b-e619-71719d8fdf63");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "wa_item", DateTime.Now, "wa_item", ed.ToString(), "wa_item", 2);
                return ed.ToString();
            }
        }
        public dynamic wa_itemcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "fa7ad0d8-05ba-f214-c13f-332810980cb6");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("application/x-www-form-urlencoded", "query=FOR%20EACH%20item%20WHERE%20item.company_it%20%3D%20%22" + company + "%22%20and%20item.web_item%20%3D%20%22yes%22%20and%20item.is_deleted%20%3D%20%22no%22%2C%20EACH%20wa_item%20WHERE%20wa_item.company_it%20%3D%20item.company_it%20AND%20wa_item.item%20%3D%20item.item&table=wa_item&filter=WHERE%20item.company_it%20%3D%20%22" + company + "%22%20and%20item.web_item%20%3D%20%22yes%22%20and%20item.is_deleted%20%3D%20%22no%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "wa_itemcounts", DateTime.Now, "wa_itemcounts", ed.ToString(), "wa_itemcounts", 2);
                return ed.ToString();
            }
        }
        public dynamic it_xref(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20it_xref%20where%20company_it%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "4f0ed2e1-0564-5e58-70d1-aec2e212a50d");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_xref", DateTime.Now, "it_xref", ed.ToString(), "it_xref", 2);
                return ed.ToString();
            }
        }
        public dynamic it_xrefcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "aeab6641-9439-a1fa-2a8a-e4ddeb395f23");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("application/x-www-form-urlencoded", "query=FOR%20EACH%20it_xref&table=it_xref&filter=where%20company_it%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_xrefcounts", DateTime.Now, "it_xrefcounts", ed.ToString(), "it_xrefcounts", 2);
                return ed.ToString();
            }
        }

        public dynamic it_tree_node(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20it_tree_node%20where%20company_it%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "da11affb-ea44-3a4c-b4c4-5ed9d9198388");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_tree_node", DateTime.Now, "it_tree_node", ed.ToString(), "it_tree_node", 2);
                return ed.ToString();
            }
        }
        public dynamic it_tree_nodecounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "a664e79a-1f32-1e78-0442-aa696128662d");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("application/x-www-form-urlencoded", "query=FOR%20EACH%20it_tree_node&table=it_tree_node&filter=where%20company_it%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_tree_nodecounts", DateTime.Now, "it_tree_nodecounts", ed.ToString(), "it_tree_nodecounts", 2);
                return ed.ToString();
            }
        }

        public dynamic it_tree_link(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20it_tree_link%20where%20company_it%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "71164a70-56c2-c0e1-343c-b52e496dc12a");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_tree_link", DateTime.Now, "it_tree_link", ed.ToString(), "it_tree_link", 2);
                return ed.ToString();
            }
        }
        public dynamic it_tree_linkcounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "cf88e221-19f3-ed09-f9d0-9493fcc75bc9");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("application/x-www-form-urlencoded", "query=FOR%20EACH%20it_tree_link&table=it_tree_link&filter=where%20company_it%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_tree_linkcounts", DateTime.Now, "it_tree_linkcounts", ed.ToString(), "it_tree_linkcounts", 2);
                return ed.ToString();
            }
        }

        public dynamic it_prodline(string Api, string Authorization, string company, int pageno)
        {
            try
            {
                int take = 500;
                int skip = ((pageno - 1) * take);
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20it_prodline%20where%20company_it%3D%22" + company + "%22%20&skip=" + skip + "&take=" + take);
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "ccd944d5-730c-bf8b-c286-70bbc361af68");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_prodline", DateTime.Now, "it_prodline", ed.ToString(), "it_prodline", 2);
                return ed.ToString();
            }
        }
        public dynamic it_prodlinecounts(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "bcf4e993-1a66-4a52-f46f-a14452c46394");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("application/x-www-form-urlencoded", "query=FOR%20EACH%20it_prodline&table=it_prodline&filter=where%20company_it%3D%22" + company + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_prodlinecounts", DateTime.Now, "it_prodlinecounts", ed.ToString(), "it_prodlinecounts", 2);
                return ed.ToString();
            }
        }

        public dynamic it_price_pct(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20it_price_pct%20WHERE%20it_price_pct.company_it%20%3D%20%22" + company + "%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "ed0703ae-4612-96ad-4598-caf52721cc97");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_price_pct", DateTime.Now, "it_price_pct", ed.ToString(), "it_price_pct", 2);
                return ed.ToString();
            }
        }

        public dynamic it_majcls(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20it_majcls%20WHERE%20it_majcls.company_it%20%3D%20%22" + company + "%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "e531fa48-13d1-92d1-60cd-b71af7bd10b2");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "it_majcls", DateTime.Now, "it_majcls", ed.ToString(), "it_majcls", 2);
                return ed.ToString();
            }
        }

        public dynamic ip_web_ln(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20ip_web_ln%20WHERE%20ip_web_ln.company_ip%20%3D%20%22" + company + "%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "3ea983f3-ff73-0cb6-aa96-5be07995d292");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "ip_web_ln", DateTime.Now, "ip_web_ln", ed.ToString(), "ip_web_ln", 2);
                return ed.ToString();
            }
        }

        public dynamic ip_web_hd(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20ip_web_hd%20WHERE%20ip_web_hd.company_ip%20%3D%20%22" + company + "%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "b57377ca-5d11-097c-2194-f089c8d5b688");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "ip_web_hd", DateTime.Now, "ip_web_hd", ed.ToString(), "ip_web_hd", 2);
                return ed.ToString();
            }
        }

        public dynamic profile(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20profile");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "c3c11a3c-0249-c158-3776-05c8f801ce16");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "profile", DateTime.Now, "profile", ed.ToString(), "profile", 2);
                return ed.ToString();
            }
        }
        public dynamic PendingOrders(string Api, string Authorization, string company, string customer, int pageno, int orderby, string stat = "oe", string opn = "yes")
        {
            try
            {
                int skip = ((pageno - 1) * 10);
                int take = 10;
                var client = new RestClient();
                if (orderby == 1)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%20%22" + customer + "%22%20and%20%20oe_head.stat%20=%20%22" + stat + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20and%20%20oe_head.order%20%3E=%20%220%22%20BREAK%20BY%20oe_head.ord_date%20desc%20%20&skip=" + skip + "&take=" + take);
                }
                else if (orderby == 2)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%20%22" + customer + "%22%20and%20%20oe_head.stat%20=%20%22" + stat + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20and%20%20oe_head.order%20%3E=%20%220%22%20BREAK%20BY%20oe_head.ord_date%20&skip=" + skip + "&take=" + take);
                }
                else if (orderby == 3)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%20%22" + customer + "%22%20and%20%20oe_head.stat%20=%20%22" + stat + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20and%20%20oe_head.order%20%3E=%20%220%22%20BREAK%20BY%20oe_head.wanted_date%20desc%20&skip=" + skip + "&take=" + take);
                }
                else if (orderby == 4)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%20%22" + customer + "%22%20and%20%20oe_head.stat%20=%20%22" + stat + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20and%20%20oe_head.order%20%3E=%20%220%22%20BREAK%20BY%20oe_head.wanted_date%20&skip=" + skip + "&take=" + take);
                }
                else if (orderby == 5)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%20%22" + customer + "%22%20and%20%20oe_head.stat%20=%20%22" + stat + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20and%20%20oe_head.order%20%3E=%20%220%22%20BREAK%20BY%20oe_head.order%20desc%20&skip=" + skip + "&take=" + take);
                }
                else if (orderby == 6)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%20%22" + customer + "%22%20and%20%20oe_head.stat%20=%20%22" + stat + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20and%20%20oe_head.order%20%3E=%20%220%22%20BREAK%20BY%20oe_head.order%20&skip=" + skip + "&take=" + take);
                }
                else
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%20%22" + customer + "%22%20and%20%20oe_head.stat%20=%20%22" + stat + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20and%20%20oe_head.order%20%3E=%20%220%22%20BREAK%20BY%20oe_head.ord_date%20desc%20%20&skip=" + skip + "&take=" + take);
                }

                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "0248b11b-ffd8-4894-a020-9f2939314381");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "PendingOrders", DateTime.Now, "PendingOrders", ed.ToString(), "PendingOrders", 2);
                return ed.ToString();
            }
        }
        public dynamic PendingOrdersCounts(string Api, string Authorization, string company, string customer, string stat = "oe", string opn = "yes")
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "4a66965a-f872-4700-901f-a21bf3cc7aaf");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20%3D%20%22" + company + "%22%20and%20%20oe_head.customer%20%3D%20%22" + customer + "%22%20and%20%20oe_head.stat%20%3D%20%22" + stat + "%22%20and%20%20oe_head.opn%20%3D%20%22" + opn + "%22%20and%20%20oe_head.order%20%3E%3D%20%220%22&table=oe_head&filter=oe_head.company_oe%20%3D%20%22" + company + "%22%20and%20%20oe_head.customer%20%3D%20%22" + customer + "%22%20and%20%20oe_head.stat%20%3D%20%22" + stat + "%22%20and%20%20oe_head.opn%20%3D%20%22" + opn + "%22%20and%20%20oe_head.order%20%3E%3D%20%220%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "PendingOrdersCounts", DateTime.Now, "PendingOrdersCounts", ed.ToString(), "PendingOrdersCounts", 2);
                return ed.ToString();
            }
        }
        public dynamic OrderView(string Api, string Authorization, string company, string order, string rec_type = "O")
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.order%20=%20%22" + order + "%22%20and%20%20oe_head.rec_type%20=%20%22" + rec_type + "%22,%20%0AEACH%20oe_line%20NO-LOCK%20WHERE%20%0Aoe_line.order%20=%20oe_head.order%20AND%20%0Aoe_line.rec_type%20=%20oe_head.rec_type%20AND%20%0Aoe_line.rec_seq%20=%20oe_head.rec_seq%20AND%20%0Aoe_line.company_oe%20=%20oe_head.company_oe&columns=oe_head.%2A,oe_line.%2A");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "2415d9c9-6d9e-49e6-8eb8-8cc0525bcf50");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "OrderView", DateTime.Now, "OrderView", ed.ToString(), "OrderView", 2);
                return ed.ToString();
            }
        }
        public dynamic OpenInvoice(string Api, string Authorization, string company, string customer, int pageno, int oderby, string rec_type = "i", string opn = "yes")
        {
            try
            {
                int skip = ((pageno - 1) * 10);
                int take = 10;
                var client = new RestClient();
                if (oderby == 1)
                {
                    //ord_date  desc
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%22" + customer + "%22%20%20and%20%20oe_head.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20BREAK%20BY%20oe_head.ord_date%20desc%20&skip=" + skip + "&take=" + take);
                }
                else if (oderby == 2)
                {
                    //ord_date  ASC
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%22" + customer + "%22%20%20and%20%20oe_head.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20BREAK%20BY%20oe_head.ord_date%20&skip=" + skip + "&take=" + take);
                }
                else if (oderby == 3)
                {
                    //wanted_date  desc
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%22" + customer + "%22%20%20and%20%20oe_head.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20BREAK%20BY%20oe_head.wanted_date%20desc%20&skip=" + skip + "&take=" + take);
                }
                else if (oderby == 4)
                {
                    //wanted_date  ASC
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%22" + customer + "%22%20%20and%20%20oe_head.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20BREAK%20BY%20oe_head.wanted_date%20&skip=" + skip + "&take=" + take);
                }
                else if (oderby == 5)
                {
                    //Order  desc
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%22" + customer + "%22%20%20and%20%20oe_head.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20BREAK%20BY%20oe_head.order%20desc%20&skip=" + skip + "&take=" + take);
                }
                else if (oderby == 6)
                {
                    //Order  ASC
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%22" + customer + "%22%20%20and%20%20oe_head.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20BREAK%20BY%20oe_head.order%20&skip=" + skip + "&take=" + take);
                }
                else
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%22" + customer + "%22%20%20and%20%20oe_head.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_head.opn%20=%20%22" + opn + "%22%20BREAK%20BY%20oe_head.ord_date%20desc%20&skip=" + skip + "&take=" + take);
                }

                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "277c688d-9271-45f2-aa50-88a8ad94d64a");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "OpenInvoice", DateTime.Now, "OpenInvoice", ed.ToString(), "OpenInvoice", 2);
                return ed.ToString();
            }
        }
        public dynamic OpenInvoiceCounts(string Api, string Authorization, string company, string customer, string rec_type = "i", string opn = "yes")
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "76550a9c-d9ec-4b7d-ba83-eecfcf870007");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20%3D%20%22" + company + "%22%20and%20%20oe_head.customer%20%3D%22" + customer + "%22%20%20and%20%20oe_head.rec_type%20%3D%20%22" + rec_type + "%22%20and%20%20oe_head.opn%20%3D%20%22" + opn + "%22&table=oe_head&filter=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20%3D%20%22" + customer + "%22%20and%20%20oe_head.customer%20%3D%22" + customer + "%22%20%20and%20%20oe_head.rec_type%20%3D%20%22" + rec_type + "%22%20and%20%20oe_head.opn%20%3D%20%22" + opn + "%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "OpenInvoiceCounts", DateTime.Now, "OpenInvoiceCounts", ed.ToString(), "OpenInvoiceCounts", 2);
                return ed.ToString();
            }
        }
        public dynamic Back_Order_Items(string Api, string Authorization, string company, string customer, int pageno, int orderby, string rec_type = "O")
        {
            try
            {
                int skip = ((pageno - 1) * 10);
                int take = 10;
                var client = new RestClient();
                if (orderby == 1)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_line%20NO-LOCK%20WHERE%20%20oe_line.company_oe%20=%20%22" + company + "%22%20and%20%20oe_line.customer%20=%20%22" + customer + "%22%20and%20%20oe_line.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_line.blanket_order=true%20and%20oe_line.q_bo_d%20%3E%20%220%22%20BREAK%20BY%20oe_line.req_date%20desc%20&skip=" + skip + "&take=" + take);
                }
                else if (orderby == 2)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_line%20NO-LOCK%20WHERE%20%20oe_line.company_oe%20=%20%22" + company + "%22%20and%20%20oe_line.customer%20=%20%22" + customer + "%22%20and%20%20oe_line.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_line.blanket_order=true%20and%20oe_line.q_bo_d%20%3E%20%220%22%20BREAK%20BY%20oe_line.req_date%20&skip=" + skip + "&take=" + take);
                }
                else if (orderby == 3)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_line%20NO-LOCK%20WHERE%20%20oe_line.company_oe%20=%20%22" + company + "%22%20and%20%20oe_line.customer%20=%20%22" + customer + "%22%20and%20%20oe_line.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_line.blanket_order=true%20and%20oe_line.q_bo_d%20%3E%20%220%22%20BREAK%20BY%20oe_line.order%20desc%20&skip=" + skip + "&take=" + take);
                }
                else if (orderby == 4)
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_line%20NO-LOCK%20WHERE%20%20oe_line.company_oe%20=%20%22" + company + "%22%20and%20%20oe_line.customer%20=%20%22" + customer + "%22%20and%20%20oe_line.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_line.blanket_order=true%20and%20oe_line.q_bo_d%20%3E%20%220%22%20BREAK%20BY%20oe_line.order%20&skip=" + skip + "&take=" + take);
                }
                else
                {
                    client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_line%20NO-LOCK%20WHERE%20%20oe_line.company_oe%20=%20%22" + company + "%22%20and%20%20oe_line.customer%20=%20%22" + customer + "%22%20and%20%20oe_line.rec_type%20=%20%22" + rec_type + "%22%20and%20%20oe_line.blanket_order=true%20and%20oe_line.q_bo_d%20%3E%20%220%22%20BREAK%20BY%20oe_line.req_date%20desc%20&skip=" + skip + "&take=" + take);
                }
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "2ab9c86b-c923-4cab-b56c-0aba2d3c6a53");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Back_Order_Items", DateTime.Now, "Back_Order_Items", ed.ToString(), "Back_Order_Items", 2);
                return ed.ToString();
            }
        }

        public dynamic Back_Order_ItemsCounts(string Api, string Authorization, string company, string customer, string rec_type = "O")
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/count");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "b7c430b0-f483-4a74-9b55-6cf4e6ec5fea");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20oe_line%20NO-LOCK%20WHERE%20%20oe_line.company_oe%20%3D%20%22" + company + "%22%20and%20%20oe_line.customer%20%3D%20%22" + customer + "%22%20and%20%20oe_line.blanket_order=true%20and%20oe_line.rec_type%20%3D%20%22" + rec_type + "%22%20and%20%20%20oe_line.q_bo_d%20%3E%20%220%22&table=oe_line&filter=FOR%20EACH%20oe_line%20NO-LOCK%20WHERE%20%20oe_line.company_oe%20%3D%20%22" + company + "%22%20and%20%20oe_line.customer%20%3D%20%22" + customer + "%22%20and%20%20oe_line.blanket_order=true%20and%20oe_line.rec_type%20%3D%20%22" + rec_type + "%22%20and%20%20%20oe_line.q_bo_d%20%3E%20%220%22", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Back_Order_ItemsCounts", DateTime.Now, "Back_Order_ItemsCounts", ed.ToString(), "Back_Order_ItemsCounts", 2);
                return ed.ToString();
            }
        }
        public dynamic OrderHistory(string Api, string Authorization, string company, string customer, string Order, string pono, string itemnum, string sdate, string edate, int pageno, int orderby)
        {
            try
            {

                string squery = string.Empty;

                string orderbyword = string.Empty;
                string takeskip = string.Empty;
                squery = Api + "/distone/rest/service/data/read?query=FOR%20EACH%20oe_head%20NO-LOCK%20WHERE%20%20oe_head.company_oe%20=%20%22" + company + "%22%20and%20%20oe_head.customer%20=%20%22" + customer + "%22%20and%20%20oe_head.rec_type%20=%20%22o%22%20";
                if (!string.IsNullOrEmpty(Order))
                {
                    squery = squery + "and%20oe_head.order=" + Order + "%20";
                }
                if (!string.IsNullOrEmpty(pono))
                {
                    squery = squery + "and%20oe_head.cu_po=%22" + pono + "%22%20";
                }
                if (!string.IsNullOrEmpty(sdate) && !string.IsNullOrEmpty(edate))
                {
                    squery = squery + "and%20%20oe_head.ord_date%20%3E=%22" + sdate + "%22%20and%20%20oe_head.ord_date%20%3C=%22" + edate + "%22";
                }
                squery = squery + ",%20%20EACH%20oe_line%20NO-LOCK%20WHERE%20%0Aoe_line.order%20=%20oe_head.order%20AND%20%0Aoe_line.rec_type%20=%20oe_head.rec_type%20AND%20%0Aoe_line.rec_seq%20=%20oe_head.rec_seq%20AND%20%0Aoe_line.company_oe%20=%20oe_head.company_oe%20";
                if (!string.IsNullOrEmpty(itemnum))
                {
                    squery = squery + "and%20oe_head.cu_po=%22" + pono + "%22%20";
                }
                if (orderby == 1)
                {
                    squery = squery + "BREAK%20BY%20oe_head.ord_date%20desc%20";
                }
                else if (orderby == 2)
                {
                    squery = squery + "BREAK%20BY%20oe_head.ord_date%20";
                }
                else if (orderby == 3)
                {
                    squery = squery + "BREAK%20BY%20oe_head.wanted_date%20desc%20";
                }
                else if (orderby == 4)
                {
                    squery = squery + "BREAK%20BY%20oe_head.wanted_date%20";
                }
                else if (orderby == 5)
                {
                    squery = squery + "BREAK%20BY%20oe_head.order%20desc%20";
                }
                else if (orderby == 6)
                {
                    squery = squery + "BREAK%20BY%20oe_head.order%20";
                }
                else
                {
                    squery = squery + "BREAK%20BY%20oe_head.ord_date%20desc%20";
                }
                if (pageno > 0)
                {
                    int skip = ((pageno - 1) * 10);
                    int take = 10;
                    squery = squery + "&skip=" + skip + "&take=" + take;
                }

                squery = squery + "&columns=oe_head.%2A";
                var client = new RestClient(squery);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "918bdac3-0b21-4456-a5e3-2ef4f7382fac");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "OrderHistory", DateTime.Now, "OrderHistory", ed.ToString(), "OrderHistory", 2);
                return ed.ToString();
            }
        }
        public dynamic OrderHistorycounts(string Api, string Authorization, string company, string customer)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "ee1dd564-bdec-4e68-9bdd-d53b8e89cc47");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("undefined", "query=FOR%20EACH%20oe_head%20NO-LOCK&table=oe_head&filter=oe_head.company_oe%20%3D%20%22" + company + "%22%20and%20%20oe_head.customer%20%3D%20%22" + customer + "%22%20and%20%20oe_head.rec_type%20%3D%20%22o%22%20", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject<countviewmodel>(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "OrderHistorycounts", DateTime.Now, "OrderHistorycounts", ed.ToString(), "OrderHistorycounts", 2);
                return ed.ToString();
            }
        }

        public dynamic PurchaseHistory(string Api, string Authorization, string company, string sum_year = "2013", string sum_month = "0", string customer = "mark")
        {
            try
            {
                string query = string.Empty;
                query = query + Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sum%20NO-LOCK%20WHERE%20%20sum.company_sa%20=%20%22" + company + "%22%20and%20sum.sum_name%20=%22customer_item_monthly%22%20and";
                if (!string.IsNullOrEmpty(sum_year))
                {
                    query = query + "%20sum.sum_year=" + sum_year + "%20and";
                }
                if (!string.IsNullOrEmpty(sum_month))
                {
                    query = query + "%20sum.sum_month=" + sum_month + "%20and";
                }
                query = query + "%20sum.sum_key_1%20=%20%22" + customer + "%22";
                var client = new RestClient(query);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "ff4754dd-d789-4948-bda3-6e3483dccce7");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "PurchaseHistory", DateTime.Now, "PurchaseHistory", ed.ToString(), "PurchaseHistory", 2);
                return ed.ToString();
            }
        }



        public dynamic Orderfinalize(string Api, string Authorization, string order, string type)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/order/finalize?order=" + order + "&type=" + type);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Postman-Token", "92d2fd1a-59bc-4839-bdd9-93c2d7940f5c");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Orderfinalize", DateTime.Now, "Orderfinalize", ed.ToString(), "Orderfinalize", 2);
                return ed.ToString();
            }
        }




        public dynamic orderupdate(string Api, string Authorization, string order, string type, string ship_id)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/order/update");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "6a49be22-f263-aba0-9538-0af0bfc3b06c");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("application/json", "{\n\t\"order\":" + order + ",\n\t\"type\": \"" + type + "\",\n\t\"changes\":{\n\t\t\"cu_po\":\"PO change\",\n\t\t\"ship_id\": \"" + ship_id + "\"\n\t}\n\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "orderupdate", DateTime.Now, "orderupdate", ed.ToString(), "orderupdate", 2);
                return ed.ToString();
            }
        }

        public dynamic imagefetchcategory(string Api, string Authorization, string category, string key, string seq, string max_width, string max_height)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/image/fetch?category=" + category + "&key=" + key + "%2C" + seq + "&max_width=" + max_width + "&max_height=" + max_height);
                var request = new RestRequest(Method.GET);
                request.AddHeader("postman-token", "29577f4e-bd05-f2bd-cca3-0cc771440deb");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "imagefetchcategory", DateTime.Now, "imagefetchcategory", ed.ToString(), "imagefetchcategory", 2);
                return ed.ToString();
            }
        }

        public dynamic orderlinesRemove(string Api, string Authorization, string order, string type, string seq)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/order/lines");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "45877d6c-dd0b-b58a-45b0-f06b1797a44f");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", Authorization);
                request.AddParameter("application/json", "{\n\t\"order\":" + order + ",\n\t\"type\": \"" + type + "\",\n\t\"remove\":[" + seq + "]\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "orderlinesRemove", DateTime.Now, "orderlinesRemove", ed.ToString(), "orderlinesRemove", 2);
                return ed.ToString();
            }
        }
        public dynamic sy_prof_label(string Api, string Authorization, string company)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_prof_label%20NO-LOCK%20WHERE%20sy_prof_label.company_sy%20=%20%22" + company + "%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "efdf097d-fd9d-424e-949f-17340f7b5c8d");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);

                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "sy_prof_label", DateTime.Now, "sy_prof_label", ed.ToString(), "sy_prof_label", 2);
                return ed.ToString();
            }
        }

        public dynamic GetItemNotes(string Api, string Authorization, string company, string keys)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_master_notes%20NO-LOCK%20WHERE%20sy_master_notes.company_sy%20%3D%20%22" + company + "%22%20and%20%20sy_master_notes.table_key%20%3D%20%22" + keys + "%22%20and%20sy_master_notes.disp_where%20%3D%20%22web%22&columns=note_type%2Ctable_key%2Ccompany_sy%2Cdisp_method%2Cdisp_where%2Cnote%2Cnote_image%2Ctech_doc_type%2Clast_update");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "76e44f43-5bf6-fc8f-c1d0-b29984c6c465");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetItemNotes", DateTime.Now, "GetItemNotes", ed.ToString(), "GetItemNotes", 2);
                return ed.ToString();
            }
        }
        public dynamic Sy_States(string Api, string Authorization)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_codes%20NO-LOCK%20%20where%20code_type=%22sy_state%22");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Postman-Token", "2bb5c50f-5c37-459c-ac50-172ac7bdb700");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Sy_States", DateTime.Now, "Sy_States", ed.ToString(), "Sy_States", 2);
                return ed.ToString();
            }
        }

        public dynamic itemsystemnotes(string Api, string Authorization, string company, string keys)
        {
            try
            {
                var client = new RestClient(Api + "/distone/rest/service/data/read?query=FOR%20EACH%20sy_master_notes%20NO-LOCK%20WHERE%20sy_master_notes.company_sy%20%3D%20%22" + company + "%22%20and%20sy_master_notes.disp_where%20%3D%20%22web%22&columns=note_type%2Ctable_key%2Ccompany_sy%2Cdisp_method%2Cdisp_where%2Cnote%2Cnote_image%2Ctech_doc_type");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "99c756e5-b6b2-2bf9-a167-ea4daac57ad6");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("authorization", Authorization);
                IRestResponse response = client.Execute(request);
                return JsonConvert.DeserializeObject(response.Content);
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "itemsystemnotes", DateTime.Now, "itemsystemnotes", ed.ToString(), "itemsystemnotes", 2);
                return ed.ToString();
            }
        }


        public bool AddConfigurationforapi(string UserMemRefNo, string ApiEndPoint, string AuthonticationToken, string client, string company, string username, string password)
        {
            bool flag = false;
            try
            {
                var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo));
                CompanySettingForMulti addnewcomp = new CompanySettingForMulti();
                addnewcomp.ApiEndPoint = ApiEndPoint;
                addnewcomp.AuthonticationToken = AuthonticationToken;
                addnewcomp.client = client;
                addnewcomp.company = company;
                addnewcomp.password = password;
                addnewcomp.username = username;
                Context2.CompanySettingForMultis.Add(addnewcomp);
                Context2.SaveChanges();
                flag = true;
            }
            catch (Exception ed)
            {
                flag = false;
            }
            return flag;
        }


        public bool AddConfiguations(string UserMemRefNo, string Key, string keyvalue)
        {
            bool flag = false;
            try
            {

                var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo));
                var getoldrec = Context2.Configs.Where(cp => cp.ConfigKey.Equals(Key)).FirstOrDefault();
                if (getoldrec != null)
                {
                    D1WebApp.ClientModel.Config configsr = new Config();
                    configsr.ConfigId = getoldrec.ConfigId;
                    configsr.ConfigKey = getoldrec.ConfigKey;
                    configsr.ConfigValue = keyvalue;
                    configsr.descr = getoldrec.descr;
                    Context2.Entry(getoldrec).CurrentValues.SetValues(configsr);
                    Context2.SaveChanges();
                }
                else
                {
                    D1WebApp.ClientModel.Config configsr = new Config();
                    var getrec111 = Context2.Configs.OrderByDescending(cp => cp.ConfigId).FirstOrDefault();
                    configsr.ConfigId = (getrec111 != null ? getrec111.ConfigId : 0) + 1;
                    configsr.ConfigKey = Key;
                    configsr.ConfigValue = keyvalue;
                    configsr.descr = Key;
                    Context2.Configs.Add(configsr);
                    Context2.SaveChanges();
                }


            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "AddConfiguations", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + ed.InnerException, "AddConfiguations", 1);
                flag = false;

            }
            return flag;
        }



        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
    }
}