using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class NonAttendanceSlicePercentageEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(NonAttendanceSlicePercentageEventHandlers).FullName;
            model.Views[0].EditHandler = "nonAttendanceSlicePercentageHandler";
        }
    }
}