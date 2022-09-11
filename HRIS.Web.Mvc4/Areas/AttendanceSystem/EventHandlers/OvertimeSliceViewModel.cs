using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.Configurations;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class OvertimeSliceViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(OvertimeSliceViewModel).FullName;
        }
        public override void CustomizeDetailGridModelForMasterDetail(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(OvertimeSliceViewModel).FullName;
            model.Views[0].EditHandler = "OvertimeSliceEditHandler";
        }


        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var overtimeForm = (OvertimeForm)parententity;
            var slice = (OvertimeSlice)entity;
    //        var overtimeForm = (OvertimeForm)typeof(OvertimeForm).GetById(requestInformation.NavigationInfo.Previous[0].RowId);
            var overTimeSlices = overtimeForm.OvertimeSlices.Where(x => x.Id != slice.Id);
            foreach (var overtimeSlice in overTimeSlices)
            {
                if (slice.StartSlice >= overtimeSlice.StartSlice && slice.EndSlice <= overtimeSlice.EndSlice)
                {
                    // شريحة نموذج الاضافي المدخلة تتقاطع مع شريحة اخرى في نفس النموذج
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(
                                CustomMessageKeysAttendanceSystemModule
                                    .OvertimeSliceConflictWithOtherOvertimeSlicesInThisOvertimeForm)),
                        Property = null
                    });
                }

                if (slice.StartSlice >= overtimeSlice.StartSlice && slice.StartSlice <= overtimeSlice.EndSlice)
                {
                    // شريحة نموذج الاضافي المدخلة تتقاطع مع شريحة اخرى في نفس النموذج
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(
                                CustomMessageKeysAttendanceSystemModule
                                    .OvertimeSliceConflictWithOtherOvertimeSlicesInThisOvertimeForm)),
                        Property = null
                    });
                }

                if (slice.EndSlice <= overtimeSlice.EndSlice && slice.EndSlice >= overtimeSlice.StartSlice)
                {
                    // شريحة نموذج الاضافي المدخلة تتقاطع مع شريحة اخرى في نفس النموذج
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(
                                CustomMessageKeysAttendanceSystemModule
                                    .OvertimeSliceConflictWithOtherOvertimeSlicesInThisOvertimeForm)),
                        Property = null
                    });
                }
            }
        }
    }
}