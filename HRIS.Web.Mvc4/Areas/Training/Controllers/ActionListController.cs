using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Training.Entities;
using HRIS.Domain.Training.Enums;
using HRIS.Domain.Training.Indexes;
using HRIS.Domain.Training.RootEntities;
using HRIS.Validation.Specification.Training.Entities;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Souccar.Core.Extensions;
using Souccar.Domain.Validation;

namespace Project.Web.Mvc4.Areas.Training.Controllers
{
    public class ActionListController : Controller
    {

        #region Add Training Needs

        [HttpPost]
        public ActionResult GetUnSelectedTrainingNeedGridModel(int id)
        {
            var gridModel = GridViewModelFactory.Create(typeof(TrainNeedViewModel), null);
            gridModel.Views[0].ReadUrl = "Training/ActionList/ReadUnSelectedTrainingNeedData/" + id;
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    Name = GlobalResource.AddSelected,//"AddButton",
                    ClassName = "k-button k-button-icontext k-grid-add AddTrainingNeedButton",
                    Handler = "AddButton",
                    ImageClass = "k-icon k-add",
                    Text = GlobalResource.AddSelected
                }
            };
            
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadUnSelectedTrainingNeedData(int id, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var course = (Course)typeof(Course).GetById(id);
            var courseTrainingNeeds = course.CourseTrainingNeeds;


            List<TrainNeedViewModel> trainingNeeds;
            if (courseTrainingNeeds.Any())
            {
                var courseTrainingNeedsIds = courseTrainingNeeds.Select(x => x.TrainingNeed.Id).ToArray();

                trainingNeeds = ServiceFactory.ORMService.All<TrainingNeed>()
                    .Where(x => !courseTrainingNeedsIds.Contains(x.Id) &&
                                (x.Status == TrainingNeedStatus.Initial))
                    .Select(x => new TrainNeedViewModel()
                    { Id = x.Id, Name = x.Name, Level = x.Level.Name, IsChecked = false }).ToList();
            }
            else
            {
                trainingNeeds = ServiceFactory.ORMService.All<TrainingNeed>()
                    .Where(x => x.Status == TrainingNeedStatus.Initial)
                    .Select(x => new TrainNeedViewModel()
                    { Id = x.Id, Name = x.Name, Level = x.Level.Name, IsChecked = false }).ToList();
            }

            var queryable = trainingNeeds.AsQueryable();
            CrudController.UpdateFilter(filter, typeof(TrainNeedViewModel));
            var dataSourse = DataSourceResult.GetDataSourceResult(queryable, typeof(TrainNeedViewModel), pageSize, skip, serverPaging, sort, filter);
            return Json(new { Data = dataSourse.Data, TotalCount = dataSourse.Total });
        }

        [HttpPost]
        public ActionResult GetSelectedTrainingNeedGridModel(int id)
        {
            var gridModel = GridViewModelFactory.Create(typeof(TrainNeedViewModel), null);
            gridModel.Views[0].ReadUrl = "Training/ActionList/ReadSelectedTrainingNeedData/" + id;
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    Name = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.RemoveSelected),//"RemoveButton",
                    ClassName = "k-button k-button-icontext k-grid-add RemoveTrainingNeedButton",
                    Handler = "RemoveButton",
                    ImageClass = "k-icon k-delete",
                    Text = ServiceFactory.LocalizationService.GetResource(TrainingLocalizationHelper
                        .GetResource(TrainingLocalizationHelper.RemoveSelected))
                }
            };
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadSelectedTrainingNeedData(int id, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var course = (Course)typeof(Course).GetById(id);
            var trainingNeeds = course.CourseTrainingNeeds.Select(x => new TrainNeedViewModel()
            { Id = x.TrainingNeed.Id, Name = x.TrainingNeed.Name, Level = x.TrainingNeed.Level.Name, IsChecked = true });


            var queryable = trainingNeeds.AsQueryable();

            CrudController.UpdateFilter(filter, typeof(TrainNeedViewModel));
            var dataSourse = DataSourceResult.GetDataSourceResult(queryable, typeof(TrainNeedViewModel), pageSize, skip, serverPaging, sort, filter);
            return Json(new { Data = dataSourse.Data, TotalCount = dataSourse.Total });
        }

        public ActionResult SaveSelectedTrainingNeed(List<IDictionary<string, object>> model, int courseId)
        {
            var course = (Course)typeof(Course).GetById(courseId);
            var start = DateTime.Now;
            if (model == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in model)
            {
                var trainingNeed = (TrainingNeed)typeof(TrainingNeed).GetById((int)item["Id"]);
                if (trainingNeed != null)
                {
                    trainingNeed.Status = TrainingNeedStatus.Pending;
                    trainingNeed.Save();

                    var courseTrainingNeed = new CourseTrainingNeed()
                    {
                        Course = course,
                        TrainingNeed = trainingNeed
                    };
                    course.AddTrainingNeed(courseTrainingNeed);
                }

            }
           // ServiceFactory.ORMService.Save(course, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(course);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AddSelectedTrainingNeed);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() {course });

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUnSelectedTrainingNeed(List<IDictionary<string, object>> model, int courseId)
        {
            var course = (Course)typeof(Course).GetById(courseId);
            var start = DateTime.Now;
            if (model == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in model)
            {
                var trainingNeed = (TrainingNeed)typeof(TrainingNeed).GetById((int)item["Id"]);
                if (trainingNeed != null)
                {
                    trainingNeed.Status = TrainingNeedStatus.Initial;
                    trainingNeed.Save();

                    var courseTrainingNeed = course.CourseTrainingNeeds.FirstOrDefault(x => x.TrainingNeed.Id == trainingNeed.Id);
                    course.CourseTrainingNeeds.Remove(courseTrainingNeed);
                }
            }
           // ServiceFactory.ORMService.Save(course, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(course);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AddUnSelectedTrainingNeed);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { course });

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Activate Training Course

        [HttpPost]
        public ActionResult GetTrainingCourseInformation(int id)
        {
            var course = (Course)typeof(Course).GetById(id);
            var result = new Dictionary<string, object>();

            if (course.CourseName != null)
                result["CourseName"] = course.CourseName.Name;

            if (course.Specialize != null)
                result["Specialize"] = course.Specialize.Name;

            if (course.CourseType != null)
                result["CourseType"] = course.CourseType.Name;

            if (course.Priority != null)
                result["Priority"] = course.Priority.Name;

            if (course.LanguageName != null)
                result["LanguageName"] = course.LanguageName.Name;

            if (course.CourseLevel != null)
                result["CourseLevel"] = course.CourseLevel.Name;

            result["Status"] = course.Status.ToString();

            result["CourseTitle"] = course.CourseTitle;
            result["Duration"] = course.Duration;
            result["NumberOfSession"] = course.NumberOfSession;
            result["OnceSessionLength"] = (course.NumberOfSession != 0) ? course.Duration / course.NumberOfSession : 0;

            if (course.Sponsor != null)
                result["Sponsor"] = course.Sponsor.Id;

            if (course.TrainingCenterName != null)
                result["TrainingCenterName"] = course.TrainingCenterName.Id;

            if (course.TrainingPlace != null)
                result["TrainingPlace"] = course.TrainingPlace.Id;

            if (course.Trainer != null)
                result["Trainer"] = course.Trainer.Id;

            result["StartDate"] = course.StartDate;
            result["EndDate"] = course.EndDate;
            result["Saturday"] = course.Saturday;
            result["Sunday"] = course.Sunday;
            result["Monday"] = course.Monday;
            result["Tuesday"] = course.Tuesday;
            result["Wednesday"] = course.Wednesday;
            result["Thursday"] = course.Thursday;
            result["Friday"] = course.Friday;

            result["StartHour"] = course.StartHour;


            return Json(result);
        }

        public ActionResult SaveTrainingCourseInformation(IDictionary<string, object> model, int id)
        {
            bool isSuccess = false;
            var start = DateTime.Now;
            string message = GlobalResource.ExceptionMessage;
            var validationMessages = new Dictionary<string, string>();

            var course = (Course)typeof(Course).GetById(id);
            var employees = course.CourseEmployees.ToList();
            try
            {
                TrainingCenterName trainingCenterName = null;
                if ((int)model["TrainingCenterName"].To(typeof(int)) != 0)
                {
                    trainingCenterName =
                        (TrainingCenterName)
                            typeof(TrainingCenterName).GetById((int)model["TrainingCenterName"].To(typeof(int)));
                }

                Trainer trainer = null;
                if ((int)model["Trainer"].To(typeof(int)) != 0)
                {
                    trainer = (Trainer)typeof(Trainer).GetById((int)model["Trainer"].To(typeof(int)));
                }

                TrainingPlace trainingPlace = null;
                if ((int)model["TrainingPlace"].To(typeof(int)) != 0)
                {
                    trainingPlace =
                        (TrainingPlace)
                            typeof(TrainingPlace).GetById((int)model["TrainingPlace"].To(typeof(int)));
                }

                CourseSponsor sponsor = null;
                if ((int)model["Sponsor"].To(typeof(int)) != 0)
                {
                    sponsor =
                        (CourseSponsor)typeof(CourseSponsor).GetById((int)model["Sponsor"].To(typeof(int)));
                }

                course.Status = CourseStatus.Activated;
                course.CourseTitle = (string)model["CourseTitle"];
                course.NumberOfSession = model["NumberOfSession"] != null ? (int)model["NumberOfSession"] : 0;
                course.Duration = model["Duration"] != null ? (int)model["Duration"] : 0;
                course.Sponsor = sponsor;
                course.TrainingCenterName = trainingCenterName;
                course.TrainingPlace = trainingPlace;
                course.Trainer = trainer;
                course.StartDate = model["StartDate"] != null ? DateTime.Parse(model["StartDate"].ToString()) : (DateTime?)null;
                course.EndDate = model["EndDate"] != null ? DateTime.Parse(model["EndDate"].ToString()) : (DateTime?)null;
                course.Saturday = model["Saturday"] != null ? bool.Parse(model["Saturday"].ToString()) : false;
                course.Sunday = model["Sunday"] != null ? bool.Parse(model["Sunday"].ToString()) : false;
                course.Monday = model["Monday"] != null ? bool.Parse(model["Monday"].ToString()) : false;
                course.Tuesday = model["Tuesday"] != null ? bool.Parse(model["Tuesday"].ToString()) : false;
                course.Wednesday = model["Wednesday"] != null ? bool.Parse(model["Wednesday"].ToString()) : false;
                course.Thursday = model["Thursday"] != null ? bool.Parse(model["Thursday"].ToString()) : false;
                course.Friday = model["Friday"] != null ? bool.Parse(model["Friday"].ToString()) : false;
                course.StartHour = model["StartHour"] != null ? DateTime.Parse(model["StartHour"].ToString()) : (DateTime?)null;

                var courseSpecification = new ActivateCourseSpecification();
                var validationResults = (List<ValidationResult>)ServiceFactory.ValidationService.Validate(course, courseSpecification);


                if (validationResults.Any())
                {
                    foreach (var error in validationResults)
                    {

                        if (validationMessages.Keys.All(x => x != error.Property.Name))
                            validationMessages.Add(error.Property.Name, error.Message);
                    }
                }
                else
                {
                   // ServiceFactory.ORMService.Save(course, UserExtensions.CurrentUser);
                    var entities = new List<IAggregateRoot>();
                    entities.Add(course);
                    var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AddTrainingCourseInformation);

                    ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { course });

                    isSuccess = true;
                    message = GlobalResource.Done;
                }

            }
            catch
                (Exception ex)
            {
                validationMessages = new Dictionary<string, string> { { "Exception", GlobalResource.ExceptionMessage } };
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message,
                Errors = validationMessages
            });
        }


        #endregion

        #region Add Suggest Staffs

        [HttpPost]
        public ActionResult GetUnSelectedSuggestStaffGridModel(int id)
        {
            var gridModel = GridViewModelFactory.Create(typeof(EmpViewModel), null);
            gridModel.Views[0].ReadUrl = "Training/ActionList/ReadUnSelectedSuggestStaffData/" + id;
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    Name = GlobalResource.AddSelected,
                    ClassName = "k-button k-button-icontext k-grid-add AddSuggestStaffButton",
                    Handler = "AddButton",
                    ImageClass = "k-icon k-add",
                    Text = GlobalResource.AddSelected
                }
            };
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadUnSelectedSuggestStaffData(int id, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var course = (Course)typeof(Course).GetById(id);
            var courseSuggestStaffs = course.CourseEmployees;

            IQueryable<Employee> employees;
            List<EmpViewModel> suggestStaffs = new List<EmpViewModel>();
            if (courseSuggestStaffs.Any())
            {
                var courseSuggestStaffsIds = courseSuggestStaffs.Select(x => x.Employee.Id).ToArray();

                employees = ServiceFactory.ORMService.All<Employee>()
                    .Where(x => !courseSuggestStaffsIds.Contains(x.Id) && x.EmployeeCard != null && x.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork);

            }
            else
            {
                employees = ServiceFactory.ORMService.All<Employee>().Where(x =>  x.EmployeeCard != null && x.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork);
            }

            foreach (var employee in employees)
            {
                

                var position = GetEmployeePosition(employee);

                if (position != null)
                {
                    var employeeViewModel = new EmpViewModel()
                    {
                        Id = employee.Id,
                        Name = employee.FullName,

                        IsVertualDeleted = employee.IsVertualDeleted,
                        IsChecked = false
                    };

                    employeeViewModel.Node = position.JobDescription != null
                        ? position.JobDescription.Node.Name
                        : string.Empty;

                    employeeViewModel.Position = position.NameForDropdown;

                    suggestStaffs.Add(employeeViewModel);
                }

                

                
            }

            var queryable = suggestStaffs.AsQueryable();
            CrudController.UpdateFilter(filter, typeof(EmpViewModel));
            var dataSourse = DataSourceResult.GetDataSourceResult(queryable, typeof(EmpViewModel), pageSize, skip, serverPaging, sort, filter);
            return Json(new { Data = dataSourse.Data, TotalCount = dataSourse.Total });
        }

        [HttpPost]
        public ActionResult GetSelectedSuggestStaffGridModel(int id)
        {
            var gridModel = GridViewModelFactory.Create(typeof(EmpViewModel), null);
            gridModel.Views[0].ReadUrl = "Training/ActionList/ReadSelectedSuggestStaffData/" + id;
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    Name = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.RemoveSelected),//"RemoveButton",
                    ClassName = "k-button k-button-icontext k-grid-add RemoveSuggestStaffButton",
                    Handler = "RemoveButton",
                    ImageClass = "k-icon k-delete",
                    Text = ServiceFactory.LocalizationService.GetResource(TrainingLocalizationHelper
                        .GetResource(TrainingLocalizationHelper.RemoveSelected))
                }
            };
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadSelectedSuggestStaffData(int id, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var course = (Course)typeof(Course).GetById(id);
            var suggestStaffs = course.CourseEmployees.Where(x => x.Type == CourseEmployeeType.Candidate).Select(x =>
                new EmpViewModel()
                {
                    Id = x.Employee.Id,
                    Name = x.Employee.FullName,
                    Position = GetEmployeePosition(x.Employee) != null
                        ? GetEmployeePosition(x.Employee).NameForDropdown
                        : string.Empty,
                    Node = GetEmployeePosition(x.Employee).JobDescription != null
                        ? GetEmployeePosition(x.Employee).JobDescription.Node.Name
                        : string.Empty,
                    IsChecked = true
                });


            var queryable = suggestStaffs.AsQueryable();

            CrudController.UpdateFilter(filter, typeof(EmpViewModel));
            var dataSource = DataSourceResult.GetDataSourceResult(queryable, typeof(EmpViewModel), pageSize, skip, serverPaging, sort, filter);
            return Json(new { Data = dataSource.Data, TotalCount = dataSource.Total });
        }

        public ActionResult SaveSelectedSuggestStaff(List<IDictionary<string, object>> model, int courseId)
        {
            var course = (Course)typeof(Course).GetById(courseId);
            var start = DateTime.Now;
            if (model == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in model)
            {
                var suggestStaff = (Employee)typeof(Employee).GetById((int)item["Id"]);
                if (suggestStaff != null)
                {
                    var courseSuggestStaff = new CourseEmployee()
                    {
                        Course = course,
                        Employee = suggestStaff,
                        Type = CourseEmployeeType.Candidate
                    };

                    course.AddCourseEmployee(courseSuggestStaff);
                }

            }
           // ServiceFactory.ORMService.Save(course, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(course);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AddSelectedSuggestStaff);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { course });


            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUnSelectedSuggestStaff(List<IDictionary<string, object>> model, int courseId)
        {
            var course = (Course)typeof(Course).GetById(courseId);
            var start = DateTime.Now;
            if (model == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in model)
            {
                var suggestStaff = (Employee)typeof(Employee).GetById((int)item["Id"]);
                if (suggestStaff != null)
                {
                    var courseEmployee = course.CourseEmployees.FirstOrDefault(x => x.Employee.Id == suggestStaff.Id);
                    course.CourseEmployees.Remove(courseEmployee);
                }
            }
         //   ServiceFactory.ORMService.Save(course, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(course);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AddeUnSelectedSuggestStaff);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() {  });

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Add Trainees

        [HttpPost]
        public ActionResult GetUnSelectedTraineeGridModel(int id)
        {
            var gridModel = GridViewModelFactory.Create(typeof(EmpViewModel), null);
            gridModel.Views[0].ReadUrl = "Training/ActionList/ReadUnSelectedTraineeData/" + id;
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    Name = GlobalResource.AddSelected,//"AddButton",
                    ClassName = "k-button k-button-icontext k-grid-add AddTraineeButton",
                    Handler = "AddButton",
                    ImageClass = "k-icon k-add",
                    Text = GlobalResource.AddSelected
                }
            };
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadUnSelectedTraineeData(int id, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var course = (Course)typeof(Course).GetById(id);

            var employees = course.CourseEmployees.Where(x => x.Type == CourseEmployeeType.Candidate).Select(x =>
                new EmpViewModel()
                {
                    Id = x.Employee.Id,
                    Name = x.Employee.FullName,
                    Position = x.Position != null ? x.Position.NameForDropdown : string.Empty,
                    Node = x.Node != null ? x.Node.Name : string.Empty
                });

            var queryable = employees.AsQueryable();
            CrudController.UpdateFilter(filter, typeof(EmpViewModel));
            var dataSourse = DataSourceResult.GetDataSourceResult(queryable, typeof(EmpViewModel), pageSize, skip, serverPaging, sort, filter);
            return Json(new { Data = dataSourse.Data, TotalCount = dataSourse.Total });
        }

        [HttpPost]
        public ActionResult GetSelectedTraineeGridModel(int id)
        {
            var gridModel = GridViewModelFactory.Create(typeof(EmpViewModel), null);
            gridModel.Views[0].ReadUrl = "Training/ActionList/ReadSelectedTraineeData/" + id;
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    Name = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.RemoveSelected),//"RemoveButton",
                    ClassName = "k-button k-button-icontext k-grid-add RemoveTraineeButton",
                    Handler = "RemoveButton",
                    ImageClass = "k-icon k-delete",
                    Text = ServiceFactory.LocalizationService.GetResource(TrainingLocalizationHelper
                        .GetResource(TrainingLocalizationHelper.RemoveSelected))
                }
            };
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadSelectedTraineeData(int id, int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var course = (Course)typeof(Course).GetById(id);

            var employees = course.CourseEmployees.Where(x => x.Type == CourseEmployeeType.Trainee).Select(x =>
                new EmpViewModel()
                {
                    Id = x.Employee.Id,
                    Name = x.Employee.FullName,
                    Position = x.Position != null ? x.Position.NameForDropdown : string.Empty,
                    Node = x.Node != null ? x.Node.Name : string.Empty
                });

            var queryable = employees.AsQueryable();
            CrudController.UpdateFilter(filter, typeof(EmpViewModel));
            var dataSourse = DataSourceResult.GetDataSourceResult(queryable, typeof(EmpViewModel), pageSize, skip, serverPaging, sort, filter);
            return Json(new { Data = dataSourse.Data, TotalCount = dataSourse.Total });
        }

        public ActionResult SaveSelectedTrainee(List<IDictionary<string, object>> model, int courseId)
        {
            var course = (Course)typeof(Course).GetById(courseId);
            var start = DateTime.Now;
            if (model == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in model)
            {
                var employee = (Employee)typeof(Employee).GetById((int)item["Id"]);
                if (employee != null)
                {
                    var courseEmployee = course.CourseEmployees.FirstOrDefault(x => x.Employee.Id == employee.Id);
                    if (courseEmployee != null)
                    {
                        courseEmployee.Type = CourseEmployeeType.Trainee;
                    }
                }

            }
            
          //  ServiceFactory.ORMService.Save(course, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(course);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AddeSelectedTrainee);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() {  });

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUnSelectedTrainee(List<IDictionary<string, object>> model, int courseId)
        {
            var course = (Course)typeof(Course).GetById(courseId);
            var start = DateTime.Now;
            if (model == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            foreach (var item in model)
            {
                var employee = (Employee)typeof(Employee).GetById((int)item["Id"]);
                if (employee != null)
                {
                    var courseEmployee = course.CourseEmployees.FirstOrDefault(x => x.Employee.Id == employee.Id);
                    if (courseEmployee != null)
                    {
                        courseEmployee.Type = CourseEmployeeType.Candidate;
                    }
                }
            }
            //ServiceFactory.ORMService.Save(course, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(course);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AddeUnSelectedTrainee);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() {  });

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Cancel Training Course

        public ActionResult CancelTrainingCourse(string description, int id)
        {
            var course = (Course)typeof(Course).GetById(id);
            var start = DateTime.Now;
            if (course == null)
                return Json(new {Success = false, Message = GlobalResource.ExceptionMessage},
                    JsonRequestBehavior.AllowGet);

            course.Status = CourseStatus.Cancelled;
            course.CancellationDescription = description;
          //  ServiceFactory.ORMService.Save(course, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(course);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CancelCourse);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() {  });


            var trainingNeeds = course.CourseTrainingNeeds.Select(x => x.TrainingNeed).ToList();
            foreach (var trainingNeed in trainingNeeds)
            {
                trainingNeed.Status = TrainingNeedStatus.Canceled;
            }
          
            var info2 = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CancelTrainingNeed);

            ServiceFactory.ORMService.SaveTransaction(trainingNeeds, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info2, start, new List<Entity>() { });


           // ServiceFactory.ORMService.SaveTransaction(trainingNeeds, UserExtensions.CurrentUser);

            return Json(new { Success = true, Message = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.TheCourseHasBeenCanceledSuccessfully) },
                JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Close training Course
        public ActionResult CloseTrainingCourse(int courseId)
        {
            var course = (Course)typeof(Course).GetById(courseId);
            var start = DateTime.Now;

            if (course == null)
                return Json(new { Success = false, Message = GlobalResource.ExceptionMessage },
                    JsonRequestBehavior.AllowGet);

            course.Status = CourseStatus.Closed;
           // ServiceFactory.ORMService.Save(course, UserExtensions.CurrentUser);
            var entities = new List<IAggregateRoot>();
            entities.Add(course);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CloseCourse);

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() {  });


            var trainingNeeds = course.CourseTrainingNeeds.Select(x => x.TrainingNeed).ToList();
            foreach (var trainingNeed in trainingNeeds)
            {
                trainingNeed.Status = TrainingNeedStatus.Closed;
            }

          //  ServiceFactory.ORMService.SaveTransaction(trainingNeeds, UserExtensions.CurrentUser);
            var info2 = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CloseTrainingNeed);

            ServiceFactory.ORMService.SaveTransaction(trainingNeeds, UserExtensions.CurrentUser, course, Souccar.Domain.Audit.OperationType.Update, info2, start, new List<Entity>() { });

            return Json(new { Success = true, Message = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.TheCourseHasBeenClosedSuccessfully) },
                JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Helper Methods

        private Position GetEmployeePosition(Employee employee)
        {
            return employee.Positions.FirstOrDefault(x => x.IsPrimary) == null
                ? null
                :
                employee.Positions.FirstOrDefault(x => x.IsPrimary).Position != null
                    ?
                    employee.Positions.FirstOrDefault(x => x.IsPrimary).Position
                    : null;
        }

        #endregion

    }

    #region View Model Class

    public class TrainNeedViewModel : Entity
    {
        public string Name { get; set; }
        public string Level { get; set; }

        public bool IsChecked { get; set; }
    }

    public class EmpViewModel : Entity
    {
        public string Name { get; set; }
        public string Node { get; set; }
        public string Position { get; set; }
        public bool IsChecked { get; set; }
    }

    #endregion

}
