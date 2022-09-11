using System;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System.Collections.Generic;
using Souccar.Infrastructure.Core;
using System.Linq;
using Souccar.Domain.Localization;
using Souccar.Domain.Audit;
using Project.Web.Mvc4.Factories;
using Souccar.Core.Extensions;

namespace Project.Web.Mvc4.Areas.Audit.Models
{
    public class LogViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(LogViewModel).FullName;
            var editLocalizationName = ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + GridModelLocalizationConst.Edit) ?? GridModelLocalizationConst.Edit.ToCapitalLetters();
            var deleteLocalizationName = ServiceFactory.LocalizationService.GetResource(GridModelLocalizationConst.ResourceGroupName + "_" + GridModelLocalizationConst.Delete) ?? GridModelLocalizationConst.Delete.ToCapitalLetters();
            if (model.ToolbarCommands.Any(x => x.Name == BuiltinCommand.Create.ToString().ToLower()))
                model.ToolbarCommands.Remove(model.ToolbarCommands.FirstOrDefault(x => x.Name == BuiltinCommand.Create.ToString().ToLower()));
            if (model.ActionList.Commands.Any(x => x.Name == editLocalizationName))
                model.ActionList.Commands.Remove(model.ActionList.Commands.FirstOrDefault(x => x.Name == editLocalizationName));
            if (model.ActionList.Commands.Any(x => x.Name == deleteLocalizationName))
                model.ActionList.Commands.Remove(model.ActionList.Commands.FirstOrDefault(x => x.Name == deleteLocalizationName));
        }
        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            var temp = (IQueryable<Log>)result.Data;
            List<Souccar.Domain.Localization.Language> langs = new List<Souccar.Domain.Localization.Language>();
            langs = ServiceFactory.ORMService.All<Souccar.Domain.Localization.Language>().ToList();
            foreach (var item in temp)
            {
                item.ClassName = getLocalizationName(item.ClassName, langs);
            }
            result.Data = temp.AsQueryable();
        }
        private string getLocalizationName(string ResourceName, List<Souccar.Domain.Localization.Language> langs)
        {
            try
            {
                string result = null;
                LocaleStringResource entityObj = null;
                foreach (var lang in langs)
                {
                    entityObj = lang.LocaleStringResources.FirstOrDefault(x => x.ResourceName.Contains(ResourceName));
                    if (entityObj != null)
                        if (entityObj.ResourceValue != null)
                            result = entityObj.ResourceValue;
                    if (lang.IsActive == true && result != null)
                        return result;

                }

                return result != null ? result : ResourceName;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}