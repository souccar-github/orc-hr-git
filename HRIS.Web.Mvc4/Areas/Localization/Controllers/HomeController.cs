using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.Enums;
using Project.Web.Mvc4.Models;
using Souccar.CodeGenerator.Resourecs;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models.Navigation;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Areas.Recruitment.Models;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Factories;
using HRIS.Domain.Personnel.Helpers;
using System.Text;
using System.IO;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Localization;
using Project.Web.Mvc4.Extensions;

using Souccar.Core.Extensions;
using Souccar.Domain.Extensions;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PMS.Helpers;
using HRIS.Domain.OrganizationChart.Helpers;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Objectives.RootEntities;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Areas.Localization.Models;
using Project.Web.Mvc4.ProjectModels;
using Project.Web.Mvc4.Areas.Reporting.Helpers;
using System.Data;
using ClosedXML.Excel;
using System.Xml;
using System.Xml.Xsl;
using HRIS.Domain.Recruitment.Helpers;
using HRIS.Domain.Workflow;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.Localization.Controllers
{
    public class HomeController : Controller
    {
        //private const string fileExcelPath = "D:\\ORC System Language\\";
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.Localization });

            return View();
        }

        public ActionResult GenerateLocalization(int langId)
        {
            var generator = new NHibernateResourceGenerator();
            var start = DateTime.Now;
            var command = GlobalResource.GenerateValues;
            var language = ServiceFactory.ORMService.GetById<Language>(langId);

            generator.GenerateByModuleNameAndLanguageId(langId, new List<string> { ModulesNames.Personnel, ModulesNames.OrganizationChart, ModulesNames.JobDescription, ModulesNames.PMS, ModulesNames.Training, ModulesNames.Recruitment }, typeof(Employee).Assembly);
            generator.GenerateByModuleNameAndLanguageId(langId, new List<string> { ModulesNames.EmployeeRelationServices, ModulesNames.HealthInsurance, ModulesNames.Objectives, ModulesNames.PayrollSystem, ModulesNames.AttendanceSystem, ModulesNames.Incentive, ModulesNames.Grades }, typeof(Employee).Assembly);

            generator.GenerateByModuleNameAndLanguageId(langId, new List<string> { ModulesNames.Security, ModulesNames.Localization, ModulesNames.Reporting }, typeof(Language).Assembly);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(CustomMessageKeysPersonnelModule), CustomMessageKeysPersonnelModule.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(PreDefinedMessageKeysSpecExpress), PreDefinedMessageKeysSpecExpress.ResourceGroupName);

            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(GridModelLocalizationConst), GridModelLocalizationConst.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(CustomMessageKeysEmployeeRelationServicesModule), CustomMessageKeysEmployeeRelationServicesModule.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(CustomMessageKeysHealthInsuranceModule), CustomMessageKeysHealthInsuranceModule.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(CustomMessageKeysPayrollSystemModule), CustomMessageKeysPayrollSystemModule.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(CustomMessageKeysAttendanceSystemModule), CustomMessageKeysAttendanceSystemModule.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(ModulesNames), ModulesNames.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(CommandsNames), CommandsNames.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(NavigationTabName), NavigationTabName.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(RecruitmentLocalizationHelper), RecruitmentLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(LocalizationHelper), LocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(AccountLocalizationHelper), AccountLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(NotificationLocalizationHelper), NotificationLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(JobDescriptionLocalizationHelper), JobDescriptionLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(TrainingLocalizationHelper), TrainingLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(OrganizationChartLocalizationHelper), OrganizationChartLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(PersonnelLocalizationHelper), PersonnelLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(EmployeeRelationServicesLocalizationHelper), EmployeeRelationServicesLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(PMSLocalizationHelper), PMSLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(ObjectiveLocalizationHelper), ObjectiveLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(IncentiveLocalizationHelper), IncentiveLocalizationHelper.ResourceGroupName);
            //  generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(IncentiveGoupesNames), IncentiveGoupesNames.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(AttendanceLocalizationHelper), AttendanceLocalizationHelper.ResourceGroupName);

            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(IndexLocalizationHelper), IndexLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(WorkflowLocalizationHelper), WorkflowLocalizationHelper.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(PersonnelGoupesNames), PersonnelGoupesNames.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(OrgChartGoupesNames), OrgChartGoupesNames.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(RecruitmentGroupsNames), RecruitmentGroupsNames.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(EmployeeRelationServicesGroupNames), EmployeeRelationServicesGroupNames.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(PMSGoupesNames), PMSGoupesNames.ResourceGroupName);
            // generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(TrainingGoupesNames), TrainingGoupesNames.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(GlobalMessages), GlobalMessages.ResourceGroupName);
            generator.GenerateOrUpdateResourceForConstFieldByLanguageId(langId, typeof(CustomMessageKeysObjectiveModule), GlobalMessages.ResourceGroupName);

            generator.GenerateOrUpdateResourceForTypeOrEnumByType(langId, typeof(EmployeeCountStatus));
            generator.GenerateOrUpdateResourceForTypeOrEnumByType(langId, typeof(Salaries));
            generator.GenerateOrUpdateResourceForTypeOrEnumByType(langId, typeof(WorkflowType));
            generator.GenerateOrUpdateResourceForTypeOrEnumByType(langId, typeof(DayOfWeek));
            generator.GenerateOrUpdateResourceForTypeOrEnumByType(langId, typeof(ObjectiveAppraisalPhase));
            generator.GenerateResourceForReports(langId);
            generator.GenerateResourceForReports(langId);

            // Indexes Builder
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { }, UserExtensions.CurrentUser, language, Souccar.Domain.Audit.OperationType.Update, command, start);

            return Content("Done");
        }

        public ActionResult GenerateReportsLocalization(int langId)
        {
            var reportResult = ReportingHelper.GenerateResourceForReports(langId);
            if (reportResult.Count > 0)
            {
                return Json(new
                {
                    Success = false,
                    Msg = GlobalResource.FailedToGenerateValuesForSomeReports,

                }, JsonRequestBehavior.AllowGet
                );
            }
            else
                return Content("Done");
        }

        public ActionResult UpdateReportsValues(int langId)
        {
            var lang = ServiceFactory.ORMService.GetById<Language>(langId);

            if (!lang.IsActive)
                return Json(new { Success = false, Msg = lang.Name + " " + LocalizationHelper.GetResource(LocalizationHelper.MustBeTheDefaultLanguage) }
                , JsonRequestBehavior.AllowGet);

            ReportingHelper.UpdateReportsRDL(lang);
            bool b;
            var reportResult = ReportingHelper.SynchronizeReports(out b);
            bool isSuccess;

            if (b)
                isSuccess = true;
            else
                isSuccess = false;

            return Json(new { Success = isSuccess, Msg = reportResult }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImportValues(int langId)
        {
            var lan = ServiceFactory.ORMService.GetById<Language>(langId);
            var start = DateTime.Now;
            var command = GlobalResource.ImportValues;
            var reader = new StreamReader(System.IO.File.OpenRead(@"d:\ORC System Language\" + lan.Name + ".csv"), Encoding.Default);

            //var lanName = id.Substring(0, 2);
            //var lan = ServiceFactory.ORMService.All<Language>().SingleOrDefault(x => x.LanguageCulture.ToLower().StartsWith(lanName));

            if (lan != null)
            {
                if (!reader.EndOfStream)
                    reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var temp = lan.LocaleStringResources.SingleOrDefault(x => x.ResourceName.Equals(values[0]));
                    if (temp != null)
                    {
                        temp.ResourceValue = values[1];
                        temp.ResourceStatus = ResourceStatus.ImportedFromExcel;
                    }
                }
            }
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { lan }, UserExtensions.CurrentUser, lan, Souccar.Domain.Audit.OperationType.Update, command, start);
            reader.Close();
            return Content("Done");
        }


        public ActionResult ExportValues(int langId)
        {
            var start = DateTime.Now;
            var command = GlobalResource.ExportValues;
            var lan = ServiceFactory.ORMService.GetById<Language>(langId);
            var resourcesList = lan.LocaleStringResources.ToList()
              .Select(x => new ExportViewModel()
              {
                  ResourceName = x.ResourceName.ToString(),
                  ResourceValue = x.ResourceValue.ToString()
              });

            string appendText = "ResourceName , ResourceValue" + Environment.NewLine; ;
            string path = "D:\\ORC System Language\\";

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            path += lan.Name + ".csv";
            foreach (var resources in resourcesList)
            {
                appendText += resources.ResourceName + "," + resources.ResourceValue + Environment.NewLine;
            }
            System.IO.File.WriteAllText(path, appendText, Encoding.UTF8);
            //System.IO.File.WriteAllText(path, String.Join("", appendText, Encoding.UTF8));
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { }, UserExtensions.CurrentUser, lan, Souccar.Domain.Audit.OperationType.Update, command, start);
            return Content("Done");
        }

        public ActionResult SaveCsvFile(IEnumerable<HttpPostedFileBase> files)
        {
            var isSuccess = false;
            var message = GlobalResource.Fail;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = string.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(file.FileName));
                    if (!fileName.Contains("csv"))
                    {
                        isSuccess = false;
                        message = "TheFileExtensionIsInvalid";//GlobalResource.TheFileExtensionIsInvalid;
                        break;
                    }

                    Session["CsvFilePhysicalPath"] = Path.Combine(System.IO.Path.GetTempPath(), fileName);
                    Session["CsvFileName"] = fileName;
                    file.SaveAs(Session["CsvFilePhysicalPath"].ToString());

                    isSuccess = true;
                    message = "";
                }
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message
            });
        }

        public ActionResult ImportCsvFile(int langId)
        {
            var success = false;
            var message = GlobalResource.Fail;

            var language = ServiceFactory.ORMService.GetById<Language>(langId);
            var path = Session["CsvFilePhysicalPath"].ToString();
            var reader = new StreamReader(System.IO.File.OpenRead(path), Encoding.Default);
            if (reader != null)
            {
                if (language != null)
                {
                    if (!reader.EndOfStream)
                        reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        var temp = language.LocaleStringResources.SingleOrDefault(x => x.ResourceName.Equals(values[0]));
                        if (temp != null)
                        {
                            temp.ResourceValue = values[1];
                            temp.ResourceStatus = ResourceStatus.ImportedFromExcel;
                        }
                    }

                    success = true;
                    language.Save();
                }

                reader.Close();
            }

            return Json(new { Success = success, Msg = message });
        }

        public FileContentResult ExportCsvFile(int langId)
        {
            var language = ServiceFactory.ORMService.GetById<Language>(langId);


            StringBuilder builder = new StringBuilder();
            builder.Append("ResourceName , ResourceValue" + Environment.NewLine);
            language.LocaleStringResources.ToList()
               .ForEach((resource) =>
               {
                   builder.Append(resource.ResourceName + ','+ resource.ResourceValue);
                   builder.Append("\r\n");
               });
            string text = builder.ToString();
            string fileName = language.Name + ".csv";
            return File(Encoding.GetEncoding(1256).GetBytes(text), "text/csv", fileName);
        }

        

        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }

    }

    
}

