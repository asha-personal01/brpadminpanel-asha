//Developed by Nayan

using System.Web;
using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.BusinessLogicLayer.ViewModels;

namespace D1WebApp.Utilities
{
    public class BannersManager
    {
        private BannersRepository Bannersrepository= new BannersRepository();        

        public BannersManager()
        {
        }
        //public List<BannerViewModel> GetAll(string UserMemRefNo)
        //{
        //    return Bannersrepository.GetAll(UserMemRefNo);
        //}
        //public List<BannerViewModel> GetBannerListByPage(int page, int pagenumber, string UserMemRefNo)
        //{
        //    return Bannersrepository.GetBannerListByPage(page, pagenumber, UserMemRefNo);
        //}
        //public bool InsertBanner(BannerViewModel NewBanner)
        //{
        //    return Bannersrepository.InsertBanner(NewBanner);
        //}
        //public BannerViewModel GetDataByID(long BannerID, string UserMemRefNo)
        //{
        //    return Bannersrepository.GetDataByID(BannerID, UserMemRefNo);
        //}
        //public bool UpdateBanner(BannerViewModel NewBanner)
        //{
        //    return Bannersrepository.UpdateBanner(NewBanner);
        //}
        //public bool DeleteBanner(long BannerID, string UserMemRefNo)
        //{
        //    return Bannersrepository.DeleteBanner(BannerID, UserMemRefNo);
        //}

    }
}