using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Areas.Personnel.Models;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    public class JEducationViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(JEducationViewModel).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var jobDes = ServiceFactory.ORMService.GetById<HRIS.Domain.JobDescription.RootEntities.JobDescription>(requestInformation.NavigationInfo.Previous[0].RowId);
            var jEducation = entity as JEducation;

            var weight = jobDes.Educations.Where(x => x.Id != jEducation.Id).Sum(x => x.Weight);

            if (100 < (weight + jEducation.Weight))
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
