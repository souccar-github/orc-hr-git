using System.Collections.Generic;
using System.Web.Mvc;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
{
    public class DeductionCardController : Controller
    {
        // عرض معلومات بطاقة الحسم
        [HttpPost]
        public ActionResult GetDeductionCardInformation(int id)
        {
            var data = new Dictionary<string, object>();
            bool isSuccess;
            try
            {
                var benefitCard = (DeductionCard)typeof(DeductionCard).GetById(id);
                data["Value"] = benefitCard.Value;
                data["Formula"] = benefitCard.Formula;
                data["ExtraValue"] = benefitCard.ExtraValue;
                data["ExtraValueFormula"] = benefitCard.ExtraValueFormula;
                isSuccess = true;
            }
            catch
            {
                isSuccess = false;
            }

            return Json(new
            {
                result = data,
                Success = isSuccess,
            });
        }
    }
}
