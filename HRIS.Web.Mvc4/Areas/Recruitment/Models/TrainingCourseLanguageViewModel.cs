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
    public class TrainingCourseLanguageViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TrainingCourseLanguageViewModel).FullName;

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var trainingCourseLanguage = entity as TrainingCourseLanguage;

            var jobApplication = ServiceFactory.ORMService.All<JobApplication>()
                .FirstOrDefault(x => x.Id == requestInformation.NavigationInfo.Previous[0].RowId);

            #region Language name already exist for same job application

            var languageAlreadyExist = ServiceFactory.ORMService.All<TrainingCourseLanguage>().Any(x => x.LanguageName.Id == trainingCourseLanguage.LanguageName.Id && x.Id != trainingCourseLanguage.Id && x.JobApplication.Id == jobApplication.Id);
            if (languageAlreadyExist)
            {
                validationResults.Add(new ValidationResult()
                {
                    Property = typeof(TrainingCourseLanguage).GetProperty("LanguageName"),
                    Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.LanguageNameAlreadyExistsForTheSameJobApplication)
                });
            }

            #endregion

        }
    }
}