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
    public class CertificationViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(CertificationViewModel).FullName;

        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var emp = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            var certification = entity as Certification;
            if (emp.Certifications.Any(x => x.Type.Name == certification.Type.Name && x.Id != certification.Id))
            {
                var prop = typeof(Skill).GetProperty("Name");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.AlreadyexistMessage),
                    Property = prop
                });

            }
            if (certification.DateOfIssuance.HasValue && certification.DateOfIssuance.GetValueOrDefault().CompareTo(emp.DateOfBirth) < 0)
            {
                var prop = typeof(Certification).GetProperty("DateOfIssuance");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.DateOfIssuanceMustBeGreaterThanDateOfBirth),
                    Property = prop
                });
            }
          
        }
    }
}