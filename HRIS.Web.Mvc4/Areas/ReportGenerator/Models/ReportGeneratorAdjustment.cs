using HRIS.Domain.Global.Constant;
using project.Web.Mvc4.Areas.ReportGenerator.Models;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.Navigation;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using System.Collections.Generic;
using System.Linq;

namespace Project.Web.Mvc4.Areas.ReportGenerator.Models
{
    public class ReportGeneratorAdjustment : ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {
            if (module.ModuleId.Equals(ModulesNames.ReportGenerator))
            {
                module.Aggregates.SingleOrDefault(x => x.TypeFullName == (typeof(Souccar.ReportGenerator.Domain.QueryBuilder.Report).FullName))
                    .Details.SingleOrDefault(x => x.TypeFullName == (typeof(QueryTree).FullName)).Details.Clear();

            }
        }

        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {
                parent.Add("QueryTree", new QueryTreeViewModel());
                parent.Add("ReportTemplate", new ReportTemplateViewModel());

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