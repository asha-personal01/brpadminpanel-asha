//Developed by Sunil

using D1WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.Utilities.GeneralConfiguration
{
    public class GeneralConfiguration
    {
        public static List<Configuration> GetSystemGeneralConfiguration = new List<Configuration>();
        private static DateTime? DateTimeForList = null;
        public static List<Configuration> GetConfigurationList()
        {
            int minutes = 0;
            if (DateTimeForList != null)
            {
                TimeSpan t1 = (DateTime)D1WebApp.Utilities.Miscellaneous.CurrentDateTime() - (DateTime)DateTimeForList;
                minutes = (int)t1.TotalMinutes;
            }
            
            if (DateTimeForList == null || minutes >= 60 || GetSystemGeneralConfiguration.Count() == 0)
            {
                GetSystemGeneralConfiguration = new List<Configuration>();
                using (D1WebAppEntities context = new D1WebAppEntities())
                {
                    GetSystemGeneralConfiguration = context.Configurations.ToList();
                }
                DateTimeForList = D1WebApp.Utilities.Miscellaneous.CurrentDateTime();
            }
            return GetSystemGeneralConfiguration;
        }

        public static Configuration CheckConfiguration(string key)
        {
            int minutes = 0;
            if (DateTimeForList != null)
            {
                TimeSpan t1 = (DateTime)D1WebApp.Utilities.Miscellaneous.CurrentDateTime() - (DateTime)DateTimeForList;
                minutes = (int)t1.TotalMinutes;
            }
            if (DateTimeForList == null || minutes >= 1)
            {
                GetSystemGeneralConfiguration = new List<Configuration>();
                GetConfigurationList();
            }
            if (GetSystemGeneralConfiguration.Count() == 0)
            {
                GetConfigurationList();
                Configuration configurationdata = GetSystemGeneralConfiguration.Where(a => a.ConfigurationKey.ToLower().Equals(key.ToLower())).FirstOrDefault();
                return configurationdata;
            }
            else
            {

                Configuration configurationdata = GetSystemGeneralConfiguration.Where(a => a.ConfigurationKey.ToLower().Equals(key.ToLower())).FirstOrDefault();
                return configurationdata;
            }


        }


        
    }
}