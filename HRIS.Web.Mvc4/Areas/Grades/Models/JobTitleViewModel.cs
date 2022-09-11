using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using  Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Grades.Models
{
    public class JobTitleViewModel : ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var jobTitle = entity as JobTitle;
            JobTitle oldJobTitle = ServiceFactory.ORMService.All<JobTitle>().FirstOrDefault(x => x.Name == jobTitle.Name);

            if (oldJobTitle != null && oldJobTitle.Id != jobTitle.Id)
            {
                var prop = typeof(JobTitle).GetProperty("Name");
                validationResults.Add(
                new ValidationResult()
                {
                    Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                    Property = prop
                });
            }
        }

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(JobTitleViewModel).FullName;

        }
    }
}