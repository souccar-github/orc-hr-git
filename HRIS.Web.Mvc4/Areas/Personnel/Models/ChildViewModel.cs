using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Data;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Entity = Souccar.Domain.DomainModel.Entity;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class ChildViewModel:ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ChildViewModel).FullName;
            model.Views[0].EditHandler = "ChildEditHandler";
            model.Views[0].ViewHandler = "ChildViewHandler";

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity,
            IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employee =
            ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            var child = entity as Child;
           
            var issameorder =
                employee.Children.Where(x => x.OrderInFamily == child.OrderInFamily && x.Id != child.Id);
            if (issameorder.Any())
            {
                var prop = typeof (Child).GetProperty("OrderInFamily");
                var propTitle = prop.GetTitle();
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", propTitle, GlobalResource.OrderInFamilyAlreadyExists),
                    Property = prop
                });
            }

            /////////////   Forbid enter date of bearth less than MarriageDate //////////////
            var spouse = child.Spouse;
            if (spouse != null)
            {
                if (spouse.MarriageDate >= child.DateOfBirth)
                {
                    var prop = typeof (Child).GetProperty("DateOfBirth");
                    var propTitle = prop.GetTitle();
                    validationResults.Add(new ValidationResult()
                    {
                        Message =
                            PersonnelLocalizationHelper.GetResource(
                                PersonnelLocalizationHelper.DateOfBirthForTheChildMustBeGreaterThanMarriageDate),
                        Property = prop
                    });
                }
                
            }
            if (child.IsDeath)
            {
                if (child.DeathDate == null)
                {
                    var prop = typeof(Child).GetProperty("DeathDate");
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage),
                            Property = prop
                        });
                }
            }
           
        }
    }
    
}