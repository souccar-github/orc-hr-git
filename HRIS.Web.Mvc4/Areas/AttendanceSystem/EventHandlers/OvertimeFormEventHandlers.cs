using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.Helpers;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class OvertimeFormEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(OvertimeFormEventHandlers).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {

            var OvertimeForm = entity as OvertimeForm;

            OvertimeForm OvertimeFormExist = null;

            OvertimeFormExist = ServiceFactory.ORMService.All<OvertimeForm>().Where(a => a.Number == OvertimeForm.Number && a.Id != OvertimeForm.Id).FirstOrDefault();
            if (OvertimeFormExist != null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                              .GetFullKey(CustomMessageKeysAttendanceSystemModule.ThereAreAnotherOverTimeTemplateWithThisNumber)),
                    Property = null
                });
                return;
            }

            if (OvertimeForm.OvertimeSlices.Count == 0)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(
                                CustomMessageKeysAttendanceSystemModule
                                    .YouMustAddOneOvertimeSliceAtLeast)),
                    Property = null             
                });
            }

        }

    }
}