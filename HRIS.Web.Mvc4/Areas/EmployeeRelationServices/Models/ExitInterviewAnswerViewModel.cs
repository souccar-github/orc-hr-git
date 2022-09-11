using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class ExitInterviewAnswerViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ExitInterviewAnswerViewModel).FullName;
            model.ToolbarCommands.RemoveAt(0);
            model.ActionListHandler = "initializeExitInterviewActionList";
            model.Views[0].ViewHandler = "ExitInterviewViewHandler";
        }
    }
}