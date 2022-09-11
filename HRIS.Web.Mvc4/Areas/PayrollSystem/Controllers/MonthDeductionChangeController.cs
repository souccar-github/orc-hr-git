//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using HRIS.Domain.PayrollSystem.RootEntities;
//using Souccar.Infrastructure.Extenstions;
//using  Project.Web.Mvc4.Extensions;

//namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
//{
//    public class MonthDeductionChangeController : Controller
//    {
//        // عرض معلومات الحسم للتغيير الشهري الحالي
//        [HttpPost]
//        public ActionResult GetDeductionCardInformation(int monthDeductionChangeId, int monthId)
//        {
//            var monthDeductionChanges = new Dictionary<string, object>();
//            bool isSuccess;
//            try
//            {
//                var monthDeductionChange = ((Month)typeof(Month).GetById(monthId)).MonthDeductionChanges.First(x => x.Id == monthDeductionChangeId);
//                monthDeductionChanges["DeductionCardId"] = monthDeductionChange.DeductionCard.Id;
//                monthDeductionChanges["Value"] = monthDeductionChange.Value;
//                monthDeductionChanges["Formula"] = monthDeductionChange.Formula;
//                monthDeductionChanges["ExtraValue"] = monthDeductionChange.ExtraValue;
//                monthDeductionChanges["ExtraValueFormula"] = monthDeductionChange.ExtraValueFormula;
//                isSuccess = true;
//            }
//            catch
//            {
//                isSuccess = false;
//            }

//            return Json(new
//            {
//                result = monthDeductionChanges,
//                Success = isSuccess,
//            }, JsonRequestBehavior.AllowGet);
//        }
//    }
//}
