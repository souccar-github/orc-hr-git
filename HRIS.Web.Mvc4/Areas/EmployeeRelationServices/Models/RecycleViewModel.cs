using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Testing.Values;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using  Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.Personnel.Enums;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class RecycleViewModel : ViewModel
    {

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RecycleViewModel).FullName;
            model.ActionListHandler = "RecycleActionListHandler";
            model.Views[0].ViewHandler = "RecycleViewHandler";

            //model.Views[0].AfterRequestEnd = "RecycledLeaveAfterRequestEnd";
          


        }

        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(requestInformation.NavigationInfo.Previous[0].RowId);
            var employees = ServiceFactory.ORMService.All<Employee>();
            var recycle = (Recycle)entity;

            foreach (var employee in employees)
            {
                var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);
                if (employeeCard != null && employeeCard.LeaveTemplateMaster != null &&
                    employeeCard.LeaveTemplateMaster.LeaveTemplateDetails.Any(x => x.LeaveSetting == leaveSetting) && 
                    employeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork &&
                    !(employeeCard.RecycledLeaves != null &&
                    employeeCard.RecycledLeaves.Any(x=> x.Year == recycle.Year)) &&
                    employeeCard.StartWorkingDate.Value.Year <= recycle.Year )
                {

                    var balance = LeaveService.GetBalance(recycle.LeaveSetting, employee,true,DateTime.Today);
                    var recycledBalance = LeaveService.GetRecycledBalance(employee, recycle.LeaveSetting, recycle.Year - 1);
                    var granted = LeaveService.GetGranted(employee, recycle.LeaveSetting, recycle.Year);
                    var remain = (balance + recycledBalance) - granted;
                    var roundedBalance = Math.Round(remain * (leaveSetting.RoundPercentage / 100), 2);
                    var newRecycledLeave = new RecycledLeave()
                    {
                        EmployeeCard = employeeCard,
                        LeaveSetting = leaveSetting,
                        RecycleType = recycle.RecycleType,
                        RoundedBalance = roundedBalance,
                        Year = recycle.Year
                    };
                    employeeCard.AddRecycledLeave(newRecycledLeave);
                    ServiceFactory.ORMService.Save(employeeCard, UserExtensions.CurrentUser);
                }
            }
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var recycle = (Recycle)entity;
            var leaveSetting =  ServiceFactory.ORMService.GetById<LeaveSetting>(requestInformation.NavigationInfo.Previous[0].RowId);

            if (operationType == CrudOperationType.Insert)
            {
                var recyclesCount = leaveSetting.Recycles.Where(x => x.Id != recycle.Id).Count(x => x.Year == recycle.Year);

                if (recyclesCount == 1)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgCannotAddMoreRecycleForThisLeaveSettingInYear),
                        Property = typeof(Recycle).GetProperty("Year")
                    });

                    return;
                }

                if (leaveSetting.RoundPercentage == 0)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotRecycleThisLeaveBecauseRoundPercentageIsZero),
                        Property = null
                    });

                    return;
                }

            }
            else if (operationType == CrudOperationType.Delete)
            {
                if (recycle.Year < leaveSetting.Recycles.Max(y => y.Year))
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouMustDeleteRecycleLeaveBalanceFromNewestToOldest),
                        Property = typeof(Recycle).GetProperty("Year")
                    });

                    return;
                }
            }
        }

        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(requestInformation.NavigationInfo.Previous[0].RowId);
            var recycle = (Recycle)entity;
            if (recycle.Year < leaveSetting.Recycles.Max(y => y.Year))
            {
                PreventDefault = true;
            }
            else
            {
                var employees = ServiceFactory.ORMService.All<Employee>();
                foreach (var employee in employees)
                {
                    if (employee.EmployeeCard != null && employee.EmployeeCard.LeaveTemplateMaster != null && employee.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork &&
                        employee.EmployeeCard.StartWorkingDate.Value.Year <= recycle.Year &&
                        employee.EmployeeCard.LeaveTemplateMaster.LeaveTemplateDetails.Any(x => x.LeaveSetting == leaveSetting))
                    {
                        var recycledLeave =
                            employee.EmployeeCard.RecycledLeaves.Where(x => x.Year == recycle.Year).ToList();

                        double roundedBalance = 0;
                        for (int i = recycledLeave.Count - 1; i >= 0; i--)
                        {
                            roundedBalance += recycledLeave[i].RoundedBalance;
                            recycledLeave[i].EmployeeCard = null;
                            recycledLeave[i].LeaveSetting = null;
                            employee.EmployeeCard.RecycledLeaves.Remove(recycledLeave[i]);
                            employee.EmployeeCard.Save();
                        }
                        var lastRecycledLeave =
                            employee.EmployeeCard.RecycledLeaves.FirstOrDefault(
                                x => x.Year == employee.EmployeeCard.RecycledLeaves.Max(y => y.Year));
                        if (lastRecycledLeave != null)
                        {
                            lastRecycledLeave.RoundedBalance -= roundedBalance;
                        }
                        employee.EmployeeCard.Save();
                    }
                }                
            }
        }

        //public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        //{
        //    var employees = ServiceFactory.ORMService.All<Employee>();
        //    var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(requestInformation.NavigationInfo.Previous[0].RowId);
        //    var recycle = (Recycle)entity;
        //    if (recycle.Year >= leaveSetting.Recycles.Max(y => y.Year))
        //    {
        //        foreach (var employee in employees)
        //        {
        //            if (employee.EmployeeCard != null)
        //            {
        //                var recycledLeave =
        //                    employee.EmployeeCard.RecycledLeaves.Where(x => x.Year == recycle.Year).ToList();

        //                double roundedBalance = 0;
        //                for (int i = recycledLeave.Count - 1; i >= 0; i--)
        //                {
        //                    roundedBalance += recycledLeave[i].RoundedBalance;
        //                    recycledLeave[i].EmployeeCard = null;
        //                    recycledLeave[i].LeaveSetting = null;
        //                    employee.EmployeeCard.RecycledLeaves.Remove(recycledLeave[i]);
        //                    employee.EmployeeCard.Save();
        //                }
        //                var lastRecycledLeave =
        //                    employee.EmployeeCard.RecycledLeaves.FirstOrDefault(
        //                        x => x.Year == employee.EmployeeCard.RecycledLeaves.Max(y => y.Year));
        //                if (lastRecycledLeave != null)
        //                {
        //                    lastRecycledLeave.RoundedBalance -= roundedBalance;
        //                }
        //                employee.EmployeeCard.Save();
        //            }
        //        }
        //    }
        //}
    }
}