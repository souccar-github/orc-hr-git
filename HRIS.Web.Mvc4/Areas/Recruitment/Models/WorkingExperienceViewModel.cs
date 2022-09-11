using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class WorkingExperienceViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(WorkingExperienceViewModel).FullName;

            model.Views[0].EditHandler = "workingExperienceEditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var workingExperience = entity as WorkingExperience;

            var jobApplication = ServiceFactory.ORMService.All<JobApplication>()
                .FirstOrDefault(x => x.Id == requestInformation.NavigationInfo.Previous[0].RowId);


            #region Check if start working date greater than date of birth of applicant

            if (workingExperience != null && (jobApplication != null && jobApplication.DateOfBirth > workingExperience.StartDate))
            {
                validationResults.Add(new ValidationResult()
                {
                    Property = typeof(WorkingExperience).GetProperty("StartDate"),
                    Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.StartWorkingDateMustBeGreaterThanDateOfBirthOfApplicant)
                });
            }

            #endregion

            #region ReferenceJobTitle is required when AuthorizationToCheck=true
            if (workingExperience.AuthorizationToCheck && workingExperience.ReferenceJobTitle == null)
            {
                var prop = typeof(WorkingExperience).GetProperty("ReferenceJobTitle");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage),
                        Property = prop
                    });
            }

            #endregion

            #region ReferenceContact is required when AuthorizationToCheck=true
            if (workingExperience.AuthorizationToCheck && workingExperience.ReferenceContact == null)
            {
                var prop = typeof(WorkingExperience).GetProperty("ReferenceContact");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage),
                        Property = prop
                    });
            }

            #endregion

            #region ReferenceFullName is required when AuthorizationToCheck=true
            if (workingExperience.AuthorizationToCheck && workingExperience.ReferenceFullName == null)
            {
                var prop = typeof(WorkingExperience).GetProperty("ReferenceFullName");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage),
                        Property = prop
                    });
            }

            #endregion

            if (!workingExperience.AuthorizationToCheck)
            {
                workingExperience.ReferenceContact = string.Empty;
                workingExperience.ReferenceEmail = string.Empty;
                workingExperience.ReferenceFullName = string.Empty;
                workingExperience.ReferenceJobTitle = null;
            }
        }
    }
}