using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.Grades.Models
{
    public class GradeBenefitDetailViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(GradeBenefitDetailViewModel).FullName;
            model.Views[0].EditHandler = "GradeBenefit_EditHandler";

        }
    }
}