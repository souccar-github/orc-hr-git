using System;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;

using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Resources.Shared.Messages;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Audit;
using Project.Web.Mvc4.Helpers.Resource;
using System.Collections.Generic;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
{
    public class BenefitDeductionServiceController : Controller
    {
        public ActionResult AddBenefitToEmployees()
        {
            return PartialView("../Service/AddBenefitToEmployees");
        }

        public ActionResult AddDeductionToEmployees()
        {
            return PartialView("../Service/AddDeductionToEmployees");
        }

        [HttpPost]
        public ActionResult ApplyAddBenefitToEmployees(
            string filterBy,
            int benefitCardId,
            double value,
            double extraValue,
            double ceilValue,
            Formula formula,
            ExtraValueFormula extraValueFormula,
            Formula ceilFormula,
            bool isForMonthlyCards,
            bool isForEmployeeHasTheSameBenefit,
            int monthId,
            ConflictOption conflictOption,
            GridFilter filter = null)
        {// تذكر أنه لا يمكن اعطاء موظف اي تعويض هو تعويض أب لتعويضات اخرى
            string message;
            DateTime start = DateTime.Now;
            var benefitCard = (BenefitCard)typeof(BenefitCard).GetById(benefitCardId);
            if(isForMonthlyCards ==false && isForEmployeeHasTheSameBenefit ==false)
            {
               

                message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.ChooseIsForMonthlyCardsOrIsForEmployeeHasTheSameBenefit));
                    return Json(new
                    {
                        Success = false,
                        Msg = message,
                    });
               

            }
            if (typeof(BenefitCard).GetAll<BenefitCard>().Any(x => x.ParentBenefitCard.Id == benefitCard.Id))
            {
                message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotSelectParentBenefit));
                return Json(new
                {
                    Success = false,
                    Msg = message,
                });
            }


            Type entityType = null;
            IQueryable<IEntity> allData = null;

            if (filterBy == "FilterByEmployee")
            {
                entityType = typeof(Employee);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<Employee>();
            }
            else if (filterBy == "FilterByPrimaryCard")
            {
                entityType = typeof(EmployeeCard);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<EmployeeCard>();
            }
            else if (filterBy == "FilterByGrade")
            {
                entityType = typeof(HRIS.Domain.Grades.RootEntities.Grade);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.Grades.RootEntities.Grade>();
            }
            else if (filterBy == "FilterByJobTitle")
            {
                entityType = typeof(JobTitle);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<JobTitle>();
            }
            else if (filterBy == "FilterByJobDescription")
            {
                entityType = typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.JobDescription.RootEntities.JobDescription>();
            }
            else if (filterBy == "FilterByPosition")
            {
                entityType = typeof(Position);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<Position>();
            }
            else if (filterBy == "FilterByNode")
            {
                entityType = typeof(Node);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<Node>();
            }
            else if (filterBy == "FilterByMajorType")
            {
                entityType = typeof(MajorType);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<MajorType>();
            }
            else if (filterBy == "FilterByMajor")
            {
                entityType = typeof(Major);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<Major>();
            }

            var queryablePrimaryCards = FilterController.GetRelatedPrimaryCards(entityType, allData, filter);

            if (isForEmployeeHasTheSameBenefit)
            {
                queryablePrimaryCards = FilterController.GetRelatedPrimaryCardsWithBenefit(queryablePrimaryCards, benefitCard);
            }


            if (isForMonthlyCards)
            {
                var month = (Month)typeof(Month).GetById(monthId);
                if (month.MonthStatus == MonthStatus.Approved || month.MonthStatus == MonthStatus.Locked)
                {
                    message = ServiceFactory.LocalizationService
                        .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotDoActionsOnLockedOrApprovedMonths));
                    return Json(new
                    {
                        Success = false,
                        Msg = message,
                    });
                }

                var filteredMonthlyCards = month.MonthlyCards.Where(x => queryablePrimaryCards.Any(y => y.Id == x.PrimaryCard.Id));

                foreach (var filteredmonthlyCard in filteredMonthlyCards)
                {
                    var isBenefitExists = filteredmonthlyCard.MonthlyEmployeeBenefits.Any(x => x.BenefitCard.Id == benefitCard.Id);
                    var canAdd = true;
                    switch (conflictOption)
                    {
                        case ConflictOption.KeepExists:
                            if (isBenefitExists)
                            {
                                canAdd = false;
                            }
                            break;
                        case ConflictOption.ReplaceIfExists:
                            if (isBenefitExists)
                            {
                                var monthlyEmployeeBenefits = filteredmonthlyCard.MonthlyEmployeeBenefits;

                                for (int i = 0; i < monthlyEmployeeBenefits.Count; i++)
                                {
                                    if (monthlyEmployeeBenefits[i].BenefitCard.Id == benefitCard.Id)
                                    {
                                        monthlyEmployeeBenefits.RemoveAt(i);
                                        i--;
                                    }
                                }
                            }
                            break;
                    }

                    if (canAdd)
                    {
                        filteredmonthlyCard.AddMonthlyEmployeeBenefit(new MonthlyEmployeeBenefit
                        {
                            BenefitCard = benefitCard,
                            Value = value,
                            Formula = formula,
                            ExtraValue = extraValue,
                            ExtraValueFormula = extraValueFormula,
                            CeilValue = ceilValue,
                            CeilFormula = ceilFormula
                        });
                        filteredmonthlyCard.Save();
                    }
                }
            }
            else
            {
                var filteredPrimaryCards = queryablePrimaryCards;

                foreach (var filteredPrimaryCard in filteredPrimaryCards)
                {
                    var benfitCardIncluded = filteredPrimaryCard.PrimaryEmployeeBenefits.FirstOrDefault(x => x.BenefitCard.Id == benefitCard.Id);
                    var isBenefitExists = filteredPrimaryCard.PrimaryEmployeeBenefits.Any(x => x.BenefitCard.Id == benefitCard.Id);
                    var canAdd = true;
                    switch (conflictOption)
                    {
                        case ConflictOption.KeepExists:
                            if (isBenefitExists)
                            {
                                canAdd = false;
                            }
                            break;
                        case ConflictOption.ReplaceIfExists:
                            if (isBenefitExists)
                            {
                                var primaryEmployeeBenefits = filteredPrimaryCard.PrimaryEmployeeBenefits;

                                for (int i = 0; i < primaryEmployeeBenefits.Count; i++)
                                {
                                    if (primaryEmployeeBenefits[i].BenefitCard.Id == benefitCard.Id)
                                    {
                                        primaryEmployeeBenefits.RemoveAt(i);
                                        i--;
                                    }
                                }
                            }
                            break;
                    }
                    if (canAdd)
                    {
                        filteredPrimaryCard.AddPrimaryEmployeeBenefit(new PrimaryEmployeeBenefit
                        {
                            BenefitCard = benefitCard,
                            Value = value,
                            Formula = formula,
                            ExtraValue = extraValue,
                            ExtraValueFormula = extraValueFormula,
                            CeilValue = ceilValue,
                            CeilFormula = ceilFormula,
                            HasExpiryDate = benfitCardIncluded.HasExpiryDate,
                            ExpiryDate = benfitCardIncluded.ExpiryDate,
                            HasStartDate = benfitCardIncluded.HasStartDate,
                            StartDate = benfitCardIncluded.StartDate,
                        });
                       // filteredPrimaryCard.Save();
                    }
                }
            }
            var info = PayrollSystemLocalizationHelper.GetResource(PayrollSystemLocalizationHelper.ApplyAddBenefitsToEmployees);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { }, UserExtensions.CurrentUser, benefitCard, OperationType.Update, info, start);
            message = Helpers.GlobalResource.DoneMessage;

            return Json(new
            {
                Success = true,
                Msg = message,
            });
        }

        [HttpPost]
        public ActionResult ApplyAddDeductionToEmployees(
            string filterBy,
            int deductionCardId,
            double value,
            double extraValue,
            Formula formula,
            ExtraValueFormula extraValueFormula,
            bool isForMonthlyCards,
            bool isForEmployeeHasTheSameDeduction,
            int monthId,
            ConflictOption conflictOption,
            GridFilter filter = null)
        {
            string message;
            DateTime start = DateTime.Now;
            var deductionCard = (DeductionCard)typeof(DeductionCard).GetById(deductionCardId);
            if (isForMonthlyCards == false && isForEmployeeHasTheSameDeduction == false)
            {

                message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.ChooseIsForMonthlyCardsOrIsForEmployeeHasTheSameDeduction));
                return Json(new
                {
                    Success = false,
                    Msg = message,
                });


            }

            Type entityType = null;
            IQueryable<IEntity> allData = null;

            if (filterBy == "FilterByEmployee")
            {
                entityType = typeof(Employee);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<Employee>();
            }
            else if (filterBy == "FilterByPrimaryCard")
            {
                entityType = typeof(EmployeeCard);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<EmployeeCard>();
            }
            else if (filterBy == "FilterByGrade")
            {
                entityType = typeof(HRIS.Domain.Grades.RootEntities.Grade);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.Grades.RootEntities.Grade>();
            }
            else if (filterBy == "FilterByJobTitle")
            {
                entityType = typeof(JobTitle);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<JobTitle>();
            }
            else if (filterBy == "FilterByJobDescription")
            {
                entityType = typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.JobDescription.RootEntities.JobDescription>();
            }
            else if (filterBy == "FilterByPosition")
            {
                entityType = typeof(Position);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<Position>();
            }
            else if (filterBy == "FilterByNode")
            {
                entityType = typeof(Node);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<Node>();
            }
            else if (filterBy == "FilterByMajorType")
            {
                entityType = typeof(MajorType);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<MajorType>();
            }
            else if (filterBy == "FilterByMajor")
            {
                entityType = typeof(Major);
                allData = ServiceFactory.ORMService.AllWithVertualDeleted<Major>();
            }

            var queryablePrimaryCards = FilterController.GetRelatedPrimaryCards(entityType, allData, filter);

            if (isForEmployeeHasTheSameDeduction)
            {
                queryablePrimaryCards = FilterController.GetRelatedPrimaryCardsWithdeDuction(queryablePrimaryCards, deductionCard);
            }

            if (isForMonthlyCards)
            {
                var month = (Month)typeof(Month).GetById(monthId);
                if (month.MonthStatus == MonthStatus.Approved || month.MonthStatus == MonthStatus.Locked)
                {
                    message = ServiceFactory.LocalizationService
                        .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.CannotDoActionsOnLockedOrApprovedMonths));
                    return Json(new
                    {
                        Success = false,
                        Msg = message,
                    });
                }

                var filteredMonthlyCards = month.MonthlyCards.Where(x => queryablePrimaryCards.Any(y => y.Id == x.PrimaryCard.Id));

                foreach (var filteredmonthlyCard in filteredMonthlyCards)
                {
                    var isDeductionExists = filteredmonthlyCard.MonthlyEmployeeDeductions.Any(x => x.DeductionCard.Id == deductionCard.Id);
                    var canAdd = true;
                    switch (conflictOption)
                    {
                        case ConflictOption.KeepExists:
                            if (isDeductionExists)
                            {
                                canAdd = false;
                            }
                            break;
                        case ConflictOption.ReplaceIfExists:
                            if (isDeductionExists)
                            {
                                var monthlyEmployeeDeductions = filteredmonthlyCard.MonthlyEmployeeDeductions;

                                for (int i = 0; i < monthlyEmployeeDeductions.Count; i++)
                                {
                                    if (monthlyEmployeeDeductions[i].DeductionCard.Id == deductionCard.Id)
                                    {
                                        monthlyEmployeeDeductions.RemoveAt(i);
                                        i--;
                                    }
                                }
                            }
                            break;
                    }

                    if (canAdd)
                    {
                        filteredmonthlyCard.AddMonthlyEmployeeDeduction(new MonthlyEmployeeDeduction
                        {
                            DeductionCard = deductionCard,
                            Value = value,
                            Formula = formula,
                            ExtraValue = extraValue,
                            ExtraValueFormula = extraValueFormula
                        });
                        filteredmonthlyCard.Save();
                    }
                }
            }
            else
            {
                var filteredPrimaryCards = queryablePrimaryCards;

                foreach (var filteredPrimaryCard in filteredPrimaryCards)
                {
                    var isDeductionExists = filteredPrimaryCard.PrimaryEmployeeDeductions.Any(x => x.DeductionCard.Id == deductionCard.Id);
                    var canAdd = true;
                    switch (conflictOption)
                    {
                        case ConflictOption.KeepExists:
                            if (isDeductionExists)
                            {
                                canAdd = false;
                            }
                            break;
                        case ConflictOption.ReplaceIfExists:
                            if (isDeductionExists)
                            {
                                var primaryEmployeeDeductions = filteredPrimaryCard.PrimaryEmployeeDeductions;

                                for (int i = 0; i < primaryEmployeeDeductions.Count; i++)
                                {
                                    if (primaryEmployeeDeductions[i].DeductionCard.Id == deductionCard.Id)
                                    {
                                        primaryEmployeeDeductions.RemoveAt(i);
                                        i--;
                                    }
                                }
                            }
                            break;
                    }
                    if (canAdd)
                    {
                        filteredPrimaryCard.AddPrimaryEmployeeDeduction(new PrimaryEmployeeDeduction
                        {
                            DeductionCard = deductionCard,
                            Value = value,
                            Formula = formula,
                            ExtraValue = extraValue,
                            ExtraValueFormula = extraValueFormula,
                            HasExpiryDate = false,
                            ExpiryDate = DateTime.Now,
                            HasStartDate = false,
                            StartDate = DateTime.Now
                        });
                        //filteredPrimaryCard.Save();
                    }
                }
            }
            var info = PayrollSystemLocalizationHelper.GetResource(PayrollSystemLocalizationHelper.ApplyAddDeductionToEmployees);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { }, UserExtensions.CurrentUser, deductionCard, OperationType.Update, info, start);
            message = Helpers.GlobalResource.DoneMessage;

            return Json(new
            {
                Success = true,
                Msg = message,
            });
        }

    }
}
