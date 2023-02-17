using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.AttendanceSystem.Entities;
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
        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }
        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            var allBeforeFilter = ((IQueryable<AttendanceDailyAdjustmentDetail>)result.Data).ToList();
            result.Data = allBeforeFilter.OrderByDescending(x => x.Date).Skip(skip).Take(pageSize).AsQueryable();
            result.Total = allBeforeFilter.Count();
        }
    }
}