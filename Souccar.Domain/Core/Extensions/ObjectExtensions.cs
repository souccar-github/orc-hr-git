using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Core.Extensions
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class ObjectExtensions
    {
        public static object CallMethod(this object obj, string methodName, Type[] paramTypes = null, object[] param = null)
        {
            var type = obj.GetType();
            var methodInfo = paramTypes == null ? type.GetMethod(methodName) : type.GetMethod(methodName, paramTypes);
            return methodInfo.Invoke(obj, param);
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            return propertyInfo.GetValue(obj,null);
        }

        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            if (propertyInfo.CanWrite)
                propertyInfo.SetValue(obj, value,null);
        }

        public static object To(this object obj, Type type)
        {
            if (obj == null || obj == string.Empty)
                return null;
            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return obj.ToString().ParseDateTime();
            
            return Convert.ChangeType(obj, type);
        }

    }
}
