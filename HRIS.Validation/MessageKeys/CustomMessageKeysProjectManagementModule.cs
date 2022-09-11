namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysProjectManagementModule
    {
        public const string ResourceGroupName = "CustomMessageKeysProjectManagementModule";

        public const string ProjectManagementQuarterIsRequired = "ProjectManagementQuarterIsRequired";

        public const string ProjectManagementToDateMustBeGreaterThanEqualToFromDate = "ProjectManagementToDateMustBeGreaterThanEqualToFromDate";

        public const string SomeProjectsDoNotHaveNames = "SomeProjectsDoNotHaveNames";
        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
