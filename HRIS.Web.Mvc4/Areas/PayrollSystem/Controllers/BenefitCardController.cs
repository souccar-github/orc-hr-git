using System.Collections.Generic;
using System.Web.Mvc;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;


namespace Project.Web.Mvc4.Areas.PayrollSystem.Controllers
{
    public class BenefitCardController : Controller
    {
        // عرض معلومات بطاقة التعويض
        [HttpPost]
        public ActionResult GetBenefitCardInformation(int id)
        {
            var data = new Dictionary<string, object>();
            bool isSuccess;
            try
            {
                var benefitCard = (BenefitCard)typeof(BenefitCard).GetById(id);
                data["Value"] = benefitCard.Value;
                data["Formula"] = benefitCard.Formula;
                data["ExtraValue"] = benefitCard.ExtraValue;
                data["ExtraValueFormula"] = benefitCard.ExtraValueFormula;
                data["CeilValue"] = benefitCard.CeilValue;
                data["CeilFormula"] = benefitCard.CeilFormula;
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
