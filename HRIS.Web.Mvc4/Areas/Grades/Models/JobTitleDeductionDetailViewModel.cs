using System;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.Grades.Models
{
    public class JobTitleDeductionDetailViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(JobTitleDeductionDetailViewModel).FullName;
            model.Views[0].EditHandler = "JobTitleDeductionDetail_EditHandler";
        }
    }
}