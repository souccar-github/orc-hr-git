using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Localization;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.Localization.Models
{
    public class LocaleStringResourceViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(LocaleStringResourceViewModel).FullName;
            model.Views[0].AfterRequestEnd = "localeStringResourcesAfterRequestEnd";

        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var localeStringResource = (LocaleStringResource)entity;
            localeStringResource.ResourceStatus = ResourceStatus.EditedFromSystem;
        }
        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var localeStringResource = (LocaleStringResource)entity;
            localeStringResource.ResourceStatus = ResourceStatus.EditedFromSystem;
        }
    }
}