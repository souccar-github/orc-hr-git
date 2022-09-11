using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Recruitment.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class ApplicantViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ApplicantViewModel).FullName;

            model.Views[0].ViewHandler = "onViewEmployee";
            model.Views[0].EditHandler = "onEditEmployee";
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            this.PreventDefault = true;

            var applicant = (Applicant)entity;
            ServiceFactory.ORMService.Save(applicant, UserExtensions.CurrentUser);
        }

        

    }
}