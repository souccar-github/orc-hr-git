using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.DTO;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Services
{

    public static class PayrollSystemIntegrationService
    {

        #region // Employee Relation Services //

       
        public static List<PayrollSystemIntegrationDTO> GetPenalties(Employee employee, bool isAccepted, DateTime? fromDate = null, DateTime? toDate = null)
        {
            // العقوبات

            List<EmployeeDisciplinary> penalties;

            if (!fromDate.HasValue && !toDate.HasValue)
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.Where(x => x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.
                    Where(x => x.DisciplinaryDate >= fromDate && x.DisciplinaryDate <= toDate && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.
                    Where(x => x.DisciplinaryDate >= fromDate && x.IsTransferToPayroll == isAccepted).ToList();
            else
                penalties = employee.EmployeeCard.EmployeeDisciplinarys.
                    Where(x => x.DisciplinaryDate <= toDate && x.IsTransferToPayroll == isAccepted).ToList();

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
                rewards = employee.EmployeeCard.EmployeeRewards.Where(x => x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue && toDate.HasValue)
                rewards = employee.EmployeeCard.EmployeeRewards.
                    Where(x => x.RewardDate >= fromDate && x.RewardDate <= toDate && x.IsTransferToPayroll == isAccepted).ToList();
            else if (fromDate.HasValue)
                rewards = employee.EmployeeCard.EmployeeRewards.
                    Where(x => x.RewardDate >= fromDate && x.IsTransferToPayroll == isAccepted).ToList();
            else
                rewards = employee.EmployeeCard.EmployeeRewards.
                    Where(x => x.RewardDate <= toDate && x.IsTransferToPayroll == isAccepted).ToList();

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
    
        public static void AcceptPenalties(Employee employee, int sourceId)
        {
            var penalty = employee.EmployeeCard.EmployeeDisciplinarys.FirstOrDefault(x => x.Id == sourceId);
            if (penalty == null)
                return;
            penalty.IsTransferToPayroll = true;
            ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
        }
        public static void AcceptRewards(Employee employee, int sourceId)
        {
            var reward = employee.EmployeeCard.EmployeeRewards.FirstOrDefault(x => x.Id == sourceId);
            if (reward == null)
                return;
            reward.IsTransferToPayroll = true;
            ServiceFactory.ORMService.Save(employee, UserExtensions.CurrentUser);
        }
      
        #endregion

        #region // Attendance //
        public enum AttendanceType
        {
            Overtime = 0,
            Absence = 1,
            NonAttendance = 2,
            Lateness = 3
        }
        public static List<PayrollSystemIntegrationDTO> ImportFromAttendance(AttendanceType attendanceType, Employee employee, DateTime date, bool isAccepted)
        {
            // الدوام الإضافي

            var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().FirstOrDefault(x => (int)x.Month == date.Month);
            var payrollSystemIntegrationDtOs = new List<PayrollSystemIntegrationDTO>();

            if (attendanceRecord != null)
            {
                var attendanceWithoutAdjustment = GetAttendanceWithoutAdjustment(attendanceRecord, employee, attendanceType, isAccepted);
                var attendanceMonthlyAdjustment = GetAttendanceMonthlyAdjustment(attendanceRecord, employee, attendanceType, isAccepted);
                var attendanceDailyAdjustment = GetAttendanceDailyAdjustment(attendanceRecord, employee, attendanceType, isAccepted);

                if (attendanceWithoutAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceWithoutAdjustment(attendanceWithoutAdjustment, attendanceType);
                    if (payrollSystemIntegrationDTO != null)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                }

                if (attendanceMonthlyAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceMonthlyAdjustment(attendanceMonthlyAdjustment, attendanceType);
                    if (payrollSystemIntegrationDTO != null)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
                }

                if (attendanceDailyAdjustment != null)
                {
                    var payrollSystemIntegrationDTO = GetDTOFromAttendanceDailyAdjustment(attendanceDailyAdjustment, attendanceType);
                    if (payrollSystemIntegrationDTO != null)
                        payrollSystemIntegrationDtOs.Add(payrollSystemIntegrationDTO);
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
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceWithoutAdjustment(AttendanceWithoutAdjustment attendanceWithoutAdjustment, AttendanceType attendanceType)
        {
            var payrollSystemIntegrationDTO = new PayrollSystemIntegrationDTO
            {
                Value = 0,
                Formula = Formula.HoursOfSalary,
                Repetition = 1,
                SourceId =  new List<int>() { attendanceWithoutAdjustment.Id }
            };

            switch (attendanceType)
            {
                case AttendanceType.Overtime:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalTotalOvertimeValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.TotalAbsenceDaysValue;
                        payrollSystemIntegrationDTO.Formula = Formula.DaysOfSalary;
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalNonAttendanceTotalValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Lateness:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceWithoutAdjustment.FinalLatenessTotalValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
            }

            return payrollSystemIntegrationDTO;
        }
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceMonthlyAdjustment(AttendanceMonthlyAdjustment attendanceMonthlyAdjustment, AttendanceType attendanceType)
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
                        payrollSystemIntegrationDTO.Value = attendanceMonthlyAdjustment.FinalOvertimeValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        payrollSystemIntegrationDTO = null;
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceMonthlyAdjustment.FinalNonAttendanceValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
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
        private static PayrollSystemIntegrationDTO GetDTOFromAttendanceDailyAdjustment(AttendanceDailyAdjustment attendanceDailyAdjustment, AttendanceType attendanceType)
        {
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
                        payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.FinalOvertimeValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
                        break;
                    }
                case AttendanceType.Absence:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.TotalAbsenceDays;
                        payrollSystemIntegrationDTO.Formula = Formula.DaysOfSalary;
                        break;
                    }
                case AttendanceType.NonAttendance:
                    {
                        payrollSystemIntegrationDTO.Value = attendanceDailyAdjustment.FinalNonAttendanceValue;
                        payrollSystemIntegrationDTO.Formula = Formula.HoursOfSalary;
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
        public static void AcceptAttendance(AttendanceType attendanceType, DateTime date, int sourceId)
        {
            var attendanceRecord = ServiceFactory.ORMService.All<AttendanceRecord>().FirstOrDefault(x => (int)x.Month == date.Month);

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

                ServiceFactory.ORMService.Save(attendanceRecord, UserExtensions.CurrentUser);
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

                ServiceFactory.ORMService.Save(attendanceRecord, UserExtensions.CurrentUser);
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

                ServiceFactory.ORMService.Save(attendanceRecord, UserExtensions.CurrentUser);
            }



        }

        #endregion

    }


}