using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Souccar.Core.Utilities;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Core;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Audit;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Services
{
    public static class SalaryIncreaseOrdinanceService
    {
        internal static int GenerateEmployees(IQueryable primaryCards, SalaryIncreaseOrdinance salaryIncreaseOrdinance)
        {
            var allPrimaryCards = (IEnumerable<EmployeeCard>)primaryCards;

            var primaryCardsNotGenerated = allPrimaryCards.Where(x => salaryIncreaseOrdinance.SalaryIncreaseOrdinanceEmployees.All(y => y.PrimaryCard.Id != x.Id));

            var cardsNotGenerated = primaryCardsNotGenerated as IList<EmployeeCard> ?? primaryCardsNotGenerated.ToList();

            foreach (var item in cardsNotGenerated)
            {
                salaryIncreaseOrdinance.AddSalaryIncreaseOrdinanceEmployee(
                    new SalaryIncreaseOrdinanceEmployee
                    {
                        PrimaryCard = new EmployeeCard { Id = item.Id },
                        SalaryBeforeIncrease = item.FinancialCard.Salary,
                        SalaryAfterIncrease = 0
                    });
            }

            salaryIncreaseOrdinance.Save();
            return cardsNotGenerated.Count();
        }

        internal static void CalculateIncreasedValues(SalaryIncreaseOrdinance salaryIncreaseOrdinance, DateTime start)
        {
            foreach (var item in salaryIncreaseOrdinance.SalaryIncreaseOrdinanceEmployees)
            {
                // يتم تطبيق النسبة ثم اضافة القيمة والنتيجة النهائية يتم تقريبها
                var tempValue = (item.SalaryBeforeIncrease * salaryIncreaseOrdinance.IncreasePercentage) / 100;
                tempValue += salaryIncreaseOrdinance.IncreaseValue;
                tempValue += item.SalaryBeforeIncrease;
                tempValue = RoundUtility.PreDefinedRoundValue(salaryIncreaseOrdinance.Round, tempValue);

                if (salaryIncreaseOrdinance.ConsiderCategorySalaryCeil)
                {
                    var categoryMaxCeil = GeneralService.GetEmployeeCategoryMaxCeil(item.PrimaryCard.Employee);
                    tempValue = categoryMaxCeil > tempValue ? tempValue : categoryMaxCeil;
                }

                item.SalaryAfterIncrease = (float)tempValue;
            }

            var commandName = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.CalculateSalaryIncreaseOrdinance);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { salaryIncreaseOrdinance }, UserExtensions.CurrentUser, salaryIncreaseOrdinance, OperationType.Update, commandName, start);

        }

        internal static void AcceptIncreasedValues(SalaryIncreaseOrdinance salaryIncreaseOrdinance, DateTime start)
        {
            foreach (var item in salaryIncreaseOrdinance.SalaryIncreaseOrdinanceEmployees)
            {
                item.PrimaryCard.FinancialCard.Salary = item.SalaryAfterIncrease;
                //IncidenceDefinitionService.InsertIncidenceDefinition(
                //   item.PrimaryCard.Employee, salaryIncreaseOrdinance.DocumentType, salaryIncreaseOrdinance.DocumentNumber,
                //   salaryIncreaseOrdinance.Date,
                //   null, null, DateTime.Now,
                //   (float)salaryIncreaseOrdinance.IncreaseValue,
                //   (float)salaryIncreaseOrdinance.IncreasePercentage,
                //   String.Empty);
            }

            salaryIncreaseOrdinance.Status = Status.Accepted;
            var commandName = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.AcceptSalaryIncreaseOrdinance);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() {  salaryIncreaseOrdinance }, UserExtensions.CurrentUser, salaryIncreaseOrdinance, OperationType.Update, commandName, start);

        }
    }
}