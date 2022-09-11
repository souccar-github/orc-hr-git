using System;
using System.Collections;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.OrganizationChart.Configurations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Utils.UI;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Training.Entities;
using HRIS.Domain.Training.Enums;
using HRIS.Domain.Training.RootEntities;
using Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Core;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.Training.Controllers
{
    public class DashboardController : Controller
    {

        public ActionResult TrainingDashboard()
        {
            return PartialView();
        }

        public ActionResult CoursesAttendanceRateChart()
        {
            return PartialView("Charts/CoursesAttendanceRateChart");
        }

        public ActionResult GetNodesTypes()
        {
            try
            {
                var nodesTypes = ServiceFactory.ORMService.All<NodeType>().Select(x => new { Name = x.Name, Id = x.Id }).ToList();

                return Json(nodesTypes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GlobalResource.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetNodes(List<Dictionary<string, object>> types)
        {
            if (types == null)
                return Json(JsonRequestBehavior.AllowGet);

            var nodesTypesIds = types.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

            var nodes = ServiceFactory.ORMService.All<Node>()
                .Where(x => nodesTypesIds.Contains(x.Type.Id)).Select(x => new { Name = x.Name, Id = x.Id }).ToList();

            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployees(List<Dictionary<string, object>> nodes)
        {
            if (nodes == null)
            {
                return Json(JsonRequestBehavior.AllowGet);
            }

            var nodesIds = nodes.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

            var positionsIds = ServiceFactory.ORMService.All<Position>()
                .Where(x => x.JobDescription != null && x.JobDescription.Node != null &&
                            nodesIds.Contains(x.JobDescription.Node.Id)).Select(x => x.Id).ToArray();

            var employeesIds = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>()
                .Where(x => positionsIds.Contains(x.Position.Id)).Select(x=>x.Employee).Select(x=>x.Id).ToList();

            var trainingEmployees = ServiceFactory.ORMService.All<Course>().SelectMany(x => x.CourseEmployees)
                .Where(x => employeesIds.Contains(x.Employee.Id) && x.Type == CourseEmployeeType.Trainee)
                .Select(x => x.Employee).ToList().Distinct();

            var employees = trainingEmployees.Select(x => new {Name = x.FullName, Id = x.Id});
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTrainingPlans()
        {
            try
            {
                var trainingPlans = ServiceFactory.ORMService.All<TrainingPlan>().Select(x => new { Name = x.PlanName, Id = x.Id }).ToList();

                return Json(trainingPlans, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GlobalResource.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetTrainingPlanCourses(List<Dictionary<string, object>> plans)
        {
            if (plans == null)
                return Json(JsonRequestBehavior.AllowGet);

            var plansIds = plans.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

            var courses = ServiceFactory.ORMService.All<Course>()
                .Where(x => plansIds.Contains(x.TrainingPlan.Id)).Select(x => new {Name = x.CourseName.Name, Id = x.CourseName.Id}).ToList()
                .Distinct();

            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCoursesByPlanAndStatus(List<Dictionary<string, object>> plans, List<Dictionary<string, object>> status)
        {
            if (plans == null || status == null)
                return Json(JsonRequestBehavior.AllowGet);

            var plansIds = plans.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();
            var listCourseStatus = status
                .SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (CourseStatus) ((int) y.Value - 1))).ToArray();

            var courses = ServiceFactory.ORMService.All<Course>()
                .Where(x => plansIds.Contains(x.TrainingPlan.Id) && listCourseStatus.Contains(x.Status))
                .Select(x => new {Name = x.CourseName.Name, Id = x.CourseName.Id}).ToList()
                .Distinct();

            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CoursesAttendanceRate(List<Dictionary<string, object>> trainingPlans,
            List<Dictionary<string, object>> courseNames)
        {
            bool success = false;
            string message = string.Empty;

            try
            {
                if (courseNames == null)
                    return Json(JsonRequestBehavior.AllowGet);

                var plansIds = trainingPlans.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();
                var courseNameIds = courseNames.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

                var courses = ServiceFactory.ORMService.All<Course>().Where(x =>
                    plansIds.Contains(x.TrainingPlan.Id) && courseNameIds.Contains(x.CourseName.Id));
                if (courses.Any())
                {
                    var numberOfTrainees = courses.SelectMany(x => x.CourseEmployees)
                        .Count(x => x.Type == CourseEmployeeType.Trainee);

                    var appraisalTrainees = courses.SelectMany(x => x.AppraisalTrainees).ToList();
                    var traineesAttendedTheCourse = appraisalTrainees.Count(x => x.TraineeAttendedTheCourse == true);

                    var traineesNotAttendedTheCourse = appraisalTrainees.Count(x => x.TraineeAttendedTheCourse == false);

                    var traineesAttendedTheCourseRate =
                        Math.Round(((double)traineesAttendedTheCourse * 100) / (double)numberOfTrainees, 2);

                    var traineesNotAttendedTheCourseRate = Math.Round(100 - traineesAttendedTheCourseRate, 2);

                    //var traineesAttendedTheCourseRate =
                    //    Math.Round(((double)traineesAttendedTheCourse * 100) / (double) (traineesAttendedTheCourse+ traineesNotAttendedTheCourse), 2);

                    //var traineesNotAttendedTheCourseRate =
                    //    Math.Round(((double)traineesNotAttendedTheCourse * 100) / (double)(traineesAttendedTheCourse + traineesNotAttendedTheCourse), 2);

                    return Json(
                        new
                        {
                            Success = true, NumberOfTrainees = numberOfTrainees,
                            TraineesAttendedTheCourse = traineesAttendedTheCourse,
                            TraineesNotAttendedTheCourse = traineesNotAttendedTheCourse,
                            TraineesAttendedTheCourseRate = traineesAttendedTheCourseRate,
                            TraineesNotAttendedTheCourseRate = traineesNotAttendedTheCourseRate
                        }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(new { Success = success, Message = message}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeParticipationRateInTheCourses(List<Dictionary<string, object>> employees)
        {
            bool success = false;
            string message = string.Empty;
            var numberOfEmployees = new List<int>();
            var employeesNames = new List<string>();
            try
            {
                if (employees != null && employees.Any())
                {
                    var employeesIds = employees.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

                    var course = ServiceFactory.ORMService.All<Course>().Where(x =>
                        x.CourseEmployees.Any(y =>
                            employeesIds.Contains(y.Employee.Id) == true && y.Type == CourseEmployeeType.Trainee));

                    foreach (var dicEmployee in employees)
                    {
                        var name = (string)dicEmployee["Name"];
                        var id = (int)dicEmployee["Id"];
                        var employeeCoursesCount = course.Count(x => x.CourseEmployees.Any(y => y.Employee.Id == id && y.Type == CourseEmployeeType.Trainee));

                        if (employeeCoursesCount > 0)
                        {
                            employeesNames.Add(name);
                            numberOfEmployees.Add(employeeCoursesCount);
                        }
                    }
                }

                success = true;
            }
            catch (Exception ex)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(
                new
                {
                    EmployeesNames = employeesNames.ToArray(),
                    NumberOfEmployees = numberOfEmployees.ToArray(),
                    Success = success,
                    Message = message
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrainingNeedsPercentage(DateTime? fromDate, DateTime? toDate)
        {
            bool success = false;
            string message = string.Empty;
            double appraisal = 0.0;
            double probation = 0.0;
            double manualEntry = 0.0;

            try
            {
                if (fromDate == null || toDate == null)
                    return Json(JsonRequestBehavior.AllowGet);

                var trainingNeeds = ServiceFactory.ORMService.All<TrainingNeed>().Where(x =>
                    x.CreationDate.Date >= fromDate.Value.Date && x.CreationDate.Date <= toDate.Value.Date).ToList();
                
                if (trainingNeeds.Any())
                {
                    var totalTrainingNeeds = trainingNeeds.Count();

                    var appraisalCount = trainingNeeds.Count(x => x.Source == TrainingNeedSource.Appraisal);
                    var probationCount = trainingNeeds.Count(x => x.Source == TrainingNeedSource.Probation);
                    var manualEntryCount = trainingNeeds.Count(x => x.Source == TrainingNeedSource.ManualEntry);

                    appraisal = Math.Round(((double) appraisalCount / (double) totalTrainingNeeds) * 100, 2);
                    probation = Math.Round(((double) probationCount / (double) totalTrainingNeeds) * 100, 2);
                    manualEntry = Math.Round(((double) manualEntryCount / (double) totalTrainingNeeds) * 100, 2);
                }

                success = true;
            }
            catch (Exception ex)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(
                new
                {
                    Success = success,
                    Message = message,
                    Appraisal = appraisal,
                    Probation = probation,
                    ManualEntry = manualEntry
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NumberOfTraineesAndCandidates(List<Dictionary<string, object>> courseNames)
        {
            bool success = false;
            string message = string.Empty;
            var numberOfTrainees = new List<int>();
            var numberOfCandidates = new List<int>();
            var listOfCoursesNames = new List<string>();
            try
            {
                if (courseNames != null)
                {
                    var courseNamesIds = courseNames.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

                    var courses = ServiceFactory.ORMService.All<Course>().Where(x => courseNamesIds.Contains(x.CourseName.Id));
                    foreach (var courseName in courseNames)
                    {
                        var id = (int) courseName["Id"];
                        var name = (string) courseName["Name"];
                        var coursesEmployees = courses.Where(x => x.CourseName.Id == id).SelectMany(x => x.CourseEmployees);

                        listOfCoursesNames.Add(name);
                        numberOfCandidates.Add(coursesEmployees.Count());
                        numberOfTrainees.Add(coursesEmployees.Count(x => x.Type == CourseEmployeeType.Trainee));
                    }
                }

                success = true;
            }
            catch (Exception ex)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(
                new
                {
                    Success = success,
                    Message = message,
                    NumberOfCandidates = numberOfCandidates.ToArray(),
                    NumberOfTrainees = numberOfTrainees.ToArray(),
                    CoursesNames= listOfCoursesNames.ToArray()
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NumberOfCoursesPerTraineesNodes(List<Dictionary<string, object>> nodes,
            List<Dictionary<string, object>> status)
        {
            bool success = false;
            string message = string.Empty;
            var numberOfCourses = new List<int>();
            var coursesNames = new List<string>();
            try
            {
                if (nodes != null && status != null)
                {
                    var nodesIds = nodes.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();
                    var listCourseStatus = status
                        .SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (CourseStatus)((int)y.Value - 1)))
                        .ToArray();

                    var positions = ServiceFactory.ORMService.All<Position>()
                        .Where(x => x.JobDescription != null && x.JobDescription.Node != null &&
                                    nodesIds.Contains(x.JobDescription.Node.Id));
                    var positionsIds = positions.Select(x => x.Id).ToArray();

                    var assigningEmployeeToPositions = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>()
                        .Where(x => positionsIds.Contains(x.Position.Id)).Distinct().ToList();

                    if (assigningEmployeeToPositions.Any())
                    {
                        foreach (var node in nodes)
                        {
                            var nodeId = (int)node["Id"];
                            var nodeName = (string)node["Name"];
                            var nodePositionsIds = positions.Where(x =>
                                x.JobDescription != null && x.JobDescription.Node != null &&
                                x.JobDescription.Node.Id == nodeId).Select(x => x.Id).ToArray();

                            var employeesIds = assigningEmployeeToPositions
                                .Where(x => nodePositionsIds.Contains(x.Position.Id)).Select(x => x.Employee)
                                .Select(x => x.Id).Distinct().ToArray();

                            if (employeesIds.Any())
                            {
                                var courses = ServiceFactory.ORMService.All<Course>().Where(x =>
                                    x.CourseEmployees.Any(y =>
                                        employeesIds.Contains(y.Employee.Id) == true &&
                                        listCourseStatus.Contains(x.Status)));

                                if (courses.Any())
                                {
                                    numberOfCourses.Add(courses.Count());
                                    coursesNames.Add(nodeName);
                                }
                            }
                        }

                        success = true;
                    }
                }

                success = true;
            }
            catch (Exception ex)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(
                new
                {
                    Success = success,
                    Message = message,
                    NumberOfCourses = numberOfCourses.ToArray(),
                    CoursesNames = coursesNames.ToArray()
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NumberOfTraineesDistributedByNode(List<Dictionary<string, object>> courseNames)
        {
            bool success = false;
            string message = string.Empty;
            var list = new ArrayList();
            var listOfCoursesNames = new List<string>();
            try
            {
                if (courseNames != null)
                {
                    var courseNamesIds = courseNames.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();
                    listOfCoursesNames = courseNames.SelectMany(x => x.Where(y => y.Key == "Name").Select(y => (string)y.Value)).ToList();

                    var courses = ServiceFactory.ORMService.All<Course>().Where(x => courseNamesIds.Contains(x.CourseName.Id));

                    var nodes = courses.SelectMany(x => x.CourseEmployees)
                        .SelectMany(x => x.Employee.Positions).Select(x => x.Position.JobDescription.Node)
                        .Distinct().ToList();

                    foreach (var node in nodes)
                    {
                        var numberOfTrainees = new int[courseNames.Count];
                        for (int i = 0; i < courseNames.Count; i++)
                        {
                            var id = (int)courseNames[i]["Id"];
                            var coursesEmployees =
                                courses.Where(x => x.CourseName.Id == id).SelectMany(x => x.CourseEmployees)
                                    .Where(x => x.Type == CourseEmployeeType.Trainee);


                            var trainees = coursesEmployees.Select(x => x.Employee).Where(x =>
                                x.Positions.Any(y => y.Position.JobDescription.Node.Id == node.Id));

                            numberOfTrainees[i] = trainees.Count();
                        }

                        var name =
                            $"{TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.NumberOfTraineesFrom)} {node.Name}";
                        list.Add(new
                        {
                            name = name,
                            data = numberOfTrainees
                        });
                    }

                }

                success = true;
            }
            catch (Exception ex)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(
                new
                {
                    Success = success,
                    Message = message,
                    List = list,
                    CoursesNames = listOfCoursesNames.ToArray()
                }, JsonRequestBehavior.AllowGet);
        }
    }
}