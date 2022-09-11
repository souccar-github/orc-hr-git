#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 19/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.Enums;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Controllers;

#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeeInfractionViewModel : ViewModel
    {
        public object Servicecontroller { get; private set; }

        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeInfractionViewModel).FullName;
            model.Views[0].EditHandler = "EmployeeInfractionEditHandler";



        }
      
        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var start = DateTime.Now;
            var serviceController = new ServiceController();
            var employeeInfraction = (EmployeeInfraction)entity;
            var employeeDisciplinary = new EmployeeDisciplinary();
            var employeeInfractionCount = ServiceFactory.ORMService.All<EmployeeInfraction>().Where(x => x.Employee == employeeInfraction.Employee).Count();
             employeeInfraction.PenaltyName = serviceController.GetPenaltyFromInfraction(employeeInfraction.Infraction, employeeInfractionCount).Name;
            //employeeInfraction.PenaltyName= 
            employeeDisciplinary.DisciplinarySetting = serviceController.GetPenaltyFromInfraction(employeeInfraction.Infraction,  employeeInfractionCount);
            employeeDisciplinary.DisciplinaryDate= employeeInfraction.CreationDate; 
            employeeDisciplinary.DisciplinaryReason = EmployeeRelationServicesLocalizationHelper.Infraction ;
            employeeDisciplinary.EmployeeCard = employeeInfraction.Employee.EmployeeCard;
            employeeDisciplinary.CreationDate = employeeInfraction.CreationDate;
            employeeDisciplinary.IsTransferToPayroll = false;
            employeeDisciplinary.DisciplinaryStatus = HRIS.Domain.Global.Enums.Status.Approved;
            var info= EmployeeRelationServicesLocalizationHelper.EmployeeDisciplinary;
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { employeeDisciplinary }, UserExtensions.CurrentUser, employeeDisciplinary, Souccar.Domain.Audit.OperationType.Insert, info, start, new List<Entity>() { employeeDisciplinary.EmployeeCard.Employee });

        }
    }
}