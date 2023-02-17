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
            UpdateEntranceExitRecord(dailyEntranceExitRecord, originalState);
        }

        private void UpdateEntranceExitRecord(DailyEnternaceExitRecord dailyEntranceExitRecord, IDictionary<string, object> oldOne)
        {

            List<EntranceExitRecord> records = new List<EntranceExitRecord>();
            var entranceExitsRecords = ServiceFactory.ORMService.All<EntranceExitRecord>().Where(x => x.Employee == dailyEntranceExitRecord.Employee);
            if (oldOne["LoginDateTime"] != null && !string.IsNullOrEmpty(oldOne["LoginDateTime"].ToString()))
            {
                var record = entranceExitsRecords.FirstOrDefault(x => x.LogDateTime == Convert.ToDateTime(oldOne["LoginDateTime"].ToString()));
                if (record != null) records.Add(record);
            }
            if (oldOne["LogoutDateTime"] != null && !string.IsNullOrEmpty(oldOne["LogoutDateTime"].ToString()))
            {
                var record = entranceExitsRecords.FirstOrDefault(x => x.LogDateTime == Convert.ToDateTime(oldOne["LogoutDateTime"].ToString()));
                if (record != null) records.Add(record);
            }
            if (oldOne["SecondLoginDateTime"] != null && !string.IsNullOrEmpty(oldOne["SecondLoginDateTime"].ToString()))
            {
                var record = entranceExitsRecords.FirstOrDefault(x => x.LogDateTime == Convert.ToDateTime(oldOne["SecondLoginDateTime"].ToString()));
                if (record != null) records.Add(record);
            }
            if (oldOne["SecondLogoutDateTime"] != null && !string.IsNullOrEmpty(oldOne["SecondLogoutDateTime"].ToString()))
            {
                var record = entranceExitsRecords.FirstOrDefault(x => x.LogDateTime == Convert.ToDateTime(oldOne["SecondLogoutDateTime"].ToString()));
                if (record != null) records.Add(record);
            }
            if (oldOne["ThirdLoginDateTime"] != null && !string.IsNullOrEmpty(oldOne["ThirdLoginDateTime"].ToString()))
            {
                var record = entranceExitsRecords.FirstOrDefault(x => x.LogDateTime == Convert.ToDateTime(oldOne["ThirdLoginDateTime"].ToString()));
                if (record != null) records.Add(record);
            }
            if (oldOne["ThirdLogoutDateTime"] != null && !string.IsNullOrEmpty(oldOne["ThirdLogoutDateTime"].ToString()))
            {
                var record = entranceExitsRecords.FirstOrDefault(x => x.LogDateTime == Convert.ToDateTime(oldOne["ThirdLogoutDateTime"].ToString()));
                if (record != null) records.Add(record);
            }
            var allEntranceExitRecordIds = records.Select(x => x.Id.ToString()).ToList();
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
                    entranceExitRecord.LoginDateTime.Value.Year,
                    entranceExitRecord.LoginDateTime.Value.Month,
                    entranceExitRecord.LoginDateTime.Value.Day,
                    entranceExitRecord.LoginDateTime.Value.Hour,
                    entranceExitRecord.LoginDateTime.Value.Minute,
                    entranceExitRecord.LoginDateTime.Value.Second);
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.LoginTime.Value.Hour, entranceExitRecord.LoginTime.Value.Minute, entranceExitRecord.LoginTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogType = LogType.Entrance;
                record.LogDate = entranceExitRecord.LoginDate.Value;
                records.Add(record);
            }
            if (entranceExitRecord.LogoutTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.LogoutDateTime.Value.Year,
                    entranceExitRecord.LogoutDateTime.Value.Month,
                    entranceExitRecord.LogoutDateTime.Value.Day,
                    entranceExitRecord.LogoutDateTime.Value.Hour,
                    entranceExitRecord.LogoutDateTime.Value.Minute,
                    entranceExitRecord.LogoutDateTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogDate = entranceExitRecord.LogoutDate.Value;
                record.LogType = LogType.Exit;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.LogoutTime.Value.Hour, entranceExitRecord.LogoutTime.Value.Minute, entranceExitRecord.LogoutTime.Value.Second);
                records.Add(record);
            }
            if (entranceExitRecord.SecondLoginTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.SecondLoginDateTime.Value.Year,
                    entranceExitRecord.SecondLoginDateTime.Value.Month,
                    entranceExitRecord.SecondLoginDateTime.Value.Day,
                    entranceExitRecord.SecondLoginDateTime.Value.Hour,
                    entranceExitRecord.SecondLoginDateTime.Value.Minute,
                    entranceExitRecord.SecondLoginDateTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogType = LogType.Entrance;
                record.LogDate = entranceExitRecord.SecondLoginDate.Value;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.SecondLoginTime.Value.Hour, entranceExitRecord.SecondLoginTime.Value.Minute, entranceExitRecord.SecondLoginTime.Value.Second);
                records.Add(record);
            }
            if (entranceExitRecord.SecondLogoutTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.SecondLogoutDateTime.Value.Year,
                    entranceExitRecord.SecondLogoutDateTime.Value.Month,
                    entranceExitRecord.SecondLogoutDateTime.Value.Day,
                    entranceExitRecord.SecondLogoutDateTime.Value.Hour,
                    entranceExitRecord.SecondLogoutDateTime.Value.Minute,
                    entranceExitRecord.SecondLogoutDateTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogType = LogType.Exit;
                record.LogDate = entranceExitRecord.SecondLogoutDate.Value;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.SecondLogoutTime.Value.Hour, entranceExitRecord.SecondLogoutTime.Value.Minute, entranceExitRecord.SecondLogoutTime.Value.Second);
                records.Add(record);
            }
            if (entranceExitRecord.ThirdLoginTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.ThirdLoginDateTime.Value.Year,
                    entranceExitRecord.ThirdLoginDateTime.Value.Month,
                    entranceExitRecord.ThirdLoginDateTime.Value.Day,
                    entranceExitRecord.ThirdLoginDateTime.Value.Hour,
                    entranceExitRecord.ThirdLoginDateTime.Value.Minute,
                    entranceExitRecord.ThirdLoginDateTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogDateTime = logDateTime;
                record.LogType = LogType.Entrance;
                record.LogDate = entranceExitRecord.ThirdLoginDate.Value;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.ThirdLoginTime.Value.Hour, entranceExitRecord.ThirdLoginTime.Value.Minute, entranceExitRecord.ThirdLoginTime.Value.Second);
                records.Add(record);
            }
            if (entranceExitRecord.ThirdLogoutTime != null)
            {
                EntranceExitRecord record = new EntranceExitRecord();
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.ThirdLogoutDateTime.Value.Year,
                    entranceExitRecord.ThirdLogoutDateTime.Value.Month,
                    entranceExitRecord.ThirdLogoutDateTime.Value.Day,
                    entranceExitRecord.ThirdLogoutDateTime.Value.Hour,
                    entranceExitRecord.ThirdLogoutDateTime.Value.Minute,
                    entranceExitRecord.ThirdLogoutDateTime.Value.Second);
                record.Employee = entranceExitRecord.Employee;
                record.LogType = LogType.Exit;
                record.LogTime = new DateTime(2000, 1, 1, entranceExitRecord.ThirdLogoutTime.Value.Hour, entranceExitRecord.ThirdLogoutTime.Value.Minute, entranceExitRecord.ThirdLogoutTime.Value.Second);
                record.LogDateTime = logDateTime;
                record.LogDate = entranceExitRecord.ThirdLogoutDate.Value;
                records.Add(record);
            }
            return records;
        }
        private void ComposeRecord(DailyEnternaceExitRecord entranceExitRecord)
        {
            entranceExitRecord.Date = new DateTime(entranceExitRecord.Date.Year, entranceExitRecord.Date.Month, entranceExitRecord.Date.Day, 0, 0, 0);
            entranceExitRecord.IsCalculated = false;
            if (entranceExitRecord.LoginTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.LoginDate.Value.Year,
                    entranceExitRecord.LoginDate.Value.Month,
                    entranceExitRecord.LoginDate.Value.Day,
                    entranceExitRecord.LoginTime.Value.Hour,
                    entranceExitRecord.LoginTime.Value.Minute,
                    entranceExitRecord.LoginTime.Value.Second);

                entranceExitRecord.LoginDateTime = logDateTime;
                entranceExitRecord.LoginTime = new DateTime(2000, 1, 1,
                    logDateTime.Hour, logDateTime.Minute, logDateTime.Second);
                entranceExitRecord.LoginDate = new DateTime(
                    logDateTime.Year,
                    logDateTime.Month,
                    logDateTime.Day,
                    0, 0, 0);
            }
            if (entranceExitRecord.LogoutTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.LogoutDate.Value.Year,
                    entranceExitRecord.LogoutDate.Value.Month,
                    entranceExitRecord.LogoutDate.Value.Day,
                    entranceExitRecord.LogoutTime.Value.Hour,
                    entranceExitRecord.LogoutTime.Value.Minute,
                    entranceExitRecord.LogoutTime.Value.Second);

                entranceExitRecord.LogoutDateTime = logDateTime;
                entranceExitRecord.LogoutTime = new DateTime(2000, 1, 1,
                    logDateTime.Hour, logDateTime.Minute, logDateTime.Second);
                entranceExitRecord.LogoutDate = new DateTime(
                    logDateTime.Year,
                    logDateTime.Month,
                    logDateTime.Day,
                    0, 0, 0);
            }
            if (entranceExitRecord.SecondLoginTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.SecondLoginDate.Value.Year,
                    entranceExitRecord.SecondLoginDate.Value.Month,
                    entranceExitRecord.SecondLoginDate.Value.Day,
                    entranceExitRecord.SecondLoginTime.Value.Hour,
                    entranceExitRecord.SecondLoginTime.Value.Minute,
                    entranceExitRecord.SecondLoginTime.Value.Second);

                entranceExitRecord.SecondLoginDateTime = logDateTime;
                entranceExitRecord.SecondLoginTime = new DateTime(2000, 1, 1,
                    logDateTime.Hour, logDateTime.Minute, logDateTime.Second);
                entranceExitRecord.SecondLoginDate = new DateTime(
                    logDateTime.Year,
                    logDateTime.Month,
                    logDateTime.Day,
                    0, 0, 0);
            }
            if (entranceExitRecord.SecondLogoutTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.SecondLogoutDate.Value.Year,
                    entranceExitRecord.SecondLogoutDate.Value.Month,
                    entranceExitRecord.SecondLogoutDate.Value.Day,
                    entranceExitRecord.SecondLogoutTime.Value.Hour,
                    entranceExitRecord.SecondLogoutTime.Value.Minute,
                    entranceExitRecord.SecondLogoutTime.Value.Second);

                entranceExitRecord.SecondLogoutDateTime = logDateTime;
                entranceExitRecord.SecondLogoutTime = new DateTime(2000, 1, 1,
                    logDateTime.Hour, logDateTime.Minute, logDateTime.Second);
                entranceExitRecord.SecondLogoutDate = new DateTime(
                    logDateTime.Year,
                    logDateTime.Month,
                    logDateTime.Day,
                    0, 0, 0);
            }
            if (entranceExitRecord.ThirdLoginTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.ThirdLoginDate.Value.Year,
                    entranceExitRecord.ThirdLoginDate.Value.Month,
                    entranceExitRecord.ThirdLoginDate.Value.Day,
                    entranceExitRecord.ThirdLoginTime.Value.Hour,
                    entranceExitRecord.ThirdLoginTime.Value.Minute,
                    entranceExitRecord.ThirdLoginTime.Value.Second);

                entranceExitRecord.ThirdLoginDateTime = logDateTime;
                entranceExitRecord.ThirdLoginTime = new DateTime(2000, 1, 1,
                    logDateTime.Hour, logDateTime.Minute, logDateTime.Second);
                entranceExitRecord.ThirdLoginDate = new DateTime(
                    logDateTime.Year,
                    logDateTime.Month,
                    logDateTime.Day,
                    0, 0, 0);
            }
            if (entranceExitRecord.ThirdLogoutTime != null)
            {
                DateTime logDateTime = new DateTime(
                    entranceExitRecord.ThirdLogoutDate.Value.Year,
                    entranceExitRecord.ThirdLogoutDate.Value.Month,
                    entranceExitRecord.ThirdLogoutDate.Value.Day,
                    entranceExitRecord.ThirdLogoutTime.Value.Hour,
                    entranceExitRecord.ThirdLogoutTime.Value.Minute,
                    entranceExitRecord.ThirdLogoutTime.Value.Second);

                entranceExitRecord.ThirdLogoutDateTime = logDateTime;
                entranceExitRecord.ThirdLogoutTime = new DateTime(2000, 1, 1,
                    logDateTime.Hour, logDateTime.Minute, logDateTime.Second);

                entranceExitRecord.ThirdLogoutDate = new DateTime(
                    logDateTime.Year,
                    logDateTime.Month,
                    logDateTime.Day,
                    0, 0, 0);
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

        public override void BeforeDelete(Entity entity)
        {
            PreventDefault = true;
        }
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var dailyEnternaceExitRecord = (DailyEnternaceExitRecord)entity;
            var dailyEnternaceExitRecordIds = new List<string>() { dailyEnternaceExitRecord.Id.ToString() };
            var datesOfDailyEnternaceExitRecord = new List<DateTime>();
            datesOfDailyEnternaceExitRecord = AttendanceService.GetDatesOfDailyEntranceExitRecords(dailyEnternaceExitRecord, datesOfDailyEnternaceExitRecord);
            var allEntranceExitRecords = ServiceFactory.ORMService.All<EntranceExitRecord>().ToList();
            var allEntranceExitRecordIds = allEntranceExitRecords.Where(x => datesOfDailyEnternaceExitRecord.Any(y=> y == x.LogDateTime) && dailyEnternaceExitRecord.Employee == x.Employee)
                     .Select(x => x.Id.ToString()).ToList();
            AttendanceService.DeleteFilteredEntranceExitWithRecordsWithFingerPrints(allEntranceExitRecordIds, dailyEnternaceExitRecordIds);
        }

    }
}