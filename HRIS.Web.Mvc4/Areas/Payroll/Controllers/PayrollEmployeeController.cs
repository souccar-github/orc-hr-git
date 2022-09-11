using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Payroll.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.PersistenceSupport;
using Souccar.NHibernate;

namespace Project.Web.Mvc4.Areas.Payroll.Controllers
{
    public class PayrollEmployeeController : Controller
    {
        //
        // GET: /Payroll/PayrollEmployee/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BrowseEmployee()
        {
            return PartialView("_PartialParamBrowse");
        }

        [HttpPost]
        public ActionResult EmployeeFinanceCard()
        {
            return PartialView("_PartialEmpFinanceCard");
        }

        public ActionResult GetData()
        {
            var result = new List<KeyValuePair<int, string>>()
                             {
                                 new KeyValuePair<int, string>(1,"yaseem"),
                                 new KeyValuePair<int, string>(2,"Alaa")

                             }; 
            return Json(result.Select(x=>new {Id=x.Key,Name=x.Value}).ToList(),JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRecruitmentSystem()
        {
            var repository = new NHibernateRepository<PayrollTable>();
            var data = repository.GetAll().Select(x => new {Id = x.Id, Name = x.No}).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategory()
        {
            var repository = new NHibernateRepository<PayrollTable>();
            var data = repository.GetAll().Select(x => new {Id = x.Id, Name = x.No}).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmploymentState()
        {
            var repository = new NHibernateRepository<PayrollTable>();
            var data = repository.GetAll().Select(x => new {Id = x.Id, Name = x.No}).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPayrollNo()
        {
            var repository = new NHibernateRepository<PayrollTable>();
            var data = repository.GetAll().Select(x => new {Id = x.Id, Name = x.No}).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeByFilter()
        {
            var repository = new NHibernateRepository<Employee>();
            var data = repository.GetAll().Select(x => new { Id = x.Id,
                                                             //SocialInsuranceNo = x.SocialInsuranceNo,                                                             
                                                            FirstName = x.FirstName,
                                                            LastName = x.LastName}).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetEmployeeDeduction()
        {
            var repository = new NHibernateRepository<Employee>();
            var data = repository.GetAll().Select(x => new
            {
                Id = x.Id,
                //SocialInsuranceNo = x.SocialInsuranceNo,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeBenefit()
        {
            var repository = new NHibernateRepository<Employee>();
            var data = repository.GetAll().Select(x => new
            {
                Id = x.Id,
                //SocialInsuranceNo = x.SocialInsuranceNo,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeLoan()
        {
            var repository = new NHibernateRepository<Employee>();
            var data = repository.GetAll().Select(x => new
            {
                Id = x.Id,
                //SocialInsuranceNo = x.SocialInsuranceNo,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
