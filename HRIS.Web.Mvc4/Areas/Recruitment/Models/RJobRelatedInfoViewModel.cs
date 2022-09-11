using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Recruitment.Configurations;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RJobRelatedInfoViewModel : ViewModel
    {
        
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RJobRelatedInfoViewModel).FullName;
            model.Views[0].EditHandler = "RJobRelatedInfoEditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var jobRelatedInfo = (RJobRelatedInfo)entity;

            var applicant =
                ServiceFactory.ORMService.GetById<Applicant>(requestInformation.NavigationInfo.Previous[0].RowId);

            if (operationType == CrudOperationType.Insert)
            {
                if (applicant.RJobRelatedInfos.Count > 0)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgYouCanNotAddMoreThanOne),
                        Property = null
                    });

                    return;
                }

            }


            var evaluationSettingsCount =
                     ServiceFactory.ORMService.All<EvaluationSettings>().Count();

            if (evaluationSettingsCount == 0)
            {
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgShouldDefineEvaluationSettingFirst),
                    Property = null
                });

                return;
            }

            if (evaluationSettingsCount > 0)
            {
                var evaluationSettings = ServiceFactory.ORMService.All<EvaluationSettings>();

                if (Enumerable.Any(evaluationSettings, evaluationSetting => jobRelatedInfo.LaborOfficeRegistrationDate < evaluationSetting.LaborOfficeStartingDate))
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = RecruitmentLocalizationHelper.GetResource(
                        RecruitmentLocalizationHelper.MsgLaborOfficeRegistrationDateShouldBeEqualOrGreaterThanLaborOfficeStartingDateInEvaluationSetting),
                        Property = null
                    });

                    return;
                }
            }

            if (jobRelatedInfo.LaborOfficeRegistrationDate >= DateTime.Today)
            {
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.MsgLaborOfficeRegistrationDateShouldBeSmallerThanToday),
                    Property = null
                });

                return;
            }


            if (jobRelatedInfo.IsWorkPreviously)
            {
                if (jobRelatedInfo.WorkSide == null)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgWorkSideRequired),
                        Property = null
                    });

                    return;
                }

                if (string.IsNullOrEmpty(jobRelatedInfo.WorkSideAgreementNumber))
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgWorkSideAgreementNumberRequired),
                        Property = null
                    });

                    return;
                }

                if (jobRelatedInfo.WorkSideAgreementDate == DateTime.MinValue)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgWorkSideAgreementDateRequired),
                        Property = null
                    });

                    return;
                }
            }

            if (jobRelatedInfo.IsFamiliesMartyrs)
            {
                if (jobRelatedInfo.KinshipType == null)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgKinshipTypeRequired),
                        Property = null
                    });

                    return;
                }

                if (string.IsNullOrEmpty(jobRelatedInfo.DocumentNumber))
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgDocumentNumberRequired),
                        Property = null
                    });

                    return;
                }

                if (jobRelatedInfo.DocumentDate == DateTime.MinValue)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgDocumentDateRequired),
                        Property = null
                    });

                    return;
                }

                if (jobRelatedInfo.IssuedBy == null)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgIssuedByRequired),
                        Property = null
                    });

                    return;
                }
            }
                    
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity,
            string customInformation = null)
        {
            this.PreventDefault = true;

            var newApplicationNumber = 1;
            var rJobRelatedInfo = (RJobRelatedInfo) entity;
            var applicant =
                ServiceFactory.ORMService.GetById<Applicant>(requestInformation.NavigationInfo.Previous[0].RowId);

            var rJobRelatedInfos = ServiceFactory.ORMService.All<RJobRelatedInfo>().ToList();
            if (rJobRelatedInfos.Count > 0)
            {
                newApplicationNumber = rJobRelatedInfos.Max(x => x.ApplicationNumber) + 1;
            }
            
            rJobRelatedInfo.ApplicationNumber = newApplicationNumber;
            applicant.AddJobRelatedInfo(rJobRelatedInfo);
            ServiceFactory.ORMService.Save(applicant, UserExtensions.CurrentUser);
        }

    }
}