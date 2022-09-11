namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysRecruitmentModule
    {
        public const string ResourceGroupName = "CustomMessageKeysRecruitmentModule";


        public const string TheValueMustBeLessThanCurrentDate = "TheValueMustBeLessThanCurrentDate";
        public const string StartWorkingDateMustBeLessThanEndWorkingDate = "StartWorkingDateMustBeLessThanEndWorkingDate";
        public const string CertificationDateOfIssuanceMustBeLessThanCertificationExpiry = "CertificationDateOfIssuanceMustBeLessThanCertificationExpiry";
        public const string TheValueMustBeLessThanOrEqualCurrentDate = "TheValueMustBeLessThanOrEqualCurrentDate";
        public const string TheValueMustBeGreaterThanOrEqualCurrentDate = "TheValueMustBeGreaterThanOrEqualCurrentDate";
        public const string DateOfBirthMustBeBetween10And100 = "DateOfBirthMustBeBetween10And100";
        public const string InterViewEndTimeMustBeGreaterInterViewStartingTime = "InterViewEndTimeMustBeGreaterInterViewStartingTime";
        public const string EmailNotValid = "EmailNotValid";

        public static string GetFullKey(string key)
        {

            return ResourceGroupName + "_" + key;
        }
    }
}