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
    public class WorkshopEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.Views[0].EditHandler = "WorkshopEventHandler";
            model.ViewModelTypeFullName = typeof(WorkshopEventHandlers).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {

            var Workshop = entity as Workshop;
            var WorkshopExist = ServiceFactory.ORMService.All<Workshop>().Where(w=>w.Number== Workshop.Number && w.Id != Workshop.Id).FirstOrDefault();
            if (WorkshopExist != null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(
                                CustomMessageKeysAttendanceSystemModule
                                    .ThereAreAnotherWorkshopWithThisNumber)),
                    Property = null

                });
            }

            if (Workshop.NormalShifts.Count == 0)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(
                                CustomMessageKeysAttendanceSystemModule
                                    .YouMustAddOneNormalShiftAtLeast)),
                    Property = null
                  
                });
            }

        }


    }
}