#region

using System.Configuration;

#endregion

namespace UI.Helpers.Session
{
    public class SessionKeyStatus
    {
        public static bool IsEnabled()
        {
            string implementSecurity = ConfigurationManager.AppSettings["CheckSession"];

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