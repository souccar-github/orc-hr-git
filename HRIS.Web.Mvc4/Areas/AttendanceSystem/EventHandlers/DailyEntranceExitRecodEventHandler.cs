using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Areas.AttendanceSystem.Services;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class DailyEntranceExitRecordEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            Type typeOfClass = typeof(DailyEnternaceExitRecord);
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
            }


            if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));

            model.Views[0].EditHandler = "DailyEntranceExitRecordEditHandler";
            model.Views[0].AfterRequestEnd = "DailyEntranceExitRecordAfterRequestEnd";

            model.ViewModelTypeFullName = typeof(DailyEntranceExitRecordEventHandlers).FullName;
        }
        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }

        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            Type typeOfClass = typeof(DailyEnternaceExitRecord);
            var filteredDate = new List<DailyEnternaceExitRecord>();
            var allBeforeFilter = ((IQueryable<DailyEnternaceExitRecord>)result.Data).ToList();
            var user = UserExtensions.CurrentUser;
            if (UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result.Data = allBeforeFilter.OrderByDescending(x => x.Date).ThenByDescending(x => x.Employee.TripleName).Skip<DailyEnternaceExitRecord>(skip).Take<DailyEnternaceExitRecord>(pageSize).AsQueryable();
                result.Total = allBeforeFilter.Count();
            }
            else
            {
                var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
                filteredDate = allBeforeFilter.Where(x => employeesCanViewThem.Any(y => y.Id == x.Employee.Id)).ToList();
                result.Data = filteredDate.OrderByDescending(x => x.Date).ThenByDescending(x => x.Employee.TripleName).Skip<DailyEnternaceExitRecord>(skip).Take<DailyEnternaceExitRecord>(pageSize).AsQueryable();
                result.Total = filteredDate.Count();
            }
        }
         public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var dailyEntranceExitRecord = (DailyEnternaceExitRecord)entity;

            ComposeRecord(dailyEntranceExitRecord);
            UpdateEntranceExitRecord(dailyEntranceExitRecord);
        }

        private void UpdateEntranceExitRecord(DailyEnternaceExitRecord dailyEntranceExitRecord)
        {
            var entranceExitsRecords = ServiceFactory.ORMService.All<EntranceExitRecord>().Where(x => x.LogDate == dailyEntranceExitRecord.Date && x.Employee == dailyEntranceExitRecord.Employee);

            var allEntranceExitRecordIds = entranceExitsRecords.Select(x => x.Id.ToString()).ToList();
            var result = allEntranceExitRecordIds.Any() ? AttendanceService.DeleteFilteredEntranceExitWithRecordsWithFingerPrints(allEntranceExitRecordIds) : true;
            if (result)
            {
                var newEntranceExitRecords = ComposeEntranceExitRecords(dailyEntranceExitRecord);
                ServiceFactory.ORMService.SaveTransaction(newEntranceExitRecords, UserExtensions.CurrentUser);
            }
        }

        private List<EntranceExitRecord> ComposeEntranceExitRecords(DailyEnternaceExitRecord entranceExitRecord)
        {
            var records = new List<EntranceExitRecord>();
            entranceExitRecord.Date = new DateTime(entranceExitRecord.Date.Year, entranceExitRecord.Date.Month, entranceExitRecord.Date.Day, 0, 0, 0);
            if (entranceExitRecord.LoginTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.LoginTime.Value.Hour,
                    entranceExitRecord.LoginTime.Value.Minute,
                    entranceExitRecord.LoginTime.Value.Second);
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.LoginTime.Value.Hour, entranceExitRecord.LoginTime.Value.Minute, entranceExitRecord.LoginTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogType = LogType.Entrance;
                record.LogDate = entranceExitRecord.Date;
                records.Add(record);
            }
            if (entranceExitRecord.LogoutTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.LogoutTime.Value.Hour,
                    entranceExitRecord.LogoutTime.Value.Minute,
                    entranceExitRecord.LogoutTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogDate = entranceExitRecord.Date;
                record.LogType = LogType.Exit;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.LogoutTime.Value.Hour, entranceExitRecord.LogoutTime.Value.Minute, entranceExitRecord.LogoutTime.Value.Second);
                records.Add(record);
            }
            if (entranceExitRecord.SecondLoginTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.SecondLoginTime.Value.Hour,
                    entranceExitRecord.SecondLoginTime.Value.Minute,
                    entranceExitRecord.SecondLoginTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogType = LogType.Entrance;
                record.LogDate = entranceExitRecord.Date;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.SecondLoginTime.Value.Hour, entranceExitRecord.SecondLoginTime.Value.Minute, entranceExitRecord.SecondLoginTime.Value.Second);
                records.Add(record);
            }
            if (entranceExitRecord.SecondLogoutTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.SecondLogoutTime.Value.Hour,
                    entranceExitRecord.SecondLogoutTime.Value.Minute,
                    entranceExitRecord.SecondLogoutTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogType = LogType.Exit;
                record.LogDate = entranceExitRecord.Date;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.SecondLogoutTime.Value.Hour, entranceExitRecord.SecondLogoutTime.Value.Minute, entranceExitRecord.SecondLogoutTime.Value.Second);
                records.Add(record);
            }
            if (entranceExitRecord.ThirdLoginTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.ThirdLoginTime.Value.Hour,
                    entranceExitRecord.ThirdLoginTime.Value.Minute,
                    entranceExitRecord.ThirdLoginTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogType = LogType.Entrance;
                record.LogDate = entranceExitRecord.Date;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.ThirdLoginTime.Value.Hour, entranceExitRecord.ThirdLoginTime.Value.Minute, entranceExitRecord.ThirdLoginTime.Value.Second);
                records.Add(record);
            }
            if (entranceExitRecord.ThirdLogoutTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.ThirdLogoutTime.Value.Hour,
                    entranceExitRecord.ThirdLogoutTime.Value.Minute,
                    entranceExitRecord.ThirdLogoutTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogType = LogType.Exit;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.ThirdLogoutTime.Value.Hour, entranceExitRecord.ThirdLogoutTime.Value.Minute, entranceExitRecord.ThirdLogoutTime.Value.Second);
                record.LogDateTime = logDateTime;
                record.LogDate = entranceExitRecord.Date;
                records.Add(record);
            }
            return records;
        }
        private void ComposeRecord(DailyEnternaceExitRecord entranceExitRecord)
        {
            entranceExitRecord.Date = new DateTime(entranceExitRecord.Date.Year, entranceExitRecord.Date.Month, entranceExitRecord.Date.Day, 0, 0, 0);
            if (entranceExitRecord.LoginTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.LoginTime.Value.Hour,
                    entranceExitRecord.LoginTime.Value.Minute,
                    entranceExitRecord.LoginTime.Value.Second);

                entranceExitRecord.LoginDateTime = logDateTime;
                entranceExitRecord.LoginTime = new DateTime(2000, 1, 1, entranceExitRecord.LoginTime.Value.Hour, entranceExitRecord.LoginTime.Value.Minute, entranceExitRecord.LoginTime.Value.Second);
            }
            if (entranceExitRecord.LogoutTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.LogoutTime.Value.Hour,
                    entranceExitRecord.LogoutTime.Value.Minute,
                    entranceExitRecord.LogoutTime.Value.Second);

                entranceExitRecord.LogoutDateTime = logDateTime;
                entranceExitRecord.LogoutTime = new DateTime(2000, 1, 1, entranceExitRecord.LogoutTime.Value.Hour, entranceExitRecord.LogoutTime.Value.Minute, entranceExitRecord.LogoutTime.Value.Second);
            }
            if (entranceExitRecord.SecondLoginTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.SecondLoginTime.Value.Hour,
                    entranceExitRecord.SecondLoginTime.Value.Minute,
                    entranceExitRecord.SecondLoginTime.Value.Second);

                entranceExitRecord.SecondLoginDateTime = logDateTime;
                entranceExitRecord.SecondLoginTime = new DateTime(2000, 1, 1, entranceExitRecord.SecondLoginTime.Value.Hour, entranceExitRecord.SecondLoginTime.Value.Minute, entranceExitRecord.SecondLoginTime.Value.Second);
            }
            if (entranceExitRecord.SecondLogoutTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.SecondLogoutTime.Value.Hour,
                    entranceExitRecord.SecondLogoutTime.Value.Minute,
                    entranceExitRecord.SecondLogoutTime.Value.Second);

                entranceExitRecord.SecondLogoutDateTime = logDateTime;
                entranceExitRecord.SecondLogoutTime = new DateTime(2000, 1, 1, entranceExitRecord.SecondLogoutTime.Value.Hour, entranceExitRecord.SecondLogoutTime.Value.Minute, entranceExitRecord.SecondLogoutTime.Value.Second);
            }
            if (entranceExitRecord.ThirdLoginTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.ThirdLoginTime.Value.Hour,
                    entranceExitRecord.ThirdLoginTime.Value.Minute,
                    entranceExitRecord.ThirdLoginTime.Value.Second);

                entranceExitRecord.ThirdLoginDateTime = logDateTime;
                entranceExitRecord.ThirdLoginTime = new DateTime(2000, 1, 1, entranceExitRecord.ThirdLoginTime.Value.Hour, entranceExitRecord.ThirdLoginTime.Value.Minute, entranceExitRecord.ThirdLoginTime.Value.Second);
            }
            if (entranceExitRecord.ThirdLogoutTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.Date.Year,
                    entranceExitRecord.Date.Month,
                    entranceExitRecord.Date.Day,
                    entranceExitRecord.ThirdLogoutTime.Value.Hour,
                    entranceExitRecord.ThirdLogoutTime.Value.Minute,
                    entranceExitRecord.ThirdLogoutTime.Value.Second);

                entranceExitRecord.ThirdLogoutDateTime = logDateTime;
                entranceExitRecord.ThirdLogoutTime = new DateTime(2000, 1, 1, entranceExitRecord.ThirdLogoutTime.Value.Hour, entranceExitRecord.ThirdLogoutTime.Value.Minute, entranceExitRecord.ThirdLogoutTime.Value.Second);
            }
        }
        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            CrudOperationType operationType, string customInformation = null)
        {
            var entranceExitRecord = (DailyEnternaceExitRecord)entity;

            ComposeRecord(entranceExitRecord);
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var entranceExitRecord = (DailyEnternaceExitRecord)entity;



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


        }
        public override void AfterUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var entranceExitRecord = (DailyEnternaceExitRecord)entity; 
            var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().Where(x => x.AttendanceMonthStatus != AttendanceMonthStatus.Locked).OrderByDescending(x => x.FromDate).FirstOrDefault();
            if (attendanceRecord != null)
            {
                AttendanceService.GenerateAttendanceRecordDetailsUntillCurrentDay(attendanceRecord);
                var attendanceWithoutAdjustments = attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == entranceExitRecord.Employee);
                var attendanceDetail = attendanceWithoutAdjustments != null ?
                    attendanceWithoutAdjustments.AttendanceWithoutAdjustmentDetails.FirstOrDefault(x => x.Date == entranceExitRecord.Date) : null;
                if(attendanceDetail != null)
                {
                    attendanceDetail.IsCalculated = false;
                    attendanceDetail.Save();
                }
                attendanceRecord.Save();
                AttendanceService.CalculateAttendanceRecord(attendanceRecord);
                attendanceRecord.AttendanceMonthStatus = AttendanceMonthStatus.Calculated;
                attendanceRecord.Save();
            }
        }

    }
}