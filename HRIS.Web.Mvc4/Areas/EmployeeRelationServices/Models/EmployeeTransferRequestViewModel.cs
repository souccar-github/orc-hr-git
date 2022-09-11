using Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeeTransferRequestViewModel: ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeTransferRequestViewModel).FullName;

            model.ActionListHandler = "initializeEmployeeTransferRequestActionList";
            if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));
        }

        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public int DestNode { get; set; }
        public int DestPosition { get; set; }
        public int SourceNode { get; set; }
        public int SourcePosition { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
        public string Note { get; set; }
        public string DestPositionName { get; internal set; }
        public string DestNodeName { get; internal set; }
        public string SourcePositionName { get; internal set; }
        public string SourceNodeName { get; internal set; }
    }
}