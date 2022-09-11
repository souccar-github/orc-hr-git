#region

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Resources;
using System.Threading;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml;
using Google.ProtocolBuffers.Collections;
using Telerik.Web.Mvc;
using UI.Areas.Services.DTO.ViewModels;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.Extensions;
using Telerik.Web.Mvc;
using UI.Helpers.Controllers;

#endregion

namespace UI.Areas.Services.Controllers
{
    public class ResourceEditorController : RootEntityController
    {

        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult GetSoftwareLanguages(string text)
        {
            CultureInfo[] languages = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var softwareLanguages = languages.Select(language => new { language.Name, language.DisplayName });

            if (!string.IsNullOrEmpty(text))
                softwareLanguages = softwareLanguages.Where(language => language.DisplayName.Contains(text)).ToList();

            var result = from softwareLanguage in softwareLanguages
                         select new { Text = softwareLanguage.DisplayName, Value = softwareLanguage.Name };
            var data = result.ToList();

            return new JsonResult { Data = data };
        }

        public ActionResult AssignLanguage(string language)
        {
            if(language != string.Empty)
            {
                var shortLanguage = language.Substring(0, language.IndexOf("-"));

                if (!Directory.Exists(WebConfigurationManager.AppSettings["GeneratorDirectory"] + @"\Resources\" + shortLanguage))
                    return new JsonResult { Data = new object() };    

                var directoryInfo = new DirectoryInfo(WebConfigurationManager.AppSettings["GeneratorDirectory"] + @"\Resources\" + shortLanguage);

                var resourceFileNames = directoryInfo.GetFiles().Select(f => new { f.Name, f.FullName });

                var result = from resourceFileName in resourceFileNames
                             select new { Text = resourceFileName.Name, Value = resourceFileName.FullName };
                var data = result.ToList();

                return new JsonResult { Data = data };    
            }

            return new JsonResult { Data = new object() };    
        }

        public ActionResult ResourceGrid(string fullPath)
        {
            Session["fullPath"] = fullPath;

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("Grid")
            });
        }

        public ActionResult ReadResource()
        {
            return this.Json(RefreshGridModel());
        }

        [HttpPost]
        public ActionResult UpdateResource(string id)
        {
            var resource = new ResourceEditorViewModel { Key = id };

            if (TryUpdateModel(resource))
            {
                var obj = new XmlDocument();
                obj.Load(Session["fullPath"].ToString());
                XmlNode rootNode = obj.SelectSingleNode("root");
                
                if (rootNode != null)

                    foreach (XmlNode node in rootNode)
                    {
                        if (node.Attributes != null && ((node.Name == "data") && node.Attributes["name"].Value == id))
                        {
                            var singleNode = node.SelectSingleNode("value");
                            if (singleNode != null) singleNode.InnerText = resource.Value;
                            break;
                        }
                    }

                obj.Save(Session["fullPath"].ToString());
            }

            return this.Json(RefreshGridModel());
        }

        public IList<ResourceEditorViewModel> GetResourceList(IList<ResourceEditorViewModel> resources)
        {
            return resources.Select(a => new ResourceEditorViewModel()
                                                     {
                                                         Key = a.Key,
                                                         Value = a.Value
                                                     }).ToList();
        }

        private GridModel RefreshGridModel()
        {
            var currentPage = this.ValueOf<int>(GridUrlParameters.CurrentPage);
            var pageSize = this.ValueOf<int>(GridUrlParameters.PageSize);
            var orderBy = this.ValueOf<string>(GridUrlParameters.OrderBy);
            var filter = this.ValueOf<string>(GridUrlParameters.Filter);
            var query = ReadResoorceFileData().AsQueryable();
            var model = query.ToGridModel(currentPage, pageSize, orderBy, null, filter);
            model.Data = GetResourceList((IList<ResourceEditorViewModel>)model.Data.AsQueryable().ToIList());
            return model;
        }

        private IEnumerable<ResourceEditorViewModel> ReadResoorceFileData()
        {
            if(Session["fullPath"] != null && Session["fullPath"].ToString() != string.Empty)
            {
                var rsxr = new ResXResourceReader(Session["fullPath"].ToString());

                return (from DictionaryEntry d in rsxr select new ResourceEditorViewModel { Key = d.Key.ToString(), Value = d.Value.ToString() }).ToList();    
            }
            
            return new List<ResourceEditorViewModel>();
        }
    }
}