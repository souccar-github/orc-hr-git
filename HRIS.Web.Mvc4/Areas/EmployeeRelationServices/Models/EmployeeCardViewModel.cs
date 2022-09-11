#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 27/08/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.JobDescription.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Infrastructure.Extenstions;
using Souccar.Domain.Validation;
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;
#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeeCardViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeCardViewModel).FullName;
          //  model.ToolbarCommands.RemoveAt(0);

            model.ActionListHandler = "initializeEmployeeCardActionList";
            if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));
        }
        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employeeCard = entity as EmployeeCard;
            var machineCodeIsExist = ServiceFactory.ORMService.All<EmployeeCard>()
                .Any(x => x.Id != employeeCard.Id && x.EmployeeMachineCode == employeeCard.EmployeeMachineCode);
            if (machineCodeIsExist)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper
                    .GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeMachineCodeIsAlreadyExist),
                    Property = typeof(EmployeeCard).GetProperty("EmployeeMachineCode")

                });
            }
        }

        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            Type typeOfClass = typeof(EmployeeCard);
            var filteredDate = new List<EmployeeCard>();
            var allBeforeFilter = ((IQueryable<EmployeeCard>)result.Data).ToList();
            var user = UserExtensions.CurrentUser;
            if (UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result.Data = allBeforeFilter.Skip<EmployeeCard>(skip).Take<EmployeeCard>(pageSize).AsQueryable();
                result.Total = allBeforeFilter.Count();
            }
            else
            {
                var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
                filteredDate = allBeforeFilter.Where(x => employeesCanViewThem.Any(y => y.EmployeeCard.Id == x.Id)).ToList();
                result.Data = filteredDate.Skip<EmployeeCard>(skip).Take<EmployeeCard>(pageSize).AsQueryable();
                result.Total = filteredDate.Count();
            }
        }


    }
}
