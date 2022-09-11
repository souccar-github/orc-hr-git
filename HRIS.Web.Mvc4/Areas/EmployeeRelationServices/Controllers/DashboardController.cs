using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Infrastructure.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Extenstions;
using Status = HRIS.Domain.Global.Enums.Status;
using HRIS.Domain.Personnel.Enums;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult EmployeeRelationServicesDashboard()
        {
            return PartialView();
        }

        public ActionResult AdministrativeLevelDashboardForEmployeeRelationsServices()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            if (UserExtensions.IsCurrentUserFullAuthorized("AdministrativeLevelDashboardForEmployeeRelationsServices"))
            {
                var employees = ServiceFactory.ORMService.All<Employee>().ToList()
                 .Select(x => new { Id = x.Id, Name = x.FullName })
                 .ToList();
                return Json(employees, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(UserExtensions.CurrentUser);
                var employees = ServiceFactory.ORMService.All<Employee>().ToList()
                    .Where(x => employeesCanViewThem.Any(y => y.Id == x.Id))
                    .Select(x => new { Id = x.Id, Name = x.FullName })
                    .ToList();
                return Json(employees, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public ActionResult GetAllNodes()
        {
            if (UserExtensions.IsCurrentUserFullAuthorized("AdministrativeLevelDashboardForEmployeeRelationsServices"))
            {
                var nodes = ServiceFactory.ORMService.All<HRIS.Domain.OrganizationChart.RootEntities.Node>()
                .ToList().Distinct().Select(x => new { Id = x.Id, Name = x.Name });

                return Json(nodes, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var currentEmployee = UserExtensions.CurrentUser.Employee();
                var node = EmployeeExtensions.GetSecondaryPositionElsePrimary(currentEmployee).JobDescription.Node;
                var listNodes = new List<HRIS.Domain.OrganizationChart.RootEntities.Node>();
                GetChildNodes(node, listNodes);
                var nodes = listNodes
                .ToList().Distinct().Select(x => new { Id = x.Id, Name = x.Name });

                return Json(nodes, JsonRequestBehavior.AllowGet);

            }
        }

        //[HttpGet]
        //public ActionResult GetAllEmployees()
        //{
        //    int pageSize = Convert.ToInt32(Request.Params.Get("pageSize"));
        //    int skip = Convert.ToInt32(Request.Params.Get("skip"));
        //    string search = Request.Params.Get("filter[filters][0][value]");
        //    var employees = ServiceFactory.ORMService.All<Employee>().ToList()
        //        .Select(x => new { Id = x.Id, Name = x.FullName })
        //        .ToList(); 

        //    var total = employees.Count();

        //    if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
        //    {
        //        var result = employees.Where(x => x.Name.ToString().ToLower().Contains(search.ToLower())).ToList();
        //        total = result.Count;
        //        return Json(new { total = total, data = result }, JsonRequestBehavior.AllowGet);

        //    }

        //    var data = employees.Skip(skip).Take(pageSize).ToList();
        //    return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult GetAllEmployeesMapper(int[] values)
        //{
        //    var indices = new List<int>();
        //    var employees = ServiceFactory.ORMService.All<Employee>().ToList()
        //        .Select(x => new { Id = x.Id, Name = x.FullName })
        //        .ToList(); 

        //    if (values != null && values.Any())
        //    {
        //        var index = 0;

        //        foreach (var employee in employees)
        //        {
        //            if (values.Contains(employee.Id))
        //            {
        //                indices.Add(index);
        //            }
        //            index += 1;
        //        }
        //    }
        //    return Json(indices, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult GetLeaveSettingsForList()
        {
            var currentEmployee = EmployeeExtensions.CurrentEmployee;
            if(currentEmployee != null)
            {
                var employeeCard = currentEmployee.EmployeeCard;
                if (employeeCard.LeaveTemplateMaster != null)
                {
                    var result = employeeCard.LeaveTemplateMaster.LeaveTemplateDetails
                        .Select(x => new { Name = x.LeaveSetting.AliasName, Id = x.LeaveSetting.Id }).ToList();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var position = employeeCard.Employee.PrimaryPosition();
                if (position != null)
                {
                    if (position.JobDescription.JobTitle.Grade.LeaveTemplateMaster != null)
                    {
                        var result = position.JobDescription.JobTitle.Grade.LeaveTemplateMaster.LeaveTemplateDetails
                            .Select(x => new { Name = x.LeaveSetting.AliasName, Id = x.LeaveSetting.Id }).ToList();
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLeaveRequestsForCurrentYear(int leaveSettingId)
        {
            try
            {
                var currentDate = DateTime.Now;
                var employee = EmployeeExtensions.CurrentEmployee;
                
                var leaveSetting = ServiceFactory.ORMService.All<LeaveSetting>()
                    .FirstOrDefault(x => x.Id == leaveSettingId);

                if (employee != null && leaveSetting != null)
                {
                    var hasMaxNum = false;
                    var maxNum = 0;
                    var balance = Math.Round(LeaveService.GetBalance(leaveSetting, employee, false, currentDate), 1);
                    var granted = Math.Round(LeaveService.GetGranted(employee, leaveSetting, currentDate.Year), 1);
                    //الاجازات المدورة
                    var recycledBalance = LeaveService.GetRecycledBalance(employee, leaveSetting, currentDate.Year - 1);
                    balance += recycledBalance;

                    var remain = Math.Round(balance - granted, 1);
                    if (leaveSetting.HasMaximumNumber)
                    {
                        hasMaxNum = true;
                        maxNum = leaveSetting.MaximumNumber;
                        granted = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.LeaveSetting == leaveSetting && x.EmployeeCard.Employee == employee).Count();
                        remain = Math.Round(maxNum - granted, 1);
                    }

                    if (leaveSetting.IsIndivisible)
                    {
                        hasMaxNum = true;
                        maxNum = (int)leaveSetting.Balance;
                        granted = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.LeaveSetting == leaveSetting && x.EmployeeCard.Employee == employee).Any()? maxNum : 0;
                        remain = Math.Round(maxNum - granted, 1);
                    }

                    return Json(new {Remain = remain, Granted = granted, Balance = balance,HasMaxNum = hasMaxNum, MaxNum = maxNum, Success = true},
                        JsonRequestBehavior.AllowGet);
                }


                return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetLeavesByMonth()
        {
            var message = string.Empty;
            try
            {
                var waitingApprovedLeaves = new int[12];
                var approvedLeaves = new int[12];

                var currentYear = DateTime.Now.Year;

                var employee = EmployeeExtensions.CurrentEmployee;
                if (employee?.EmployeeCard != null)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        var date = new DateTime(currentYear, i, 1);
                        var leaves = LeaveService.GetLeaveRequestsInMonth(employee.EmployeeCard, date);

                        waitingApprovedLeaves[i - 1] = leaves.Count(x => x.LeaveStatus == Status.Draft);
                        approvedLeaves[i - 1] = leaves.Count(x => x.LeaveStatus == Status.Approved);
                    }

                    return Json(
                        new
                        {
                            WaitingApprovedLeaves = waitingApprovedLeaves, ApprovedLeaves = approvedLeaves,
                            Success = true
                        }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(new {Success = false, Message = message}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeLifeCycle(int employeeId)
        {
            var list = new List<EmployeeLifeCycle>();
            var employeeCard = ServiceFactory.ORMService.All<EmployeeCard>()
                .FirstOrDefault(x => x.Employee.Id == employeeId);
            if (employeeCard != null)
            {
                //Assignments
                var assignmentResourceValue = typeof(Assignment).GetTitle();
                var assignments = employeeCard.Assignments.Select(x => new EmployeeLifeCycle
                { Name = assignmentResourceValue, Date = x.AssigningDate, Description = x.Comment}).ToList();

                list.AddRange(assignments);

                //Employee Transfers
                var transfersResourceValue = typeof(EmployeeTransfer).GetTitle();
                var transfers = employeeCard.EmployeeTransfers.Select(x => new EmployeeLifeCycle
                { Name = transfersResourceValue, Date = x.LeavingDate, Description = x.Comment}).ToList();

                list.AddRange(transfers);

                //Employee Promotions
                var promotionResourceValue = typeof(EmployeePromotion).GetTitle();
                var promotions = employeeCard.EmployeePromotions.Select(x => new EmployeeLifeCycle
                { Name = promotionResourceValue, Date = x.PositionJoiningDate, Description = x.Comment}).ToList();

                list.AddRange(promotions);

                //Financial Promotions
                var financialPromotionResourceValue = typeof(FinancialPromotion).GetTitle();
                var financialPromotions = employeeCard.FinancialPromotions.Select(x => new EmployeeLifeCycle
                { Name = financialPromotionResourceValue, Date = x.CreationDate, Description = x.Comment}).ToList();

                list.AddRange(financialPromotions);

                //Employee Rewards
                var rewardsResourceValue = typeof(EmployeeReward).GetTitle();
                var rewards = employeeCard.EmployeeRewards.Select(x => new EmployeeLifeCycle
                { Name = rewardsResourceValue, Date = x.RewardDate, Description = x.Comment}).ToList();

                list.AddRange(rewards);

                //Employee Terminations
                var terminationsResourceValue = typeof(EmployeeTermination).GetTitle();
                var terminations = employeeCard.EmployeeTerminations.Select(x => new EmployeeLifeCycle
                { Name = terminationsResourceValue, Date = x.LastWorkingDate, Description = x.Comment}).ToList();

                list.AddRange(terminations);

                //Employee Resignations
                var resignationsResourceValue = typeof(EmployeeResignation).GetTitle();
                var resignations = employeeCard.EmployeeResignations.Select(x => new EmployeeLifeCycle
                { Name = resignationsResourceValue, Date = x.LastWorkingDate, Description = x.Comment}).ToList();

                list.AddRange(resignations);

                //Employee Disciplinary
                var disciplinaryValue = typeof(EmployeeDisciplinary).GetTitle();
                var disciplinary = employeeCard.EmployeeDisciplinarys.Select(x => new EmployeeLifeCycle
                { Name = disciplinaryValue, Date = x.DisciplinaryDate, Description = x.Comment }).ToList();

                list.AddRange(disciplinary);


                return Json(new {Data = list.OrderBy(x=>x.Date), Success = true}, JsonRequestBehavior.AllowGet);

            }

            return Json(new {Success = false}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeLeavesForYear(int employeeId,int year)
        {
            var message = string.Empty;
            var list = new ArrayList();
            try
            {
                var employee = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == employeeId);
                if (employee?.EmployeeCard != null)
                {
                    
                    var leaveSettings = employee.EmployeeCard.LeaveTemplateMaster.LeaveTemplateDetails
                        .Select(x => x.LeaveSetting).ToList();

                    foreach (var leaveSetting in leaveSettings)
                    {
                        var data = new int[12];
                        var name = leaveSetting.Name;

                        for (int i = 1; i <= 12; i++)
                        {
                            var date = new DateTime(year, i, 1);
                            var leaves = LeaveService.GetLeaveRequestsInMonth(employee.EmployeeCard, leaveSetting, date);

                            var leavesCountForSpecificType = leaves.Count(x => x.LeaveStatus == Status.Approved);

                            data[i - 1] = leavesCountForSpecificType;
                        }

                        if (data.Any(x => x != 0))//النحقق إذا كان شهر واحد على الاقل فيه اجازة
                        {
                            list.Add(new { data = data, name = name });
                        }
                    }


                    return Json(new { Success = true, Data = list }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(new { Success = false, Message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrentEmployeeLeavesForYear(int year)
        {
            var message = string.Empty;
            var list = new ArrayList();
            try
            {
                var employee = EmployeeExtensions.CurrentEmployee;

                if (employee?.EmployeeCard != null)
                {

                    var leaveSettings = employee.EmployeeCard.LeaveTemplateMaster.LeaveTemplateDetails
                        .Select(x => x.LeaveSetting).ToList();

                    foreach (var leaveSetting in leaveSettings)
                    {
                        var data = new int[12];
                        var name = leaveSetting.Name;

                        for (int i = 1; i <= 12; i++)
                        {
                            var date = new DateTime(year, i, 1);
                            var leaves = LeaveService.GetLeaveRequestsInMonth(employee.EmployeeCard, leaveSetting, date);

                            var leavesCountForSpecificType = leaves.Count(x => x.LeaveStatus == Status.Approved);

                            data[i - 1] = leavesCountForSpecificType;
                        }

                        if (data.Any(x => x != 0))//النحقق إذا كان شهر واحد على الاقل فيه اجازة
                        {
                            list.Add(new { data = data, name = name });
                        }
                    }


                    return Json(new { Success = true, Data = list }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(new { Success = false, Message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNodeLeavesForYear(int nodeId,int year)
        {
            var message = string.Empty;
            var list = new List<Dictionary<string, int[]>>();
            try
            {
                var listNodes = new List<HRIS.Domain.OrganizationChart.RootEntities.Node>();

                var node = ServiceFactory.ORMService.All<HRIS.Domain.OrganizationChart.RootEntities.Node>()
                    .FirstOrDefault(x => x.Id == nodeId);

                GetChildNodes(node, listNodes);
                listNodes.Add(node);

                var employees = GetNodeEmployees(listNodes);

                foreach (var employee in employees)
                {
                    var dic = new Dictionary<string, int[]>();

                    if (employee?.EmployeeCard != null)
                    {

                        var leaveSettings = employee.EmployeeCard.LeaveTemplateMaster.LeaveTemplateDetails
                            .Select(x => x.LeaveSetting).ToList();

                        foreach(var leaveSetting in leaveSettings)
                        {
                            var data = new int[12];
                            var name = leaveSetting.Name;

                            for (int i = 1; i <= 12; i++)
                            {
                                var date = new DateTime(year, i, 1);
                                var leaves = LeaveService.GetLeaveRequestsInMonth(employee.EmployeeCard, leaveSetting, date);

                                var leavesCountForSpecificType = leaves.Count(x => x.LeaveStatus == Status.Approved);

                                data[i - 1] = leavesCountForSpecificType;
                            }

                            if (data.Any(x => x != 0))//النحقق إذا كان شهر واحد على الاقل فيه اجازة
                            {
                                dic.Add(name, data);
                            }
                        }

                    }

                    list.Add(dic);
                }

                var result = ConcatList(list);

                return Json(new { Success = true, Data = result.ToList() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                message = GlobalResource.ExceptionMessage;
            }

            return Json(new { Success = false, Message = message }, JsonRequestBehavior.AllowGet);
        }

        public void GetChildNodes(HRIS.Domain.OrganizationChart.RootEntities.Node parentNode, IList<HRIS.Domain.OrganizationChart.RootEntities.Node> children)
        {
            foreach (var node in parentNode.Children)
            {
                children.Add(node);
                if (node.Children.Any())
                    GetChildNodes(node, children);
            }
        }

        public IEnumerable<Employee> GetNodeEmployees(IList<HRIS.Domain.OrganizationChart.RootEntities.Node> nodes)
        {
            var nodesIds = nodes.Select(x=>x.Id).ToArray();
            var jobDescriptionIds = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => nodesIds.Contains(x.Node.Id)).Select(x => x.Id).ToArray();

            var employees = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>()
                .Where(x => jobDescriptionIds.Contains(x.Position.JobDescription.Id) && x.IsPrimary == true)
                .Select(x => x.Employee).Where(x => x.EmployeeCard.CardStatus != EmployeeCardStatus.Resigned).ToList().Distinct();

            return employees;
        }

        public Dictionary<string, int[]> ConcatList(List<Dictionary<string, int[]>> list)
        {
            var leavesNames = list.SelectMany(d => d.Keys).ToList().Distinct();
            var result = new Dictionary<string, int[]>();

            foreach (var leaveName in leavesNames)
            {
                var values = list.SelectMany(x => x).Where(x => x.Key == leaveName).Select(v => v.Value);

                var data = new int[12];
                foreach (var value in values)
                {
                    for (int i = 0; i <= 11; i++)
                    {
                        data[i] += value[i];
                    }
                }

                result.Add(leaveName, data);
            }

            return result;
        }
         
    }

    public class EmployeeLifeCycle
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

}
