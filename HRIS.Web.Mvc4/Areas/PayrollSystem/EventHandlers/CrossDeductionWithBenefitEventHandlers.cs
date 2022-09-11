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
    public class CrossDeductionWithBenefitEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            GridViewModelFactory.AddRefField(model, "DeductionCard", "PayrollSystem/DropDownListHelper/GetPrimaryDeductionCards/");
            //Call Method
            model.Views[0].EditHandler = "CrossDeductionWithBenefit_EditHandler";
            model.ViewModelTypeFullName = typeof(CrossDeductionWithBenefitEventHandlers).FullName;
          
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var crossDeductionWithBenefit = (CrossDeductionWithBenefit)entity;
            if (crossDeductionWithBenefit.CrossType == CrossType.AsDefined)
            {
                crossDeductionWithBenefit.CrossFormula = CrossFormula.Nothing;
            }
            else if (crossDeductionWithBenefit.CrossType == CrossType.Custom && crossDeductionWithBenefit.CrossFormula == CrossFormula.Nothing)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotSelectCustomCrossTypeWithNothingCrossFormula)),
                    Property = typeof(CrossDeductionWithBenefit).GetProperty("CrossFormula")
                });
            }
        }
    }
}