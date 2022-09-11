using System.Web.Mvc;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Validation.MessageKeys;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.PayrollSystem.Helper
{
    public class StatusManagementHelperController : Controller
    {
        public ActionResult GetRecordAlreadyAuditedResult()
        {
            return Json(new
            {
                Success = false,
                Msg = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.RecordAlreadyAudited))
            });
        }

        public ActionResult GetRecordAlreadyNotAuditedResult()
        {
            return Json(new
            {
                Success = false,
                Msg = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysPayrollSystemModule.RecordAlreadyNotAudited))
            });
        }

        public ActionResult GetFaildResult()
        {
            return Json(new
            {
                Success = false,
                Msg = Helpers.GlobalResource.FailMessage
            });
        }

        public ActionResult GetSuccessResult()
        {
            return Json(new
            {
                Success = true,
                Msg = Helpers.GlobalResource.DoneMessage
            });
        }
        
   
    }
}
