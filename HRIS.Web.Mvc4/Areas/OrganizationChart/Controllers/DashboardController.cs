using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Personnel.Enums;
using System.Collections;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult OrgChartDashboard()
        {
            return PartialView();
        }

        public ActionResult OrgChartDashboardTree()
        {
            return PartialView();
        }

        public ActionResult GetOrgCharts()
        {
            var data = new ArrayList();
            var nodes = ServiceFactory.ORMService.All<Node>().ToList();
            foreach (var node in nodes)
            {
                int employeeCount = GetEmployeesCount(nodes, node.Id); ;
                

                data.Add(new
                {
                    name = node.Name,
                    id = node.Id.ToString(),
                    parent = node.Parent != null ? node.Parent.Id.ToString() : string.Empty,
                    value = employeeCount
                });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public int GetEmployeesCount(IList<Node> nodes, int nodeId)
        {
            int employeeCount = 0;
            var jobDescriptionIds = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => x.Node.Id == nodeId).Select(x => x.Id).ToArray();

            var employees = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>()
                .Where(x => jobDescriptionIds.Contains(x.Position.JobDescription.Id))
                .Select(x => x.Employee).Where(x => x.EmployeeCard.CardStatus != EmployeeCardStatus.Resigned).ToList().Distinct();

            employeeCount += employees.Count();

            var childrenNodes = nodes.Where(x=> x.Parent!=null && x.Parent.Id==nodeId).ToList();
            if (childrenNodes.Any())
            {
                foreach (var node in childrenNodes)
                {
                    employeeCount += GetEmployeesCount(nodes, node.Id);
                }
                
            }

            return employeeCount;
        }

        public ActionResult GetOrgChartTreeData()
        {
            var positions = typeof(Position).GetAll<Position>().ToList();
            var employees = typeof(Employee).GetAll<Employee>().ToList();
            var employeeCards = typeof(EmployeeCard).GetAll<EmployeeCard>().ToList();
            var data = GetOrgTreeNodes(employees, positions, employeeCards, 0);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public string getImg(AssigningEmployeeToPosition assign)
        {
            Employee employee = null;
            if (assign != null)
                employee = assign.Employee;
            var themingName = UserExtensions.CurrentUserTheming;

            if (employee == null)
                return "../Content/images/theme-" + themingName + "/placeholder.jpg";

            if (string.IsNullOrEmpty(employee.PhotoId))
                return "../Content/images/theme-" + themingName + "/placeholder.jpg";
            return "../Content/EmployeesPhoto/" + employee.PhotoId;
        }
        public JsonResult GetOrgTreeNodes(List<Employee> employess, List<Position> positions, List<EmployeeCard> employeeCards, int? id)
        {

            var nodes = id == 0 ?
                        positions.Where(x => x.Manager == null) :
                        positions.Where(x => x.Manager != null && x.Manager.Id == id);
            foreach (var node in nodes)
            {

                Employee temp = null;
                if (node.AssigningEmployeeToPosition != null)
                    temp = employess.FirstOrDefault(x => x.Id == node.AssigningEmployeeToPosition.Employee.Id);

                if (temp != null)
                {
                    if (temp.EmployeeCard != null)
                    {
                        temp.EmployeeCard = employeeCards.FirstOrDefault(x => x.Id == temp.EmployeeCard.Id);
                        if (temp.EmployeeCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork)
                            node.AssigningEmployeeToPosition.Employee = temp;
                    }
                }

                //node.JobDescription.Node = nodesList.FirstOrDefault(x => x.Id == node.JobDescription.Node.Id);
                //node.JobDescription.JobTitle = (JobTitle)typeof(JobTitle).GetById(node.JobDescription.JobTitle.Id);
            }


            var data = nodes.
                Select(x => new
                {
                    Id = x.Id,
                    Name = x.AssigningEmployeeToPosition != null && x.AssigningEmployeeToPosition.Employee != null ?
                    x.AssigningEmployeeToPosition.Employee.FirstName + ' ' + x.AssigningEmployeeToPosition.Employee.LastName :
                    "Vacant",
                    Title = x.JobDescription.Name,
                    ImgURL = getImg(x.AssigningEmployeeToPosition),
                    Children = GetOrgTreeNodes(employess, positions, employeeCards, x.Id)
                }).ToArray();



            return Json(data);
        }

    }
}
