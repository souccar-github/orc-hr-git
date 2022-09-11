using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.AttendanceSystem.Helpers;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class InfractionFormEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(InfractionFormEventHandlers).FullName;
            
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {

            var InfractionForm = entity as InfractionForm;

            InfractionForm InfractionFormExist = null;
            InfractionFormExist = ServiceFactory.ORMService.All<InfractionForm>().Where(a => (a.Number == InfractionForm.Number || a.Infraction == InfractionForm.Infraction) && a.Id != InfractionForm.Id).FirstOrDefault();
            if(InfractionFormExist != null)
            {
                var infractionFormExistOnName = InfractionFormExist.Number == InfractionForm.Number ? true : false;
                if (!infractionFormExistOnName)
                {
                    validationResults.Add(new ValidationResult()
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                                  .GetFullKey(CustomMessageKeysAttendanceSystemModule.ThereAreAnotherInfractionTemplateWithThisName)),
                        Property = null
                    });
                    return;
                }
                if (infractionFormExistOnName)
                {
                    validationResults.Add(new ValidationResult()
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                                  .GetFullKey(CustomMessageKeysAttendanceSystemModule.ThereAreAnotherInfractionTemplateWithThisNumber)),
                        Property = null
                    });
                    return;
                }
            }
            

            if (InfractionForm.InfractionSlices.Count == 0)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(
                                CustomMessageKeysAttendanceSystemModule
                                    .YouMustAddOneInfractionSliceAtLeast)),
                    Property = null

                });
            }

        }
    }
}