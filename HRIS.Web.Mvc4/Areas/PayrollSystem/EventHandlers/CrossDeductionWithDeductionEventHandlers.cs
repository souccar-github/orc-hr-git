using System;
using System.Collections.Generic;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Factories;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class CrossDeductionWithDeductionEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            GridViewModelFactory.AddRefField(model, "DeductionCard", "PayrollSystem/DropDownListHelper/GetPrimaryDeductionCards/");
            model.Views[0].EditHandler = "CrossDeductionWithDeduction_EditHandler";
            model.ViewModelTypeFullName = typeof(CrossDeductionWithDeductionEventHandlers).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var crossDeductionWithDeduction = (CrossDeductionWithDeduction) entity;
            if (crossDeductionWithDeduction.CrossType == CrossType.AsDefined)
            {
                crossDeductionWithDeduction.CrossFormula = CrossFormula.Nothing;
            }
            else if (crossDeductionWithDeduction.CrossType == CrossType.Custom && crossDeductionWithDeduction.CrossFormula == CrossFormula.Nothing)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotSelectCustomCrossTypeWithNothingCrossFormula)),
                    Property = typeof(CrossDeductionWithDeduction).GetProperty("CrossFormula")
                });
            }
        }
    }
}