using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class TemporaryWorkshopEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, System.Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TemporaryWorkshopEventHandlers).FullName;
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var temporaryWorkshop = (TemporaryWorkshop)entity;
            var workshop = temporaryWorkshop.Workshop;
            var temporaryWorkshops = workshop.TemporaryWorkshops.Where(x => x.Id != temporaryWorkshop.Id);
            foreach (var temporary in temporaryWorkshops)
            {
                if (temporaryWorkshop.FromDate >= temporary.FromDate && temporaryWorkshop.ToDate <= temporary.ToDate ||
                    temporaryWorkshop.FromDate >= temporary.FromDate && temporaryWorkshop.FromDate <= temporary.ToDate ||
                    temporaryWorkshop.ToDate <= temporary.ToDate && temporaryWorkshop.ToDate >= temporary.FromDate)
                {
                    // الوردية الاستثنائية المدخلة تتقاطع مع ورديات استثنائية اخرى في نفس الوردية
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(CustomMessageKeysAttendanceSystemModule.TemporaryWorkshopConflictWithOtherTemporaryWorkshopsInThisWorkshop)),
                        Property = null
                    });
                }
            }
        }
    }
}