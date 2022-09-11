using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class ExitInterviewViewModel : ViewModel
    {    
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ExitInterviewViewModel).FullName;
            model.ToolbarCommands.RemoveAt(0);
            model.ActionListHandler = "initializeExitInterviewActionList";
            model.Views[0].ViewHandler = "ExitInterviewViewHandler";
        }
    }
}