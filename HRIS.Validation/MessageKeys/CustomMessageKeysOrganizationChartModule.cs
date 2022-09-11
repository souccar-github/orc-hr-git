
namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysOrganizationChartModule
    {
        public const string ResourceGroupName = "CustomMessageKeysOrganizationChartModule";

        public static string GetFullKey(string key)
        {

            return ResourceGroupName + "_" + key;
        }
    }
}
