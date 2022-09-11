using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysEmployeeRelationServicesModule
    {
        public const string ResourceGroupName = "CustomMessageKeysEmployeeRelationServicesModule";
        public const string SalaryPercentageIsRequired = "SalaryPercentageIsRequired";
        public const string AmountIsRequired = "SalaryPercentageIsRequired";
        public const string CurrencyTypeIsRequired = "CurrencyTypeIsRequired";

        public const string SorryYouHaveAlreadySubmittedThisResignationRequest = "SorryYouHaveAlreadySubmittedThisResignationRequest";
     
        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
