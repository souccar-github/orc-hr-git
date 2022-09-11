
using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PayrollSystem.Entities;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Factories;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers
{

    public class FinancialCardEventHandler : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
           // model.Views[0].EditHandler = "FinancialCardEditHandler";
            model.ViewModelTypeFullName = typeof(FinancialCardEventHandler).FullName;
            var deleteLocalizationName = ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + GridModelLocalizationConst.Delete) ?? GridModelLocalizationConst.Delete.ToCapitalLetters();

            if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));
            if (model.ActionList.Commands.Any(x => x.Name == deleteLocalizationName))
                model.ActionList.Commands.Remove(model.ActionList.Commands.FirstOrDefault(x => x.Name == deleteLocalizationName));
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var benefitCard = (FinancialCard)entity;

            
        }
        
    }
}