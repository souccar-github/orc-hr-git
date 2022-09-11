using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4
{
    public enum Locale
    {
        Rtl,
        Ltr
    }

    public static class CurrentLocale
    {
        public static Locale Language = Locale.Rtl;
    }
}