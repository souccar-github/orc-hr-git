using HRIS.Domain.PMS.RootEntities;
using  Project.Web.Mvc4.Areas.PMS.EventHandlers;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.PMS.Controllers
{
    public class SectionController : Controller
    {

        public ActionResult GetSectionViewModel(int? id)
        {
            var viewModel = new AppraisalSectionEventHandlers();
            if (id == null || id == 0)
                return Json(viewModel, JsonRequestBehavior.AllowGet);

            var section = ServiceFactory.ORMService.GetById<AppraisalSection>((int)id);
            viewModel.Id = section.Id;
            viewModel.Items = section.Items.Select(x => new SectionItemViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Weight = x.Weight,
                Kpis = x.Kpis.Select(y => new SectionItemKpiViewModel()
                {
                    Description = y.Description,
                    Weight = y.Weight,
                    Value = y.Value,
                    Id = y.Id
                }).ToList()
            }).ToList();
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }
    }
}
