using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Services
{
    public static class AttendanceService
    {
        public static int GetAttendance(DateTime startDate, DateTime endDate)
        {
            var absenceDays = AbsenceService.GetAbsence(startDate, endDate);
            var days = (endDate - startDate).Days;
            
            return days - absenceDays;
        }

        public static int GetEmployeeAttendance(Employee employee, DateTime startDate, DateTime endDate)
        {
            var absenceDays = AbsenceService.GetEmployeeAbsence(employee, startDate, endDate);
            var days = (endDate - startDate).Days;
            
            return days - absenceDays;
        }

    }
}