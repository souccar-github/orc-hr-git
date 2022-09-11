using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class AttendanceDailyAdjustmentDetailEventHandlers:ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ActionList.Commands.RemoveAt(2);
            model.ToolbarCommands.RemoveAt(0);
            model.Views[0].ViewHandler = "RemoveEditButtonViewHandler";
            model.ViewModelTypeFullName = typeof(AttendanceDailyAdjustmentDetailEventHandlers).FullName;
        }
    }
}