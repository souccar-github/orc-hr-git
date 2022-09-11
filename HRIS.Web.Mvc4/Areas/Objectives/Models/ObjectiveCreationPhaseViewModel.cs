using HRIS.Domain.Objectives.RootEntities;
using HRIS.Domain.Workflow;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;
using Souccar.Domain.Extensions;
using  Project.Web.Mvc4.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Infrastructure.Extenstions;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Areas.Workflow.Models.Models;

namespace Project.Web.Mvc4.Areas.Objectives.Models
{
    public class ObjectiveCreationPhaseViewModel : PhasePeriodViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ObjectiveCreationPhaseViewModel).FullName;
            model.Views[0].EditHandler = "PhasePeriodEditHandler";
        }
        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            base.AfterValidation(requestInformation, entity, originalState, validationResults, operationType, customInformation, parententity);
            var currentPhase = entity as PhasePeriod;
            if (currentPhase.Year >= 1900)
            {
                if (ServiceFactory.ORMService.All<ObjectiveCreationPhase>().Any(x => x.Id != currentPhase.Id && ((x.StartDate >= currentPhase.StartDate && x.StartDate <= currentPhase.EndDate) || (x.EndDate >= currentPhase.StartDate && x.EndDate <= currentPhase.EndDate))))
                {
                    var prop = typeof(PhasePeriod).GetProperty("Period");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.PeriodErrorMessage),
                        Property = prop
                    });
                }
            }
        }

    }
}