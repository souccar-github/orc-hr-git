using HRIS.Domain.AttendanceSystem.Configurations;
using Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.Configurations;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{
    public class NonAttendanceFormController : Controller
    {
        public ActionResult GetSectionViewModel(int? id)
        {
            var viewModel = new NonAttendanceFormEventHandlers();
            if (id == null || id == 0)
                return Json(viewModel, JsonRequestBehavior.AllowGet);

            var section = ServiceFactory.ORMService.GetById<NonAttendanceForm>((int)id);
            viewModel.Id = section.Id;
            viewModel.Items = section.NonAttendanceSlices.Select(x => new SectionSliceItemViewModel()
            {
                Id = x.Id,
                StartSlice = x.StartSlice,
                EndSlice = x.EndSlice,
                Value = x.Value,
                InfractionForm = new InfractionForm()

                { Description = x.InfractionForm.NameForDropdown,
                    Id = x.InfractionForm.Id,
                    Number = x.InfractionForm.Number,
                    Infraction = x.InfractionForm.Infraction,
             
              
                },
                Percentages = x.NonAttendanceSlicePercentages.Select(y => new SectionPercentagesItemViewModel()
                {
                    Id = y.Id,
                    PercentageOrder = y.PercentageOrder,
                    Percentage = y.Percentage
                }).ToList()
            }).ToList();
            return Json(viewModel, JsonRequestBehavior.AllowGet);

        }

    }
}
