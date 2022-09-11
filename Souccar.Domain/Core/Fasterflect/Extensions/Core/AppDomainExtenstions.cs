using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Core.Fasterflect
{
    public static class AppDomainExtenstions
    {
        static Caching.Cache<string, Type> cache = new Caching.Cache<string, Type>();
        //public static Type GetType(this AppDomain appDomain, string typeFullName)
        //{
        //    Type result = cache.Get(typeFullName);
        //    if (result != null)
        //        return result;
        //    else
        //    {
        //        var query = from assembly in appDomain.GetAssemblies()
        //                    from type in assembly.GetTypes()
        //                    where !type.IsFrameworkType() && (type.FullName == typeFullName)
        //                    select type;

        //        result = query.SingleOrDefault();
        //        cache.Insert(typeFullName, result, Caching.CacheStrategy.Permanent);
        //    }

        //    return result;
        //}

        public static Type GetType(this AppDomain appDomain, string typeFullName)
        {
            Type result = cache.Get(typeFullName);
            if (result != null)
                return result;
            else
            {
                var assemblies = appDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    result = assembly.GetType(typeFullName);
                    if (result != null)
                        break;
                }
                cache.Insert(typeFullName, result, Caching.CacheStrategy.Permanent);
            }
            return result;
        }
    }
}
