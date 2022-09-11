using Souccar.Infrastructure.Core;
using System.Collections.Generic;
using HRIS.Domain.OrganizationChart.RootEntities;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Personnel.Enums;
using System;
using System.Collections;

namespace Project.Web.Mvc4.Areas.Personnel.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PersonnelDashboard()
        {
            return PartialView();
        }

        public ActionResult GetParentNodes()
        {
            var nodes = ServiceFactory.ORMService.All<Node>()
                .Where(x => x.Children.Count > 0).ToList().Distinct().Select(x => new { Id = x.Id, Name = x.Name });

            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChildrenNodes(List<Dictionary<string, object>> parentNodes)
        {
            if(parentNodes==null)
                return Json(JsonRequestBehavior.AllowGet);

            var nodesIds = parentNodes.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

            var children = new List<Node>();

            var nodes = ServiceFactory.ORMService.All<Node>().Where(x => nodesIds.Contains(x.Id));
            foreach (var node in nodes)
            {
                children.AddRange(node.Children);
                //GetChildNodes(node, children);
            }

            var results = children.Distinct().Select(x => new { Id = x.Id, Name = x.Name });

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMalesAndFemalesPercentageData(List<Dictionary<string, object>> parentsNodes,List<Dictionary<string, object>> childrenNodes)
        {
            double percentageOfMaleEmployees = 0.0;
            double percentageOfFemaleEmployees = 0.0;
            int numberOfMales = 0;
            int numberOfFemales = 0;

            

            var employees = GetEmployees(parentsNodes, childrenNodes);

            if (employees.Any())
            {
                numberOfMales = employees.Where(x => x.Gender == Gender.Male).Count();
                numberOfFemales = employees.Where(x => x.Gender == Gender.Female).Count();
                var totalEmployeesCount = employees.Count();

                percentageOfMaleEmployees = Math.Round((double)(numberOfMales * 100) / totalEmployeesCount, 1);
                percentageOfFemaleEmployees = Math.Round((double)(numberOfFemales * 100) / totalEmployeesCount, 1);
            }

            return Json(
                new {
                    PercentageOfMaleEmployees = percentageOfMaleEmployees,
                    PercentageOfFemaleEmployees = percentageOfFemaleEmployees,
                    NumberOfMales = numberOfMales, NumberOfFemales= numberOfFemales
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBloodGroupPercentageData()
        {
            var employees = ServiceFactory.ORMService.All<Employee>().Where(x => x.EmployeeCard.CardStatus != EmployeeCardStatus.Resigned);

            var total = employees.Count();

            var a_PlusCount = employees.Where(x => x.BloodType == BloodType.A_Plus).Count();
            var a_NegativeCount = employees.Where(x => x.BloodType == BloodType.A_Negative).Count();
            var ab_PlusCount = employees.Where(x => x.BloodType == BloodType.AB_Plus).Count();
            var ab_NegativeCount = employees.Where(x => x.BloodType == BloodType.AB_Negative).Count();
            var b_PlusCount = employees.Where(x => x.BloodType == BloodType.B_Plus).Count();
            var b_NegativeCount = employees.Where(x => x.BloodType == BloodType.B_Negative).Count();
            var o_PlusCount = employees.Where(x => x.BloodType == BloodType.O_Plus).Count();
            var o_NegativeCount = employees.Where(x => x.BloodType == BloodType.O_Negative).Count();
            var nothingCount = employees.Where(x => x.BloodType == BloodType.Nothing).Count();

            var BloodGroup = new
            {
                A_Plus = Math.Round((double)(a_PlusCount * 100) / total, 1),
                A_Negative = Math.Round((double)(a_NegativeCount * 100) / total, 1),
                AB_Plus = Math.Round((double)(ab_PlusCount * 100) / total, 1),
                AB_Negative = Math.Round((double)(ab_NegativeCount * 100) / total, 1),
                B_Plus = Math.Round((double)(b_PlusCount * 100) / total, 1),
                B_Negative = Math.Round((double)(b_NegativeCount * 100) / total, 1),
                O_Plus = Math.Round((double)(o_PlusCount * 100) / total, 1),
                O_Negative = Math.Round((double)(o_NegativeCount * 100) / total, 1),
                Nothing = Math.Round((double)(nothingCount * 100) / total, 1),
                A_PlusCount= a_PlusCount,
                A_NegativeCount= a_NegativeCount,
                AB_PlusCount= ab_PlusCount,
                AB_NegativeCount= ab_NegativeCount,
                B_PlusCount= b_PlusCount,
                B_NegativeCount= b_NegativeCount,
                O_PlusCount= o_PlusCount,
                O_NegativeCount= o_NegativeCount,
                NothingCount= nothingCount
            };

            return Json(BloodGroup, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetReligionsPercentageData(List<Dictionary<string, object>> parentsNodes, List<Dictionary<string, object>> childrenNodes)
        {
            int numberOfMuslims = 0;
            int numberOfChristians = 0;
            int numberOfJewishs = 0;
            int numberOfOthers = 0;
            int totalEmployees = 0;
            var employees = new List<Employee>();

            if (parentsNodes != null)
                employees = GetEmployees(parentsNodes, childrenNodes).ToList();

            if (employees.Any())
            {
                numberOfMuslims = employees.Where(x => x.Religion == Religion.Muslim).Count();
                numberOfChristians = employees.Where(x => x.Religion == Religion.Christian).Count();
                numberOfJewishs = employees.Where(x => x.Religion == Religion.Jewish).Count();
                numberOfOthers = employees.Where(x => x.Religion == Religion.Other).Count();

                totalEmployees = employees.Count();
            }

            return Json(new
            {
                NumberOfMuslims = numberOfMuslims,
                NumberOfChristians = numberOfChristians,
                NumberOfJewishs = numberOfJewishs,
                NumberOfOthers = numberOfOthers,
                MuslimsPercentage = totalEmployees > 0 ? Math.Round((double)(numberOfMuslims * 100) / totalEmployees, 1) : 0.0,
                ChristiansPercentage = totalEmployees > 0 ? Math.Round((double)(numberOfChristians * 100) / totalEmployees, 1) : 0.0,
                JewishsPercentage = totalEmployees > 0 ? Math.Round((double)(numberOfJewishs * 100) / totalEmployees, 1) : 0.0,
                OthersPercentage = totalEmployees > 0 ? Math.Round((double)(numberOfOthers * 100) / totalEmployees, 1) : 0.0,

            },JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSocialStatusPercentageData(List<Dictionary<string, object>> parentsNodes, List<Dictionary<string, object>> childrenNodes)
        {
            int numberOfSingles = 0;
            int numberOfMarrieds = 0;
            int numberOfDivorceds = 0;
            int numberOfWidows = 0;
            int numberOfEngageds = 0;
            int total = 0;

            var employees = GetEmployees(parentsNodes, childrenNodes);
            if (employees.Any())
            {
                numberOfSingles = employees.Where(x => x.MaritalStatus == MaritalStatus.Single).Count();
                numberOfMarrieds = employees.Where(x => x.MaritalStatus == MaritalStatus.Married).Count();
                numberOfDivorceds = employees.Where(x => x.MaritalStatus == MaritalStatus.Divorced).Count();
                numberOfWidows = employees.Where(x => x.MaritalStatus == MaritalStatus.Widow).Count();
                numberOfEngageds = employees.Where(x => x.MaritalStatus == MaritalStatus.Engaged).Count();

                total = employees.Count();
            }

            var socialStatus = new
            {
                NumberOfSingles = numberOfSingles,
                NumberOfMarrieds = numberOfMarrieds,
                NumberOfDivorceds = numberOfDivorceds,
                NumberOfWidows = numberOfWidows,
                NumberOfEngageds = numberOfEngageds,
                SinglesPercentage = total > 0 ? Math.Round((double)(numberOfSingles * 100) / total, 1) : 0,
                MarriedsPercentage = total > 0 ? Math.Round((double)(numberOfMarrieds * 100) / total, 1) : 0,
                DivorcedsPercentage = total > 0 ? Math.Round((double)(numberOfDivorceds * 100) / total, 1) : 0,
                WidowsPercentage = total > 0 ? Math.Round((double)(numberOfWidows * 100) / total, 1) : 0,
                EngagedsPercentage = total > 0 ? Math.Round((double)(numberOfEngageds * 100) / total, 1) : 0
            };

            return Json(socialStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDisabilityPercentageData()
        {
            var employees = ServiceFactory.ORMService.All<Employee>().Where(x => x.EmployeeCard.CardStatus != EmployeeCardStatus.Resigned);

            var total = employees.Count();
            var disabilityCount = employees.Where(x => x.DisabilityExist).Count();
            var disability = new
            {
                NormalCount = total- disabilityCount,
                DisabilityCount = disabilityCount,
                NormalPercentage = total > 0 ? Math.Round((double)((total - disabilityCount) * 100) / total, 1) : 0,
                DisabilityPercentage = total > 0 ? Math.Round((double)(disabilityCount * 100) / total, 1) : 0
            };

            return Json(disability, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNationalityPercentageData(string type)
        {
            var nationalities = new
            {
                NationalityNames = new ArrayList(),
                NationalityPercentages = new ArrayList(),
                NumberOfEmployees = new ArrayList()
            };

            if(!string.IsNullOrEmpty(type))
            {
                var employees = new List<Employee>();
                if (type.Equals("BasicNationality"))
                {
                    employees = ServiceFactory.ORMService.All<Employee>().Where(x => x.EmployeeCard.CardStatus != EmployeeCardStatus.Resigned && x.Nationality != null).ToList();
                }
                else if (type.Equals("OtherNationality"))
                {
                    employees = ServiceFactory.ORMService.All<Employee>().Where(x => x.EmployeeCard.CardStatus != EmployeeCardStatus.Resigned && x.OtherNationalityExist && x.OtherNationality != null).ToList();
                }
                
                var totalEmployees = employees.Count();
                var nationalitiesNames = type.Equals("BasicNationality") ? employees.Select(x=>x.Nationality.Name).Distinct(): employees.Select(x => x.OtherNationality.Name).Distinct();
                foreach (var nationality in nationalitiesNames)
                {
                    var numberOfEmployees = type.Equals("BasicNationality") ? employees.Where(x => x.Nationality.Name.Equals(nationality)).Count()
                                                                            : employees.Where(x => x.OtherNationality.Name.Equals(nationality)).Count();

                    var employeesPercentage = Math.Round((double)(numberOfEmployees * 100) / totalEmployees, 2);

                    nationalities.NationalityNames.Add(nationality);
                    nationalities.NationalityPercentages.Add(employeesPercentage);
                    nationalities.NumberOfEmployees.Add(numberOfEmployees);
                }
            }
            
            return Json(nationalities, JsonRequestBehavior.AllowGet);
        }

        #region Helper method

        //public List<Dictionary<string, object>> GetMultiLevelChildrenNodes(List<Dictionary<string, object>> parentsNodes)
        //{
        //    var children = new List<Node>();

        //    var nodesIds = parentsNodes.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

        //    var nodes = ServiceFactory.ORMService.All<Node>().Where(x => nodesIds.Contains(x.Id));
        //    foreach (var node in nodes)
        //    {
        //        GetChildNodes(node, children);
        //    }


        //}
        public void GetChildNodes(Node parentNode, IList<Node> children)
        {
            foreach (var node in parentNode.Children)
            {
                children.Add(node);
                if (node.Children.Any())
                    GetChildNodes(node, children);
            }
        }

        public IEnumerable<Employee> GetEmployees(List<Dictionary<string, object>> parentsNodes, List<Dictionary<string, object>> childrenNodes)
        {
            IEnumerable<Employee> employees = new List<Employee>();

            if (childrenNodes != null && parentsNodes != null)
            {
                var employeesByChildrenNodes = GetEmployeesByChildNodes(childrenNodes).ToList();
                var employeesByParentsNodes = GetEmployeesByNodes(parentsNodes).ToList();
                employees = employeesByParentsNodes.Concat(employeesByChildrenNodes).ToList().Distinct();
            }
            else if ((parentsNodes == null || parentsNodes.Count == 0))
            {
                employees = GetEmployeesByChildNodes(childrenNodes);
            }

            else if (childrenNodes == null || childrenNodes.Count == 0)
            {
                employees = GetEmployeesByNodes(parentsNodes);
            }

            return employees;
        }

        public IEnumerable<Employee> GetEmployeesByNodes(IList<Dictionary<string, object>> nodes)
        {
            var nodesIds = nodes.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();
            var jobDescriptionIds = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => nodesIds.Contains(x.Node.Id)).Select(x => x.Id).ToArray();

            var employees = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>()
                .Where(x => jobDescriptionIds.Contains(x.Position.JobDescription.Id) && x.IsPrimary == true)
                .Select(x => x.Employee).Where(x => x.EmployeeCard.CardStatus != EmployeeCardStatus.Resigned).ToList().Distinct();

            return employees;
        }

        public IEnumerable<Employee> GetEmployeesByChildNodes(IList<Dictionary<string, object>> listOfDicNode)
        {
            var list = new List<Node>();
            var nodesIds = listOfDicNode.SelectMany(x => x.Where(y => y.Key == "Id").Select(y => (int)y.Value)).ToArray();

            var nodes = ServiceFactory.ORMService.All<Node>().Where(x => nodesIds.Contains(x.Id));
            foreach (var node in nodes)
            {
                var childNodes = new List<Node>();
                GetChildNodes(node, childNodes);

                list.AddRange(childNodes);
                list.Add(node);
            }

            var ids = list.Select(y => y.Id).ToArray();
            var jobDescriptionIds = ServiceFactory.ORMService
                .All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => ids.Contains(x.Node.Id)).Select(x => x.Id).ToArray();

            var employees = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>()
                .Where(x => jobDescriptionIds.Contains(x.Position.JobDescription.Id) && x.IsPrimary == true)
                .Select(x => x.Employee).Where(x => x.EmployeeCard.CardStatus != EmployeeCardStatus.Resigned).ToList().Distinct();

            return employees;
        }
        #endregion

    }
}
