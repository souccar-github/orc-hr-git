using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class JobApplicationViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(JobApplicationViewModel).FullName;
            model.Views[0].EditHandler = "jobApplicationEditorTemplate";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var jobApplication = entity as JobApplication;

            #region  Applicant Id No Already Exists

            if (jobApplication != null && !string.IsNullOrEmpty(jobApplication.ApplicantNo))
            {
                var applicantNoAlreadyExists = ServiceFactory.ORMService.All<JobApplication>().Any(x =>
                    x.ApplicantNo == jobApplication.ApplicantNo && x.Id != jobApplication.Id);
                if (applicantNoAlreadyExists)
                {
                    var prop = typeof(JobApplication).GetProperty("ApplicantNo");
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = $"{prop.GetTitle()} {GlobalResource.AlreadyexistMessage}",
                            Property = prop
                        });
                }
            }

            #endregion

            #region  Identification No Already Exists

            if (jobApplication != null && !string.IsNullOrEmpty(jobApplication.IdentificationNo))
            {
                var identificationNoAlreadyExists = ServiceFactory.ORMService.All<JobApplication>().Any(x =>
                    x.IdentificationNo == jobApplication.IdentificationNo && x.Id != jobApplication.Id);
                if (identificationNoAlreadyExists)
                {
                    var prop = typeof(JobApplication).GetProperty("IdentificationNo");
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = $"{prop.GetTitle()} {GlobalResource.AlreadyexistMessage}",
                            Property = prop
                        });
                }
            }

            #endregion

            #region OtherNationality is required when OtherNationalityExist=true
            if (jobApplication.OtherNationalityExist && jobApplication.OtherNationality == null)
            {
                var prop = typeof(JobApplication).GetProperty("OtherNationality");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage),
                        Property = prop
                    });
            }
            #endregion

            #region DisabilityType is required when DisabilityExist=true
            if (jobApplication.DisabilityExist && jobApplication.DisabilityType == null)
            {
                var prop = typeof(JobApplication).GetProperty("DisabilityType");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage),
                        Property = prop
                    });
            }

            #endregion

            #region Duration is required when HaveWorkPermit=true
            if (jobApplication.HaveWorkPermit && string.IsNullOrEmpty(jobApplication.Duration))
            {
                var prop = typeof(JobApplication).GetProperty("Duration");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage),
                        Property = prop
                    });
            }
            #endregion

            jobApplication.MilitaryStatus = MilitaryStatus.Nothing;
        }

        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var jobApplication = entity as JobApplication;
            if (jobApplication != null)
            {
                jobApplication.ApplicationYear = jobApplication.ApplicationDate.Year.ToString();
                jobApplication.ApplicationStatus = ApplicationStatus.Initiated;
                jobApplication.EnterBy = EnterBy.ViaHrStaff;

                jobApplication.Requester = UserExtensions.CurrentUser;
                ServiceFactory.ORMService.Save(jobApplication, UserExtensions.CurrentUser);
            }
        }
    }
}