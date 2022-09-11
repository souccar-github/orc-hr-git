using System;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Extensions;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Services
{
    public static class GeneralService
    {
        public static bool IsEmployeeHasBankAccount(Employee employee)
        {
            if (employee == null)
            {
                return false;
            }
            var primaryCard =
                typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            return primaryCard != null && primaryCard.BankingInformations.Any();
        }

        public static DateTime? EmployeeStartDate(Employee employee)
        {
           //return IncidenceDefinitionService.GetStartDate(employee);
            if (employee == null)
            {
                return null;
            }
            var primaryCard =
                typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            return primaryCard.StartWorkingDate;

            //var appointmentInformations = employee.IncidenceDefinitions.Where(x => x.Type == IncidenceType.AssignEmployee).
            //    OrderByDescending(x => x.StartDate).FirstOrDefault();

            //var hireDate = appointmentInformations == null ? (DateTime?)null : appointmentInformations.StartDate;
            //return hireDate;
        }
        //public static DateTime? EmployeeStartDate(int employeeId)
        //{
        //    var employee = (Employee)typeof (Employee).GetById(employeeId);
        //    return EmployeeStartDate(employee);
        //}

        public static DateTime? EmployeeHireDate(Employee employee)
        {
            return null;
        }

        public static DateTime? EmployeeHireDate(int employeeId)
        {
            var employee = (Employee)typeof(Employee).GetById(employeeId);
            return EmployeeHireDate(employee);
        }

        public static DateTime? EmployeeAbruptionDate(EmployeeCard primaryCard)
        {
            if (primaryCard != null)
            {
                var employeeResignationsLastOrDefault = primaryCard.EmployeeResignations.LastOrDefault();
                var employeeTerminationsLastOrDefault = primaryCard.EmployeeTerminations.LastOrDefault();
                if (employeeResignationsLastOrDefault != null && employeeTerminationsLastOrDefault!=null)
                    return employeeResignationsLastOrDefault.LastWorkingDate > employeeTerminationsLastOrDefault.LastWorkingDate ?
                    employeeResignationsLastOrDefault.LastWorkingDate : employeeTerminationsLastOrDefault.LastWorkingDate;

                return employeeResignationsLastOrDefault != null
                    ? employeeResignationsLastOrDefault.LastWorkingDate
                    : employeeTerminationsLastOrDefault != null ? employeeTerminationsLastOrDefault.LastWorkingDate : (DateTime?)null;
            }
            return null;
        }

        //public static double? GetEmploymentMonths(Employee employee)
        //{
        //    var hireDate = EmployeeHireDate(employee);
        //    if (hireDate.HasValue)
        //    {
        //        return 12 * (DateTime.Now.Year - hireDate.Value.Year) + (DateTime.Now.Month - hireDate.Value.Month);
        //    }
        //    return null;
        //}

       
        public static DateTime? GetChildProfitStartDate(Child child)
        {

            return child.ChildBenefitStartDate;
        }

        public static DateTime? GetChildProfitStartDate(int childId)
        {
            var child = typeof(Employee).GetAll<Employee>()
                .First(x => x.Children.Any(y => y.Id == childId))
                .Children.First(x => x.Id == childId);
            return GetChildProfitStartDate(child);
        }

        public static DateTime? GetSpouseProfitStartDate(Spouse spouse)
        {//todo: مطلوب طريقة لمعرفة تاريخ استحقاق الزوجة للنفقات

            return spouse.DateOfFamilyBenefitActivation;

        }
        public static DateTime? GetSpouseProfitStartDate(int spouseId)
        {
            var spouse = typeof(Employee).GetAll<Employee>()
                .First(x => x.Spouse.Any(y => y.Id == spouseId))
                .Spouse.First(x => x.Id == spouseId);
            return GetSpouseProfitStartDate(spouse);
        }

        public static double GetEmployeeCategoryMaxCeil(Employee employee)
        {
            var primaryPosition = employee.PrimaryPosition();
            return
                primaryPosition != null
                ? primaryPosition.JobDescription.JobTitle.Grade.MaxSalary
                : 0;
        }
    }
}