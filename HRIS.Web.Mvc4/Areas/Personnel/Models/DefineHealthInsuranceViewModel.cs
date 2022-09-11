using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.Personnel.Models
{
    public class DefineHealthInsuranceViewModel : ViewModel
    {
        //
        // GET: /Personnel/DefineHealthInsuranceViewModel/
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(DefineHealthInsuranceViewModel).FullName;
            model.Views[0].EditHandler = "DefaultValueEditHandler";
        }
    }
}
