
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using NHibernate.Criterion;
using NHibernate.Transform;
using Souccar.Infrastructure.Core;
using System;
using System.Linq;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Services
{
    public static class HolidayService
    {
        
        
        public static bool IsPublicHoliday(DateTime date)
        {
            return ServiceFactory.ORMService.All<PublicHoliday>().Any(x => x.DayOfWeek == date.DayOfWeek);
        }

        
        public static bool IsFixedHoliday(DateTime date)
        {
            return ServiceFactory.ORMService.All<FixedHoliday>().ToList().Any(x =>x.StartDate<=date&&x.EndDate>=date);
        }
        

        public static bool IsChangeableHoliday(DateTime date)
        {
            return ServiceFactory.ORMService.All<ChangeableHoliday>().Any(x => x.StartDate <= date && x.EndDate >= date);
        }

        public static bool IsRegularDate(int day, int month)
        {
            var monthDaysCount = DateTime.DaysInMonth(DateTime.Now.Year, month);
            if (day <= monthDaysCount)
                return true;

            return false;
        }

    }

}