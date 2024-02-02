using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D1WebApp.Constants
{
    public enum App_CacheKeys : long
    {
        SRCH_BY_PRODUCT_CACHE_FILTER = 1, // search by product Cache data like size,make,grade,producer
        SRCH_BY_PRODUCT_INIT = 2, // search by product init
        SRCH_PRODUCT_CITY = 3,  // Search Product City
        SRCH_BUYER_WINDOW_INIT = 4,  // search by buyer window init
        SRCH_SELLER_WINDOW_INIT = 5, // search by seller window init
        SRCH_SELLER_WINDOW_CACHE_FILTER = 6, // search seller window Cache data like size,make,grade,producer
        SRCH_BY_COMPANY_INIT = 7, // search by company init method
        SellerProfileInit = 8,
        SellerProfileData = 9,
        ORG_BUSINESS_TYPE_SRCH_PRODUCT = 10, // org -business-type Search product
        SEC_BUSINESS_TYPE_SRCH_PRODUCT = 11, // sec -business-type Search product
        ASSETCATEGORY_PRODUCT = 12, // Asset Category Search Product
        PRODUCT_SRCH_PRODUCT = 13, // Product Search product
        SRCH_PRODUCT_STATES = 14,   // States Product Search
        SRCH_PRODUCT_CITIES = 15,   // City Product Search
        SRCH_BUYER_WINDOW_CACHE_FILTER = 16, // search buyer window Cache data like size,make,grade,producer

        // these are keys used in memory cacher
        HOME_AVL_BRANDS = 5001,
        HOME_AVL_BRANDS2 = 5042,
        HOME_AVL_PRODUCTS = 2043,
        SRCH_PRD_GETAUTOPRODUCTNAME = 5002,
        SRCH_MB_GETAUTOMAKENAME = 5041,
        HOME_LEADING_PRODUCT = 5003,
        HOME_SOURCE_BY_STATE = 5004,
        VIEW_STATEDATA_SOURCE_BY_STATE = 5005,
        HOME_NEWS = 5006,
        BUYER_LANDING_INIT = 5007, // buyer landing Init
        SELLER_LANDING_INIT = 5008, // seller landing Init
        PRODUCT_LANDING_INIT = 5009, // product landing Init
        Home_Promotional = 5010,// Home Promotional news

        ALL_COUNTRY = 5011, // All Country
        ALL_COUNTRY_STATE = 5012,   // Country Selected State
        ALL_STATE_CITY = 5013,   // State selected cities
        ASSET_CATEGORY_HOME = 5014, // Asset category with product home
        PRODUCT_CATALOG = 5015, // Asset category with product catalog
        AUTO_COMPLETE_CITY_STATE_COUNTRY = 5016, // Auto complete city state country
        ASSET_BANNER_ADVER_HOME = 5017,   // Asset category Home page banner advertise


        GET_PRODUCTLIST = 5018, // Get Product List
        GET_SIZELIST = 5019, // Get Size List 
        GET_PRODUCERTYPELIST = 5020, // Get ProducerType List
        GET_GRADELIST = 5021, // Get Grade List
        GET_MAKELIST = 5022,  // Get Make List
        GET_CONTENTSBYID = 5023,
        GET_MAKEPRODUCERBYPRODUCT = 5024,
        GET_LEADINGPRODUCTS = 5025,
        GET_MEDIACOVERAGE = 5026,
        GET_CLIENTSPEAK = 5027,
        GET_MEDIA_FOR_INDEX = 5028,
        GET_CLIENT_FOR_INDEX = 5029,
        GET_INDUSTRY_ALERT = 5030,
        GET_PRODUCTDATALIST = 5031, // Get All productwise size,Producer,Grade,Make list  
        GET_Live_Rate_Home_Page = 5032,
        SRCH_BUYER_WINDOW_CATEGORY_INIT = 5033,  // search by category buyer window init
        SRCH_SELLER_WINDOW_CATEGORY_INIT = 5034, // search by category seller window init

        SRCH_PRODUCT_COUNTRY = 5035,    // Country Product Search
        SRCH_BY_COMPANY_COUNTRY = 5036, // Country Company Search
        SRCH_BY_COMPANY_STATE = 5037,   // State Company Search
        SRCH_BY_COMPANY_CITY = 5038,    // City Company Search
        SRCH_BY_COMPANY_ORGBISTYPE = 5039,    // Organization Business Type Company Search
        SRCH_BY_COMPANY_SERVICES = 5040,    // Services Company Search
        GetProductCatelog=5041,
        MarketWatchHome=5043,
        MarketWatchSystem = 5044,

    };

    public enum App_CacheKeyTimeInSeconds : long
    {
        // these are keys used in memory cacher. these represent the time for which the cache is being created
        HOME_AVL_BRANDS = 60 * 60, // Home Page Available Brands
        HOME_AVL_BRANDS1 = 60 * 60,
        HOME_AVL_PRODUCTS = 60 * 60,
        GetProductCatelog = 60 * 60,
        SRCH_PRD_GETAUTOPRODUCTNAME = 60 * 60,
        SRCH_MB_GETAUTOMAKENAME = 60 * 60,
        HOME_LEADING_PRODUCT = 60 * 5, // Home page Leading Product
        HOME_SOURCE_BY_STATE = 60 * 5, // home page source by state
        VIEW_STATEDATA_SOURCE_BY_STATE = 60 * 5, // View State Data - source by state
        HOME_NEWS = 60 * 60, // home news
        SRCH_BY_COMPANY_INIT = 60 * 15, // search by company init
        SRCH_BUYER_WINDOW_INIT = 60 * 15, // search by buyer window init
        BUYER_LANDING_INIT = 60 * 15, // buyer landing Init
        SRCH_BUYER_WINDOW_CACHE_FILTER = 60 * 15, // search buyer window Cache data like size,make,grade,producer
        SRCH_SELLER_WINDOW_INIT = 60 * 15, // search by seller window init
        SELLER_LANDING_INIT = 60 * 15, // seller landing Init
        SRCH_SELLER_WINDOW_CACHE_FILTER = 60 * 15, // search seller window Cache data like size,make,grade,producer
        SRCH_BY_PRODUCT_INIT = 60 * 15, // search by product init
        PRODUCT_LANDING_INIT = 60 * 15, //  product landing Init
        ORG_BUSINESS_TYPE_SRCH_PRODUCT = 60 * 15, // org -business-type Search product
        SEC_BUSINESS_TYPE_SRCH_PRODUCT = 60 * 15, // sec -business-type Search product
        ASSETCATEGORY_PRODUCT = 60 * 15,  // asset category search product
        PRODUCT_SRCH_PRODUCT = 60 * 15,   // Product Search product
        SRCH_PRODUCT_STATES = 60 * 15,   // States Product Search
        SRCH_PRODUCT_CITIES = 60 * 15,   // Cities Product Search
        SRCH_BY_PRODUCT_CACHE_FILTER = 60 * 15, // search by product Cache data like size,make,grade,producer
        SRCH_PRODUCT_CITY = 60 * 15,  // Search Product City
        Home_Promotional = 60 * 15,

        ALL_COUNTRY = 60 * 60, // All Country
        ALL_COUNTRY_STATE = 60 * 60,   // Country Selected State
        ALL_STATE_CITY = 60 * 60,  // State selected cities
        ASSET_CATEGORY_HOME = 60 * 30, // Asset category with product home
        PRODUCT_CATALOG = 60 * 30, // Asset category with product catalog
        AUTO_COMPLETE_CITY_STATE_COUNTRY = 60 * 30, // Auto complete city state country
        ASSET_BANNER_ADVER_HOME = 60 * 15,   // Asset category Home page banner advertise

        GET_PRODUCTLIST = 60 * 15, // Get Product List
        GET_SIZELIST = 60 * 15, // Get Size List 
        GET_PRODUCERTYPELIST = 60 * 15, // Get ProducerType List
        GET_GRADELIST = 60 * 15, // Get Grade List
        GET_MAKELIST = 60 * 15,  // Get Make List
        GET_CONTENTSBYIDLIST = 60 * 15,
        GET_MAKEPRODUCERBYPRODUCTLIST = 60 * 15,
        GET_LEADINGPRODUCTLIST = 60 * 15,
        GET_MEDIACOVERAGELIST = 60 * 15,
        GET_CLIENTSPEAKFORHOME = 60 * 15,
        GET_MEDIA_FOR_INDEXLIST = 60 * 15,
        GET_CLIENT_FOR_INDEXLIST = 60 * 15,
        GET_INDUSTRY_ALERT = 60 * 15,
        GET_Live_Rate_Home_Page = 60 * 15,
        GET_PRODUCTDATALIST = 60 * 15, // Get All productwise size,Producer,Grade,Make list  
        SRCH_BUYER_WINDOW_CATEGORY_INIT = 60 * 15,   // search by category buyer window init
        SRCH_SELLER_WINDOW_CATEGORY_INIT = 0,  // search by category seller window init

        SRCH_PRODUCT_COUNTRY = 60 * 15,     // Country Product Search
        SRCH_BY_COMPANY_COUNTRY = 60 * 15,  // Country Company Search
        SRCH_BY_COMPANY_STATE = 60 * 15,    // State Company Search
        SRCH_BY_COMPANY_CITY = 60 * 15,     // City Company Search
        SRCH_BY_COMPANY_ORGBISTYPE = 60 * 15,    // Organization Business Type Company Search
        SRCH_BY_COMPANY_SERVICES = 60 * 15,    // Services Company Search
        MarKetWatch_Time = 60 * 60,
        MarketWatchSystem_Time = 60 * 60,

    };

   
}
