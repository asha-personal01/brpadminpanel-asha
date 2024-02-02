using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D1WebApp.ClientModel;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class ItemDetailsViewModel
    {
        public string memRefNo { get; set; }
        public long ItemDocId { get; set; }
        public string Item { get; set; }
        public string DocType { get; set; }
        public string DocName { get; set; }
        public string DocDetailsUrl { get; set; }
        public int? Sequence { get; set; }
        public bool IMType { get; set; }
    }
    public class ManufracturerParamViewModel
    {
        public string memRefNo { get; set; }
        public int pageno { get; set; }
        public string filterQuery { get; set; }
    }
    public class ManufItemDetailsViewModel
    {
        public int TotalPage { get; set; }
        public string memRefNo { get; set; }
        public long ItemDocId { get; set; }
        public string Item { get; set; }
        public string DocType { get; set; }
        public string DocName { get; set; }
        public string DocDetailsUrl { get; set; }
        public int? Sequence { get; set; }
        public bool? IMType { get; set; }
    }
    public class ItemPriceListModel
    {
        public string Item { get; set; }
        public double Price { get; set; }
    }

    public class ItemListModel
    {
        public string item1 { get; set; }
        public bool? discontinued { get; set; }
        public int TotalPage { get; set; }
    }

    public class CartReportParam
    {
        public string memRefNo { get; set; }
        public string ItemName { get; set; }
        public string UserId { get; set; }
        public string Customer { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class CartDetailsViewModel
    {
        public string Item { get; set; }
        public int? Quantity { get; set; }
        public string Price { get; set; }
        public string Customer { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Date { get; set; }
    }

    public class UploadFolderResponse
    {
        public string Message { get; set; }
        public List<string> FileNames { get; set; }
        public string FolderPath { get; set; }
    }
}
