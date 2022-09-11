
namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysPersonnelModule
    {
        public const string ResourceGroupName = "CustomMessageKeysPersonnelModule";
        public const string AddFemaleMilitaryService = "AddFemaleMilitaryService";
        public const string AddMaritalStatusToUnMarried = "AddMaritalStatusToUnMarried";
        public const string OtherNationalityCanNotEqualNationality = "OtherNationalityCanNotEqualNationality";

        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
