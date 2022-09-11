using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.JobDescription.Models
{
    public class PositionInfoViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobDescription { get; set; }
        public string Node { get; set; }
        public string Code { get; set; }
    }
}