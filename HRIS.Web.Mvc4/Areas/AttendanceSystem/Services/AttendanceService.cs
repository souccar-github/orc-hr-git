using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HRIS.Domain.AttendanceSystem.DTO;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.AttendanceSystem.Enums;

using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Extensions;
using WebGrease.Css.Extensions;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using Souccar.Infrastructure.Core;
using HRIS.Domain.AttendanceSystem.Configurations;
using Souccar.Core.Utilities;
using HRIS.Domain.Personnel.Entities;
using DevExpress.XtraCharts.Native;
using FluentNHibernate.Testing.Values;
using System.Collections;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.Grades.RootEntities;
using System.Data.SqlClient;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Services
{//todo : Mhd Update changeset no.1

    public class AttendanceService
    {

        #region Attendance Record Calculation

        public static void CalculateAttendanceRecord(AttendanceRecord attendanceRecord)
        {
            IList<OvertimeOrder> overTimeOrders = ServiceFactory.ORMService.All<OvertimeOrder>().ToList();
            List<IAggregateRoot> entities = new List<IAggregateRoot>();
            List<AttendanceInfraction> attendanceRecordInfractions = ServiceFactory.ORMService.All<AttendanceInfraction>().Where(x => x.AttendanceRecord.Id == attendanceRecord.Id).ToList();
            ServiceFactory.ORMService.DeleteTransaction(attendanceRecordInfractions, UserExtensions.CurrentUser);
            List<AttendanceInfraction> attendanceInfractions = ServiceFactory.ORMService.All<AttendanceInfraction>().Where(x => x.IsActiveForNextPenalties).ToList();
            List<AttendanceInfraction> updatedInfractions = new List<AttendanceInfraction>();

            GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            if (attendanceRecord.AttendanceWithoutAdjustments.Any())
                CalculateAttendanceWithoutAdjustment(attendanceRecord.AttendanceWithoutAdjustments, generalSetting, overTimeOrders, attendanceInfractions, updatedInfractions, entities);

            if (attendanceRecord.AttendanceDailyAdjustments.Any())
                CalculateAttendanceDailyAdjustment(attendanceRecord.AttendanceDailyAdjustments, generalSetting, overTimeOrders, attendanceInfractions, updatedInfractions, entities);

            if (attendanceRecord.AttendanceMonthlyAdjustments.Any())
                CalculateAttendanceMonthlyAdjustment(attendanceRecord.AttendanceMonthlyAdjustments, generalSetting, overTimeOrders, attendanceInfractions, updatedInfractions, entities);
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
            attendanceRecord.Save();
        }

        public static void CalculateAttendanceMonthlyAdjustment(IList<AttendanceMonthlyAdjustment> attendanceMonthlyAdjustments, GeneralSettings generalSetting, IList<OvertimeOrder> overTimeOrders, List<AttendanceInfraction> attendanceInfractions, List<AttendanceInfraction> updatedInfractions, List<IAggregateRoot> entites)
        {
            OvertimeForm generalSettingsOvertimeForm = generalSetting != null ? generalSetting.OvertimeForm : null;
            var employeeAttendanceCards = attendanceMonthlyAdjustments.Select(x => x.EmployeeAttendanceCard).ToList();

            DateTime fromDate = attendanceMonthlyAdjustments[0].AttendanceRecord.FromDate;

            DateTime endDate = attendanceMonthlyAdjustments[0].AttendanceRecord.ToDate >= DateTime.Now.Date ? DateTime.Now.Date : attendanceMonthlyAdjustments[0].AttendanceRecord.ToDate;

            // اعادة كافة التواترات وما يقابلها من ورديات خلال الفترة المحددة
            Dictionary<Employee, List<WorkshopRecurrenceDTO>> allRecurrences = GetWorkshopsRecurrenceInPeriod(employeeAttendanceCards, fromDate, endDate);
            IList<Employee> employees = new List<Employee>();
            foreach (var emp in employeeAttendanceCards)
            {
                employees.Add(emp.Employee);
            }
            IList<LeaveRequest> EmpDayLeave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.IsHourlyLeave == false && employeeAttendanceCards.Contains(x.EmployeeCard)).ToList();
            IList<LeaveRequest> EmpHourlyLeave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.IsHourlyLeave == true && employeeAttendanceCards.Contains(x.EmployeeCard)).ToList();
            IList<HourlyMission> EmpHourlyMission = ServiceFactory.ORMService.All<HourlyMission>().Where(x => employees.Contains(x.Employee) && x.Status == HRIS.Domain.Global.Enums.Status.Approved).ToList();
            IList<TravelMission> EmpTravelMission = ServiceFactory.ORMService.All<TravelMission>().Where(x => employees.Contains(x.Employee) && x.Status == HRIS.Domain.Global.Enums.Status.Approved).ToList();
            foreach (var attendanceMonthlyAdjustment in attendanceMonthlyAdjustments)
            {
                List<EmployeeDisciplinary> employeeDisciplinarysShouldRemoved = new List<EmployeeDisciplinary>();
                employeeDisciplinarysShouldRemoved.AddRange(attendanceMonthlyAdjustment.EmployeeDisciplinarys.Where(x => !attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails.Any(y=> x.DisciplinaryDate == y.Date)));
                foreach (var item in employeeDisciplinarysShouldRemoved)
                {
                    attendanceMonthlyAdjustment.EmployeeDisciplinarys.Remove(item);
                }
                if (attendanceMonthlyAdjustment.IsCalculated)
                    continue;
                attendanceMonthlyAdjustment.InitialOvertimeValue = 0.0;
                attendanceMonthlyAdjustment.FinalOvertimeValue = 0.0;
                attendanceMonthlyAdjustment.FinalNonAttendanceValue = 0.0;
                attendanceMonthlyAdjustment.InitialOvertimeValueFormatedValue = String.Empty;
                attendanceMonthlyAdjustment.FinalOvertimeValueFormatedValue = String.Empty;
                attendanceMonthlyAdjustment.FinalNonAttendanceValueFormatedValue = String.Empty;

                var recurrences = allRecurrences[attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee];

                for (var i = 0; i < attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails.Count; i++)
                {
                    if (attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].IsCalculated)
                        continue;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].RecurrenceIndex = 0;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].IsWorkDay = false;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].HasVacation = false;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].IsOffDay = false;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].IsHoliday = false;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].HasMission = false;

                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].VacationValue = 0.0;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].OvertimeOrderValue = 0.0;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].WorkHoursValue = 0.0;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].ActualWorkHoursValue = 0.0;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].MissionValue = 0.0;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].VacationValueFormatedValue = String.Empty;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].OvertimeOrderValueFormatedValue = String.Empty;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].WorkHoursValueFormatedValue = String.Empty;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].ActualWorkHoursValueFormatedValue = String.Empty;
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].MissionValueFormatedValue = String.Empty;



                    var entranceExitRecords = GetEntranceExitRecords(recurrences[i], attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee);

                    if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Work)
                    {
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].IsWorkDay = true;
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].WorkHoursValue = recurrences[i].Workshop.TotalWorkHours;
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].RecurrenceIndex =
                            recurrences[i].RecurrenceIndex;
                    }
                    else if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Off)
                    {
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].IsOffDay = true;
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].RecurrenceIndex =
                            recurrences[i].RecurrenceIndex;
                    }
                    else if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Holiday)
                    {
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].IsHoliday = true;
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].RecurrenceIndex =
                            recurrences[i].RecurrenceIndex;
                    }

                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].WorkHoursValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].WorkHoursValue);
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].ActualWorkHoursValue = GetActualWorkHoursValue(entranceExitRecords);
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].ActualWorkHoursValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].ActualWorkHoursValue);
                    #region Vacation Values

                    if (AttendanceSystemIntegrationService.IsEmployeeHasDailyVacation(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Date, EmpDayLeave))
                    {
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].HasVacation = true;
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].VacationValue +=
                            attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].WorkHoursValue;
                    }
                    else if (AttendanceSystemIntegrationService.IsEmployeeHasHourlyVacation(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Workshop.NormalShifts, EmpHourlyLeave))
                    {
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].HasVacation = true;
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].VacationValue +=
                            AttendanceSystemIntegrationService.GetEmployeeHourlyVacation(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Workshop.NormalShifts, EmpHourlyLeave);
                    }
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].VacationValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].VacationValue);
                    #endregion

                    #region Mission Values

                    if (AttendanceSystemIntegrationService.IsEmployeeHasDailyMission(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee,
                        recurrences[i].Date, EmpTravelMission))
                    {
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].HasMission = true;
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].MissionValue +=
                            attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].WorkHoursValue;
                    }
                    else if (AttendanceSystemIntegrationService.IsEmployeeHasHourlyMission(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee,
                        recurrences[i].Workshop.NormalShifts, EmpHourlyMission))
                    {
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].HasMission = true;
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].MissionValue +=
                            AttendanceSystemIntegrationService.GetEmployeeHourlyMission(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee,
                                recurrences[i].Workshop.NormalShifts, EmpHourlyMission);
                    }
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].MissionValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].MissionValue);
                    #endregion

                    #region Overtime Values

                    if (IsEmployeeHasOvertimeOrder(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Date, overTimeOrders))
                    {
                        attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].OvertimeOrderValue = GetEmployeeOvertimeOrderValue(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Date, overTimeOrders);
                    }
                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].OvertimeOrderValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].OvertimeOrderValue);
                    #endregion

                    attendanceMonthlyAdjustment.AttendanceMonthlyAdjustmentDetails[i].IsCalculated = true;
                }

                #region Overtime Values

                if (GetOvertimeForm(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee, generalSettingsOvertimeForm).NeedOverTimeAcceptance)
                {
                    attendanceMonthlyAdjustment.InitialOvertimeValue =
                        attendanceMonthlyAdjustment.ExpectedOvertimeValue >
                        attendanceMonthlyAdjustment.TotalOvertimeOrderValue
                            ? attendanceMonthlyAdjustment.TotalOvertimeOrderValue
                            : attendanceMonthlyAdjustment.ExpectedOvertimeValue;

                    attendanceMonthlyAdjustment.InitialOvertimeValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceMonthlyAdjustment.InitialOvertimeValue);
                }
                else
                {
                    attendanceMonthlyAdjustment.InitialOvertimeValue = attendanceMonthlyAdjustment.ExpectedOvertimeValue;
                    attendanceMonthlyAdjustment.InitialOvertimeValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceMonthlyAdjustment.InitialOvertimeValue);
                }

                #endregion

                #region Final Values

                attendanceMonthlyAdjustment.FinalOvertimeValue = GetNormalOverTimeFinalValue(attendanceMonthlyAdjustment.InitialOvertimeValue, attendanceMonthlyAdjustment.EmployeeAttendanceCard, generalSetting);
                attendanceMonthlyAdjustment.FinalOvertimeValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceMonthlyAdjustment.FinalOvertimeValue);

                attendanceMonthlyAdjustment.FinalNonAttendanceValue = GetNonAttendanceFinalValue(attendanceMonthlyAdjustment.InitialNonAttendanceValue, attendanceMonthlyAdjustment.EmployeeAttendanceCard, generalSetting);
                attendanceMonthlyAdjustment.FinalNonAttendanceValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceMonthlyAdjustment.FinalNonAttendanceValue);

                var NonAttendanceInfractionInfo = "";
                var infraction = GetAttendanceInfraction(attendanceMonthlyAdjustment.InitialNonAttendanceValue, attendanceMonthlyAdjustment.AttendanceRecord,
                    attendanceMonthlyAdjustment.EmployeeAttendanceCard, generalSetting,
                    attendanceInfractions.Where(x => x.EmployeeCard.Id == attendanceMonthlyAdjustment.EmployeeAttendanceCard.Id).ToList(),
                    attendanceMonthlyAdjustment.AttendanceRecord.FromDate, true, updatedInfractions, out NonAttendanceInfractionInfo);
                if (infraction != null)
                {
                    entites.Add(infraction);
                    attendanceInfractions.Add(infraction);
                    var employeeDisciplinary = new EmployeeDisciplinary()
                    {
                        DisciplinaryDate = attendanceMonthlyAdjustment.AttendanceRecord.FromDate,
                        DisciplinaryReason = NonAttendanceInfractionInfo,
                        DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Approved,
                        DisciplinarySetting = infraction.Penalty,
                        Comment = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.FromAttendanceRecordThatIsName) + " " + attendanceMonthlyAdjustment.AttendanceRecord.Month + "-" + attendanceMonthlyAdjustment.AttendanceRecord.Year,
                        Creator = UserExtensions.CurrentUser
                    };
                    var employeeDisciplinaryExist = attendanceMonthlyAdjustment.EmployeeDisciplinarys.FirstOrDefault(x => x.DisciplinaryDate == employeeDisciplinary.DisciplinaryDate && x.Comment == employeeDisciplinary.Comment);
                    if (employeeDisciplinaryExist == null)
                        attendanceMonthlyAdjustment.AddEmployeeDisciplinary(employeeDisciplinary);
                    else
                    {
                        employeeDisciplinaryExist.DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Approved;
                        employeeDisciplinaryExist.DisciplinaryReason = NonAttendanceInfractionInfo;
                        employeeDisciplinaryExist.DisciplinarySetting = infraction.Penalty;
                        employeeDisciplinaryExist.Creator = UserExtensions.CurrentUser;
                        entites.Add(employeeDisciplinaryExist);
                    }
                }
                //attendanceMonthlyAdjustment.Penalty =
                //    GetNonAttendancePenalty(attendanceMonthlyAdjustment.InitialNonAttendanceValue,
                //        attendanceMonthlyAdjustment.EmployeeAttendanceCard, generalSetting);
            }
            #endregion
        }

        public static void CalculateAttendanceDailyAdjustment(IList<AttendanceDailyAdjustment> attendanceDailyAdjustments, GeneralSettings generalSetting, IList<OvertimeOrder> overTimeOrders, List<AttendanceInfraction> attendanceInfractions, List<AttendanceInfraction> updatedInfractions, List<IAggregateRoot> entites)
        {

            var employeeAttendanceCards = attendanceDailyAdjustments.Select(x => x.EmployeeAttendanceCard).ToList();

            OvertimeForm generalSettingsOvertimeForm = generalSetting != null ? generalSetting.OvertimeForm : null;

            DateTime fromDate = attendanceDailyAdjustments[0].AttendanceRecord.FromDate;

            DateTime endDate = attendanceDailyAdjustments[0].AttendanceRecord.ToDate >= DateTime.Now.Date ? DateTime.Now.Date : attendanceDailyAdjustments[0].AttendanceRecord.ToDate;

            // اعادة كافة التواترات وما يقابلها من ورديات خلال الفترة المحددة
            var allRecurrences = GetWorkshopsRecurrenceInPeriod(employeeAttendanceCards, fromDate, endDate);
            IList<Employee> employees = new List<Employee>();
            foreach (var emp in employeeAttendanceCards)
            {
                employees.Add(emp.Employee);
            }
            IList<LeaveRequest> EmpDayLeave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.IsHourlyLeave == false && employeeAttendanceCards.Contains(x.EmployeeCard)).ToList();
            IList<LeaveRequest> EmpHourlyLeave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.IsHourlyLeave == true && employeeAttendanceCards.Contains(x.EmployeeCard)).ToList();
            IList<HourlyMission> EmpHourlyMission = ServiceFactory.ORMService.All<HourlyMission>().Where(x => employees.Contains(x.Employee) && x.Status == HRIS.Domain.Global.Enums.Status.Approved).ToList();
            IList<TravelMission> EmpTravelMission = ServiceFactory.ORMService.All<TravelMission>().Where(x => employees.Contains(x.Employee) && x.Status == HRIS.Domain.Global.Enums.Status.Approved).ToList();

            foreach (var attendanceDailyAdjustment in attendanceDailyAdjustments)
            {
                List<EmployeeDisciplinary> employeeDisciplinarysShouldRemoved = new List<EmployeeDisciplinary>();
                employeeDisciplinarysShouldRemoved.AddRange(attendanceDailyAdjustment.EmployeeDisciplinarys.Where(x => !attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails.Any(y => x.DisciplinaryDate == y.Date)));
                foreach (var item in employeeDisciplinarysShouldRemoved)
                {
                    attendanceDailyAdjustment.EmployeeDisciplinarys.Remove(item);
                }
                if (attendanceDailyAdjustment.IsCalculated)
                    continue;
                attendanceDailyAdjustment.FinalOvertimeValue = 0.0;
                attendanceDailyAdjustment.FinalNonAttendanceValue = 0.0;
                attendanceDailyAdjustment.FinalOvertimeValueFormatedValue = String.Empty;
                attendanceDailyAdjustment.FinalNonAttendanceValueFormatedValue = String.Empty;

                var recurrences = allRecurrences[attendanceDailyAdjustment.EmployeeAttendanceCard.Employee];

                for (var i = 0; i < attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails.Count; i++)
                {

                    if (attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].IsCalculated)
                        continue;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].RecurrenceIndex = 0;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HasVacation = false;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HasMission = false;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].IsOffDay = false;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].IsHoliday = false;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].IsWorkDay = false;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].VacationValue = 0;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].WorkHoursValue = 0;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].MissionValue = 0;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].ActualWorkHoursValue = 0;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].OvertimeOrderValue = 0;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].NormalOvertimeValue = 0;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HolidayOvertimeValue = 0;

                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].VacationValueFormatedValue = String.Empty;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].WorkHoursValueFormatedValue = String.Empty;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].MissionValueFormatedValue = String.Empty;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].ActualWorkHoursValueFormatedValue = String.Empty;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].OvertimeOrderValueFormatedValue = String.Empty;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].NormalOvertimeValueFormatedValue = String.Empty;
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HolidayOvertimeValueFormatedValue = String.Empty;




                    var entranceExitRecords = GetEntranceExitRecords(recurrences[i],
                        attendanceDailyAdjustment.EmployeeAttendanceCard.Employee);

                    if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Work)
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].IsWorkDay = true;
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].WorkHoursValue = recurrences[i].Workshop.TotalWorkHours;
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].RecurrenceIndex = recurrences[i].RecurrenceIndex;
                    }
                    else if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Off)
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].IsOffDay = true;
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].RecurrenceIndex =
                            recurrences[i].RecurrenceIndex;
                    }
                    else if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Holiday)
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].IsHoliday = true;
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].RecurrenceIndex =
                            recurrences[i].RecurrenceIndex;
                    }
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].WorkHoursValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].WorkHoursValue);
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].ActualWorkHoursValue = GetActualWorkHoursValue(entranceExitRecords);
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].ActualWorkHoursValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].ActualWorkHoursValue);
                    #region Vacation Values

                    if (
                        AttendanceSystemIntegrationService.IsEmployeeHasDailyVacation(
                            attendanceDailyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Date, EmpDayLeave))
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HasVacation = true;
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].VacationValue += attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].WorkHoursValue;
                    }
                    else if (
                        AttendanceSystemIntegrationService.IsEmployeeHasHourlyVacation(attendanceDailyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Workshop.NormalShifts, EmpHourlyLeave))
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HasVacation = true;
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].VacationValue +=
                            AttendanceSystemIntegrationService.GetEmployeeHourlyVacation(attendanceDailyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Workshop.NormalShifts, EmpHourlyLeave);
                    }
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].VacationValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].VacationValue);
                    #endregion

                    #region Mission Values

                    if (AttendanceSystemIntegrationService.IsEmployeeHasDailyMission(attendanceDailyAdjustment.EmployeeAttendanceCard.Employee,
                        recurrences[i].Date, EmpTravelMission))
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HasMission = true;
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].MissionValue +=
                            attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].WorkHoursValue;
                    }
                    else if (AttendanceSystemIntegrationService.IsEmployeeHasHourlyMission(attendanceDailyAdjustment.EmployeeAttendanceCard.Employee,
                        recurrences[i].Workshop.NormalShifts, EmpHourlyMission))
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HasMission = true;
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].MissionValue +=
                            AttendanceSystemIntegrationService.GetEmployeeHourlyMission(attendanceDailyAdjustment.EmployeeAttendanceCard.Employee,
                                recurrences[i].Workshop.NormalShifts, EmpHourlyMission);
                    }
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].MissionValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].MissionValue);
                    #endregion

                    #region Overtime Values

                    if (IsEmployeeHasOvertimeOrder(attendanceDailyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Date, overTimeOrders))
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].OvertimeOrderValue = GetEmployeeOvertimeOrderValue(attendanceDailyAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Date, overTimeOrders);
                    }
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].OvertimeOrderValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].OvertimeOrderValue);

                    double tempOvertimeValue;
                    if (GetOvertimeForm(attendanceDailyAdjustment.EmployeeAttendanceCard.Employee, generalSettingsOvertimeForm).NeedOverTimeAcceptance)
                    {
                        tempOvertimeValue =
                            attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].OrderedOvertimeValue;
                    }
                    else
                    {
                        tempOvertimeValue =
                            attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].ExpectedOvertimeValue;
                    }

                    if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Work)
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].NormalOvertimeValue = tempOvertimeValue;
                    }
                    else
                    {
                        attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HolidayOvertimeValue = tempOvertimeValue;
                    }
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].NormalOvertimeValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].NormalOvertimeValue);
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HolidayOvertimeValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].HolidayOvertimeValue);
                    #endregion

                    var NonAttendanceInfractionInfo = "";
                    var infractionOfNonAttendance = GetAttendanceInfraction(attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].NonAttendanceHoursValue, attendanceDailyAdjustment.AttendanceRecord,
                        attendanceDailyAdjustment.EmployeeAttendanceCard, generalSetting, attendanceInfractions.Where(x => x.EmployeeCard.Id == attendanceDailyAdjustment.EmployeeAttendanceCard.Id).ToList(),
                        recurrences[i].Date, true, updatedInfractions, out NonAttendanceInfractionInfo);
                    if (infractionOfNonAttendance != null)
                    {
                        entites.Add(infractionOfNonAttendance);
                        attendanceInfractions.Add(infractionOfNonAttendance);
                        var employeeDisciplinary = new EmployeeDisciplinary()
                        {
                            DisciplinaryDate = recurrences[i].Date,
                            DisciplinaryReason = NonAttendanceInfractionInfo,
                            DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Approved,
                            DisciplinarySetting = infractionOfNonAttendance.Penalty,
                            Comment = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.FromAttendanceRecordThatIsName) + " " + attendanceDailyAdjustment.AttendanceRecord.Month + "-" + attendanceDailyAdjustment.AttendanceRecord.Year,
                            Creator = UserExtensions.CurrentUser
                        };
                        var employeeDisciplinaryExist = attendanceDailyAdjustment.EmployeeDisciplinarys.FirstOrDefault(x => x.DisciplinaryDate == employeeDisciplinary.DisciplinaryDate && x.Comment == employeeDisciplinary.Comment);
                        if (employeeDisciplinaryExist == null)
                            attendanceDailyAdjustment.AddEmployeeDisciplinary(employeeDisciplinary);
                        else
                        {
                            employeeDisciplinaryExist.DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Approved;
                            employeeDisciplinaryExist.DisciplinaryReason = NonAttendanceInfractionInfo;
                            employeeDisciplinaryExist.DisciplinarySetting = infractionOfNonAttendance.Penalty;
                            employeeDisciplinaryExist.Creator = UserExtensions.CurrentUser;
                            entites.Add(employeeDisciplinaryExist);
                        }
                    }
                    attendanceDailyAdjustment.AttendanceDailyAdjustmentDetails[i].IsCalculated = true;
                }

                #region Final Values

                attendanceDailyAdjustment.FinalOvertimeValue = 0;
                attendanceDailyAdjustment.FinalOvertimeValue += GetNormalOverTimeFinalValue(attendanceDailyAdjustment.TotalNormalOvertimeValue, attendanceDailyAdjustment.EmployeeAttendanceCard, generalSetting);
                attendanceDailyAdjustment.FinalOvertimeValue += GetHolidayOverTimeFinalValue(attendanceDailyAdjustment.TotalHolidayOvertimeValue, attendanceDailyAdjustment.EmployeeAttendanceCard, generalSetting);
                attendanceDailyAdjustment.FinalOvertimeValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceDailyAdjustment.FinalOvertimeValue);

                attendanceDailyAdjustment.FinalNonAttendanceValue = GetNonAttendanceFinalValue(attendanceDailyAdjustment.InitialNonAttendanceValue, attendanceDailyAdjustment.EmployeeAttendanceCard, generalSetting);
                attendanceDailyAdjustment.FinalNonAttendanceValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceDailyAdjustment.FinalNonAttendanceValue);




            }

            #endregion

        }

        public static void CalculateAttendanceWithoutAdjustment(IList<AttendanceWithoutAdjustment> attendanceWithoutAdjustments, GeneralSettings generalSetting, IList<OvertimeOrder> overTimeOrders, List<AttendanceInfraction> attendanceInfractions, List<AttendanceInfraction> updatedInfractions, List<IAggregateRoot> entites)
        {

            var employeeAttendanceCards = attendanceWithoutAdjustments.Select(x => x.EmployeeAttendanceCard).ToList();


            DateTime fromDate = attendanceWithoutAdjustments[0].AttendanceRecord.FromDate;

            DateTime endDate = attendanceWithoutAdjustments[0].AttendanceRecord.ToDate >= DateTime.Now.Date ? DateTime.Now.Date : attendanceWithoutAdjustments[0].AttendanceRecord.ToDate;

            // اعادة كافة التواترات وما يقابلها من ورديات خلال الفترة المحددة
            var allRecurrences = GetWorkshopsRecurrenceInPeriod(employeeAttendanceCards, fromDate, endDate);
            foreach (var attendanceWithoutAdjustment in attendanceWithoutAdjustments)
            {
                List<EmployeeDisciplinary> employeeDisciplinarysShouldRemoved = new List<EmployeeDisciplinary>();
                employeeDisciplinarysShouldRemoved.AddRange(attendanceWithoutAdjustment.EmployeeDisciplinarys.Where(x => !attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails.Any(y => x.DisciplinaryDate == y.Date)));
                foreach (var item in employeeDisciplinarysShouldRemoved)
                {
                    attendanceWithoutAdjustment.EmployeeDisciplinarys.Remove(item);
                }
                if (attendanceWithoutAdjustment.IsCalculated)
                    continue;
                attendanceWithoutAdjustment.FinalLatenessTotalValue = 0.0;
                attendanceWithoutAdjustment.FinalNonAttendanceTotalValue = 0.0;
                attendanceWithoutAdjustment.FinalTotalOvertimeValue = 0.0;
                attendanceWithoutAdjustment.FinalLatenessTotalValueFormatedValue = String.Empty;
                attendanceWithoutAdjustment.FinalNonAttendanceTotalValueFormatedValue = String.Empty;
                attendanceWithoutAdjustment.FinalTotalOvertimeValueFormatedValue = String.Empty;



                var recurrences = allRecurrences[attendanceWithoutAdjustment.EmployeeAttendanceCard.Employee];

                for (var i = 0; i < attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails.Count; i++)
                {
                    if (attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].IsCalculated)
                        continue;
                    #region Reset values

                    //todo Mhd Alsaadi ClearAttendanceDetail يفضل نقلها الى ميثود   
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RecurrenceIndex = 0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ExpectedOvertimeValue = 0.0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ExpectedOvertimeRanges = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].OvertimeOrderValue = 0.0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].OvertimeOrderRanges = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ParticularOvertimeValue = 0.0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].HolidayOvertimeValue = 0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NormalOvertimeValue = 0.0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ActualWorkValue = 0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ActualWorkRanges = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].LatenessHoursValue = 0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].LatenessHoursRanges = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].MissionValue = 0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].MissionRanges = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NonAttendanceHoursValue = 0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NonAttendanceHoursRanges = String.Empty;

                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].VacationValue = 0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].VacationRanges = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RequiredWorkHoursValue = 0;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RequiredWorkHoursRanges = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].IsWorkDay = false;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].IsOffDay = false;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].IsHoliday = false;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].HasMission = false;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].HasVacation = false;

                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ActualWorkFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ExpectedOvertimeFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].HolidayOvertimeFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].LatenessHoursFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].MissionFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NonAttendanceHoursFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NormalOvertimeFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].OvertimeOrderFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ParticularOvertimeFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RequiredWorkHoursFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RestFormatedValue = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].VacationFormatedValue = String.Empty;

                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RestRanges = String.Empty;
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RestValue = 0;

                    #endregion


                    var entranceExitRecords = GetEntranceExitRecords(recurrences[i],
                        attendanceWithoutAdjustment.EmployeeAttendanceCard.Employee);

                    if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Work)
                    {
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].IsWorkDay = true;
                        var counter = i;
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[counter].RequiredWorkHoursRanges = "";
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RequiredWorkHoursValue =
                            recurrences[i].Workshop.TotalWorkHours;


                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RecurrenceIndex =
                            recurrences[i].RecurrenceIndex;
                    }
                    else if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Off)
                    {
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].IsOffDay = true;
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RecurrenceIndex =
                            recurrences[i].RecurrenceIndex;
                    }
                    else if (recurrences[i].RecurrenceType == WorkshopRecurrenceTypeDTO.Holiday)
                    {
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].IsHoliday = true;
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RecurrenceIndex =
                            recurrences[i].RecurrenceIndex;
                    }
                    else
                    {
                        throw new Exception("This case is not supported.");
                    }

                    var originalOvertimeOrderValue =
                        GetEmployeeOvertimeOrderValue(attendanceWithoutAdjustment.EmployeeAttendanceCard.Employee, recurrences[i].Date, overTimeOrders);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].OriginalOvertimeOrderFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(originalOvertimeOrderValue);
                    //حساب الاضافي
                    SetOvertimeValuesForWithoutAdjustment(
                        recurrences[i],
                        attendanceWithoutAdjustment.EmployeeAttendanceCard,
                        entranceExitRecords,
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i], generalSetting, overTimeOrders);

                    //حساب الدوام (التأخر الصباحي , الغياب , عدم التواجد ....)
                    SetAttendanceValuesForWithoutAdjustment(
                        recurrences[i],
                        attendanceWithoutAdjustment.EmployeeAttendanceCard,
                        entranceExitRecords,
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i]);

                    //حساب الدوام الفعلي ومجالاته
                    SetActualWorkValuesForWithoutAdjustment(
                        recurrences[i],
                        attendanceWithoutAdjustment.EmployeeAttendanceCard,
                        entranceExitRecords,
                        attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i]);


                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ActualWorkFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ActualWorkValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ExpectedOvertimeFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ExpectedOvertimeValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].HolidayOvertimeFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].HolidayOvertimeValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].LatenessHoursFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].LatenessHoursValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].MissionFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].MissionValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NonAttendanceHoursFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NonAttendanceHoursValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NormalOvertimeFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NormalOvertimeValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].OvertimeOrderFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].OvertimeOrderValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ParticularOvertimeFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].ParticularOvertimeValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RequiredWorkHoursFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RequiredWorkHoursValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].VacationFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].VacationValue);
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RestFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].RestValue);
                    var LatenessInfractionInfo = "";
                    var NonAttendanceInfractionInfo = "";
                    var infractionOfLateness = GetAttendanceInfraction(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].LatenessHoursValue, attendanceWithoutAdjustment.AttendanceRecord,
                        attendanceWithoutAdjustment.EmployeeAttendanceCard, generalSetting,
                        attendanceInfractions.Where(x => x.EmployeeCard.Id == attendanceWithoutAdjustment.EmployeeAttendanceCard.Id).ToList(),
                        recurrences[i].Date, false, updatedInfractions, out LatenessInfractionInfo);
                    var infractionOfNonAttendance = GetAttendanceInfraction(attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].NonAttendanceHoursValue, attendanceWithoutAdjustment.AttendanceRecord,
                        attendanceWithoutAdjustment.EmployeeAttendanceCard, generalSetting,
                        attendanceInfractions.Where(x => x.EmployeeCard.Id == attendanceWithoutAdjustment.EmployeeAttendanceCard.Id).ToList(),
                        recurrences[i].Date, true, updatedInfractions, out NonAttendanceInfractionInfo);
                    if (infractionOfLateness != null)
                    {
                        entites.Add(infractionOfLateness);
                        attendanceInfractions.Add(infractionOfLateness);
                        var employeeDisciplinary = new EmployeeDisciplinary()
                        {
                            DisciplinaryDate = recurrences[i].Date,
                            DisciplinaryReason = LatenessInfractionInfo,
                            DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Approved,
                            DisciplinarySetting = infractionOfLateness.Penalty,
                            Comment = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.FromAttendanceRecordThatIsName) + " " + attendanceWithoutAdjustment.AttendanceRecord.Month + "-" + attendanceWithoutAdjustment.AttendanceRecord.Year,
                            Creator = UserExtensions.CurrentUser
                        };
                        var employeeDisciplinaryExist = attendanceWithoutAdjustment.EmployeeDisciplinarys.FirstOrDefault(x => x.DisciplinaryDate == employeeDisciplinary.DisciplinaryDate && x.Comment == employeeDisciplinary.Comment);
                        if (employeeDisciplinaryExist == null)
                            attendanceWithoutAdjustment.AddEmployeeDisciplinary(employeeDisciplinary);
                        else
                        {
                            employeeDisciplinaryExist.DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Approved;
                            employeeDisciplinaryExist.DisciplinaryReason = NonAttendanceInfractionInfo;
                            employeeDisciplinaryExist.DisciplinarySetting = infractionOfNonAttendance.Penalty;
                            employeeDisciplinaryExist.Creator = UserExtensions.CurrentUser;
                            entites.Add(employeeDisciplinaryExist);
                        }
                    }
                    if (infractionOfNonAttendance != null)
                    {
                        entites.Add(infractionOfNonAttendance);
                        attendanceInfractions.Add(infractionOfNonAttendance);
                        var employeeDisciplinary = new EmployeeDisciplinary()
                        {
                            DisciplinaryDate = recurrences[i].Date,
                            DisciplinaryReason = NonAttendanceInfractionInfo,
                            DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Approved,
                            DisciplinarySetting = infractionOfNonAttendance.Penalty,
                            Comment = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.FromAttendanceRecordThatIsName) + " " + attendanceWithoutAdjustment.AttendanceRecord.Month + "-" + attendanceWithoutAdjustment.AttendanceRecord.Year,
                            Creator = UserExtensions.CurrentUser
                        };
                        var employeeDisciplinaryExist = attendanceWithoutAdjustment.EmployeeDisciplinarys.FirstOrDefault(x => x.DisciplinaryDate == employeeDisciplinary.DisciplinaryDate && x.Comment == employeeDisciplinary.Comment);
                        if (employeeDisciplinaryExist == null)
                            attendanceWithoutAdjustment.AddEmployeeDisciplinary(employeeDisciplinary);
                        else
                        {
                            employeeDisciplinaryExist.DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Approved;
                            employeeDisciplinaryExist.DisciplinaryReason = NonAttendanceInfractionInfo;
                            employeeDisciplinaryExist.DisciplinarySetting = infractionOfNonAttendance.Penalty;
                            employeeDisciplinaryExist.Creator = UserExtensions.CurrentUser;
                            entites.Add(employeeDisciplinaryExist);
                        }
                    }
                    attendanceWithoutAdjustment.AttendanceWithoutAdjustmentDetails[i].IsCalculated = true;
                }
                // اجمالي الاضافي
                attendanceWithoutAdjustment.FinalTotalOvertimeValue =
                    GetNormalOverTimeFinalValue(attendanceWithoutAdjustment.TotalNormalOvertimeValue, attendanceWithoutAdjustment.EmployeeAttendanceCard, generalSetting) +
                    GetHolidayOverTimeFinalValue(attendanceWithoutAdjustment.TotalHolidayOvertimeValue, attendanceWithoutAdjustment.EmployeeAttendanceCard, generalSetting) +
                    GetParticularOverTimeFinalValue(attendanceWithoutAdjustment.TotalParticularOvertimeValue, attendanceWithoutAdjustment.EmployeeAttendanceCard, generalSetting);



                //اجمالي عدم التواجد
                attendanceWithoutAdjustment.FinalNonAttendanceTotalValue = GetNonAttendanceFinalValue(attendanceWithoutAdjustment.InitialNonAttendanceTotalValue, attendanceWithoutAdjustment.EmployeeAttendanceCard, generalSetting);
                //attendanceWithoutAdjustment.NonAttendancePenalty = GetNonAttendancePenalty(attendanceWithoutAdjustment.InitialNonAttendanceTotalValue, attendanceWithoutAdjustment.EmployeeAttendanceCard, generalSetting);

                // اجمالي التأخرات
                attendanceWithoutAdjustment.FinalLatenessTotalValue = GetLatenessFinalValue(attendanceWithoutAdjustment.InitialLatenessTotalValue, attendanceWithoutAdjustment.EmployeeAttendanceCard, generalSetting);
                //attendanceWithoutAdjustment.LatenessPenalty = GetLatenessPenalty(attendanceWithoutAdjustment.InitialLatenessTotalValue, attendanceWithoutAdjustment.EmployeeAttendanceCard, generalSetting);

                attendanceWithoutAdjustment.FinalTotalOvertimeValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.FinalTotalOvertimeValue);
                attendanceWithoutAdjustment.FinalNonAttendanceTotalValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.FinalNonAttendanceTotalValue);
                attendanceWithoutAdjustment.FinalLatenessTotalValueFormatedValue = DateTimeFormatter.ConvertDoubleToTimeFormat(attendanceWithoutAdjustment.FinalLatenessTotalValue);
            }


        }

        #endregion

        #region Attendance Record Calculation Helper


        private class TimeLineNodeDTO
        {
            public DateTime LogDateTime { get; set; }
            public LogType LogType { get; set; }
            public bool IsMission { get; set; }
            public bool IsLateness { get; set; }
            public bool IsNonAttendance { get; set; }
            public bool IsVacation { get; set; }
            public bool IsAttendance { get; set; }
            public bool IsRestTime { get; set; }
            public bool IsAbsent { get; set; }
        }
        private class OverTimeNodeDTO
        {
            public DateTime LogDateTime { get; set; }
            public LogType LogType { get; set; }
            public bool IsParticular { get; set; }
        }

        private static LinkedList<TimeLineNodeDTO> SetAttendanceValuesForWithoutAdjustment(WorkshopRecurrenceDTO recurrence, EmployeeCard employeeAttendanceCard, List<EntranceExitRecord> entranceExitRecords, AttendanceWithoutAdjustmentDetail attendanceWithoutAdjustmentDetail)
        {


            var timeLine = new LinkedList<TimeLineNodeDTO>();
            if (recurrence.RecurrenceType == WorkshopRecurrenceTypeDTO.Holiday || recurrence.RecurrenceType == WorkshopRecurrenceTypeDTO.Off)
            {
                return timeLine;
            }


            IList<LeaveRequest> EmpDayLeave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.IsHourlyLeave == false && x.EmployeeCard == employeeAttendanceCard).ToList();
            IList<LeaveRequest> EmpHourlyLeave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.IsHourlyLeave == true && x.EmployeeCard == employeeAttendanceCard).ToList();
            IList<HourlyMission> EmpHourlyMission = ServiceFactory.ORMService.All<HourlyMission>().Where(x => x.Employee == employeeAttendanceCard.Employee && x.Status == HRIS.Domain.Global.Enums.Status.Approved).ToList();
            IList<TravelMission> EmpTravelMission = ServiceFactory.ORMService.All<TravelMission>().Where(x => x.Employee == employeeAttendanceCard.Employee && x.Status == HRIS.Domain.Global.Enums.Status.Approved).ToList();

            var workshopNormalShiftsRanges = recurrence.Workshop.Prepare(recurrence.Date).NormalShifts;// GetWorkshopNormalShiftsRanges(recurrence.Workshop.NormalShifts.ToList(), recurrence.Date);
            LinkedListNode<TimeLineNodeDTO> currentNode;

            foreach (var range in workshopNormalShiftsRanges)
            {
                var rangeTimeLine = new LinkedList<TimeLineNodeDTO>();
                #region Attendance

                entranceExitRecords.Where(
                    x =>
                        x.LogDateTime >= range.EntryTime &&
                        x.LogDateTime <= range.ExitTime)
                    .Select(x => new TimeLineNodeDTO
                    {
                        LogDateTime = x.LogDateTime,
                        LogType = x.LogType,
                        IsAttendance = true
                    })
                    .OrderBy(x => x.LogDateTime)
                    .ForEach(x => rangeTimeLine.AddLast(x));

                if (rangeTimeLine.Count != 0)
                {
                    // وهي حالة وجود تسجيلا دخول وخروج ضمن بداية ونهاية الدوام اما حالة ان الدخول والخروج خارج اوقات الدوام ستعالج بعد الشرط الحالي
                    if (rangeTimeLine.Last.Value.LogType == LogType.Entrance)
                    {
                        // مما يعني  أنه خرج بعد نهاية الدوام لذلك نعتبره خرج نهاية الدوام ونتجاهل خروجه بعد الدوام لأنه دوام بدون تقاص
                        rangeTimeLine.AddLast(new TimeLineNodeDTO
                        {
                            LogDateTime = range.ExitTime,
                            LogType = LogType.Exit,
                            IsAttendance = true
                        });
                    }
                    if (rangeTimeLine.First.Value.LogType == LogType.Exit)
                    {
                        // مما يعني  أنه دخل قبل بداية الدوام لذلك نعتبره دخل أول الدوام ونتجاهل دخوله قبل الدوام لأنه دوام بدون تقاص
                        rangeTimeLine.AddFirst(new TimeLineNodeDTO
                        {
                            LogDateTime = range.EntryTime,
                            LogType = LogType.Entrance,
                            IsAttendance = true
                        });
                    }
                    if (rangeTimeLine.First.Value.LogDateTime <=
                        range.EntryTime.AddMinutes(range.IgnoredPeriodAfterEntryTime))
                    {
                        // أي أنه دخل ضمن فترة السماحية بالتالي نعتبره دخل بداية الدوام بشكل نظامي
                        rangeTimeLine.First.Value.LogDateTime = range.EntryTime;
                    }

                    if (rangeTimeLine.Last.Value.LogDateTime >=
                        range.ExitTime.AddMinutes(-range.IgnoredPeriodBeforeExitTime))
                    {
                        // أي أنه دخل ضمن فترة السماحية بالتالي نعتبره دخل بداية الدوام بشكل نظامي
                        rangeTimeLine.Last.Value.LogDateTime = range.ExitTime;
                    }
                }
                else
                {
                    var temp =
                        entranceExitRecords.Where(
                            x => x.LogDateTime >= range.ShiftRangeStartTime && x.LogDateTime <= range.ShiftRangeEndTime);
                    if (temp.Count() != 0)
                    {
                        // أي يوجد له دخول أو خروج خارج أوقات الدوام بالتالي يتم أعتباره دخل وخرج بشكل نظامي
                        rangeTimeLine.AddFirst(new TimeLineNodeDTO
                        {
                            LogDateTime = range.EntryTime,
                            LogType = LogType.Entrance,
                            IsAttendance = true
                        });
                        rangeTimeLine.AddLast(new TimeLineNodeDTO
                        {
                            LogDateTime = range.ExitTime,
                            LogType = LogType.Exit,
                            IsAttendance = true
                        });
                    }
                }

                #endregion

                #region Daily Mission

                var isEmployeeHasDailyMission = AttendanceSystemIntegrationService.IsEmployeeHasDailyMission(employeeAttendanceCard.Employee,
                    recurrence.Date, EmpTravelMission);
                if (isEmployeeHasDailyMission)
                {
                    // يتم تجاهل جميع التسجيلات بيوم المهمة واستبدالها بدخول وخروج واحد يكافئ بداية ونهاية الدوام
                    rangeTimeLine.Clear();

                    // أي أنه بمهمة يومية وليس لدينا اي تسجيل دخول أو خروج عندها نضيف له دوام يبدأ وينتهي حسب الفترة المحددة
                    rangeTimeLine.AddFirst(new TimeLineNodeDTO
                    {
                        LogDateTime = range.EntryTime,
                        LogType = LogType.Entrance,
                        IsMission = true
                    });
                    rangeTimeLine.AddLast(new TimeLineNodeDTO
                    {
                        LogDateTime = range.ExitTime,
                        LogType = LogType.Exit,
                        IsMission = true
                    });
                }
                #endregion
                #region Hourly Missions

                else
                {
                    // أختبار حالة المهمة الساعية في حال عدم وجود المهمة اليومية
                    var currentRangeMissions =
                        AttendanceSystemIntegrationService.GetEmployeeHourlyMissionDetails(employeeAttendanceCard.Employee, recurrence.Workshop.NormalShifts, EmpHourlyMission)
                            .Where(x => x.StartDateTime >= range.EntryTime && x.EndDateTime <= range.ExitTime)
                            .ToList();

                    foreach (var currentRangeMission in currentRangeMissions)
                    {
                        currentNode = rangeTimeLine.First;
                        var startMissionNode = new TimeLineNodeDTO
                        {
                            LogDateTime = currentRangeMission.StartDateTime,
                            LogType = LogType.Entrance,
                            IsMission = true
                        };
                        var endMissionNode = new TimeLineNodeDTO
                        {
                            LogDateTime = currentRangeMission.EndDateTime,
                            LogType = LogType.Exit,
                            IsMission = true
                        };
                        bool startAdded = false;
                        bool endAdded = false;
                        while (currentNode != null)
                        {
                            if (currentNode.Value.LogDateTime < currentRangeMission.StartDateTime)
                            {
                                currentNode = currentNode.Next;
                                continue;
                            }
                            if (currentNode.Value.LogDateTime >= currentRangeMission.StartDateTime && currentNode.Value.LogDateTime <= currentRangeMission.EndDateTime)
                            {
                                if (currentNode.Value.LogType == LogType.Exit && !startAdded)
                                {
                                    startAdded = true;
                                    if (currentNode.Previous.Value.LogDateTime < currentRangeMission.StartDateTime)
                                    {
                                        rangeTimeLine.AddBefore(currentNode, new TimeLineNodeDTO
                                        {
                                            LogDateTime = currentRangeMission.StartDateTime,
                                            LogType = LogType.Exit,
                                            IsAttendance = currentNode.Value.IsAttendance,
                                            IsMission = currentNode.Value.IsMission,
                                            IsLateness = currentNode.Value.IsLateness,
                                            IsNonAttendance = currentNode.Value.IsNonAttendance,
                                            IsVacation = currentNode.Value.IsVacation,
                                            IsRestTime = currentNode.Value.IsRestTime
                                        });
                                    }
                                    rangeTimeLine.AddBefore(currentNode, startMissionNode);
                                }
                                var swap = currentNode.Next;
                                rangeTimeLine.Remove(currentNode);
                                currentNode = swap;
                                continue;
                            }
                            if (currentNode.Value.LogDateTime > currentRangeMission.EndDateTime)
                            {
                                if (currentNode.Value.LogType == LogType.Exit)
                                {
                                    endAdded = true;
                                    if (!startAdded)
                                    {
                                        startAdded = true;
                                        rangeTimeLine.AddBefore(currentNode, startMissionNode);
                                    }
                                    rangeTimeLine.AddBefore(currentNode, endMissionNode);

                                    rangeTimeLine.AddBefore(currentNode, new TimeLineNodeDTO
                                    {
                                        LogDateTime = currentRangeMission.EndDateTime,
                                        LogType = LogType.Entrance,
                                        IsAttendance = currentNode.Value.IsAttendance,
                                        IsMission = currentNode.Value.IsMission,
                                        IsLateness = currentNode.Value.IsLateness,
                                        IsNonAttendance = currentNode.Value.IsNonAttendance,
                                        IsVacation = currentNode.Value.IsVacation,
                                        IsRestTime = currentNode.Value.IsRestTime
                                        // في الخطوات السابقة لم نعالج سوا الحضور لذا من المؤكد انه سجل حضور
                                    });
                                    break;
                                }
                                if (currentNode.Value.LogType == LogType.Entrance)
                                {
                                    endAdded = true;
                                    if (!startAdded)
                                    {
                                        startAdded = true;
                                        rangeTimeLine.AddBefore(currentNode, startMissionNode);
                                    }
                                    rangeTimeLine.AddBefore(currentNode, endMissionNode);
                                    break;
                                }
                            }
                            currentNode = currentNode.Next;
                        }
                        if (!startAdded)
                        {
                            rangeTimeLine.AddLast(startMissionNode);
                        }
                        if (!endAdded)
                        {
                            rangeTimeLine.AddLast(endMissionNode);
                        }
                    }
                }

                #endregion

                #region Daily Vacation

                var isEmployeeHasDailyVacation =
                    AttendanceSystemIntegrationService.IsEmployeeHasDailyVacation(employeeAttendanceCard.Employee,
                        recurrence.Date, EmpDayLeave);
                if (isEmployeeHasDailyVacation)
                {
                    // يتم تجاهل جميع التسجيلات بيوم الاجازة واستبدالها بدخول وخروج واحد يكافئ بداية ونهاية الدوام
                    rangeTimeLine.Clear();
                    // أي أنه بإجازة يومية وليس لدينا اي تسجيل دخول أو خروج عندها نضيف له دوام يبدأ وينتهي حسب الفترة المحددة
                    rangeTimeLine.AddFirst(new TimeLineNodeDTO
                    {
                        LogDateTime = range.EntryTime,
                        LogType = LogType.Entrance,
                        IsVacation = true
                    });
                    rangeTimeLine.AddLast(new TimeLineNodeDTO
                    {
                        LogDateTime = range.ExitTime,
                        LogType = LogType.Exit,
                        IsVacation = true
                    });
                }
                #endregion
                #region Hourly Vacations

                else
                {
                    // أختبار حالة الاجازة الساعية في حال عدم وجود الاجازة اليومية
                    var currentRangevacations = AttendanceSystemIntegrationService.GetEmployeeHourlyVacationDetails(employeeAttendanceCard.Employee, range, EmpHourlyLeave);
                    foreach (var currentRangeVacation in currentRangevacations)
                    {
                        currentNode = rangeTimeLine.First;
                        var startVacationNode = new TimeLineNodeDTO
                        {
                            LogDateTime = currentRangeVacation.FromDateTime.GetValueOrDefault(),
                            LogType = LogType.Entrance,
                            IsVacation = true
                        };
                        var endVacationNode = new TimeLineNodeDTO
                        {
                            LogDateTime = currentRangeVacation.ToDateTime.GetValueOrDefault(),
                            LogType = LogType.Exit,
                            IsVacation = true
                        };
                        bool startAdded = false;
                        bool endAdded = false;
                        while (currentNode != null)
                        {
                            if (currentNode.Value.LogDateTime < currentRangeVacation.FromDateTime)
                            {
                                currentNode = currentNode.Next;
                                continue;
                            }
                            if (currentNode.Value.LogDateTime >= currentRangeVacation.FromDateTime && currentNode.Value.LogDateTime <= currentRangeVacation.ToDateTime)
                            {
                                if (currentNode.Value.LogType == LogType.Exit && !startAdded)
                                {
                                    startAdded = true;
                                    if (currentNode.Previous != null && currentNode.Previous.Value.LogDateTime < currentRangeVacation.FromDateTime)
                                    {
                                        rangeTimeLine.AddBefore(currentNode, new TimeLineNodeDTO
                                        {
                                            LogDateTime = currentRangeVacation.FromDateTime.GetValueOrDefault(),
                                            LogType = LogType.Exit,
                                            IsAttendance = currentNode.Value.IsAttendance,
                                            IsMission = currentNode.Value.IsMission,
                                            IsLateness = currentNode.Value.IsLateness,
                                            IsNonAttendance = currentNode.Value.IsNonAttendance,
                                            IsVacation = currentNode.Value.IsVacation,
                                            IsRestTime = currentNode.Value.IsRestTime
                                        });
                                    }
                                    rangeTimeLine.AddBefore(currentNode, startVacationNode);
                                }
                                var swap = currentNode.Next;
                                rangeTimeLine.Remove(currentNode);
                                currentNode = swap;
                                continue;
                            }
                            if (currentNode.Value.LogDateTime > currentRangeVacation.ToDateTime)
                            {
                                if (currentNode.Value.LogType == LogType.Exit)
                                {
                                    endAdded = true;
                                    if (!startAdded)
                                    {
                                        startAdded = true;
                                        rangeTimeLine.AddBefore(currentNode, startVacationNode);
                                    }
                                    rangeTimeLine.AddBefore(currentNode, endVacationNode);

                                    rangeTimeLine.AddBefore(currentNode, new TimeLineNodeDTO
                                    {
                                        LogDateTime = currentRangeVacation.ToDateTime.GetValueOrDefault(),
                                        LogType = LogType.Entrance,
                                        IsAttendance = currentNode.Value.IsAttendance,
                                        IsMission = currentNode.Value.IsMission,
                                        IsLateness = currentNode.Value.IsLateness,
                                        IsNonAttendance = currentNode.Value.IsNonAttendance,
                                        IsVacation = currentNode.Value.IsVacation,
                                        IsRestTime = currentNode.Value.IsRestTime
                                    });
                                    break;
                                }
                                if (currentNode.Value.LogType == LogType.Entrance)
                                {
                                    endAdded = true;
                                    if (!startAdded)
                                    {
                                        startAdded = true;
                                        rangeTimeLine.AddBefore(currentNode, startVacationNode);
                                    }
                                    rangeTimeLine.AddBefore(currentNode, endVacationNode);
                                    break;
                                }
                            }
                            currentNode = currentNode.Next;
                        }
                        if (!startAdded)
                        {
                            rangeTimeLine.AddLast(startVacationNode);
                        }
                        if (!endAdded)
                        {
                            rangeTimeLine.AddLast(endVacationNode);
                        }
                    }
                }

                #endregion

                #region Add Rest Time Ranges

                {
                    var restPeriod = range.RestPeriod;
                    currentNode = rangeTimeLine.First;
                    while (currentNode != null && restPeriod > 0)
                    {
                        if (currentNode.Value.LogDateTime >= range.RestRangeStartTime &&
                            currentNode.Value.LogDateTime <= range.RestRangeEndTime)
                        {
                            if (currentNode.Value.LogType == LogType.Entrance)
                            {
                                var tempStartRestTime = range.RestRangeStartTime.Value;
                                var tempEndRestTime = currentNode.Value.LogDateTime;
                                if (currentNode.Previous != null &&
                                    currentNode.Previous.Value.LogDateTime > range.RestRangeStartTime)
                                {
                                    tempStartRestTime = currentNode.Previous.Value.LogDateTime;
                                }

                                tempEndRestTime = restPeriod > (tempEndRestTime - tempStartRestTime).TotalMinutes
                                    ? tempEndRestTime
                                    : tempStartRestTime.AddMinutes(restPeriod);
                                restPeriod -= (int)(tempEndRestTime - tempStartRestTime).TotalMinutes;
                                if (tempStartRestTime != tempEndRestTime)
                                {
                                    rangeTimeLine.AddBefore(currentNode, new TimeLineNodeDTO
                                    {
                                        LogDateTime = tempStartRestTime,
                                        LogType = LogType.Entrance,
                                        IsRestTime = true
                                    });
                                    rangeTimeLine.AddBefore(currentNode, new TimeLineNodeDTO
                                    {
                                        LogDateTime = tempEndRestTime,
                                        LogType = LogType.Exit,
                                        IsRestTime = true
                                    });
                                }
                            }
                            if (currentNode.Value.LogType == LogType.Exit)
                            {
                                var tempStartRestTime = currentNode.Value.LogDateTime;
                                var tempEndRestTime = range.RestRangeEndTime.Value;
                                if (currentNode.Next != null &&
                                    currentNode.Next.Value.LogDateTime < range.RestRangeEndTime)
                                {
                                    tempEndRestTime = currentNode.Next.Value.LogDateTime;
                                }

                                tempEndRestTime = restPeriod > (tempEndRestTime - tempStartRestTime).TotalMinutes
                                    ? tempEndRestTime
                                    : tempStartRestTime.AddMinutes(restPeriod);
                                restPeriod -= (int)(tempEndRestTime - tempStartRestTime).TotalMinutes;
                                if (tempStartRestTime != tempEndRestTime)
                                {
                                    rangeTimeLine.AddAfter(currentNode, new TimeLineNodeDTO
                                    {
                                        LogDateTime = tempEndRestTime,
                                        LogType = LogType.Exit,
                                        IsRestTime = true
                                    });
                                    rangeTimeLine.AddAfter(currentNode, new TimeLineNodeDTO
                                    {
                                        LogDateTime = tempStartRestTime,
                                        LogType = LogType.Entrance,
                                        IsRestTime = true
                                    });
                                }
                            }
                        }
                        currentNode = currentNode.Next;
                    }
                }

                #endregion

                #region Add Lateness Ranges

                if (rangeTimeLine.Count > 0)
                {
                    if (rangeTimeLine.First.Value.LogDateTime >
                        range.EntryTime.AddMinutes(range.IgnoredPeriodAfterEntryTime))
                    {
                        rangeTimeLine.AddFirst(new TimeLineNodeDTO
                        {
                            LogDateTime = rangeTimeLine.First.Value.LogDateTime,
                            LogType = LogType.Exit,
                            IsLateness = true
                        });
                        rangeTimeLine.AddFirst(new TimeLineNodeDTO
                        {
                            LogDateTime = range.EntryTime,
                            LogType = LogType.Entrance,
                            IsLateness = true
                        });
                    }
                }

                #endregion

                #region Add Non Attendance Ranges

                {
                    if (rangeTimeLine.Count > 0)
                    {
                        if (rangeTimeLine.Last.Value.LogDateTime <
                            range.ExitTime.AddMinutes(-range.IgnoredPeriodBeforeExitTime))
                        {
                            rangeTimeLine.AddLast(new TimeLineNodeDTO
                            {
                                LogDateTime = rangeTimeLine.Last.Value.LogDateTime,
                                LogType = LogType.Entrance,
                                IsNonAttendance = true
                            });
                            rangeTimeLine.AddLast(new TimeLineNodeDTO
                            {
                                LogDateTime = range.ExitTime,
                                LogType = LogType.Exit,
                                IsNonAttendance = true
                            });
                        }

                        currentNode = rangeTimeLine.First;
                        while (currentNode != null && currentNode.Next != null)
                        {
                            if (currentNode.Value.LogType == LogType.Exit &&
                                currentNode.Next.Value.LogDateTime > currentNode.Value.LogDateTime)
                            {
                                // ترتيب الاضافة مهم يجب عدم التبديل
                                rangeTimeLine.AddBefore(currentNode.Next, new TimeLineNodeDTO
                                {
                                    LogDateTime = currentNode.Next.Value.LogDateTime,
                                    LogType = LogType.Exit,
                                    IsNonAttendance = true
                                });
                                rangeTimeLine.AddAfter(currentNode, new TimeLineNodeDTO
                                {
                                    LogDateTime = currentNode.Value.LogDateTime,
                                    LogType = LogType.Entrance,
                                    IsNonAttendance = true
                                });
                            }
                            currentNode = currentNode.Next;
                        }
                    }
                    else
                    {
                        rangeTimeLine.AddFirst(new TimeLineNodeDTO
                        {
                            LogDateTime = range.EntryTime,
                            LogType = LogType.Entrance,
                            IsNonAttendance = true,
                            IsAbsent = true
                        });
                        rangeTimeLine.AddLast(new TimeLineNodeDTO
                        {
                            LogDateTime = range.ExitTime,
                            LogType = LogType.Exit,
                            IsNonAttendance = true,
                            IsAbsent = true
                        });
                    }
                }

                #endregion

                currentNode = rangeTimeLine.First;
                while (currentNode != null)
                {
                    timeLine.AddLast(currentNode.Value);
                    currentNode = currentNode.Next;
                }
            }

            #region Final Results

            {
                currentNode = timeLine.First;
                while (currentNode != null && currentNode.Next != null)
                {
                    var range = "{" + currentNode.Value.LogDateTime.ToString("HH") + ":" +
                                    currentNode.Value.LogDateTime.ToString("mm") + "-" + currentNode.Next.Value.LogDateTime.ToString("HH") +
                                    ":" + currentNode.Next.Value.LogDateTime.ToString("mm") + "} ";
                    //var range = restRange.Replace("[_Rest_]", "");
                    var rangeValue = (currentNode.Next.Value.LogDateTime - currentNode.Value.LogDateTime).TotalHours;

                    if (currentNode.Value.IsLateness)
                    {
                        attendanceWithoutAdjustmentDetail.LatenessHoursValue += rangeValue;
                        attendanceWithoutAdjustmentDetail.LatenessHoursRanges += range;
                    }
                    else if (currentNode.Value.IsMission)
                    {
                        attendanceWithoutAdjustmentDetail.HasMission = true;
                        attendanceWithoutAdjustmentDetail.MissionValue += rangeValue;
                        attendanceWithoutAdjustmentDetail.MissionRanges += range;
                        attendanceWithoutAdjustmentDetail.RequiredWorkHoursValue -= rangeValue;
                    }
                    else if (currentNode.Value.IsNonAttendance && !currentNode.Value.IsAbsent)
                    {
                        attendanceWithoutAdjustmentDetail.NonAttendanceHoursValue += rangeValue;
                        attendanceWithoutAdjustmentDetail.NonAttendanceHoursRanges += range;
                    }
                    else if (currentNode.Value.IsVacation)
                    {
                        attendanceWithoutAdjustmentDetail.HasVacation = true;
                        attendanceWithoutAdjustmentDetail.VacationValue += rangeValue;
                        attendanceWithoutAdjustmentDetail.VacationRanges += range;
                        attendanceWithoutAdjustmentDetail.RequiredWorkHoursValue -= rangeValue;
                    }
                    else if (currentNode.Value.IsRestTime)
                    {
                        attendanceWithoutAdjustmentDetail.RestValue += rangeValue;
                        attendanceWithoutAdjustmentDetail.RestRanges += range;
                    }
                    currentNode = currentNode.Next.Next;
                }


                var requiredNodes = new LinkedList<TimeLineNodeDTO>(timeLine);
                currentNode = requiredNodes.First;
                while (currentNode != null)
                {
                    if (currentNode.Value.IsMission || currentNode.Value.IsVacation)
                    {
                        var swap = currentNode.Next;
                        requiredNodes.Remove(currentNode);
                        currentNode = swap;
                        continue;
                    }
                    currentNode = currentNode.Next;
                }
                currentNode = requiredNodes.First;
                while (currentNode != null)
                {
                    if (currentNode.Next != null && currentNode.Value.LogDateTime == currentNode.Next.Value.LogDateTime)
                    {
                        var swap = currentNode.Next.Next;
                        requiredNodes.Remove(currentNode.Next);
                        requiredNodes.Remove(currentNode);
                        currentNode = swap;
                        continue;
                    }
                    currentNode = currentNode.Next;
                }

                currentNode = requiredNodes.First;
                while (currentNode != null && currentNode.Next != null)
                {
                    var range = "{" + currentNode.Value.LogDateTime.ToString("HH") + ":" + currentNode.Value.LogDateTime.ToString("mm") +
                                "-" + currentNode.Next.Value.LogDateTime.ToString("HH") + ":" +
                                currentNode.Next.Value.LogDateTime.ToString("mm") + "} ";
                    attendanceWithoutAdjustmentDetail.RequiredWorkHoursRanges += range;
                    currentNode = currentNode.Next.Next;
                }
            }

            #endregion
            return timeLine;
        }

        private static void SetActualWorkValuesForWithoutAdjustment(WorkshopRecurrenceDTO recurrence, EmployeeCard employeeAttendanceCard, List<EntranceExitRecord> entranceExitRecords, AttendanceWithoutAdjustmentDetail attendanceWithoutAdjustmentDetail)
        {
            var mergedRecords = entranceExitRecords.Select(z =>
                    new
                    {
                        Record = z,
                        OrderWhenConflict = z.LogType == LogType.Entrance ? 2 : 1
                    }).ToList();
            var finalOrderedResult = mergedRecords.OrderBy(x => x.Record.LogDateTime).ThenBy(x => x.OrderWhenConflict).ToList();
            for (int i = 0; i < finalOrderedResult.Count; i += 2)
            {
                if(finalOrderedResult.Count > i+ 1)
                {
                    var currentValue = (finalOrderedResult[i + 1].Record.LogDateTime - finalOrderedResult[i].Record.LogDateTime).TotalHours;
                    var currentRange = "{" + finalOrderedResult[i].Record.LogDateTime.ToString("HH") + ":" + finalOrderedResult[i].Record.LogDateTime.ToString("mm") + "-" +
                                       finalOrderedResult[i + 1].Record.LogDateTime.ToString("HH") + ":" + finalOrderedResult[i + 1].Record.LogDateTime.ToString("mm") + "} ";
                    attendanceWithoutAdjustmentDetail.ActualWorkValue += currentValue;
                    attendanceWithoutAdjustmentDetail.ActualWorkRanges += currentRange;
                }
            }
        }

        public static void SetOvertimeValuesForWithoutAdjustment(WorkshopRecurrenceDTO recurrence,
            EmployeeCard employeeAttendanceCard, List<EntranceExitRecord> entranceExitRecords,
            AttendanceWithoutAdjustmentDetail attendanceWithoutAdjustmentDetail, GeneralSettings generalSettings, IList<OvertimeOrder> overTimeOrders)
        {
            var timeLine = new LinkedList<OverTimeNodeDTO>();
            LinkedListNode<OverTimeNodeDTO> currentNode;
            if (recurrence.RecurrenceType == WorkshopRecurrenceTypeDTO.Holiday ||
                recurrence.RecurrenceType == WorkshopRecurrenceTypeDTO.Off)
            {
                entranceExitRecords.Select(x => new OverTimeNodeDTO
                {
                    LogDateTime = x.LogDateTime,
                    LogType = x.LogType,
                    IsParticular = false
                })
                    .OrderBy(x => x.LogDateTime)
                    .ForEach(x => timeLine.AddLast(x));

            }
            else
            {
                var preparedWorkshop = recurrence.Workshop.Prepare(recurrence.Date);
                // GetWorkshopNormalShiftsRanges(recurrence.Workshop.NormalShifts.ToList(), recurrence.Date);

                foreach (var range in preparedWorkshop.NormalShifts)
                {
                    var rangeTimeLine = new LinkedList<OverTimeNodeDTO>();
                    //var rangeTimeLine = new LinkedList<TimeLineNodeDTO>();

                    #region Before Entrance

                    entranceExitRecords.Where(y =>
                        y.LogDateTime < range.EntryTime.AddMinutes(-range.IgnoredPeriodBeforeEntryTime) &&
                        y.LogDateTime >= range.ShiftRangeStartTime).Select(x => new OverTimeNodeDTO
                        {
                            LogDateTime = x.LogDateTime,
                            LogType = x.LogType,
                            IsParticular = false
                        })
                        .OrderBy(x => x.LogDateTime)
                        .ForEach(x => rangeTimeLine.AddLast(x));

                    if (rangeTimeLine.Any() && rangeTimeLine.Last.Value.LogType == LogType.Entrance)
                    {
                        rangeTimeLine.AddLast(new OverTimeNodeDTO
                        {
                            //حساب الاضافي في الفترة الممتدة من الحد الادنى للدخول و فترة الدخول و في حال كانت  اخر تسجيلة دخول فيجب تسكير الدخول بخروج في بداية الدوام
                            LogDateTime = range.EntryTime,
                            LogType = LogType.Exit,
                            IsParticular = false
                        });
                    }
                    if (rangeTimeLine.Any() && rangeTimeLine.First.Value.LogType == LogType.Exit)
                    {
                        rangeTimeLine.AddFirst(new OverTimeNodeDTO
                        {
                            //حساب الاضافي في الفترة الممتدة من الحد الادنى للدخول و فترة الدخول و في حال كانت  اول تسجيلة خروج فيجب اضافة الدخول في بداية الفترة لانها تعتبر دخول تم قبل بداية الفترة والذي يتم اهماله
                            LogDateTime = range.ShiftRangeStartTime,
                            LogType = LogType.Entrance,
                            IsParticular = false
                        });
                    }

                    #endregion

                    currentNode = rangeTimeLine.First;
                    while (currentNode != null)
                    {
                        timeLine.AddLast(currentNode.Value);
                        currentNode = currentNode.Next;
                    }
                    #region After Exit
                    rangeTimeLine = new LinkedList<OverTimeNodeDTO>();
                    entranceExitRecords.Where(y =>
                        y.LogDateTime > range.ExitTime.AddMinutes(range.IgnoredPeriodAfterExitTime) &&
                        y.LogDateTime <= range.ShiftRangeEndTime).Select(x => new OverTimeNodeDTO
                        {
                            LogDateTime = x.LogDateTime,
                            LogType = x.LogType,
                            IsParticular = false
                        })
                        .OrderBy(x => x.LogDateTime)
                        .ForEach(x => rangeTimeLine.AddLast(x));

                    if (rangeTimeLine.Any() && rangeTimeLine.Last.Value.LogType == LogType.Entrance)
                    {
                        rangeTimeLine.AddLast(new OverTimeNodeDTO
                        {
                            LogDateTime = range.ShiftRangeEndTime,
                            LogType = LogType.Exit,
                            IsParticular = false
                        });
                    }
                    if (rangeTimeLine.Any() && rangeTimeLine.First.Value.LogType == LogType.Exit)
                    {
                        rangeTimeLine.AddFirst(new OverTimeNodeDTO
                        {
                            LogDateTime = range.ExitTime,
                            LogType = LogType.Entrance,
                            IsParticular = false
                        });
                    }

                    #endregion


                    currentNode = rangeTimeLine.First;
                    while (currentNode != null)
                    {
                        timeLine.AddLast(currentNode.Value);
                        currentNode = currentNode.Next;
                    }
                }

                #region  particular Overtime Shift Ranges اضافي الفترات الخاصة

                foreach (var range in preparedWorkshop.ParticularOvertimeShifts)
                {
                    currentNode = timeLine.First;
                    var startParticularOvertimeNode = new OverTimeNodeDTO
                    {
                        LogDateTime = range.StartTime,
                        LogType = LogType.Entrance,
                        IsParticular = true
                    };
                    var endParticularOvertimeNode = new OverTimeNodeDTO
                    {
                        LogDateTime = range.EndTime,
                        LogType = LogType.Exit,
                        IsParticular = true
                    };
                    //bool startAdded = false;
                    //bool endAdded = false;
                    while (currentNode != null)
                    {
                        if (currentNode.Value.LogDateTime < range.StartTime)
                        {
                            currentNode = currentNode.Next;
                            continue;
                        }
                        if (currentNode.Value.LogDateTime >= range.StartTime &&
                            currentNode.Value.LogDateTime <= range.EndTime)
                        {
                            if (currentNode.Value.LogType == LogType.Exit && currentNode.Previous.Value.LogDateTime < range.StartTime)
                            {
                                //startAdded = true;
                                timeLine.AddBefore(currentNode, new OverTimeNodeDTO
                                {
                                    LogDateTime = range.StartTime,
                                    LogType = LogType.Exit,
                                    IsParticular = currentNode.Value.IsParticular
                                });
                                timeLine.AddBefore(currentNode, startParticularOvertimeNode);
                            }
                            // var swap = currentNode.Next;
                            // timeLine.Remove(currentNode);
                            currentNode.Value.IsParticular = true;
                            currentNode = currentNode.Next;
                            continue;
                        }
                        if (currentNode.Value.LogDateTime > range.EndTime)
                        {
                            if (currentNode.Value.LogType == LogType.Exit && currentNode.Previous.Value.LogDateTime < range.EndTime)
                            {
                                timeLine.AddBefore(currentNode, endParticularOvertimeNode);
                                //startAdded = true;
                                timeLine.AddBefore(currentNode, new OverTimeNodeDTO
                                {
                                    LogDateTime = range.EndTime,
                                    LogType = LogType.Entrance,
                                    IsParticular = currentNode.Value.IsParticular
                                });
                                //timeLine.AddBefore(currentNode, startParticularOvertimeNode);
                            }
                        }
                        currentNode = currentNode.Next;
                    }
                }

                #endregion
            }


            #region final Result
            OvertimeForm generalSettingsOvertimeForm = generalSettings != null ? generalSettings.OvertimeForm : null;
            var overtimeOrderValue = 0.0;
            var overtimeForm = GetOvertimeForm(employeeAttendanceCard.Employee, generalSettingsOvertimeForm);
            if (overtimeForm.NeedOverTimeAcceptance)
            {
                overtimeOrderValue = GetEmployeeOvertimeOrderValue(employeeAttendanceCard.Employee, recurrence.Date, overTimeOrders);
            }
            currentNode = timeLine.First;
            while (currentNode != null && currentNode.Next != null)
            {
                var range = "{" + currentNode.Value.LogDateTime.ToString("HH") + ":" + currentNode.Value.LogDateTime.ToString("mm") + "-" + currentNode.Next.Value.LogDateTime.ToString("HH") + ":" + currentNode.Next.Value.LogDateTime.ToString("mm") + "} ";
                var rangeValue = (currentNode.Next.Value.LogDateTime - currentNode.Value.LogDateTime).TotalHours;
                attendanceWithoutAdjustmentDetail.ExpectedOvertimeValue += rangeValue;
                attendanceWithoutAdjustmentDetail.ExpectedOvertimeRanges += range;

                if (overtimeForm.NeedOverTimeAcceptance)
                {
                    if (overtimeOrderValue > 0)
                    {
                        if (overtimeOrderValue < rangeValue)
                        {
                            rangeValue = overtimeOrderValue;
                            range = "{" + currentNode.Value.LogDateTime.ToString("HH") + ":" + currentNode.Value.LogDateTime.ToString("mm") + "-" +
                                currentNode.Value.LogDateTime.AddHours(rangeValue).ToString("HH") + ":" + currentNode.Value.LogDateTime.AddHours(rangeValue).ToString("mm") + "} ";
                            overtimeOrderValue = 0;
                        }
                        else
                        {
                            overtimeOrderValue -= rangeValue;
                        }
                        attendanceWithoutAdjustmentDetail.OvertimeOrderValue += rangeValue;
                        attendanceWithoutAdjustmentDetail.OvertimeOrderRanges += range;
                    }
                    else
                    {
                        rangeValue = 0;
                    }
                }

                if (rangeValue > 0)
                {
                    if (recurrence.RecurrenceType == WorkshopRecurrenceTypeDTO.Work)
                    {
                        if (currentNode.Value.IsParticular) // if true so it is particular overtime
                        {
                            attendanceWithoutAdjustmentDetail.ParticularOvertimeValue += rangeValue;
                        }
                        else
                        {
                            attendanceWithoutAdjustmentDetail.NormalOvertimeValue += rangeValue;
                        }
                    }
                    else
                    {
                        attendanceWithoutAdjustmentDetail.HolidayOvertimeValue += rangeValue;
                    }
                }
                currentNode = currentNode.Next.Next;
            }

            #endregion

        }

        public static
        void LockAttendanceRecord(AttendanceRecord attendanceRecord)
        {
            attendanceRecord.AttendanceMonthStatus = AttendanceMonthStatus.Locked;
        }

        public static double GetEmployeeOvertimeOrderValue(Employee employee, DateTime date, IList<OvertimeOrder> OverTimeOrders)
        {
            if (!IsEmployeeHasOvertimeOrder(employee, date, OverTimeOrders)) return 0;

            var result = OverTimeOrders
                .First(x => date >= x.FromDate && date <= x.ToDate && x.Employee.Id == employee.Id).OvertimeHoursPerDay;
            return result;
        }

        public static bool IsEmployeeHasOvertimeOrder(Employee employee, DateTime date, IList<OvertimeOrder> OverTimeOrders)
        {
            var result =
                OverTimeOrders
                .Any(x => date >= x.FromDate && date <= x.ToDate && x.Employee.Id == employee.Id);

            return result;
        }

        public static double GetActualWorkHoursValue(List<EntranceExitRecord> entranceExitRecords)
        {
            var result = 0.0;
            var allEntrance = Enumerable.ToArray(entranceExitRecords.Where(x => x.LogType == LogType.Entrance).OrderBy(x => x.LogDateTime));
            var allExit = Enumerable.ToArray(entranceExitRecords.Where(x => x.LogType == LogType.Exit).OrderBy(x => x.LogDateTime));

            for (var i = 0; i < allEntrance.Count(); i++)
            {
                if (allExit[i].LogDateTime < allEntrance[i].LogDateTime)
                    result += 24 + (allExit[i].LogDateTime - allEntrance[i].LogDateTime).TotalHours;
                else
                    result += (allExit[i].LogDateTime - allEntrance[i].LogDateTime).TotalHours;
            }
            return result;
        }

        public static List<EntranceExitRecord> GetEntranceExitRecords(WorkshopRecurrenceDTO workshopRecurrenceDTO, Employee employee)
        {

            List<EntranceExitRecord> result;
            var ranges = workshopRecurrenceDTO.Workshop.Prepare(workshopRecurrenceDTO.Date).NormalShifts;// GetWorkshopNormalShiftsRanges(workshopRecurrenceDTO.Workshop.NormalShifts.ToList(), workshopRecurrenceDTO.Date);

            //todo Mhd Alsadi: تم التحويل الى ليست بسبب مشكلة هايبرنيت مع التواريخ سيتم تنفيذ الحل المقترح
            result = typeof(EntranceExitRecord).GetAll<EntranceExitRecord>().ToList()
                .Where(x => ranges.Any(y => x.LogDateTime >= y.ShiftRangeStartTime && x.LogDateTime <= y.ShiftRangeEndTime) && x.Employee.Id == employee.Id).ToList();
            return result;
        }
        #region Non Attendance, Lateness (Penalty and Value)

        private static AttendanceInfraction GetAttendanceInfraction(double value, AttendanceRecord record, EmployeeCard card, GeneralSettings generalSettings, List<AttendanceInfraction> infractions, DateTime dateOfDay, bool isNonAttendance, List<AttendanceInfraction> attendanceInfractions, out string info)
        {
            NonAttendanceForm generalSettingsAbsenceForm = null;
            NonAttendanceForm absenceForm = null;
            InfractionForm infractionForm = null;
            AttendanceInfraction attendanceInfraction = null;
            DisciplinarySetting penalty = null;
            int repeationNumber = 0;
            string infoOfInfraction = "";
            if (value == 0)
            {
                info = infoOfInfraction;
                return attendanceInfraction;
            }
            if (isNonAttendance)
            {
                generalSettingsAbsenceForm = generalSettings != null ? generalSettings.AbsenceForm : null;
                var nonAttendanceForm = GetAbsenceForm(card.Employee, generalSettingsAbsenceForm);
                absenceForm = nonAttendanceForm != null ?
                nonAttendanceForm :
                generalSettingsAbsenceForm;
            }
            else
            {
                generalSettingsAbsenceForm = generalSettings != null ? generalSettings.LatenessForm : null;
                var latenessForm = GetLatenessForm(card.Employee, generalSettingsAbsenceForm);
                absenceForm = latenessForm != null ?
                latenessForm :
                generalSettingsAbsenceForm;
            }



            value = value * 60;
            if (absenceForm != null)
            {
                infractionForm = absenceForm.InfractionForm;
                if (absenceForm.NonAttendanceSlices.Any())
                {
                    var nonAttendanceSlice =
                            absenceForm.NonAttendanceSlices.FirstOrDefault(x => value >= x.StartSlice && value <= x.EndSlice)
                            ??
                            absenceForm.NonAttendanceSlices.OrderBy(x => x.EndSlice).Last();
                    infractionForm = nonAttendanceSlice.InfractionForm;
                }
                if (infractionForm != null)
                {
                    infoOfInfraction = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.AccordingToTheInfractionForm) + " " + infractionForm.Number.ToString() + " " +
                            AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.WithNonAttendanceValue) + " " + value.ToString() + " " + AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.Minutes) + " ";
                    var infractionsByTypeWithLastResetIsLessThanAbsenceFormLastReset = infractions.Where(x => x.Infraction.Number == infractionForm.Number && x.PenaltyDate <= absenceForm.LastReset).ToList();
                    foreach (var item in infractionsByTypeWithLastResetIsLessThanAbsenceFormLastReset.Where(x => attendanceInfractions.Any(y => y.Id != x.Id)))
                    {
                        item.IsActiveForNextPenalties = false;
                        attendanceInfractions.Add(item);
                    }
                    var lastInfractionByType = infractions.Any(x => x.Infraction.Number == infractionForm.Number)
                        ? infractions.Where(x => x.Infraction.Number == infractionForm.Number).OrderByDescending(x => x.RepeationNumber).FirstOrDefault()
                        : null;
                    repeationNumber = lastInfractionByType != null ? lastInfractionByType.RepeationNumber + 1 : repeationNumber + 1;
                    infoOfInfraction += repeationNumber > 0 ? AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.And)
                        + " " + repeationNumber.ToString() + " " + AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.Recurrences) : "";
                    var infractionFormSlice =
                            infractionForm.InfractionSlices.FirstOrDefault(x => repeationNumber >= x.MinimumRecurrence && repeationNumber <= x.MaximumRecurrence)
                            ??
                            infractionForm.InfractionSlices.OrderBy(x => x.MaximumRecurrence).Last();
                    penalty = infractionFormSlice.Penalty;
                    attendanceInfraction = new AttendanceInfraction()
                    {
                        AttendanceRecord = record,
                        Infraction = infractionForm,
                        IsActiveForNextPenalties = true,
                        Penalty = penalty,
                        EmployeeCard = card,
                        PenaltyDate = dateOfDay,
                        RepeationNumber = repeationNumber
                    };
                }
            }
            info = infoOfInfraction;
            return attendanceInfraction;
        }

        private static DisciplinarySetting GetNonAttendancePenalty(double initialNonAttendanceValue, EmployeeCard employeeAttendanceCard, GeneralSettings generalSettings)
        {
            AttendanceForm generalSettingsAttendanceForm = generalSettings != null ? generalSettings.AttendanceForm : null;
            NonAttendanceForm generalSettingsAbsenceForm = generalSettings != null ? generalSettings.AbsenceForm : null;
            if (generalSettingsAttendanceForm != null && generalSettingsAbsenceForm != null)
            {
                initialNonAttendanceValue = initialNonAttendanceValue * 60;
                //لا داعي لاختبار انه بدون تقاص لان هذه الميثود لا تستخدم الا بحالة دون تقاص
                if (initialNonAttendanceValue > 0)
                {
                    var calculationMethod = GetAttendanceForm(employeeAttendanceCard.Employee, generalSettingsAttendanceForm).CalculationMethod;
                    DisciplinarySetting penalty = null;
                    var absenceForm = GetAbsenceForm(employeeAttendanceCard.Employee, generalSettingsAbsenceForm);
                    int absenceCount = 0;
                    InfractionForm infractionForm = null;
                    if (absenceForm.InfractionForm != null)
                    {
                        if (calculationMethod == CalculationMethod.DailyAdjustment)
                        {
                            absenceCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                                .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= absenceForm.LastReset)
                                .SelectMany(x => x.AttendanceDailyAdjustments)
                                .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                                .Count(x => x.InitialNonAttendanceValue > 0);
                        }
                        else if (calculationMethod == CalculationMethod.MonthlyAdjustment)
                        {
                            absenceCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                                .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= absenceForm.LastReset)
                                .SelectMany(x => x.AttendanceMonthlyAdjustments)
                                .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                                .Count(x => x.InitialNonAttendanceValue > 0);
                        }
                        else if (calculationMethod == CalculationMethod.WithoutAdjustment)
                        {
                            absenceCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                                .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= absenceForm.LastReset)
                                .SelectMany(x => x.AttendanceWithoutAdjustments)
                                .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                                .Count(x => x.InitialNonAttendanceTotalValue > 0);
                        }

                        infractionForm = absenceForm.InfractionForm;
                    }
                    else
                    {
                        if (absenceForm.NonAttendanceSlices.Any())
                        {
                            var nonAttendanceSlice =
                                absenceForm.NonAttendanceSlices.FirstOrDefault(x => initialNonAttendanceValue >= x.StartSlice && initialNonAttendanceValue <= x.EndSlice)
                                ??
                                absenceForm.NonAttendanceSlices.OrderBy(x => x.EndSlice).Last();

                            if (calculationMethod == CalculationMethod.DailyAdjustment)
                            {
                                absenceCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                                    .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= absenceForm.LastReset)
                                    .SelectMany(x => x.AttendanceDailyAdjustments)
                                    .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                                    .Count(x => x.InitialNonAttendanceValue * 60 >= nonAttendanceSlice.StartSlice && x.InitialNonAttendanceValue * 60 <= nonAttendanceSlice.EndSlice);
                            }
                            else if (calculationMethod == CalculationMethod.MonthlyAdjustment)
                            {
                                absenceCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                                    .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= absenceForm.LastReset)
                                    .SelectMany(x => x.AttendanceMonthlyAdjustments)
                                    .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                                    .Count(x => x.InitialNonAttendanceValue * 60 >= nonAttendanceSlice.StartSlice && x.InitialNonAttendanceValue * 60 <= nonAttendanceSlice.EndSlice);
                            }
                            else if (calculationMethod == CalculationMethod.WithoutAdjustment)
                            {
                                absenceCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                                    .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= absenceForm.LastReset)
                                    .SelectMany(x => x.AttendanceWithoutAdjustments)
                                    .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                                    .Count(x => x.InitialNonAttendanceTotalValue * 60 >= nonAttendanceSlice.StartSlice && x.InitialNonAttendanceTotalValue * 60 <= nonAttendanceSlice.EndSlice);
                            }
                            infractionForm = nonAttendanceSlice.InfractionForm;
                        }
                    }
                    if (infractionForm != null && infractionForm.InfractionSlices.Any())
                    {
                        absenceCount++;
                        // الشريحة المناسبة في نموذج المخالفة
                        var infractionFormSlice =
                            infractionForm.InfractionSlices.FirstOrDefault(x => absenceCount >= x.MinimumRecurrence && absenceCount <= x.MaximumRecurrence)
                            ??
                            infractionForm.InfractionSlices.OrderBy(x => x.MaximumRecurrence).Last();
                        penalty = infractionFormSlice.Penalty;
                    }
                    return penalty;
                }
            }
            return null;
        }

        private static double GetNonAttendanceFinalValue(double initialNonAttendanceValue, EmployeeCard employeeAttendanceCard, GeneralSettings generalSettings)
        {
            AttendanceForm generalSettingsAttendanceForm = generalSettings != null ? generalSettings.AttendanceForm : null;
            NonAttendanceForm generalSettingsAbsenceForm = generalSettings != null ? generalSettings.AbsenceForm : null;
            if (generalSettingsAttendanceForm != null && generalSettingsAbsenceForm != null)
            {
                initialNonAttendanceValue = initialNonAttendanceValue * 60;
                var absenceForm = GetAbsenceForm(employeeAttendanceCard.Employee, generalSettingsAbsenceForm);
                if (initialNonAttendanceValue > 0 && absenceForm.NonAttendanceSlices.Any())
                {
                    var nonAttendanceSlice =
                        absenceForm.NonAttendanceSlices.FirstOrDefault(x => initialNonAttendanceValue >= x.StartSlice && initialNonAttendanceValue <= x.EndSlice) ??
                        absenceForm.NonAttendanceSlices.OrderBy(x => x.StartSlice).Last();
                    var absenceCount = 0;
                    var calculationMethod = GetAttendanceForm(employeeAttendanceCard.Employee, generalSettingsAttendanceForm).CalculationMethod;


                    if (calculationMethod == CalculationMethod.DailyAdjustment)
                    {
                        absenceCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                            .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= absenceForm.LastReset)
                            .SelectMany(x => x.AttendanceDailyAdjustments)
                            .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                            .Count(x => x.InitialNonAttendanceValue * 60 >= nonAttendanceSlice.StartSlice && x.InitialNonAttendanceValue * 60 <= nonAttendanceSlice.EndSlice);
                    }
                    else if (calculationMethod == CalculationMethod.MonthlyAdjustment)
                    {
                        absenceCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                            .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= absenceForm.LastReset)
                            .SelectMany(x => x.AttendanceMonthlyAdjustments)
                            .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                            .Count(x => x.InitialNonAttendanceValue * 60 >= nonAttendanceSlice.StartSlice && x.InitialNonAttendanceValue * 60 <= nonAttendanceSlice.EndSlice);
                    }
                    else if (calculationMethod == CalculationMethod.WithoutAdjustment)
                    {
                        absenceCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                            .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= absenceForm.LastReset)
                            .SelectMany(x => x.AttendanceWithoutAdjustments)
                            .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                            .Count(x => x.InitialNonAttendanceTotalValue * 60 >= nonAttendanceSlice.StartSlice && x.InitialNonAttendanceTotalValue * 60 <= nonAttendanceSlice.EndSlice);
                    }

                    if (nonAttendanceSlice.NonAttendanceSlicePercentages.Any())
                    {
                        absenceCount++;
                        var percentagesSlice =
                        nonAttendanceSlice.NonAttendanceSlicePercentages.FirstOrDefault(x => x.PercentageOrder == absenceCount) ??
                        nonAttendanceSlice.NonAttendanceSlicePercentages.OrderBy(x => x.PercentageOrder).Last();

                        if (nonAttendanceSlice.Value != 0)
                        {
                            return nonAttendanceSlice.Value * percentagesSlice.Percentage / 100 / 60;
                        }
                        return initialNonAttendanceValue * percentagesSlice.Percentage / 100 / 60;
                    }
                }
            }
            return 0;
        }

        private static DisciplinarySetting GetLatenessPenalty(double initialLatenessValue, EmployeeCard employeeAttendanceCard, GeneralSettings generalSettings)
        {
            DisciplinarySetting penalty = null;
            NonAttendanceForm generalSettingsLatenessForm = generalSettings != null ? generalSettings.LatenessForm : null;
            if (generalSettingsLatenessForm != null)
            {
                initialLatenessValue = initialLatenessValue * 60;
                //لا داعي لاختبار انه بدون تقاص لان هذه الميثود لا تستخدم الا بحالة دون تقاص
                if (initialLatenessValue == 0)
                {
                    return null;
                }
                var latenessForm = GetLatenessForm(employeeAttendanceCard.Employee, generalSettingsLatenessForm);
                int latenessCount = 0;
                InfractionForm infractionForm = null;
                if (latenessForm.InfractionForm != null)
                {
                    latenessCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                        .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= latenessForm.LastReset)
                        .SelectMany(x => x.AttendanceWithoutAdjustments)
                        .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                        .Count(x => x.InitialNonAttendanceTotalValue > 0);
                    infractionForm = latenessForm.InfractionForm;
                }
                else
                {
                    if (latenessForm.NonAttendanceSlices.Any())
                    {
                        var nonAttendanceSlice =
                            latenessForm.NonAttendanceSlices.FirstOrDefault(x => initialLatenessValue >= x.StartSlice && initialLatenessValue <= x.EndSlice)
                            ??
                            latenessForm.NonAttendanceSlices.OrderBy(x => x.EndSlice).Last();

                        latenessCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                            .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= latenessForm.LastReset)
                            .SelectMany(x => x.AttendanceWithoutAdjustments)
                            .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                            .Count(x => x.InitialNonAttendanceTotalValue * 60 >= nonAttendanceSlice.StartSlice && x.InitialNonAttendanceTotalValue * 60 <= nonAttendanceSlice.EndSlice);
                        infractionForm = nonAttendanceSlice.InfractionForm;
                    }
                }
                if (infractionForm != null && infractionForm.InfractionSlices.Any())
                {
                    latenessCount++;
                    // الشريحة المناسبة في نموذج المخالفة
                    var infractionFormSlice =
                        infractionForm.InfractionSlices.FirstOrDefault(x => latenessCount >= x.MinimumRecurrence && latenessCount <= x.MaximumRecurrence)
                        ??
                        infractionForm.InfractionSlices.OrderBy(x => x.MaximumRecurrence).Last();
                    penalty = infractionFormSlice.Penalty;
                }
            }
            return penalty;
        }

        private static double GetLatenessFinalValue(double initialLatenessValue, EmployeeCard employeeAttendanceCard, GeneralSettings generalSettings)
        {
            NonAttendanceForm generalSettingsLatenessForm = generalSettings != null ? generalSettings.LatenessForm : null;
            if (generalSettingsLatenessForm != null)
            {
                initialLatenessValue = initialLatenessValue * 60;
                var latenessForm = GetLatenessForm(employeeAttendanceCard.Employee, generalSettingsLatenessForm);
                if (initialLatenessValue > 0 && latenessForm.NonAttendanceSlices.Any())
                {
                    var nonAttendanceSlice =
                        latenessForm.NonAttendanceSlices.FirstOrDefault(x => initialLatenessValue >= x.StartSlice && initialLatenessValue <= x.EndSlice) ??
                        latenessForm.NonAttendanceSlices.OrderBy(x => x.StartSlice).Last();

                    var latenessCount = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                        .Where(x => x.AttendanceMonthStatus == AttendanceMonthStatus.Locked && x.FromDate >= latenessForm.LastReset)
                        .SelectMany(x => x.AttendanceWithoutAdjustments)
                        .Where(x => x.EmployeeAttendanceCard.Id == employeeAttendanceCard.Id).ToList()
                        .Count(x => x.InitialLatenessTotalValue * 60 >= nonAttendanceSlice.StartSlice && x.InitialLatenessTotalValue * 60 <= nonAttendanceSlice.EndSlice);

                    latenessCount++;
                    if (nonAttendanceSlice.NonAttendanceSlicePercentages.Any())
                    {
                        var percentagesSlice =
                        nonAttendanceSlice.NonAttendanceSlicePercentages.FirstOrDefault(x => x.PercentageOrder == latenessCount) ??
                        nonAttendanceSlice.NonAttendanceSlicePercentages.OrderBy(x => x.PercentageOrder).Last();

                        if (nonAttendanceSlice.Value != 0)
                        {
                            return nonAttendanceSlice.Value * percentagesSlice.Percentage / 100 / 60;
                        }
                        return initialLatenessValue * percentagesSlice.Percentage / 100 / 60;
                    }
                }
            }
            return 0;
        }

        #endregion
        private static double GetNormalOverTimeFinalValue(double initialOvertimeValue, EmployeeCard employeeAttendanceCard, GeneralSettings generalSettings)
        {
            OvertimeForm generalSettingsOvertimeForm = generalSettings != null ? generalSettings.OvertimeForm : null;
            if (generalSettingsOvertimeForm != null)
            {
                initialOvertimeValue = initialOvertimeValue * 60;
                var overtimeForm = GetOvertimeForm(employeeAttendanceCard.Employee, generalSettingsOvertimeForm);
                if (overtimeForm.OvertimeSlices == null || overtimeForm.OvertimeSlices.Count == 0 || initialOvertimeValue <= 0)
                {
                    return initialOvertimeValue / 60;
                }
                var slice =
                    overtimeForm.OvertimeSlices.FirstOrDefault(x => initialOvertimeValue >= x.StartSlice && initialOvertimeValue <= x.EndSlice) ??
                    overtimeForm.OvertimeSlices.OrderBy(x => x.StartSlice).Last();

                if (slice.NormalValue != 0)
                {
                    return slice.NormalValue * slice.NormalPercentage / 100.0 / 60.0;
                }
                return initialOvertimeValue * slice.NormalPercentage / 100 / 60;
            }
            return 0;
        }

        private static double GetHolidayOverTimeFinalValue(double initialOvertimeValue, EmployeeCard employeeAttendanceCard, GeneralSettings generalSettings)
        {
            OvertimeForm generalSettingsOvertimeForm = generalSettings != null ? generalSettings.OvertimeForm : null;
            if (generalSettingsOvertimeForm != null)
            {
                initialOvertimeValue = initialOvertimeValue * 60;
                var overtimeForm = GetOvertimeForm(employeeAttendanceCard.Employee, generalSettingsOvertimeForm);
                if (overtimeForm.OvertimeSlices == null || overtimeForm.OvertimeSlices.Count == 0 || initialOvertimeValue <= 0)
                {
                    return initialOvertimeValue / 60;
                }
                var slice =
                    overtimeForm.OvertimeSlices.FirstOrDefault(x => initialOvertimeValue >= x.StartSlice && initialOvertimeValue <= x.EndSlice) ??
                    overtimeForm.OvertimeSlices.OrderBy(x => x.StartSlice).Last();

                if (slice.HolidayValue != 0)
                {
                    return slice.HolidayValue * slice.HolidayPercentage / 100.0 / 60.0;
                }
                return initialOvertimeValue * slice.HolidayPercentage / 100 / 60;
            }
            return 0;
        }

        private static double GetParticularOverTimeFinalValue(double initialOvertimeValue, EmployeeCard employeeAttendanceCard, GeneralSettings generalSettings)
        {
            OvertimeForm generalSettingsOvertimeForm = generalSettings != null ? generalSettings.OvertimeForm : null;
            if (generalSettingsOvertimeForm != null)
            {
                initialOvertimeValue = initialOvertimeValue * 60;
                var overtimeForm = GetOvertimeForm(employeeAttendanceCard.Employee, generalSettingsOvertimeForm);
                if (overtimeForm.OvertimeSlices == null || overtimeForm.OvertimeSlices.Count == 0 || initialOvertimeValue <= 0)
                {
                    return initialOvertimeValue / 60;
                }
                var slice =
                    overtimeForm.OvertimeSlices.FirstOrDefault(x => initialOvertimeValue >= x.StartSlice && initialOvertimeValue <= x.EndSlice) ??
                    overtimeForm.OvertimeSlices.OrderBy(x => x.StartSlice).Last();

                if (slice.ParticularShiftValue != 0)
                {
                    return slice.ParticularShiftValue * slice.ParticularShiftPercentage / 100.0 / 60.0;
                }
                return initialOvertimeValue * slice.ParticularShiftPercentage / 100 / 60;
            }
            return 0;
        }

        #endregion

        #region Attendance Record Generation

        public static void GenerateAttendanceRecords(IEnumerable<EmployeeCard> employeeAttendanceCards, DateTime fromDate, DateTime toDate, string note)
        {
            IList<Employee> employees = new List<Employee>();
            var start = DateTime.Now;
            foreach (var emp in employeeAttendanceCards)
            {
                employees.Add(emp.Employee);
            }
            IList<HourlyMission> EmpHourlyMission = ServiceFactory.ORMService.All<HourlyMission>().Where(x => employees.Contains(x.Employee) && x.Status == HRIS.Domain.Global.Enums.Status.Approved && x.Date >= fromDate && x.Date <= toDate).ToList();
            IList<LeaveRequest> EmpDayLeave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => employees.Contains(x.EmployeeCard.Employee) && x.IsHourlyLeave == false && x.StartDate >= fromDate && x.EndDate <= toDate).ToList();
            IList<TravelMission> EmpTravelMission = ServiceFactory.ORMService.All<TravelMission>().Where(x => employees.Contains(x.Employee) && x.Status == HRIS.Domain.Global.Enums.Status.Approved && x.FromDate >= fromDate && x.ToDate <= toDate).ToList();
            IList<LeaveRequest> EmpHourlyLeave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => employees.Contains(x.EmployeeCard.Employee) && x.IsHourlyLeave == true && x.StartDate >= fromDate && x.EndDate <= toDate).ToList();
            var workshopsRecurrences = GetWorkshopsRecurrenceInPeriod(employeeAttendanceCards.ToList(), fromDate, toDate);
            var entities = new List<IAggregateRoot>();
            var allFingerprintTransferredData = ServiceFactory.ORMService.All<FingerprintTransferredData>();
            var allEntranceExitRecordData = ServiceFactory.ORMService.All<EntranceExitRecord>();
            foreach (var employeeAttendanceCard in employeeAttendanceCards)
            {
                var allFingerprintTransferredDataOfEmployee = allFingerprintTransferredData.Where(x => x.Employee.Id == employeeAttendanceCard.Employee.Id).ToList();
                var allEntranceExitRecordDataOfEmployee = allEntranceExitRecordData.Where(x => x.Employee.Id == employeeAttendanceCard.Employee.Id).ToList();
                var cardEntities = new List<IAggregateRoot>();

                var workshopRecurrences = workshopsRecurrences[employeeAttendanceCard.Employee];
                foreach (var recurrence in workshopRecurrences.Where(x => x.RecurrenceType == WorkshopRecurrenceTypeDTO.Work))
                {
                    var workshopNormalShiftsRanges = recurrence.Workshop.NormalShifts.OrderBy(x => x.NormalShiftOrder).ToList();//GetWorkshopNormalShiftsRanges(recurrence.Workshop.NormalShifts.ToList(), recurrence.Date);


                    foreach (var shift in workshopNormalShiftsRanges)
                    {
                        if (!employeeAttendanceCard.AttendanceDemand ||
                            AttendanceSystemIntegrationService.IsEmployeeHasDailyMission(employeeAttendanceCard.Employee, recurrence.Date, EmpTravelMission) ||
                            AttendanceSystemIntegrationService.IsEmployeeHasDailyVacation(employeeAttendanceCard.Employee, recurrence.Date, EmpDayLeave))
                        {
                            break; // todo Mhd Alsadi: معالجة حالة الشيك ان الفترة تتبع لليوم التالي او السابق 
                            //todo Mhd Alsadi: بحيث اذا اليوم السابق فيه عطلة والفترة تتبع لليوم التالي يتم توليد الفترة الثانية التابعة لليوم التالي ولا نولد الفترة التابعة لليوم الاول
                        }

                        var hourlyMissions = AttendanceSystemIntegrationService.GetEmployeeHourlyMissionDetails(employeeAttendanceCard.Employee, recurrence.Workshop.NormalShifts, EmpHourlyMission)
                            .Where(x => x.StartDateTime >= shift.EntryTime && x.EndDateTime <= shift.ExitTime).ToList();
                        var hourlyvacations = AttendanceSystemIntegrationService.GetEmployeeHourlyVacationDetails(employeeAttendanceCard.Employee, shift, EmpHourlyLeave);
                        var missionVacationRanges = hourlyMissions.Select(x => new { Start = x.StartDateTime, End = x.EndDateTime }).ToList();
                        missionVacationRanges.AddRange(hourlyvacations.Select(x => new { Start = x.FromDateTime.GetValueOrDefault(), End = x.ToDateTime.GetValueOrDefault() }).ToList());
                        missionVacationRanges = missionVacationRanges.OrderBy(x => x.Start).ToList();

                        var entranceRecord = new EntranceExitRecord
                        {
                            Employee = employeeAttendanceCard.Employee,
                            InsertSource = InsertSource.AutoGenerate,
                            LogDateTime = shift.EntryTime,
                            LogTime = new DateTime(2000, 1, 1, shift.EntryTime.Hour, shift.EntryTime.Minute, shift.EntryTime.Second),
                            LogDate = new DateTime(shift.EntryTime.Year, shift.EntryTime.Month, shift.EntryTime.Day, 0, 0, 0),
                            LogType = LogType.Entrance,
                            ErrorType = ErrorType.None,
                            Note = note
                        };
                        if (!CheckEntranceExitRecordDuplicate(allEntranceExitRecordDataOfEmployee,
                            allFingerprintTransferredDataOfEmployee, shift.EntryTime, InsertSource.AutoGenerate,
                            LogType.Entrance, 0))
                        {
                            if (missionVacationRanges.Any())
                            {
                                if (missionVacationRanges.First().Start != entranceRecord.LogDateTime)
                                {
                                    cardEntities.Add(entranceRecord);
                                }
                            }
                            else
                            {
                                cardEntities.Add(entranceRecord);
                            }
                        }

                        var exitRecord = new EntranceExitRecord
                        {
                            Employee = employeeAttendanceCard.Employee,
                            InsertSource = InsertSource.AutoGenerate,
                            LogDateTime = shift.ExitTime,
                            LogTime = new DateTime(2000, 1, 1, shift.ExitTime.Hour, shift.ExitTime.Minute, shift.ExitTime.Second),
                            LogDate = new DateTime(shift.ExitTime.Year, shift.ExitTime.Month, shift.ExitTime.Day, 0, 0, 0),
                            LogType = LogType.Exit,
                            ErrorType = ErrorType.None,
                            Note = note
                        };
                        if (!CheckEntranceExitRecordDuplicate(allEntranceExitRecordDataOfEmployee,
                           allFingerprintTransferredDataOfEmployee, shift.ExitTime, InsertSource.AutoGenerate, LogType.Exit, 0))
                        {
                            if (missionVacationRanges.Any())
                            {
                                if (missionVacationRanges.Last().End != exitRecord.LogDateTime)
                                {
                                    cardEntities.Add(exitRecord);
                                }
                            }
                            else
                            {
                                cardEntities.Add(exitRecord);
                            }
                        }


                        if (missionVacationRanges.Any())
                        {
                            for (int i = 0; i < missionVacationRanges.Count; i++)
                            {
                                if (missionVacationRanges[i].Start != entranceRecord.LogDateTime)
                                {
                                    var exitRecord1 = new EntranceExitRecord
                                    {
                                        Employee = employeeAttendanceCard.Employee,
                                        InsertSource = InsertSource.AutoGenerate,
                                        LogDateTime = missionVacationRanges[i].Start,
                                        LogTime = new DateTime(2000, 1, 1, missionVacationRanges[i].Start.Hour, missionVacationRanges[i].Start.Minute, missionVacationRanges[i].Start.Second),
                                        LogDate = new DateTime(missionVacationRanges[i].Start.Year, missionVacationRanges[i].Start.Month, missionVacationRanges[i].Start.Day, 0, 0, 0),
                                        LogType = LogType.Exit,
                                        ErrorType = ErrorType.None,
                                        Note = note
                                    };
                                    if (!CheckEntranceExitRecordDuplicate(allEntranceExitRecordDataOfEmployee,
                                       allFingerprintTransferredDataOfEmployee, exitRecord1.LogDateTime,
                                       InsertSource.AutoGenerate, LogType.Exit, 0))
                                    {
                                        if (i == 0 || missionVacationRanges[i].Start != missionVacationRanges[i - 1].End)
                                        {
                                            cardEntities.Add(exitRecord1);
                                        }

                                    }
                                }
                                if (missionVacationRanges[i].End != exitRecord.LogDateTime)
                                {
                                    var entranceRecord1 = new EntranceExitRecord
                                    {
                                        Employee = employeeAttendanceCard.Employee,
                                        InsertSource = InsertSource.AutoGenerate,
                                        LogDateTime = missionVacationRanges[i].End,
                                        LogTime = new DateTime(2000, 1, 1, missionVacationRanges[i].End.Hour, missionVacationRanges[i].End.Minute, missionVacationRanges[i].End.Second),
                                        LogDate = new DateTime(missionVacationRanges[i].End.Year, missionVacationRanges[i].End.Month, missionVacationRanges[i].End.Day, 0, 0, 0),
                                        LogType = LogType.Entrance,
                                        ErrorType = ErrorType.None,
                                        Note = note
                                    };

                                    if (!CheckEntranceExitRecordDuplicate(allEntranceExitRecordDataOfEmployee,
                                         allFingerprintTransferredDataOfEmployee, entranceRecord1.LogDateTime,
                                         InsertSource.AutoGenerate, LogType.Entrance, 0))
                                    {
                                        if (i == missionVacationRanges.Count - 1 || missionVacationRanges[i].End != missionVacationRanges[i + 1].Start)
                                        {
                                            cardEntities.Add(entranceRecord1);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (cardEntities.Count > 0)
                {
                    entities.AddRange(cardEntities);
                    cardEntities = new List<IAggregateRoot>();
                }
            }
            if (entities.Count() > 0)
            {
                var info = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.AutoGenerateAttendanceRecords);
                ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, null, Souccar.Domain.Audit.OperationType.Update, info, start, null);
            }
        }

        public static int GenerateAttendanceRecordForSelectedEmployees(IQueryable employeeAttendanceCards, AttendanceRecord attendanceRecord, GeneralSettings generalSettings)
        {
            var allEmployeeAttendanceCards = (IEnumerable<EmployeeCard>)employeeAttendanceCards;

            // الشهور الشبيهة هي الشهور التي لها نفس رقم الشهر ونفس السنة
            var similarAttendanceRecords = typeof(AttendanceRecord).GetAll<AttendanceRecord>()
                 .Where(x =>
                     x.Id != attendanceRecord.Id &&
                     x.Year == attendanceRecord.Year &&
                     x.Month == attendanceRecord.Month).ToList();

            // لمعرفة البطاقة الاساسية التي لم يتم توليد بطاقات شهرية لها سواء بنفس الشهر او بغير اشهر
            // ويكون الرابط بين الاشهر هو رقم الشهر
            // اذا كان الشهر من نوع رواتب يمنع التكرار للبطاقات الشهرية بنفس الشهر وبغير اشهر مماثلة
            // أما اذا كان الشهر من نوع تعويضات يمنع التكرار بنفس الشهر ويسمح مع غير اشهر
            var employeeWithNoAttendanceDailyAdjustments = allEmployeeAttendanceCards
                .Where(x =>
                    (attendanceRecord.AttendanceDailyAdjustments.All(y => y.EmployeeAttendanceCard.Employee != x.Employee) &&
                    similarAttendanceRecords.SelectMany(z => z.AttendanceDailyAdjustments).All(a => a.EmployeeAttendanceCard.Employee != x.Employee)));

            var employeeWithNoAttendanceMonthlyAdjustments = allEmployeeAttendanceCards
                .Where(x =>
                    (attendanceRecord.AttendanceMonthlyAdjustments.All(y => y.EmployeeAttendanceCard.Employee != x.Employee) &&
                    similarAttendanceRecords.SelectMany(z => z.AttendanceMonthlyAdjustments).All(a => a.EmployeeAttendanceCard.Employee != x.Employee)));

            var employeeWithNoAttendanceWithoutAdjustments = allEmployeeAttendanceCards
                .Where(x =>
                    (attendanceRecord.AttendanceWithoutAdjustments.All(y => y.EmployeeAttendanceCard.Employee != x.Employee) &&
                    similarAttendanceRecords.SelectMany(z => z.AttendanceWithoutAdjustments).All(a => a.EmployeeAttendanceCard.Employee != x.Employee)));

            var employeesWithNoAttendanceRecord = allEmployeeAttendanceCards.Where(x =>
            employeeWithNoAttendanceDailyAdjustments.Any(y => y.Id == x.Id) &&
            employeeWithNoAttendanceMonthlyAdjustments.Any(y => y.Id == x.Id) &&
            employeeWithNoAttendanceWithoutAdjustments.Any(y => y.Id == x.Id));
            //todo Mhd Alsaadi: similarAttendanceRecords.ToList() تم تحويلها الى ليست لوجود مشكلة بالكيويري الكومبلكس في انهايبرنيت

            var cardsWithNoAttendanceRecords = employeesWithNoAttendanceRecord as IList<EmployeeCard> ??
                                          employeesWithNoAttendanceRecord.ToList();

            GenerateAttendanceRecordForSelectedEmployees(cardsWithNoAttendanceRecords, attendanceRecord, generalSettings);

            attendanceRecord.AttendanceMonthStatus = AttendanceMonthStatus.Generated;
            return cardsWithNoAttendanceRecords.Count();
        }
        public static void GenerateAttendanceRecordDetailsUntillCurrentDay(AttendanceRecord attendanceRecord)
        {
            var entities = new List<IAggregateRoot>();
            var days = 0;
            DateTime endDate = attendanceRecord.ToDate >= DateTime.Now.Date ? DateTime.Now.Date : attendanceRecord.ToDate.AddDays(1);
            if (attendanceRecord.FromDate < endDate)
            {
                days = (endDate - attendanceRecord.FromDate).Days;
            }
            if (days > 0)
            {
                foreach (var item in attendanceRecord.AttendanceWithoutAdjustments)
                {
                    List<DateTime> realDays = new List<DateTime>();  
                    var totalDaysOfEmployee = days;
                    var date = attendanceRecord.FromDate;
                    var hireDate = item.EmployeeAttendanceCard.StartWorkingDate;
                    var abruptionDate = item.EmployeeAttendanceCard.EndWorkingDate;
                    date = hireDate.HasValue && hireDate.Value > attendanceRecord.FromDate ? 
                        hireDate.Value : 
                        attendanceRecord.FromDate;
                    var daysConflictWithAbruptionDate = abruptionDate.HasValue && abruptionDate.Value > attendanceRecord.FromDate &&
                        abruptionDate.Value < endDate ?
                        (endDate - abruptionDate.Value).TotalDays : 0;
                    if (date != attendanceRecord.FromDate) totalDaysOfEmployee = (int)(endDate - date).TotalDays;
                    totalDaysOfEmployee = totalDaysOfEmployee - (int)(daysConflictWithAbruptionDate);
                    var oldListOfDetail = new List<AttendanceWithoutAdjustmentDetail>();
                    oldListOfDetail.AddRange(item.AttendanceWithoutAdjustmentDetails);
                    item.AttendanceWithoutAdjustmentDetails.Clear();
                    for (var i = 0; i < totalDaysOfEmployee; i++)
                    {
                        realDays.Add(date);
                        if (!oldListOfDetail.Any(x=> x.Date == date))
                        {
                            var detail = new AttendanceWithoutAdjustmentDetail
                            {
                                Date = date,
                                DayOfWeek = date.DayOfWeek
                            };
                            item.AddAttendanceMonthlyAdjustmentDetail(detail);
                        }
                        else
                        {
                            item.AddAttendanceMonthlyAdjustmentDetail(oldListOfDetail.FirstOrDefault(x => x.Date == date));
                        }
                        date = date.AddDays(1);
                    }
                    //item.AttendanceWithoutAdjustmentDetails = item.AttendanceWithoutAdjustmentDetails.Where(x => realDays.Any(y => y == x.Date)).ToList();
                    entities.Add(item);
                }
                foreach (var item in attendanceRecord.AttendanceDailyAdjustments)
                {
                    List<DateTime> realDays = new List<DateTime>();
                    var totalDaysOfEmployee = days;
                    var date = attendanceRecord.FromDate;
                    var hireDate = item.EmployeeAttendanceCard.StartWorkingDate;
                    var abruptionDate = item.EmployeeAttendanceCard.EndWorkingDate;
                    date = hireDate.HasValue && hireDate.Value > attendanceRecord.FromDate ?
                        hireDate.Value :
                        attendanceRecord.FromDate;
                    var daysConflictWithAbruptionDate = abruptionDate.HasValue && abruptionDate.Value > attendanceRecord.FromDate &&
                        abruptionDate.Value < endDate ?
                        (endDate - abruptionDate.Value).TotalDays : 0;
                    if (date != attendanceRecord.FromDate) totalDaysOfEmployee = (int)(endDate - date).TotalDays;
                    totalDaysOfEmployee = totalDaysOfEmployee - (int)(daysConflictWithAbruptionDate);
                    var oldListOfDetail = new List<AttendanceDailyAdjustmentDetail>();
                    oldListOfDetail.AddRange(item.AttendanceDailyAdjustmentDetails);
                    item.AttendanceDailyAdjustmentDetails.Clear();
                    for (var i = 0; i < totalDaysOfEmployee; i++)
                    {
                        realDays.Add(date);
                        if (!oldListOfDetail.Any(x => x.Date == date))
                        {
                            var detail = new AttendanceDailyAdjustmentDetail
                            {
                                Date = date,
                                DayOfWeek = date.DayOfWeek
                            };
                            item.AddAttendanceDailyAdjustmentDetail(detail);
                        }
                        else
                        {
                            item.AddAttendanceDailyAdjustmentDetail(oldListOfDetail.FirstOrDefault(x => x.Date == date));
                        }
                        date = date.AddDays(1);
                    }
                    entities.Add(item);
                }
                foreach (var item in attendanceRecord.AttendanceMonthlyAdjustments)
                {
                    List<DateTime> realDays = new List<DateTime>();
                    var totalDaysOfEmployee = days;
                    var date = attendanceRecord.FromDate;
                    var hireDate = item.EmployeeAttendanceCard.StartWorkingDate;
                    var abruptionDate = item.EmployeeAttendanceCard.EndWorkingDate;
                    date = hireDate.HasValue && hireDate.Value > attendanceRecord.FromDate ?
                        hireDate.Value :
                        attendanceRecord.FromDate;
                    var daysConflictWithAbruptionDate = abruptionDate.HasValue && abruptionDate.Value > attendanceRecord.FromDate &&
                        abruptionDate.Value < endDate ?
                        (endDate - abruptionDate.Value).TotalDays : 0;
                    if (date != attendanceRecord.FromDate) totalDaysOfEmployee = (int)(endDate - date).TotalDays;
                    totalDaysOfEmployee = totalDaysOfEmployee - (int)(daysConflictWithAbruptionDate);
                    var oldListOfDetail = new List<AttendanceMonthlyAdjustmentDetail>();
                    oldListOfDetail.AddRange(item.AttendanceMonthlyAdjustmentDetails);
                    item.AttendanceMonthlyAdjustmentDetails.Clear();
                    for (var i = 0; i < totalDaysOfEmployee; i++)
                    {
                        realDays.Add(date);
                        if (!oldListOfDetail.Any(x => x.Date == date))
                        {
                            var detail = new AttendanceMonthlyAdjustmentDetail
                            {
                                Date = date,
                                DayOfWeek = date.DayOfWeek
                            };
                            item.AddAttendanceMonthlyAdjustmentDetail(detail);
                        }
                        else
                        {
                            item.AddAttendanceMonthlyAdjustmentDetail(oldListOfDetail.FirstOrDefault(x => x.Date == date));
                        }
                        date = date.AddDays(1);
                    }
                    entities.Add(item);
                }
            }
            ServiceFactory.ORMService.SaveTransaction<IAggregateRoot>(entities, UserExtensions.CurrentUser);
        }
        

        public static void GenerateAttendanceRecordForSelectedEmployees(IList<EmployeeCard> employeeAttendanceCards, AttendanceRecord attendanceRecord, GeneralSettings generalSettings)
        {
            AttendanceForm generalSettingsAttendanceForm = generalSettings != null ? generalSettings.AttendanceForm : null;
            if (generalSettingsAttendanceForm != null)
                foreach (var employeeAttendanceCard in employeeAttendanceCards)
                {
                    switch (GetAttendanceForm(employeeAttendanceCard.Employee, generalSettingsAttendanceForm).CalculationMethod)
                    {
                        case CalculationMethod.WithoutAdjustment:
                            {
                                var masterRecord = new AttendanceWithoutAdjustment
                                {
                                    EmployeeAttendanceCard = employeeAttendanceCard
                                };
                                attendanceRecord.AddAttendanceWithoutAdjustment(masterRecord);
                                break;
                            }
                        case CalculationMethod.DailyAdjustment:
                            {
                                var masterRecord = new AttendanceDailyAdjustment
                                {
                                    EmployeeAttendanceCard = employeeAttendanceCard
                                };
                                attendanceRecord.AddAttendanceDailyAdjustment(masterRecord);
                                break;
                            }
                        case CalculationMethod.MonthlyAdjustment:
                            {
                                var masterRecord = new AttendanceMonthlyAdjustment
                                {
                                    EmployeeAttendanceCard = employeeAttendanceCard
                                };
                                attendanceRecord.AddAttendanceMonthlyAdjustment(masterRecord);
                                break;
                            }
                        default:
                            throw new Exception("Calculation Method Not Supported");
                    }
                }
        }

        public static void GenerateAttendanceRecordDetails(IList<EmployeeCard> employeeAttendanceCards, AttendanceRecord attendanceRecord, GeneralSettings generalSettings)
        {
            AttendanceForm generalSettingsAttendanceForm = generalSettings != null ? generalSettings.AttendanceForm : null;
            if (generalSettingsAttendanceForm != null)
                foreach (var employeeAttendanceCard in employeeAttendanceCards)
                {
                    switch (GetAttendanceForm(employeeAttendanceCard.Employee, generalSettingsAttendanceForm).CalculationMethod)
                    {
                        case CalculationMethod.WithoutAdjustment:
                            {
                                var masterRecord = new AttendanceWithoutAdjustment
                                {
                                    EmployeeAttendanceCard = employeeAttendanceCard
                                };
                                var days = (DateTime.Now.Date - attendanceRecord.FromDate).Days;
                                var date = attendanceRecord.FromDate;
                                for (var i = 0; i < days; i++)
                                {
                                    var detail = new AttendanceWithoutAdjustmentDetail
                                    {
                                        Date = date,
                                        DayOfWeek = date.DayOfWeek
                                    };
                                    masterRecord.AddAttendanceMonthlyAdjustmentDetail(detail);
                                    date = date.AddDays(1);
                                }
                                attendanceRecord.AddAttendanceWithoutAdjustment(masterRecord);
                                break;
                            }
                        case CalculationMethod.DailyAdjustment:
                            {
                                var masterRecord = new AttendanceDailyAdjustment
                                {
                                    EmployeeAttendanceCard = employeeAttendanceCard
                                };
                                var days = (DateTime.Now.Date - attendanceRecord.FromDate).Days;
                                var date = attendanceRecord.FromDate;
                                for (var i = 0; i < days; i++)
                                {
                                    var detail = new AttendanceDailyAdjustmentDetail
                                    {
                                        Date = date,
                                        DayOfWeek = date.DayOfWeek
                                    };
                                    masterRecord.AddAttendanceDailyAdjustmentDetail(detail);
                                    date = date.AddDays(1);
                                }
                                attendanceRecord.AddAttendanceDailyAdjustment(masterRecord);
                                break;
                            }
                        case CalculationMethod.MonthlyAdjustment:
                            {
                                var masterRecord = new AttendanceMonthlyAdjustment
                                {
                                    EmployeeAttendanceCard = employeeAttendanceCard
                                };

                                var days = (DateTime.Now.Date - attendanceRecord.FromDate).Days;
                                var date = attendanceRecord.FromDate;
                                for (var i = 0; i < days; i++)
                                {
                                    var detail = new AttendanceMonthlyAdjustmentDetail
                                    {
                                        Date = date,
                                        DayOfWeek = date.DayOfWeek
                                    };
                                    masterRecord.AddAttendanceMonthlyAdjustmentDetail(detail);
                                    date = date.AddDays(1);
                                }
                                attendanceRecord.AddAttendanceMonthlyAdjustment(masterRecord);
                                break;
                            }
                        default:
                            throw new Exception("Calculation Method Not Supported");
                    }
                }
        }

        #endregion

        #region Attendance Record Generation Helper

        public static int GetRecurrenceIndex(AttendanceForm attendanceForm, DateTime fromDate)
        {
            var index = 0;
            var totalRecurrences = attendanceForm.WorkshopRecurrences.Count;

            var firstDayInYear = new DateTime(fromDate.Year, 1, 1);
            var lastDayInYear = new DateTime(fromDate.Year, 12, 31);

            for (var day = firstDayInYear.Date; day.Date <= lastDayInYear.Date; day = day.AddDays(1))
            {
                if (day == fromDate)
                {
                    return index;
                }
                else if (index < totalRecurrences - 1)
                {
                    index++;
                }
                else
                    index = 0;

            }
            return index;
        }


        public static int GetRecurrenceIndexByDate(AttendanceForm attendanceForm, DateTime fromDate)
        {
            var thisMonth = fromDate.Month;
            var fromDateDay = fromDate.Day;
            DateTime lastMonthDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);
            if (fromDate.Month == 1)
                lastMonthDate = new DateTime(fromDate.Year - 1, 12, fromDate.Day);
            else
                lastMonthDate = new DateTime(fromDate.Year, fromDate.Month - 1, fromDate.Day);

            var recurrences = attendanceForm.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList();
            var recurrencesCount = attendanceForm.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList().Count();

            var index = 0;
            if (attendanceForm.CalculationMethod == CalculationMethod.MonthlyAdjustment)
            {
                var lastAttendanceMonthlyAdjustmentDetail = ServiceFactory.ORMService.All<AttendanceMonthlyAdjustmentDetail>().Where(x => x.Date.Month == lastMonthDate.Month
                    && x.Date.Year == lastMonthDate.Year ).OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastAttendanceMonthlyAdjustmentDetail != null)
                {
                    var recurrenceIndex = recurrences.FindIndex(x => x.RecurrenceOrder == lastAttendanceMonthlyAdjustmentDetail.RecurrenceIndex);
                    if (recurrenceIndex + 1 < recurrences.Count())
                    {
                        index = recurrenceIndex + 1;
                        if (fromDateDay > 1)
                        {
                            var firstDayInMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
                            var enteredDayInMonth = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);

                            for (var day = firstDayInMonth.Date; day.Date <= enteredDayInMonth.Date; day = day.AddDays(1))
                            {
                                if (day == fromDate)
                                {
                                    return index;
                                }
                                else if (index < recurrencesCount - 1)
                                {
                                    index++;
                                }
                                else
                                    index = 0;

                            }
                        }
                        return index;
                    }
                }
            }
            if (attendanceForm.CalculationMethod == CalculationMethod.DailyAdjustment)
            {
                var lastAttendanceDailyAdjustmentDetail = ServiceFactory.ORMService.All<AttendanceDailyAdjustmentDetail>().Where(x => x.Date.Month == lastMonthDate.Month
                    && x.Date.Year == lastMonthDate.Year).OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastAttendanceDailyAdjustmentDetail != null)
                {
                    var recurrenceIndex = recurrences.FindIndex(x => x.RecurrenceOrder == lastAttendanceDailyAdjustmentDetail.RecurrenceIndex);
                    if (recurrenceIndex + 1 < recurrences.Count())
                    {
                        index = recurrenceIndex + 1;
                        if (fromDateDay > 1)
                        {
                            var firstDayInMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
                            var enteredDayInMonth = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);

                            for (var day = firstDayInMonth.Date; day.Date <= enteredDayInMonth.Date; day = day.AddDays(1))
                            {
                                if (day == fromDate)
                                {
                                    return index;
                                }
                                else if (index < recurrencesCount - 1)
                                {
                                    index++;
                                }
                                else
                                    index = 0;

                            }
                        }
                        return index;
                    }
                }
            }

            if (attendanceForm.CalculationMethod == CalculationMethod.WithoutAdjustment)
            {
                var lastAttendanceWithoutAdjustmentDetail = ServiceFactory.ORMService.All<AttendanceWithoutAdjustmentDetail>().Where(x => x.Date.Month == lastMonthDate.Month
                    && x.Date.Year == lastMonthDate.Year).OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastAttendanceWithoutAdjustmentDetail != null)
                {
                    var recurrenceIndex = recurrences.FindIndex(x => x.RecurrenceOrder == lastAttendanceWithoutAdjustmentDetail.RecurrenceIndex);
                    if (recurrenceIndex + 1 < recurrences.Count())
                    {
                        index = recurrenceIndex + 1;
                        if (fromDateDay > 1)
                        {
                            var firstDayInMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
                            var enteredDayInMonth = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);

                            for (var day = firstDayInMonth.Date; day.Date <= enteredDayInMonth.Date; day = day.AddDays(1))
                            {
                                if (day == fromDate)
                                {
                                    return index;
                                }
                                else if (index < recurrencesCount - 1)
                                {
                                    index++;
                                }
                                else
                                    index = 0;

                            }
                        }
                        return index;
                    }
                }
            }
            else
                index = 0;
            return index;
        }

        public static int GetRecurrenceIndexByDate(AttendanceForm attendanceForm, DateTime fromDate,EmployeeCard employeeCard)
        {
            var thisMonth = fromDate.Month;
            var fromDateDay = fromDate.Day;
            int year, month;
            if (fromDate.Month == 1)
            {
                year = fromDate.Year - 1;
                month = 12;
            }
            else
            {
                year = fromDate.Year ;
                month = fromDate.Month - 1;
            }
            var lastMonthDate = new DateTime(year, month, 1).AddDays(fromDate.Day - 1);

            //DateTime lastMonthDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);
            //if (fromDate.Month == 1)
            //    lastMonthDate = new DateTime(fromDate.Year - 1, 12, fromDate.Day);
            //else if(fromDate.Month == 3) // الشهر الثاني 28
            //{
            //    if (DateTime.IsLeapYear(fromDate.Year)) // سنة كبيسة
            //    {
            //        lastMonthDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day - 29);
            //    }
            //    else
            //    {
            //        lastMonthDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day - 28);
            //    }
            //}
            //else
            //    lastMonthDate = new DateTime(fromDate.Year, fromDate.Month - 1, fromDate.Day);

            var recurrences = attendanceForm.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList();
            var recurrencesCount = attendanceForm.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList().Count();

            var index = 0;
            if (attendanceForm.CalculationMethod == CalculationMethod.MonthlyAdjustment)
            {
                var lastAttendanceMonthlyAdjustmentDetail = ServiceFactory.ORMService.All<AttendanceMonthlyAdjustmentDetail>().Where(x => x.Date.Month == lastMonthDate.Month
                    && x.Date.Year == lastMonthDate.Year && x.AttendanceMonthlyAdjustment.EmployeeAttendanceCard.Id == employeeCard.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastAttendanceMonthlyAdjustmentDetail != null)
                {
                    var recurrenceIndex = recurrences.FindIndex(x => x.RecurrenceOrder == lastAttendanceMonthlyAdjustmentDetail.RecurrenceIndex);
                    if (recurrenceIndex + 1 < recurrences.Count())
                    {
                        index = recurrenceIndex + 1;
                        if (fromDateDay > 1)
                        {
                            var firstDayInMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
                            var enteredDayInMonth = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);

                            for (var day = firstDayInMonth.Date; day.Date <= enteredDayInMonth.Date; day = day.AddDays(1))
                            {
                                if (day == fromDate)
                                {
                                    return index;
                                }
                                else if (index < recurrencesCount - 1)
                                {
                                    index++;
                                }
                                else
                                    index = 0;

                            }
                        }
                        return index;
                    }
                }
            }
            if (attendanceForm.CalculationMethod == CalculationMethod.DailyAdjustment)
            {
                var lastAttendanceDailyAdjustmentDetail = ServiceFactory.ORMService.All<AttendanceDailyAdjustmentDetail>().Where(x => x.Date.Month == lastMonthDate.Month
                    && x.Date.Year == lastMonthDate.Year && x.AttendanceDailyAdjustment.EmployeeAttendanceCard.Id == employeeCard.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastAttendanceDailyAdjustmentDetail != null)
                {
                    var recurrenceIndex = recurrences.FindIndex(x => x.RecurrenceOrder == lastAttendanceDailyAdjustmentDetail.RecurrenceIndex);
                    if (recurrenceIndex + 1 < recurrences.Count())
                    {
                        index = recurrenceIndex + 1;
                        if (fromDateDay > 1)
                        {
                            var firstDayInMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
                            var enteredDayInMonth = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);

                            for (var day = firstDayInMonth.Date; day.Date <= enteredDayInMonth.Date; day = day.AddDays(1))
                            {
                                if (day == fromDate)
                                {
                                    return index;
                                }
                                else if (index < recurrencesCount - 1)
                                {
                                    index++;
                                }
                                else
                                    index = 0;

                            }
                        }
                        return index;
                    }
                }
            }

            if (attendanceForm.CalculationMethod == CalculationMethod.WithoutAdjustment)
            {
                var lastAttendanceWithoutAdjustmentDetail = ServiceFactory.ORMService.All<AttendanceWithoutAdjustmentDetail>().Where(x => x.Date.Month == lastMonthDate.Month
                    && x.Date.Year == lastMonthDate.Year && x.AttendanceWithoutAdjustment.EmployeeAttendanceCard.Id == employeeCard.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastAttendanceWithoutAdjustmentDetail != null)
                {
                    var recurrenceIndex = recurrences.FindIndex(x => x.RecurrenceOrder == lastAttendanceWithoutAdjustmentDetail.RecurrenceIndex);
                    if (recurrenceIndex + 1 < recurrences.Count())
                    {
                        index = recurrenceIndex + 1;
                        if (fromDateDay > 1)
                        {
                            var firstDayInMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
                            var enteredDayInMonth = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);

                            for (var day = firstDayInMonth.Date; day.Date <= enteredDayInMonth.Date; day = day.AddDays(1))
                            {
                                if (day == fromDate)
                                {
                                    return index;
                                }
                                else if (index < recurrencesCount - 1)
                                {
                                    index++;
                                }
                                else
                                    index = 0;

                            }
                        }
                        return index;
                    }
                }
            }
            else
                index = 0;
            return index;
        }


        public static Dictionary<Employee, List<WorkshopRecurrenceDTO>> GetWorkshopsRecurrenceInPeriod(IList<EmployeeCard> employeesAttendanceCard, DateTime fromDate, DateTime toDate)
        {
            Dictionary<Employee, IList<WorkshopRecurrence>> EmployeesRecurrences = new Dictionary<Employee, IList<WorkshopRecurrence>>();
            Dictionary<Employee, int> EmployeesRecurrenceIndexes = new Dictionary<Employee, int>();
            var totalDays = (int)(toDate - fromDate).TotalDays + 1;
            IList<Employee> employees = new List<Employee>();
            foreach (var emp in employeesAttendanceCard)
            {
                employees.Add(emp.Employee);
            }
            Dictionary<Employee, AttendanceForm> forms = GetAttendanceForms(employees);
            foreach (var item in forms)
            {
                IList<WorkshopRecurrence> recurrences = item.Value.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList();
                Employee Emp = item.Key;
                EmployeesRecurrences.Add(Emp, recurrences);
                var recurrenceIndex = GetRecurrenceIndexByDate(forms[Emp], fromDate,Emp.EmployeeCard);
                EmployeesRecurrenceIndexes.Add(Emp, recurrenceIndex);
            }
            Dictionary<Employee, List<WorkshopRecurrenceDTO>> results = new Dictionary<Employee, List<WorkshopRecurrenceDTO>>();
            IList<PublicHoliday> publicHolidays;
            IList<FixedHoliday> fixedHolidays;
            IList<ChangeableHoliday> changeableHolidays;
            AttendanceSystemIntegrationService.GetHolidays(out publicHolidays, out fixedHolidays, out changeableHolidays);
            foreach (var e in employees)
            {
                var totalDaysOfEmployee = totalDays;
                var employeeCard = e.EmployeeCard;
                var cumulativeDate = fromDate;
                var hireDate = employeeCard.StartWorkingDate;
                var abruptionDate = employeeCard.EndWorkingDate;
                cumulativeDate = hireDate.HasValue && hireDate.Value > fromDate ?
                    hireDate.Value :
                    fromDate;
                var daysConflictWithAbruptionDate = abruptionDate.HasValue && abruptionDate.Value > fromDate &&
                    abruptionDate.Value < toDate ?
                    (toDate - abruptionDate.Value).TotalDays : 0;
                if(cumulativeDate != fromDate) totalDaysOfEmployee = (int)(toDate - cumulativeDate).TotalDays + 1;
                totalDaysOfEmployee = totalDaysOfEmployee - (int)(daysConflictWithAbruptionDate);
                var result = new List<WorkshopRecurrenceDTO>();
                for (var i = 0; i < totalDaysOfEmployee; i++)
                {
                    if (AttendanceSystemIntegrationService.IsHoliday(cumulativeDate, publicHolidays, fixedHolidays, changeableHolidays) && forms[e].RelyHolidaies)
                    {
                        result.Add(new WorkshopRecurrenceDTO
                        {
                            Date = cumulativeDate,
                            RecurrenceType = WorkshopRecurrenceTypeDTO.Holiday,
                            Workshop = GetCorrespondentWorkshop(employeeCard, EmployeesRecurrences[e][EmployeesRecurrenceIndexes[e]].Workshop, cumulativeDate),
                            RecurrenceIndex = EmployeesRecurrences[e][EmployeesRecurrenceIndexes[e]].RecurrenceOrder
                        });
                        EmployeesRecurrenceIndexes[e]++;
                    }
                    else
                    {
                        if (EmployeesRecurrences[e][EmployeesRecurrenceIndexes[e]].IsOff)
                        {
                            result.Add(new WorkshopRecurrenceDTO
                            {
                                Date = cumulativeDate,
                                RecurrenceType = WorkshopRecurrenceTypeDTO.Off,
                                Workshop = GetCorrespondentWorkshop(employeeCard, EmployeesRecurrences[e][EmployeesRecurrenceIndexes[e]].Workshop, cumulativeDate),
                                RecurrenceIndex = EmployeesRecurrences[e][EmployeesRecurrenceIndexes[e]].RecurrenceOrder
                            });
                        }
                        else
                        {
                            result.Add(new WorkshopRecurrenceDTO
                            {
                                Date = cumulativeDate,
                                RecurrenceType = WorkshopRecurrenceTypeDTO.Work,
                                Workshop = GetCorrespondentWorkshop(employeeCard, EmployeesRecurrences[e][EmployeesRecurrenceIndexes[e]].Workshop, cumulativeDate),//recurrences[recurrenceIndex].Workshop
                                RecurrenceIndex = EmployeesRecurrences[e][EmployeesRecurrenceIndexes[e]].RecurrenceOrder
                            });
                        }
                        EmployeesRecurrenceIndexes[e]++;
                    }

                    cumulativeDate = cumulativeDate.AddDays(1);
                    if (EmployeesRecurrenceIndexes[e] >= EmployeesRecurrences[e].Count)
                    {
                        EmployeesRecurrenceIndexes[e] = 0;
                    }
                }
                results.Add(e, result);
            }
            return results;
        }

        public static Dictionary<Employee, AttendanceForm> GetAttendanceForms(IList<Employee> employees)
        {
            var employee_current = new Employee();
            Dictionary<Employee, AttendanceForm> attendanceForms = new Dictionary<Employee, AttendanceForm>();
            try
            {
                var grades = ServiceFactory.ORMService.All<HRIS.Domain.Grades.RootEntities.Grade>().ToList();
                var generalSettingsAttendanceForm = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault().AttendanceForm;
                foreach (var employee in employees)
                {
                    employee_current = employee;
                    AttendanceForm attendanceForm = new AttendanceForm();
                    AttendanceForm gradeAttendanceForm = null;
                    AttendanceForm employeeCardAttendanceForm = employee.EmployeeCard?.AttendanceForm;
                    var grade = GetGradeOfEmployee(employee);
                    if (grade != null)
                        gradeAttendanceForm = grade.AttendanceForm;
                    if (employeeCardAttendanceForm != null)
                        attendanceForm = employeeCardAttendanceForm;
                    else if (gradeAttendanceForm != null)
                        attendanceForm = gradeAttendanceForm;
                    else
                        attendanceForm = generalSettingsAttendanceForm;
                    attendanceForms.Add(employee, attendanceForm);

                }
                return attendanceForms;
            }
            catch (Exception e)
            {
                return attendanceForms;
            }
        }



        public static Workshop GetCorrespondentWorkshop(EmployeeCard employeeCard, Workshop workshop, DateTime date)
        {
            EmployeeTemporaryWorkshop empTemporaryWorkshop;
            TemporaryWorkshop generalTemporaryWorkshop;
            if (employeeCard != null)
            {
                empTemporaryWorkshop = employeeCard.TemporaryWorkshops.FirstOrDefault(x => date >= x.FromDate && date <= x.ToDate);
                if (empTemporaryWorkshop != null)
                {
                    workshop = empTemporaryWorkshop.Workshop;
                }
                else
                {
                    generalTemporaryWorkshop = workshop.TemporaryWorkshops.FirstOrDefault(x => date >= x.FromDate && date <= x.ToDate);
                    if (generalTemporaryWorkshop != null)
                    {
                        workshop = generalTemporaryWorkshop.AlternativeWorkshop;
                    }
                }
            }
            return workshop.Prepare(date);
        }
        #endregion

        public static bool CheckEntranceExitRecordsConsistency(AttendanceRecord attendanceRecord)
        {
            var entities = new List<IAggregateRoot>();

            var entranceExitRecords = ServiceFactory.ORMService.All<EntranceExitRecord>();
            var notCheckedEntranceExitRecords = entranceExitRecords.Where(x => x.IsChecked == false || x.IsChecked == null);
            var success = true;
            //احضار كامل بطاقات الدوام للموظفين الموجودين بهذا الشهر سواء كانو بدون تقاص او تقاص يومي او شهري
            var employeeAttendanceCards = attendanceRecord.AttendanceWithoutAdjustments.Select(x => x.EmployeeAttendanceCard).ToList();
            employeeAttendanceCards.AddRange(attendanceRecord.AttendanceDailyAdjustments.Select(x => x.EmployeeAttendanceCard));
            employeeAttendanceCards.AddRange(attendanceRecord.AttendanceMonthlyAdjustments.Select(x => x.EmployeeAttendanceCard));

            // كل ريكورد سجل دخول وخروج بتعامل معو بضيفو بهاد الليست لان هالشي بفيدني لاعرف السجلات خارج المجال
            var allEntranceExitRecords = new List<EntranceExitRecord>();
            var AllRecurrences = GetWorkshopsRecurrenceInPeriod(employeeAttendanceCards,
                    attendanceRecord.FromDate,
                    attendanceRecord.ToDate);
            foreach (var employeeAttendanceCard in employeeAttendanceCards)
            {// من اجل كل موظف بالشهر نقوم بالتالي

                // احضار ورديات الموظف من اول يوم بالشهر حتى اخر يوم بالشهر
                var recurrences = AllRecurrences[employeeAttendanceCard.Employee];
                var ranges = recurrences.ToList().SelectMany(x => x.Workshop.Prepare(x.Date).NormalShifts).ToList();
                var unCheckedRecords = notCheckedEntranceExitRecords.ToList().Where(x => ranges.Any(y => x.LogDateTime >= y.ShiftRangeStartTime &&
                                                             x.LogDateTime <= y.ShiftRangeEndTime) && x.Employee.Id == employeeAttendanceCard.Employee.Id).ToList();
                if (unCheckedRecords.Any())
                    return false;

            }
            return success;
        }
        public static WorkshopRecurrenceDTO GetWorkshopsRecurrenceInDate(EmployeeCard employeeAttendanceCard, DateTime date,
            AttendanceForm form, IList<PublicHoliday> publicHolidays, IList<FixedHoliday> fixedHolidays, IList<ChangeableHoliday> changeableHolidays)
        {
            IList<WorkshopRecurrence> recurrences = form.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList();

            var recurrenceIndex = GetRecurrenceIndexByDate(form, date, employeeAttendanceCard);
            if (AttendanceSystemIntegrationService.IsHoliday(date, publicHolidays, fixedHolidays, changeableHolidays) && form.RelyHolidaies)
            {
                return new WorkshopRecurrenceDTO
                {
                    Date = date,
                    RecurrenceType = WorkshopRecurrenceTypeDTO.Holiday,
                    Workshop = GetCorrespondentWorkshop(employeeAttendanceCard, recurrences[recurrenceIndex].Workshop, date),
                    RecurrenceIndex = recurrences[recurrenceIndex].RecurrenceOrder
                };
            }
            else
            {
                if (recurrences[recurrenceIndex].IsOff)
                {
                    return new WorkshopRecurrenceDTO
                    {
                        Date = date,
                        RecurrenceType = WorkshopRecurrenceTypeDTO.Off,
                        Workshop = GetCorrespondentWorkshop(employeeAttendanceCard, recurrences[recurrenceIndex].Workshop, date),
                        RecurrenceIndex = recurrences[recurrenceIndex].RecurrenceOrder
                    };
                }
                else
                {
                    return new WorkshopRecurrenceDTO
                    {
                        Date = date,
                        RecurrenceType = WorkshopRecurrenceTypeDTO.Work,
                        Workshop = GetCorrespondentWorkshop(employeeAttendanceCard, recurrences[recurrenceIndex].Workshop, date),
                        RecurrenceIndex = recurrences[recurrenceIndex].RecurrenceOrder
                    };
                }
            }
        }

        public static List<KeyValuePair< string, string>> CheckEntranceExitRecordsConsistency(IList<int> filteredEntranceExitRecordIds)
        {
            var entities = new List<IAggregateRoot>();
            var allEntranceExitRecords = new List<EntranceExitRecord>();
            var success = true;
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            IList<PublicHoliday> publicHolidays;
            IList<FixedHoliday> fixedHolidays;
            IList<ChangeableHoliday> changeableHolidays;
            AttendanceSystemIntegrationService.GetHolidays(out publicHolidays, out fixedHolidays, out changeableHolidays);
            var entranceExitRecords = ServiceFactory.ORMService.All<EntranceExitRecord>().ToList();

           var entranceExitRecordsAfterFilter = !filteredEntranceExitRecordIds.Any() ? entranceExitRecords : entranceExitRecords.Where(x => filteredEntranceExitRecordIds.Any(y => y == x.Id)).ToList();
            var notCheckedEntranceExitRecords = entranceExitRecordsAfterFilter.Where(x => x.IsChecked == false || x.IsChecked == null);
            if (notCheckedEntranceExitRecords.Any())
            {
                var notCheckedEntranceExitRecordsGroupedByEmp = notCheckedEntranceExitRecords
                .GroupBy(x => x.Employee);

                var employeeIds = notCheckedEntranceExitRecordsGroupedByEmp.Select(x => x.Key.Id);
                var employees = ServiceFactory.ORMService.All<Employee>().ToList().Where(x => employeeIds.Any(y => y == x.Id));

                Dictionary<Employee, AttendanceForm> forms = GetAttendanceForms(employees.ToList());
                foreach (var item in notCheckedEntranceExitRecordsGroupedByEmp)
                {
                    var employeeCard = item.Key.EmployeeCard;
                    var days = item.Select(x => x.LogDate).Distinct();
                    foreach (var day in days)
                    {
                        var recurrence = GetWorkshopsRecurrenceInDate(employeeCard, new DateTime(day.Date.Year, day.Date.Month, day.Date.Day),
                            forms[item.Key], publicHolidays, fixedHolidays, changeableHolidays);
                        var ranges = recurrence.Workshop.Prepare(recurrence.Date).NormalShifts;

                        var employeeEntranceExitRecordsForthisDay = entranceExitRecords.ToList().Where(x => ranges.Any(y => x.LogDateTime >= y.ShiftRangeStartTime &&
                                                             x.LogDateTime <= y.ShiftRangeEndTime) && x.Employee.Id == item.Key.Id).ToList();
                        allEntranceExitRecords.AddRange(employeeEntranceExitRecordsForthisDay);
                        var preparedEntranceExitRecords = employeeEntranceExitRecordsForthisDay.Select(x => new { Record = x, IsFake = false, OrderBy = x.LogType == LogType.Entrance ? 1 : 2 }).OrderBy(x => x.Record.LogDateTime).ThenBy(x => x.OrderBy).ToList();
                        if (preparedEntranceExitRecords.Count() % 2 != 0)
                        {// اذا كانت سجلاتي فردية سأضيف سجل وهمي لتحقيق فكرة ازواج من السجلات
                            preparedEntranceExitRecords.Add(new { Record = preparedEntranceExitRecords.Last().Record, IsFake = true, OrderBy = 3 });
                        }

                        preparedEntranceExitRecords = preparedEntranceExitRecords.OrderBy(x => x.Record.LogDateTime).ThenBy(x => x.OrderBy).ToList();
                        for (int i = 0; i < preparedEntranceExitRecords.Count(); i += 2)
                        {
                            // اختبار كل زوجين
                            preparedEntranceExitRecords[i].Record.ErrorType = ErrorType.None;

                            preparedEntranceExitRecords[i + 1].Record.IsChecked = true;
                            preparedEntranceExitRecords[i].Record.IsChecked = true;
                            preparedEntranceExitRecords[i + 1].Record.ErrorType = ErrorType.None;
                            if (!preparedEntranceExitRecords[i + 1].IsFake)
                            {// اذا كان الزوج الثاني ليس وهمي اي ان سجلات عبارة عن أزواج بالتالي الاخطاء المتاحة هي التالية
                                if (preparedEntranceExitRecords[i].Record.LogType == LogType.Exit && preparedEntranceExitRecords[i + 1].Record.LogType == LogType.Entrance)
                                {
                                    preparedEntranceExitRecords[i].Record.ErrorType = ErrorType.EntranceTimeGreaterThanExitTime;
                                    preparedEntranceExitRecords[i + 1].Record.IsChecked = false;
                                    preparedEntranceExitRecords[i].Record.IsChecked = false;
                                    success = false;
                                    result.Add(new KeyValuePair<string, string>( preparedEntranceExitRecords[i].Record.Employee.FullName, AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.EntranceTimeGreaterThanExitTime)));
                                }
                                else if (preparedEntranceExitRecords[i].Record.LogType == LogType.Entrance && preparedEntranceExitRecords[i + 1].Record.LogType == LogType.Entrance)
                                {
                                    preparedEntranceExitRecords[i + 1].Record.ErrorType = ErrorType.MultipleEntrance;
                                    preparedEntranceExitRecords[i + 1].Record.IsChecked = false;
                                    preparedEntranceExitRecords[i].Record.IsChecked = false;
                                    success = false;
                                    result.Add(new KeyValuePair<string, string>(preparedEntranceExitRecords[i].Record.Employee.FullName, AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.MultipleEntrance)));
                                }
                                else if (preparedEntranceExitRecords[i].Record.LogType == LogType.Exit && preparedEntranceExitRecords[i + 1].Record.LogType == LogType.Exit)
                                {
                                    preparedEntranceExitRecords[i + 1].Record.ErrorType = ErrorType.MultipleExit;
                                    preparedEntranceExitRecords[i + 1].Record.IsChecked = false;
                                    preparedEntranceExitRecords[i].Record.IsChecked = false;
                                    success = false;
                                    result.Add(new KeyValuePair<string, string>(preparedEntranceExitRecords[i].Record.Employee.FullName, AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.MultipleExit)));
                                }
                            }
                            else
                            {
                                // أما اذا كان الزوج الثاني وهمي اي ان سجلات ليست أزواج بالتالي الاخطاء المتاحة هي التالية
                                if (preparedEntranceExitRecords[i].Record.LogType == LogType.Exit)
                                {
                                    preparedEntranceExitRecords[i].Record.ErrorType = ErrorType.ExitWithoutEntrance;
                                    preparedEntranceExitRecords[i + 1].Record.IsChecked = false;
                                    preparedEntranceExitRecords[i].Record.IsChecked = false;
                                    success = false;
                                    result.Add(new KeyValuePair<string, string>(preparedEntranceExitRecords[i].Record.Employee.FullName, AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.ExitWithoutEntrance)));
                                }
                                else if (preparedEntranceExitRecords[i].Record.LogType == LogType.Entrance)
                                {
                                    preparedEntranceExitRecords[i].Record.ErrorType = ErrorType.EntranceWithoutExit;
                                    preparedEntranceExitRecords[i + 1].Record.IsChecked = false;
                                    preparedEntranceExitRecords[i].Record.IsChecked = false;
                                    success = false;
                                    result.Add(new KeyValuePair<string, string>(preparedEntranceExitRecords[i].Record.Employee.FullName, AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.EntranceWithoutExit)));
                                }
                            }
                            entities.Add(preparedEntranceExitRecords[i].Record);
                            entities.Add(preparedEntranceExitRecords[i + 1].Record);
                        }

                        var firstShift = recurrence.Workshop.NormalShifts.OrderBy(x => x.NormalShiftOrder).FirstOrDefault();

                        var lastShift = recurrence.Workshop.NormalShifts.OrderByDescending(x => x.NormalShiftOrder).FirstOrDefault();
                        var outOfRangeRecords = entranceExitRecords.ToList().Where(x => ranges.Any(y => x.LogDateTime >= firstShift.ShiftRangeStartTime &&
                              x.LogDateTime <= lastShift.ShiftRangeEndTime) && x.Employee.Id == item.Key.Id && !employeeEntranceExitRecordsForthisDay.Any(z => z.Id == x.Id)).ToList();

                        outOfRangeRecords.ForEach(x =>
                        {
                            x.ErrorType = ErrorType.OutOfShiftRange;
                            x.IsChecked = false;
                            entities.Add(x);
                            success = false;
                            result.Add(new KeyValuePair<string, string>(string.Empty, AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.OutOfShiftRange)));
                        });
                    }

                }
                ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);

            }

            return result;
        }

        public static int DeleteFilteredEntranceExitRecords(IQueryable filteredEntranceExitRecords, bool withoutFilters)
        {
            var allEntranceExitRecordIds = new List<string>();
            var allEntranceExitRecord = (IEnumerable<EntranceExitRecord>)filteredEntranceExitRecords;
            if (!withoutFilters)
                allEntranceExitRecordIds = allEntranceExitRecord.Select(x => x.Id.ToString()).ToList();
            var result = DeleteFilteredEntranceExitWithRecordsWithFingerPrints(allEntranceExitRecordIds);
            if (result)
                return allEntranceExitRecord.Count();
            else
                return 0;
        }
        internal static bool DeleteFilteredEntranceExitWithRecordsWithFingerPrints(List<string> entranceExitIds)
        {
            using (var l_oConnection = new SqlConnection(System.Configuration.ConfigurationManager.
                                                   ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                    var entranceExitIdsStr = "";
                    var whereFingerprintTransferredDataStr = "";
                    var whereentranceExitStr = "";
                    if (entranceExitIds.Count > 0)
                    {
                        for (var i = 0; i < entranceExitIds.Count; i++)
                        {
                            if (i != entranceExitIds.Count - 1)
                                entranceExitIdsStr = entranceExitIdsStr + entranceExitIds[i] + ", ";
                            else
                                entranceExitIdsStr = entranceExitIdsStr + entranceExitIds[i];
                        }
                        whereFingerprintTransferredDataStr = " Where EntranceExitRecord_Id in (" + entranceExitIdsStr + ")";
                        whereentranceExitStr = " Where Id in (" + entranceExitIdsStr + ")";
                    }
                    l_oConnection.Open();

                    // Create a String to hold the query.
                    string queryFingerprintTransferredData = "DELETE FROM FingerprintTransferredData" + whereFingerprintTransferredDataStr;

                    string queryEntranceExitRecord = "DELETE FROM EntranceExitRecord" + whereentranceExitStr;

                    // Create a SqlCommand object and pass the constructor the connection string and the query string.
                    SqlCommand queryCommandFingerprintTransferredData = new SqlCommand(queryFingerprintTransferredData, l_oConnection);
                    sqlAdapter.DeleteCommand = queryCommandFingerprintTransferredData;
                    sqlAdapter.DeleteCommand.ExecuteNonQuery();
                    queryCommandFingerprintTransferredData.Dispose();
                    SqlCommand queryCommandEntranceExitRecord = new SqlCommand(queryEntranceExitRecord, l_oConnection);
                    sqlAdapter.DeleteCommand = queryCommandEntranceExitRecord;
                    sqlAdapter.DeleteCommand.ExecuteNonQuery();
                    queryCommandEntranceExitRecord.Dispose();

                    l_oConnection.Close();
                    return true;

                }
                catch (SqlException ex)
                {
                    return false;
                }
            }
        }
        public static bool CheckEntranceExitRecordDuplicate(List<EntranceExitRecord> EntranceExitRecordData,
            List<FingerprintTransferredData> allFingerprintTransferredData,
            DateTime time, InsertSource insertSource, LogType logType, int ignorePeriodMinutes,
            bool igoneTypeOfFingerPint = false)
        {
            var entranceExitRecord = EntranceExitRecordData
               .Where(x => x.LogType == logType && ((x.LogDateTime >= time.AddMinutes(-ignorePeriodMinutes)) && (x.LogDateTime <= time.AddMinutes(ignorePeriodMinutes))));
            if (igoneTypeOfFingerPint)
                entranceExitRecord = EntranceExitRecordData
               .Where(x => ((x.LogDateTime >= time.AddMinutes(-ignorePeriodMinutes)) && (x.LogDateTime <= time.AddMinutes(ignorePeriodMinutes))));
            return entranceExitRecord.Any();
        }

        #region Get Attendance System Settings
        public static AttendanceForm GetAttendanceForm(Employee employee, AttendanceForm GSAttendanceForm)
        {
            AttendanceForm attendanceForm = new AttendanceForm();
            try
            {
                AttendanceForm gradeAttendanceForm = null;
                var employeeCardAttendanceForm = employee.EmployeeCard?.AttendanceForm;
                var grade = GetGradeOfEmployee(employee);
                if (grade != null)
                    gradeAttendanceForm = grade.AttendanceForm;
                if (employeeCardAttendanceForm != null)
                    attendanceForm = employeeCardAttendanceForm;
                else if (gradeAttendanceForm != null)
                    attendanceForm = gradeAttendanceForm;
                else
                    attendanceForm = GSAttendanceForm;
                return attendanceForm;
            }
            catch (Exception e)
            {
                return attendanceForm;
            }
        }
        public static NonAttendanceForm GetLatenessForm(Employee employee, NonAttendanceForm GSLatenessForm)
        {
            NonAttendanceForm latenessForm = new NonAttendanceForm();
            try
            {
                NonAttendanceForm gradeLatenessForm = null;
                var employeeCardLatenessForm = employee.EmployeeCard?.LatenessForm;
                var grade = GetGradeOfEmployee(employee);
                if (grade != null)
                    gradeLatenessForm = grade.LatenessForm;
                if (employeeCardLatenessForm != null)
                    latenessForm = employeeCardLatenessForm;
                else if (gradeLatenessForm != null)
                    latenessForm = gradeLatenessForm;
                else
                    latenessForm = GSLatenessForm;

                return latenessForm;
            }
            catch (Exception e)
            {
                return latenessForm;
            }
        }
        public static OvertimeForm GetOvertimeForm(Employee employee, OvertimeForm GSOverTimeForm)
        {
            OvertimeForm overtimeForm = new OvertimeForm();
            try
            {
                OvertimeForm gradeOvertimeForm = null;
                var employeeCardOvertimeForm = employee.EmployeeCard?.OvertimeForm;
                var grade = GetGradeOfEmployee(employee);
                if (grade != null)
                    gradeOvertimeForm = grade.OvertimeForm;
                if (employeeCardOvertimeForm != null)
                    overtimeForm = employeeCardOvertimeForm;
                else if (gradeOvertimeForm != null)
                    overtimeForm = gradeOvertimeForm;
                else
                    overtimeForm = GSOverTimeForm;
                return overtimeForm;
            }
            catch (Exception e)
            {
                return overtimeForm;
            }
        }
        public static NonAttendanceForm GetAbsenceForm(Employee employee, NonAttendanceForm GSAbsenceForm)
        {
            NonAttendanceForm absenceForm = new NonAttendanceForm();
            try
            {
                NonAttendanceForm gradeAbsenceForm = null;
                var employeeCardAbsenceForm = employee.EmployeeCard?.AbsenceForm;
                var grade = GetGradeOfEmployee(employee);
                if (grade != null)
                    gradeAbsenceForm = grade.AbsenceForm;
                if (employeeCardAbsenceForm != null)
                    absenceForm = employeeCardAbsenceForm;
                else if (gradeAbsenceForm != null)
                    absenceForm = gradeAbsenceForm;
                else
                    absenceForm = GSAbsenceForm;
                return absenceForm;
            }
            catch (Exception e)
            {
                return absenceForm;
            }
        }
        #endregion


        public static Grade GetGradeOfEmployee(Employee employee)
        {
            try
            {
                if (employee != null)
                {
                    var employeeCardOvertimeForm = employee.EmployeeCard?.OvertimeForm;
                    if (employee.Positions.Where(x => x.IsPrimary == true).Any())
                    {
                        var _position = employee.Positions.Where(x => x.IsPrimary == true).FirstOrDefault();
                        var grade = _position.Position?.JobDescription?.JobTitle?.Grade;
                        if (grade != null)
                            return grade;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static void ResetNonAttendanceFormLastReset()
        {
            var nonAttendanceForms = ServiceFactory.ORMService.All<NonAttendanceForm>().ToList();
            foreach (var nonAttendanceForm in nonAttendanceForms)
            {
                if (
                    (nonAttendanceForm.NextReset.Year < DateTime.Now.Year) ||
                    (nonAttendanceForm.NextReset.Year == DateTime.Now.Year && nonAttendanceForm.NextReset.Month <= DateTime.Now.Month))
                {
                    nonAttendanceForm.LastReset = nonAttendanceForm.NextReset;
                    nonAttendanceForm.Save();
                }
            }
        }
    }

}