using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels
{
    public class DetailListErorrs
    {
        public DetailListErorrs()
        {
            DetailObjectsErrors = new List<DetailObjectErrors>();
        }
        public string DetailName { get; set; }
        public List<DetailObjectErrors> DetailObjectsErrors { get; set; }
    }
}