using D1WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.Models
{
    public partial class Configuration
    {
        public bool Equals(Configuration other)
        {
            return null != other && ConfigurationKey == other.ConfigurationKey;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Configuration);
        }
        public override int GetHashCode()
        {
            return ConfigurationKey.GetHashCode();
        }
    }
}