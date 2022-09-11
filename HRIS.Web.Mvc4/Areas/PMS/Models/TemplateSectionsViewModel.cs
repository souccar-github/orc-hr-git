using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.PMS.Models
{
    public class TemplateSectionsViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TemplateSectionsViewModel).FullName;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIncluded { get; set; }
        public float Weight { get; set; }
    }
}