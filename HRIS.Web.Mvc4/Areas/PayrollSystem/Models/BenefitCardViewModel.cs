using System;
using System.Linq;
using FluentNHibernate.Conventions;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;

using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PayrollSystem.Configurations;
using  Project.Web.Mvc4.Helpers;
using Souccar.Domain.DomainModel;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Models
{
    public class BenefitCardViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(BenefitCardViewModel).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation,
            Souccar.Domain.DomainModel.Entity entity,
            System.Collections.Generic.IDictionary<string, object> originalState,
            System.Collections.Generic.IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var benefitCard = entity as BenefitCard;
            BenefitCard oldbenefitCard = ServiceFactory.ORMService.All<BenefitCard>().FirstOrDefault(x => x.Name == benefitCard.Name);

            if (oldbenefitCard != null && oldbenefitCard.Id != benefitCard.Id)
            {
                var prop = typeof(BenefitCard).GetProperty("Name");
                validationResults.Add(
                new ValidationResult()
                {
                    Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                    Property = prop
                });
            }
        }
    }
}