using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class SkillViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(SkillViewModel).FullName;

        }
        public override void AfterValidation(RequestInformation requestInformation,Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var emp = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            var skl = entity as Skill;
            if (emp.Skills.Any(x => x.SkillType == skl.SkillType && x.Id != skl.Id))
            {
                var prop = typeof(Skill).GetProperty("Name");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.AlreadyexistMessage),
                    Property = prop
                });

            }
        }
    }
}