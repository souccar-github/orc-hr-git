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
using  Project.Web.Mvc4.Areas.JobDescription.Helpers;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Infrastructure.Extenstions;
using Souccar.Domain.Validation;
#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class AssigningEmployeeToPositionViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AssigningEmployeeToPositionViewModel).FullName;
           model.ActionListHandler = "initializeAssigningEmployeeToPositionActionList";
            model.Views[0].AfterRequestEnd = "AssignEToPAfterRequestEndHandler";
          model.ToolbarCommands.RemoveAt(0);
        }

        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var assigningEmployeeToPosition = entity as AssigningEmployeeToPosition;
            var employee = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            var weight = employee.Positions.Where(x => x != assigningEmployeeToPosition).Sum(x => x.Weight);
            if ((weight + assigningEmployeeToPosition.Weight) > 100)
            {
                var thirdProp = typeof(AssigningEmployeeToPosition).GetProperty("Weight");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", thirdProp.GetTitle(), EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TotalWeightGreaterThan100)),
                        Property = thirdProp
                    });
            }
        }
    }    
}