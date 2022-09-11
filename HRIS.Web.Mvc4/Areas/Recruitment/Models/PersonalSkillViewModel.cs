using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class PersonalSkillViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(PersonalSkillViewModel).FullName;

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var personalSkill = entity as PersonalSkill;

            var jobApplication = ServiceFactory.ORMService.All<JobApplication>()
                .FirstOrDefault(x => x.Id == requestInformation.NavigationInfo.Previous[0].RowId);

            #region Skill type already exist for same job application

            var skillTypeAlreadyExist = ServiceFactory.ORMService.All<PersonalSkill>().Any(x => x.SkillType.Id == personalSkill.SkillType.Id && x.Id != personalSkill.Id && x.JobApplication.Id == jobApplication.Id);
            if (skillTypeAlreadyExist)
            {
                validationResults.Add(new ValidationResult()
                {
                    Property = typeof(PersonalSkill).GetProperty("SkillType"),
                    Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.SkillTypeAlreadyExistsForTheSameJobApplication)
                });
            }

            #endregion

        }
    }
}