#region

using System;
using System.Web;

#endregion

namespace UI.Helpers.Cache
{
    public static class CacheProvider
    {
        public static T Get<T>(string cacheKey, Func<T> getValuesFunc) where T : class
        {
            var values = HttpRuntime.Cache.Get(cacheKey) as T;

            if (values == null)
            {
                values = getValuesFunc();
                HttpContext.Current.Cache.Insert(cacheKey, values);
            }

            return values;
        }

        public static T GetFromDataSource<T>(string cacheKey, Func<T> getValuesFunc) where T : class
        {
            T values = getValuesFunc();

            HttpContext.Current.Cache.Insert(cacheKey, values);

            return values;
        }

        public static void Set(string cacheKey, object values)
        {
            HttpContext.Current.Cache.Insert(cacheKey, values);
        }

        public static bool ForceUpdate(string cacheKey)
        {
            try
            {
                HttpContext.Current.Cache.Remove(cacheKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}