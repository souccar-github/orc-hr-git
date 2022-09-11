using System.Web;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;
using System.Linq;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using System;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Models.Navigation
{
    /// <summary>
    /// Update:Yaseen Alrefaee
    /// Add order security
    /// </summary>
    public class Aggregate : NavigationItem, IAuthorizable
    {
        public virtual string AggregateId { get; set; }
        public virtual IList<Detail> Details { get; set; }
        public virtual IList<ActionListCommand> ActionListCommands { get; set; }

        public Aggregate()
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