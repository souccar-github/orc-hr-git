using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using HRIS.Domain.Recruitment.Entities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class ProfessionalCertificationViewModel:ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ProfessionalCertificationViewModel).FullName;

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var professionalCertification = entity as ProfessionalCertification;

            var jobApplication = ServiceFactory.ORMService.All<JobApplication>()
                .FirstOrDefault(x => x.Id == requestInformation.NavigationInfo.Previous[0].RowId);


            #region Check if start working date greater than date of birth of applicant

            if (professionalCertification != null && (jobApplication != null && jobApplication.DateOfBirth > professionalCertification.DateOfIssuance))
            {
                validationResults.Add(new ValidationResult()
                {
                    Property = typeof(ProfessionalCertification).GetProperty("DateOfIssuance"),
                    Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.StartWorkingDateMustBeGreaterThanDateOfBirthOfApplicant)
                });
            }

            #endregion

            #region Certificate name already exist for same job application

            var certificationNameAlreadyExist = ServiceFactory.ORMService.All<ProfessionalCertification>().Any(x => x.Type == professionalCertification.Type && x.JobApplication.Id == jobApplication.Id);
            if (certificationNameAlreadyExist)
            {
                validationResults.Add(new ValidationResult()
                {
                    Property = typeof(ProfessionalCertification).GetProperty("Type"),
                    Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.CertificationNameAlreadyExistsForTheSameJobApplication)
                });
            }

            #endregion

        }
    }
}