using System.IO;

using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using Project.Web.Mvc4.Areas.Reporting.Helpers;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using Project.Web.Mvc4.ProjectModels;
using Souccar.CodeGenerator.Resourecs;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Localization;
using Souccar.Domain.Reporting;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;

namespace Project.Web.Mvc4.Areas.Reporting.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class ReportDefinitionViewModel : ViewModel
    {

        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ReportDefinitionViewModel).FullName;
            model.Views[0].EditHandler = "ReportDefinitionEditHandler";
            model.Views[0].ViewHandler = "ReportDefinitionViewHandler";
            model.ActionListHandler = "RemoveEditFromSystemReport";
            model.Views[0].AfterRequestEnd = "ReportDefinitionAfterRequestEnd";

            if (type == typeof(ReportDefinition))
                model.ToolbarCommands.Add(
                     new ToolbarCommand
                     {
                         Additional = false,
                         ClassName = "grid-action-button GenerateBasicReports",
                         Handler = "",
                         ImageClass = "",
                         Name = "GenerateBasicReportsButton",
                         Text = GlobalResource.GenerateBasicReports
                     });
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null, IList<DetailData> Details = null)
        {
            var reportDefinition = (ReportDefinition)entity;

            reportDefinition.FileName = GetValidReportNameCreatedByUser(reportDefinition.FileName).Replace(".rdl", "");

            #region Check if title or report name is exist
            var repoDefTitle = ServiceFactory.ORMService.All<ReportDefinition>().FirstOrDefault(x => x.Title == reportDefinition.Title);
            var repoDefFileName = ServiceFactory.ORMService.All<ReportDefinition>().FirstOrDefault(x => x.FileName == reportDefinition.FileName);
            if (repoDefTitle != null && repoDefTitle.Title == reportDefinition.Title && repoDefTitle.Id != reportDefinition.Id)
            {
                var titlePropInfo = typeof(ReportDefinition).GetProperty("Title");
                validationResults.Add(new ValidationResult()
                {
                    Message = String.Format(ReportLocalizationHelper.GetResource(ReportLocalizationHelper.TitleAlreadyExists)),
                    Property = titlePropInfo
                });
            }
            if (repoDefFileName != null && repoDefFileName.FileName == reportDefinition.FileName && repoDefFileName.Id != reportDefinition.Id)
            {
                var fileNamePropInfo = typeof(ReportDefinition).GetProperty("FileName");
                validationResults.Add(new ValidationResult()
                {
                    Message = String.Format(ReportLocalizationHelper.GetResource(ReportLocalizationHelper.FileNameAlreadyExists)),
                    Property = fileNamePropInfo
                });
            }
            #endregion

            //Created by
            reportDefinition.CreatedBy = UserExtensions.CurrentUser;
        }


        public override void AfterUpdate(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var reportDefinition = entity as ReportDefinition;

            BuildNavigation.GetModule((entity as ReportDefinition).ModuleName.ToString()).UpdateReport();

            #region add new file to reports folder and remove old

            if (!string.IsNullOrEmpty(ReportingHelper.ReportName))
            {
                RemoveOldFile((string)originalState["FileName"]);
                ReportingHelper.DeleteItemFromSSRSServer((string)originalState["FileName"]);
                MoveToSsrsFolder();
                //Deploy
                ReportingHelper.SynchronizeReport(GetValidReportNameCreatedByUser(reportDefinition.FileName));

                ReportingHelper.Dispose();
            }
            #endregion

            #region update localization
            var languages = ServiceFactory.ORMService.All<Language>();

            var generator = new NHibernateResourceGenerator();
            var group = generator.GetResourceGroupByGroupName("Reports");
            if (reportDefinition.Title != (string)originalState["Title"] && reportDefinition.FileName == (string)originalState["FileName"])
            {
                foreach (var language in languages)
                {
                    var loc = language.LocaleStringResources.SingleOrDefault(x => x.ResourceName == reportDefinition.FileName);
                    if (loc != null)
                    {
                        loc.ResourceGroup = group;
                        loc.Language = language;
                        loc.ResourceName = reportDefinition.FileName;
                        loc.ResourceValue = reportDefinition.Title.ToCapitalLetters();
                        loc.ResourceStatus = ResourceStatus.Defualt;
                    }
                    language.Save(null);
                }
            }
            else if (reportDefinition.FileName != (string)originalState["FileName"])
            {
                foreach (var language in languages)
                {
                    generator.AddLocaleForResourceByLanguage(language, group, reportDefinition.FileName, reportDefinition.Title);
                    language.Save(null);
                }
            }

            #endregion

        }

        public override void AfterDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var reportDefinition = entity as ReportDefinition;

            string assemblyFile = (new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;

            var binFolderPath = Path.GetDirectoryName(assemblyFile);
            var parentPath = Path.GetDirectoryName(binFolderPath);

            var path = parentPath + "\\Content\\UploadedFiles\\Souccar.Domain.Report.ReportDefinition\\SSRS\\";
            //Solve space problem of url
            path = System.Web.HttpUtility.UrlDecode(path);
            var pathWithFile = path + reportDefinition.FileName + ".rdl";
            if (File.Exists(pathWithFile))
                File.Delete(pathWithFile);

            ReportingHelper.DeleteItemFromSSRSServer(reportDefinition.FileName);
        }

        public override void AfterInsert(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, string customInformation = null)
        {
            var reportDefinition = entity as ReportDefinition;

            BuildNavigation.GetModule(reportDefinition.ModuleName.ToString()).UpdateReport();

            #region Add file to reports folder
            MoveToSsrsFolder();
            ReportingHelper.Dispose();
            //Deploy
            ReportingHelper.SynchronizeReport(GetValidReportNameCreatedByUser(reportDefinition.FileName));
            #endregion

            #region Add Localization
            var languages = ServiceFactory.ORMService.All<Language>();

            var generator = new NHibernateResourceGenerator();
            var group = generator.GetResourceGroupByGroupName("Reports");
            foreach (var language in languages)
            {
                generator.AddLocaleForResourceByLanguage(language, group, reportDefinition.FileName, reportDefinition.Title);
                language.Save(null);
            }

            #endregion


        }

        #region Other methods

        private string GetValidReportNameCreatedByUser(string name)
        {
            return string.Format("{0}_{1}", "CreatedByUser", Path.GetFileNameWithoutExtension(name.Split('_').Last()));
        }

        private void MoveToSsrsFolder()
        {
            string fullPathWithSourceFile = ReportingHelper.SourcePath + "\\" + ReportingHelper.ReportName;
            if (File.Exists(fullPathWithSourceFile))
            {
                string fullPathWithDestinationFile = ReportingHelper.DestinationPath + "\\" + GetValidReportNameCreatedByUser(ReportingHelper.ReportName);

                if (File.Exists(fullPathWithDestinationFile))
                    File.Delete(fullPathWithDestinationFile + ".rdl");

                File.Move(fullPathWithSourceFile, fullPathWithDestinationFile + ".rdl");
            }
        }

        public void RemoveOldFile(string fileName)
        {
            string pathWithFile = ReportingHelper.DestinationPath + "\\" + fileName + ".rdl";
            if (File.Exists(pathWithFile))
            {
                File.Delete(pathWithFile);
            }
        }
        #endregion
    }
}