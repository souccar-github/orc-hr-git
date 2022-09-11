using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Souccar.Infrastructure.Extenstions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Auther: Yaseen Alrefaee
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetDataSource(this Type type)
        {
            var result = new List<Dictionary<string, object>>();
            if (type == null||!type.IsEnum)
                return null;
            var values = Enum.GetValues(type);
            foreach (var value in values)
            {
                var data = new Dictionary<string, object>();
                var name = ServiceFactory.LocalizationService.GetResource(type.FullName + "." + value.ToString());
                data["Name"] = !string.IsNullOrEmpty(name) ? name : value.ToString().ToCapitalLetters();
                data["Id"] = (int)value;
                result.Add(data);
            }
            return result;
        }
    }
}
