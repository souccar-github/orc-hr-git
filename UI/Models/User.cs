#region

using System.Collections.Generic;
using System.Text;

#endregion

namespace UI.Models
{
    public class User
    {
        public User()
        {
            UserName = "";
            FirstName = "";
            LastName = "";
            Email = "";

            Groups = new List<string>();
        }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<string> Groups { get; set; }

        public string GetFullName()
        {
            var stringBuilder = new StringBuilder();


            if ((!(string.IsNullOrEmpty(FirstName))))
            {
                stringBuilder.Append(FirstName);
                stringBuilder.Append(" ");
            }

            stringBuilder.Append(LastName);

            return stringBuilder.ToString();
        }

        public string SerializeGroups()
        {
            string text = "";

            foreach (string itemLoopVariable in Groups)
            {
                string item = itemLoopVariable;

                if ((string.IsNullOrEmpty(text)))
                {
                    text = item;
                }
                else
                {
                    text = text + "|" + item;
                }
            }

            return text;
        }

        public string SerializeArray(string[] arrOfStr)
        {
            string text = string.Empty;

            foreach (string itemLoopVariable in arrOfStr)
            {
                string item = itemLoopVariable;

                if ((string.IsNullOrEmpty(text)))
                {
                    text = item;
                }
                else
                {
                    text = text + "|" + item;
                }
            }

            return text;
        }
    }
}