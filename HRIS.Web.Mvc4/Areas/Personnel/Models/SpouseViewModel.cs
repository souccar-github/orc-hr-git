using HRIS.Domain.Personnel.Entities;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Validation.MessageKeys;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class SpouseViewModel:ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(SpouseViewModel).FullName;
            model.Views[0].EditHandler = "SpouseEditHandler";
            model.Views[0].ViewHandler = "SpouseViewHandler";

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            var spouse = entity as Spouse;
            
            if (employee.Spouse.Any(x => x.Order == spouse.Order && x.Id != spouse.Id))
            {
                var prop = typeof(Spouse).GetProperty("Order");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.OrderAlreadyexistMessage),
                    Property = prop
                });

            }
            if (spouse.ResidencyExpiryDate.HasValue &&
                spouse.ResidencyExpiryDate.GetValueOrDefault().CompareTo(spouse.DateOfBirth) < 0)
            {
                var prop = typeof(Spouse).GetProperty("ResidencyExpiryDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", PreDefinedMessageKeysSpecExpress.GreaterThanEqualTo),
                    Property = prop
                });
            }
            if (spouse.PassportExpiryDate.HasValue &&
                spouse.PassportExpiryDate.GetValueOrDefault().CompareTo(spouse.DateOfBirth) < 0)
            {
                var prop = typeof(Spouse).GetProperty("PassportExpiryDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", PreDefinedMessageKeysSpecExpress.GreaterThanEqualTo),
                    Property = prop
                });
            }
        }
    }
}