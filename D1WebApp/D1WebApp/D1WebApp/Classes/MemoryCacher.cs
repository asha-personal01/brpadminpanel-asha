using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;

using D1WebApp.Constants;

namespace D1WebApp.Utilities
{
    
    public static class MemoryCacher
    {
        private static MemoryCache memoryCache = MemoryCache.Default;


        public static object GetValue(string key)
        {
            return memoryCache.Get(key);
        }


        public static bool Add(string key, object value, long timeOffsetInSeconds)
        {
            DateTimeOffset absExpiration = DateTimeOffset.UtcNow.AddSeconds(timeOffsetInSeconds);
            return memoryCache.Add(key, value, absExpiration);
        }

        public static void Delete(string key)
        {
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }

        public static List<GetListofCacheKey> getCacheKeys()
        {
            List<GetListofCacheKey> allCacheKeys = new List<GetListofCacheKey>();
            
            foreach (var item in memoryCache)
            {
                GetListofCacheKey singleKey = new GetListofCacheKey();

                singleKey.sDisplayKey = item.Key.ToString();
                
                singleKey.sKey = item.Key.ToString();


                allCacheKeys.Add(singleKey);
            }
            return allCacheKeys;
        }

        public static void Delete(long key)
        {
            string sKey = key.ToString();
            Delete(sKey);
        }
    }
    public class GetListofCacheKey
    {
        public string sKey { get; set; }
        public string sDisplayKey { get; set; }
    }
        
}