using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Souccar.Domain.Validation
{
    /// <Author>
    /// Muhammad Alsaadi
    /// </Author>

    /// <summary>
    /// Spec Express framework has its own pre-defined message keys
    /// in this enum we have all these keys to do some manuplation on key in "DefaultValidationMessagesStor.cs"
    /// Important: don't change any of these keys 
    /// </summary>
    public enum PreDefinedValidatorKeys
    {
        Alpha,
        Between,
        Contains,
        CountEqualTo,
        CountGreaterThan,
        CountGreaterThanEqualTo,
        CountLessThan,
        CountLessThanEqualTo,
        EqualTo,
        GreaterThan,
        GreaterThanEqualTo,
        IsEmpty,
        IsFalse,
        IsInFuture,
        IsInPast,
        IsInSet,
        IsTrue,
        ItemsAreUnique,
        LengthBetween,
        LengthEqualTo,
        LessThan,
        LessThanEqualTo,
        Matches,
        MaxLength,
        MinLength,
        Not_Alpha,
        Not_Between,
        Not_Contains,
        Not_CountEqualTo,
        Not_CountGreaterThan,
        Not_CountGreaterThanEqualTo,
        Not_CountLessThan,
        Not_CountLessThanEqualTo,
        Not_EqualTo,
        Not_GreaterThan,
        Not_GreaterThanEqualTo,
        Not_IsEmpty,
        Not_IsFalse,
        Not_IsInFuture,
        Not_IsInPast,
        Not_IsInSet,
        Not_IsTrue,
        Not_ItemsAreUnique,
        Not_LengthBetween,
        Not_LengthEqualTo,
        Not_LessThan,
        Not_LessThanEqualTo,
        Not_Matches,
        Not_MaxLength,
        Not_MinLength,
        Not_Numeric,
        Numeric,
        Required,
        SpecificationRule
    }
}
