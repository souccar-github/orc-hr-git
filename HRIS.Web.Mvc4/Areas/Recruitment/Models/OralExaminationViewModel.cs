using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.RootEntities;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class OralExaminationViewModel : ViewModel
    {
        
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(OralExaminationViewModel).FullName;
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            this.PreventDefault = true;

            var oralExamination = (OralExamination)entity;
            var advertisement = ServiceFactory.ORMService.GetById<Advertisement>(requestInformation.NavigationInfo.Previous[0].RowId);
            advertisement.Status = AdvertisementStatus.Underway;
            advertisement.AddOralExamination(oralExamination);
            ServiceFactory.ORMService.Save(advertisement, UserExtensions.CurrentUser);
        }

    }
}