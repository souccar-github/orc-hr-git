using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    public class RoleView
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescreption { get; set; }
        public bool IsSelected { get; set; }
    }
}