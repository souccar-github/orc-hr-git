using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Appraisals
{
    public class KpiViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }
        public float Value { get; set; }
    }
}