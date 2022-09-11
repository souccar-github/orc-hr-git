using System;
using System.Collections.Generic;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Entity = Souccar.Domain.DomainModel.Entity;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class TemporaryWorkshopViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TemporaryWorkshopViewModel).FullName;
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var temworkshop = (TemporaryWorkshop)entity;
            var empcrad = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (empcrad.StartWorkingDate.HasValue && temworkshop.FromDate.CompareTo(empcrad.StartWorkingDate.GetValueOrDefault()) < 0)
            {
                var prop = typeof(TemporaryWorkshop).GetProperty("FromDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.FromDateMustBeGreaterThanStartWorkingDate),
                    Property = prop
                });
            }
            if (temworkshop.ToDate.CompareTo(temworkshop.FromDate) < 0)
            {
                var prop = typeof(TemporaryWorkshop).GetProperty("ToDate");
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.ToDateMustBeGreaterThanFromDate),
                    Property = prop
                });
            }
        }
    }
}