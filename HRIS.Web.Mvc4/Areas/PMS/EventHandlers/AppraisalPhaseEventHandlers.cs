//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.Workflow;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Areas.Workflow.Models.Models;

namespace Project.Web.Mvc4.Areas.PMS.EventHandlers
{
    public class AppraisalPhaseEventHandlers : PhasePeriodViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AppraisalPhaseEventHandlers).FullName;
            model.Views[0].EditHandler = "PhasePeriodEditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            base.AfterValidation(requestInformation,entity,originalState,validationResults,operationType,customInformation = null,parententity = null);
            var currentPhase = entity as PhasePeriod;
            if (!validationResults.Any())
            {
                if (ServiceFactory.ORMService.All<AppraisalPhase>().Any(x => x.Id != currentPhase.Id && ((x.StartDate >= currentPhase.StartDate && x.StartDate <= currentPhase.EndDate)
                 || (x.EndDate >= currentPhase.StartDate && x.EndDate <= currentPhase.EndDate)
                  || (x.StartDate <= currentPhase.StartDate && x.EndDate >= currentPhase.EndDate))))
                {
                    var prop = typeof(PhasePeriod).GetProperty("Period");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.YouCanNotInsertMoreThanOnePhaseForTheSamePeriod),
                        Property = prop
                    });
                }
            }
            
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            base.BeforeInsert(requestInformation, entity, customInformation);
        }
        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            base.BeforeValidation(requestInformation, entity, originalState, operationType, customInformation);
        }
    }
}