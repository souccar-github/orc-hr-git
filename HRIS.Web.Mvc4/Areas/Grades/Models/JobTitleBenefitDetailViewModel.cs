using System;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.Grades.Models
{
    public class JobTitleBenefitDetailViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(JobTitleBenefitDetailViewModel).FullName;
            model.Views[0].EditHandler = "JobTitleBenefitDetail_EditHandler";
        }
    }
}