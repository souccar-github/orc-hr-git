using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MonthlyEmployeeDeductionEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            var MonthlyCard = ServiceFactory.ORMService.GetById<MonthlyCard>(requestInformation.NavigationInfo.Previous[1].RowId);

            if (MonthlyCard.Month.MonthStatus==MonthStatus.Approved || MonthlyCard.Month.MonthStatus == MonthStatus.Locked)
            {
               
                model.ToolbarCommands.RemoveAt(0);
            }
                model.ViewModelTypeFullName = typeof(MonthlyEmployeeDeductionEventHandlers).FullName;
            model.SchemaFields.SingleOrDefault(x => x.Name == "FinalValue").Editable = false;
            model.SchemaFields.SingleOrDefault(x => x.Name == "InitialValue").Editable = false;

            //if (requestInformation.NavigationInfo.Previous[1] != null && requestInformation.NavigationInfo.Previous[1].TypeName == typeof(MonthDeductionChange).FullName)
            //{
            //    var monthId = requestInformation.NavigationInfo.Previous[0].RowId;
            //    //Add Ref Field
            //    GridViewModelFactory.AddRefField(model, "MonthlyCard", "PayrollSystem/DropDownListHelper/GetMonthlyCardsForMonth?monthId=" + monthId);

            //    //Call Method
            //    model.Views[0].EditHandler = "MonthDeductionChange_MonthlyEmployeeDeduction_EditHandler";
            //}
            //else
            //{
                //Call Method
                model.Views[0].EditHandler = "MonthlyCard_MonthlyEmployeeDeduction_EditHandler";
                model.Views[0].ViewHandler = "MonthlyCard_MonthlyEmployeeDeduction_ViewHandler";
          
        }

       
      
           
        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var monthlyEmployeeDeduction = (MonthlyEmployeeDeduction)entity;
            MonthService.SetMonthlyCardStatusToUnCalculated(monthlyEmployeeDeduction.MonthlyCard.Id);
        }

        public override void AfterUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            string customInformation = null)
        {
            var monthlyEmployeeDeduction = (MonthlyEmployeeDeduction)entity;
            MonthService.SetMonthlyCardStatusToUnCalculated(monthlyEmployeeDeduction.MonthlyCard.Id);
        }

        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var monthlyEmployeeDeduction = (MonthlyEmployeeDeduction)entity;
            MonthService.SetMonthlyCardStatusToUnCalculated(monthlyEmployeeDeduction.MonthlyCard.Id);
        }
    }
}