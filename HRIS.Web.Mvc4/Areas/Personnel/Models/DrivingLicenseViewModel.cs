using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class DrivingLicenseViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(DrivingLicenseViewModel).FullName;
            
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity,
            IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        
        {
            var drive = (DrivingLicense) entity;
            var emp = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (drive.IssuanceDate.CompareTo(emp.DateOfBirth) < 0)
            {
                var prop = typeof(DrivingLicense).GetProperty("IssuanceDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.IssuanceDateMustBeGreaterThanDateOfBirth),
                    Property = prop
                });
            }
        }
    }
}