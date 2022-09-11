using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels
{
    public class DetailObj
    {
        public IList<PropObj> Properties { get; set; }
        public int Id
        {
            get
            {
                if (Properties == null || Properties.All(x => x.PropName != "Id"))

                    return 0;
                return Convert.ToInt32(Properties.Single(x => x.PropName == "Id").Value);

            }
        }
    }
}