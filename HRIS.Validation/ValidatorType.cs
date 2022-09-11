using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation
{
    public enum ValidatorType
    {
        Required,
        EqualTo,
        Between,
        GreaterThan,
        GreaterThanEqualTo,
        LessThan,
        LessThanEqualTo,
        IsInFuture,
        IsInPast,
        IsNumeric,
        IsTrue,
        IsFalse,
        IsAlpha, 
        Contains,
        IsEmpty,
        LengthBetween,
        LengthEqualTo,
        Matches,
        MaxLength,
        MinLength
    }

}
