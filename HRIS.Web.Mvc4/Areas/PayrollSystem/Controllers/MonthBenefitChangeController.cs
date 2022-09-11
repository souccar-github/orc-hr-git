//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using HRIS.Domain.PayrollSystem.Entities;
//using HRIS.Domain.PayrollSystem.RootEntities;
//using  Project.Web.Mvc4.Models;
//using Souccar.Infrastructure.Extenstions;
//using  Project.Web.Mvc4.Extensions;

//namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
//{
//    public class MonthBenefitChangeController : Controller
//    {
//        // عرض معلومات التعويض للتغيير الشهري الحالي
//        [HttpPost]
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
//    }
//}
