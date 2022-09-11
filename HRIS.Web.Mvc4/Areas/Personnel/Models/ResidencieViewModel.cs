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
    public class ResidencieViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ResidencieViewModel).FullName;
            model.Views[0].EditHandler = "ResidencieEditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
      
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            var residency = entity as Residency;

            if (residency.FirstName != employee.FirstName)
            {
                var prop = typeof(Residency).GetProperty("FirstName");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.FirstNameInEmployeeInformationDiffersFromResidenceInformation),
                    Property = prop
                });

            }
            if (residency.LastName != employee.LastName)
            {
                var prop = typeof(Residency).GetProperty("LastName");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.LastNameInEmployeeInformationDiffersFromResidenceInformation),
                    Property = prop
                });

            }
            if (residency.FatherName != employee.FatherName)
            {
                var prop = typeof(Residency).GetProperty("FatherName");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.FatherNameInEmployeeInformationDiffersFromResidenceInformation),
                    Property = prop
                });

            }
            if (residency.MotherName != employee.MotherName)
            {
                var prop = typeof(Residency).GetProperty("MotherName");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.MotherNameInEmployeeInformationDiffersFromResidenceInformation),
                    Property = prop
                });

            }
        }
    }
}