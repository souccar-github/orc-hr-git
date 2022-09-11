using System;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RecruitmentRequestAttachmentViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RecruitmentRequestAttachmentViewModel).FullName;

            
        }
    }
}