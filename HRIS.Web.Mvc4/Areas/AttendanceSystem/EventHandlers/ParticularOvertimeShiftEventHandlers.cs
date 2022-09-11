using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Entities;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.Configurations;
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{//todo : Mhd Update changeset no.1
    public class ParticularOvertimeShiftEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ParticularOvertimeShiftEventHandlers).FullName;
            model.Views[0].EditHandler = "ParticularOvertimeShiftEventHandler";
        }
        
        public override void CustomizeDetailGridModelForMasterDetail(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ParticularOvertimeShiftEventHandlers).FullName;
            model.Views[0].EditHandler = "ParticularOvertimeShiftEventHandler";
        }

        //public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
        //    IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        //{
        //    var particularOvertimeShift = ((ParticularOvertimeShift)entity).Prepare(DateTime.Now.Date);
        //    var workshop = ((Workshop)parententity).Prepare(DateTime.Now.Date);
           

        //    var particularOvertimeShifts = workshop.ParticularOvertimeShifts.Where(x => x.Id != particularOvertimeShift.Id);
        //    if (particularOvertimeShifts.Any(item =>
        //                                            particularOvertimeShift.StartTime >= item.StartTime && particularOvertimeShift.StartTime < item.EndTime ||
        //                                            particularOvertimeShift.EndTime > item.StartTime && particularOvertimeShift.EndTime <= item.EndTime ||
        //                                            particularOvertimeShift.StartTime < item.StartTime && particularOvertimeShift.EndTime > item.EndTime))
        //    {
        //        validationResults.Add(new ValidationResult
        //        {
        //            Message = Helpers.Resource.AttendanceLocalizationHelper.GetResource(Helpers.Resource.AttendanceLocalizationHelper.ParticularOvertimeShiftConflictWithOtherParticularOvertimeShiftsInThisWorkshop),
        //            Property = null
        //        });
        //    }
        //}

    }
}