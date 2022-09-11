#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 08/03/2015
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
using HRIS.Domain.Personnel.Configurations;
using HRIS.Domain.JobDescription.Entities;
#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class AssignmentViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AssignmentViewModel).FullName;
            model.ActionListHandler = "initializeAssignmentActionList";
            model.Views[0].EditHandler = "AssignmentEditHandler";
            model.Views[0].ViewHandler = "AssignmentViewHandler";
            //model.ActionList.Commands.RemoveAt(2);
            //model.ActionList.Commands.RemoveAt(1);
        }
        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            var position = new Position();
            if (customInformation != null)
            {
                var strPosition = customInformation.Split('"');
                foreach (var str in strPosition)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        var positionId = int.Parse(str);
                        if (positionId != 0)
                            position = ServiceFactory.ORMService.GetById<Position>(positionId);
                        break;
                    }

                }
                var assignment = (Assignment)entity;
                assignment.Position = position;
            }
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var assignment = (Assignment)entity;
            if (assignment.Position == null)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = GlobalResource.RequiredMessage,
                    Property = typeof(Assignment).GetProperty("Position")
                });
            }
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var assignment = (Assignment)entity;
            assignment.Creator = UserExtensions.CurrentUser;
            assignment.CreationDate = DateTime.Now;
            var currentUser = UserExtensions.CurrentUser;
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(requestInformation.NavigationInfo.Previous[0].RowId);
            employeeCard.Employee.Status = EmployeeStatus.InPosition;
            employeeCard.CardStatus=EmployeeCardStatus.OnHeadOfHisWork;
            if (employeeCard.Employee.Positions.Any())
            {
                var secondaryPosition = new AssigningEmployeeToPosition()
                {
                    Position = assignment.Position,
                    Employee = employeeCard.Employee,
                    IsPrimary = false,
                    Weight = 0,
                    CreationDate = DateTime.Now
                };
                var positionsLogOfEmployee = new PositionsLogOfEmployee()
                {
                    Position = assignment.Position,
                    JobDescription = assignment.Position?.JobDescription,
                    Employee = employeeCard.Employee,
                    IsPrimary = false,
                    AssigningDate = assignment.AssigningDate,
                    AssigmentType = HRIS.Domain.EmployeeRelationServices.Enums.AssigmentType.Assigment
                };
                employeeCard.Employee.AddEmployeePosition(secondaryPosition);
                employeeCard.Employee.AddPositionLogToEmployee(positionsLogOfEmployee);
                assignment.AssigningEmployeeToPosition = secondaryPosition;
                assignment.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Assigned);
                assignment.Position.JobDescription.JobTitle.Vacancies --;
            }
            else
            {
                var primaryPosition = new AssigningEmployeeToPosition()
                {
                    Position = assignment.Position,
                    Employee = employeeCard.Employee,
                    IsPrimary = true,
                    Weight = 100,
                    CreationDate = DateTime.Now
                };
                var positionsLogOfEmployee = new PositionsLogOfEmployee()
                {
                    Position = assignment.Position,
                    JobDescription = assignment.Position?.JobDescription,
                    Employee = employeeCard.Employee,
                    IsPrimary = true,
                    AssigningDate = assignment.AssigningDate,
                    AssigmentType = HRIS.Domain.EmployeeRelationServices.Enums.AssigmentType.Assigment
                };
                employeeCard.Employee.AddEmployeePosition(primaryPosition);
                employeeCard.Employee.AddPositionLogToEmployee(positionsLogOfEmployee);
                assignment.AssigningEmployeeToPosition = primaryPosition;
                assignment.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Assigned);
                assignment.Position.JobDescription.JobTitle.Vacancies --;
            }


            if (ServiceFactory.ORMService.All<EmployeeCodeSetting>().Any())
            {
                var employeeCodeSetting = ServiceFactory.ORMService.All<EmployeeCodeSetting>().First();
                employeeCard.Employee.Code = JobDescriptionHelper.GetCode(employeeCodeSetting, employeeCard.Employee);
            }
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { employeeCard }, currentUser);

        }
    }    
}