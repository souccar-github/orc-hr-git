using HRIS.Domain.EmployeeRelationServices.DTO;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Core;
using System;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.Enums;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Services
{
    public static class LeaveRequestService
    {


        #region // General //

        public static DateTime GetEndDate(FixedLeaveType leaveType, DateTime startDate, int requiredDays)
        {
            if (leaveType == FixedLeaveType.Hourly)
            {
                return startDate;
            }
            else if (leaveType == FixedLeaveType.Administrative)
            {
                var endDate = startDate;
                var i = 0;
                var holidayDays = 0;

                while (i < requiredDays)
                {
                    if (HolidayService.IsPublicHoliday(endDate) || HolidayService.IsChangeableHoliday(endDate) ||
                        HolidayService.IsFixedHoliday(endDate))
                        holidayDays++;
                    else
                        i++;
                        
                    endDate = endDate.AddDays(1);
                    
                }

                return startDate.AddDays(requiredDays + holidayDays - 1);
            }
            else
                return startDate.AddDays(requiredDays - 1);

        }
        public static bool IsSameCurrentYear(DateTime date)
        {
            if (date.Year == DateTime.Now.Year)
                return true;

            return false;
        }

        #endregion

    }

}