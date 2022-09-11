using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class ExperienceViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ExperienceViewModel).FullName;
            model.Views[0].EditHandler = "ExperienceEditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
       
        {
            var experience = entity as Experience;
            var employee = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);


            if (experience.StartDate.HasValue && experience.StartDate.GetValueOrDefault() <= employee.DateOfBirth)
            {
                var prop = typeof(Experience).GetProperty("StartDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.StartDateMustBeGreaterThanDateOfBirth),
                    Property = prop
                });
            }
            if(experience.EndDate != null && experience.EndDate.Value >= DateTime.Now.Date)
            {
                var prop = typeof(Experience).GetProperty("EndDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.EndDateMustBeLessThanNowDate),
                    Property = prop
                });

            }

        }
    }
}