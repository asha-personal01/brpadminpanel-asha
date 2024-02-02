
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace D1WebApp.BusinessLogicLayer.ViewModels
//{
//    public class CompanyProfileViewModel
//    {
//        public string UserMemRefNo { get; set; }
//        public int CompanyID { get; set; }
//        public string CompanyName { get; set; }
//        public string CompanyAddress { get; set; }
//        public string CompanyInfo { get; set; }
//        public string CompanyVision { get; set; }
//        public string CompanyMission { get; set; }
//        public string CompanyEmail { get; set; }
//        public string CompanyContact1 { get; set; }
//        public string CompanyContact2 { get; set; }
//        public string CompanyMobile1 { get; set; }
//        public string CompanyMobile2 { get; set; }

//        public static CompanyProfileViewModel ConvertToViewModel(CompanyProfile NewCOmpany)
//        {
//            CompanyProfileViewModel CompanyProfileView = new CompanyProfileViewModel();
//            CompanyProfileView.CompanyAddress = NewCOmpany.CompanyAddress;
//            CompanyProfileView.CompanyContact1 = NewCOmpany.CompanyContact1;
//            CompanyProfileView.CompanyContact2 = NewCOmpany.CompanyContact2;
//            CompanyProfileView.CompanyEmail = NewCOmpany.CompanyEmail;
//            CompanyProfileView.CompanyID = NewCOmpany.CompanyID;
//            CompanyProfileView.CompanyInfo = NewCOmpany.CompanyInfo;
//            CompanyProfileView.CompanyMission = NewCOmpany.CompanyMission;
//            CompanyProfileView.CompanyMobile1 = NewCOmpany.CompanyMobile1;
//            CompanyProfileView.CompanyMobile2 = NewCOmpany.CompanyMobile2;
//            CompanyProfileView.CompanyName = NewCOmpany.CompanyName;
//            CompanyProfileView.CompanyVision = NewCOmpany.CompanyVision;            
//            return CompanyProfileView;
//        }
//        public static CompanyProfile ConvertToModel(CompanyProfileViewModel CompanyProfileView) 
//        {
//            CompanyProfile CompanyProfile1 = new CompanyProfile();
//            CompanyProfile1.CompanyAddress = CompanyProfileView.CompanyAddress;
//            CompanyProfile1.CompanyContact1 = CompanyProfileView.CompanyContact1;
//            CompanyProfile1.CompanyContact2 = CompanyProfileView.CompanyContact2;
//            CompanyProfile1.CompanyEmail = CompanyProfileView.CompanyEmail;
//            if (CompanyProfileView.CompanyID != null && CompanyProfileView.CompanyID > 0)
//            {
//                CompanyProfile1.CompanyID = CompanyProfileView.CompanyID;
//            }
//            CompanyProfile1.CompanyInfo = CompanyProfileView.CompanyInfo;
//            CompanyProfile1.CompanyMission = CompanyProfileView.CompanyMission;
//            CompanyProfile1.CompanyMobile1 = CompanyProfileView.CompanyMobile1;
//            CompanyProfile1.CompanyMobile2 = CompanyProfileView.CompanyMobile2;
//            CompanyProfile1.CompanyName = CompanyProfileView.CompanyName;
//            CompanyProfile1.CompanyVision = CompanyProfileView.CompanyVision;            
//            return CompanyProfile1;
//        }

//        public static List<CompanyProfileViewModel> COnvertToViewModelList(List<CompanyProfile> CompanyProfilelist)
//        {
//            List<CompanyProfileViewModel> COnvertToViewModelList = new List<CompanyProfileViewModel>();
//            foreach (var item in CompanyProfilelist)
//            {
//                COnvertToViewModelList.Add(CompanyProfileViewModel.ConvertToViewModel(item));
//            }
//            return COnvertToViewModelList;
//        }
//    }
//}