using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Enums;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using NHibernate.Mapping;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Services
{
    public static class SelfService
    {
        public static void Transfer(EmployeeTransfer transfer, EmployeeCard employeeCard)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var currentUser=UserExtensions.CurrentUser;
            transfer.Creator = currentUser;
            transfer.CreationDate = DateTime.Now;
            transfer.SourcePosition.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
            transfer.SourcePosition.JobDescription.JobTitle.Vacancies++;
            transfer.DestinationPosition.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Assigned);
            transfer.DestinationPosition.JobDescription.JobTitle.Vacancies--;
            employeeCard.AddEmployeeTransfer(transfer);
            var ep = transfer.SourcePosition.AssigningEmployeeToPosition;
            ep.Position = transfer.DestinationPosition;
            transfer.DestinationPosition.AssigningEmployeeToPosition = ep;
            transfer.SourcePosition.AssigningEmployeeToPosition = null;
            var historyOfPrimaryPosition = employeeCard.Employee.LogOfPositions.FirstOrDefault(x => x.IsPrimary && x.Position.Id == transfer.SourcePosition.Id);
            var positionsLogOfEmployee = new PositionsLogOfEmployee()
            {
                Position = transfer.DestinationPosition,
                JobDescription = transfer.DestinationPosition?.JobDescription,
                Employee = employeeCard.Employee,
                IsPrimary = ep.IsPrimary,
                AssigningDate = transfer.StartingDate,
                AssigmentType = HRIS.Domain.EmployeeRelationServices.Enums.AssigmentType.Transfer
            };
            employeeCard.Employee.AddPositionLogToEmployee(positionsLogOfEmployee);
            ep.CreationDate = DateTime.Now;
            entities.Add(transfer);
            entities.Add(ep);
            if (historyOfPrimaryPosition != null)
            {
                historyOfPrimaryPosition.DisengagementType = HRIS.Domain.EmployeeRelationServices.Enums.DisengagementType.DisengageFromTransfer;
                historyOfPrimaryPosition.LeavingDate = transfer.LeavingDate;
                entities.Add(historyOfPrimaryPosition);
            }
           // ServiceFactory.ORMService.SaveTransaction(entities, currentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeTransfer);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, transfer, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { transfer.EmployeeCard.Employee });
        }
        public static void EndJobSecondPosition(EndingSecondaryPositionEmployee endSecondaryPosition, EmployeeCard employeeCard)
        {
            var entities = new List<IAggregateRoot>();
            var start = DateTime.Now;
            var currentUser = UserExtensions.CurrentUser;
            endSecondaryPosition.Creator = currentUser;
            var positions = endSecondaryPosition.EmployeeCard.Employee.Positions;
            var position = endSecondaryPosition.Position.AssigningEmployeeToPosition;
            var historyOfPrimaryPosition = employeeCard.Employee.LogOfPositions.FirstOrDefault(x => !x.IsPrimary && x.Position.Id == endSecondaryPosition.Position.Id);
            position.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
            position.Position.JobDescription.JobTitle.Vacancies++;
            var assignment =
                    ServiceFactory.ORMService.All<Assignment>()
                        .SingleOrDefault(x => x.AssigningEmployeeToPosition == position);
            position.Position.AssigningEmployeeToPosition = null;

            if (assignment != null)
                assignment.AssigningEmployeeToPosition = null;
            position.Position = null;
            positions.Remove(position);
            employeeCard.AddEndingSecondaryPosition(endSecondaryPosition);
            entities.Add(endSecondaryPosition.EmployeeCard.Employee);
            entities.Add(assignment);
            entities.Add(position);
            if (historyOfPrimaryPosition != null)
            {
                historyOfPrimaryPosition.DisengagementType = HRIS.Domain.EmployeeRelationServices.Enums.DisengagementType.DisengageFromSecondaryPosition;
                historyOfPrimaryPosition.LeavingDate = endSecondaryPosition.LeavingDate;
                entities.Add(historyOfPrimaryPosition);
            }
            //ServiceFactory.ORMService.SaveTransaction(entities, currentUser);
           
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EndSecondaryPosition);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, endSecondaryPosition, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { endSecondaryPosition.EmployeeCard.Employee });

        }

        public static void Promotion(EmployeePromotion promotion,EmployeeCard employeeCard)
        {
            var entities = new List<IAggregateRoot>();
            var currentUser = UserExtensions.CurrentUser;
            promotion.Creator = currentUser;
            promotion.CreationDate = DateTime.Now;
            employeeCard.AddEmployeePromotion(promotion);

            var primaryPosition = employeeCard.Employee.Positions.SingleOrDefault(x => x.IsPrimary);
            var historyOfPrimaryPosition = employeeCard.Employee.LogOfPositions.FirstOrDefault(x => x.IsPrimary && x.Position.Id == primaryPosition.Position.Id);
            
            promotion.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Assigned);
            promotion.Position.JobDescription.JobTitle.Vacancies--;

            if (primaryPosition != null)
            {
                historyOfPrimaryPosition.DisengagementType = HRIS.Domain.EmployeeRelationServices.Enums.DisengagementType.DisengageFromPromotion;
                primaryPosition.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
                primaryPosition.Position.JobDescription.JobTitle.Vacancies++;
            }

            var ep=primaryPosition.Position.AssigningEmployeeToPosition;
            primaryPosition.Position.AssigningEmployeeToPosition = null;
            ep.Position = promotion.Position;
            promotion.Position.AssigningEmployeeToPosition = ep;
            ep.CreationDate = DateTime.Now;
            var positionsLogOfEmployee = new PositionsLogOfEmployee()
            {
                Position = promotion.Position,
                JobDescription = promotion.Position?.JobDescription,
                Employee = employeeCard.Employee,
                IsPrimary = ep.IsPrimary,
                AssigningDate = promotion.PositionJoiningDate,
                AssigmentType = HRIS.Domain.EmployeeRelationServices.Enums.AssigmentType.Promotion
            };
            employeeCard.Employee.AddPositionLogToEmployee(positionsLogOfEmployee);
            promotion.PromotionStatus = Status.Approved;
            entities.Add(employeeCard);
            entities.Add(employeeCard.Employee);
            entities.Add(ep);
            if (historyOfPrimaryPosition != null)
            {
                historyOfPrimaryPosition.LeavingDate = promotion.PositionSeparationDate;
                entities.Add(historyOfPrimaryPosition);
            }
            ServiceFactory.ORMService.SaveTransaction(entities, currentUser);
        }
    }
}