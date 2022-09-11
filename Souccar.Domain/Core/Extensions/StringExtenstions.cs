using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Souccar.Core.Fasterflect;

namespace Souccar.Core.Extensions
{
    public static class StringExtenstions
    {
        public static string ToCapitalLetters(this string value)
        {
            return Regex.Replace(value, @"(?<a>[a-z])(?<b>[A-Z0-9])", @"${a} ${b}");
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(this string s)
        {
             return Convert.ToDateTime(s, CultureInfo.GetCultureInfo("ar-SY"));
            // return Convert.ToDateTime(s, CultureInfo.GetCultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name));
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type ToType(this string className)
        {
            return className == null ? null : AppDomain.CurrentDomain.GetType(className);
        }

        /// <summary>
        /// Author:Yaseen Alrefaee
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ConvertToType(this string obj, Type type)
        {
            if (string.IsNullOrEmpty(obj))
                return obj;
            return type == typeof(DateTime) ? obj.ParseDateTime() : Convert.ChangeType(obj, type);
        }
    }
}
