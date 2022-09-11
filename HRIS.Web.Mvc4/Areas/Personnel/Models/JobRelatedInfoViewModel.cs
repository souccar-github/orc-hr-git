using System;
using System.Collections.Generic;
using HRIS.Domain.Personnel.Entities;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class JobRelatedInfoViewModel : ViewModel
    {
        
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(JobRelatedInfoViewModel).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var jobRelatedInfo = (JobRelatedInfo)entity;

            if (jobRelatedInfo.IsWorkPreviously)
            {
                if (jobRelatedInfo.WorkSide == null)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.MsgWorkSideRequired,
                        Property = null
                    });

                    return;
                }

                if (string.IsNullOrEmpty(jobRelatedInfo.WorkSideAgreementNumber))
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.MsgWorkSideAgreementNumberRequired,
                        Property = null
                    });

                    return;
                }

                if (jobRelatedInfo.WorkSideAgreementDate == DateTime.MinValue)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.MsgWorkSideAgreementDateRequired,
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
                        Message = PersonnelLocalizationHelper.MsgKinshipTypeRequired,
                        Property = null
                    });

                    return;
                }

                if (string.IsNullOrEmpty(jobRelatedInfo.DocumentNumber))
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.MsgDocumentNumberRequired,
                        Property = null
                    });

                    return;
                }

                if (jobRelatedInfo.DocumentDate == DateTime.MinValue)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.MsgDocumentDateRequired,
                        Property = null
                    });

                    return;
                }

                if (jobRelatedInfo.IssuedBy == null)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = PersonnelLocalizationHelper.MsgIssuedByRequired,
                        Property = null
                    });

                    return;
                }
            }
                    
        }

        

    }
}