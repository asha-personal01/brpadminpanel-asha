//Developed by Nayan

using D1WebApp.BusinessLogicLayer.ViewModels;
using System;
using System.Linq;
using System.Web;
using D1WebApp.Utilities;
using D1WebApp.Utilities.GeneralConfiguration;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using D1WebApp.ClientModel;
using RestSharp;
using Newtonsoft.Json;
using D1WebApp.Models;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Security.Policy;
using WebGrease.Css.Ast;
using System.Web.UI.WebControls;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public class SEORepository : ISEORepository, IDisposable
    {
        public string Errorlog = HttpContext.Current.Server.MapPath("~/SQL").ToString() + "\\Errorlog_" + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";

        public SEORepository()
        {

        }
        public dynamic GetAllItNodeTreeMetaList(string memRefNo)
        {
            List<GetTreeNodeMetaList_Result> itTreeNodelist = new List<GetTreeNodeMetaList_Result>();

            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                itTreeNodelist = context.GetTreeNodeMetaList("", 0).ToList();
                return itTreeNodelist;
            }
            catch(Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetAllItNodeTreeMetaList", DateTime.Now, "GetAllItNodeTreeMetaList", ed.ToString(), "GetAllItNodeTreeMetaList", 2);
            }
            return itTreeNodelist;
        }
        public dynamic GetAllItemMetaList(string memRefNo)
        {
            List<GetItemMetaList_Result> itemMetalist = new List<GetItemMetaList_Result>();

            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                itemMetalist = context.GetItemMetaList("", 0).ToList();
                return itemMetalist;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetAllItemMetaList", DateTime.Now, "GetAllItemMetaList", ed.ToString(), "GetAllItemMetaList", 2);
            }
            return itemMetalist;
        }
        public dynamic GetAllProdLineMetaList(string memRefNo)
        {
            List<GetProdLineMetaList_Result> prodLineMetalist = new List<GetProdLineMetaList_Result>();

            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                prodLineMetalist = context.GetProdLineMetaList("", 0).ToList();
                return prodLineMetalist;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetAllProdLineMetaList", DateTime.Now, "GetAllProdLineMetaList", ed.ToString(), "GetAllProdLineMetaList", 2);
            }
            return prodLineMetalist;
        }
        public dynamic GetAllMajClsMetaList(string memRefNo)
        {
            List<GetMajClsMetaList_Result> majClsMetalist = new List<GetMajClsMetaList_Result>();

            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                majClsMetalist = context.GetMajClsMetaList("", 0).ToList();
                return majClsMetalist;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetAllMajClsMetaList", DateTime.Now, "GetAllMajClsMetaList", ed.ToString(), "GetAllMajClsMetaList", 2);
            }
            return majClsMetalist;
        }
        public dynamic GetItNodeTreeMetaList(string memRefNo, string filterQuery, int pageno)
        {
            List<GetTreeNodeMetaList_Result> itTreeNodelist = new List<GetTreeNodeMetaList_Result>();
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (pageno == 0)
                {
                    pageno = 1;
                }

                itTreeNodelist = context.GetTreeNodeMetaList(filterQuery, pageno).ToList();

                return itTreeNodelist;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetItNodeTreeMetaList", DateTime.Now, "GetItNodeTreeMetaList", ed.ToString(), "GetItNodeTreeMetaList", 2);
                return itTreeNodelist;
            }
        }

        public dynamic GetItemMetaList(string memRefNo, string filterQuery, int pageno)
        {
            List<GetItemMetaList_Result> itemMetalist = new List<GetItemMetaList_Result>();
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (pageno == 0)
                {
                    pageno = 1;
                }

                itemMetalist = context.GetItemMetaList(filterQuery, pageno).ToList();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetItemMetaList", DateTime.Now, "GetItemMetaList", ed.ToString(), "GetItemMetaList", 2);
            }
            return itemMetalist;
        }

        public dynamic GetItProdLineMetaList(string memRefNo, string filterQuery, int pageno)
        {
            List<GetProdLineMetaList_Result> itemMetalist = new List<GetProdLineMetaList_Result>();
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (pageno == 0)
                {
                    pageno = 1;
                }

                itemMetalist = context.GetProdLineMetaList(filterQuery, pageno).ToList();

            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetItProdLineMetaList", DateTime.Now, "GetItProdLineMetaList", ed.ToString(), "GetItProdLineMetaList", 2);
            }
            return itemMetalist;
        }

        public dynamic GetMajclsetaList(string memRefNo, string filterQuery, int pageno)
        {
            List<GetMajClsMetaList_Result> majClsMetalist = new List<GetMajClsMetaList_Result>();
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (pageno == 0)
                {
                    pageno = 1;
                }

                majClsMetalist = context.GetMajClsMetaList(filterQuery, pageno).ToList();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetMajclsetaList", DateTime.Now, "GetMajclsetaList", ed.ToString(), "GetMajclsetaList", 2);
            }
            return majClsMetalist;
        }
        public dynamic UpdateTreeNodeMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(UpdateMetaViewModel.MemRefNo));
                if (UpdateMetaViewModel.Seq > 0)
                {
                    var getold = context.it_tree_node.Where(cp => cp.C__seq == UpdateMetaViewModel.Seq).FirstOrDefault();
                    var getNew = context.it_tree_node.Where(cp => cp.C__seq == UpdateMetaViewModel.Seq).FirstOrDefault();
                    getNew.descr = UpdateMetaViewModel.Description;
                    getNew.meta = UpdateMetaViewModel.Meta;
                    getNew.title_tag = UpdateMetaViewModel.TitleTag;
                    getNew.meta_descr = UpdateMetaViewModel.MetaDescription;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "UpdateTreeNodeMeta", DateTime.Now, "UpdateTreeNodeMeta", "Primary ID is missing", "UpdateTreeNodeMeta", 2);
                    return "Primary ID is missing";
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateTreeNodeMeta", DateTime.Now, "UpdateTreeNodeMeta", ed.ToString(), "UpdateTreeNodeMeta", 2);
                return ed.InnerException.ToString();
            }
        }
        public dynamic UpdateItemMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(UpdateMetaViewModel.MemRefNo));
                if (UpdateMetaViewModel.Seq > 0)
                {
                    var getold = context.items.Where(cp => cp.C__seq == UpdateMetaViewModel.Seq).FirstOrDefault();
                    var getNew = context.items.Where(cp => cp.C__seq == UpdateMetaViewModel.Seq).FirstOrDefault();
                    getNew.descr = UpdateMetaViewModel.Description;
                    getNew.meta = UpdateMetaViewModel.Meta;
                    getNew.title_tag = UpdateMetaViewModel.TitleTag;
                    getNew.meta_descr = UpdateMetaViewModel.MetaDescription;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "UpdateItemMeta", DateTime.Now, "UpdateItemMeta", "Primary ID is missing", "UpdateItemMeta", 2);
                    return "Primary ID is missing";
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateItemMeta", DateTime.Now, "UpdateItemMeta", ed.ToString(), "UpdateItemMeta", 2);
                return ed.InnerException.ToString();
            }
        }

        public dynamic UpdateProdlineMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(UpdateMetaViewModel.MemRefNo));
                if (UpdateMetaViewModel.Seq > 0)
                {
                    var getold = context.it_prodline.Where(cp => cp.C__seq == UpdateMetaViewModel.Seq).FirstOrDefault();
                    var getNew = context.it_prodline.Where(cp => cp.C__seq == UpdateMetaViewModel.Seq).FirstOrDefault();
                    getNew.web_descr = UpdateMetaViewModel.Description;
                    getNew.meta = UpdateMetaViewModel.Meta;
                    getNew.title_tag = UpdateMetaViewModel.TitleTag;
                    getNew.meta_descr = UpdateMetaViewModel.MetaDescription;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "UpdateProdlineMeta", DateTime.Now, "UpdateProdlineMeta", "Primary ID is missing", "UpdateProdlineMeta", 2);
                    return "Primary ID is missing";
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateProdlineMeta", DateTime.Now, "UpdateProdlineMeta", ed.ToString(), "UpdateProdlineMeta", 2);
                return ed.InnerException.ToString();
            }
        }

        public dynamic UpdateItMajClsMeta(UpdateMetaViewModel UpdateMetaViewModel)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(UpdateMetaViewModel.MemRefNo));
                if (UpdateMetaViewModel.Seq > 0)
                {
                    var getold = context.it_majcls.Where(cp => cp.C__seq == UpdateMetaViewModel.Seq).FirstOrDefault();
                    var getNew = context.it_majcls.Where(cp => cp.C__seq == UpdateMetaViewModel.Seq).FirstOrDefault();
                    getNew.web_descr = UpdateMetaViewModel.Description;
                    getNew.meta = UpdateMetaViewModel.Meta;
                    getNew.title_tag = UpdateMetaViewModel.TitleTag;
                    getNew.meta_descr = UpdateMetaViewModel.MetaDescription;
                    context.Entry(getold).CurrentValues.SetValues(getNew);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "UpdateItMajClsMeta", DateTime.Now, "UpdateItMajClsMeta", "Primary ID is missing", "UpdateItMajClsMeta", 2);
                    return "Primary ID is missing";
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateItMajClsMeta", DateTime.Now, "UpdateItMajClsMeta", ed.ToString(), "UpdateItMajClsMeta", 2);
                return ed.InnerException.ToString();
            }
        }

        public dynamic UpdateItTreeNodeMetaData(string memRefNo, List<ImportMetaDataViewModel> updateTreeNodeMetaList) 
        {
            try 
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (updateTreeNodeMetaList.Count > 0)
                {
                    string jsonobj = JsonConvert.SerializeObject(updateTreeNodeMetaList);
                    jsonobj = jsonobj.Replace("'", "");
                    context.Update_it_tree_node_Data(jsonobj);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "UpdateItTreeNodeMetaData", DateTime.Now, "UpdateItTreeNodeMetaData", "Primary ID is missing", "UpdateItTreeNodeMetaData", 2);
                    return false;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateItTreeNodeMetaData", DateTime.Now, "UpdateItTreeNodeMetaData", ed.ToString(), "UpdateItTreeNodeMetaData", 2);
                return false;
            }
        }

        public dynamic UpdateItemMetaData(string memRefNo, List<ImportMetaDataViewModel> updateItemMetaList)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (updateItemMetaList.Count > 0)
                {
                    string jsonobj = JsonConvert.SerializeObject(updateItemMetaList);
                    jsonobj = jsonobj.Replace("'", "\'");
                    context.Update_item_meta_Data(jsonobj);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "UpdateItemMetaData", DateTime.Now, "UpdateItemMetaData", "Primary ID is missing", "UpdateItemMetaData", 2);
                    return false;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateItemMetaData", DateTime.Now, "UpdateItemMetaData", ed.ToString(), "UpdateItemMetaData", 2);
                return false;
            }
        }
        public dynamic UpdatProdLineMetaData(string memRefNo, List<ImportMetaDataViewModel> updateProdLineMetaList)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (updateProdLineMetaList.Count > 0)
                {
                    string jsonobj = JsonConvert.SerializeObject(updateProdLineMetaList);
                    jsonobj = jsonobj.Replace("'", "");
                    context.Update_prodline_meta_Data(jsonobj);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "UpdatProdLineMetaData", DateTime.Now, "UpdatProdLineMetaData", "Primary ID is missing", "UpdatProdLineMetaData", 2);
                    return false;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdatProdLineMetaData", DateTime.Now, "UpdatProdLineMetaData", ed.ToString(), "UpdatProdLineMetaData", 2);
                return false;
            }
        }
        public dynamic UpdateMajClsMetaData(string memRefNo, List<ImportMetaDataViewModel> updateMajClsMetaList)
        {
            try
            {
                var context = new ClientEntities(ErrorLogs.BuildConnectionString(memRefNo));
                if (updateMajClsMetaList.Count > 0)
                {
                    string jsonobj = JsonConvert.SerializeObject(updateMajClsMetaList);
                    jsonobj = jsonobj.Replace("'", "");
                    context.Update_majcls_meta_Data(jsonobj);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    ErrorLogs.ErrorLog(0, "UpdateMajClsMetaData", DateTime.Now, "UpdateMajClsMetaData", "Primary ID is missing", "UpdateMajClsMetaData", 2);
                    return false;
                }
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "UpdateMajClsMetaData", DateTime.Now, "UpdateMajClsMetaData", ed.ToString(), "UpdateMajClsMetaData", 2);
                return false;
            }
        }

        public void Dispose()
        {

            GC.SuppressFinalize(this);
        }
    }
}