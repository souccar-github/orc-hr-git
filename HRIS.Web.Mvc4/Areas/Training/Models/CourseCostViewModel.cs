using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;

namespace Project.Web.Mvc4.Areas.Training.Models
{
    public class CourseCostViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(CourseCostViewModel).FullName;
        }
    }
}