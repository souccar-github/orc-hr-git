using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.Entities;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Services
{
    public static class LeaveService
    {
        public static double GetBalance(LeaveSetting leaveSetting, Employee employee, bool IsForRecycle, DateTime endDate)
        {
            double balance = 0;
            var employeeServiceHistory = EmployeeService.GetYearsOfService(employee, endDate);
            if (employeeServiceHistory.Years == 0 && employeeServiceHistory.Months == 0 && employeeServiceHistory.Days == 0)
                return 0.00;
            var yearsOfService = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            var monthsOfService = employeeServiceHistory.Months;

            var DiffBetweenMonthsOfNowAndMonthsOfServ = monthsOfService - (endDate.Month - 1);
            if (IsForRecycle)
            {
                ///here not important to to  diff so diff had assigned to -1
                DiffBetweenMonthsOfNowAndMonthsOfServ = -1;
                if (employeeCard.StartWorkingDate.Value.Year == (endDate.Year - 1) && employeeServiceHistory.Years > 0)
                    yearsOfService = employeeServiceHistory.Years - 1;
                else
                    yearsOfService = employeeServiceHistory.Years;
            }
            else
            {
                yearsOfService = employeeServiceHistory.Years;
            }


            //var leaveSetting = ServiceFactory.ORMService.All<LeaveSetting>().FirstOrDefault(x => x.Type == leaveType);
            if (leaveSetting != null)
            {
                if (leaveSetting.BalanceSlices != null && leaveSetting.BalanceSlices.Count > 0)
                {
                    var leaveSettingBalanceSlice = leaveSetting.BalanceSlices.FirstOrDefault(
                        x => x.FromYearOfServices <= yearsOfService && x.ToYearOfServices > yearsOfService);

                    balance = leaveSettingBalanceSlice != null ? leaveSettingBalanceSlice.Balance : leaveSetting.Balance;
                    if (yearsOfService != 0 && leaveSettingBalanceSlice != null && yearsOfService == leaveSettingBalanceSlice.FromYearOfServices && leaveSetting.IsAffectedByStartWorkingDate)
                    {
                        var meritDate = endDate.AddMonths(employeeServiceHistory.Months * -1);
                        meritDate = meritDate.AddDays(employeeServiceHistory.Days * -1);
                        if (endDate.Year == meritDate.Year)
                        {
                            var remainMonths = 0.0;
                            if (DateTime.IsLeapYear(meritDate.Year))
                            {
                                remainMonths = Math.Round(new DateTime(meritDate.Year, 12, 31).Subtract(meritDate).Days / (366.00 / 12));
                            }
                            else
                            {
                                remainMonths = Math.Round(new DateTime(meritDate.Year, 12, 31).Subtract(meritDate).Days / (365.00 / 12));
                            }
                            var pastSlice = leaveSetting.BalanceSlices.FirstOrDefault(x => x.ToYearOfServices == leaveSettingBalanceSlice.FromYearOfServices);
                            var addedBalance = leaveSettingBalanceSlice.Balance - pastSlice.Balance;
                            var thisYearAddedBalnce = addedBalance * remainMonths / 12;
                            balance = pastSlice.Balance + thisYearAddedBalnce;
                        }
                    }
                }
                else
                    balance = leaveSetting.Balance;


                var startWorkingDate = employeeCard.StartWorkingDate.Value;
                if (leaveSetting.IsAffectedByAssigningDate && yearsOfService == 0 && endDate.Year == startWorkingDate.Year)
                {
                    if (employeeCard != null)
                    {
                        if (startWorkingDate == null)
                        {
                            startWorkingDate = DateTime.Today;
                        }
                        if (DateTime.IsLeapYear(startWorkingDate.Year))
                        {
                            balance = Math.Round((new DateTime(startWorkingDate.Year, 12, 31).Subtract(startWorkingDate).Days - 1) / (366.00 / 12)) * balance / 12;
                        }
                        else
                        {
                            balance = Math.Round((new DateTime(startWorkingDate.Year, 12, 31).Subtract(startWorkingDate).Days - 1) / (365.00 / 12)) * balance / 12;
                        }
                    }
                }
            }

            return balance;
        }

        public static double GetMonthlyBalance(LeaveSetting leaveSetting, Employee employee, DateTime startDate)
        {
            double monthlyBalance = 0;
            var employeeServiceHistory = EmployeeService.GetYearsOfService(employee, DateTime.Today);
            var yearsOfService = employeeServiceHistory.Years;
            //var leaveSetting = ServiceFactory.ORMService.All<LeaveSetting>().FirstOrDefault(x => x.Type == leaveType);
            if (leaveSetting != null)
            {
                if (leaveSetting.BalanceSlices != null && leaveSetting.BalanceSlices.Count > 0)
                {
                    var leaveSettingBalanceSlice = leaveSetting.BalanceSlices.FirstOrDefault(
                        x => x.FromYearOfServices <= yearsOfService && x.ToYearOfServices > yearsOfService);
                    if (leaveSettingBalanceSlice != null && leaveSettingBalanceSlice.HasMonthlyBalance)
                    {
                        monthlyBalance = leaveSettingBalanceSlice.MonthlyBalance;
                        var maximumBalance = leaveSettingBalanceSlice.MaximumRoundedLeaveDays;
                        var leavesInYear = GetLeavesOfYear(startDate, leaveSetting, employee);
                        var remainLeavesInMonth = monthlyBalance * startDate.Month - leavesInYear;
                        monthlyBalance = maximumBalance == 0 ? monthlyBalance : remainLeavesInMonth >= maximumBalance ? maximumBalance : remainLeavesInMonth;
                    }
                }
                else
                {
                    if (leaveSetting.HasMonthlyBalance)
                    {
                        monthlyBalance = leaveSetting.MonthlyBalance;
                        var maximumBalance = leaveSetting.MaximumRoundedLeaveDays;
                        var leavesInYear = GetLeavesOfYear(startDate, leaveSetting, employee);
                        var remainLeavesInMonth = monthlyBalance * startDate.Month - leavesInYear;
                        monthlyBalance = maximumBalance == 0 ? monthlyBalance : remainLeavesInMonth >= maximumBalance ? maximumBalance : remainLeavesInMonth;
                    }
                }

            }

            return monthlyBalance;
        }

        public static double GetLeavesOfYear(DateTime date, LeaveSetting leaveSetting, Employee employee)
        {
            var empCard = employee.EmployeeCard;
            var leavesInYear = ServiceFactory.ORMService.All<LeaveRequest>()
                .Where(x => x.LeaveSetting == leaveSetting && x.LeaveStatus == Status.Approved && x.StartDate.Year == date.Year && x.EndDate.Year == date.Year && x.StartDate.Month < date.Month && x.EmployeeCard == empCard)
                .ToList();
            var leaves = leavesInYear.Count() > 0 ? leavesInYear.Select(x => x.SpentDays).Sum() : 0.0;
            var endYearLeaves = ServiceFactory.ORMService.All<LeaveRequest>()
                .Where(x => x.LeaveSetting == leaveSetting && x.LeaveStatus == Status.Approved && x.StartDate.Year == date.Year && x.EndDate.Year != date.Year && x.StartDate.Month < date.Month && x.EmployeeCard == empCard)
                .ToList()
                .Select(x => new DateTime(date.Year, 12, 31).Subtract(x.StartDate).TotalDays);
            var startYearLeaves = ServiceFactory.ORMService.All<LeaveRequest>()
                .Where(x => x.LeaveSetting == leaveSetting && x.LeaveStatus == Status.Approved && x.StartDate.Year != date.Year && x.EndDate.Year == date.Year && x.StartDate.Month < date.Month && x.EmployeeCard == empCard)
                .ToList()
                .Select(x => x.EndDate.Subtract(new DateTime(date.Year, 1, 1)).TotalDays);
            leaves += startYearLeaves.Count() > 0 ? startYearLeaves.Sum() : 0.0;
            leaves += endYearLeaves.Count() > 0 ? endYearLeaves.Sum() : 0.0;

            return leaves;

        }

        public static bool HasMonthlyBalance(LeaveSetting leaveSetting, Employee employee)
        {
            bool hasMonthlyBalance = false;
            var employeeServiceHistory = EmployeeService.GetYearsOfService(employee, DateTime.Today);
            var yearsOfService = employeeServiceHistory.Years;
            //var leaveSetting = ServiceFactory.ORMService.All<LeaveSetting>().FirstOrDefault(x => x.Type == leaveType);
            if (leaveSetting != null)
            {
                if (leaveSetting.BalanceSlices != null && leaveSetting.BalanceSlices.Count > 0)
                {
                    var leaveSettingBalanceSlice = leaveSetting.BalanceSlices.FirstOrDefault(
                        x => x.FromYearOfServices <= yearsOfService && x.ToYearOfServices > yearsOfService);
                    if (leaveSettingBalanceSlice != null && leaveSettingBalanceSlice.HasMonthlyBalance)
                        hasMonthlyBalance = true;
                }
                else
                {
                    if (leaveSetting.HasMonthlyBalance)
                        hasMonthlyBalance = true;
                }
            }

            return hasMonthlyBalance;
        }

        public static double GetRecycledBalance(Employee employee, LeaveSetting leaveSetting, int year)
        {
            double recycledBalance = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.RecycledLeaves != null && employeeCard.RecycledLeaves.Count > 0)
            {
                var recycledLeave = employeeCard.RecycledLeaves.FirstOrDefault(x => x.LeaveSetting.Type == leaveSetting.Type && x.Year == year
                    && x.RecycleType == RecycleType.Balance);
                if (recycledLeave != null)
                    recycledBalance = recycledLeave.RoundedBalance;
            }

            return recycledBalance;
        }

        public static double GetGranted(Employee employee, LeaveSetting leaveSetting, int year)
        {
            double granted = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.LeaveRequests != null && employeeCard.LeaveRequests.Count > 0)
            {
                var leaveRequests = GetLeaveRequestsInYear(employeeCard, leaveSetting, year);
                granted += leaveRequests.Sum(leaveRequest => GetDaysCountInLeave(leaveRequest, year));
            }
            granted = Math.Round(granted, 2);
            return granted;
        }
        public static double GetGranted(Employee employee, LeaveSetting leaveSetting)
        {
            double granted = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.LeaveRequests != null && employeeCard.LeaveRequests.Count > 0)
            {
                var leaveRequests = GetLeaveRequests(employeeCard, leaveSetting);
                granted += leaveRequests.Count * leaveSetting.Balance;
            }
            granted = Math.Round(granted, 2);
            return granted;
        }
        public static double GetMonthlyGranted(Employee employee, LeaveSetting leaveSetting, DateTime date)
        {
            double monthlyGranted = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.LeaveRequests != null && employeeCard.LeaveRequests.Count > 0)
            {
                var leaveRequests = GetLeaveRequestsInMonth(employeeCard, leaveSetting, new DateTime(date.Year, date.Month, date.Day));
                monthlyGranted += leaveRequests.Sum(leaveRequest => GetMonthlyDaysCountInLeave(leaveRequest));

            }

            return monthlyGranted;
        }

        public static List<LeaveRequest> GetLeaveRequestsInYear(EmployeeCard employeeCard, LeaveSetting leaveSetting, int year)
        {
            var firstDay = new DateTime(year, 1, 1);
            var lastDay = new DateTime(year, 12, 31);
            return
                employeeCard.LeaveRequests.Where(
                    x =>
                        (x.LeaveSetting.Type == leaveSetting.Type) && ((x.StartDate >= firstDay && x.StartDate <= lastDay) ||
                        (x.EndDate >= firstDay && x.EndDate <= lastDay) ||
                        (x.StartDate <= firstDay && x.EndDate >= lastDay)) && ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft))).ToList();
        }
        public static List<LeaveRequest> GetLeaveRequests(EmployeeCard employeeCard, LeaveSetting leaveSetting)
        {
            return
                employeeCard.LeaveRequests.Where(
                    x =>
                        (x.LeaveSetting.Type == leaveSetting.Type) && ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft))).ToList();
        }
        public static List<LeaveRequest> GetLeaveRequestsInMonth(EmployeeCard employeeCard, LeaveSetting leaveSetting, DateTime date)
        {
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            return
                employeeCard.LeaveRequests.Where(
                    x =>
                        ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&
                        x.LeaveSetting.Type == leaveSetting.Type && ((x.StartDate >= firstDay && x.StartDate <= lastDay) ||
                        (x.EndDate >= firstDay && x.EndDate <= lastDay) ||
                        (x.StartDate <= firstDay && x.EndDate >= lastDay))).ToList();
        }
        public static List<LeaveRequest> GetLeaveRequestsInMonth(EmployeeCard employeeCard, DateTime date)
        {
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            return
                employeeCard.LeaveRequests.Where(
                    x =>
                        ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&
                        ((x.StartDate >= firstDay && x.StartDate <= lastDay) ||
                         (x.EndDate >= firstDay && x.EndDate <= lastDay) ||
                         (x.StartDate <= firstDay && x.EndDate >= lastDay))).ToList();
        }

        public static double GetDaysCountInLeave(LeaveRequest leaveRequest, int year)
        {
            var firstDay = new DateTime(year, 1, 1);
            var lastDay = new DateTime(year, 12, 31);
            if (leaveRequest.StartDate.Year == leaveRequest.EndDate.Year)
                return leaveRequest.SpentDays;
            else if (leaveRequest.StartDate.Year == year)
            {
                return (lastDay - leaveRequest.StartDate).TotalDays;
            }
            else if (leaveRequest.EndDate.Year == year)
            {
                return (leaveRequest.EndDate - firstDay).TotalDays;
            }
            else
            {
                return firstDay.DayOfYear;
            }
        }
        public static double GetMonthlyDaysCountInLeave(LeaveRequest leaveRequest)
        {
            if (leaveRequest.StartDate.Month == leaveRequest.EndDate.Month)
                return leaveRequest.SpentDays;

            return 0;
        }

        public static DateTime GetEndDate(DateTime startDate, double days, bool isContinues, Employee employee)
        {
            GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            var attendanceForm = Project.Web.Mvc4.Areas.AttendanceSystem.Services.AttendanceService.GetAttendanceForm(employee, generalSetting.AttendanceForm);
            IList<WorkshopRecurrence> recurrences = attendanceForm != null && attendanceForm.WorkshopRecurrences.Any() ? attendanceForm.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList() : new List<WorkshopRecurrence>();
            var endDate = startDate;

            if (isContinues)
                endDate = startDate.AddDays(days - 1);
            else
            {
                var date = startDate;
                var i = 0;
                var holidayDays = 0;

                while (i < days)
                {
                    if ((HolidayService.IsPublicHoliday(date) || HolidayService.IsChangeableHoliday(date) ||
                        HolidayService.IsFixedHoliday(date)) && (attendanceForm != null ? attendanceForm.RelyHolidaies : true))
                        holidayDays++;
                    else
                    {
                        if ((attendanceForm != null ? !attendanceForm.RelyHolidaies : true) && recurrences.Count == 7)
                            if (recurrences[(int)date.DayOfWeek].IsOff)
                                holidayDays++;
                            else
                                i++;
                    }

                    date = date.AddDays(1);
                }

                endDate = startDate.AddDays(days + holidayDays - 1);
            }

            return endDate;
        }

        public static double GetSpentDays(DateTime startDate, DateTime endDate, bool isContinues, Employee employee)
        {
            GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            var publicHolidays = ServiceFactory.ORMService.All<PublicHoliday>().ToList();
            var changableHolidays = ServiceFactory.ORMService.All<ChangeableHoliday>().ToList();
            var fixedHolidays = ServiceFactory.ORMService.All<FixedHoliday>().ToList();
            var attendanceForm = Project.Web.Mvc4.Areas.AttendanceSystem.Services.AttendanceService.GetAttendanceForm(employee, generalSetting.AttendanceForm);
            return GetSpentDays(attendanceForm, startDate, endDate, isContinues, publicHolidays, fixedHolidays, changableHolidays);
        }

        public static double GetSpentDays(AttendanceForm attendanceForm, DateTime startDate, DateTime endDate, bool isContinues,
            List<PublicHoliday> publicHolidays, List<FixedHoliday> fixedHolidays, List<ChangeableHoliday> changableHolidays)
        {
            IList<WorkshopRecurrence> recurrences = attendanceForm != null ? attendanceForm.WorkshopRecurrences.OrderBy(x => x.RecurrenceOrder).ToList() : new List<WorkshopRecurrence>();
            double spentDays = 0;

            if (isContinues)
            {
                while (DateTime.Parse(startDate.ToShortDateString()) <= DateTime.Parse(endDate.ToShortDateString()))
                {
                    spentDays++;
                    startDate = startDate.AddDays(1);
                }
            }
            else
            {
                while (DateTime.Parse(startDate.ToShortDateString()) <= DateTime.Parse(endDate.ToShortDateString()))
                {
                    if ((attendanceForm != null ? !attendanceForm.RelyHolidaies : true) ||
                        (!publicHolidays.Any(x => x.DayOfWeek == startDate.DayOfWeek) &&
                         !fixedHolidays.Any(x => x.StartDate <= startDate && x.EndDate >= startDate) &&
                         !changableHolidays.Any(x => x.StartDate <= startDate && x.EndDate >= startDate)))
                    {
                        if ((attendanceForm != null ? !attendanceForm.RelyHolidaies : true) && recurrences.Count == 7)
                        {
                            if (!recurrences[(int)startDate.DayOfWeek].IsOff)
                            {
                                spentDays++;
                            }
                        }
                        else
                            spentDays++;
                    }

                    startDate = startDate.AddDays(1);
                }
            }

            return spentDays;
        }

        public static int GetCountInYears(Employee employee, LeaveSetting leaveSetting)
        {
            var countInYears = 0;
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null && employeeCard.LeaveRequests != null && employeeCard.LeaveRequests.Count > 0)
                countInYears = employeeCard.LeaveRequests.Count(x => x.LeaveStatus != Status.Rejected && x.LeaveSetting.Type == leaveSetting.Type);

            return countInYears;
        }

        public static bool IsValidIntervalDays(DateTime requestDate, DateTime startDate, int intervalDays)
        {
            if (intervalDays > 0)
            {
                if ((DateTime.Parse(startDate.ToLongDateString()) - DateTime.Parse(requestDate.ToLongDateString())).Days < intervalDays)
                    return false;
            }

            return true;
        }

        public static bool IsValidLeaveDate(EmployeeCard employeeCard, LeaveSetting leaveSetting, DateTime stratDate, DateTime endDate,
            LeaveRequest leaveRequestBeforeUpdate = null)
        {
            if (leaveRequestBeforeUpdate != null)
            {
                return !employeeCard.LeaveRequests.Where(x => x != leaveRequestBeforeUpdate).Any(x =>
                    ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&
                   ((stratDate >= DateTime.Parse(x.StartDate.ToShortDateString()) && stratDate <= DateTime.Parse(x.EndDate.ToShortDateString())) ||
                    (endDate >= DateTime.Parse(x.StartDate.ToShortDateString()) && endDate <= DateTime.Parse(x.EndDate.ToShortDateString()))));
            }
            else
            {
                return !employeeCard.LeaveRequests.Any(x =>
                    ((x.LeaveStatus == Status.Approved) || (x.LeaveStatus == Status.Draft)) &&
                   (((stratDate >= DateTime.Parse(x.StartDate.ToShortDateString()) && stratDate <= DateTime.Parse(x.EndDate.ToShortDateString())) ||
                    (endDate >= DateTime.Parse(x.StartDate.ToShortDateString()) && endDate <= DateTime.Parse(x.EndDate.ToShortDateString())))));
            }
        }

        public static bool IsHourlyLeaveValidLeave(EmployeeCard employeeCard, LeaveSetting leaveSetting, DateTime stratDate,
            TimeSpan stratTime, TimeSpan endTime, LeaveRequest leaveRequest, LeaveRequest leaveRequestBeforeUpdate = null)
        {
            var dailyleaves = employeeCard.LeaveRequests.Where(x => x.IsHourlyLeave == false && leaveRequest.StartDate >= x.StartDate && leaveRequest.EndDate <= x.EndDate).ToList().SingleOrDefault();
            if (dailyleaves != null)
            {
                return false;
            }
            GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            var attendance = AttendanceSystem.Services.AttendanceService.GetAttendanceForm(employeeCard.Employee, generalSetting.AttendanceForm);
            var shiftslist = attendance.WorkshopRecurrences.SelectMany(x => x.Workshop == null ? new List<NormalShift>() : x.Workshop.NormalShifts.ToList()).ToList();

            bool isValid = false;
            foreach (var item in shiftslist)
            {
                isValid = true;
                var fromTime = new DateTime(2000, 1, 1, leaveRequest.FromTime.Value.Hour, leaveRequest.FromTime.Value.Minute, leaveRequest.FromTime.Value.Second);
                var toTime = new DateTime(2000, 1, 1, leaveRequest.ToTime.Value.Hour, leaveRequest.ToTime.Value.Minute, leaveRequest.ToTime.Value.Second);
                leaveRequest.FromTime = fromTime;
                leaveRequest.ToTime = toTime;

                leaveRequest.FromDateTime = new DateTime(leaveRequest.StartDate.Year, leaveRequest.StartDate.Month,
                    leaveRequest.StartDate.Day, leaveRequest.FromTime.Value.Hour, leaveRequest.FromTime.Value.Minute,
                    leaveRequest.FromTime.Value.Second);

                leaveRequest.ToDateTime = new DateTime(leaveRequest.EndDate.Year, leaveRequest.EndDate.Month,
                leaveRequest.EndDate.Day, leaveRequest.ToTime.Value.Hour, leaveRequest.ToTime.Value.Minute,
                leaveRequest.ToTime.Value.Second);

                var entrytime = new DateTime(leaveRequest.FromDateTime.Value.Year, leaveRequest.FromDateTime.Value.Month,
                        leaveRequest.FromDateTime.Value.Day, item.EntryTime.Hour, item.EntryTime.Minute,
                        item.EntryTime.Second);
                var exittime = new DateTime(leaveRequest.ToDateTime.Value.Year, leaveRequest.ToDateTime.Value.Month,
                        leaveRequest.ToDateTime.Value.Day, item.ExitTime.Hour, item.ExitTime.Minute,
                        item.ExitTime.Second);

                

                if (leaveRequest.FromDateTime.Value < entrytime || leaveRequest.FromDateTime.Value > exittime)
                {
                    isValid = false;
                }
                else if (leaveRequest.ToDateTime.Value < entrytime || leaveRequest.ToDateTime.Value > exittime)
                {
                    isValid = false;
                }

                if (isValid)
                    break;
            }


            //التأكد من عدم التقاطع بين وقتين
            if (leaveRequestBeforeUpdate != null)
            {

                foreach (var item in employeeCard.LeaveRequests)
                {
                    if (leaveRequestBeforeUpdate != item)
                    {
                        if (item.IsHourlyLeave)
                        {
                            if (leaveRequest.FromDateTime.Value >= item.FromDateTime.Value && leaveRequest.FromDateTime.Value <= item.ToDateTime.Value)
                            {
                                return false;
                            }
                            else if (leaveRequest.ToDateTime.Value >= item.FromDateTime.Value && leaveRequest.ToDateTime.Value <= item.ToDateTime.Value)
                            {
                                return false;
                            }
                            else if (leaveRequest.FromDateTime.Value <= item.FromDateTime.Value && leaveRequest.ToDateTime.Value >= item.ToDateTime.Value)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                foreach (var item in employeeCard.LeaveRequests)
                {
                    if (item.IsHourlyLeave)
                    {
                        if (leaveRequest.FromDateTime.Value >= item.FromDateTime.Value && leaveRequest.FromDateTime.Value <= item.ToDateTime.Value)
                        {
                            return false;
                        }
                        else if (leaveRequest.ToDateTime.Value >= item.FromDateTime.Value && leaveRequest.ToDateTime.Value <= item.ToDateTime.Value)
                        {
                            return false;
                        }
                        else if (leaveRequest.FromDateTime.Value <= item.FromDateTime.Value && leaveRequest.ToDateTime.Value >= item.ToDateTime.Value)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

        }

    }
}