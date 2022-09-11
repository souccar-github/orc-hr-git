using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.OrganizationChart.Configurations;
using HRIS.Domain.Recruitment.Enums;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Web.Mvc4.Helpers.Resource;
using System.Collections;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Workflow;
using Souccar.Domain.Workflow.Enums;

namespace Project.Web.Mvc4.Areas.Recruitment.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Recruitment/Dashboard/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RecruitmentDashboard()
        {
            return PartialView();
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

        public ActionResult GetJobDescriptions(List<Dictionary<string, object>> nodes)
        {
            if (nodes == null)
            {
                return Json(JsonRequestBehavior.AllowGet);
            }

            var nodesIds = nodes.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

            var allNodes = new List<Node>();

            var parentNodes = ServiceFactory.ORMService.All<Node>().Where(x => nodesIds.Contains(x.Id));
            
            foreach (var parentNode in parentNodes)
            {
                allNodes.Add(parentNode);
                GetChildNodes(parentNode, allNodes);
            }

            var jobDescriptionsFromJobApplications = ServiceFactory.ORMService.All<JobApplication>()
                .Where(x => x.Position.JobDescription != null).Select(x => x.Position.JobDescription).ToList();

            var jobDescriptionsFromRecruitmentRequests = ServiceFactory.ORMService.All<RecruitmentRequest>()
                .Where(x => x.RequestedPosition.JobDescription != null).Select(x => x.RequestedPosition.JobDescription)
                .ToList();

            var jobDescriptions = jobDescriptionsFromJobApplications.Concat(jobDescriptionsFromRecruitmentRequests)
                .Where(x => allNodes.Select(y=>y.Id).Contains(x.Node.Id)).Select(x => new { Name = x.Name, Id = x.Id }).Distinct().ToList();

            return Json(jobDescriptions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEvaluators()
        {
            var workflowItems = ServiceFactory.ORMService.All<WorkflowItem>().Where(x => x.Type == WorkflowType.InterviewEvaluation);
            var workflowApprovals = workflowItems.Where(x => x.Type == WorkflowType.InterviewEvaluation).SelectMany(x => x.Approvals).ToList();
            var evaluators = workflowApprovals.Select(x => x.User).Select(x => new { Name = x.FullName, Id = x.Id }).Distinct().ToList();

            return Json(evaluators, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JobApplicationsPercentage(DateTime? fromDate, DateTime? toDate, List<Dictionary<string, object>> jobDescriptions)
        {
            try
            {
                int total = 0;
                int totalWithInterview = 0;
                int totalThatAccepted = 0;
                if (jobDescriptions != null)
                {
                    var jobDescriptionIds = jobDescriptions.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

                    var jobApplications = ServiceFactory.ORMService.All<JobApplication>()
                        .Where(x =>
                            x.ApplicationDate >= fromDate &&
                            x.ApplicationDate <= toDate &&
                            (jobDescriptionIds.Contains(x.Position.JobDescription.Id) || jobDescriptionIds.Contains(x.RecruitmentRequest.RequestedPosition.JobDescription.Id))
                        ).ToList();

                    total = jobApplications.Count();
                    totalWithInterview = jobApplications.Where(x => x.Interviews.Any()).Count();
                    totalThatAccepted = jobApplications.Where(x => x.ApplicationStatus == ApplicationStatus.Accepted).Count();
                }

                return Json(
                    new
                    {
                        Success = true,
                        Total = total,
                        TotalWithInterview = totalWithInterview,
                        TotalThatAccepted = totalThatAccepted
                    }
                    , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GlobalResource.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InterviewsStatusDuringSpecificPeriod(DateTime? fromDate, DateTime? toDate, List<Dictionary<string, object>> jobDescriptions)
        {
            try
            {
                var acceptedInterviews = new ArrayList();
                var rejectedInterviews = new ArrayList();
                var forFutureInterviews = new ArrayList();
                var months = GetMonthsBetweenTwoDates(fromDate.Value, toDate.Value);
                if (jobDescriptions != null)
                {
                    var jobDescriptionIds = jobDescriptions.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

                    var jobApplications = ServiceFactory.ORMService.All<JobApplication>()
                    .Where(x =>
                    x.ApplicationDate >= fromDate &&
                    x.ApplicationDate <= toDate &&
                    (jobDescriptionIds.Contains(x.Position.JobDescription.Id) || jobDescriptionIds.Contains(x.RecruitmentRequest.RequestedPosition.JobDescription.Id))
                    ).ToList();


                    foreach (var month in months)
                    {
                        var acceptedJobApplications = jobApplications.Where(x => x.ApplicationStatus == ApplicationStatus.Accepted && x.ApplicationDate.ToString("MMMM") == month).Count();
                        var rejectedJobApplications = jobApplications.Where(x => x.ApplicationStatus == ApplicationStatus.Rejected && x.ApplicationDate.ToString("MMMM") == month).Count();
                        var forFutureJobApplications = jobApplications.Where(x => x.ApplicationStatus == ApplicationStatus.ForFuture && x.ApplicationDate.ToString("MMMM") == month).Count();

                        acceptedInterviews.Add(acceptedJobApplications);
                        rejectedInterviews.Add(rejectedJobApplications);
                        forFutureInterviews.Add(forFutureJobApplications);
                    }
                }

                return Json(new
                {
                    Success = true,
                    AcceptedInterviews = acceptedInterviews,
                    RejectedInterviews = rejectedInterviews,
                    ForFutureInterviews = forFutureInterviews,
                    Months = months
                }
                        , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GlobalResource.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult RecruitmentRequests(DateTime? fromDate, DateTime? toDate, List<Dictionary<string, object>> jobDescriptions)
        {
            try
            {
                var acceptedRequests = new ArrayList();
                var rejectedRequests = new ArrayList();
                var finishedRequests = new ArrayList();
                var jobDescriptionsNames = new List<string>();

                if (jobDescriptions != null)
                {
                    var jobDescriptionIds = jobDescriptions.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

                    var recruitmentRequests = ServiceFactory.ORMService.All<RecruitmentRequest>()
                    .Where(x =>
                    x.RequestDate >= fromDate &&
                    x.RequestDate <= toDate &&
                    jobDescriptionIds.Contains(x.RequestedPosition.JobDescription.Id)
                    ).ToList();

                    jobDescriptionsNames = recruitmentRequests.Select(x => x.RequestedPosition.JobDescription.Name).Distinct().ToList();

                    foreach (var jobDescriptionId in recruitmentRequests.Select(x=>x.RequestedPosition.JobDescription.Id).Distinct())
                    {
                        var accepted = recruitmentRequests.Where(x => x.RequestStatus == RequestStatus.Accepted && x.RequestedPosition.JobDescription.Id == jobDescriptionId).ToList();
                        var rejected = recruitmentRequests.Where(x => x.RequestStatus == RequestStatus.Rejected && x.RequestedPosition.JobDescription.Id == jobDescriptionId).ToList();
                        var finished = recruitmentRequests.Where(x => x.RequestStatus == RequestStatus.Finished && x.RequestedPosition.JobDescription.Id == jobDescriptionId).ToList();

                        acceptedRequests.Add(accepted.Count);
                        rejectedRequests.Add(rejected.Count);
                        finishedRequests.Add(finished.Count);
                    }
                }

                return Json(new
                {
                    Success = true,
                    AcceptedRequests = acceptedRequests,
                    RejectedRequests = rejectedRequests,
                    FinishedRequests = finishedRequests,
                    JobDescriptions = jobDescriptionsNames
                }
                        , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = GlobalResource.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult InterviewMarks(DateTime? fromDate, DateTime? toDate, List<Dictionary<string, object>> jobDescriptions, List<Dictionary<string, object>> evaluators)
        {
            int numberOfInterviews = 0;
            int belowExpected = 0;
            int needTraining = 0;
            int expected = 0;
            int upExpected = 0;
            int outstanding = 0;

            if (jobDescriptions != null && evaluators != null)
            {
                var jobDescriptionIds = jobDescriptions.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();
                var evaluatorsIds = evaluators.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

                var interviews = ServiceFactory.ORMService.All<Interview>()
                .Where(x =>
                    x.JobApplication.ApplicationDate >= fromDate &&
                    x.JobApplication.ApplicationDate <= toDate &&
                    (jobDescriptionIds.Contains(x.JobApplication.RecruitmentRequest.RequestedPosition.Id) || jobDescriptionIds.Contains(x.JobApplication.Position.JobDescription.Id)) &&
                    x.Evaluators.Any(y => evaluatorsIds.Contains(y.User.Id))

                ).ToList();

                numberOfInterviews = interviews.Count;

                foreach (var interview in interviews)
                {
                    var appraisalSetting = interview.InterviewAppraisalSetting;
                    var mark = interview.FinalMark;

                    if (mark >= appraisalSetting.FromMarkBelowExpected && mark <= appraisalSetting.ToMarkBelowExpected)
                        belowExpected++;
                    else if (mark >= appraisalSetting.FromMarkNeedTraining && mark <= appraisalSetting.ToMarkNeedTraining)
                        needTraining++;
                    else if (mark >= appraisalSetting.FromMarkExpected && mark <= appraisalSetting.ToMarkExpected)
                        expected++;
                    else if (mark >= appraisalSetting.FromMarkUpExpected && mark <= appraisalSetting.ToMarkUpExpected)
                        upExpected++;
                    else if (mark >= appraisalSetting.FromMarkDistinct)
                        outstanding++;
                }

            }

            return Json(new
            {
                Success = true,
                BelowExpected = Math.Round((double)(belowExpected * 100) / numberOfInterviews, 2),
                NeedTraining = Math.Round((double)(needTraining * 100) / numberOfInterviews, 2),
                Expected = Math.Round((double)(expected * 100) / numberOfInterviews, 2),
                UpExpected = Math.Round((double)(upExpected * 100) / numberOfInterviews, 2),
                Outstanding = Math.Round((double)(outstanding * 100) / numberOfInterviews, 2)
            }
            , JsonRequestBehavior.AllowGet);
        }

        public ActionResult InterviewsEvaluators(DateTime? fromDate, DateTime? toDate, List<Dictionary<string, object>> nodes)
        {
            try
            {
                var acceptedInterviews = new ArrayList();
                var rejectedInterviews = new ArrayList();
                var forFutureInterviews = new ArrayList();
                var evaluatorsNames = new List<string>();

                if (nodes != null)
                {
                    var nodesIds = nodes.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();
                    var nodesWithChildrenIds = GetChildrenNodes(nodesIds).Select(x => x.Id).ToList();

                    var interviews = ServiceFactory.ORMService.All<Interview>()
                    .Where(x =>
                        x.JobApplication.ApplicationDate >= fromDate &&
                        x.JobApplication.ApplicationDate <= toDate &&
                        nodesWithChildrenIds.Contains(x.JobApplication.Position.JobDescription.Node.Id)
                    ).ToList();

                    var users = interviews.SelectMany(x => x.Evaluators)
                        .Select(x => new {Id = x.User.Id, FullName = x.User.FullName}).Distinct();

                    foreach (var user in users)
                    {
                        var acceptedCount = interviews.Where(x => x.JobApplication.ApplicationStatus == ApplicationStatus.Accepted && x.Evaluators.Any(y => y.User.Id == user.Id)).Count();
                        var rejectedCount = interviews.Where(x => x.JobApplication.ApplicationStatus == ApplicationStatus.Rejected && x.Evaluators.Any(y => y.User.Id == user.Id)).Count();
                        var forFutureCount = interviews.Where(x => x.JobApplication.ApplicationStatus == ApplicationStatus.ForFuture && x.Evaluators.Any(y => y.User.Id == user.Id)).Count();

                        acceptedInterviews.Add(acceptedCount);
                        rejectedInterviews.Add(rejectedCount);
                        forFutureInterviews.Add(forFutureCount);

                        evaluatorsNames.Add(user.FullName);
                    }
                }
                return Json(new
                {
                    Success = true,
                    AcceptedInterviews = acceptedInterviews,
                    RejectedInterviews = rejectedInterviews,
                    ForFutureInterviews = forFutureInterviews,
                    Evaluators = evaluatorsNames
                }
                            , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = GlobalResource.ExceptionMessage
                }, JsonRequestBehavior.AllowGet);
            }

        }


        #region Methods

        private static void GetChildNodes(Node parentNode, IList<Node> children)
        {
            foreach (var node in parentNode.Children)
            {
                children.Add(node);
                if (node.Children.Any())
                    GetChildNodes(node, children);
            }
        }

        private List<Node> GetChildrenNodes(int[] nodesIds)
        {
            var nodesWithChildren = new List<Node>();
            foreach (var nodeId in nodesIds)
            {
                var node = ServiceFactory.ORMService.All<Node>().FirstOrDefault(x => x.Id == nodeId);
                if (node != null)
                {
                    GetChildNodes(node, nodesWithChildren);
                }
            }

            return nodesWithChildren.Distinct().ToList();
        }

        public IList<string> GetMonthsBetweenTwoDates(DateTime startDate, DateTime endDate)
        {
            var end = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));

            return Enumerable.Range(0, Int32.MaxValue)
                                 .Select(e => startDate.AddMonths(e))
                                 .TakeWhile(e => e <= end)
                                 .Select(e => e.ToString("MMMM")).ToList();
        }


        #endregion

    }
}
