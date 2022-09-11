using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Souccar.Core.CustomAttribute;

namespace Souccar.Core.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool IsSimpleProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(decimal);
        }

        public static bool IsCollectionProperty(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType != typeof(string) && propertyInfo.PropertyType.GetInterfaces().Any(inter => inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static int GetOrder(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).Order : 0;
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetGroupName(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            var result= attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).Group : string.Empty;
            return result ?? "";
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetReferenceReadUrl(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            var result = attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).ReferenceReadUrl : string.Empty;
            return result ?? "";
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetCascadeFrom(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            var result = attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).CascadeFrom : string.Empty;
            return result ?? "";
        }
        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static int GetStep(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).Step : 1;
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static int GetWidth(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).Width : 0;
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool GetIsReference(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 && ((UserInterfaceParameterAttribute)attrs.First()).IsReference;
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool GetIsDateTime(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 && ((UserInterfaceParameterAttribute)attrs.First()).IsDateTime;
        }
        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool GetIsTime(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 && ((UserInterfaceParameterAttribute)attrs.First()).IsTime;
        }
        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool GetIsFile(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 && ((UserInterfaceParameterAttribute)attrs.First()).IsFile;
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetAcceptExtension(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            var result= attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).AcceptExtension : string.Empty;
            return result ?? "";
        }

        public static int GetFileSize(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            var result = attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).FileSize : 0;
            return result ;
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool GetIsHidden(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 && ((UserInterfaceParameterAttribute)attrs.First()).IsHidden;
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool GetIsNonEditable(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 && ((UserInterfaceParameterAttribute)attrs.First()).IsNonEditable;
        }
        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetImagePath(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0? ((UserInterfaceParameterAttribute)attrs.First()).ImageColumnPath:"";
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetDefaultImageName(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).DefaultImageName : "";
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static IList<int> GetViewsIds(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).ViewsIds : null;
        }

        public static string GetDisplayFieldName(this PropertyInfo propertyInfo)
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(UserInterfaceParameterAttribute), false);
            return attrs.Count() != 0 ? ((UserInterfaceParameterAttribute)attrs.First()).DisplayFieldName : "";
        }
    }
}
