using System;
using System.Linq;
using FluentNHibernate.Conventions;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Areas.PayrollSystem.EventHandlers;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PayrollSystem.Configurations;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Models
{
    public class PayrollSystemViewModel:ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
 

            #region PrimaryEmployeeDeduction
            if (type == typeof(PrimaryEmployeeDeduction))
            {
                //Call Method
                model.Views[0].EditHandler = "PrimaryEmployeeDeduction_EditHandler";
            }
            #endregion


            #region MonthlyCard
            if (type == typeof(MonthlyCard))
            {
                // يمنع الاضافة للبطاقة الشهرية لانها تتولد تلقائيا
                model.IsAddable = false;

                model.SchemaFields.SingleOrDefault(x => x.Name == "IsCalculated").Editable = false;
                model.Views[0].ViewHandler = "MonthlyCard_ViewHandler";

            }
            #endregion

            #region Employee
            if (type == typeof(Employee) && requestInformation.NavigationInfo.Module.Name == ModulesNames.PayrollSystem)
            {
                model.IsAddable = false;
                model.IsDeleteable = false;
                model.IsEditable = false;

            }
            #endregion

            #region Children
            if (type == typeof(Child) && requestInformation.NavigationInfo.Module.Name == ModulesNames.PayrollSystem)
            {
                model.Views[0].EditHandler = "PayrollSystemChildBenefit_EditHandler";
                model.IsAddable = false;
                model.IsDeleteable = false;
            }
            #endregion

            #region Spouse
            if (type == typeof(Spouse) && requestInformation.NavigationInfo.Module.Name == ModulesNames.PayrollSystem)
            {
                model.Views[0].EditHandler = "PayrollSystemSpouseBenefit_EditHandler";
                model.IsAddable = false;
                model.IsDeleteable = false;
            }
            #endregion

            #region LoanPayment
            if (type == typeof(LoanPayment))
            {
                if (requestInformation.NavigationInfo.Previous[0] != null && requestInformation.NavigationInfo.Previous[0].TypeName == typeof(Month).FullName)
                {
                    var primaryEmployeeCardId = ((MonthlyCard)typeof(MonthlyCard).GetById(requestInformation.NavigationInfo.Previous[1].RowId)).PrimaryCard.Id;

                    //Add Ref Field
                    GridViewModelFactory.AddRefField(model, "EmployeeLoan",
                        "PayrollSystem/DropDownListHelper/GetNotFinishedEmployeeLoans?primaryEmployeeCardId=" + primaryEmployeeCardId);
                    var month =
                         ServiceFactory.ORMService.GetById<Month>(requestInformation.NavigationInfo.Previous[0].RowId);
                    if (month.MonthStatus == MonthStatus.Approved ||
                        month.MonthStatus == MonthStatus.Locked)
                    {
                        foreach (var command in model.ActionList.Commands) //إخفاء كل خيارات الاكشن ليست ماعدا العرض
                        {
                            command.ShowCommand = command.Name ==
                                                  ServiceFactory.LocalizationService.GetResource(
                                                      GridModelLocalizationConst.ResourceGroupName +
                                                      "_" + GridModelLocalizationConst.View);
                        }

                        if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                            model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));
                    }

                    model.Views[0].EditHandler = "PayrollSystemEmpLoan_EditHandler";
                }
                else
                {
                    var primaryCardId = requestInformation.NavigationInfo.Previous[0].RowId;
                    //Add Ref Field
                    GridViewModelFactory.AddRefField(model, "MonthlyCard", "PayrollSystem/DropDownListHelper/GetMonthlyCardsForPrimaryCard?primaryCardId=" + primaryCardId);

                    //Call Method
                    model.Views[0].EditHandler = "PayrollSystemEmpLoan_EditHandler";
                }

                model.ViewModelTypeFullName = typeof(LoanPaymentEventHandlers).FullName;
                model.IsEditable = false;
            }
            #endregion
            #region GradeBenefitDetail
            if (type == typeof(GradeBenefitDetail))
            {
                model.Views[0].EditHandler = "GradeBenefitDetail_EditHandler";
            }
            #endregion

            #region GradeDeductionDetail
            if (type == typeof(GradeDeductionDetail))
            {
                model.Views[0].EditHandler = "GradeDeductionDetail_EditHandler";
            }
            #endregion

            #region JobDescriptionBenefitDetail
            if (type == typeof(JobDescriptionBenefitDetail))
            {
                model.Views[0].EditHandler = "JobDescriptionBenefitDetail_EditHandler";
            }
            #endregion

            #region JobDescriptionDeductionDetail
            if (type == typeof(JobDescriptionDeductionDetail))
            {
                model.Views[0].EditHandler = "JobDescriptonDeduction_EditHandler";
            }
            #endregion

            #region JobTitleBenefitDetail
            if (type == typeof(JobTitleBenefitDetail))
            {
                model.Views[0].EditHandler = "JobTitleBenefitDetail_EditHandler";
            }
            #endregion

            #region JobTitleDeductionDetail
            if (type == typeof(JobTitleDeductionDetail))
            {
                model.Views[0].EditHandler = "JobTitleDeductionDetail_EditHandler";
            }
            #endregion

            #region NodeBenefitDetail
            if (type == typeof(NodeBenefitDetail))
            {
                model.Views[0].EditHandler = "NodeBenefitDetail_EditHandler";
            }
            #endregion

            #region NodeDeductionDetail
            if (type == typeof(NodeDeductionDetail))
            {
                model.Views[0].EditHandler = "NodeDeductionDetail_EditHandler";
            }
            #endregion

            #region PositionBenefitDetail
            if (type == typeof(PositionBenefitDetail))
            {
                model.Views[0].EditHandler = "PositionBenefit_EditHandler";
            }
            #endregion

            #region PositionDeductionDetail
            if (type == typeof(PositionDeductionDetail))
            {
                model.Views[0].EditHandler = "PositionDeduction_EditHandler";
            }
            #endregion


        }
    }
    
}