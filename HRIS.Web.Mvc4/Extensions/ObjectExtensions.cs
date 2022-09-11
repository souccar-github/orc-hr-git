using Souccar.Domain.DomainModel;
using Souccar.Reflector;
using System;
using Souccar.Domain.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Extensions
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class ObjectExtensions
    {
        public static IDictionary<string, object> ToDynamicObj(this object obj)
        {
            var type = obj.GetType();
            return type.ToDynamicData((Entity) obj);
        }
    }
}