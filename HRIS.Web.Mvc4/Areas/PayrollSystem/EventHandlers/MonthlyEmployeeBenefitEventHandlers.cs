using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Areas.PayrollSystem.Services;
using  Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Factories;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{
    public class MonthlyEmployeeBenefitEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            var MonthlyCard = ServiceFactory.ORMService.GetById<MonthlyCard>(requestInformation.NavigationInfo.Previous[1].RowId);

            if (MonthlyCard.Month.MonthStatus == MonthStatus.Approved || MonthlyCard.Month.MonthStatus == MonthStatus.Locked)
            {
                
                model.ToolbarCommands.RemoveAt(0);
            }
            model.ViewModelTypeFullName = typeof(MonthlyEmployeeBenefitEventHandlers).FullName;
            model.SchemaFields.SingleOrDefault(x => x.Name == "FinalValue").Editable = false;
            model.SchemaFields.SingleOrDefault(x => x.Name == "InitialValue").Editable = false;
            model.ViewModelTypeFullName = typeof(MonthlyEmployeeBenefitEventHandlers).FullName;
            //if (requestInformation.NavigationInfo.Previous[1] != null && requestInformation.NavigationInfo.Previous[1].TypeName == typeof(MonthBenefitChange).FullName)
            //{
            //    var monthId = requestInformation.NavigationInfo.Previous[0].RowId;
            //    //Add Ref Field
            //    GridViewModelFactory.AddRefField(model, "MonthlyCard", "PayrollSystem/DropDownListHelper/GetMonthlyCardsForMonth?monthId=" + monthId);
            //    //Call Method
            //    model.Views[0].EditHandler = "MonthBenefitChange_MonthlyEmployeeBenefit_EditHandler";
            //}
            
                //Call Method
                model.Views[0].EditHandler = "MonthlyCard_MonthlyEmployeeBenefit_EditHandler";
                model.Views[0].ViewHandler = "MonthlyCard_MonthlyEmployeeBenefit_ViewHandler";
            
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var monthlyEmployeeBenefit = (MonthlyEmployeeBenefit)entity;
            //if (requestInformation.NavigationInfo.Previous[1] != null &&
            //    requestInformation.NavigationInfo.Previous[1].TypeName == typeof(MonthBenefitChange).FullName)
            //{
            //    if (monthlyEmployeeBenefit.MonthlyCard == null || monthlyEmployeeBenefit.MonthlyCard.IsTransient())
            //    {
            //        validationResults.Add(new ValidationResult
            //        {
            //            Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule
            //                .GetFullKey(CustomMessageKeysPayrollSystemModule.MonthlyCardIsRequired)),
            //            Property = typeof(MonthlyEmployeeBenefit).GetProperty("MonthlyCard")
            //        });
            //    }
            //}

            if (typeof(BenefitCard).GetAll<BenefitCard>().Any(x => x.ParentBenefitCard.Id == monthlyEmployeeBenefit.BenefitCard.Id))
            {// تم وضعها هنا لتشمل تعويضات شهرية من البطاقة الشهرية او تعويضات شهرية من التغييرات الشهرية
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotSelectParentBenefit)),
                    Property = typeof(MonthlyEmployeeBenefit).GetProperty("BenefitCard")
                });
            }
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            
        }


        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var monthlyEmployeeBenefit = (MonthlyEmployeeBenefit)entity;
            MonthService.SetMonthlyCardStatusToUnCalculated(monthlyEmployeeBenefit.MonthlyCard.Id);
        }

        public override void AfterUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            string customInformation = null)
        {
            var monthlyEmployeeBenefit = (MonthlyEmployeeBenefit)entity;
            MonthService.SetMonthlyCardStatusToUnCalculated(monthlyEmployeeBenefit.MonthlyCard.Id);
        }

        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var monthlyEmployeeBenefit = (MonthlyEmployeeBenefit)entity;
            MonthService.SetMonthlyCardStatusToUnCalculated(monthlyEmployeeBenefit.MonthlyCard.Id);
        }
    }

}