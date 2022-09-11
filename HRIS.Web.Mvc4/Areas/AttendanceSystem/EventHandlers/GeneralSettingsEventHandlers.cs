using HRIS.Domain.AttendanceSystem.Configurations;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class GeneralSettingsEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(GeneralSettingsEventHandlers).FullName;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
        }
        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            if (ServiceFactory.ORMService.All<GeneralSettings>().Any() && operationType == CrudOperationType.Insert)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format(GlobalResource.YouCanNotAddMoreThenOneGeneralSetting),
                    Property = null
                });

            }

        }
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
        }
    }
}