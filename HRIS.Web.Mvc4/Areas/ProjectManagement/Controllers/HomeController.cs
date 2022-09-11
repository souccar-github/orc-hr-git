using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;

using HRIS.Domain.ProjectManagement;
using Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;

using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

using Souccar.Core.Extensions;
using HRIS.Domain.ProjectManagement.Indexes;






namespace Project.Web.Mvc4.Areas.ProjectManagement.Controllers
{
    public class HomeController : Controller
    {
        private string _message = string.Empty;
        private bool _isSuccess;
        private List<ValidationResult> _validationResults;

        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.ProjectManagement });

            return View();
        }
        [HttpPost]
        public ActionResult GetKPItInfo(IDictionary<string, object> model)
        {
            var project = GetProject((int)model["ProjectId"].To(typeof(int)));
            var result = new Dictionary<string, object>();

            if (project != null)
            {
                result["KPIwieght"] = project.KPIwieght;
                result["KPIvalue"] = project.KPIvalue;
                result["KPIdescription"] = project.KPIdescription == null ? string.Empty : project.KPIdescription.ToString();
                result["KPItype"] = project.KPItype == null ? string.Empty : project.KPItype.Id.ToString(); 
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult SaveKpiInformation(IDictionary<string, object> model)
        {
            if (IsValidKPIInfo(model))
            {
                InitialzeDefaultValues();

                try
                {
                    var project = GetProject((int)model["ProjectId"].To(typeof(int)));


                    if (project != null)
                    {


                        KPItype kpiType = null;

                        if (model["KPItype"] != null && !string.IsNullOrEmpty(model["KPItype"].ToString()))
                            kpiType = ServiceFactory.ORMService.GetById<KPItype>((int)model["KPItype"].To(typeof(int)));

                        project.KPItype = kpiType;
                        project.KPIwieght = (int)model["KPIwieght"].To(typeof(int));
                        project.KPIvalue = (int)model["KPIvalue"].To(typeof(int));
                        project.KPIdescription = model["KPIdescription"] == null ? string.Empty : model["KPIdescription"].ToString(); 
                        project.Save();
                        _isSuccess = true;
                            

                    }
                }
                catch
                {
                }
            }


            return Json(new
            {
                Success = _isSuccess,
                Msg = _message
            });
        }

        private void InitialzeDefaultValues()
        {
            _isSuccess = false;
            _message = Helpers.GlobalResource.FailMessage;
        }
        private HRIS.Domain.ProjectManagement.RootEntities.Project GetProject(int id)
        {
            var project = ServiceFactory.ORMService.GetById<HRIS.Domain.ProjectManagement.RootEntities.Project>(id);

            return project;
        }
        private bool IsValidKPIInfo(IDictionary<string, object> model)
        {
            if (!model.ContainsKey("KPItype") || model["KPItype"] == null ||
                string.IsNullOrEmpty(model["KPItype"].ToString()) ||
                int.Parse(model["KPItype"].ToString()) == 0)
            {
                _message = ProjectManagementLocalizationHelper.GetResource(ProjectManagementLocalizationHelper.MsgKpiTypeIsRequired);
                return false;
            }

            if (!model.ContainsKey("KPIwieght") || model["KPIwieght"] == null ||
                string.IsNullOrEmpty(model["KPIwieght"].ToString()) ||
                int.Parse(model["KPIwieght"].ToString()) == 0)
            {
                _message = ProjectManagementLocalizationHelper.GetResource(ProjectManagementLocalizationHelper.MsgKpiWieghtIsRequired);
                return false;
            }

            if (!model.ContainsKey("KPIvalue") || model["KPIvalue"] == null ||
                string.IsNullOrEmpty(model["KPIvalue"].ToString()) ||
                int.Parse(model["KPIvalue"].ToString()) == 0)
            {
                _message = ProjectManagementLocalizationHelper.GetResource(ProjectManagementLocalizationHelper.MsgKpiVlaueIsRequired);
                return false;
            }

            return true;
        }
    }
}
