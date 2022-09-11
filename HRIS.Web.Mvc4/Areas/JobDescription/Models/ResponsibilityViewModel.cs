using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.JobDescription.Configurations;
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
    public class ResponsibilityViewModel : ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var previous = requestInformation.NavigationInfo.Previous;
            if (operationType == CrudOperationType.Insert || operationType == CrudOperationType.Update)
            {
                var jobDescription = (HRIS.Domain.JobDescription.RootEntities.JobDescription)
                    typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).GetById(previous[0].RowId);
                var role = jobDescription.Roles.SingleOrDefault(x => x.Id.Equals(previous[1].RowId));
                var weight = role.Responsibilities.Where(x => x.Id != entity.Id).Sum(x => x.Weight);
                if (100 < (weight + ((Responsibility)entity).Weight))
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = string.Format("{0}{1}", GlobalResource.LessThanEqMessage, 100 - weight)
                        ,
                            Property = typeof(Responsibility).GetProperty("Weight")
                        });

            }
        }

        public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ResponsibilityViewModel).FullName;
            model.Views[0].AfterRequestEnd = "ResponsibilityAfterRequestEndHandler";

        }
    }

    public class RoleViewModel : ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var previous = requestInformation.NavigationInfo.Previous;
            if (operationType == CrudOperationType.Insert || operationType == CrudOperationType.Update)
            {
                var jobDescription = (HRIS.Domain.JobDescription.RootEntities.JobDescription)
                    typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription).GetById(previous[0].RowId);
                var weight = jobDescription.Roles.Where(x => x.Id != entity.Id).Sum(x => x.Weight);
                if (100 < (weight + ((Role)entity).Weight))
                {
                    var prop=typeof(Role).GetProperty("Weight");
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = string.Format("{0} {1} {2}", prop.GetTitle(), GlobalResource.LessThanEqMessage, 100 - weight),
                            Property = prop
                        });
                }
            }
        }

       public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RoleViewModel).FullName;
            model.Views[0].AfterRequestEnd = "RoleAfterRequestEndHandler";

        }
    }


    public class CompetenceCategoryViewModel : ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var category = entity as CompetenceCategory;
            var previous = requestInformation.NavigationInfo.Previous;
            if (operationType == CrudOperationType.Insert || operationType == CrudOperationType.Update)
            {
                if (ServiceFactory.ORMService.All<CompetenceCategory>().Any(x => x.Name == category.Name && x.Type == category.Type && x.Id != category.Id))
                {
                    var nameProp = typeof(CompetenceCategory).GetProperty("Name");
                    var typeProp = typeof(CompetenceCategory).GetProperty("Type");
                    validationResults.Add(new ValidationResult()
                    {
                        Message = string.Format("({0} - {1}) {2} {3}", nameProp.GetTitle(), typeProp.GetTitle(), GlobalResource.Pair, GlobalResource.AlreadyexistMessage),
                        Property = null
                    });

                }

            }
        }

       public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(CompetenceCategoryViewModel).FullName;
            //model.Views[0].AfterRequestEnd = "RoleAfterRequestEndHandler";

        }
    }

    public class CompetenceCategoryLevelDescriptionViewModel : ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var temp = entity as CompetenceCategoryLevelDescription;
            var previous = requestInformation.NavigationInfo.Previous;
            if (operationType == CrudOperationType.Insert || operationType == CrudOperationType.Update)
            {
                var competenceCategory = ServiceFactory.ORMService.GetById<CompetenceCategory>(requestInformation.NavigationInfo.Previous.First().RowId);
                if (competenceCategory.LevelDescriptions.Any(x => x.Level == temp.Level && x.Id != temp.Id))
                {
                    var prop = typeof(CompetenceCategoryLevelDescription).GetProperty("Level");
                    validationResults.Add(new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                        Property = prop
                    });

                }

            }
        }

       public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(CompetenceCategoryLevelDescriptionViewModel).FullName;
            //model.Views[0].AfterRequestEnd = "RoleAfterRequestEndHandler";

        }
    }
}