#region

using System.Configuration;

#endregion

namespace UI.Helpers.Security
{
    public class SecurityKeyStatus
    {
        public static bool IsEnabled()
        {
            string implementSecurity = ConfigurationManager.AppSettings["ImplementSecurity"];

            if (!string.IsNullOrEmpty(implementSecurity))
            {
                if (implementSecurity == "True")
                {
                    return true;
                }
            }

            return false;
        }
    }
}