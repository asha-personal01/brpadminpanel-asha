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
using System.Text.RegularExpressions;
using System.DirectoryServices;
using Microsoft.Web.Administration;
using D1WebApp.ViewModels;
using D1WebApp.Utilities.Classes;
using D1WebApp.DataAccessLayer.Repositories;
using System.Net.Http;
using System.Net;

namespace D1WebApp.BussinessLogicLayer.Controllers
{
#if !DEBUG
      [System.Web.Mvc.RequireHttps]
#endif


    public class EcommerceController : ApiController
    {
        public UserManager usermanager = new UserManager();
        private IApiForVendorPortalRepository ApiForVendorPortalRepository;
        public EcommerceController()
        {
            ApiForVendorPortalRepository = new ApiForVendorPortalRepository(new D1WebAppEntities());
        }

        [HttpPost]
        public bool SendOfferDetailsMail(PurchaseOrderDetailsViewModel PurchaseOrderDetails)
        {
            return D1WebApp.Utilities.Mail.SendOfferDetailsMail(PurchaseOrderDetails);
        }

        [HttpGet]
        public dynamic GetToken(long UserID)
        {
            string token = string.Empty;
            token = ApiForVendorPortalRepository.GetToken(UserID);
            usermanager.InsertNewTokenToUser(UserID, token);
            return token;
        }
        [HttpGet]
        public dynamic SyncVendorLoginDetails(long UserID)
        {
            bool flag = false;
            try
            {
                var Getuser = usermanager.GetUserByID(UserID);
                WebApiRequest w1 = new WebApiRequest();
                List<VendorUserLoginDetail> VendorUserLoginList = VendorLoginDetailsViewModel.ConvertToModelListForUser(w1.GetVendorLoginDetails(Getuser.CompanyID, Getuser.toaken, Getuser.ApiEndPoint), Getuser.UserID, Getuser.CompanyID);
                usermanager.UpdateVendorUserLoginDetails(VendorUserLoginList);
                flag = true;
            }
            catch (Exception ed)
            {
                flag = false;
            }
            return flag;
        }


        [HttpGet]
        public dynamic SyncNowOption(string memRefNo)
        {
            var getuser = usermanager.GetUserAlldataByMemrefno(memRefNo);
            var project = usermanager.GetProjectbyUserid(getuser.UserID);
            if (project.ProjectTypeID == 2)
            {
                string token = string.Empty;
                token = ApiForVendorPortalRepository.GetToken(getuser.UserID);
                usermanager.InsertNewTokenToUser(getuser.UserID, token);
                WebApiRequest w1 = new WebApiRequest();
                List<VendorUserLoginDetail> VendorUserLoginList = VendorLoginDetailsViewModel.ConvertToModelListForUser(w1.GetVendorLoginDetails(getuser.CompanyID, token, getuser.ApiEndPoint), getuser.UserID, getuser.CompanyID);
                return usermanager.InsertVendorUserLoginDetailsForSync(VendorUserLoginList);
            }
            else if (project.ProjectTypeID == 1)
            {
                var getcon = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("domainpath");
                WebApiRequest w1 = new WebApiRequest();
                return w1.DataSyncNow(getcon.ConfigurationValue, getuser.MemberRefNo, DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString());
            }
            else
            {
                return false;
            }
        }










        [HttpPost]
        public dynamic PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string filename = httpRequest.Form["memRefNo"];
                        int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
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

                            string targetDirectory = targetURL.ConfigurationValue + filename;
                            string UIpath = targetDirectory + "\\" + filename + "api";
                            UIpath = UIpath + "\\Content\\item\\logo.png";
                            //  where you want to attach your imageurl
                            //if needed write the code to update the table
                            // var filePath = HttpContext.Current.Server.MapPath(UIpath);
                            //Userimage myfolder name where i want to save my image
                            postedFile.SaveAs(UIpath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                //var res = string.Format("some Message");
                dict.Add("error", ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }


        [HttpGet]
        public dynamic getPOList(long UserID, string RecType, string vendor, int pageno, string PO, string PartNo, string VendorPart, string UPC, string Date)
        {
            string error = string.Empty;
            try
            {
                var obj = ApiForVendorPortalRepository.getPOList(UserID, RecType, vendor, pageno, PO, PartNo, VendorPart, UPC, Date);
                try
                {
                    if (obj.ToString().Contains("Invalid Token"))
                    {
                        error = obj.ToString() + "Checked";
                        GetToken(UserID);
                    }
                }
                catch (Exception ed)
                {
                    ErrorLogs.ErrorLog(0, "getPOList", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "getPOList", 1);
                }
                return obj;
            }
            catch (Exception ed)
            {
                GetToken(UserID);
                return "Please Try again Later";
            }
        }

        [HttpPost]
        public dynamic SubmitPO(List<POViewModel> poModel)
        {
            string error = string.Empty;
            try
            {
                var obj = ApiForVendorPortalRepository.SubmitPO(poModel);
                try
                {
                    if (obj.ToString().Contains("Invalid Token"))
                    {
                        error = obj.ToString() + "Checked";
                        GetToken(poModel[0].UserID);
                    }
                }
                catch (Exception ed)
                {
                    ErrorLogs.ErrorLog(0, "SubmitPO", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "SubmitPO", 1);
                }
                return obj;
            }
            catch (Exception ed)
            {
                GetToken(poModel[0].UserID);
                return "Please Try again Later";
            }
        }

        [HttpGet]
        public dynamic getItemList(long UserID, string vendor, string Item, string VendorItem, string Warehouse, int PageNo)
        {
            string error = string.Empty;
            try
            {
                var obj = ApiForVendorPortalRepository.getItemList(UserID, vendor, Item, VendorItem, Warehouse, PageNo);
                try
                {
                    if (obj.ToString().Contains("Invalid Token"))
                    {
                        error = obj.ToString() + "Checked";
                        GetToken(UserID);
                    }
                }
                catch (Exception ed)
                {
                    ErrorLogs.ErrorLog(0, "getItemList", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "getItemList", 1);
                }
                return obj;
            }
            catch (Exception ed)
            {
                GetToken(UserID);
                return "Please Try again Later";
            }
        }

        [HttpGet]
        public dynamic getItemListCount(long UserID, string vendor, string Item, string VendorItem, string Warehouse)
        {
            string error = string.Empty;
            try
            {
                var obj = ApiForVendorPortalRepository.getItemListCount(UserID, vendor, Item, VendorItem, Warehouse);
                try
                {
                    if (obj.ToString().Contains("Invalid Token"))
                    {
                        error = obj.ToString() + "Checked";
                        GetToken(UserID);
                    }
                }
                catch (Exception ed)
                {
                    ErrorLogs.ErrorLog(0, "getItemList", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "getItemList", 1);
                }
                return obj;
            }
            catch (Exception ed)
            {
                GetToken(UserID);
                return "Please Try again Later";
            }
        }


        [HttpGet]
        public dynamic getPOListCount(long UserID, string RecType, string vendor)
        {
            string error = string.Empty;
            try
            {
                var obj = ApiForVendorPortalRepository.getPOListCount(UserID, RecType, vendor);
                try
                {
                    if (obj.ToString().Contains("Invalid Token"))
                    {
                        error = obj.ToString() + "Checked";
                        GetToken(UserID);
                    }
                }
                catch (Exception ed)
                {
                    ErrorLogs.ErrorLog(0, "getPOListCount", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "getPOListCount", 1);
                }
                return obj;
            }
            catch (Exception ed)
            {
                GetToken(UserID);
                return "Please Try again Later";
            }
        }

        [HttpGet]
        public dynamic getProductLinePO(long UserID, string RecType, string vendor, int PageNo)
        {
            string error = string.Empty;
            try
            {
                var obj = ApiForVendorPortalRepository.getProductLinePO(UserID, RecType, vendor, PageNo);
                try
                {
                    if (obj.ToString().Contains("Invalid Token"))
                    {
                        error = obj.ToString() + "Checked";
                        GetToken(UserID);
                    }
                }
                catch (Exception ed)
                {
                    ErrorLogs.ErrorLog(0, "getProductLinePO", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "getProductLinePO", 1);
                }
                return obj;
            }
            catch (Exception ed)
            {
                GetToken(UserID);
                return "Please Try again Later";
            }
        }

        [HttpGet]
        public dynamic getProductLinePOCount(long UserID, string RecType, string vendor)
        {
            string error = string.Empty;
            try
            {
                var obj = ApiForVendorPortalRepository.getProductLinePOCount(UserID, RecType, vendor);
                try
                {
                    if (obj.ToString().Contains("Invalid Token"))
                    {
                        error = obj.ToString() + "Checked";
                        GetToken(UserID);
                    }
                }
                catch (Exception ed)
                {
                    ErrorLogs.ErrorLog(0, "getProductLinePOCount", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "getProductLinePOCount", 1);
                }
                return obj;
            }
            catch (Exception ed)
            {
                GetToken(UserID);
                return "Please Try again Later";
            }
        }

        [HttpGet]
        public dynamic getPoDtl(long UserID, string vendor, string PONO)
        {
            string error = string.Empty;
            try
            {
                var obj = ApiForVendorPortalRepository.getPoDtl(UserID, vendor, PONO);
                try
                {
                    if (obj.ToString().Contains("Invalid Token"))
                    {
                        error = obj.ToString() + "Checked";
                        GetToken(UserID);
                    }
                }
                catch (Exception ed)
                {
                    ErrorLogs.ErrorLog(0, "getPoDtl", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "getPoDtl", 1);
                }
                return obj;
            }
            catch (Exception ed)
            {
                GetToken(UserID);
                return "Please Try again Later";
            }
        }
        [HttpGet]
        public dynamic SearchByPart(long UserID, string RecType, string Vendor, string sword)
        {
            string error = string.Empty;
            try
            {
                var obj = ApiForVendorPortalRepository.SearchByPart(UserID, RecType, Vendor, sword);
                try
                {
                    if (obj.ToString().Contains("Invalid Token"))
                    {
                        error = obj.ToString() + "Checked";
                        GetToken(UserID);
                    }
                }
                catch (Exception ed)
                {
                    ErrorLogs.ErrorLog(0, "SearchByPart", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "SearchByPart", 1);
                }
                return obj;
            }
            catch (Exception ed)
            {
                GetToken(UserID);
                return "Please Try again Later";
            }
        }

        [HttpPost]
        public dynamic InsertTrackingNo(POTrackingViewModel poTraaackingViewModel)
        {
            var obj = ApiForVendorPortalRepository.InsertTrackingNo(poTraaackingViewModel);
            string error = string.Empty;
            try
            {
                if (obj == null)
                {
                    GetToken(Convert.ToInt32(poTraaackingViewModel.UserID));
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getPoDtl", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "getPoDtl", 1);
            }
            return obj;
        }

        [HttpGet]
        public dynamic GetTrackingNo(string UserID, string ManifiestId)
        {
            var obj = ApiForVendorPortalRepository.GetTrackingNo(UserID, ManifiestId);
            string error = string.Empty;
            try
            {
                if (obj == null)
                {
                    GetToken(Convert.ToInt32(UserID));
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "getTrackingNo", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "getTrackingNo", 1);
            }
            return obj;
        }

        [HttpPost]
        public dynamic Itemavailability(Itemavaibi itemList)
        {
            var obj = ApiForVendorPortalRepository.Itemavailability(itemList.UserID, itemList.items, itemList.warehouse, "array");
            string error = string.Empty;
            try
            {
                if (obj == null)
                {
                    GetToken(Convert.ToInt32(itemList.UserID));
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "Itemavailability", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + error, "Itemavailability", 1);
            }
            return obj;
        }

        [HttpPost]
        public HttpPostResponse AddSchedulerConfig(SchedulerConfigViewModel schedulerConfigVM)
        {
            HttpRequest newrequest = HttpContext.Current.Request;
            string authenticationToken = Convert.ToString(newrequest.Headers["AuthenticationToken"]);
            string memRefNo = Convert.ToString(newrequest.Headers["MemberReferenceNo"]);

            if (!string.IsNullOrEmpty(authenticationToken) && !string.IsNullOrEmpty(memRefNo))
            {
                if (usermanager.ValidateToken(memRefNo, authenticationToken))
                {
                    bool flag = false;
                    var getuser = usermanager.GetUserAlldataByMemrefno(schedulerConfigVM.MemRefNo);
                    schedulerConfigVM.UserId = getuser.UserID;
                    flag = ApiForVendorPortalRepository.AddSchedulerConfig(schedulerConfigVM);
                    if (flag == true)
                    {
                        return (new HttpPostResponse { }.CreateResponse("Success", "Scheduler config inserted", ""));
                    }
                    else
                    {
                        return (new HttpPostResponse { }.CreateResponse("Failed", "Please try again letter", ""));
                    }
                }
                else
                    return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
            }
            else
                return (new HttpPostResponse { }.CreateResponse("Redirect", "Invalid user", "Login"));
        }
    }
}
