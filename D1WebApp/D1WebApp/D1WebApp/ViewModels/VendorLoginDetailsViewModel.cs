using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D1WebApp.Models;
using D1WebApp.Utilities;

namespace D1WebApp.ViewModels
{

    public class UserAuthViewModel
    {
        public string client { get; set; }
        public string company { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public class AuthTokenViewModel
    {
        public string access_token { get; set; }
        public string grant_token { get; set; }
    }

    public class VendorLoginDetailsViewModel
    {
        public long UserSubUserID { get; set; }
        public long UserID { get; set; }
        public string CompanyID { get; set; }
        public string vendor { get; set; }
        public string web_passwd { get; set; }
        public string __rowids { get; set; }
        public string __seq { get; set; }
        

        public static VendorLoginDetailsViewModel ConvertToViewModel(VendorUserLoginDetail VendorUserLogin)
        {
            VendorLoginDetailsViewModel VendorLoginDetailsView = new VendorLoginDetailsViewModel();
            VendorLoginDetailsView.UserID = VendorUserLogin.UserID;
            VendorLoginDetailsView.UserSubUserID = VendorUserLogin.UserSubUserID;
            VendorLoginDetailsView.vendor = VendorUserLogin.vendor;
            VendorLoginDetailsView.web_passwd = VendorUserLogin.web_passwd;
            VendorLoginDetailsView.__rowids = VendorUserLogin.C__rowids;
            VendorLoginDetailsView.__seq = VendorUserLogin.C__seq;
            VendorLoginDetailsView.CompanyID = VendorUserLogin.CompanyID;
            return VendorLoginDetailsView;
        }
        public static VendorUserLoginDetail ConvertToModel(VendorLoginDetailsViewModel VendorLoginDetailsView)
        {
            VendorUserLoginDetail VendorUserLogin = new VendorUserLoginDetail();
            VendorUserLogin.C__seq = VendorLoginDetailsView.__seq;
            VendorUserLogin.C__rowids = VendorLoginDetailsView.__rowids;
            VendorUserLogin.UserID = VendorLoginDetailsView.UserID;
            if (VendorLoginDetailsView.UserSubUserID != null && VendorLoginDetailsView.UserSubUserID > 0)
            {
                VendorUserLogin.UserSubUserID = VendorLoginDetailsView.UserSubUserID;
            }
            VendorUserLogin.vendor = VendorLoginDetailsView.vendor;
            VendorUserLogin.web_passwd = EasyMD5.Hash(VendorLoginDetailsView.web_passwd);
            VendorUserLogin.CompanyID = VendorLoginDetailsView.CompanyID;
            return VendorUserLogin;
        }
        public static List<VendorLoginDetailsViewModel> ConvertToViewList(List<VendorUserLoginDetail> VendorUserLoginList)
        {
            List<VendorLoginDetailsViewModel> VendorLoginDetailsViewList = new List<VendorLoginDetailsViewModel>();
            foreach (var item in VendorUserLoginList)
            {
                VendorLoginDetailsViewList.Add(VendorLoginDetailsViewModel.ConvertToViewModel(item));
            }
            return VendorLoginDetailsViewList;
        }
        public static List<VendorUserLoginDetail> ConvertToModelList(List<VendorLoginDetailsViewModel> VendorLoginDetailsViewList)
        {
            List<VendorUserLoginDetail> VendorUserLoginList = new List<VendorUserLoginDetail>();
            foreach (var item in VendorLoginDetailsViewList)
            {
                VendorUserLoginList.Add(VendorLoginDetailsViewModel.ConvertToModel(item));
            }
            return VendorUserLoginList;
        }
        public static List<VendorUserLoginDetail> ConvertToModelListForUser(List<VendorLoginDetailsViewModel> VendorLoginDetailsViewList,long UserID,string CompanyID)
        {
            List<VendorUserLoginDetail> VendorUserLoginList = new List<VendorUserLoginDetail>();
            foreach (var item in VendorLoginDetailsViewList)
            {
                item.UserID = UserID;
                item.CompanyID = CompanyID;
                VendorUserLoginList.Add(VendorLoginDetailsViewModel.ConvertToModel(item));
            }
            return VendorUserLoginList;
        }

    }
}