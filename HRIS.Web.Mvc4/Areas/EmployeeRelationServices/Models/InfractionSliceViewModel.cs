using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.EmployeeRelationServices.Configurations;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class InfractionSliceViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(InfractionSliceViewModel).FullName;
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var slice = (InfractionSlice)entity;
            var infractionForm = (InfractionForm)parententity;
            var infractionSlices = infractionForm.InfractionSlices.Where(x => x.Id != slice.Id);
            foreach (var infractionSlice in infractionSlices)
            {
                if (slice.MinimumRecurrence >= infractionSlice.MinimumRecurrence && slice.MaximumRecurrence <= infractionSlice.MaximumRecurrence ||
                    slice.MinimumRecurrence >= infractionSlice.MinimumRecurrence && slice.MinimumRecurrence <= infractionSlice.MaximumRecurrence ||
                    slice.MaximumRecurrence <= infractionSlice.MaximumRecurrence && slice.MaximumRecurrence >= infractionSlice.MinimumRecurrence)
                {
                    // شريحة المخالفة المدخلة تتقاطع مع شريحة اخرى في نفس النموذج
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(CustomMessageKeysAttendanceSystemModule.InfractionSliceConflictWithOtherInfractionSlicesInThisInfractionForm)),
                        Property = null
                    });
                }
            }
        }
    }
}