using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.Configurations;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{
    public class WorkshopController : Controller
    {

        [HttpPost]
        public ActionResult CheckParticularOvertimeShift(ParticularOvertimeShift Model, List<NormalShift> NormalShifts, Workshop Workshop, bool IsSummerDate)
        {
            var isParticularOvertimeShiftInNormalShifts = false;
            var particularOvertimeShift = Model;
            var ThereAreNormalShifts = true;
            var diffrence = 0;
            if (IsSummerDate)
                diffrence = 1;

            particularOvertimeShift.StartTime = new DateTime(2000, 1, 1, (particularOvertimeShift.StartTime.Hour + diffrence) == 24 ? 0 : (particularOvertimeShift.StartTime.Hour + diffrence), particularOvertimeShift.StartTime.Minute, particularOvertimeShift.StartTime.Second);
            particularOvertimeShift.EndTime = new DateTime(2000, 1, 1, (particularOvertimeShift.EndTime.Hour + diffrence) == 24 ? 0 : (particularOvertimeShift.EndTime.Hour + diffrence), particularOvertimeShift.EndTime.Minute, particularOvertimeShift.EndTime.Second);

            particularOvertimeShift = particularOvertimeShift.Prepare(DateTime.Now.Date);
          
         
            if (NormalShifts != null && NormalShifts.Count>0)
            {
                foreach (var normal in NormalShifts)
                {
                    normal.RestRangeEndTime = DateTime.Now;
                    normal.RestRangeStartTime = DateTime.Now;
                    var normalShift= normal.Prepare(DateTime.Now.Date);

                    var tempEntity = particularOvertimeShift.Prepare(normalShift.ShiftRangeStartTime);
                    if ((tempEntity.StartTime >= normalShift.ShiftRangeStartTime && tempEntity.EndTime <= normalShift.EntryTime) || (tempEntity.StartTime >= normalShift.ExitTime && tempEntity.EndTime <= normalShift.ShiftRangeEndTime))
                    {
                        isParticularOvertimeShiftInNormalShifts = true;
                        break;
                    }
                    if (normalShift.ShiftRangeEndTime.Day > normalShift.ShiftRangeStartTime.Day)
                    {
                        tempEntity = particularOvertimeShift.Prepare(normalShift.ShiftRangeEndTime);
                        if ((tempEntity.StartTime >= normalShift.ShiftRangeStartTime && tempEntity.EndTime <= normalShift.EntryTime) || (tempEntity.StartTime >= normalShift.ExitTime && tempEntity.EndTime <= normalShift.ShiftRangeEndTime))
                        {
                            isParticularOvertimeShiftInNormalShifts = true;
                            break;
                        }
                    }
                }
           
            }
            else
            {
                ThereAreNormalShifts = false;
            }

            return Json(new {IsParticularOvertimeShiftInNormalShifts = isParticularOvertimeShiftInNormalShifts , ThereAreNormalShift= ThereAreNormalShifts }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckParticularOvertimeShiftForEditAction(int WorkshopID, ParticularOvertimeShift Model)
        {
            var entityType = typeof(NormalShift);
            Workshop Workshop = ServiceFactory.ORMService.All<Workshop>().Where(w => w.Id == WorkshopID).FirstOrDefault();
            var Normalshifts = ServiceFactory.ORMService.All<NormalShift>().Where(w => w.Workshop == Workshop).ToList();

            var isParticularOvertimeShiftInNormalShifts = false;
            var particularOvertimeShift = Model;
            var ThereAreNormalShifts = true;
            var diffrence = 0;

            particularOvertimeShift.StartTime = new DateTime(2000, 1, 1, (particularOvertimeShift.StartTime.Hour + diffrence) == 24 ? 0 : (particularOvertimeShift.StartTime.Hour + diffrence), particularOvertimeShift.StartTime.Minute, particularOvertimeShift.StartTime.Second);
            particularOvertimeShift.EndTime = new DateTime(2000, 1, 1, (particularOvertimeShift.EndTime.Hour + diffrence) == 24 ? 0 : (particularOvertimeShift.EndTime.Hour + diffrence), particularOvertimeShift.EndTime.Minute, particularOvertimeShift.EndTime.Second);

            particularOvertimeShift = particularOvertimeShift.Prepare(DateTime.Now.Date);


            if (Normalshifts != null && Normalshifts.Count > 0)
            {
                foreach (var normal in Normalshifts)
                {
                   // normal.RestRangeEndTime = DateTime.Now;
                   // normal.RestRangeStartTime = DateTime.Now;
                    var normalShift = normal.Prepare(DateTime.Now.Date);

                    var tempEntity = particularOvertimeShift.Prepare(normalShift.ShiftRangeStartTime);
                    if ((tempEntity.StartTime >= normalShift.ShiftRangeStartTime && tempEntity.EndTime <= normalShift.EntryTime) || (tempEntity.StartTime >= normalShift.ExitTime && tempEntity.EndTime <= normalShift.ShiftRangeEndTime))
                    {
                        isParticularOvertimeShiftInNormalShifts = true;
                        break;
                    }
                    if (normalShift.ShiftRangeEndTime.Day > normalShift.ShiftRangeStartTime.Day)
                    {
                        tempEntity = particularOvertimeShift.Prepare(normalShift.ShiftRangeEndTime);
                        if ((tempEntity.StartTime >= normalShift.ShiftRangeStartTime && tempEntity.EndTime <= normalShift.EntryTime) || (tempEntity.StartTime >= normalShift.ExitTime && tempEntity.EndTime <= normalShift.ShiftRangeEndTime))
                        {
                            isParticularOvertimeShiftInNormalShifts = true;
                            break;
                        }
                    }
                }

            }
            else
            {
                ThereAreNormalShifts = false;
            }

            return Json(new { IsParticularOvertimeShiftInNormalShifts = isParticularOvertimeShiftInNormalShifts, ThereAreNormalShift = ThereAreNormalShifts }, JsonRequestBehavior.AllowGet);
        }
    }
}
