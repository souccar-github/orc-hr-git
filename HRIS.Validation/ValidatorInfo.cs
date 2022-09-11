using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation
{
    public class ValidatorInfo
    {
        public ValidatorType ValidatorType { get; set; }
        public List<ValidatorRule> ValidatorRules { get; set; }

        public ValidatorInfo()
        {
            ValidatorRules = new List<ValidatorRule>();
        }
    }

    public class ValidatorRule
    {
        public string Message { get; set; }
        public List<object> Parameters { get; set; }
        public bool IsValue { get; set; }

        public ValidatorRule()
        {
            Parameters = new List<object>();
        }
    }
}
