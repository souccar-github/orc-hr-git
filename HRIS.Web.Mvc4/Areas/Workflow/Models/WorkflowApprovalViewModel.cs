using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraCharts.Native;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Factories;
using Souccar.Core.Extensions;
using System;

namespace Project.Web.Mvc4.Areas.Workflow.Models
{
    public class WorkflowApprovalViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {

            model.ViewModelTypeFullName = typeof(WorkflowStepViewModel).FullName;
            var editLocalizationName = ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + GridModelLocalizationConst.Edit) ?? GridModelLocalizationConst.Edit.ToCapitalLetters();
            var deleteLocalizationName = ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + GridModelLocalizationConst.Delete) ?? GridModelLocalizationConst.Delete.ToCapitalLetters();
            if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));
            if (model.ActionList.Commands.Any(x => x.Name == editLocalizationName))
                model.ActionList.Commands.Remove(model.ActionList.Commands.FirstOrDefault(x => x.Name == editLocalizationName));
            if (model.ActionList.Commands.Any(x => x.Name == deleteLocalizationName))
                model.ActionList.Commands.Remove(model.ActionList.Commands.FirstOrDefault(x => x.Name == deleteLocalizationName));
        }
    }
}