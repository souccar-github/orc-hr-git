using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Helpers
{
    public class ThemingHelper
    {
        public const ThemingType DefaultTheme = ThemingType.Orange;
        public static readonly List<ThemingType> Themes = new List<ThemingType>() { ThemingType.Formal, ThemingType.Lady, ThemingType.LightBlue, ThemingType.Orange, ThemingType.Sepia };
        public static bool IsSupportedTheme(string theme)
        {
            return Themes.Any(x => (x.ToString().Equals(theme)) || (((int)x).ToString().Equals(theme)));
        }
    }
}