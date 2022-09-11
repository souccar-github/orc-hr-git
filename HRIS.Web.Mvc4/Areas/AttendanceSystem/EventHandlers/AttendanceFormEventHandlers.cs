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
    public class AttendanceFormEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AttendanceFormEventHandlers).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {

            var AttendanceForm = entity as AttendanceForm;

            AttendanceForm AttendanceFormExist = null;

            AttendanceFormExist = ServiceFactory.ORMService.All<AttendanceForm>().Where(a => a.Number == AttendanceForm.Number && a.Id != AttendanceForm.Id).FirstOrDefault();
            if (AttendanceFormExist !=null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                              .GetFullKey(CustomMessageKeysAttendanceSystemModule.ThereAreAnotherAttendanceTemplateWithThisNumber)),
                    Property = null
                });
                return;
            }

            if (AttendanceForm.WorkshopRecurrences.Count == 0)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(
                                CustomMessageKeysAttendanceSystemModule
                                    .YouMustAddOneWorkshopRecurrencesAtLeast)),
                    Property = null
                   
                });
            }

        }
    }
}