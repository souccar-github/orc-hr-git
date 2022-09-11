using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation
{
    public class ValidationRules
    {
        public string PropertyName { get; set; }
        public List<ValidatorInfo> Validators { get; set; }
    }
}
