
namespace HRIS.Validation.MessageKeys
{
    public static class PreDefinedMessageKeysSpecExpress
    {
        public const string ResourceGroupName = "PreDefinedMessageKeysSpecExpress";
        public const string SpecificationRule = "SpecificationRule";
        public const string Required = "Required";
        public const string Numeric = "Numeric";
        public const string NotNumeric = "Not_Numeric";
        public const string NotMinLength = "Not_MinLength";
        public const string NotMaxLength = "Not_MaxLength";
        public const string NotMatches = "Not_Matches";
        public const string NotLessThanEqualTo = "NotLessThanEqualTo";
        public const string NotLessThan = "Not_LessThan";
        public const string NotLengthEqualTo = "Not_LengthEqualTo";
        public const string NotLengthBetween = "Not_LengthBetween";
        public const string NotItemsAreUnique = "Not_ItemsAreUnique";
        public const string NotIsTrue = "Not_IsTrue";
        public const string NotIsInSet = "Not_IsInSet";
        public const string NotIsInPast = "Not_IsInPast";
        public const string NotIsInFuture = "Not_IsInFuture";
        public const string NotIsFalse = "Not_IsFalse";
        public const string NotIsEmpty = "Not_IsEmpty";
        public const string NotGreaterThanEqualTo = "Not_GreaterThanEqualTo";
        public const string NotGreaterThan = "Not_GreaterThan";
        public const string NotEqualTo = "Not_EqualTo";
        public const string NotCountLessThanEqualTo = "Not_CountLessThanEqualTo";
        public const string NotCountLessThan = "Not_CountLessThan";
        public const string NotCountGreaterThanEqualTo = "Not_CountGreaterThanEqualTo";
        public const string NotCountGreaterThan = "Not_CountGreaterThan";
        public const string NotCountEqualTo = "Not_CountEqualTo";
        public const string NotContains = "Not_Contains";
        public const string NotBetween = "Not_Between";
        public const string NotAlpha = "Not_Alpha";
        public const string MinLength = "MinLength";
        public const string MaxLength = "MaxLength";
        public const string Matches = "Matches";
        public const string LessThanEqualTo = "LessThanEqualTo";
        public const string LessThan = "LessThan";
        public const string LengthEqualTo = "LengthEqualTo";
        public const string LengthBetween = "LengthBetween";
        public const string ItemsAreUnique = "ItemsAreUnique";
        public const string IsTrue = "IsTrue";
        public const string IsInSet = "IsInSet";
        public const string IsInPast = "IsInPast";
        public const string IsInFuture = "IsInFuture";
        public const string IsFalse = "IsFalse";
        public const string IsEmpty = "IsEmpty";
        public const string GreaterThanEqualTo = "GreaterThanEqualTo";
        public const string GreaterThan = "GreaterThan";
        public const string EqualTo = "EqualTo";
        public const string CountLessThanEqualTo = "CountLessThanEqualTo";
        public const string CountLessThan = "CountLessThan";
        public const string CountGreaterThanEqualTo = "CountGreaterThanEqualTo";
        public const string CountGreaterThan = "CountGreaterThan";
        public const string CountEqualTo = "CountEqualTo";
        public const string Contains = "Contains";
        public const string Between = "Between";
        public const string Alpha = "Alpha";

        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
