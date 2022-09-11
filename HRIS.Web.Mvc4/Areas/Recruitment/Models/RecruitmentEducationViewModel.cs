using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Recruitment.Entities;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RecruitmentEducationViewModel:ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RecruitmentEducationViewModel).FullName;
        }
    }
}