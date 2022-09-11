#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Personnel.Entities;
using Resources.Areas.Personnel.Entities.Employee;
using Resources.Areas.Personnel.Views.Shared;
using Resources.Shared.Messages;
using Telerik.Web.Mvc.Extensions;
using UI.Areas.Personnel.Controllers.EntitiesRoots;
using UI.Areas.Personnel.Helpers;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using EmployeeRules = Validation.Personnel.Entities.EmployeeRules;
using UI.Filters;

#endregion

namespace UI.Areas.Personnel.Controllers.Entities
{
    public class EmployeeController : EmployeeAggregateController, IRule<Employee>
    {
        #region IRule<Employee> Members

        public ObjectRules<Employee> Rules
        {
            get { return new EmployeeRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("CountryOfBirth.Name");
            ModelState.Remove("Race.Name");
            ModelState.Remove("Nationality.Name");
            ModelState.Remove("Gender.Name");
            ModelState.Remove("MilitaryStatus.Name");
            ModelState.Remove("MaritalStatus.Name");
            ModelState.Remove("Religion.Name");
            ModelState.Remove("Nationality.Name");
            ModelState.Remove("OtherNationality.Name");
            ModelState.Remove("BloodType.Name");
            ModelState.Remove("DisabilityExist.Name");
        }

        #endregion

        #region CRUD

        #region Read
        //[SecurityAction("Manage personnel model",
        //    PermssionSet = "Personnel",
        //    Description = "Allows user to access the personnel model"

        //    )]
        public ActionResult Index(int id = 0, int selectedTabOrder = 0, bool ribbon = false)
        {
            PrePublish();

            #region Security

            if (ViewData["CanRead"] != null && !(bool)ViewData["CanRead"])
            {
                return ErrorPartialMessage(General.CanReadMessage);
            }

            #endregion

            #region Manage Tab, Path, and MastersList

            if (ribbon)
            {
                ClearMasterRecords();
                SaveTabIndex(0);
            }
            else
            {
                if (selectedTabOrder > 0)
                {
                    SaveTabIndex(selectedTabOrder);
                }

                if (id != 0)
                {
                    SetMasterRecordValue(MasterRecordOrder.First, id);
                }
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: PersonnelAreaRegistration.GetAreaName, nodeName: Navigator.Employee);

            #endregion

            #region Get Data

            IQueryable<Employee> employees = Service.GetAll();

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.First) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.First);

                int count = employees.Where(employee => (employee.Id >= masterRecordValue)).Count();

                pageNo = count != 0 ? count : 1;
            }

            ViewData["employees"] = employees;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);
            ViewData["PageTo"] = pageNo;

            ViewData["Path"] = HttpContext.Server.MapPath("~/Areas/Personnel/Uploads");

            #endregion

            return View();
        }

        public ActionResult PartialMasterInfo(int selectedRowId = 0)
        {
            PrePublish();

            if (selectedRowId != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.First, selectedRowId);
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: PersonnelAreaRegistration.GetAreaName, nodeName: Navigator.Employee);

            Employee employee = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            ViewData["ChildrenCounts"] = employee.Children.Count;
            ViewData["DependentsCounts"] = employee.Dependents.Count;

            return PartialView("EmployeeUserControl", employee);
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            LoadStepsList();

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, areaName: PersonnelAreaRegistration.GetAreaName,
                      nodeName: EmployeeModel.EmployeeGeneralInfo);

            return View("Insert", new Employee());
        }

        [HttpPost]
        //[SecurityAction("create personnel model",
        //    PermssionSet = "Personnel",
        //    Description = "Allows user to create the personnel information"

        //    )]
        public ActionResult Insert(Employee employee)
        {
            PrePublish();

            if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
            {
                ErrorPartialMessage(General.NotAllowedToAddMessage);
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }

            var employees = Service.GetAll().Where(x => x.LoginName == employee.LoginName);
            if (employees.Count() != 0)
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("LoginName",
                                                           string.Format(General.LoginNameExistMessage,
                                                                         employees.First().FirstName,
                                                                         employees.First().LastName))
                                };
                ModelState.AddModelErrors(error);
            }

            if ((Rules.GetBrokenRules(employee).Count == 0) && (TryValidateModel(employee)))
            {
                Service.Update(employee);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(employee));

                return View("Insert", employee);
            }

            SetMasterRecordValue(MasterRecordOrder.First, employee.Id);

            PrePublish();

            CacheProvider.ForceUpdate(PersonnelCacheKeys.Employee.ToString());

            return RedirectToAction("Index", new { id = employee.Id });
        }

        #endregion

        #region Update

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee employee = Service.LoadById(id);

            return PartialView("EmployeeUserControlEdit", employee);
        }

        [HttpPost]
        //[SecurityAction("update personnel information",
        //    PermssionSet = "Personnel",
        //    Description = "Allows user to update the personnel information"

        //    )]
        public JsonResult JsonEdit(Employee employee)
        {
            PrePublish();

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = ErrorPartialMessage(General.CanUpdateMessage)
                });
            }

            if ((Rules.GetBrokenRules(employee).Count == 0) && (TryValidateModel(employee)))
            {
                Service.Update(employee);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(employee));

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("EmployeeUserControlEdit", employee)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.First, employee.Id);

            PrePublish();

            CacheProvider.ForceUpdate(PersonnelCacheKeys.Employee.ToString());

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("EmployeeUserControl", employee)
            });
        }

        #endregion

        #region Delete

        [HttpPost]
        //[SecurityAction("delete personnel information",
        //    PermssionSet = "Personnel",
        //    Description = "Allows user to delete the personnel information"

        //    )]
        public ActionResult Delete(int id)
        {
            PrePublish();

            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                return RedirectToAction("Index");
            }

            Employee employee = Service.LoadById(id);

            if (TryUpdateModel(employee))
            {
                try
                {
                    Service.Delete(employee);
                }
                catch (Exception)
                {
                    SetGlobalErrorMessage(General.EntityCurrentlyInUse);
                    return RedirectToAction("Index", this.GridRouteValues());
                }
            }

            SetMasterRecordValue(MasterRecordOrder.First, 0);

            return RedirectToAction("Index", this.GridRouteValues());
        }

        #endregion

        #endregion

        #region Back To Master

        public ActionResult GoToEmployee(int id = 0, int selectedTabOrder = 0, bool ribbon = false)
        {
            if (selectedTabOrder > 0)
            {
                SaveTabIndex(selectedTabOrder);
            }

            return RedirectToAction("Index", "Employee", new { id, ribbon });
        }

        #endregion

        #region Upload

        [HttpPost]
        public ActionResult Upload(int empId = 0)
        {
            var imageHandler = new ImageHandler();

            const float hightZoomScale = 0.15f;
            const float widthtZoomScale = 0.13f;

            HttpPostedFileBase file = Request.Files[0];

            if (file != null)
                if (file.ContentLength > 0)
                {
                    imageHandler.CurrentBitmap = (Bitmap)Image.FromStream(file.InputStream);
                    imageHandler.BitmapPath = file.FileName;

                    var newHight = (int)(imageHandler.CurrentBitmap.Height * hightZoomScale);
                    var newWidth = (int)(imageHandler.CurrentBitmap.Width * widthtZoomScale);

                    imageHandler.Resize(newWidth, newHight);

                    string filePath = Path.Combine(HttpContext.Server.MapPath("~/Areas/Personnel/Uploads"),
                                                   Path.GetFileName(imageHandler.BitmapPath));

                    imageHandler.SaveBitmap(filePath, empId);

                    return RedirectToAction("Index", new { id = empId });
                }

            ErrorPartialMessage(General.NoFileUoload);

            return Json(new
            {
                Success = false,
                Message = General.NoPhotoDelete
            });
        }

        public ActionResult DeletePhoto(int empId)
        {
            string filePath = HttpContext.Server.MapPath("~/Areas/Personnel/Uploads");
            string fullPath = string.Format("{0}//{1}.jpg", filePath, empId);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);

                return Json(new
                {
                    Success = true,
                    Message = "Deleted"
                });
            }

            return Json(new
            {
                Success = false,
                Message = General.NoPhotoDelete
            });
        }

        #endregion
    }
}