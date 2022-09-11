using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RChildViewModel : ViewModel
    {
public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RChildViewModel).FullName;
            model.Views[0].EditHandler = "ChildEditHandler";
            model.Views[0].ViewHandler = "ChildViewHandler";

        }
    }
}