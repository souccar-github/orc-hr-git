using System.Collections;
using System.Web.Security;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using NHibernate.Hql.Ast.ANTLR.Tree;
using Souccar.Core.Fasterflect;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;
using Souccar.Domain.Security;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Areas.JobDescription.Helpers;
using Project.Web.Mvc4.Areas.Security.Helpers;
using HRIS.Domain.Personnel.Configurations;
using Souccar.Domain.Audit;
using Project.Web.Mvc4.Factories;
using Souccar.Core.Extensions;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class EmployeeViewModel : ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var employee = entity as Employee;
            var emp = EmployeeExtensions.GetEmployeeByCode(employee.Code);


            //var user = UserExtensions.GetUserByUsername(employee.Id.ToString());
            //if (user != null && employee.User() != user)
            //{
            //    var prop = typeof(Employee).GetProperty("Code");
            //    validationResults.Add(
            //    new ValidationResult()
            //    {
            //        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
            //        Property = prop
            //    });
            //}

            emp = EmployeeExtensions.GetEmployeeByIdentificationNo((entity as Employee).IdentificationNo);
            if (emp != null && emp.Id != entity.Id)
            {
                var prop = typeof(Employee).GetProperty("IdentificationNo");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage),
                        Property = prop
                    });
            }

            if (employee.OtherNationalityExist && employee.OtherNationality == null)
            {
                var prop = typeof(Employee).GetProperty("OtherNationality");
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage),
                        Property = prop
                    });
            }

            if (employee.DisabilityExist)
            {
                if (employee.DisabilityType == null)
                {
                    var prop = typeof(Employee).GetProperty("DisabilityType");
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage),
                            Property = prop
                        });
                }
            }

        }
        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }

        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            Type typeOfClass = typeof(Employee);
            var filteredDate = new List<Employee>();
            var allBeforeFilter = ((IQueryable<Employee>)result.Data).ToList();
            var user = UserExtensions.CurrentUser;
            if (UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result.Data = allBeforeFilter.Skip<Employee>(skip).Take<Employee>(pageSize).AsQueryable();
                result.Total = allBeforeFilter.Count();
            }
            else
            {
                var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
                filteredDate = allBeforeFilter.Where(x => employeesCanViewThem.Any(y => y.Id == x.Id)).ToList();
                result.Data = filteredDate.Skip<Employee>(skip).Take<Employee>(pageSize).AsQueryable();
                result.Total = filteredDate.Count();
            }
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var emp = (Employee)entity;
            var financialCard = new FinancialCard
            {
                Employee = emp,
                SalaryDeservableType = SalaryDeservableType.Nothing
            };

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { emp, financialCard }, UserExtensions.CurrentUser);

            var empCard = new EmployeeCard { CardStatus = EmployeeCardStatus.New, FinancialCard = financialCard, Employee = emp, ProbationPeriodEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0) };
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { empCard }, UserExtensions.CurrentUser);
            if (ServiceFactory.ORMService.All<EmployeeCodeSetting>().Any())
            {
                var employeeCodeSetting = ServiceFactory.ORMService.All<EmployeeCodeSetting>().First();
                emp.Code = JobDescriptionHelper.GetCode(employeeCodeSetting, emp);
            }

            if (emp.Gender == 0)
            {
                emp.MilitaryStatus = MilitaryStatus.Nothing;
            }

            if (!emp.OtherNationalityExist)
            {

                emp.OtherNationality = null;
            }
            UserHelper.CreateUser(emp, emp.Username, UserHelper.DefaultPassword, false, false);
            var user = UserHelper.GetUserByUsername(emp.Username);
            emp.User = user;
            emp.Save();
            //PreventDefault = true;
        }

        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var emp = (Employee)entity;
            //PreventDefault = true;

            if (emp.Gender == 0)
            {
                emp.MilitaryStatus = MilitaryStatus.Nothing;
            }

            if (!emp.OtherNationalityExist)
            {

                emp.OtherNationality = null;
            }

            UserHelper.UpdateUser(emp, emp.Username, UserHelper.DefaultPassword);
            emp.Save();

        }

        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            PreventDefault = true;
            var emp = (Employee)entity;
            UserHelper.DeleteUser(emp, emp.Username);
            emp.IsVertualDeleted = true;
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { emp }, UserExtensions.CurrentUser, emp, OperationType.Delete);


        }



        public List<Node> GetNodes(int? Parentid)
        {

            if (Parentid == null)
            {
                var result = new List<Node>();
                result = ServiceFactory.ORMService.All<Node>().Where(x => x.Parent == null && !x.IsHistorical).ToList<Node>();
                var temp = result;
                foreach (var node in temp.ToList())
                {
                    if (((Node)node).Children.Count > 0)
                    {
                        result.InsertRange(result.IndexOf(node) + 1, GetNodes(((Node)node).Id));
                    }
                }
                return result;
            }
            else
            {
                var result = new List<Node>();
                result = (ServiceFactory.ORMService.GetById<Node>((int)Parentid)).Children.Where(x => !x.IsHistorical).ToList<Node>();
                var temp = result;
                foreach (var node in temp.ToList())
                {
                    if (((Node)node).Children.Count > 0)
                    {
                        result.InsertRange(result.IndexOf(node) + 1, GetNodes(((Node)node).Id));
                    }
                }
                return result;
            }


        }
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EmployeeViewModel).FullName;
            model.Views[0].ViewHandler = "onViewEmployee";
            model.Views[0].EditHandler = "onEditEmployee";
            model.Views[0].Columns.SingleOrDefault(x => x.FieldName == "PhotoPath").Title = " ";
            model.ActionListHandler = "showOrHideActivateEmployee";
            model.Views[0].ShowTwoColumns = true;
        }
    }

    public class LogOfPositionsViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(LogOfPositionsViewModel).FullName;
            var editLocalizationName = ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + GridModelLocalizationConst.Edit) ?? GridModelLocalizationConst.Edit.ToCapitalLetters();
            var deleteLocalizationName = ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + GridModelLocalizationConst.Delete) ?? GridModelLocalizationConst.Delete.ToCapitalLetters();
            if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));
            if (model.ActionList.Commands.Any(x => x.Name == editLocalizationName))
                model.ActionList.Commands.Remove(model.ActionList.Commands.FirstOrDefault(x => x.Name == editLocalizationName));
            if (model.ActionList.Commands.Any(x => x.Name == deleteLocalizationName))
                model.ActionList.Commands.Remove(model.ActionList.Commands.FirstOrDefault(x => x.Name == deleteLocalizationName));
        }
    }
}

