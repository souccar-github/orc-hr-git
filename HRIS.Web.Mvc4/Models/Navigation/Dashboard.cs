using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Models.Navigation
{
    /// <summary>
    /// Update:Yaseen Alrefaee
    /// Add Localization security
    /// </summary>
    public class Dashboard : NavigationItem, IAuthorizable
    {
        public virtual string DashboardId { get; set; }
     
    }
}