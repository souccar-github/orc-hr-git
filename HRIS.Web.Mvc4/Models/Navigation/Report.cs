using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Models.Navigation
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class Report : NavigationItem, IAuthorizable
    {
        public virtual string ReportId { get; set; }
        public virtual string FileName { get; set; }

    }
}

