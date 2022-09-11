using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Payroll.Entities;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Models.Navigation;
using Souccar.Domain.Localization;
using  Project.Web.Mvc4.Models;

namespace Project.Web.Mvc4.Areas.Payroll.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    ///  
    
    public class PayrollAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public override void AdjustModule(Module module)
        {
            module.Aggregates.Add(new Aggregate()
            {
                Action = "BrowseEmployee",
                Controller = "Payroll/PayrollEmployee",
                AggregateId = "BrowseEmployee",
                Title = "Browse Employee"
            });
            module.Aggregates.Add(new Aggregate()
            {
                Action = "EmployeeFinanceCard",
                Controller = "Payroll/PayrollEmployee",
                AggregateId = "EmployeeFinanceCard",
                Title = "Employee Finance Card "
            });
        }

        public override ViewModel AdjustGridModel(string type)
        {


            if (parent.Count == 0)
            {
                parent.Add("EmployeeBenefit", new EmployeeBenefitViewModel());
                parent.Add("EmployeeDeduction", new EmployeeDeductionViewModel());




            }
            try
            {
                return parent[type];
            }
            catch
            {

                return new ViewModel();
            }

            //if (type == typeof(LocaleStringResource))
            //{
            //    model.Views[1].EditorMode = GridEditorMode.Inline.ToString().ToLower();
            //}
        }
    }
}