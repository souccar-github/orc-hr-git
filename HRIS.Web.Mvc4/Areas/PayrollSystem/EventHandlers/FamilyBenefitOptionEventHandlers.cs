using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class FamilyBenefitOptionEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(FamilyBenefitOptionEventHandlers).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            if (entity.IsTransient())
            {
                var recordCount = typeof(FamilyBenefitOption).GetAll<FamilyBenefitOption>().Count();
                if (recordCount >= 1)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.MoreThanOneRowNotAllowed)),
                        Property = null
                    });
                }
            }
        }
    }
}