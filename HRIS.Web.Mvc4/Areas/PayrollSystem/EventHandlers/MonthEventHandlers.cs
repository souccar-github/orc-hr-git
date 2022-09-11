using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.RootEntities;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class MonthEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(MonthEventHandlers).FullName;
            model.Views[0].EditHandler = "Month_EditHandler";
            model.SchemaFields.SingleOrDefault(x => x.Name == "MonthStatus").Editable = false;
            model.IsEditable = false;
            model.ActionListHandler = "RemoveDeleteForLockedMonth";
            //var Month = ServiceFactory.ORMService.GetById<Month>(requestInformation.NavigationInfo.Previous[0].RowId);
            //if (Month.MonthStatus == MonthStatus.Approved || Month.MonthStatus == MonthStatus.Locked)
            //{

            //    model.ActionList.Commands.RemoveAt(2);
            //}
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var month = (Month)entity;
            if (month.ImportFromAttendance)
            {
                var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().FirstOrDefault(x =>
                           x.AttendanceMonthStatus == HRIS.Domain.AttendanceSystem.Enums.AttendanceMonthStatus.Locked &&
                           x.Month == month.MonthName && x.Year == month.Year);
                month.FromDate = attendanceRecord != null ? attendanceRecord.FromDate : new DateTime(month.Year, (int)month.MonthName, 1);
                month.ToDate = attendanceRecord != null ? attendanceRecord.ToDate : new DateTime(month.Year, (int)month.MonthName, DateTime.DaysInMonth(month.Year, (int)month.MonthName));
            }
            else
            {
                month.FromDate = new DateTime(month.Year, (int)month.MonthName, 1);
                month.ToDate = new DateTime(month.Year, (int)month.MonthName, DateTime.DaysInMonth(month.Year, (int)month.MonthName));
            }
            var MonthNameExist = ServiceFactory.ORMService.All<Month>().Where(a => a.Name.Equals(month.Name) && a.Id != month.Id).FirstOrDefault();
            if (MonthNameExist != null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(CustomMessageKeysAttendanceSystemModule.TheMonthNameIsAlreadyExists)),
                    Property = null
                });
                return;
            }


            //todo Mhd Alsadi: منع تعديل نوع الشهر بعد اضافته
            if (entity.IsTransient())
            {
                var recordCount = typeof(Month).GetAll<Month>().Count(x => month.MonthType == MonthType.SalaryAndBenefit && x.MonthType == MonthType.SalaryAndBenefit && (x.MonthStatus != MonthStatus.Locked && x.MonthStatus != MonthStatus.Rejected));
                if (recordCount > 0)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotCreateNewMonthWhileNotAllPreviousMonthsNotLocked)),
                        Property = null
                    });
                }
            }
        }
        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var month = (Month)entity;
            if (month.MonthStatus == MonthStatus.Locked || month.MonthStatus == MonthStatus.Approved)
            {
                PreventDefault = true;
            }
        }


    }
}