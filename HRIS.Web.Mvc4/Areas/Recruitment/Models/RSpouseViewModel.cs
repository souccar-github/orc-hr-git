using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RSpouseViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RSpouseViewModel).FullName;
            model.Views[0].EditHandler = "SpouseEditHandler";
            model.Views[0].ViewHandler = "SpouseViewHandler";

        }

    }
}