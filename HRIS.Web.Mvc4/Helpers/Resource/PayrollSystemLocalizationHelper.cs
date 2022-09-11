using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
namespace Project.Web.Mvc4.Helpers.Resource
{
    public class PayrollSystemLocalizationHelper
    {
        public const string ResourceGroupName = "EmployeeRelationServicesModule";
        public const string ForEmployeeHasTheSameBenefit = "ForEmployeeHasTheSameBenefit";
        public const string ForEmployeeHasTheSameDeduction = "ForEmployeeHasTheSameDeduction";
        public const string YouCannotEditLockedMonth = "YouCannotEditLockedMonth";
        public const string YouCannotDuplicateLoanNumber = "YouCannotDuplicateLoanNumber";
        public const string YourSalaryLessThanMonthlyInstalmentValue = "YourSalaryLessThanMonthlyInstalmentValue";
        public const string YouCanNotAddLoanPaymentBecauseTheStatusOfThisMonthIsApproved = "YouCanNotAddLoanPaymentBecauseTheStatusOfThisMonthIsApproved";
        public const string YouCanNotAddLoanPaymentBecauseTheStatusOfThisMonthIsLocked = "YouCanNotAddLoanPaymentBecauseTheStatusOfThisMonthIsLocked";
        public const string ApplyAddBenefitsToEmployees = "ApplyAddBenefitsToEmployees";
        public const string ApplyAddDeductionToEmployees = "ApplyAddDeductionToEmployees";
        public const string ApplyCopySalaries = "ApplyCopySalaries";

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }
    }
}