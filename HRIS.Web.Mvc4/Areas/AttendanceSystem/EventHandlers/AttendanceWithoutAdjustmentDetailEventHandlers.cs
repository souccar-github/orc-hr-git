using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.DocumentView;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;

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
        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            List<IAggregateRoot> entities = new List<IAggregateRoot>();
            var detail = (AttendanceDailyAdjustmentDetail)entity;
            var dailyRecord = ServiceFactory.ORMService.All<DailyEnternaceExitRecord>().FirstOrDefault(x => x.Date == detail.Date);
            dailyRecord.IsCalculated = false;
            entities.Add(dailyRecord);
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
    }
}