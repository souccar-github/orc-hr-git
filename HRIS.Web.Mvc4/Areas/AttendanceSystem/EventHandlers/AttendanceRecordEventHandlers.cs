using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.Entities;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class AttendanceRecordEventHandlers : ViewModel
    {


        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            //model.ActionList.Commands.Add(new ActionListCommand()
            //{
            //    GroupId = 1, 
            //    Order = 1,
            //    HandlerName = "GenerateAttendanceRecord",
            //    Name = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.GenerateTitle)),
            //    ShowCommand = true
            //});
            //model.ActionList.Commands.Add(new ActionListCommand()
            //{
            //    GroupId = 1,
            //    Order = 2,
            //    HandlerName = "CalculateAttendanceRecord",
            //    Name = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.CalculateTitle)),
            //    ShowCommand = true
            //});
            //model.ActionList.Commands.Add(new ActionListCommand()
            //{
            //    GroupId = 1,
            //    Order = 2,
            //    HandlerName = "LockAttendanceRecord",
            //    Name = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.LockTitle)),
            //    ShowCommand = true
            //});

            model.SchemaFields.SingleOrDefault(x => x.Name ==
                                                    typeof (AttendanceRecord).GetPropertyNameAsString<AttendanceRecord>(
                                                        y => y.AttendanceMonthStatus)).Editable = false;
            model.ViewModelTypeFullName = typeof (AttendanceRecordEventHandlers).FullName;
            model.Views[0].EditHandler = "attendanceRecordEditHandler";
            model.Views[0].GridHandler = "attendanceRecordGridHandler";
            //model.IsEditable = false;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity,
            string customInformation = null)
        {
            var attendanceRecord = (AttendanceRecord) entity;
            attendanceRecord.AttendanceMonthStatus = AttendanceMonthStatus.Created;
        }
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var attendanceRecord = (AttendanceRecord)entity;

            List<AttendanceInfraction> attendanceInfractions = ServiceFactory.ORMService.All<AttendanceInfraction>().Where(x => x.IsActiveForNextPenalties && x.AttendanceRecord == attendanceRecord).ToList();
            ServiceFactory.ORMService.DeleteTransaction(attendanceInfractions, UserExtensions.CurrentUser);
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity,
            IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var attendanceRecord = (AttendanceRecord) entity;
            attendanceRecord.FromDate = new DateTime(attendanceRecord.FromDate.Year, attendanceRecord.FromDate.Month, attendanceRecord.FromDate.Day);
            attendanceRecord.ToDate = new DateTime(attendanceRecord.ToDate.Year, attendanceRecord.ToDate.Month, attendanceRecord.ToDate.Day);
            if (attendanceRecord.Month == HRIS.Domain.Global.Enums.Month.Nothing)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(
                                CustomMessageKeysAttendanceSystemModule.MonthCanNotBeNothing)),
                    Property = attendanceRecord.GetType().GetProperty("Month")
                });
            }

            var RecordNumberExist = typeof(AttendanceRecord).GetAll<AttendanceRecord>().Count(x => x.Id != attendanceRecord.Id && (x.Year == attendanceRecord.Year && x.Month == attendanceRecord.Month));
            if (RecordNumberExist > 0)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource( CustomMessageKeysAttendanceSystemModule.GetFullKey(
                                CustomMessageKeysAttendanceSystemModule.AttendanceRecordYearAndMonthMustBeUnique)),
                    Property = null
                });
            }

            var recordCount =
                    typeof (AttendanceRecord).GetAll<AttendanceRecord>() .Count(x => x.Id != attendanceRecord.Id && x.Name == attendanceRecord.Name);
                if (recordCount > 0)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(
                                    CustomMessageKeysAttendanceSystemModule.AttendanceRecordNameMustBeUnique)),
                        Property = null
                    });
                }
           
            var fromeDateMonth = attendanceRecord.FromDate.Month;
            var tomeDateMonth = attendanceRecord.ToDate.Month;
            recordCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>().Count(x => x.Id != attendanceRecord.Id &&
            //fromeDateMonth != tomeDateMonth
            x.FromDate <= attendanceRecord.ToDate && x.FromDate >= attendanceRecord.FromDate);
            if (recordCount > 0)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule
                                    .ThereIsMonthIntersectWithAnotherMonth)),
                    Property = null
                });
            }

            recordCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>().Count(x => x.Id != attendanceRecord.Id && x.ToDate < attendanceRecord.FromDate && x.AttendanceMonthStatus != AttendanceMonthStatus.Locked);
            if (recordCount > 0)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule
                                    .CannotCreateAttendanceRecordWhileAllPreviousAttendanceRecordNotLocked)),
                    Property = null
                });
            }

        }

    }
}