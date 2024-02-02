//Developed by Nayan

using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.Utilities;
using D1WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IApiForVendorPortalRepository : IDisposable
    {
        dynamic GetToken(long UserID);
        dynamic getPOList(long UserID, string RecType, string vendor, int PageNo, string PO, string PartNo, string VendorPart, string UPC, string Date);
        dynamic getProductLinePO(long UserID, string RecType, string vendor, int PageNo);
        dynamic getPoDtl(long UserID, string vendor, string PONO);
        dynamic SearchByPart(long UserID, string RecType, string Vendor, string sword);
        dynamic getPOListCount(long UserID, string RecType, string vendor);
        dynamic getProductLinePOCount(long UserID, string RecType, string vendor);
        dynamic InsertTrackingNo(POTrackingViewModel poTrackingNo);
        dynamic GetTrackingNo(string UserID, string ManifiestId);
        dynamic getItemList(long UserID, string vendor, string Item, string VendorItem, string Warehouse, int PageNo);
        dynamic getItemListCount(long UserID, string vendor, string Item, string VendorItem, string Warehouse);
        dynamic Itemavailability(string UserID, string item, string warehouse, string format);
        dynamic SubmitPO(List<POViewModel> poModel);
        bool AddSchedulerConfig(SchedulerConfigViewModel schedulerConfig);
    }
}