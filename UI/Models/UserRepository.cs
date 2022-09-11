#region

using System;
using System.DirectoryServices;

#endregion

namespace UI.Models
{
    public class UserRepository
    {
        private readonly string _server;
        private DirectoryEntry _directoryEntry;
        private DirectorySearcher _directorySearcher;

        public UserRepository(string server)
        {
            _server = server;
        }

        public User Authenticate(string userName, string password)
        {
            User user = null;

            try
            {
                _directoryEntry = new DirectoryEntry("LDAP://" + _server, userName, password,
                                                     AuthenticationTypes.Secure);

                _directorySearcher = new DirectorySearcher(_directoryEntry)
                                         {
                                             SearchScope = SearchScope.Subtree,
                                             Filter = String.Format("(SAMAccountName={0})", userName)
                                         };

                _directorySearcher.PropertiesToLoad.Add("cn");

                SearchResult result = _directorySearcher.FindOne();

                if (result != null)
                {
                    user = GetUserByPath(result.Path);
                }

                return user;
            }
            catch (Exception e)
            {
                //to do,we make for testing purpose(nhibernet profile issue)
                return createGestUser(userName);
            }
        }

        private User GetUserByPath(string userPath)
        {
            _directoryEntry = new DirectoryEntry(userPath);

            var user = new User
                           {
                               UserName = _directoryEntry.Properties["sAMAccountName"].Value.ToString(),
                               FirstName = _directoryEntry.Properties["givenname"].Value.ToString(),
                               LastName = _directoryEntry.Properties["sn"].Value.ToString(),
                               Email = _directoryEntry.Properties["mail"].Value.ToString()
                           };

            foreach (object group in _directoryEntry.Properties["memberOf"])
            {
                user.Groups.Add(GetGroup((string) group));
            }

            _directoryEntry.Close();

            return user;
        }
        private User createGestUser(string userName)
        {
            

            var user = new User
            {
                UserName = userName,
                FirstName = userName,
                LastName = userName,
                Email = userName+".souccar.com"
            };

           

            return user;
        }

        private static string GetGroup(string path)
        {
            string value = string.Empty;

            int index1 = path.IndexOf("=", 1);
            int index2 = path.IndexOf(",", 1);

            if (index1 != -1)
            {
                value = path.Substring((index1 + 1), (index2 - index1) - 1);
            }

            return value;
        }
    }
}