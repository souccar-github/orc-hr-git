using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Extensions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class TaxSliceEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TaxSliceEventHandlers).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var taxSlice = (TaxSlice)entity;
            if(taxSlice.StartSlice > taxSlice.EndSlice)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.TheEndSliceMustBeGreaterThanTheStartSlice)),
                    Property = null
                });
            }
            var conflictEntityWithStartRange =
                    typeof(TaxSlice).GetAll<TaxSlice>().FirstOrDefault(x => taxSlice.StartSlice >= x.StartSlice && taxSlice.StartSlice < x.EndSlice);
            var conflictEntityWithEndRange =
                                typeof(TaxSlice).GetAll<TaxSlice>().FirstOrDefault(x => taxSlice.EndSlice > x.StartSlice && taxSlice.EndSlice <= x.EndSlice);
            
            if (conflictEntityWithStartRange != null && taxSlice.Id != conflictEntityWithStartRange.Id)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.ConflictSlices)),
                    Property = typeof(TaxSlice).GetProperty("StartSlice")
                });
            }

            if (conflictEntityWithEndRange != null && taxSlice.Id != conflictEntityWithEndRange.Id)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.ConflictSlices)),
                    Property = typeof(TaxSlice).GetProperty("EndSlice")
                });
            }
        }
    }
}