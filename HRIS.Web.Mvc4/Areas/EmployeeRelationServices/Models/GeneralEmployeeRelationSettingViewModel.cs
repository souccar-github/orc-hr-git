using System;
using System.Collections.Generic;
using System.Linq;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Project.Web.Mvc4.Helpers.Resource;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Validation.MessageKeys;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class GeneralEmployeeRelationSettingViewModel : ViewModel
    {

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(GeneralEmployeeRelationSettingViewModel).FullName;
            //model.Views[0].EditHandler = "ChangeableHolidayEditHandler";
        }

        public override ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
        {
            var recordCount = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().Count();
            if (recordCount >= 1)
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = false, message = CustomMessageKeysPayrollSystemModule.MoreThanOneRowNotAllowed });
            else
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = "" });

        }

    }
}