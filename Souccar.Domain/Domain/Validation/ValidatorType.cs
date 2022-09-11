namespace Souccar.Domain.Validation
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
