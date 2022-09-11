//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using HRIS.Domain.PayrollSystem.Configurations;
//using HRIS.Domain.PayrollSystem.Entities;
//using HRIS.Domain.PayrollSystem.RootEntities;
//using HRIS.Validation.MessageKeys;
//using  Project.Web.Mvc4.Models;
//using Project.Web.Mvc4.Models.GridModel;
//using Souccar.Domain.DomainModel;
//using Souccar.Domain.Validation;
//using Souccar.Infrastructure.Core;
//using Souccar.Infrastructure.Extenstions;

//namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
//{

//    public class PrimaryEmployeeBenefitEventHandlers : ViewModel
//    {
//        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
//        {
//            model.Views[0].EditHandler = "PrimaryEmployeeBenefit_EditHandler";
//            model.ViewModelTypeFullName = typeof(PrimaryEmployeeBenefitEventHandlers).FullName;
//        }

//        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
//            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
//        {
//            var primaryEmployeeBenefit = (PrimaryEmployeeBenefit)entity;

//            if (typeof(BenefitCard).GetAll<BenefitCard>().Any(x => x.ParentBenefitCard.Id == primaryEmployeeBenefit.BenefitCard.Id))
//            {// تم وضعها هنا لتشمل تعويضات شهرية من البطاقة الشهرية او تعويضات شهرية من التغييرات الشهرية
//                validationResults.Add(new ValidationResult
//                {
//                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotSelectParentBenefit)),
//                    Property = typeof(PrimaryEmployeeBenefit).GetProperty("BenefitCard")
//                });
//            }
//        }
//    }

//}