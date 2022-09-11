using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Souccar.Domain.Localization
{
    public class LanguageFactory
    {
        public static Language Create(CultureInfo cultureInfo)
        {
            return new Language() { Name = cultureInfo.EnglishName, DisplayName = cultureInfo.NativeName };
        }

    }
}
