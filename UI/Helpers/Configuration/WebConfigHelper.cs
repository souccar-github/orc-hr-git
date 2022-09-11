#region

using System.Configuration;
using System.Web.Configuration;

#endregion

namespace UI.Helpers.Configuration
{
    public static class WebConfigHelper
    {
        public static string Read(string key)
        {
            var configuration = WebConfigurationManager.OpenWebConfiguration("~");

            var appSettingsSection = (AppSettingsSection) configuration.GetSection("appSettings");

            if (appSettingsSection != null)
            {
                return appSettingsSection.Settings[key].Value;
            }

            return null;
        }

        public static void Modify(string key, string value)
        {
            System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");

            var appSettingsSection = (AppSettingsSection) configuration.GetSection("appSettings");

            if (appSettingsSection != null)
            {
                appSettingsSection.Settings[key].Value = value;
                configuration.Save();
            }
        }
    }
}