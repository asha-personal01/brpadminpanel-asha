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


namespace D1WebApp.DataAccessLayer.Repositories
{
    public class BannersRepository : IBannersRepository, IDisposable
    {
        
        public BannersRepository()
        {
            
        }

        //public bool DeleteBanner(long BannerID, string UserMemRefNo)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //        Banner getoldbanner = Context2.Banners.Find(BannerID);
        //        Context2.Banners.Remove(getoldbanner);
        //        Context2.SaveChanges();
        //        Context2.Dispose();
        //        flag = true;
        //    }
        //    catch(Exception ed) { }
        //    return flag;
        //}

        //public bool UpdateBanner(BannerViewModel NewBanner)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(NewBanner.UserMemRefNo.ToString()));
        //    Banner getooldbanner = Context2.Banners.Find(NewBanner.BannerID);
        //    Banner newbanner = BannerViewModel.ConvertViewModelToModel(NewBanner);
        //    Context2.Entry(getooldbanner).CurrentValues.SetValues(newbanner);
        //    Context2.SaveChanges();
        //    Context2.Dispose();
        //    flag = true;
        //    }
        //    catch (Exception ed) { }
        //    return flag;
        //}

        //public BannerViewModel GetDataByID(long BannerID, string UserMemRefNo)
        //{
        //    BannerViewModel BannerView = new BannerViewModel();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //        var getbanner = Context2.Banners.Find(BannerID);
        //        BannerView = new BannerViewModel(getbanner);
        //    }catch(Exception ed) { }
        //        return BannerView;
        //}

        //public bool InsertBanner(BannerViewModel NewBanner)
        //{
        //    bool flag = false;
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(NewBanner.UserMemRefNo.ToString()));
        //    if (NewBanner.BannerID != null && NewBanner.BannerID > 0)
        //    {
        //        Banner oldbanner = Context2.Banners.Find(NewBanner.BannerID);
        //        Banner nBanner = BannerViewModel.ConvertViewModelToModel(NewBanner);
        //        Context2.Entry(oldbanner).CurrentValues.SetValues(nBanner);
        //        Context2.SaveChanges();
        //        Context2.Dispose();
        //        flag = true;
        //    }
        //    else
        //    {
        //        Banner nBanner = BannerViewModel.ConvertViewModelToModel(NewBanner);
        //        Context2.Banners.Add(nBanner);
        //        Context2.SaveChanges();
        //        Context2.Dispose();
        //        flag = true;
        //    }
        //    }
        //    catch (Exception ed) { }
        //    return flag;
        //}
        //public List<BannerViewModel> GetAll(string UserMemRefNo)
        //{
        //    List<BannerViewModel> GetAll = new List<BannerViewModel>();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //    GetAll = (from br in Context2.Banners
        //              select new BannerViewModel
        //              {
        //                  Active=br.Active,
        //                  BannerID=br.BannerID,
        //                  Image=br.Image,
        //                  Link=br.Link,
        //                  Rank=br.Rank,
        //                  Source=br.Source,
        //                  Title=br.Title,                          
        //              }).OrderBy(cp => cp.Rank).ToList();
        //    Context2.Dispose();
        //    }
        //    catch (Exception ed) { }
        //    return GetAll;
        //}

        //public List<BannerViewModel> GetBannerListByPage(int page, int pagenumber, string UserMemRefNo)
        //{
        //    List<BannerViewModel> GetAll = new List<BannerViewModel>();
        //    try
        //    {
        //        var Context2 = new ClientEntities(ErrorLogs.BuildConnectionString(UserMemRefNo.ToString()));
        //    GetAll = (from br in Context2.Banners.OrderByDescending(c => c.BannerID).Skip((page - 1) * pagenumber).Take(pagenumber).ToList()
        //              select new BannerViewModel
        //              {
        //                  Active = br.Active,
        //                  BannerID = br.BannerID,
        //                  Image = br.Image,
        //                  Link = br.Link,
        //                  Rank = br.Rank,
        //                  Source = br.Source,
        //                  Title = br.Title,
        //              }).ToList();
        //    Context2.Dispose();
        //    }
        //    catch (Exception ed) { }
        //    return GetAll;
        //}

     

        public void Dispose()
        {
         
            GC.SuppressFinalize(this);
        }
    }
}