using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.AttendanceSystem.Entities;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class AttendanceMonthlyAdjustmentEventHandlers:ViewModel

    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AttendanceMonthlyAdjustmentEventHandlers).FullName;
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
            Type typeOfClass = typeof(AttendanceMonthlyAdjustment);
            var filteredDate = new List<AttendanceMonthlyAdjustment>();
            var allBeforeFilter = ((IQueryable<AttendanceMonthlyAdjustment>)result.Data).ToList();
            var user = UserExtensions.CurrentUser;
            if (UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result.Data = allBeforeFilter.Skip<AttendanceMonthlyAdjustment>(skip).Take<AttendanceMonthlyAdjustment>(pageSize).AsQueryable();
                result.Total = allBeforeFilter.Count();
            }
            else
            {
                var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
                filteredDate = allBeforeFilter.Where(x => employeesCanViewThem.Any(y => y.EmployeeCard.Id == x.EmployeeAttendanceCard.Id)).ToList();
                result.Data = filteredDate.Skip<AttendanceMonthlyAdjustment>(skip).Take<AttendanceMonthlyAdjustment>(pageSize).AsQueryable();
                result.Total = filteredDate.Count();
            }
        }
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var detail = (AttendanceMonthlyAdjustment)entity;
            var attendanceRecord = detail.AttendanceRecord;

            List<AttendanceInfraction> attendanceInfractions = ServiceFactory.ORMService.All<AttendanceInfraction>().Where(x => x.IsActiveForNextPenalties && x.AttendanceRecord == attendanceRecord && x.EmployeeCard == detail.EmployeeAttendanceCard).ToList();
            ServiceFactory.ORMService.DeleteTransaction(attendanceInfractions, UserExtensions.CurrentUser);
        }
    }
}