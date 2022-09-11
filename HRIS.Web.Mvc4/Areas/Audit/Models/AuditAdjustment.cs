using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Audit.Models
{
    public class AuditAdjustment : ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();

        public override void AdjustModule(Module module)
        {
        }

        public override ViewModel AdjustGridModel(string type)
        {

            if (parent.Count == 0)
            {
                parent.Add("Log", new LogViewModel());
            }
            try
            {
                return parent[type];
            }
            catch
            {

                return new ViewModel();
            }
        }

        
    }
}