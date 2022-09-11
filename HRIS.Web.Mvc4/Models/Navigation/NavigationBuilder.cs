using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web;
using System.Web.Mvc;
using  Project.Web.Mvc4.Areas;

namespace Project.Web.Mvc4.Models.Navigation
{
    public abstract class NavigationBuilder
    {
        private static Dictionary<string, NavigationBuilder> parent = new Dictionary<string, NavigationBuilder>();
        public abstract void BuildDomainTab();
            public abstract void BuildLocalizationTab();
            public abstract void BuildReportTab();
        public abstract string BuildTabDesign();


        public abstract string GetStyle();
        
        public abstract IList<NavigationTab> GetNavigationTab();
        }
    }
