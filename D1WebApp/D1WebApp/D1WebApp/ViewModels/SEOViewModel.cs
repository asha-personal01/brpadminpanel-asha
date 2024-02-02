using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using D1WebApp.ClientModel;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class ItTreeNodeMetaViewModel
    {
        public int TotalPage { get; set;  }
        public long Seq { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class ItemMetaViewModel
    {
        public int TotalPage { get; set; }
        public long Seq { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class ProdLineMetaViewModel
    {
        public int TotalPage { get; set; }
        public long Seq { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class ItMajClsMetaViewModel
    {
        public int TotalPage { get; set; }
        public long Seq { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }

    public class UpdateMetaViewModel
    {
        public string MemRefNo { get; set; }
        public long Seq { get; set; }
        public string TitleTag { get; set; }
        public string Meta { get; set; }
        public string MetaDescription { get; set; }
        public string Description { get; set; }
    }

    public class ImportMetaDataViewModel
    {
        public long Seq { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
    }
}
