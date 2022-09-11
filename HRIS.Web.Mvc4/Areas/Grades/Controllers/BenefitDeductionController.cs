//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using HRIS.Domain.PayrollSystem.Configurations;
//using HRIS.Domain.PayrollSystem.RootEntities;
//using Souccar.Infrastructure.Extenstions;

//namespace Project.Web.Mvc4.Areas.Grades.Controllers
//{
//    public class BenefitDeductionController : Controller
//    {
//        //
//        // GET: /OrganizationChart/BenefitDeduction/
//        public ActionResult GetBenefitCardInformation(int monthBenefitChangeId, int monthId)
//        {
//            var monthBenefitChanges = new Dictionary<string, object>();
//            bool isSuccess;
//            try
//            {
//                var monthBenefitChange = ((Month)typeof(Month).GetById(monthId)).MonthBenefitChanges.First(x => x.Id == monthBenefitChangeId);
//                monthBenefitChanges["BenefitCardId"] = monthBenefitChange.BenefitCard.Id;
//                monthBenefitChanges["Value"] = monthBenefitChange.Value;
//                monthBenefitChanges["Formula"] = monthBenefitChange.Formula;
//                monthBenefitChanges["ExtraValue"] = monthBenefitChange.ExtraValue;
//                monthBenefitChanges["ExtraValueFormula"] = monthBenefitChange.ExtraValueFormula;
//                monthBenefitChanges["CeilValue"] = monthBenefitChange.CeilValue;
//                monthBenefitChanges["CeilFormula"] = monthBenefitChange.CeilFormula;
//                isSuccess = true;
//            }
//            catch
//            {
//                isSuccess = false;
//            }

//            return Json(new
//            {
//                result = monthBenefitChanges,
//                Success = isSuccess,
//            }, JsonRequestBehavior.AllowGet);
//        }


//        [HttpPost]
//        public ActionResult GetDeductionCardInformation(int id)
//        {
//            var data = new Dictionary<string, object>();
//            bool isSuccess;
//            try
//            {
//                var benefitCard = (DeductionCard)typeof(DeductionCard).GetById(id);
//                data["Value"] = benefitCard.Value;
//                data["Formula"] = benefitCard.Formula;
//                data["ExtraValue"] = benefitCard.ExtraValue;
//                data["ExtraValueFormula"] = benefitCard.ExtraValueFormula;
//                isSuccess = true;
//            }
//            catch
//            {
//                isSuccess = false;
//            }

//            return Json(new
//            {
//                result = data,
//                Success = isSuccess,
//            });
//        }

//    }
//}
