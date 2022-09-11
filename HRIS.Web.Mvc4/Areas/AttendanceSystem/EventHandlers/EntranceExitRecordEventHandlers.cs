using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using System;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.AttendanceSystem.Configurations;
using Project.Web.Mvc4.Areas.AttendanceSystem.Services;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class EntranceExitRecordEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.SchemaFields.SingleOrDefault(x => x.Name == typeof(EntranceExitRecord).GetPropertyNameAsString<EntranceExitRecord>(y => y.InsertSource)).Editable = false;
            //model.SchemaFields.SingleOrDefault(x => x.Name == typeof(EntranceExitRecord).GetPropertyNameAsString<EntranceExitRecord>(y => y.Status)).Editable = false;
            model.SchemaFields.SingleOrDefault(x => x.Name == typeof(EntranceExitRecord).GetPropertyNameAsString<EntranceExitRecord>(y => y.ErrorType)).Editable = false;
            //model.SchemaFields.SingleOrDefault(x => x.Name == typeof(EntranceExitRecord).GetPropertyNameAsString<EntranceExitRecord>(y => y.ErrorMessage)).Editable = false;
            //model.ToolbarCommands.Add(
            //new ToolbarCommand
            //{
            //    Additional = false,
            //    ClassName = "grid-action-button AcceptEntranceExitRecordButton",
            //    Handler = "",
            //    ImageClass = "",
            //    Name = "AcceptEntranceExitRecordButton",
            //    Text =
            //        ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.AcceptEntranceExitRecords))
            //});
            model.Views[0].GridHandler = "entranceExitGridHandler";
            Type typeOfClass = typeof(EntranceExitRecord);
            if (UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                model.ToolbarCommands.Add(
               new ToolbarCommand
               {
                   Additional = false,
                   ClassName = "grid-action-button DeleteEntranceExitRecordButton",
                   Handler = "",
                   ImageClass = "",
                   Name = "DeleteEntranceExitRecordButton",
                   Text =
                       ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.DeleteEntranceExitRecords))
               });
                model.ToolbarCommands.Add(
               new ToolbarCommand
               {
                   Additional = false,
                   ClassName = "grid-action-button CheckEntranceExitRecordsButton",
                   Handler = "",
                   ImageClass = "",
                   Name = "CheckEnternaceExitRecordsButton",
                   Text =
                       ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.CheckEntranceExitRecords))
               });
            }



            //model.ToolbarCommands.Add(
            //  new ToolbarCommand
            //  {
            //      Additional = false,
            //      ClassName = "grid-action-button GenerateEntranceExitRecordErrorsButton",
            //      Handler = "",
            //      ImageClass = "",
            //      Name = "GenerateEntranceExitRecordErrorsButton",
            //      Text =
            //          ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.GenerateEntranceExitRecordError))
            //  });

            model.Views[0].AfterRequestEnd = "EntranceExitRecordAfterRequestEnd";

            model.Views[0].EditHandler = "EntranceExitRecordEditHandler";

            model.ViewModelTypeFullName = typeof(EntranceExitRecordEventHandlers).FullName;
        }
        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }

        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            Type typeOfClass = typeof(EntranceExitRecord);
            var filteredDate = new List<EntranceExitRecord>();
            var allBeforeFilter = ((IQueryable<EntranceExitRecord>)result.Data).ToList();
            var user = UserExtensions.CurrentUser;
            if (UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result.Data = allBeforeFilter.OrderByDescending(x=> x.LogDate).ThenByDescending(x=>x.Employee.TripleName).ThenByDescending(x => x.LogTime).Skip<EntranceExitRecord>(skip).Take<EntranceExitRecord>(pageSize).AsQueryable();
                result.Total = allBeforeFilter.Count();
            }
            else
            {
                var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
                filteredDate = allBeforeFilter.Where(x => employeesCanViewThem.Any(y => y.Id == x.Employee.Id)).ToList();
                result.Data = filteredDate.OrderByDescending(x => x.LogDate).ThenByDescending(x => x.Employee.TripleName).ThenByDescending(x=> x.LogTime).Skip<EntranceExitRecord>(skip).Take<EntranceExitRecord>(pageSize).AsQueryable();
                result.Total = filteredDate.Count();
            }
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var entranceExitRecord = (EntranceExitRecord)entity;
            entranceExitRecord.InsertSource = InsertSource.Manual;
            //entranceExitRecord.Status = EntranceExitStatus.Ok;
            entranceExitRecord.ErrorType = ErrorType.None;

            DateTime logDateTime = new DateTime(
                entranceExitRecord.LogDate.Year,
                entranceExitRecord.LogDate.Month,
                entranceExitRecord.LogDate.Day,
                entranceExitRecord.LogTime.Hour,
                entranceExitRecord.LogTime.Minute,
                entranceExitRecord.LogTime.Second);

            entranceExitRecord.LogDateTime = logDateTime;

            entranceExitRecord.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.LogTime.Hour, entranceExitRecord.LogTime.Minute, entranceExitRecord.LogTime.Second);

            entranceExitRecord.LogDate = new DateTime(entranceExitRecord.LogDate.Year, entranceExitRecord.LogDate.Month, entranceExitRecord.LogDate.Day, 0, 0, 0);
            
            //entranceExitRecord.ErrorMessage = string.Empty;

            //var oldEntranceExitRecord = ServiceFactory.ORMService.All<EntranceExitRecord>()
            //    .Where(x => x.Employee == entranceExitRecord.Employee 
            //        && x.LogDateTime.Value.Month == entranceExitRecord.LogDateTime.Value.Month
            //        && x.LogType == entranceExitRecord.LogType);


        }
        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var entranceExitRecord = (EntranceExitRecord)entity;

            DateTime logDateTime = new DateTime(
                entranceExitRecord.LogDate.Year,
                entranceExitRecord.LogDate.Month,
                entranceExitRecord.LogDate.Day,
                entranceExitRecord.LogTime.Hour,
                entranceExitRecord.LogTime.Minute,
                entranceExitRecord.LogTime.Second);

            entranceExitRecord.LogDateTime = logDateTime;
            //entranceExitRecord.ErrorMessage = string.Empty;
            entranceExitRecord.ErrorType = ErrorType.None;
       
            entranceExitRecord.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.LogTime.Hour, entranceExitRecord.LogTime.Minute, entranceExitRecord.LogTime.Second);

            entranceExitRecord.LogDate = new DateTime(entranceExitRecord.LogDate.Year, entranceExitRecord.LogDate.Month, entranceExitRecord.LogDate.Day, 0, 0, 0);


        }

        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            CrudOperationType operationType, string customInformation = null)
        {
            var entranceExitRecord = (EntranceExitRecord)entity;
            DateTime logDateTime = new DateTime(
            entranceExitRecord.LogDate.Year,
            entranceExitRecord.LogDate.Month,
            entranceExitRecord.LogDate.Day,
            entranceExitRecord.LogTime.Hour,
            entranceExitRecord.LogTime.Minute,
            entranceExitRecord.LogTime.Second);

            entranceExitRecord.LogDateTime = logDateTime;

            entranceExitRecord.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.LogTime.Hour, entranceExitRecord.LogTime.Minute, entranceExitRecord.LogTime.Second);

            entranceExitRecord.LogDate = new DateTime(entranceExitRecord.LogDate.Year, entranceExitRecord.LogDate.Month, entranceExitRecord.LogDate.Day, 0, 0, 0);
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var entranceExitRecord = (EntranceExitRecord)entity;
            var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == entranceExitRecord.Employee.Id);

            //DateTime logDateTime = new DateTime(
            //entranceExitRecord.LogDate.Year,
            //entranceExitRecord.LogDate.Month,
            //entranceExitRecord.LogDate.Day,
            //entranceExitRecord.LogTime.Hour,
            //entranceExitRecord.LogTime.Minute,
            //entranceExitRecord.LogTime.Second);

            //entranceExitRecord.LogDateTime = logDateTime;

            DateTime? datetime = entranceExitRecord.LogDateTime;
            DateTime entranceExitRecordDatetime = new DateTime();
            if (datetime.HasValue)
                entranceExitRecordDatetime = datetime.Value;

            if (originalState != null)
            {
                if (entranceExitRecord.UpdateReason == null)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                        .GetFullKey(CustomMessageKeysAttendanceSystemModule.UpdateReasonIsRequired)),
                        Property = typeof(EntranceExitRecord).GetProperty("UpdateReason")
                    });
                }
            }

            if (operationType == CrudOperationType.Insert)
            {
                var allFingerprintTransferredDataOfEmployee = ServiceFactory.ORMService.All<FingerprintTransferredData>().Where(x => x.Employee.Id == entranceExitRecord.Employee.Id).ToList();
                var allEntranceExitRecordDataOfEmployee = ServiceFactory.ORMService.All<EntranceExitRecord>().Where(x => x.Employee.Id == entranceExitRecord.Employee.Id).ToList();
                if (AttendanceService.CheckEntranceExitRecordDuplicate(allEntranceExitRecordDataOfEmployee,
                    allFingerprintTransferredDataOfEmployee, entranceExitRecordDatetime, InsertSource.Manual, entranceExitRecord.LogType, 0))
                {

                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                        .GetFullKey(CustomMessageKeysAttendanceSystemModule.EntranceExitRecordAlreadyExist)),
                        Property = typeof(EntranceExitRecord).GetProperty("Employee")
                    });
                }
            }
            //var entranceExitRecord = (EntranceExitRecord)entity;

            //var employeeAttendanceCard =
            //    typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == entranceExitRecord.Employee.Id);

            //if (employeeAttendanceCard == null)
            //{
            //    validationResults.Add(new ValidationResult
            //    {
            //        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
            //        .GetFullKey(CustomMessageKeysAttendanceSystemModule.EmployeeDoesNotHasEmployeeAttendanceCard)),
            //        Property = typeof(EntranceExitRecord).GetProperty("Employee")
            //    });
            //}
            //else
            //{
            //    var workshopRecurrence = AttendanceService.GetWorkshopRecurrenceInPeriod(employeeAttendanceCard, entranceExitRecord.LogDateTime.GetValueOrDefault(), entranceExitRecord.LogDateTime.GetValueOrDefault());
            //    var recordDuplicationExistence =
            //        AttendanceService.CheckRecordDuplicationExistence(entranceExitRecord.LogType, workshopRecurrence[0].Workshop, entranceExitRecord.LogDateTime);
            //    if (recordDuplicationExistence.Any())
            //    {
            //        entranceExitRecord.ErrorType = entranceExitRecord.LogType == LogType.Entrance
            //            ? ErrorType.MultipleEntrance
            //            : ErrorType.MultipleExit;
            //        entranceExitRecord.Status = EntranceExitStatus.Error;
            //    }
            //}

        }
        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var entranceExitRecord = (EntranceExitRecord)entity;
            UpdateEntranceRecordsAfterCrudOperation(entranceExitRecord.Employee, entranceExitRecord.LogDate);
        }
        public override void AfterUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var entranceExitRecord = (EntranceExitRecord)entity;
            UpdateEntranceRecordsAfterCrudOperation(entranceExitRecord.Employee, entranceExitRecord.LogDate);
        }
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var entranceExitRecord = (EntranceExitRecord)entity;

            var fingerprintTransferredData = ServiceFactory.ORMService.All<FingerprintTransferredData>().Where(x => x.EntranceExitRecord == entranceExitRecord).FirstOrDefault();
            if (fingerprintTransferredData != null)
                fingerprintTransferredData.Delete();
            UpdateEntranceRecordsAfterCrudOperation(entranceExitRecord.Employee, entranceExitRecord.LogDateTime);
        }

        private void UpdateEntranceRecordsAfterCrudOperation(Employee employee, DateTime logDate)
        {
            var entities = new List<IAggregateRoot>();
            var allEntranceExitRecords = new List<EntranceExitRecord>();
            IList<PublicHoliday> publicHolidays;
            IList<FixedHoliday> fixedHolidays;
            IList<ChangeableHoliday> changeableHolidays;
            EmployeeRelationServices.Services.AttendanceSystemIntegrationService.GetHolidays(out publicHolidays, out fixedHolidays, out changeableHolidays);
            var entranceExitRecords = ServiceFactory.ORMService.All<EntranceExitRecord>();

            GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            AttendanceForm form = AttendanceService.GetAttendanceForm(employee, generalSetting.AttendanceForm);
            var recurrence = AttendanceService.GetWorkshopsRecurrenceInDate(employee.EmployeeCard, new DateTime(logDate.Date.Year, logDate.Date.Month, logDate.Date.Day),
                            form, publicHolidays, fixedHolidays, changeableHolidays);
            var ranges = recurrence.Workshop.Prepare(recurrence.Date).NormalShifts;
            if (!ranges.Any(y => logDate >= y.ShiftRangeStartTime && logDate <= y.ShiftRangeEndTime))
            {
                recurrence = AttendanceService.GetWorkshopsRecurrenceInDate(employee.EmployeeCard, new DateTime(logDate.Date.Year, logDate.Date.Month, logDate.Date.Day).AddDays(-1),
                            form, publicHolidays, fixedHolidays, changeableHolidays);
                ranges = recurrence.Workshop.Prepare(recurrence.Date).NormalShifts;
            }
            var firstShift = recurrence.Workshop.NormalShifts.OrderBy(x => x.NormalShiftOrder).FirstOrDefault();

            var lastShift = recurrence.Workshop.NormalShifts.OrderByDescending(x => x.NormalShiftOrder).FirstOrDefault();
            var employeeEntranceExitRecordsForthisDay = entranceExitRecords.ToList().Where(x => ranges.Any(y => x.LogDateTime >= y.ShiftRangeStartTime &&
                                                 x.LogDateTime <= y.ShiftRangeEndTime) && x.Employee.Id == employee.Id).ToList();
            allEntranceExitRecords.AddRange(employeeEntranceExitRecordsForthisDay);
            var outOfRangeRecords = entranceExitRecords.ToList().Where(x => ranges.Any(y => x.LogDateTime >= firstShift.ShiftRangeStartTime &&
                              x.LogDateTime <= lastShift.ShiftRangeEndTime) && x.Employee.Id == employee.Id && !employeeEntranceExitRecordsForthisDay.Any(z => z.Id == x.Id)).ToList();
            allEntranceExitRecords.AddRange(outOfRangeRecords);
            allEntranceExitRecords.ForEach(x =>
            {
                x.IsChecked = false;
                entities.Add(x);
            });

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
        }
    }
}