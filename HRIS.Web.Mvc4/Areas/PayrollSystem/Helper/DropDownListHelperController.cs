using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Models;
using Souccar.Core.Extensions;
using Souccar.Domain.DomainModel;
using Souccar.Domain.PersistenceSupport;
using Souccar.Infrastructure.Extenstions;



namespace Project.Web.Mvc4.Areas.PayrollSystem.Helper
{
    public class DropDownListHelperController : Controller
    {
        // قائمة الحسميات الاساسية
        [HttpPost]
        public ActionResult GetPrimaryDeductionCards()
        {
            var deductionCards = typeof(DeductionCard).GetAll<DeductionCard>()
                .Where(x => x.IsPrimaryDeduction);
            var result = new ArrayList();
            foreach (var item in deductionCards)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEmptyList()
        {
            var result = new ArrayList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        // قائمة الاشهر التي نوعها رواتب وتعويضات
        [HttpPost]
        public ActionResult GetSalaryAndBenefitMonths()
        {
            var months = typeof(Month).GetAll<Month>()
                .Where(x => x.MonthType == MonthType.SalaryAndBenefit);
            var result = new ArrayList();
            foreach (var item in months)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        //بطاقات الموظفين الشهرية في شهر محدد
        [HttpPost]
        public ActionResult GetMonthlyCardsForMonth(int monthId)
        {
            var month = (Month)typeof(Month).GetById(monthId);
            var result = new ArrayList();
            foreach (var item in month.MonthlyCards)
            {

                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        //بطاقات الموظفين الشهرية لبطاقة اساسية
        [HttpPost]
        public ActionResult GetMonthlyCardsForPrimaryCard(int primaryCardId)
        {
            var monthlyCards = typeof(Month).GetAll<Month>().SelectMany(x => x.MonthlyCards).Where(x => x.PrimaryCard.Id == primaryCardId);
            var result = new ArrayList();
            foreach (var item in monthlyCards)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        //قروض الموظف الغير منتهية
        [HttpPost]
        public ActionResult GetNotFinishedEmployeeLoans(int primaryEmployeeCardId)
        {
            var primaryCard = (EmployeeCard)typeof(EmployeeCard).GetById(primaryEmployeeCardId);
            var notFinishedEmployeeLoans = primaryCard.EmployeeLoans.Where(x => x.RemainingAmountOfLoan > 0);
            var result = new ArrayList();
            foreach (var item in notFinishedEmployeeLoans)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] =item.LoanNumber;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        // 
        [HttpPost]
        public ActionResult GetMonths()
        {
            var months = typeof(Month).GetAll<Month>()
                .Where(x => x.MonthType == MonthType.SalaryAndBenefit);
            var result = new ArrayList();
            foreach (var item in months)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ReadAuditableReferanceToList(string typeName, RequestInformation requestInformation)
        {
            var type = typeName.ToType();
            if (type == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            var repository = typeof(IRepository<>).CreateGenericInstance(type);

            var query = (IQueryable<IAggregateRoot>)repository.CallMethod("GetAll", new Type[] { });
            var result = new ArrayList();
            foreach (var item in query)
            {
                //if (item.GetPropertyValue("AuditState").ToString() == AuditState.NotAudited.ToString())
                //{
                //    continue;
                //}
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.GetPropertyValue("Id");
                if (item.GetType().GetProperties().Any(x => x.Name == "NameForDropdown"))
                {
                    temp["Name"] = item.GetPropertyValue("NameForDropdown");
                }
                else if (item.GetType().GetProperties().Any(x => x.Name == "Name"))
                {
                    temp["Name"] = item.GetPropertyValue("Name");
                }
                else if (item.GetType().GetProperties().Any(x => x.Name == "Title"))
                {
                    temp["Name"] = item.GetPropertyValue("Title");
                }
                else
                {
                    temp["Name"] = item.GetPropertyValue("Id");
                }
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        //// البلدان التي لها فئات اغتراب بالتالي يمكن ظهورها عند ادخال اذن السفر
        //[HttpPost]
        //public ActionResult GetCountriesHaveTravelCategory()
        //{
        //    var travelCategories = typeof(TravelCategory).GetAll<TravelCategory>();
        //    var countries = typeof(Country).GetAll<Country>().Where(x => travelCategories.Any(y => y.TravelCategoryCountries.Any(z => z.Country.Id == x.Id)));
        //    var result = new ArrayList();
        //    var emptyRecord = new Dictionary<string, object>();
        //    emptyRecord["Id"] = 0;
        //    emptyRecord["Name"] = String.Empty;
        //    result.Add(emptyRecord);

        //    foreach (var item in countries)
        //    {
        //        var temp = new Dictionary<string, object>();
        //        temp["Id"] = item.Id;
        //        temp["Name"] = item.Name;
        //        result.Add(temp);
        //    }
        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        //}
    }
}
