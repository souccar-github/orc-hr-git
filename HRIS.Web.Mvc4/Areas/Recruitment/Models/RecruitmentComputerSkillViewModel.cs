using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RecruitmentComputerSkillViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RecruitmentComputerSkillViewModel).FullName;

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var recruitmentComputerSkill = entity as RecruitmentComputerSkill;

            var jobApplication = ServiceFactory.ORMService.All<JobApplication>()
                .FirstOrDefault(x => x.Id == requestInformation.NavigationInfo.Previous[0].RowId);


            #region Skill type already exist for same job application

            var skillTypeAlreadyExist = ServiceFactory.ORMService.All<RecruitmentComputerSkill>().Any(x => x.SkillType.Id == recruitmentComputerSkill.SkillType.Id && x.Id != recruitmentComputerSkill.Id && x.JobApplication.Id == jobApplication.Id);
            if (skillTypeAlreadyExist)
            {
                validationResults.Add(new ValidationResult()
                {
                    Property = typeof(RecruitmentComputerSkill).GetProperty("SkillType"),
                    Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.SkillTypeAlreadyExistsForTheSameJobApplication)
                });
            }

            #endregion

        }

    }
}