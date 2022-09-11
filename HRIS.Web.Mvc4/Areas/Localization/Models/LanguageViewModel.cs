using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.Localization;
using System;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Extensions;

namespace Project.Web.Mvc4.Areas.Localization.Models
{
    public class LanguageViewModel : ViewModel
    {

        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(LanguageViewModel).FullName;
            model.ActionList.Commands.Add(new ActionListCommand()
            {
                GroupId = 1,
                Order = 1,
                HandlerName = "SetAsDefaultLanguage",
                Name = GlobalResource.SetAsDefaultLanguage,
                ShowCommand = true
            });

            model.ActionList.Commands.Add(new ActionListCommand()
            {
                GroupId = 1,
                Order = 2,
                HandlerName = "GenerateValues",
                Name = GlobalResource.GenerateValues,
                ShowCommand = true
            });

            model.ActionList.Commands.Add(new ActionListCommand()
            {
                GroupId = 1,
                Order = 3,
                HandlerName = "GenerateReportsValues",
                Name = GlobalResource.GenerateReportsValues,
                ShowCommand = true
            });

            model.ActionList.Commands.Add(new ActionListCommand()
            {
                GroupId = 1,
                Order = 4,
                HandlerName = "UpdateReportsValues",
                Name = GlobalResource.UpdateReportsValues,
                ShowCommand = true
            });

            model.ActionList.Commands.Add(new ActionListCommand()
            {
                GroupId = 1,
                Order = 5,
                HandlerName = "ExportValues",
                Name = GlobalResource.ExportValues,
                ShowCommand = true
            });

            model.ActionList.Commands.Add(new ActionListCommand()
            {
                GroupId = 1,
                Order = 6,
                HandlerName = "ImportValues",
                Name = GlobalResource.ImportValues,
                ShowCommand = true
            });

        }

        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var language = (Language)entity;
            language.IsActive = false;
            language.Save();
        }
    }
}