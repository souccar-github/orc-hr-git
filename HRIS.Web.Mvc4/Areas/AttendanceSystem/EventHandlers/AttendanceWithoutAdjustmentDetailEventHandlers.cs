using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.DocumentView;
using HRIS.Domain.AttendanceSystem.Entities;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;

namespace Project

    .Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class AttendanceWithoutAdjustmentDetailEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AttendanceWithoutAdjustmentDetailEventHandlers).FullName;
            model.ActionList.Commands.RemoveAt(2);
            model.ToolbarCommands.RemoveAt(0);
            model.Views[0].ViewHandler = "RemoveEditButtonViewHandler";
        }
        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }

        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            Type typeOfClass = typeof(AttendanceWithoutAdjustmentDetail);
            var allBeforeFilter = ((IQueryable<AttendanceWithoutAdjustmentDetail>)result.Data).ToList();
            result.Data = allBeforeFilter.OrderByDescending(x => x.Date).Skip(skip).Take(pageSize).AsQueryable();
            result.Total = allBeforeFilter.Count();
        }
    }
}