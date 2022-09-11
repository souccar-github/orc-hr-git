using System.Collections.Generic;
using System.Linq;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using HRIS.Domain.OrganizationChart.RootEntities;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Core;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Domain.Validation;
using  Project.Web.Mvc4.Helpers;
using Souccar.Domain.DomainModel;

namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    public class JLanguageViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(JLanguageViewModel).FullName;

        }
        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var jobDes = ServiceFactory.ORMService.GetById<HRIS.Domain.JobDescription.RootEntities.JobDescription>(requestInformation.NavigationInfo.Previous[0].RowId);
            var lan = entity as JLanguage;
            if (jobDes.Languages.Any(x => x.LanguageName == lan.LanguageName && x.Id != lan.Id))
            {
                var prop = typeof(JLanguage).GetProperty("LanguageName");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                    Property = prop
                });

            }

            var weight = jobDes.Languages.Where(x => x.Id != lan.Id).Sum(x => x.Weight);

            if (100 < (weight + lan.Weight))
            {
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", GlobalResource.LessThanEqMessage, 100 - weight)
                        ,
                        Property = typeof(Competence).GetProperty("Weight")
                    });
            }
        }
    }
}