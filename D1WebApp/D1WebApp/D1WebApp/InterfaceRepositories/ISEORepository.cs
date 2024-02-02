//Developed by Nayan

using D1WebApp.BusinessLogicLayer.ViewModels;

using D1WebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Web;

namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface ISEORepository : IDisposable
    {
        dynamic GetItNodeTreeMetaList(string memRefNo, string filterQuery, int pageno);
        dynamic GetItemMetaList(string memRefNo, string filterQuery, int pageno);
        dynamic GetItProdLineMetaList(string memRefNo, string filterQuery, int pageno);
        dynamic GetMajclsetaList(string memRefNo, string filterQuery, int pageno);
        dynamic GetAllItNodeTreeMetaList(string memRefNo);
        dynamic GetAllItemMetaList(string memRefNo);
        dynamic GetAllProdLineMetaList(string memRefNo);
        dynamic GetAllMajClsMetaList(string memRefNo);

    }
}