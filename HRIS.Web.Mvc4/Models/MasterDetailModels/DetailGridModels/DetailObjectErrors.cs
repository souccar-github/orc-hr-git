using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels
{
    public class DetailObjectErrors
    {
        public DetailObjectErrors()
        {
            EntityErrors = new Dictionary<string, string>();
        }
        public int Id { get; set; }
        public Dictionary<string, string> EntityErrors { get; set; }
    }
}