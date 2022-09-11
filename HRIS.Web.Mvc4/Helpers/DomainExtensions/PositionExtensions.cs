using HRIS.Domain.JobDescription.Entities;
using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Helpers.DomainExtensions { 
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
public static class PositionExtensions
    {
        public static User User(this Position position)
        {
            var emp = position.Employee;
            return emp == null ? null : emp.User();
        }
    }
}