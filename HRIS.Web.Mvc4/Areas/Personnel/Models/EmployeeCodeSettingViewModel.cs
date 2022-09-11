#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 30/04/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference

using FluentNHibernate.Data;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.JobDescription.Helpers;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Entity = Souccar.Domain.DomainModel.Entity;
using HRIS.Domain.Personnel.Configurations;
using Project.Web.Mvc4.Extensions;

#endregion
namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class EmployeeCodeSettingViewModel: ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeCodeSettingViewModel).FullName;

        }
        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employeeCodeSettings = entity as EmployeeCodeSetting;
            var codeSetting = ServiceFactory.ORMService.All<EmployeeCodeSetting>();
            if (codeSetting.Any(x => x.Id != employeeCodeSettings.Id))
            {
               
                validationResults.Add(new ValidationResult()
                {
                    Message = string.Format("{0} {1}", "", GlobalResource.YouCanNotAddMoreThenOneCodeSetting),
                    Property = null
                });
            }
        }

        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var employeeCodeSetting = entity as EmployeeCodeSetting;
            var employees = ServiceFactory.ORMService.All<Employee>();
            foreach (var employee in employees)
            {
                employee.Code = JobDescriptionHelper.GetCode(employeeCodeSetting, employee);
                employee.Save();     
            }

        }

        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            string customInformation = null)
        {
            var employees = ServiceFactory.ORMService.All<Employee>();
            foreach (var employee in employees)
            {
                if (ServiceFactory.ORMService.All<EmployeeCodeSetting>().Any())
                {
                    var employeeCodeSetting = ServiceFactory.ORMService.All<EmployeeCodeSetting>().First();
                    employee.Code = JobDescriptionHelper.GetCode(employeeCodeSetting, employee);
                }
            }
        }
    }
}