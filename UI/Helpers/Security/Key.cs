#region

using System.Configuration;

#endregion

namespace UI.Helpers.Security
{
    public class Key
    {
        public static bool Status()
        {
            string implementSecurity = ConfigurationManager.AppSettings["ImplementSecurity"];

            if (implementSecurity != null && implementSecurity.Trim() == "True")
            {
                return true;
            }

            return false;
        }
    }
}