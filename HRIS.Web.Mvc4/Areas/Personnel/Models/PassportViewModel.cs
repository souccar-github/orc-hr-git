using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using Souccar.Domain.DomainModel;

using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class PassportViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(PassportViewModel).FullName;
            model.Views[0].EditHandler = "PassportEditHandler";
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            var passport = entity as Passport;

            if (passport.FirstName != employee.FirstName)
            {
                var prop = typeof(Passport).GetProperty("FirstName");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.FirstNameInEmployeeInformationDiffersFromPassportInformation),
                    Property = prop
                });

            }
            if (passport.LastName != employee.LastName)
            {
                var prop = typeof(Passport).GetProperty("LastName");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.LastNameInEmployeeInformationDiffersFromPassportInformation),
                    Property = prop
                });

            }
            if (passport.FatherName != employee.FatherName)
            {
                var prop = typeof(Passport).GetProperty("FatherName");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.FatherNameInEmployeeInformationDiffersFromPassportInformation),
                    Property = prop
                });

            }
            if (passport.MotherName != employee.MotherName)
            {
                var prop = typeof(Passport).GetProperty("MotherName");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.MotherNameInEmployeeInformationDiffersFromPassportInformation),
                    Property = prop
                });

            }

            if (passport.FirstNameL2 != employee.FirstNameL2)
            {
                var prop = typeof(Passport).GetProperty("FirstNameL2");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.FirstNameL2InEmployeeInformationDiffersFromPassportInformation),
                    Property = prop
                });

            }

            if (passport.LastNameL2 != employee.LastNameL2)
            {
                var prop = typeof(Passport).GetProperty("LastNameL2");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.LastNameL2InEmployeeInformationDiffersFromPassportInformation),
                    Property = prop
                });

            }

            if (passport.IssuanceDate <= employee.DateOfBirth)
            {
                var prop = typeof(Passport).GetProperty("IssuanceDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.IssuanceDateMustBeGreaterThanDateOfBirth),
                    Property = prop
                });
            }
        }        
    }
}