using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.DTO;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Status = HRIS.Domain.Global.Enums.Status;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Services
{

    public static class PayrollIntegrationService
    {

        #region Employee Relation Services

        public static List<PayrollSystemIntegrationDTO> GetLeaves(Employee employee,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption,
            List<PublicHoliday> publicHolidays,
            List<FixedHoliday> fixedHolidays,
            List<ChangeableHoliday> changeableHolidays,
            HRIS.Domain.AttendanceSystem.Configurations.AttendanceForm form,
            bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // الاجازات

            var result = new List<PayrollSystemIntegrationDTO>();
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);

            if (employeeCard != null)
            {
                List<LeaveRequest> leaveRequests;

                if (!fromDate.HasValue && !toDate.HasValue)
                    leaveRequests = employeeCard.LeaveRequests.Where(x => x.LeaveStatus == Status.Approved && x.IsTransferToPayroll == isAccepted).ToList();
                else if (fromDate.HasValue && toDate.HasValue)
                    leaveRequests = employeeCard.LeaveRequests.
                        Where(x => x.LeaveStatus == Status.Approved && x.IsTransferToPayroll == isAccepted &&
                        ((x.StartDate <= fromDate && x.EndDate >= toDate) ||
                         (x.StartDate <= toDate && x.EndDate >= toDate) ||
                         (x.StartDate <= fromDate && x.EndDate >= fromDate) ||
                         (x.StartDate >= fromDate && x.EndDate <= toDate)))
                        .ToList();
                else if (fromDate.HasValue)
                    leaveRequests = employeeCard.LeaveRequests.
                        Where(x => x.StartDate == fromDate && x.LeaveStatus == Status.Approved && x.IsTransferToPayroll == isAccepted).ToList();
                else
                    leaveRequests = employeeCard.LeaveRequests.
                        Where(x => x.EndDate == toDate && x.LeaveStatus == Status.Approved && x.IsTransferToPayroll == isAccepted).ToList();

                if (leaveRequests.Count > 0)
                {
                    var leaveRequestsGroupedByLeaveType = leaveRequests.GroupBy(x => x.LeaveSetting);
                    foreach (var item in leaveRequestsGroupedByLeaveType)
                    {
                        var ids = new List<int>();
                        var order = 0;
                        var spentDaysValueForDeduction = 0.00;
                        var leaveSetting = item.Key;
                        var leavesRequested = item.ToList();
                        double totalLeaveSpentDays = 0;
                        double extraValues = 0;
                        foreach (var leaveRequest in leavesRequested)
                        {
                            ids.Add(leaveRequest.Id);
                            double leaveSpentDays = fromDate.HasValue && toDate.HasValue &&
                                (leaveRequest.StartDate < fromDate.Value || leaveRequest.EndDate > toDate.Value) ?
                                GetSpentDaysBetweenTwoDates(leaveRequest, form, fromDate.Value, toDate.Value, publicHolidays, fixedHolidays, changeableHolidays)
                                : leaveRequest.SpentDays;
                            totalLeaveSpentDays += leaveSpentDays;
                        }
                        if (leaveSetting.PaidSlices != null && leaveSetting.PaidSlices.Count > 0)
                        {
                            foreach (var pidSlice in leaveSetting.PaidSlices.OrderBy(x => x.FromBalance))
                            {
                                order += 1;
                                extraValues = pidSlice.PaidPercentage;
                                if (totalLeaveSpentDays == 0)
                                    break;
                                if (pidSlice.ToBalance > totalLeaveSpentDays)
                                {
                                    spentDaysValueForDeduction = totalLeaveSpentDays;
                                    totalLeaveSpentDays = 0;
                                }
                                else
                                {
                                    spentDaysValueForDeduction = pidSlice.ToBalance;
                                    totalLeaveSpentDays -= pidSlice.ToBalance;
                                }
                                result.Add(new PayrollSystemIntegrationDTO
                                {
                                    Value = spentDaysValueForDeduction,
                                    Formula = leaveSetting.DeductionCard != null && leaveSetting.DeductionCard.Id != 0 ? leaveSetting.DeductionCard.Formula :
                                              generalOption.LeaveDeduction != null ? generalOption.LeaveDeduction.Formula : Formula.DaysOfPackageSalary,
                                    ExtraValue = -extraValues,
                                    ExtraValueFormula = ExtraValueFormula.PercentageOfInitialValue,
                                    Repetition = 1,
                                    SourceId = ids,
                                    SourceType = 0,
                                    Note = leaveSetting.NameForDropdown + " " + EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AccordingToLeavePaidSliceWhichOrderIs) + order,
                                    DeductionCard = leaveSetting.DeductionCard != null && leaveSetting.DeductionCard.Id != 0 ? leaveSetting.DeductionCard :
                                              generalOption.LeaveDeduction != null ? generalOption.LeaveDeduction : null
                                });

                            }
                        }
                        else
                        {
                            extraValues = leaveSetting.PaidPercentage;
                            result.Add(new PayrollSystemIntegrationDTO
                            {
                                Value = totalLeaveSpentDays,
                                Formula = leaveSetting.DeductionCard != null && leaveSetting.DeductionCard.Id != 0 ? leaveSetting.DeductionCard.Formula :
                                              generalOption.LeaveDeduction != null ? generalOption.LeaveDeduction.Formula : Formula.DaysOfPackageSalary,
                                ExtraValue = -extraValues,
                                ExtraValueFormula = ExtraValueFormula.PercentageOfInitialValue,
                                Repetition = 1,
                                SourceId = ids,
                                SourceType = 0,
                                Note = leaveSetting.NameForDropdown,
                                DeductionCard = leaveSetting.DeductionCard != null && leaveSetting.DeductionCard.Id != 0 ? leaveSetting.DeductionCard :
                                              generalOption.LeaveDeduction != null ? generalOption.LeaveDeduction : null
                            });
                        }
                        if (leaveSetting.PaidPercentage == 0 && generalOption.MinimunOfNonPaidLeaveDaysToRemoveWeeklyHolidays > 0 &&
                            form.RelyHolidaies)
                        {
                            ids = new List<int>();
                            var holidayDaysCountToAddAsLeaveDeduction = 0;
                            foreach (var leaveRequest in leavesRequested)
                            {
                                var lastDayOfLeave = leaveRequest.EndDate;
                                var lastWeekDays = toDate > lastDayOfLeave ?
                                    GetWeekDaysBetweenTwoDates(lastDayOfLeave, toDate.Value, publicHolidays) : new List<DateTime>();
                                if (lastWeekDays.Count > 0)
                                    foreach (var weekDay in lastWeekDays)
                                    {
                                        if ((weekDay.Date - leaveRequest.EndDate.Date).TotalDays < 7 &&
                                            leaveRequest.SpentDays >= generalOption.MinimunOfNonPaidLeaveDaysToRemoveWeeklyHolidays)
                                        {
                                            holidayDaysCountToAddAsLeaveDeduction += 1;
                                            ids.Add(leaveRequest.Id);
                                        }
                                    }
                            }
                            if (holidayDaysCountToAddAsLeaveDeduction > 0)
                            {
                                result.Add(new PayrollSystemIntegrationDTO
                                {
                                    Value = holidayDaysCountToAddAsLeaveDeduction,
                                    Formula = leaveSetting.DeductionCard != null && leaveSetting.DeductionCard.Id != 0 ? leaveSetting.DeductionCard.Formula :
                                              generalOption.LeaveDeduction != null ? generalOption.LeaveDeduction.Formula : Formula.DaysOfPackageSalary,
                                    ExtraValue = 0,
                                    ExtraValueFormula = ExtraValueFormula.PercentageOfInitialValue,
                                    Repetition = 1,
                                    SourceId = ids,
                                    SourceType = 0,
                                    Note = EmployeeRelationServicesLocalizationHelper.GetResource(
                                    EmployeeRelationServicesLocalizationHelper.TotalWeeklyDaysValueHaveDeletedAccordingToEmployeeNonPaidLeaveDays)
                                });
                            }
                        }

                    }

                }
            }

            return result;
        }
        private static List<DateTime> GetWeekDaysBetweenTwoDates(DateTime lastDayOfLeave, DateTime toDate, List<PublicHoliday> publicHolidays)
        {
            try
            {
                IList<DateTime> dates = new List<DateTime>();
                //maybe the last day of leave is weekly day so we want to remove it
                var currentDay = lastDayOfLeave.Date.AddDays(1);
                var diff = (toDate.Date - lastDayOfLeave.Date).TotalDays;
                while (currentDay < toDate.Date)
                {
                    currentDay = currentDay.AddDays(1);
                    if (publicHolidays.Any(x => x.DayOfWeek == currentDay.DayOfWeek))
                        dates.Add(currentDay);
                }
                return dates.ToList();
            }
            catch (Exception ex)
            {
                return new List<DateTime>();
            }
        }

        private static double GetSpentDaysBetweenTwoDates(LeaveRequest leaveRequest, AttendanceForm attendanceForm, DateTime fromDate, DateTime toDate,
            List<PublicHoliday> publicHolidays, List<FixedHoliday> fixedHolidays, List<ChangeableHoliday> changableHolidays)
        {
            var spentDays = LeaveService.GetSpentDays(attendanceForm, leaveRequest.StartDate >= fromDate ? leaveRequest.StartDate : fromDate,
                leaveRequest.EndDate <= toDate ? leaveRequest.EndDate : toDate, leaveRequest.LeaveSetting.IsContinuous, publicHolidays, fixedHolidays, changableHolidays);
            return spentDays;
        }

        public static List<PayrollSystemIntegrationDTO> GetRecycledLeaves(Employee employee, bool isAccepted)
        {
            // الاجازات السنوية المدورة مالياً

            var recycledLeaves = new List<RecycledLeave>();
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null)
            {
                recycledLeaves = employeeCard.RecycledLeaves.Where(
                    x => x.RecycleType == RecycleType.Salary && x.IsTransferToPayroll == isAccepted).ToList();
            }

            var result = new List<PayrollSystemIntegrationDTO>();

            result.AddRange(recycledLeaves
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.RoundedBalance,
                    Formula = Formula.DaysOfSalary,
                    Repetition = 1,
                    SourceId = new List<int>() { x.Id }
                }).ToList());

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetPenalties(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // العقوبات

            List<EmployeeDisciplinary> penalties;

            if (!fromDate.HasValue && !toDate.HasValue)
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.Where(x => x.DisciplinaryStatus == Status.Approved && x.DisciplinarySetting.IsDeductFromSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.
                    Where(x => x.DisciplinaryDate >= fromDate && x.DisciplinaryDate <= toDate && x.DisciplinaryStatus == Status.Approved && x.DisciplinarySetting.IsDeductFromSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.
                    Where(x => x.DisciplinaryDate >= fromDate && x.DisciplinaryStatus == Status.Approved && x.DisciplinarySetting.IsDeductFromSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.
                    Where(x => x.DisciplinaryDate <= toDate && x.DisciplinaryStatus == Status.Approved && x.DisciplinarySetting.IsDeductFromSalary && x.IsTransferToPayroll == isAccepted).ToList();

            var result = penalties
                .Select(x => new PayrollSystemIntegrationDTO
                {
                    Value = x.DisciplinarySetting.Value,
                    Formula = x.DisciplinarySetting.DeductionType == DeductionType.PercentageOfPackageSalary ? Formula.PercentageOfPackageSalary :
                    x.DisciplinarySetting.DeductionType == DeductionType.DaysOfPackageSalary ? Formula.DaysOfPackageSalary :
                    Formula.FixedValue,
                    Repetition = 1,
                    SourceId = new List<int>() { x.Id }
                }).ToList();

            return result;
        }
        public static List<PayrollSystemIntegrationDTO> GetRewards(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // المكافآت

            List<EmployeeReward> rewards;

            if (!fromDate.HasValue && !toDate.HasValue)
                rewards = employee.EmployeeCard.EmployeeRewards.Where(x => x.RewardStatus == Status.Approved && x.RewardSetting.IsAddedToSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                rewards = employee.EmployeeCard.EmployeeRewards.
                    Where(x => x.RewardDate >= fromDate && x.RewardDate <= toDate && x.RewardStatus == Status.Approved && x.RewardSetting.IsAddedToSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                rewards = employee.EmployeeCard.EmployeeRewards.
                    Where(x => x.RewardDate >= fromDate && x.RewardStatus == Status.Approved && x.RewardSetting.IsAddedToSalary && x.IsTransferToPayroll == isAccepted).ToList();
            else
                rewards = employee.EmployeeCard.EmployeeRewards.
                    Where(x => x.RewardDate <= toDate && x.RewardStatus == Status.Approved && x.RewardSetting.IsAddedToSalary && x.IsTransferToPayroll == isAccepted).ToList();

            var result = rewards
               .Select(x => new PayrollSystemIntegrationDTO
               {
                   Value = x.RewardSetting.IsPercentage ? x.RewardSetting.Percentage : x.RewardSetting.FixedValue,
                   Formula = x.RewardSetting.IsPercentage ? Formula.PercentageOfSalary : Formula.FixedValue,
                   Repetition = 1,
                   SourceId = new List<int>() { x.Id }
               }).ToList();

            return result;
        }

        public static IList<LeaveRequest> AcceptLeave(Employee employee, MonthlyEmployeeDeduction monthlyEmplyeeDeduction)
        {
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null)
            {
                var leaves = employeeCard.LeaveRequests.Where(x => monthlyEmplyeeDeduction.LeaveDeductions.Any(y => y.LeaveId == x.Id));
                if (!leaves.Any())
                    return new List<LeaveRequest>();
                foreach (var leave in leaves)
                {
                    leave.IsTransferToPayroll = monthlyEmplyeeDeduction.MonthlyCard.Month.ToDate < leave.EndDate ? false : true;
                }
                return leaves.ToList();
            }
            return new List<LeaveRequest>();
        }
        public static RecycledLeave AcceptRecycledLeave(Employee employee, int sourceId)
        {
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null)
            {
                var recycledLeave = employeeCard.RecycledLeaves.FirstOrDefault(x => x.Id == sourceId);
                if (recycledLeave == null)
                    return null;
                recycledLeave.IsTransferToPayroll = true;
                return recycledLeave;
            }
            return null;
        }
        public static EmployeeDisciplinary AcceptPenalty(Employee employee, int sourceId)
        {
            var penalty = employee.EmployeeCard.EmployeeDisciplinarys.FirstOrDefault(x => x.Id == sourceId);
            if (penalty == null)
                return null;
            penalty.IsTransferToPayroll = true;
            return penalty;
        }
        public static List<IAggregateRoot> AcceptAdvances(Employee employee)
        {
            var advances = employee.EmployeeCard.EmployeeAdvances.Where(x => x.SourceId > 0).ToList();
            var entities = new List<IAggregateRoot>();
            if (advances.Count() == 0)
                return entities;
            foreach (var advance in advances)
            {
                advance.IsTransferToPayroll = true;
                entities.Add(advance);
                return entities;
            }
            return entities;
        }
        public static EmployeeReward AcceptReward(Employee employee, int sourceId)
        {
            var reward = employee.EmployeeCard.EmployeeRewards.FirstOrDefault(x => x.Id == sourceId);
            if (reward == null)
                return null;
            reward.IsTransferToPayroll = true;
            return reward;
        }

        #endregion

        #region Attendance

        public enum AttendanceType
        {
            Overtime = 0,
            Absence = 1,
            NonAttendance = 2,
            Lateness = 3
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attendanceType"></param>
        /// <param name="employee"></param>
        /// <param name="date"></param>
        /// <param name="isAccepted"></param>
        /// <returns></returns>
        public static List<PayrollSystemIntegrationDTO> ImportFromAttendance(AttendanceType attendanceType,
            Employee employee, DateTime fromDate, DateTime toDate, bool isAccepted,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption,
            List<PublicHoliday> publicHolidays,
            HRIS.Domain.AttendanceSystem.Configurations.AttendanceForm form)
        {
            // الدوام الإضافي

            var extraDays = 0;
            var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().FirstOrDefault(x => x.FromDate == fromDate && x.ToDate == toDate && x.AttendanceMonthStatus == AttendanceMonthStatus.Locked);
            var payrollSystemIntegrationDtOs = new List<PayrollSystemIntegrationDTO>();

            if (attendanceRecord != null)
            {
                var attendanceWithoutAdjustment = GetAttendanceWithoutAdjustment(attendanceRecord, employee, attendanceType, isAccepted);
                var attendanceMonthlyAdjustment = GetAttendanceMonthlyAdjustment(attendanceRecord, employee, attendanceType, isAccepted);
                var attendanceDailyAdjustment = GetAttendanceDailyAdjustment(attendanceRecord, employee, attendanceType, isAccepted);

                if (attendanceWithoutAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceWithoutAdjustment(attendanceWithoutAdjustment, attendanceType, toDate, generalOption, publicHolidays, form, out extraDays);
                    if (payrollSystemIntegrationDTO != null && payrollSystemIntegrationDTO.Value > 0)
                    {
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                        //extra days meaning count of weekly holiday days must to remove from employee when he has minimum absense days in same week
                        if (extraDays > 0)
                        {
                            payrollSystemIntegrationDtOs.Add(new PayrollSystemIntegrationDTO
                            {
                                Value = extraDays,
                                Formula = payrollSystemIntegrationDTO.Formula,
                                ExtraValue = payrollSystemIntegrationDTO.ExtraValue,
                                ExtraValueFormula = payrollSystemIntegrationDTO.ExtraValueFormula,
                                Repetition = 1,
                                SourceId = new List<int>() { attendanceWithoutAdjustment.Id },
                                Note = EmployeeRelationServicesLocalizationHelper.GetResource(
                                EmployeeRelationServicesLocalizationHelper.TotalWeeklyDaysValueHaveDeletedAccordingToEmployeeAbsenceDays)
                            });
                        }
                    }
                }

                if (attendanceMonthlyAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceMonthlyAdjustment(attendanceMonthlyAdjustment, attendanceType, generalOption, publicHolidays, form);
                    if (payrollSystemIntegrationDTO != null && payrollSystemIntegrationDTO.Value > 0)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                }

                if (attendanceDailyAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceDailyAdjustment(attendanceDailyAdjustment, attendanceType, toDate, generalOption, publicHolidays, form, out extraDays);
                    if (payrollSystemIntegrationDTO != null && payrollSystemIntegrationDTO.Value > 0)
                    {
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                        //extra days meaning count of weekly holiday days must to remove from employee when he has minimum absense days in same week
                        if (extraDays > 0)
                        {
                            payrollSystemIntegrationDtOs.Add(new PayrollSystemIntegrationDTO
                            {
                                Value = extraDays,
                                Formula = payrollSystemIntegrationDTO.Formula,
                                ExtraValue = payrollSystemIntegrationDTO.ExtraValue,
                                ExtraValueFormula = payrollSystemIntegrationDTO.ExtraValueFormula,
                                Repetition = 1,
                                SourceId = new List<int>() { attendanceWithoutAdjustment.Id },
                                Note = EmployeeRelationServicesLocalizationHelper.GetResource(
                                EmployeeRelationServicesLocalizationHelper.TotalWeeklyDaysValueHaveDeletedAccordingToEmployeeAbsenceDays)
                            });
                        }
                    }
                }
            }

            return payrollSystemIntegrationDtOs;
        }
        private static AttendanceWithoutAdjustment GetAttendanceWithoutAdjustment(AttendanceRecord attendanceRecord, Employee employee, AttendanceType attendanceType, bool isAccepted)
        {
            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsOvertimeTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Absence:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsAbsenceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.NonAttendance:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsNonAttendanceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Lateness:
                    {
                        return attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsLatenessTransferToPayroll == isAccepted);
                    }
            }

            return null;
        }
        private static AttendanceMonthlyAdjustment GetAttendanceMonthlyAdjustment(AttendanceRecord attendanceRecord, Employee employee, AttendanceType attendanceType, bool isAccepted)
        {
            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsOvertimeTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Absence:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsAbsenceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.NonAttendance:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsNonAttendanceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Lateness:
                    {
                        return attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsLatenessTransferToPayroll == isAccepted);
                    }
            }

            return null;
        }
        private static AttendanceDailyAdjustment GetAttendanceDailyAdjustment(AttendanceRecord attendanceRecord, Employee employee, AttendanceType attendanceType, bool isAccepted)
        {
            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsOvertimeTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Absence:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsAbsenceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.NonAttendance:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsNonAttendanceTransferToPayroll == isAccepted);
                    }
                case AttendanceType.Lateness:
                    {
                        return attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.EmployeeAttendanceCard.Employee == employee
                            && x.IsLatenessTransferToPayroll == isAccepted);
                    }
            }

            return null;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceWithoutAdjustment(
            AttendanceWithoutAdjustment attendanceWithoutAdjustment,
            AttendanceType attendanceType, DateTime toDate,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption,
            List<PublicHoliday> publicHolidays,
            HRIS.Domain.AttendanceSystem.Configurations.AttendanceForm form, out int extraDays)
        {
            extraDays = 0;
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                Repetition = 1,
                SourceId = new List<int>() { attendanceWithoutAdjustment.Id }
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        if (attendanceWithoutAdjustment.FinalTotalOvertimeValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalTotalOvertimeValue;
                            payrollSystemIntegrationDTO.Formula = generalOption.OvertimeBenefit != null ? generalOption.OvertimeBenefit.Formula : Formula.HoursOfSalary;
                            payrollSystemIntegrationDTO.ExtraValue = generalOption.OvertimeBenefit != null ? generalOption.OvertimeBenefit.ExtraValue : 0;
                            payrollSystemIntegrationDTO.ExtraValueFormula = generalOption.OvertimeBenefit != null ? generalOption.OvertimeBenefit.ExtraValueFormula : ExtraValueFormula.None;
                        }
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        if (attendanceWithoutAdjustment.TotalAbsenceDaysValue > 0)
                        {
                            var absenceDaysCountToAddAsDeduction = 0;
                            var absencesDays = new List<DateTime>();
                            if (generalOption.MinimunOfNonAttendanceDaysToRemoveWeeklyHolidays > 0 &&
                            form.RelyHolidaies)
                            {
                                foreach (var absence in attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails.
                                Where(x => x.ActualWorkValue <= 0 && !x.HasMission && !x.HasVacation && !x.IsOffDay && !x.IsHoliday))
                                {
                                    absencesDays.Add(absence.Date);
                                }
                                absencesDays = absencesDays.OrderBy(x => x.Date).ToList();
                                var weeklyDays = GetWeekDaysBetweenTwoDates(absencesDays.FirstOrDefault(), toDate, publicHolidays);
                                foreach (var weekDay in weeklyDays)
                                {
                                    var absenceDaysBeforeWeekDay = absencesDays.Where(x => x.Date < weekDay && (weekDay.Date - x.Date).TotalDays <= 7).ToList();
                                    if (absenceDaysBeforeWeekDay.Count >= generalOption.MinimunOfNonAttendanceDaysToRemoveWeeklyHolidays)
                                        absenceDaysCountToAddAsDeduction += 1;
                                }
                            }
                            extraDays = absenceDaysCountToAddAsDeduction;
                            payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.TotalAbsenceDaysValue;
                            payrollSystemIntegrationDTO.Formula = generalOption.AbsenceDaysDeduction != null ? generalOption.AbsenceDaysDeduction.Formula : Formula.DaysOfPackageSalary;
                            payrollSystemIntegrationDTO.ExtraValue = generalOption.AbsenceDaysDeduction != null ? generalOption.AbsenceDaysDeduction.ExtraValue : 0;
                            payrollSystemIntegrationDTO.ExtraValueFormula = generalOption.AbsenceDaysDeduction != null ? generalOption.AbsenceDaysDeduction.ExtraValueFormula : ExtraValueFormula.None;
                        }
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        if (attendanceWithoutAdjustment.FinalNonAttendanceTotalValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalNonAttendanceTotalValue;
                            payrollSystemIntegrationDTO.Formula = generalOption.NonAttendanceDeduction != null ? generalOption.NonAttendanceDeduction.Formula : Formula.HoursOfPackageSalary;
                            payrollSystemIntegrationDTO.ExtraValue = generalOption.NonAttendanceDeduction != null ? generalOption.NonAttendanceDeduction.ExtraValue : 0;
                            payrollSystemIntegrationDTO.ExtraValueFormula = generalOption.NonAttendanceDeduction != null ? generalOption.NonAttendanceDeduction.ExtraValueFormula : ExtraValueFormula.None;
                        }
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        if (attendanceWithoutAdjustment.FinalLatenessTotalValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalLatenessTotalValue;
                            payrollSystemIntegrationDTO.Formula = generalOption.LatenessDeduction != null ? generalOption.LatenessDeduction.Formula : Formula.HoursOfPackageSalary;
                            payrollSystemIntegrationDTO.ExtraValue = generalOption.LatenessDeduction != null ? generalOption.LatenessDeduction.ExtraValue : 0;
                            payrollSystemIntegrationDTO.ExtraValueFormula = generalOption.LatenessDeduction != null ? generalOption.LatenessDeduction.ExtraValueFormula : ExtraValueFormula.None;
                        }
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceMonthlyAdjustment(AttendanceMonthlyAdjustment attendanceMonthlyAdjustment, AttendanceType attendanceType,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption,
            List<PublicHoliday> publicHolidays,
            HRIS.Domain.AttendanceSystem.Configurations.AttendanceForm form)
        {
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                Repetition = 1,
                SourceId = new List<int>() { attendanceMonthlyAdjustment.Id }
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        if (attendanceMonthlyAdjustment.FinalOvertimeValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceMonthlyAdjustment.FinalOvertimeValue;
                            payrollSystemIntegrationDTO.Formula = generalOption.OvertimeBenefit != null ? generalOption.OvertimeBenefit.Formula : Formula.HoursOfSalary;
                            payrollSystemIntegrationDTO.ExtraValue = generalOption.OvertimeBenefit != null ? generalOption.OvertimeBenefit.ExtraValue : 0;
                            payrollSystemIntegrationDTO.ExtraValueFormula = generalOption.OvertimeBenefit != null ? generalOption.OvertimeBenefit.ExtraValueFormula : ExtraValueFormula.None;
                        }
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        if (attendanceMonthlyAdjustment.FinalNonAttendanceValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceMonthlyAdjustment.FinalNonAttendanceValue;
                            payrollSystemIntegrationDTO.Formula = generalOption.NonAttendanceDeduction != null ? generalOption.NonAttendanceDeduction.Formula : Formula.HoursOfPackageSalary;
                            payrollSystemIntegrationDTO.ExtraValue = generalOption.NonAttendanceDeduction != null ? generalOption.NonAttendanceDeduction.ExtraValue : 0;
                            payrollSystemIntegrationDTO.ExtraValueFormula = generalOption.NonAttendanceDeduction != null ? generalOption.NonAttendanceDeduction.ExtraValueFormula : ExtraValueFormula.None;
                        }
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceDailyAdjustment(
            AttendanceDailyAdjustment attendanceDailyAdjustment, AttendanceType attendanceType, DateTime toDate,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption,
            List<PublicHoliday> publicHolidays,
            HRIS.Domain.AttendanceSystem.Configurations.AttendanceForm form, out int extraDays)
        {
            extraDays = 0;
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                Repetition = 1,
                SourceId = new List<int>() { attendanceDailyAdjustment.Id }
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        if (attendanceDailyAdjustment.FinalOvertimeValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.FinalOvertimeValue;
                            payrollSystemIntegrationDTO.Formula = generalOption.OvertimeBenefit != null ? generalOption.OvertimeBenefit.Formula : Formula.HoursOfSalary;
                            payrollSystemIntegrationDTO.ExtraValue = generalOption.OvertimeBenefit != null ? generalOption.OvertimeBenefit.ExtraValue : 0;
                            payrollSystemIntegrationDTO.ExtraValueFormula = generalOption.OvertimeBenefit != null ? generalOption.OvertimeBenefit.ExtraValueFormula : ExtraValueFormula.None;
                        }
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        if (attendanceDailyAdjustment.TotalAbsenceDays > 0)
                        {
                            var absenceDaysCountToAddAsDeduction = 0;
                            var absencesDays = new List<DateTime>();
                            if (generalOption.MinimunOfNonAttendanceDaysToRemoveWeeklyHolidays > 0 &&
                            form.RelyHolidaies)
                            {
                                foreach (var absence in attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails.
                                Where(x => x.DailyAdjustmentAttendanceStatus == DailyAdjustmentAttendanceStatus.Absence))
                                {
                                    absencesDays.Add(absence.Date);
                                }
                                absencesDays = absencesDays.OrderBy(x => x.Date).ToList();
                                var weeklyDays = GetWeekDaysBetweenTwoDates(absencesDays.FirstOrDefault(), toDate, publicHolidays);
                                foreach (var weekDay in weeklyDays)
                                {
                                    var absenceDaysBeforeWeekDay = absencesDays.Where(x => x.Date < weekDay && (weekDay.Date - x.Date).TotalDays <= 7).ToList();
                                    if (absenceDaysBeforeWeekDay.Count >= generalOption.MinimunOfNonAttendanceDaysToRemoveWeeklyHolidays)
                                        absenceDaysCountToAddAsDeduction += 1;
                                }
                            }
                            extraDays = absenceDaysCountToAddAsDeduction;
                            payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.TotalAbsenceDays;
                            payrollSystemIntegrationDTO.Formula = generalOption.AbsenceDaysDeduction != null ? generalOption.AbsenceDaysDeduction.Formula : Formula.DaysOfPackageSalary;
                            payrollSystemIntegrationDTO.ExtraValue = generalOption.AbsenceDaysDeduction != null ? generalOption.AbsenceDaysDeduction.ExtraValue : 0;
                            payrollSystemIntegrationDTO.ExtraValueFormula = generalOption.AbsenceDaysDeduction != null ? generalOption.AbsenceDaysDeduction.ExtraValueFormula : ExtraValueFormula.None;
                            //payrollSystemIntegrationDTO.Note = absenceDaysCountToAddAsDeduction > 0 ?
                            //    EmployeeRelationServicesLocalizationHelper.GetResource(
                            //        EmployeeRelationServicesLocalizationHelper.TotalAbsenceDaysValueWithDeletedHolidayDaysWhichValueIs) + " " +
                            //        absenceDaysCountToAddAsDeduction : "";
                        }
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        if (attendanceDailyAdjustment.FinalNonAttendanceValue > 0)
                        {
                            payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.FinalNonAttendanceValue;
                            payrollSystemIntegrationDTO.Formula = generalOption.NonAttendanceDeduction != null ? generalOption.NonAttendanceDeduction.Formula : Formula.HoursOfPackageSalary;
                            payrollSystemIntegrationDTO.ExtraValue = generalOption.NonAttendanceDeduction != null ? generalOption.NonAttendanceDeduction.ExtraValue : 0;
                            payrollSystemIntegrationDTO.ExtraValueFormula = generalOption.NonAttendanceDeduction != null ? generalOption.NonAttendanceDeduction.ExtraValueFormula : ExtraValueFormula.None;
                        }
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        public static void AcceptAttendance(AttendanceRecord attendanceRecord, AttendanceType attendanceType, int sourceId)
        {

            if (attendanceRecord != null)
            {
                AcceptAttendanceWithoutAdjustment(attendanceType, attendanceRecord, sourceId);
                AcceptAttendanceMonthlyAdjustment(attendanceType, attendanceRecord, sourceId);
                AcceptAttendanceDailyAdjustment(attendanceType, attendanceRecord, sourceId);
            }
        }
        private static void AcceptAttendanceWithoutAdjustment(AttendanceType attendanceType, AttendanceRecord attendanceRecord, int sourceId)
        {
            if (attendanceRecord != null)
            {
                var attendanceWithoutAdjustment =
                    attendanceRecord.AttendanceWithoutAdjustments.FirstOrDefault(x => x.Id == sourceId);

                if (attendanceWithoutAdjustment != null)
                {
                    switch (attendanceType)
                    {
                        case AttendanceType.Overtime:
                            {
                                attendanceWithoutAdjustment.IsOvertimeTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Absence:
                            {
                                attendanceWithoutAdjustment.IsAbsenceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.NonAttendance:
                            {
                                attendanceWithoutAdjustment.IsNonAttendanceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Lateness:
                            {
                                attendanceWithoutAdjustment.IsLatenessTransferToPayroll = true;
                                break;
                            }
                    }
                }

            }

        }
        private static void AcceptAttendanceMonthlyAdjustment(AttendanceType attendanceType, AttendanceRecord attendanceRecord, int sourceId)
        {
            if (attendanceRecord != null)
            {
                var attendanceMonthlyAdjustment =
                    attendanceRecord.AttendanceMonthlyAdjustments.FirstOrDefault(x => x.Id == sourceId);

                if (attendanceMonthlyAdjustment != null)
                {
                    switch (attendanceType)
                    {
                        case AttendanceType.Overtime:
                            {
                                attendanceMonthlyAdjustment.IsOvertimeTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Absence:
                            {
                                attendanceMonthlyAdjustment.IsAbsenceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.NonAttendance:
                            {
                                attendanceMonthlyAdjustment.IsNonAttendanceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Lateness:
                            {
                                attendanceMonthlyAdjustment.IsLatenessTransferToPayroll = true;
                                break;
                            }
                    }
                }

            }



        }
        private static void AcceptAttendanceDailyAdjustment(AttendanceType attendanceType, AttendanceRecord attendanceRecord, int sourceId)
        {
            if (attendanceRecord != null)
            {
                var attendanceDailyAdjustment =
                    attendanceRecord.AttendanceDailyAdjustments.FirstOrDefault(x => x.Id == sourceId);

                if (attendanceDailyAdjustment != null)
                {
                    switch (attendanceType)
                    {
                        case AttendanceType.Overtime:
                            {
                                attendanceDailyAdjustment.IsOvertimeTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Absence:
                            {
                                attendanceDailyAdjustment.IsAbsenceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.NonAttendance:
                            {
                                attendanceDailyAdjustment.IsNonAttendanceTransferToPayroll = true;
                                break;
                            }
                        case AttendanceType.Lateness:
                            {
                                attendanceDailyAdjustment.IsLatenessTransferToPayroll = true;
                                break;
                            }
                    }
                }
            }



        }
        private static double GetSpentDaysBetweenTwoDates(TravelMission travelMission, Employee employee, AttendanceForm attendanceForm, DateTime fromDate, DateTime toDate,
            List<PublicHoliday> publicHolidays, List<FixedHoliday> fixedHolidays, List<ChangeableHoliday> changableHolidays)
        {
            var spentDays = LeaveService.GetSpentDays(attendanceForm, travelMission.FromDate >= fromDate ? travelMission.FromDate : fromDate,
                travelMission.ToDate <= toDate ? travelMission.ToDate : toDate, false, publicHolidays, fixedHolidays, changableHolidays);
            return spentDays;
        }
        public static double GetHolidayDays(Employee employee, AttendanceForm attendanceForm, DateTime startDate, DateTime endDate,
            List<PublicHoliday> publicHolidays, List<FixedHoliday> fixedHolidays, List<ChangeableHoliday> changableHolidays, List<OvertimeOrder> overtimeOrders, AttendanceForm form)
        {
            var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().FirstOrDefault(x => x.FromDate == startDate && x.ToDate == endDate && x.AttendanceMonthStatus == AttendanceMonthStatus.Locked);

            overtimeOrders = overtimeOrders.Where(x => x.TakeConsiderationHolidaysDeduction).ToList();
            var recurrenceIndex = AttendanceSystem.Services.AttendanceService.GetRecurrenceIndexByDate(attendanceForm, startDate);
            IList<WorkshopRecurrence> recurrences = attendanceForm != null ? attendanceForm.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList() : new List<WorkshopRecurrence>();
            double spentDays = 0;
            var relyHolidays = attendanceForm != null ? attendanceForm.RelyHolidaies : true;
            while (DateTime.Parse(startDate.ToShortDateString()) <= DateTime.Parse(endDate.ToShortDateString()))
            {
                if (!CheckOvertimeOrderIsAppliedInCustomDay(overtimeOrders, startDate, employee, attendanceRecord, attendanceForm))
                {

                    if (relyHolidays && (publicHolidays.Any(x => x.DayOfWeek == startDate.DayOfWeek) ||
                         fixedHolidays.Any(x => x.StartDate <= startDate && x.EndDate >= startDate) ||
                         changableHolidays.Any(x => x.StartDate <= startDate && x.EndDate >= startDate)))
                    {
                        spentDays++;
                    }
                    else if (recurrences[(int)recurrenceIndex].IsOff)
                    {
                        spentDays++;
                    }
                }
                recurrenceIndex++;
                startDate = startDate.AddDays(1);
                if (recurrenceIndex >= recurrences.Count)
                {
                    recurrenceIndex = 0;
                }
            }
            return spentDays;
        }

        private static bool CheckOvertimeOrderIsAppliedInCustomDay(List<OvertimeOrder> overtimeOrders, DateTime startDate, Employee employee, AttendanceRecord attendanceRecord, AttendanceForm attendanceForm)
        {
            var isExisted = false;
            try
            {
                var overtimeOrder = overtimeOrders.Where(x => x.FromDate <= startDate && x.ToDate >= startDate).FirstOrDefault();
                if (overtimeOrder != null)
                {
                    switch (attendanceForm.CalculationMethod)
                    {
                        case CalculationMethod.DailyAdjustment:
                            var attendanceDailyAdjustment = GetAttendanceDailyAdjustment(attendanceRecord, employee, AttendanceType.Overtime, false);
                            isExisted = attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails.Any(x => x.Date.Date == startDate && x.OvertimeOrderValue > 0);
                            break;
                        case CalculationMethod.MonthlyAdjustment:
                            var attendanceMonthlyAdjustment = GetAttendanceMonthlyAdjustment(attendanceRecord, employee, AttendanceType.Overtime, false);
                            isExisted = attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails.Any(x => x.Date.Date == startDate && x.OvertimeOrderValue > 0);
                            break;
                        case CalculationMethod.WithoutAdjustment:
                            var attendanceWithoutAdjustment = GetAttendanceWithoutAdjustment(attendanceRecord, employee, AttendanceType.Overtime, false);
                            isExisted = attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails.Any(x => x.Date.Date == startDate && x.OvertimeOrderValue > 0);
                            break;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return isExisted;
        }


        public static PayrollSystemIntegrationDTO GetHolidaysDeduction(Employee employee,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption,
            List<PublicHoliday> publicHolidays,
            List<FixedHoliday> fixedHolidays,
            List<ChangeableHoliday> changeableHolidays,
            HRIS.Domain.AttendanceSystem.Configurations.AttendanceForm form,
            List<OvertimeOrder> overtimeOrders, DateTime fromDate, DateTime toDate)
        {


            if (employee != null)
            {
                double total = GetHolidayDays(employee, form, fromDate.Date, toDate.Date, publicHolidays, fixedHolidays, changeableHolidays, overtimeOrders, form);
                if (total > 0)
                    return new PayrollSystemIntegrationDTO
                    {
                        Value = total,
                        Formula = generalOption.HolidayDeduction != null ? generalOption.HolidayDeduction.Formula : Formula.DaysOfPackageSalary,
                        ExtraValue = generalOption.HolidayDeduction != null ? generalOption.HolidayDeduction.ExtraValue : -100,
                        ExtraValueFormula = generalOption.HolidayDeduction != null ? generalOption.HolidayDeduction.ExtraValueFormula : ExtraValueFormula.PercentageOfInitialValue,
                        Repetition = 1,
                        SourceType = 0,
                        DeductionCard = generalOption.HolidayDeduction
                    };
            }

            return null;
        }
        public static PayrollSystemIntegrationDTO GetHourlyMissionsBenefit(Employee employee, List<HourlyMission> hourlyMissions,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption)
        {

            var financialCard = employee?.EmployeeCard?.FinancialCard;
            if (financialCard != null && hourlyMissions.Any() && (generalOption.HourlyMissionValue > 0 || financialCard.HourlyMissionValue > 0))
            {
                var missionValue = financialCard.HourlyMissionValue > 0 ? financialCard.HourlyMissionValue : generalOption.HourlyMissionValue;
                var ids = new List<int>();
                double total = 0;
                List<HourlyMission> missions = hourlyMissions.Where(x => x.Employee.Id == employee.Id).ToList();
                foreach (var mission in missions)
                {
                    ids.Add(mission.Id);
                    total += (mission.EndDateTime - mission.StartDateTime).TotalHours;
                }
                if (total > 0)
                    return new PayrollSystemIntegrationDTO
                    {
                        Value = total * missionValue,
                        Formula = generalOption.HourlyMissionBenefit != null ? generalOption.HourlyMissionBenefit.Formula : Formula.HoursOfPackageSalary,
                        ExtraValue = generalOption.HourlyMissionBenefit != null ? generalOption.HourlyMissionBenefit.ExtraValue : 0,
                        ExtraValueFormula = generalOption.HourlyMissionBenefit != null ? generalOption.HourlyMissionBenefit.ExtraValueFormula : ExtraValueFormula.None,
                        Repetition = 1,
                        SourceId = ids,
                        SourceType = 0
                    };
            }

            return null;
        }
        public static PayrollSystemIntegrationDTO GetInternalTravelMissionsBenefit(Employee employee, List<TravelMission> missions,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption)
        {
            var financialCard = employee?.EmployeeCard?.FinancialCard;
            if (financialCard != null && missions.Any() && (generalOption.InternalTravelMissionValue > 0 || financialCard.InternalTravelMissionValue > 0))
            {
                var missionValue = financialCard.InternalTravelMissionValue > 0 ? financialCard.InternalTravelMissionValue : generalOption.InternalTravelMissionValue;
                var ids = new List<int>();
                double total = 0;
                foreach (var mission in missions)
                {
                    double spentDays = (mission.ToDate.Date - mission.FromDate.Date).TotalDays + 1;
                    ids.Add(mission.Id);
                    total += spentDays;
                }
                if (total > 0)
                    return new PayrollSystemIntegrationDTO
                    {
                        Value = total * missionValue,
                        Formula = generalOption.InternalTravelMissionBenefit != null ? generalOption.InternalTravelMissionBenefit.Formula : Formula.DaysOfPackageSalary,
                        ExtraValue = generalOption.InternalTravelMissionBenefit != null ? generalOption.InternalTravelMissionBenefit.ExtraValue : 0,
                        ExtraValueFormula = generalOption.InternalTravelMissionBenefit != null ? generalOption.InternalTravelMissionBenefit.ExtraValueFormula : ExtraValueFormula.None,
                        Repetition = 1,
                        SourceId = ids,
                        SourceType = 0
                    };
            }

            return null;
        }
        public static PayrollSystemIntegrationDTO GetExternalTravelMissionsBenefit(Employee employee, List<TravelMission> missions,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption)
        {
            var financialCard = employee?.EmployeeCard?.FinancialCard;
            if (financialCard != null && missions.Any() && (generalOption.ExternalTravelMissionValue > 0 || financialCard.ExternalTravelMissionValue > 0))
            {
                var result = new PayrollSystemIntegrationDTO();
                var missionValue = financialCard.ExternalTravelMissionValue > 0 ? financialCard.ExternalTravelMissionValue : generalOption.ExternalTravelMissionValue;
                var ids = new List<int>();
                double total = 0;
                foreach (var mission in missions)
                {
                    double spentDays = (mission.ToDate.Date - mission.FromDate.Date).TotalDays + 1;
                    ids.Add(mission.Id);
                    total += spentDays;
                }
                if (total > 0)
                    return new PayrollSystemIntegrationDTO
                    {
                        Value = total * missionValue,
                        Formula = generalOption.ExternalTravelMissionBenefit != null ? generalOption.ExternalTravelMissionBenefit.Formula : Formula.DaysOfPackageSalary,
                        ExtraValue = generalOption.ExternalTravelMissionBenefit != null ? generalOption.ExternalTravelMissionBenefit.ExtraValue : 0,
                        ExtraValueFormula = generalOption.ExternalTravelMissionBenefit != null ? generalOption.ExternalTravelMissionBenefit.ExtraValueFormula : ExtraValueFormula.None,
                        Repetition = 1,
                        SourceId = ids,
                        SourceType = 0
                    };
            }

            return null;
        }
        public static PayrollSystemIntegrationDTO GetHourlyMissionsDeduction(Employee employee, List<HourlyMission> hourlyMissions,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption)
        {
            if (employee != null && hourlyMissions.Any())
            {
                var ids = new List<int>();
                double total = 0;
                foreach (var mission in hourlyMissions)
                {
                    ids.Add(mission.Id);
                    total += (mission.EndDateTime - mission.StartDateTime).TotalHours;
                }
                if (total > 0)
                    return new PayrollSystemIntegrationDTO
                    {
                        Value = total,
                        Formula = generalOption.HourlyMissionDeduction != null ? generalOption.HourlyMissionDeduction.Formula : Formula.DaysOfPackageSalary,
                        ExtraValue = generalOption.HourlyMissionDeduction != null ? generalOption.HourlyMissionDeduction.ExtraValue : -100,
                        ExtraValueFormula = generalOption.HourlyMissionDeduction != null ? generalOption.HourlyMissionDeduction.ExtraValueFormula : ExtraValueFormula.PercentageOfInitialValue,
                        Repetition = 1,
                        SourceId = ids,
                        SourceType = 0,
                        DeductionCard = generalOption.HourlyMissionDeduction
                    };
            }

            return null;
        }
        public static PayrollSystemIntegrationDTO GetDailyMissionsDeduction(Employee employee, List<TravelMission> missions,
            HRIS.Domain.PayrollSystem.Configurations.GeneralOption generalOption,
            List<PublicHoliday> publicHolidays,
            List<FixedHoliday> fixedHolidays,
            List<ChangeableHoliday> changeableHolidays,
            HRIS.Domain.AttendanceSystem.Configurations.AttendanceForm form, DateTime fromDate, DateTime toDate)
        {

            if (employee != null && missions.Any())
            {
                var ids = new List<int>();
                double total = 0;
                foreach (var mission in missions)
                {
                    double spentDays = GetSpentDaysBetweenTwoDates(mission, employee, form, fromDate.Date, toDate.Date, publicHolidays, fixedHolidays, changeableHolidays);
                    ids.Add(mission.Id);
                    total += spentDays;
                }
                if (total > 0)
                    return new PayrollSystemIntegrationDTO
                    {
                        Value = total,
                        Formula = generalOption.TravelMissionDeduction != null ? generalOption.TravelMissionDeduction.Formula : Formula.DaysOfPackageSalary,
                        ExtraValue = generalOption.TravelMissionDeduction != null ? generalOption.TravelMissionDeduction.ExtraValue : -100,
                        ExtraValueFormula = generalOption.TravelMissionDeduction != null ? generalOption.TravelMissionDeduction.ExtraValueFormula : ExtraValueFormula.PercentageOfInitialValue,
                        Repetition = 1,
                        SourceId = ids,
                        SourceType = 0,
                        DeductionCard = generalOption.TravelMissionDeduction
                    };
            }

            return null;
        }

        #endregion

    }


}