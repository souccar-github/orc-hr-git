using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Domain.Validation;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.PayrollSystem.Configurations;
using Remotion.Mixins.Validation;
using Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Extenstions;
using HRIS.Validation.MessageKeys;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class DeductionCardEventHandlers : ViewModel
    {
        //
        // GET: /PayrollSystem/DeductionCardEventHandlers/

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.Views[0].EditHandler = "DeductionCard_EditHandler";
            model.ViewModelTypeFullName = typeof(DeductionCardEventHandlers).FullName;
        }
        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var deductionCard = entity as HRIS.Domain.PayrollSystem.Configurations.DeductionCard;
            var primaryEmployeeDeduction = ServiceFactory.ORMService.All<PrimaryEmployeeDeduction>().Where(x => x.DeductionCard == deductionCard).ToList();

            var monthlyEmployeeDeduction = ServiceFactory.ORMService.All<MonthlyEmployeeDeduction>().Where(x => x.DeductionCard == deductionCard).ToList();

            if (primaryEmployeeDeduction.Count()>0 || monthlyEmployeeDeduction.Count() > 0)

            {
                PreventDefault = true;
            }
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var deductionCard = entity as DeductionCard;
            DeductionCard olddeductionCard = ServiceFactory.ORMService.All<DeductionCard>().FirstOrDefault(x => x.Name == deductionCard.Name);

            if (olddeductionCard != null && olddeductionCard.Id != deductionCard.Id)
            {
                var prop = typeof(DeductionCard).GetProperty("Name");
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                    Property = prop
                });
                return;
            }
        }


    }
}
