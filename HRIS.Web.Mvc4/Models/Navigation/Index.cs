using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;


namespace Project.Web.Mvc4.Models.Navigation
{
    public class Index : NavigationItem, IAggregateRoot,IAuthorizable
    {
        public virtual string IndexId { get; set; }
    }
}