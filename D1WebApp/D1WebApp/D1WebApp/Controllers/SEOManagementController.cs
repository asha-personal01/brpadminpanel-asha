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
using D1WebApp.InterfaceRepositories;
using D1WebApp.Repositories;
using D1WebApp.ViewModels;
using D1WebApp.Controllers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Linq;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace D1WebApp.BussinessLogicLayer.Controllers
{
    #if !DEBUG
          [System.Web.Mvc.RequireHttps]
    #endif

    public class SEOManagementController : ApiController
    {
        public SEOManager SEOManager = new SEOManager();

        [HttpGet]
        public dynamic GetAllItNodeTreeMetaList(string UserMemRefNo)
        {
            return SEOManager.GetAllItNodeTreeMetaList(UserMemRefNo);
        }
        [HttpGet]
        public dynamic GetAllItemMetaList(string UserMemRefNo)
        {
            return SEOManager.GetAllItemMetaList(UserMemRefNo);
        }
        [HttpGet]
        public dynamic GetAllProdLineMetaList(string UserMemRefNo)
        {
            return SEOManager.GetAllProdLineMetaList(UserMemRefNo);
        }
        [HttpGet]
        public dynamic GetAllMajClsMetaList(string UserMemRefNo)
        {
            return SEOManager.GetAllMajClsMetaList(UserMemRefNo);
        }
        [HttpGet]
        public dynamic GetItNodeTreeMetaList(string UserMemRefNo, string filterQuery, int pageno)
        {
            return SEOManager.GetItNodeTreeMetaList(UserMemRefNo, filterQuery, pageno);
        }
        [HttpGet]
        public dynamic GetItemMetaList(string UserMemRefNo, string filterQuery, int pageno)
        {
            return SEOManager.GetItemMetaList(UserMemRefNo, filterQuery, pageno);
        }
        [HttpGet]
        public dynamic GetItProdLineMetaList(string UserMemRefNo, string filterQuery, int pageno)
        {
            return SEOManager.GetItProdLineMetaList(UserMemRefNo, filterQuery, pageno);
        }
        [HttpGet]
        public dynamic GetMajclsetaList(string UserMemRefNo, string filterQuery, int pageno)
        {
            return SEOManager.GetMajclsetaList(UserMemRefNo, filterQuery, pageno);
        }
        [HttpPost]
        public dynamic UpdateTreeNodeMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            return SEOManager.UpdateTreeNodeMeta(UpdateMetaViewModel);
        }
        [HttpPost]
        public dynamic UpdateItemMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            return SEOManager.UpdateItemMeta(UpdateMetaViewModel);
        }
        [HttpPost]
        public dynamic UpdateProdlineMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            return SEOManager.UpdateProdlineMeta(UpdateMetaViewModel);
        }
        [HttpPost]
        public dynamic UpdateItMajClsMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            return SEOManager.UpdateItMajClsMeta(UpdateMetaViewModel);
        }
        [HttpPost]
        public dynamic ImportItTreeNodeMetaData()
        {
            bool status = false;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                List<ImportMetaDataViewModel> itTreeNodeList = new List<ImportMetaDataViewModel>();

                var memRefNo = httpRequest.Form["memRefNo"];

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string FileName = httpRequest.Form["FileName"];

                        IList<string> AllowedFileExtensions = new List<string> { ".csv" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload csv file");
                            status = false;
                        }
                        else
                        {
                            itTreeNodeList = ReadCsvDataForMetaDetails(postedFile.InputStream);
                            if(itTreeNodeList.Count > 0)
                            {
                                status = SEOManager.UpdateItTreeNodeMetaData(memRefNo, itTreeNodeList); 
                            }
                        }
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                status = false;
                return status;
            }
        }

        [HttpPost]
        public dynamic ImportItemMetaData()
        {
            bool status = false;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                List<ImportMetaDataViewModel> itemList = new List<ImportMetaDataViewModel>();

                var memRefNo = httpRequest.Form["memRefNo"];

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string FileName = httpRequest.Form["FileName"];

                        IList<string> AllowedFileExtensions = new List<string> { ".csv" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload csv file");
                            status = false;
                        }
                        else
                        {
                            itemList = ReadCsvDataForMetaDetails(postedFile.InputStream);
                            if (itemList.Count > 0)
                            {
                                status = SEOManager.UpdateItemMetaData(memRefNo, itemList);
                            }
                        }
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost]
        public dynamic ImportProdLineMetaData()
        {
            bool status = false;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                List<ImportMetaDataViewModel> prodLineList = new List<ImportMetaDataViewModel>();

                var memRefNo = httpRequest.Form["memRefNo"];

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string FileName = httpRequest.Form["FileName"];

                        IList<string> AllowedFileExtensions = new List<string> { ".csv" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload csv file");
                            status = false;
                        }
                        else
                        {
                            prodLineList = ReadCsvDataForMetaDetails(postedFile.InputStream);
                            if (prodLineList.Count > 0)
                            {
                                status = SEOManager.UpdatProdLineMetaData(memRefNo, prodLineList);
                            }
                        }
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                status = false; return status;
            }
        }

        [HttpPost]
        public dynamic ImportMajClsMetaData()
        {
            bool status = false;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                List<ImportMetaDataViewModel> majClsList = new List<ImportMetaDataViewModel>();

                var memRefNo = httpRequest.Form["memRefNo"];

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        string FileName = httpRequest.Form["FileName"];

                        IList<string> AllowedFileExtensions = new List<string> { ".csv" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload csv file");
                            status = false;
                        }
                        else
                        {
                            majClsList = ReadCsvDataForMetaDetails(postedFile.InputStream);
                            if (majClsList.Count > 0)
                            {
                                status = SEOManager.UpdateMajClsMetaData(memRefNo, majClsList);
                            }
                        }
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                status = false; return status;
            }
        }

        private List<ImportMetaDataViewModel> ReadCsvDataForMetaDetails(Stream inputStream)
        {
            var records = new List<ImportMetaDataViewModel>();
            try
            {
                using (var reader = new StreamReader(inputStream))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();

                    while (csv.Read())
                    {
                        var itTreeNodeListModel = new ImportMetaDataViewModel
                        {
                            Seq = csv.GetField<long>(0), 
                            Description = csv.GetField<string>(2),
                            Meta = csv.GetField<string>(3), 
                            TitleTag = csv.GetField<string>(4), 
                            MetaDescription = csv.GetField<string>(5) 
                        };
                        records.Add(itTreeNodeListModel);
                    }
                }
            }
            catch (Exception ed) {
                throw new Exception(ed.Message);
            }
            
            return records;
        }
        
    }
}
