using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mconrad.azurewebsites.net
{
    public static class Common
    {
        public static string CacheManagerHeaderDescription
        {
            get
            {
                return "CacheManager is a common interface and abstraction layer for caching for .Net written in C#.";
            }
        }

        public static string CacheManagerDescription
        {
            get
            {
                return "CacheManager is a common interface and abstraction layer for caching written in C#.";
            }
        }

        public static string CacheManagerDescriptionOpenSource
        {
            get
            {
                return "It is open source and available via Nuget.";
            }
        }

        public static string CacheManagerDescriptionAdvanced
        {
            get
            {
                return "It supports various cache providers and implements many advanced features.";
            }
        }
    }
}