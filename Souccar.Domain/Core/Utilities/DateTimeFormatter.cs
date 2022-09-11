using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Souccar.Core.Utilities
{
    public static class DateTimeFormatter
    {
        public static string ConvertDoubleToTimeFormat(double number)
        {
            var timeSpan = TimeSpan.FromHours(number);
            var hours = (timeSpan.Days * 24) + timeSpan.Hours;
            var minuts = timeSpan.Minutes;
            var result = (hours.ToString().Length == 1 ? "0" + hours : hours.ToString(CultureInfo.InvariantCulture)) + ":" + (minuts.ToString().Length == 1 ? "0" + minuts : minuts.ToString(CultureInfo.InvariantCulture));
            return result;
        }
    }
}
