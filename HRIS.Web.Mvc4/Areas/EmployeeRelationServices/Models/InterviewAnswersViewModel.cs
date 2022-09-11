using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class InterviewAnswersViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.IsAddable = false;
        }
    }
}