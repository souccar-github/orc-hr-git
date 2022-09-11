using HRIS.Domain.Global.Constant;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Models.Navigation;
using Souccar.Domain.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Reporting.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class ReportingAdjustment : ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {

        }

        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("ReportDefinition", new ReportDefinitionViewModel());


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