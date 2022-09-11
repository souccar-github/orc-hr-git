using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{//todo : Mhd Update changeset no.1
    public class OvertimeOrderEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
           
                model.SchemaFields.SingleOrDefault(x => x.Name == typeof(OvertimeOrder).GetPropertyNameAsString<OvertimeOrder>(y => y.Number)).Editable = false;
                model.ViewModelTypeFullName = typeof(OvertimeOrderEventHandlers).FullName;
            
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var overtimeOrder = (OvertimeOrder)entity;

            // عند محاولة قراءة الاضافي  عند حساب الدوام يفترض الشرط ان  الوقت فيها مصفر ولكن هنا القيمة للوقت تساوي الوقت الحالي لذلك سنقوم بتصفير الوقت
            var date = overtimeOrder.FromDate;
            overtimeOrder.FromDate = new DateTime(date.Year, date.Month, date.Day);
            date = overtimeOrder.ToDate;
            overtimeOrder.ToDate = new DateTime(date.Year, date.Month, date.Day);
        }

        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            string customInformation = null)
        {
            var overtimeOrder = (OvertimeOrder)entity;

            // عند محاولة قراءة الاضافي  عند حساب الدوام يفترض الشرط ان  الوقت فيها مصفر ولكن هنا القيمة للوقت تساوي الوقت الحالي لذلك سنقوم بتصفير الوقت
            var date = overtimeOrder.FromDate;
            overtimeOrder.FromDate = new DateTime(date.Year, date.Month, date.Day);
            date = overtimeOrder.ToDate;
            overtimeOrder.ToDate = new DateTime(date.Year, date.Month, date.Day);
        }

        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }

        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            Type typeOfClass = typeof(OvertimeOrder);
            var filteredDate = new List<OvertimeOrder>();
            var allBeforeFilter = ((IQueryable<OvertimeOrder>)result.Data).ToList();
            var user = UserExtensions.CurrentUser;
            if (UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result.Data = allBeforeFilter.Skip<OvertimeOrder>(skip).Take<OvertimeOrder>(pageSize).AsQueryable();
                result.Total = allBeforeFilter.Count();
            }
            else
            {
                var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
                filteredDate = allBeforeFilter.Where(x => employeesCanViewThem.Any(y => y.Id == x.Employee.Id)).ToList();
                result.Data = filteredDate.Skip<OvertimeOrder>(skip).Take<OvertimeOrder>(pageSize).AsQueryable();
                result.Total = filteredDate.Count();
            }
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var overtimeOrder = (OvertimeOrder)entity;

            if (!typeof(OvertimeOrder).GetAll<OvertimeOrder>().Any())
            {
                overtimeOrder.Number = 1;
            }
            else
            {
                overtimeOrder.Number = typeof(OvertimeOrder).GetAll<OvertimeOrder>().Max(x => x.Number) + 1;
            }

            var conflictEntityWithStartRange =
                    typeof(OvertimeOrder).GetAll<OvertimeOrder>().FirstOrDefault(x => overtimeOrder.FromDate > x.FromDate && overtimeOrder.FromDate < x.ToDate);
            var conflictEntityWithEndRange =
                                typeof(OvertimeOrder).GetAll<OvertimeOrder>().FirstOrDefault(x => overtimeOrder.ToDate > x.FromDate && overtimeOrder.ToDate < x.ToDate);

            if (conflictEntityWithStartRange != null && overtimeOrder.Id != conflictEntityWithStartRange.Id)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.ConflictSlices)),
                    Property = typeof(OvertimeOrder).GetProperty("FromDate")
                });
            }

            if (conflictEntityWithEndRange != null && overtimeOrder.Id != conflictEntityWithEndRange.Id)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.ConflictSlices)),
                    Property = typeof(OvertimeOrder).GetProperty("ToDate")
                });
            }
        }
    }
}