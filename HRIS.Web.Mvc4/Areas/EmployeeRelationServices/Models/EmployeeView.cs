using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EmployeeView
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public bool LastName { get; set; }
    }
}