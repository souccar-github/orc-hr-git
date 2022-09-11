using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RecruitmentInformationViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RecruitmentInformationViewModel).FullName;
            
            //model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 4, HandlerName = "GetPassedPersonsInOralExam",
            //    Name =RecruitmentLocalizationHelper.GetResource( RecruitmentLocalizationHelper.GetPassedPersonsInOralExam), ShowCommand = true});
            //model.ActionList.Commands.Add(new ActionListCommand() { GroupId = 1, Order = 5, HandlerName = "SuccessfulIssuanceUser", 
            //    Name =RecruitmentLocalizationHelper.GetResource( RecruitmentLocalizationHelper.SuccessfulIssuanceUser), ShowCommand = true});
        }

        

    }
}