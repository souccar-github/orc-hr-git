using Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeeLoanRequestViewModel: ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeLoanRequestViewModel).FullName;

            model.ActionListHandler = "initializeEmployeeLoanRequestActionList";
            if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));
        }

        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public int FirstRepresentative { get; set; }
        public int SecondRepresentative { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public string SecondRepresentativeName { get; internal set; }
        public string FirstRepresentativeName { get; internal set; }
        public double TotalAmount { get; set; }
        public int PaymentsCount { get; set; }
        public double MonthlyInstallment { get; set; }
    }
}