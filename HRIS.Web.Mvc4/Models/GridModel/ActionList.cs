using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.GridModel
{
    [Serializable]
    public class ActionList
    {
        public ActionList()
        {
            this.Commands = new List<ActionListCommand>();
        }

        public int GroupsCount { get; set; }
        public IList<ActionListCommand> Commands { get; private set; }

        public void OrderCommands()
        {
            this.Commands = Commands.OrderBy(c => c.GroupId).ThenBy(c => c.Order).ToList();
        }
    }
}