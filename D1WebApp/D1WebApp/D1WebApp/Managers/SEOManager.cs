//Developed by Nayan

using System.Web;
using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.ClientModel;
using System.Web.UI;

namespace D1WebApp.Utilities
{
    public class SEOManager
    {
        private SEORepository SEORepository = new SEORepository();

        public SEOManager()
        {
        }
        
        public dynamic GetAllItNodeTreeMetaList(string memRefNo)
        {
            return SEORepository.GetAllItNodeTreeMetaList(memRefNo);
        }
        public dynamic GetAllItemMetaList(string memRefNo)
        {
            return SEORepository.GetAllItemMetaList(memRefNo);
        }
        public dynamic GetAllProdLineMetaList(string memRefNo)
        {
            return SEORepository.GetAllProdLineMetaList(memRefNo);
        }
        public dynamic GetAllMajClsMetaList(string memRefNo)
        {
            return SEORepository.GetAllMajClsMetaList(memRefNo);
        }
        public dynamic GetItNodeTreeMetaList(string memRefNo, string filterQuery, int pageno)
        {
            return SEORepository.GetItNodeTreeMetaList(memRefNo, filterQuery, pageno);
        }
        public dynamic GetItemMetaList(string memRefNo, string filterQuery,int pageno)
        {
            return SEORepository.GetItemMetaList(memRefNo, filterQuery, pageno);
        }
        public dynamic GetItProdLineMetaList(string memRefNo, string filterQuery, int pageno)
        {
            return SEORepository.GetItProdLineMetaList(memRefNo, filterQuery, pageno);
        }
        public dynamic GetMajclsetaList(string memRefNo, string filterQuery, int pageno)
        {
            return SEORepository.GetMajclsetaList(memRefNo, filterQuery, pageno);
        }
        public dynamic UpdateTreeNodeMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            return SEORepository.UpdateTreeNodeMeta(UpdateMetaViewModel);
        }
        public dynamic UpdateItemMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            return SEORepository.UpdateItemMeta(UpdateMetaViewModel);
        }
        public dynamic UpdateProdlineMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            return SEORepository.UpdateProdlineMeta(UpdateMetaViewModel);
        }
        public dynamic UpdateItMajClsMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            return SEORepository.UpdateItMajClsMeta(UpdateMetaViewModel);
        }
        public dynamic UpdateItTreeNodeMetaData(string memRefNo, List<ImportMetaDataViewModel> updateTreeNodeMetaList)
        {
            return SEORepository.UpdateItTreeNodeMetaData(memRefNo, updateTreeNodeMetaList);
        }
        public dynamic UpdateItemMetaData(string memRefNo, List<ImportMetaDataViewModel> updateTreeNodeMetaList)
        {
            return SEORepository.UpdateItemMetaData(memRefNo, updateTreeNodeMetaList);
        }
        public dynamic UpdatProdLineMetaData(string memRefNo, List<ImportMetaDataViewModel> updateProdLineMetaList)
        {
            return SEORepository.UpdatProdLineMetaData(memRefNo, updateProdLineMetaList);
        }
        public dynamic UpdateMajClsMetaData(string memRefNo, List<ImportMetaDataViewModel> updateMajClsMetaList)
        {
            return SEORepository.UpdateMajClsMetaData(memRefNo, updateMajClsMetaList);
        }
    }
}