using System;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.DTO;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Services
{
    public static class EmployeeService
    {
        public static EmployeeServiceHistoryDTO GetYearsOfService(Employee employee, DateTime endDate)
        {
            var employeeServiceHistory = new EmployeeServiceHistoryDTO { Years = 0, Months = 0, Days = 0 };
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
            if (employeeCard != null)
            {
                var startWorkingDate = employeeCard.StartWorkingDate ?? DateTime.Today;
                if (endDate.Year < startWorkingDate.Year)
                    return employeeServiceHistory;
                var yearsDifference = endDate.Year - startWorkingDate.Year;
                var monthsDifference = endDate.Month - startWorkingDate.Month;
                var daysDifference = endDate.Day - startWorkingDate.Day;

                if (yearsDifference >= 0)
                    employeeServiceHistory.Years = endDate.Year - startWorkingDate.Year;

                if (monthsDifference >= 0)
                    employeeServiceHistory.Months = endDate.Month - startWorkingDate.Month;
                else
                {
                    if (employeeServiceHistory.Years > 0)
                        employeeServiceHistory.Years--;
                    employeeServiceHistory.Months = 12 - (startWorkingDate.Month - endDate.Month);
                }

                if (daysDifference >= 0)
                    employeeServiceHistory.Days = endDate.Day - startWorkingDate.Day;
                else
                {
                    if (employeeServiceHistory.Months > 0)
                        employeeServiceHistory.Months--;
                    else
                    {
                        if (employeeServiceHistory.Years > 0)
                        {
                            employeeServiceHistory.Years--;
                            employeeServiceHistory.Months = 11;
                        }
                    }
                    employeeServiceHistory.Days =
                            DateTime.DaysInMonth(startWorkingDate.Year, startWorkingDate.Month) - (startWorkingDate.Day - endDate.Day);
                }
              
            }
            return employeeServiceHistory;
        }

    }
}