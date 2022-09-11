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
using System.Web.Script.Serialization;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Entities.Competency;
using HRIS.Domain.PMS.Entities.JobDescription;
using HRIS.Domain.PMS.Entities.objective;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Areas.PMS.Models;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.PMS.EventHandlers
{
    public class AppraisalTemplateEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AppraisalTemplateEventHandlers).FullName;

            //Show Windows with Two Columns
            model.Views[0].ShowTwoColumns = true;

            if (requestInformation.NavigationInfo.Module.Name.Equals(ModulesNames.PMS))
            {
                model.Views[0].EditHandler = "AppraisalTemplateEditHandler";
                model.Views[0].ViewHandler = "ViewStaticAppraisalSectionWeight";
            }
            else if(requestInformation.NavigationInfo.Module.Name.Equals(ModulesNames.Recruitment))
            {
                model.Views[0].EditHandler = "InterviewTemplateEditHandler";
                model.Views[0].ViewHandler = "InterviewTemplateViewHandler";
            }

            

        }

        /// <summary>
        /// حذف الموظفين المرتبطين بالإستمارة قبل حذف الإستمارة
        /// </summary>
        /// <param name="requestInformation"></param>
        /// <param name="entity"></param>
        public override void BeforeDelete(
            RequestInformation requestInformation, 
            Entity entity, 
            string customInformation = null)
        {
            var appraisalTemplate = (AppraisalTemplate)entity;
            var templateAppraisalPositions = typeof(TemplateAppraisalPositions).GetAll<TemplateAppraisalPositions>()
                .Where(x => x.AppraisalTemplate.Id == appraisalTemplate.Id);
            foreach (var templateAppraisalPositionse in templateAppraisalPositions)
            {
                templateAppraisalPositionse.AppraisalTemplate = null;
                templateAppraisalPositionse.Delete();
            }
        }

        public override void AfterValidation(
            RequestInformation requestInformation, 
            Entity entity, 
            IDictionary<string, object> originalState, 
            IList<ValidationResult> validationResults, 
            CrudOperationType operationType,
            string customInformation = null, Entity parententity = null)
        {
            var template = (AppraisalTemplate)entity;
            var temp = new JavaScriptSerializer();
            var sections = temp.Deserialize<List<TemplateSectionsViewModel>>(customInformation);
            var sum = sections.Where(x => x.IsIncluded).Sum(x => x.Weight);
            if (template.Competency)
                sum += template.CompetencyWeight;
            if (template.JobDescription)
                sum += template.JobDescriptionWeight;
            if (template.Objective)
                sum += template.ObjectiveWeight;

            if (sum != 100)
                validationResults.Add(new ValidationResult() { Property = null, Message = GlobalResource.InalidWeightSumMessage });
        }

        public override void BeforeInsert(
            RequestInformation requestInformation, 
            Entity entity, 
            string customInformation = null)
        {
            var template = (AppraisalTemplate)entity;
            var temp = new JavaScriptSerializer();
            var sections = temp.Deserialize<List<TemplateSectionsViewModel>>(customInformation);
            template.TemplateSectionWeights.Clear();
            foreach (var sectionViewModel in sections.Where(x => x.IsIncluded))
            {
                var section = ServiceFactory.ORMService.GetById<AppraisalSection>(sectionViewModel.Id);
                template.AddTemplateSectionWeight(new TemplateSectionWeight()
                {
                    AppraisalSection = section,
                    Weight = sectionViewModel.Weight
                });
            }
        }

        public override void BeforeUpdate(
            RequestInformation requestInformation, 
            Entity entity, 
            IDictionary<string, object> originalState,
            string customInformation = null)
        {
            var template = (AppraisalTemplate)entity;
            var temp = new JavaScriptSerializer();
            var sections = temp.Deserialize<List<TemplateSectionsViewModel>>(customInformation);
            template.TemplateSectionWeights.Clear();
            foreach (var sectionViewModel in sections.Where(x => x.IsIncluded && x.Weight>0))
            {
                var section = ServiceFactory.ORMService.GetById<AppraisalSection>(sectionViewModel.Id);
                template.AddTemplateSectionWeight(new TemplateSectionWeight()
                {
                    AppraisalSection = section,
                    Weight = sectionViewModel.Weight
                });
            }
        }
    }
}
