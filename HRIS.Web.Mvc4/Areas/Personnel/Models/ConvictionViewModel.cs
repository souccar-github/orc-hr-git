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
    public class ConvictionViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ConvictionViewModel).FullName;
            model.Views[0].EditHandler = "ConvictionEditHandler";
            model.Views[0].ViewHandler = "ConvictionViewHandler";

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var conviction = entity as Conviction;
            var employee = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (employee.Convictions.Any(x => x.Number == conviction.Number && x.Id != conviction.Id))
            {
                var prop = typeof(Conviction).GetProperty("Number");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.NumberAlreadyexistMessage),
                    Property = prop
                });
            }


            if (conviction.ReleaseDate<=employee.DateOfBirth)
            {
                var prop = typeof(Conviction).GetProperty("ReleaseDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.ReleaseDateMustBeGreaterThanDateOfBirth),
                    Property = prop
                });
            }

        }
    }
}