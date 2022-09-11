using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Payroll.Models
{
    public class EmployeeBenefitViewModel:ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            //GridViewModelFactory.AddRefField(model, new List<int>() { 0 }, "Benefit", "Benefit", typeof(HRIS.Domain.Payroll.RootEntities.Benefit).FullName, 160, model.Views[0].Columns.Max(x => x.Order) + 1, "Reference/ReadToList/");//"Reference/ReadToList/"
        }
    }
    public class EmployeeDeductionViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            //GridViewModelFactory.AddRefField(model, new List<int>() { 0 }, "Deduction", "Deduction", typeof(HRIS.Domain.Payroll.RootEntities.Deduction).FullName, 160, model.Views[0].Columns.Max(x => x.Order) + 1, "Reference/ReadToList/");//"Reference/ReadToList/"
        }
    }
}