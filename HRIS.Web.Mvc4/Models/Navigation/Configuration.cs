using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Models.Navigation
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>

    public class Configuration : NavigationItem, IAuthorizable
    {
        public virtual string ConfigurationId { get; set; }
        public virtual IList<Detail> Details { get; set; }
        public virtual IList<ActionListCommand> ActionListCommands { get; set; }

        public Configuration()
        {
            Details = new List<Detail>();
            ActionListCommands = new List<ActionListCommand>();
        }

        public ActionListCommand GetActionListCommand(string commandId)
        {
            return ActionListCommands.SingleOrDefault(d => d.Name== commandId);
        }

        public Detail GetDetail(string detailId)
        {
            return Details.SingleOrDefault(d => d.DetailId == detailId);
        }

        public void OrderDetailsByGroup()
        {
            Details = Details.OrderBy(d => d.GroupOrder).ToList();
        }

    }
}

