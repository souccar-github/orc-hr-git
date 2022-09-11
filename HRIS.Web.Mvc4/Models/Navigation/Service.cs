using System.Collections.Generic;
using System.Web;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Services.Sys;
using Souccar.Infrastructure.Core;
using System.Linq;

namespace Project.Web.Mvc4.Models.Navigation
{
    /// <summary>
    /// Update:Yaseen Alrefaee
    /// Add order security
    /// </summary>
    public class Service : NavigationItem,IAuthorizable
    {
        public virtual string ServiceId { get; set; }
    }
}