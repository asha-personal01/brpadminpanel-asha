
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace D1WebApp.BusinessLogicLayer.ViewModels
//{
//    public class BannerViewModel
//    {
//        public string UserMemRefNo{ get; set; }
//        public long BannerID { get; set; }
//        public string Title { get; set; }
//        public string Image { get; set; }
//        public string Link { get; set; }
//        public string Source { get; set; }
//        public Nullable<bool> Active { get; set; }
//        public Nullable<int> Rank { get; set; }
//        public BannerViewModel()
//        { }

//        public BannerViewModel(Banner Banners)
//        {            
//            this.BannerID = Banners.BannerID;
//            this.Title = Banners.Title;
//            this.Active = Banners.Active;
//            this.Image = Banners.Image;
//            this.Link = Banners.Link;
//            this.Source = Banners.Source;
//            this.Rank = Banners.Rank;

//        }

        
//        public static List<BannerViewModel> ConvertModelToViewModel(List<Banner> Banners)
//        {
//            List<BannerViewModel> BannerList = new List<BannerViewModel>();
//            foreach (Banner list in Banners)
//            {
//                BannerList.Add(new BannerViewModel(list));
//            }
//            return BannerList;
//        }

      
//        public static Banner ConvertViewModelToModel(BannerViewModel Banner)
//        {
//            Banner Banners = new Banner();
//            if (Banner.BannerID != null && Banner.BannerID > 0)
//            {
//                Banners.BannerID = Banner.BannerID;
//            }
//            Banners.Title = Banner.Title;
//            Banners.Active = Banner.Active;
//            Banners.Image = Banner.Image;
//            Banners.Link = Banner.Link;
//            Banners.Source = Banner.Source;
//            Banners.Rank = Banner.Rank;
//            return Banners;
//        }

      
      
//    }
//}