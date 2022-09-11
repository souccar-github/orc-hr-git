using System.Collections.Generic;

namespace Souccar.Domain.Validation
{
    public class ValidationRules
    {
        public string PropertyName { get; set; }
        public List<ValidatorInfo> Validators { get; set; }
    }
}
