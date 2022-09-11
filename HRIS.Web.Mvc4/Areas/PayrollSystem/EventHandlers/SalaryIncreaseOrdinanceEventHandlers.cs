using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class SalaryIncreaseOrdinanceEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.Views[0].EditHandler = "SalaryIncreaseOrdinance_EditHandler";
            model.SchemaFields.SingleOrDefault(x => x.Name == "Status").Editable = false;
            model.ActionListHandler = "initializeSalaryIncreaseOrdinanceActionList";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            //var salaryIncreaseOrdinance = (SalaryIncreaseOrdinance)entity;
            //if (salaryIncreaseOrdinance.IncreaseValue > 0 && salaryIncreaseOrdinance.IncreasePercentage > 0)
            //{
            //    validationResults.Add(new ValidationResult
            //    {
            //        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule
            //            .GetFullKey(CustomMessageKeysPayrollSystemModule.YouCanOnlySetIncreaseValueOrIncreasePercentage)),
            //        Property = typeof(SalaryIncreaseOrdinance).GetProperty("IncreaseValue")
            //    });
            //}
        }
    }
}