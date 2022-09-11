using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Reporting;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers;
using Souccar.CodeGenerator.Resourecs;
using Souccar.Infrastructure.Extenstions;
using System.Xml;
using System.Text.RegularExpressions;
using Souccar.Infrastructure.Services.Sys;
using Souccar.Domain.Localization;
using Souccar.Domain.Security;
using System.Configuration;
using System.Net;
using Microsoft.Reporting.WebForms;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.DomainModel;

namespace Project.Web.Mvc4.Areas.Reporting.Helpers
{
    public class ReportingHelper
    {
        public static IEnumerable<System.Web.HttpPostedFileBase> HttpPostFiles { get; set; }
        public static string SourcePath { get; set; }
        public static string DestinationPath { get; set; }
        public static string ReportName { get; set; }
        public static string DownloadReports()
        {
            try
            {
                var reportService = new SSRS_Service.ReportingService2005();
                var serverUrl = System.Configuration.ConfigurationManager.AppSettings["SSRS_ServerURL"];
                var ReportProject = System.Configuration.ConfigurationManager.AppSettings["SSRS_ReportProject"];
                ReportProject = ReportProject.Remove(ReportProject.Length - 1);
                reportService.Url = serverUrl + "/ReportService2005.asmx?WSDL";
                reportService.Credentials = System.Net.CredentialCache.DefaultCredentials;

                var reports =
                    reportService.ListChildren(ReportProject, false)
                        .Where(x => x.Type == SSRS_Service.ItemTypeEnum.Report)
                        .ToList();
                int failedReportCount = 0;
                foreach (var report in reports)
                {
                    try
                    {
                        var reportDefinition = reportService.GetReportDefinition(report.Path);
                        System.IO.File.WriteAllBytes(
                            HostingEnvironment.MapPath(
                                "~/Content/UploadedFiles/Souccar.Domain.Report.ReportDefinition/SSRS/") + report.Name +
                            ".rdl", reportDefinition);
                    }
                    catch (Exception e)
                    {
                        failedReportCount++;
                    }

                }
                return string.Format("succesfully download {0} from {1} report", (reports.Count - failedReportCount), reports.Count);
            }
            catch (Exception e)
            {
                return string.Format("{0}", LocalizationHelper.FailMessage);
            }

        }
        public static string SynchronizeReports(out bool isSuccess)
        {
            try
            {
                var reportService = new SSRS_Service.ReportingService2005();
                var serverUrl = System.Configuration.ConfigurationManager.AppSettings["SSRS_ServerURL"];
                var reportProject = System.Configuration.ConfigurationManager.AppSettings["SSRS_ReportProject"];
                reportProject = reportProject.Remove(reportProject.Length - 1);
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var userId = connectionString.Split(';').Where(x => x.Contains("User ID")).FirstOrDefault().Split('=')[1];
                var password = connectionString.Split(';').Where(x => x.Contains("Password")).FirstOrDefault().Split('=')[1];
                reportService.Url = serverUrl + "/ReportService2005.asmx?WSDL";
                string user = ConfigurationManager.AppSettings["SSRS_UserName"];
                string pass = ConfigurationManager.AppSettings["SSRS_Password"];
                string domain = ConfigurationManager.AppSettings["SSRS_Domain"];
                if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(domain))
                {
                    reportService.Credentials = new NetworkCredential(user, pass, domain);
                }
                else
                {
                    reportService.Credentials = System.Net.CredentialCache.DefaultCredentials;
                }
                var catalogItems = reportService.ListChildren("/", true);
                var oldReportProjectFolder = catalogItems.FirstOrDefault(x => x.Type == Project.Web.Mvc4.SSRS_Service.ItemTypeEnum.Folder && x.Path == @"" + reportProject);
                string path = oldReportProjectFolder != null ? oldReportProjectFolder.Path : reportProject;
                if (oldReportProjectFolder != null)
                {
                    reportService.DeleteItem(oldReportProjectFolder.Path);
                }
                var pathSections = path.Split('/');
                if (pathSections.Length == 2)
                    reportService.CreateFolder(reportProject.Replace("/", pathSections[0]), "/", null);
                else
                {
                    reportService.CreateFolder(pathSections[pathSections.Length - 1], "/" + pathSections[pathSections.Length - 2], null);
                }
                var c = new Controllers.HomeController();
                c.GenerateBasicReports();

                var reportsFiles = GetReportsFilesFromFolder(HostingEnvironment.MapPath("~/Content/UploadedFiles/Souccar.Domain.Report.ReportDefinition/SSRS"));
                var failedReports = new List<string>();
                foreach (var reportFile in reportsFiles)
                {
                    //if (ServiceFactory.ORMService.All<ReportDefinition>().FirstOrDefault(x => x.FileName == reportFile.Key) != null)
                    //{
                    try
                    {
                        byte[] definition;
                        FileStream stream = File.OpenRead(reportFile.Value);
                        definition = new Byte[stream.Length];
                        stream.Read(definition, 0, (int)stream.Length);

                        reportService.CreateReport(reportFile.Key, reportProject, false, definition, null);

                        var report =
                                reportService.ListChildren(reportProject, false)
                                    .FirstOrDefault(
                                        x => x.Type == SSRS_Service.ItemTypeEnum.Report && x.Name == reportFile.Key);
                        if (report != null)
                        {
                            var dataSource = new SSRS_Service.DataSourceDefinition()
                            {
                                OriginalConnectStringExpressionBased = false,
                                ConnectString = connectionString,
                                CredentialRetrieval = SSRS_Service.CredentialRetrievalEnum.Store,
                                Enabled = true,
                                UserName = userId,
                                Password = password,
                                UseOriginalConnectString = false,
                                WindowsCredentials = false,
                                Extension = "SQL"
                            };
                            var datasources = reportService.GetItemDataSources(report.Path);
                            foreach (var d in datasources)
                            {
                                d.Item = dataSource;
                            }
                            reportService.SetItemDataSources(report.Path, datasources);
                        }

                    }
                    catch (Exception e)
                    {
                        var failedReport = ServiceFactory.ORMService.All<ReportDefinition>().FirstOrDefault(x => x.FileName == reportFile.Key);
                        ServiceFactory.ORMService.Delete(failedReport, UserExtensions.CurrentUser);
                        failedReports.Add(string.Format("{0} :{1}", reportFile.Key, e.Message));
                    }
                    //}
                    //else
                    //{
                    //    failedReports.Add(string.Format("{0} :{1}", reportFile.Key, "Not Generated"));
                    //}
                }


                var sharedDataSource = reportService.ListChildren("/", true).FirstOrDefault(x => x.Type == SSRS_Service.ItemTypeEnum.DataSource);
                if (sharedDataSource != null)
                {
                    var DataSource = reportService.GetDataSourceContents(sharedDataSource.Path);

                    DataSource.OriginalConnectStringExpressionBased = false;
                    DataSource.UseOriginalConnectString = false;
                    DataSource.ConnectString = connectionString;
                    DataSource.CredentialRetrieval = SSRS_Service.CredentialRetrievalEnum.Store;
                    DataSource.UserName = userId;
                    DataSource.Password = password;

                    reportService.SetDataSourceContents(sharedDataSource.Path, DataSource);

                }
                isSuccess = true;
                return string.Format("{0} {1} From {2} \n {3}", "Successfully Deploy Reports",
                    (reportsFiles.Count - failedReports.Count), reportsFiles.Count, string.Join(Environment.NewLine, failedReports));
            }
            catch (Exception e)
            {
                isSuccess = false;
                return string.Format("{0} :{1}", LocalizationHelper.FailMessage, e.Message);
            }




        }
        private static Dictionary<string, string> GetReportsFilesFromFolder(string sDir)
        {
            var result = new Dictionary<string, string>();
            try
            {
                foreach (string file in Directory.GetFiles(sDir).Where(x => x.EndsWith(".rdl")).ToList())
                {
                    result.Add((file.Split('\\')[file.Split('\\').Length - 1]).Replace(".rdl", ""), file);
                }

                //foreach (string subDir in Directory.GetDirectories(sDir))
                //{
                //    foreach (string file in Directory.GetFiles(subDir).Where(x => x.EndsWith(".rdl")).ToList())
                //    {
                //        result.Add((file.Split('\\')[file.Split('\\').Length - 1]).Replace(".rdl", ""), file);
                //    }
                //}
                return result;
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>();
            }
        }
        public static void PassReportParameter(Microsoft.Reporting.WebForms.ReportViewer reportViewer)
        {
            var reportParams = new List<ReportParameter>();
            try
            {
                if (UserExtensions.CurrentUser != null && UserExtensions.CurrentUser.Username != "admin")
                {
                    var userIdPar = new ReportParameter("currentUserEmployeeId");
                    var employee = typeof(Employee).GetAll<Employee>().FirstOrDefault(x => x.Id == Int32.Parse(UserExtensions.CurrentUser.Username));
                    userIdPar.Values.Add(employee != null ? employee.Id.ToString() : "0");
                    reportParams.Add(userIdPar);
                    reportViewer.ServerReport.SetParameters(reportParams);
                }
            }
            catch (ArgumentException e)
            {
                reportParams.Clear();
            }
        }
        public static List<string> GenerateResourceForReports(int langId)
        {
            NHibernateResourceGenerator nrg = new NHibernateResourceGenerator();
            var start = DateTime.Now;
            var Language = nrg.GetLanguageById(langId);
            var group = nrg.GetResourceGroupByGroupName("Reports");
            var failedReports = new List<string>();
            foreach (var report in typeof(ReportDefinition).GetAll<ReportDefinition>())
            {
                if(report.FileName == "PSJobDescriptionReport6")
                {
                    var c = 1;
                }
                //nrg.AddLocaleForResourceByLanguage(Language, group, report.FileName, report.Title);
                var reportGroup = nrg.GetResourceGroupByGroupName("Reports");
                var reportResources = GetReportResources(report.FileName, failedReports);
                if (reportResources != null)
                    foreach (var resource in reportResources)
                        nrg.AddLocaleForResourceByLanguage(Language, reportGroup, reportGroup.Name + "." + resource.Key, resource.Value);
            }

         //   Language.Save(null);
            var entities = new List<IAggregateRoot>();
            entities.Add(Language);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.GenerateReports);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, Language, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() {  });

            return failedReports;

        }

        private static Dictionary<string, string> GetReportResources(string reportFileName, List<string> failedReports)
        {
            try
            {
                var resources = new Dictionary<string, string>();

                var reportFile = GetReportFileFromFolder(HostingEnvironment
                    .MapPath("~/Content/UploadedFiles/Souccar.Domain.Report.ReportDefinition/SSRS"), reportFileName);

                XmlDocument xmlFile = new XmlDocument();
                xmlFile.Load(reportFile.Value);

                XmlNamespaceManager xMngr = new XmlNamespaceManager(xmlFile.NameTable);
                var def = xmlFile.ChildNodes[1].Attributes["xmlns"].Value;
                xMngr.AddNamespace("def", def);

                var nodeList = xmlFile.DocumentElement.SelectNodes("//def:Textbox", xMngr);
                if (nodeList.Count > 0)
                {
                    foreach (XmlNode node in nodeList)
                    {
                        var resourceName = node.Attributes["Name"].Value;
                        if (resourceName.ToLower().StartsWith("label") && !resourceName.Contains("0"))
                        {
                            resourceName = resourceName.Substring(5);
                            var value = node.SelectSingleNode("descendant::def:Value", xMngr);
                            resources[resourceName] = value.InnerText;
                        }

                    }
                }

                nodeList = xmlFile.DocumentElement.SelectNodes("//def:ReportParameter", xMngr);
                if (nodeList.Count > 0)
                {
                    foreach (XmlNode node in nodeList)
                    {
                        var resourceName = node.Attributes["Name"].Value;
                        var isHidden = node.SelectSingleNode("descendant::def:Hidden", xMngr);

                        if (isHidden != null && isHidden.InnerText == "true")
                            continue;

                        var prompt = node.SelectSingleNode("descendant::def:Prompt", xMngr);

                        if (!resourceName.Contains("0"))
                        {
                            resources[resourceName] = prompt.InnerText;

                            var parameterValues = node.SelectNodes("descendant::def:ParameterValue", xMngr);
                            if (parameterValues.Count > 0)
                            {
                                foreach (XmlNode parameterValue in parameterValues)
                                    resources[resourceName + "." + parameterValue.ChildNodes[0].InnerText] = parameterValue.ChildNodes[1].InnerText;
                            }
                        }

                    }
                }

                return resources;
            }
            catch (Exception ex)
            {
                failedReports.Add(reportFileName);
                return null;
            }
        }

        private static string getExistingResourceName(string resourceName, Language lang)
        {
            var resourceNameList = Regex.Split(resourceName, @"0+");
            //var module = resourceNameList[0];
            var module = resourceNameList[0].ToLower();
            var resource = "";
            for (int i = 1; i < resourceNameList.Length; i++)
            {
                //resource += "." + resourceNameList[i];
                resource += "." + resourceNameList[i].ToLower();
            }
            var localeStringResource = lang.LocaleStringResources.FirstOrDefault(x => x.ResourceName.ToLower()
                .Contains(module) && x.ResourceName.ToLower().EndsWith(resource));
            return localeStringResource != null ? localeStringResource.ResourceName : null;
        }

        public static void UpdateReportsRDL(Language lang)
        {
            var reportsFiles = GetReportsFilesFromFolder(
                HostingEnvironment.MapPath("~/Content/UploadedFiles/Souccar.Domain.Report.ReportDefinition/SSRS"));

            var failedReports = new List<string>();
            foreach (var reportFile in reportsFiles)
            {
                try
                {
                    //var reportDefinition = ServiceFactory.ORMService.All<ReportDefinition>().FirstOrDefault(x => x.FileName == reportFile.Key);
                    var resourceGroup = "Reports";

                    XmlDocument xmlFile = new XmlDocument();
                    xmlFile.Load(reportFile.Value);

                    XmlNamespaceManager xMngr = new XmlNamespaceManager(xmlFile.NameTable);
                    var def = xmlFile.ChildNodes[1].Attributes["xmlns"].Value;
                    xMngr.AddNamespace("def", def);
                    var nls = new NHibernateLocalizationService();

                    var nodeList = xmlFile.DocumentElement.SelectNodes("//def:Textbox", xMngr);
                    if (nodeList.Count > 0)
                    {
                        foreach (XmlNode node in nodeList)
                        {
                            var resourceName = node.Attributes["Name"].Value;
                            if (resourceName.ToLower().StartsWith("label"))
                            {
                                resourceName = resourceName.Substring(5);
                                var value = node.SelectSingleNode("descendant::def:Value", xMngr);
                                if (!resourceName.Contains("0"))
                                {
                                    resourceName = resourceGroup + "." + resourceName;
                                    value.InnerText = nls.GetResource(lang, resourceName);
                                }
                                else
                                {
                                    resourceName = getExistingResourceName(resourceName, lang);
                                    if (resourceName != null)
                                        value.InnerText = nls.GetResource(lang, resourceName);
                                }
                            }
                        }
                    }


                    var direction = "";
                    if (lang.Rtl)
                    {
                        direction = "RTL";
                        nodeList = xmlFile.DocumentElement.SelectNodes("//def:LayoutDirection", xMngr);
                        if (nodeList.Count > 0)
                        {
                            foreach (XmlNode node in nodeList)
                                node.InnerText = direction;
                        }
                        else
                        {
                            nodeList = xmlFile.DocumentElement.SelectNodes("//def:Tablix", xMngr);

                            if (nodeList.Count > 0)
                            {
                                foreach (XmlNode node in nodeList)
                                {

                                    var tablixRowHierarchy = node.SelectSingleNode("descendant::def:TablixRowHierarchy", xMngr);
                                    XmlElement layoutDirection = xmlFile.CreateElement("LayoutDirection", def);
                                    if (layoutDirection != null)
                                    {
                                        layoutDirection.InnerText = "RTL";
                                        node.InsertAfter(layoutDirection, tablixRowHierarchy);
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        direction = "LTR";
                        nodeList = xmlFile.DocumentElement.SelectNodes("//def:LayoutDirection", xMngr);
                        if (nodeList.Count > 0)
                        {
                            foreach (XmlNode node in nodeList)
                                node.InnerText = direction;
                        }
                    }


                    nodeList = xmlFile.DocumentElement.SelectNodes("//def:ReportParameter", xMngr);
                    if (nodeList.Count > 0)
                    {
                        foreach (XmlNode node in nodeList)
                        {
                            var resourceName = resourceGroup + "." + node.Attributes["Name"].Value;
                            var isHidden = node.SelectSingleNode("descendant::def:Hidden", xMngr);

                            if (isHidden != null && isHidden.InnerText == "true")
                                continue;

                            var prompt = node.SelectSingleNode("descendant::def:Prompt", xMngr);
                            var parameterValues = node.SelectNodes("descendant::def:ParameterValue", xMngr);

                            if (!node.Attributes["Name"].Value.Contains("0"))
                            {
                                prompt.InnerText = nls.GetResource(lang, resourceName);

                                if (parameterValues.Count > 0)
                                {
                                    foreach (XmlNode parameterValue in parameterValues)
                                        parameterValue.ChildNodes[1].InnerText =
                                            nls.GetResource(lang, resourceName + "." + parameterValue.ChildNodes[0].InnerText);
                                }
                            }
                            else
                            {
                                resourceName = getExistingResourceName(node.Attributes["Name"].Value, lang);
                                if (resourceName != null)
                                    prompt.InnerText = nls.GetResource(lang, resourceName);

                                if (parameterValues.Count > 0)
                                {
                                    var resourceNameSplit = Regex.Split(resourceName, @"\.+");
                                    var assembly = typeof(ModulesNames).Assembly;
                                    var e = assembly.GetTypes().SingleOrDefault(classType => classType.IsEnum && classType.Name.EndsWith(resourceNameSplit[resourceNameSplit.Length - 1]));

                                    if (e == null)
                                    {
                                        assembly = typeof(User).Assembly;
                                        e = assembly.GetTypes().SingleOrDefault(classType => classType.IsEnum && classType.Name.EndsWith(resourceNameSplit[resourceNameSplit.Length - 1]));
                                    }

                                    if (e != null)
                                        foreach (XmlNode parameterValue in parameterValues)
                                        {
                                            var key = e.GetEnumName(Int32.Parse(parameterValue.FirstChild.InnerText));
                                            parameterValue.ChildNodes[1].InnerText = nls.GetResource(lang, e.FullName + "." + key);
                                        }
                                }
                            }



                        }
                    }

                    xmlFile.Save(reportFile.Value);
                  

                }
                catch (Exception ex)
                {
                }
            }

        }

        public static string SynchronizeReport(string fileName)
        {
            try
            {
                var reportService = new SSRS_Service.ReportingService2005();
                var serverUrl = System.Configuration.ConfigurationManager.AppSettings["SSRS_ServerURL"];
                var reportProject = System.Configuration.ConfigurationManager.AppSettings["SSRS_ReportProject"];
                reportProject = reportProject.Remove(reportProject.Length - 1);
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var userId = connectionString.Split(';').Where(x => x.Contains("User ID")).FirstOrDefault().Split('=')[1];
                var password = connectionString.Split(';').Where(x => x.Contains("Password")).FirstOrDefault().Split('=')[1];
                reportService.Url = serverUrl + "/ReportService2005.asmx?WSDL";
                string user = ConfigurationManager.AppSettings["SSRS_UserName"];
                string pass = ConfigurationManager.AppSettings["SSRS_Password"];
                string domain = ConfigurationManager.AppSettings["SSRS_Domain"];
                if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(domain))
                {
                    reportService.Credentials = new NetworkCredential(user, pass, domain);
                }
                else
                {
                    reportService.Credentials = System.Net.CredentialCache.DefaultCredentials;
                }

                var reportFile = GetReportFileFromFolder(HostingEnvironment.MapPath("~/Content/UploadedFiles/Souccar.Domain.Report.ReportDefinition/SSRS"), fileName);

                var bytes = System.IO.File.ReadAllBytes(reportFile.Value);
                reportService.CreateReport(reportFile.Key, reportProject, true, bytes, null);

                var report =
                            reportService.ListChildren(reportProject, false)
                                .FirstOrDefault(
                                    x => x.Type == SSRS_Service.ItemTypeEnum.Report && x.Name == reportFile.Key);

                var dataSource = new SSRS_Service.DataSourceDefinition()
                {
                    OriginalConnectStringExpressionBased = false,
                    ConnectString = connectionString,
                    CredentialRetrieval = SSRS_Service.CredentialRetrievalEnum.Store,
                    Enabled = true,
                    UserName = userId,
                    Password = password,
                    UseOriginalConnectString = false,
                    WindowsCredentials = false,
                    Extension = "SQL"
                };
                var datasources = reportService.GetItemDataSources(report.Path);
                foreach (var d in datasources)
                {
                    d.Item = dataSource;
                }
                reportService.SetItemDataSources(report.Path, datasources);

                return "Success";

            }

            catch (Exception e)
            {
                return string.Format("{0} :{1}", LocalizationHelper.FailMessage, e.Message);
            }
        }
        private static KeyValuePair<string, string> GetReportFileFromFolder(string sDir, string fileName)
        {
            try
            {
                var file = Directory.GetFiles(sDir).FirstOrDefault(x => x.EndsWith(".rdl") && x.Contains(fileName));
                var result = new KeyValuePair<string, string>(
                    (file.Split('\\')[file.Split('\\').Length - 1]).Replace(".rdl", ""),
                    file);
                return result;
            }
            catch (Exception e)
            {
                return new KeyValuePair<string, string>();
            }
        }



        internal static bool CheckReportIfExistInDb(string fileName)
        {
            string[] fileNameSections = fileName.Split('_');
            var filenamesFromDb = ServiceFactory.ORMService.All<ReportDefinition>();
            foreach (var reportDefinition in filenamesFromDb)
            {
                if (reportDefinition.FileName.Split('_').Last() == fileNameSections.Last())
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteItemFromSSRSServer(string fileName)
        {
            try
            {
                var reportService = new SSRS_Service.ReportingService2005();
                var serverUrl = System.Configuration.ConfigurationManager.AppSettings["SSRS_ServerURL"];
                var reportProject = System.Configuration.ConfigurationManager.AppSettings["SSRS_ReportProject"];
                reportProject = reportProject.Remove(reportProject.Length - 1);
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                var userId = connectionString.Split(';').Where(x => x.Contains("User ID")).FirstOrDefault().Split('=')[1];
                var password = connectionString.Split(';').Where(x => x.Contains("Password")).FirstOrDefault().Split('=')[1];
                reportService.Url = serverUrl + "/ReportService2005.asmx?WSDL";
                reportService.Credentials = System.Net.CredentialCache.DefaultCredentials;
                var oldReportProjectFolder = reportService.ListChildren("/", false).FirstOrDefault(x => x.Type == Project.Web.Mvc4.SSRS_Service.ItemTypeEnum.Folder && x.Path == reportProject);
                if (oldReportProjectFolder != null)
                {
                    reportService.DeleteItem(oldReportProjectFolder.Path + "/" + fileName);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static void Dispose()
        {
            ReportName = "";
            SourcePath = "";
            DestinationPath = "";
        }
    }

}