#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml;
using Telerik.Web.Mvc;
using UI.Areas.Services.DTO.ViewModels;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.Extensions;
using Telerik.Web.Mvc;
using UI.Helpers.Controllers;
using System.IO;

#endregion

namespace UI.Areas.Services.Controllers
{
    public class ResourceFileTranslateController : RootEntityController
    {

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult TranslateResourceFile(string sourceFilePath, string sourceFileName,string language)
        {
            try
            {
                var shortLanguage = language.Substring(0, language.IndexOf("-"));
                const string suffix = ".resx"; //put it in app config file
                var destinationFileName = sourceFileName.Insert(sourceFileName.IndexOf(suffix), "." + shortLanguage);
                var destinationDirectory = WebConfigurationManager.AppSettings["GeneratorDirectory"] + @"\Resources\" + shortLanguage + @"\";
                var destinationFilePath = destinationDirectory + destinationFileName;
                if (!Directory.Exists(destinationDirectory))
                    Directory.CreateDirectory(destinationDirectory);

                if (System.IO.File.Exists(destinationFilePath))
                    return Json(new
                    {
                        Success = false,
                        Msg = Resources.Areas.Services.ResourceFileTranslate.ResourceFileTranslateRules.FileAlreadyExist
                    });

                System.IO.File.Copy(sourceFilePath, destinationFilePath);    
                 
            }
            catch(Exception exception)
            {
                return Json(new
                {
                    Success = false,
                    Msg = Resources.Areas.Services.ResourceFileTranslate.ResourceFileTranslateRules.OperationFailed
                });
            }

            return Json(new
            {
                Success = true,
                Msg = Resources.Areas.Services.ResourceFileTranslate.ResourceFileTranslateRules.OperationSuccessed
            });
        }

        [HttpPost]
        public ActionResult GetRsourceFileNames(string text)
        {
            var directoryInfo = new DirectoryInfo(WebConfigurationManager.AppSettings["GeneratorDirectory"] + @"\Resources\en");

            var resourceFileNames = directoryInfo.GetFiles().Select(f => new { f.Name, f.FullName });

            if (!string.IsNullOrEmpty(text))
                resourceFileNames = resourceFileNames.Where(file => file.Name.Contains(text));

            var result = from resourceFileName in resourceFileNames
                         select new { Text = resourceFileName.Name, Value = resourceFileName.FullName };
            var data = result.ToList();

            return new JsonResult { Data = data };
        }

        [HttpPost]
        public ActionResult GetSoftwareLanguages(string text)
        {
            CultureInfo[] languages = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var softwareLanguages = languages.Select(language => new { language.Name, language.DisplayName });

            if (!string.IsNullOrEmpty(text))
                softwareLanguages = softwareLanguages.Where(language => language.DisplayName.Contains(text));

            var result = from softwareLanguage in softwareLanguages
                         select new { Text = softwareLanguage.DisplayName, Value = softwareLanguage.Name };
            var data = result.ToList();

            return new JsonResult { Data = data };
        }

    }
}