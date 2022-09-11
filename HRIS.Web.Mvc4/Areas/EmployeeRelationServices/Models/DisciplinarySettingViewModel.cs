#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 04/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion
namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class DisciplinarySettingViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(DisciplinarySettingViewModel).FullName;
            model.Views[0].EditHandler = "DisciplinarySettingEditHandler";
            model.Views[0].ViewHandler = "DisciplinarySettingViewHandler";
        }
    }
}