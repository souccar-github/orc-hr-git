using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.JobDescription.Entities;
using  Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

using  Project.Web.Mvc4.Helpers;
using HRIS.Domain.JobDescription.RootEntities;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    public class CompetenceViewModel : ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity,
            IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var competence = entity as Competence;
            var previous = requestInformation.NavigationInfo.Previous;
            if (operationType == CrudOperationType.Insert || operationType == CrudOperationType.Update)
            {
                var jobDescription = (HRIS.Domain.JobDescription.RootEntities.JobDescription)
                    typeof (HRIS.Domain.JobDescription.RootEntities.JobDescription).GetById(previous[0].RowId);
                var weight = jobDescription.Competencies.Where(x => x.Id != entity.Id).Sum(x => x.Weight);
                if (100 < (weight + competence.Weight))
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = string.Format("{0} {1}", GlobalResource.LessThanEqMessage, 100 - weight)
                            ,
                            Property = typeof (Competence).GetProperty("Weight")
                        });

                if (
                    jobDescription.Competencies.Any(
                        x => x.Level == competence.Level && x.Id != competence.Id))
                {
                    var prop = typeof(Competence).GetProperty("Level");
                    validationResults.Add(new ValidationResult()
                                          {
                                              Message =
                                                  string.Format("{0} {1}", prop.GetTitle(),
                                                      GlobalResource.AlreadyexistMessage),
                                              Property = prop
                                          });

                }

            }
        }

       public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type,
            RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof (CompetenceViewModel).FullName;
            //model.Views[0].AfterRequestEnd = "RoleAfterRequestEndHandler";

        }
    }
}