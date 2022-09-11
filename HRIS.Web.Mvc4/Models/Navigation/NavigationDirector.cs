using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.Navigation
{
    public class NavigationDirector
    {
            public void   MakeNavigationTab(NavigationBuilder builder)
            {
                builder.BuildDomainTab();
                builder.BuildLocalizationTab();
                builder.BuildReportTab();
            }
        }
    }
