using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PayrollSystem.Configurations;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class GeneralOptionEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(GeneralOptionEventHandlers).FullName;
            model.Views[0].EditHandler = "GeneralOptionAdvance_EditHandler";
            model.Views[0].ViewHandler = "GeneralOptionAdvance_ViewHandler";
            
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            //if (entity.IsTransient())
            //{
            //    var recordCount = typeof(GeneralOption).GetAll<GeneralOption>().Count();
            //    if (recordCount >= 1)
            //    {
            //        validationResults.Add(new ValidationResult
            //        {
            //            Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.MoreThanOneRowNotAllowed)),
            //            Property = null
            //        });
            //    }
            //}

            var generalOption = (GeneralOption)entity;
            if(!generalOption.Salary && !generalOption.InsuranceSalary && !generalOption.BenefitSalary && !generalOption.TempSalary1 && !generalOption.TempSalary2)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.YouMustCheckOneOfSalaryFiledsAsPackageSalary)),
                    Property = null
                });
            }
            //if (typeof(BenefitCard).GetAll<BenefitCard>().Any(x => x.ParentBenefitCard.Id == generalOption.FamilyBenefit.Id))
            //{// تذكر في حال وجود تعويضات اخرى غير العائلي ان يتم التأكد أنه لم يتم اختيار تعويض هو تعويض اب لغيره من التعويضات
            //    validationResults.Add(new ValidationResult
            //    {
            //        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotSelectParentBenefit)),
            //        Property = typeof(GeneralOption).GetProperty("FamilyBenefit")
            //    });
            //}
        }

        public override ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
        {
            var recordCount = ServiceFactory.ORMService.All<GeneralOption>().Count();
            if (recordCount >= 1)
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = false, message = CustomMessageKeysPayrollSystemModule.MoreThanOneRowNotAllowed });
           else
               return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = "" });
            
        }
    }
}