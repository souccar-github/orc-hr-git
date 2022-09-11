using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web;
using  Project.Web.Mvc4.Areas.OrganizationChart.Models;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.Grades.Models
{
    public class GradeDeductionDetailViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(GradeDeductionDetailViewModel).FullName;
            model.Views[0].EditHandler = "GradeDeduction_EditHandler";

        }
    }
}