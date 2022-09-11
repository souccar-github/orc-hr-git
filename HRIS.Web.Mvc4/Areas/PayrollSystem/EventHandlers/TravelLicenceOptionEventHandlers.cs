using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Core.Utilities;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PayrollSystem.Configurations;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class TravelLicenceOptionEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TravelLicenceOptionEventHandlers).FullName;
            model.Views[0].EditHandler = "TravelLicenceOption_EditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            if (entity.IsTransient())
            {
                var recordCount = typeof(TravelLicenceOption).GetAll<TravelLicenceOption>().Count();
                if (recordCount >= 1)
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule
                            .GetFullKey(CustomMessageKeysPayrollSystemModule.MoreThanOneRowNotAllowed)),
                        Property = null
                    });
                }
            }
            var travelLicenceOption = (TravelLicenceOption) entity;
            //RoundUtility.Round(travelLicenceOption.KiloPrice, RoundDirection.Normal, RoundSite.AfterComma, 2);
            //RoundUtility.Round(travelLicenceOption.FoodExternalPercentage, RoundDirection.Normal, RoundSite.AfterComma, 6);
            //RoundUtility.Round(travelLicenceOption.RestExternalPercentage, RoundDirection.Normal, RoundSite.AfterComma, 6);
        }
    }
}