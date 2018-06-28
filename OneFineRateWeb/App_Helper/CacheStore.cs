using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace OneFineRateWeb.Handlers
{
    internal static class CacheStore
    {
        internal static void AddToCache(string key, object o, int durationInMinutes)
        {
            Debug.WriteLine("Added to cache: " + key);
            WebCache.Set(key, o, durationInMinutes, false);
        }

        internal static dynamic Get(string key)
        {
            return WebCache.Get(key);
        }
    }
}