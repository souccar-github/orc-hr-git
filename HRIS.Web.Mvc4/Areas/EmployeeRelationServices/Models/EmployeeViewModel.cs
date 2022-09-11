using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{

    public class EmployeeViewModel : ViewModel
    {

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeViewModel).FullName;
            model.IsAddable = false;
            model.IsEditable = false;
            model.IsDeleteable = false;
            model.ActionListHandler = "";
            var columns = new List<string>() { "FirstName", "LastName", "PhotoPath", "Code", "FullName", "Gender", "Age", "Status", "EmploymentStatus", "SalaryStatus" };
            foreach (var column in model.Views[0].Columns)
            {
                column.Hidden = true;
            }
            foreach (var column in model.Views[0].Columns.Where(x => columns.Contains(x.FieldName)))
            {
                column.Hidden = false;
            }

        }
    }
}