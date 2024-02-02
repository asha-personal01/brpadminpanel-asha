//Developed by Nayan

using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Linq;
using System.Web;
using D1WebApp.Models;
using D1WebApp.Utilities;
using D1WebApp.Utilities.GeneralConfiguration;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Validation;

using D1WebApp.ViewModels;
using D1WebApp.Utilities.Classes;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class ApiForVendorPortalRepository : IApiForVendorPortalRepository, IDisposable
    {
        private D1WebAppEntities context;
        public ApiForVendorPortalRepository(D1WebAppEntities context)
        {
            this.context = context;
        }
        public dynamic GetToken(long UserID)
        {
            try
            {
                WebApiRequest w1 = new WebApiRequest();
                var getclient = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("client");
                var getuser = context.Users.Find(UserID);
                if (getuser.IsActive)
                {
                    return w1.GetAuthonticationToken(getclient.ConfigurationValue, getuser.CompanyID, getuser.ApiUserName, getuser.ApiPwd, getuser.ApiEndPoint);
                }
                else
                {
                    return "User is Deactivated";
                }
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetTokenrepo", DateTime.Now, "GetToken", ed.ToString(), "GetToken", 2);
                return ed.ToString();
            }
        }

        public dynamic getPOList(long UserID, string RecType, string vendor, int PageNo, string PO, string PartNo, string VendorPart, string UPC, string Date)
        {
            try
            {
                WebApiRequest w1 = new WebApiRequest();
                var getuser = context.Users.Find(UserID);
                if (getuser.IsActive)
                {
                    return w1.getPOList(getuser.ApiEndPoint, getuser.CompanyID, RecType, vendor, getuser.ApiToken, PO, PartNo, VendorPart, UPC, Date, PageNo);
                }
                else
                {
                    return "User is Deactivated";
                }
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getPOListrepo", DateTime.Now, "getPOListrepo", ed.ToString(), "getPOListrepo", 2);
                return ed.ToString();
            }
        }

        public dynamic SubmitPO(List<POViewModel> poModel)
        {
            try
            {
                WebApiRequest w1 = new WebApiRequest();
                var getuser = context.Users.Find(poModel[0].UserID);
                if (getuser.IsActive)
                {
                    return w1.SubmitPO(getuser.ApiEndPoint, getuser.ApiToken, getuser.CompanyID, poModel);
                }
                else
                {
                    return "User is Deactivated";
                }
            }catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "SubmitPOrepo", DateTime.Now, "SubmitPOrepo", ed.ToString(), "SubmitPOrepo", 2);
                return ed.ToString();
            }
        }

        public dynamic getItemList(long UserID, string vendor, string Item, string VendorItem, string Warehouse, int PageNo)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(UserID);
            if (getuser.IsActive)
            {
                return w1.getItemList(getuser.ApiEndPoint, getuser.CompanyID, vendor, Item, VendorItem, Warehouse, getuser.ApiToken, PageNo);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getItemListrepo", DateTime.Now, "getItemListrepo", ed.ToString(), "getItemListrepo", 2);
                return ed.ToString();
            }
        }
        public dynamic getItemListCount(long UserID, string vendor, string Item, string VendorItem, string Warehouse)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(UserID);
            if (getuser.IsActive)
            {
                return w1.getItemListCount(getuser.ApiEndPoint, getuser.CompanyID, vendor, Item, VendorItem, Warehouse, getuser.ApiToken);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getItemListCountrepo", DateTime.Now, "getItemListCountrepo", ed.ToString(), "getItemListCountrepo", 2);
                return ed.ToString();
            }
        }

        public dynamic getPOListCount(long UserID, string RecType, string vendor)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(UserID);
            if (getuser.IsActive)
            {
                return w1.getPOListCount(getuser.ApiEndPoint, getuser.CompanyID, RecType, vendor, getuser.ApiToken);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getPOListCountrepo", DateTime.Now, "getPOListCountrepo", ed.ToString(), "getPOListCountrepo", 2);
                return ed.ToString();
            }
        }

        public dynamic getProductLinePO(long UserID, string RecType, string vendor, int PageNo)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(UserID);
            if (getuser.IsActive)
            {
                return w1.getProductLinePO(getuser.ApiEndPoint, getuser.ApiToken, getuser.CompanyID, RecType, vendor, PageNo);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getProductLinePOrepo", DateTime.Now, "getProductLinePOrepo", ed.ToString(), "getProductLinePOrepo", 2);
                return ed.ToString();
            }
        }

        public dynamic getProductLinePOCount(long UserID, string RecType, string vendor)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(UserID);
            if (getuser.IsActive)
            {
                return w1.getProductLinePOCount(getuser.ApiEndPoint, getuser.ApiToken, getuser.CompanyID, RecType, vendor);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getProductLinePOCountrepo", DateTime.Now, "getProductLinePOCountrepo", ed.ToString(), "getProductLinePOCountrepo", 2);
                return ed.ToString();
            }
        }

        public dynamic getPoDtl(long UserID, string vendor, string PONO)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(UserID);
            if (getuser.IsActive)
            {
                return w1.getPoDtl(getuser.ApiEndPoint, getuser.ApiToken, getuser.CompanyID, vendor, PONO);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getPoDtlrepo", DateTime.Now, "getPoDtlrepo", ed.ToString(), "getPoDtlrepo", 2);
                return ed.ToString();
            }
        }

        public dynamic InsertTrackingNo(POTrackingViewModel poTrackingNo)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(Convert.ToInt32(poTrackingNo.UserID));
            if (getuser.IsActive)
            {
                return w1.InsertTrackingNo(getuser.ApiEndPoint, getuser.ApiToken, getuser.CompanyID, poTrackingNo.Manifest_id, poTrackingNo.Order, poTrackingNo.rec_type, poTrackingNo.tracking_no, poTrackingNo.manifest_carrier, poTrackingNo.service_type, poTrackingNo.ship_via_code, poTrackingNo.ship_date, poTrackingNo.pkg_wgt, poTrackingNo.pkg_no, poTrackingNo.pack_type, poTrackingNo.descr);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "InsertTrackingNorepo", DateTime.Now, "InsertTrackingNorepo", ed.ToString(), "InsertTrackingNorepo", 2);
                return ed.ToString();
            }
        }

        public dynamic GetTrackingNo(string UserID, string ManifiestId)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(Convert.ToInt32(UserID));
            if (getuser.IsActive)
            {
                return w1.GetTrackingNumber(getuser.ApiEndPoint, getuser.ApiToken, getuser.CompanyID, ManifiestId);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetTrackingNorepo", DateTime.Now, "GetTrackingNorepo", ed.ToString(), "GetTrackingNorepo", 2);
                return ed.ToString();
            }
        }

        public dynamic Itemavailability(string UserID, string item, string warehouse, string format)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(Convert.ToInt32(UserID));
            if (getuser.IsActive)
            {
                return w1.Itemavailability(getuser.ApiToken, getuser.ApiEndPoint, item, warehouse, format);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Itemavailabilityrepo", DateTime.Now, "Itemavailabilityrepo", ed.ToString(), "Itemavailabilityrepo", 2);
                return ed.ToString();
            }
        }

        public dynamic SearchByPart(long UserID, string RecType, string Vendor, string sword)
        {
            try { 
            WebApiRequest w1 = new WebApiRequest();
            var getuser = context.Users.Find(UserID);
            if (getuser.IsActive)
            {
                return w1.SearchByPart(getuser.ApiEndPoint, getuser.CompanyID, RecType, Vendor, getuser.ApiToken, sword);
            }
            else
            {
                return "User is Deactivated";
            }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "SearchByPartrepo", DateTime.Now, "SearchByPartrepo", ed.ToString(), "SearchByPartrepo", 2);
                return ed.ToString();
            }
        }

        public bool AddSchedulerConfig(SchedulerConfigViewModel schedulerConfigVM)
        {

            bool flag = false;
            try
            {
                var getoldrecord = context.SchedulerConfigs.Where(cp => cp.SchedulerName.Equals(schedulerConfigVM.SchedulerName) && cp.UserId == schedulerConfigVM.UserId).FirstOrDefault();
                if (getoldrecord != null)
                {
                    getoldrecord.SchedulerTime = schedulerConfigVM.SchedulerTime;
                    getoldrecord.SchedulerTables = schedulerConfigVM.SchedulerTables;
                    getoldrecord.Active = schedulerConfigVM.Active;
                    context.Entry(getoldrecord).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                else
                {
                    SchedulerConfig schedulerConfig = new SchedulerConfig();
                    schedulerConfig = SchedulerConfigViewModel.ConvertToModel(schedulerConfigVM);
                    context.SchedulerConfigs.Add(schedulerConfig);
                    context.SaveChanges();
                }
                flag = true;
            }
            catch (Exception ed) {
                ErrorLogs.ErrorLog(0, "AddSchedulerConfigrepo", DateTime.Now, "AddSchedulerConfigrepo", ed.ToString(), "AddSchedulerConfigrepo", 2);
                flag = false; }
            return flag;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}