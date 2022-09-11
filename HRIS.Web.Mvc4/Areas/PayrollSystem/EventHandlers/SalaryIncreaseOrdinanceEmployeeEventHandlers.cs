using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.PayrollSystem.Entities;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class SalaryIncreaseOrdinanceEmployeeEventHandlers:ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(SalaryIncreaseOrdinanceEmployeeEventHandlers).FullName;
            model.Views[0].EditHandler = "SalaryIncreaseOrdinanceEmployee_EditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,Entity parententity = null)
        {
            var salaryIncreaseOrdinanceEmployee = (SalaryIncreaseOrdinanceEmployee)entity;
         
            salaryIncreaseOrdinanceEmployee.SalaryBeforeIncrease = salaryIncreaseOrdinanceEmployee.PrimaryCard.FinancialCard.Salary;
            salaryIncreaseOrdinanceEmployee.SalaryAfterIncrease = 0;

            var EmployeeSalaryIncreased = ServiceFactory.ORMService.All<SalaryIncreaseOrdinanceEmployee>()
                .Where(e=>e.PrimaryCard.Id == salaryIncreaseOrdinanceEmployee.PrimaryCard.Id).FirstOrDefault();
           
                if (EmployeeSalaryIncreased !=null)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.TheSalaryForThisEmployeeHasBeenAlreadyIncreased)),
                        Property = null
                    });
                    return;
                }
            }
        }
    }


