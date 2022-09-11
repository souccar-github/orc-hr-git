using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Services
{//todo : Mhd Update changeset no.1
    public static class AttendanceSystemIntegrationService
    {
        #region Get a Employee Missions
        public static bool IsEmployeeHasHourlyMission(Employee employee, IList<NormalShift> normalShifts, IList<HourlyMission> hourlyMissions)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل المهمة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            return GetEmployeeHourlyMissionDetails(employee, normalShifts, hourlyMissions).Any();
        }

        public static double GetEmployeeHourlyMission(Employee employee, IList<NormalShift> normalShifts, IList<HourlyMission> hourlyMissions)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل المهمة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            var result = normalShifts.Sum(normalShift => GetEmployeeHourlyMissionDetails(employee, normalShift, hourlyMissions).Sum(x => (x.EndDateTime - x.StartDateTime).TotalHours));
            return result;
        }

        public static List<HourlyMission> GetEmployeeHourlyMissionDetails(Employee employee, NormalShift normalShift, IList<HourlyMission> hourlyMissions)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل المهمة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            return GetEmployeeHourlyMissionDetails(employee, new List<NormalShift> { normalShift }, hourlyMissions);
        }

        public static List<HourlyMission> GetEmployeeHourlyMissionDetails(Employee employee, IList<NormalShift> normalShifts, IList<HourlyMission> hourlyMissions)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل المهمة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            var result = normalShifts.SelectMany(normalShift =>
                hourlyMissions
                .Where(x =>
                    x.Employee.Id == employee.Id &&
                    x.StartDateTime >= normalShift.EntryTime &&
                    x.StartDateTime < normalShift.ExitTime &&
                    x.EndDateTime > normalShift.EntryTime &&
                    x.EndDateTime <= normalShift.ExitTime)).ToList();
            return result;
        }

        public static bool IsEmployeeHasDailyMission(Employee employee, DateTime date, IList<TravelMission> travelMissions)
        {
            var travelMission =
                travelMissions.Any(x => x.FromDate <= date && x.ToDate >= date && x.Employee.Id == employee.Id);
            
            return travelMission;
        }

        #endregion

        #region Get a vacations an Holidays From ERS

        public static bool IsEmployeeHasHourlyVacation(Employee employee, NormalShift normalShift, IList<LeaveRequest> leaves)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل الاجازة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            return IsEmployeeHasHourlyVacation(employee, new List<NormalShift> { normalShift }, leaves);
        }
        public static bool IsEmployeeHasHourlyVacation(Employee employee, IList<NormalShift> normalShifts, IList<LeaveRequest> leaves)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل الاجازة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            return GetEmployeeHourlyVacationDetails(employee, normalShifts, leaves).Any();
        }

        public static double GetEmployeeHourlyVacation(Employee employee, NormalShift normalShift, IList<LeaveRequest> leaves)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل الاجازة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            return GetEmployeeHourlyVacation(employee, new List<NormalShift> { normalShift }, leaves);
        }

        public static double GetEmployeeHourlyVacation(Employee employee, IList<NormalShift> normalShifts, IList<LeaveRequest> leaves)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل الاجازة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            var result = normalShifts.Sum(normalShift => GetEmployeeHourlyVacationDetails(employee, normalShift, leaves).Sum(x => (x.ToDateTime.GetValueOrDefault() - x.FromDateTime.GetValueOrDefault()).TotalHours));
            return result;
        }

        public static List<LeaveRequest> GetEmployeeHourlyVacationDetails(Employee employee, NormalShift normalShift, IList<LeaveRequest> leaves)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل الاجازة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            return GetEmployeeHourlyVacationDetails(employee, new List<NormalShift> { normalShift }, leaves);
        }

        public static List<LeaveRequest> GetEmployeeHourlyVacationDetails(Employee employee, IList<NormalShift> normalShifts, IList<LeaveRequest> leaves)
        {//لان بحالة الوردية الممتدة على يومين مابيشتغل الكود الماضي يلي بيعتمد على تايخ واحد لان محتمل الاجازة تجي بالفترة الثانية يلي هي بتاريخ يوم زيادة عن تاريخ اليوم يلي كنا نمرقو
            var result = normalShifts.SelectMany(normalShift =>
                leaves
                .Where(x =>
                    x.EmployeeCard.Employee == employee &&
                    x.FromDateTime >= normalShift.EntryTime &&
                    x.FromDateTime < normalShift.ExitTime &&
                    x.ToDateTime > normalShift.EntryTime &&
                    x.ToDateTime <= normalShift.ExitTime)).ToList();
            return result;
        }
        public static bool IsEmployeeHasDailyVacation(Employee employee, DateTime date, IList<LeaveRequest> EmpLeave)
        {
            var EmpLeaveRequest = EmpLeave.Where(x => x.EmployeeCard.Employee == employee && x.StartDate <= date && x.EndDate >= date).ToList();
            if (EmpLeaveRequest.Count != 0)
                return true;
            return false;
        }


        public static void GetHolidays(out IList<PublicHoliday> publicHolidays, out IList<FixedHoliday> fixedHolidays, out IList<ChangeableHoliday> changeableHolidays)
        {
            publicHolidays = ServiceFactory.ORMService.All<PublicHoliday>().ToList(); 
            fixedHolidays = ServiceFactory.ORMService.All<FixedHoliday>().ToList();
            changeableHolidays = ServiceFactory.ORMService.All<ChangeableHoliday>().ToList();
        }

        public static bool IsHoliday(DateTime date, IList<PublicHoliday> publicHolidays, IList<FixedHoliday> fixedHolidays, IList<ChangeableHoliday> changeableHolidays)
        {
            foreach (var publicHoliday in publicHolidays)
                if ((int)publicHoliday.DayOfWeek == (int)date.DayOfWeek)
                    return true;

            foreach (var fixedHoliday in fixedHolidays)
                if ((int)date.Day == (int)fixedHoliday.Day && (int)date.Month == (int)fixedHoliday.Month)
                    return true;

            foreach (var changeableHoliday in changeableHolidays)
                if (date >= changeableHoliday.StartDate && date <= changeableHoliday.EndDate)
                    return true;


            return false;
        }
        

        #endregion

    }
}