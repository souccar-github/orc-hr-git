using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.Navigation
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public interface IAuthorizable
    {
        IList<string> Roles { get; }
        bool IsAuthorized { get; }
    }
}