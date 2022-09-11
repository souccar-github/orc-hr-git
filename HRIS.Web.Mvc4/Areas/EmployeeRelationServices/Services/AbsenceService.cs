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
    public static class AbsenceService
    {
        public static int GetAbsence(DateTime startDate, DateTime endDate)
        {
            var absenceDays = 0;
            var days = (endDate - startDate).Days;

            for (var i = 0; i < days - 1; i++)
            {
                if (HolidayService.IsPublicHoliday(startDate.AddDays(i)))
                {
                    absenceDays++;
                    continue;
                }
                if (HolidayService.IsChangeableHoliday(startDate.AddDays(i)))
                {
                    absenceDays++;
                    continue;
                }
                if (HolidayService.IsFixedHoliday(startDate.AddDays(i)))
                    absenceDays++;

            }

            return absenceDays;
        }

        public static int GetEmployeeAbsence(Employee employee, DateTime startDate, DateTime endDate)
        {
            var absenceDays = 0;
            var days = (endDate - startDate).Days;

            for (var i = 0; i < days - 1; i++)
            {
               var date = startDate.AddDays(i);
               if (HolidayService.IsPublicHoliday(date))
               {
                   absenceDays++;
                    continue;
               }
               if (HolidayService.IsChangeableHoliday(date))
               {
                   absenceDays++;
                    continue;
               }
            
              if (HolidayService.IsFixedHoliday(date))
             {
                  absenceDays++;
                  continue;
              }
            }

            return absenceDays;
        }

    }
}