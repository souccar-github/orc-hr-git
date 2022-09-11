using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PayrollSystem.Entities;
using Project.Web.Mvc4.Helpers;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{

    public class BenefitCardEventHandlers : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.Views[0].EditHandler = "BenefitCard_EditHandler";
            model.ViewModelTypeFullName = typeof(BenefitCardEventHandlers).FullName;
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var benefitCard = (BenefitCard)entity;

            if (IsItemBrokenParentsChain(benefitCard))
            {// الهدف هو اختبار ان التعويض الحالي هو ليس اب لاي من التعويضات التي أعلى منه حتى لايكون لدينا سلسلة من نوع اب يشير الى ابن وابن يشير الى اب
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.ParentBenefitCardBrokenParentsChain)),
                    Property = typeof(BenefitCard).GetProperty("ParentBenefitCard")
                });
            }

            if (benefitCard.ParentBenefitCard != null)
            {
                if (typeof(EmployeeCard).GetAll<EmployeeCard>().Any(x => x.PrimaryEmployeeBenefits.Any(y => y.BenefitCard.Id == benefitCard.ParentBenefitCard.Id)) ||
                       // typeof(Month).GetAll<Month>().Any(x => x.MonthBenefitChanges.Any(y => y.BenefitCard.Id == benefitCard.ParentBenefitCard.Id)) || 
                        typeof(Month).GetAll<Month>().Any(x => x.MonthlyCards.Any(y => y.MonthlyEmployeeBenefits.Any(z => z.BenefitCard.Id == benefitCard.ParentBenefitCard.Id))))
                {// لا يمكن استخدام هذا التعويض كتعويض أب في واجهة بطاقة التعويض لأان التعويض الاب المختار قد تم استخدامه مسبقا كتعويض عادي اي تعويض في البطاقات الاساسية للموظفين او التغييرات الشهرية او البطاقات الشهرية
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule
                        .GetFullKey(CustomMessageKeysPayrollSystemModule.CannotUseBenefitCardAsParentBecauseItsUsedAsNormalBenefit)),
                        Property = typeof(BenefitCard).GetProperty("ParentBenefitCard")
                    });
                }
            }
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

        private bool IsItemBrokenParentsChain(BenefitCard benefitCard)
        {
            var values = new SortedSet<int>();
            CheckBrokenParentsChain(benefitCard.ParentBenefitCard, values);
            return values.Contains(benefitCard.Id);
        }

        private void CheckBrokenParentsChain(BenefitCard benefitCard, ISet<int> values)
        {
            while (benefitCard != null && !values.Contains(benefitCard.Id))
            {
                values.Add(benefitCard.Id);
                CheckBrokenParentsChain(benefitCard.ParentBenefitCard, values);
            }
        }
        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var benefitCard = entity as HRIS.Domain.PayrollSystem.Configurations.BenefitCard;
            var primaryEmployeeBenefitCard = ServiceFactory.ORMService.All<PrimaryEmployeeBenefit>().Where(x => x.BenefitCard == benefitCard).ToList();

            var monthlyEmployeeBenefitCard = ServiceFactory.ORMService.All<MonthlyEmployeeBenefit>().Where(x => x.BenefitCard == benefitCard).ToList();

            if (primaryEmployeeBenefitCard.Count() > 0 || monthlyEmployeeBenefitCard.Count() > 0)

            {
                PreventDefault = true;
            }
        }
    }
}