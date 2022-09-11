using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Data;
using HRIS.Domain.Recruitment.Configurations;
using HRIS.Domain.Recruitment.RootEntities;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Infrastructure.Core;
using Entity = Souccar.Domain.DomainModel.Entity;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class EvaluationSettingsViewModel : ViewModel
    {

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            //add
            model.ViewModelTypeFullName = typeof(EvaluationSettingsViewModel).FullName;
            model.Views[0].EditHandler = "EvaluationSettingsEditHandler";
            model.Views[0].Columns.SingleOrDefault(x => x.FieldName == "RecruitmentType").ReadUrl = "Recruitment/Reference/ReadRecruitmentType";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var evaluationSettings = (EvaluationSettings)entity;
            
            if (operationType == CrudOperationType.Insert)
            {
                var evaluationSettingsCount = 
                    ServiceFactory.ORMService.All<EvaluationSettings>().Count(x => x.RecruitmentType == evaluationSettings.RecruitmentType);

                if (evaluationSettingsCount == 1)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message =RecruitmentLocalizationHelper.GetResource( RecruitmentLocalizationHelper.MsgAlreadyExistSettingForThisRecruitmentType),
                        Property = null
                    });

                    return;
                }

            }

            if (evaluationSettings.WrittenWeightFactor + evaluationSettings.OralWeightFactor +
                evaluationSettings.OldnessWeightFactor
                + evaluationSettings.MartyrSonFactor != 100)
            {
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message =RecruitmentLocalizationHelper.GetResource( RecruitmentLocalizationHelper.MsgWrittenOralOldnessMartyrSonTotalFactorsShouldBeOneHundred),
                    Property = null
                });

                return;
            }
        }
    }
}