using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.Configurations;
using Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.AttendanceSystem.Helpers;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{
    public class NormalShiftEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(NormalShiftEventHandlers).FullName;
            model.Views[0].EditHandler = "NormalShiftEditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var workshop = (Workshop)parententity;
            var normalShiftEntity = (NormalShift)entity;
            var oneDay = new TimeSpan(01, 00, 00, 00, 00);
            int ShiftExist = 0;

            ShiftExist = workshop.NormalShifts.Count(n => n.NormalShiftOrder == normalShiftEntity.NormalShiftOrder && n.Id != normalShiftEntity.Id);
            if (ShiftExist > 0)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                              .GetFullKey(CustomMessageKeysAttendanceSystemModule.ThereAreAnotherShiftWithThisOrder)),
                    Property = null
                });
                return;
            }
            if ((normalShiftEntity.RestRangeStartTime == normalShiftEntity.RestRangeEndTime ) ||(normalShiftEntity.RestRangeStartTime == new DateTime() || normalShiftEntity.RestRangeStartTime == null) && (normalShiftEntity.RestRangeEndTime == new DateTime() || normalShiftEntity.RestRangeEndTime == null) && normalShiftEntity.RestPeriod == 0)
            {
                normalShiftEntity.RestRangeStartTime = normalShiftEntity.EntryTime;
                normalShiftEntity.RestRangeEndTime = normalShiftEntity.ExitTime;
            }
            else
            {
                if (normalShiftEntity.RestRangeStartTime == new DateTime() || normalShiftEntity.RestRangeStartTime == null)
                {
                    normalShiftEntity.RestRangeStartTime = normalShiftEntity.EntryTime;
                }
                if (normalShiftEntity.RestRangeEndTime == new DateTime() || normalShiftEntity.RestRangeEndTime == null)
                {
                    normalShiftEntity.RestRangeEndTime = normalShiftEntity.ExitTime;
                }
            }

            normalShiftEntity = normalShiftEntity.Prepare(DateTime.Now.Date);
            //if (normalShiftEntity.EntryTime < normalShiftEntity.ShiftRangeStartTime)
            //{
            //    validationResults.Add(new ValidationResult
            //    {// لا يمكن أن يكون وقت الدخول أصغير من وقت بداية المجال
            //        Message = ServiceFactory.LocalizationService
            //                        .GetResource(CustomMessageKeysAttendanceSystemModule
            //                        .GetFullKey(CustomMessageKeysAttendanceSystemModule.EntryTimeCannotBeLessThanRangeStartTime)),
            //        Property = typeof(NormalShift).GetProperty(typeof(NormalShift).GetPropertyNameAsString<NormalShift>(x => x.EntryTime))
            //    });
            //}
            if (normalShiftEntity.RestRangeStartTime < normalShiftEntity.EntryTime)
            {
                validationResults.Add(new ValidationResult
                {// لا يمكن أن يكون وقت بداية الاستراحة أصغر من وقت الدخول
                    Message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.RestRangeStartTimeCannotBeLessThanEntryTime)),
                    Property = typeof(NormalShift).GetProperty(typeof(NormalShift).GetPropertyNameAsString<NormalShift>(x => x.RestRangeStartTime))
                });
            }
            if (normalShiftEntity.RestRangeEndTime < normalShiftEntity.RestRangeStartTime)
            {
                validationResults.Add(new ValidationResult
                {// لا يمكن أن يكون وقت نهاية الاستراحة أصغير من وقت بداية الاستراحة
                    Message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.RestRangeEndTimeCannotBeLessThanRestRangeStartTime)),
                    Property = typeof(NormalShift).GetProperty(typeof(NormalShift).GetPropertyNameAsString<NormalShift>(x => x.RestRangeEndTime))
                });
            }
            if (normalShiftEntity.ExitTime < normalShiftEntity.RestRangeEndTime)
            {
                validationResults.Add(new ValidationResult
                {// لا يمكن أن يكون وقت الخروج أصغير من وقت نهاية الاستراحة
                    Message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.ExitTimeCannotBeLessThanRestRangeEndTime)),
                    Property = typeof(NormalShift).GetProperty(typeof(NormalShift).GetPropertyNameAsString<NormalShift>(x => x.ExitTime))
                });
            }
            //if (normalShiftEntity.ShiftRangeEndTime < normalShiftEntity.ExitTime)
            //{
            //    validationResults.Add(new ValidationResult
            //    {// لا يمكن أن يكون وقت نهاية المجال أصغير من وقت نهاية الخروج
            //        Message = ServiceFactory.LocalizationService
            //                        .GetResource(CustomMessageKeysAttendanceSystemModule
            //                        .GetFullKey(CustomMessageKeysAttendanceSystemModule.ShiftRangeEndTimeCannotBeLessThanExitTime)),
            //        Property = typeof(NormalShift).GetProperty(typeof(NormalShift).GetPropertyNameAsString<NormalShift>(x => x.ShiftRangeEndTime))
            //    });
            //}

            if ((normalShiftEntity.ShiftRangeEndTime - normalShiftEntity.ShiftRangeStartTime) >= oneDay)
            {
                // الفترة ليست ممتدة على اكثر من يومين 
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.TheShiftMastBeDuringOneDay)),
                    Property = null
                });
            }

            if (normalShiftEntity.ShiftRangeEndTime < normalShiftEntity.ExitTime || normalShiftEntity.EntryTime < normalShiftEntity.ShiftRangeStartTime)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                          .GetFullKey(CustomMessageKeysAttendanceSystemModule.TheRangeOfEntryTimeAndExitTimeMustBeWithinTheRangeOfMinimumTimeOfEntryAndMaximumTimeOfExit)),

                    Property = null
                });
                return;
            }

            var normalShifts = workshop.NormalShifts.Where(x => x.Id != normalShiftEntity.Id);

            foreach (var item in normalShifts)
            {
                var normal = normalShiftEntity.Prepare(new DateTime(2000, 1, 1));
                var itemShiftRanges = item.Prepare(new DateTime(2000, 1, 1));
                if (itemShiftRanges.ShiftRangeStartTime >= normal.ShiftRangeStartTime && itemShiftRanges.ShiftRangeStartTime < normal.ShiftRangeEndTime
                    || itemShiftRanges.ShiftRangeEndTime > normal.ShiftRangeStartTime && itemShiftRanges.ShiftRangeEndTime <= normal.ShiftRangeEndTime
                    ||itemShiftRanges.ShiftRangeStartTime<=normal.ShiftRangeStartTime&&itemShiftRanges.ShiftRangeEndTime>=normal.ShiftRangeEndTime)
                {
                    // الفترة المدخلة تتقاطع مع فترات اخرى في نفس الوردية
                    validationResults.Add(new ValidationResult
                    {
                        Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                            .GetFullKey(CustomMessageKeysAttendanceSystemModule.NormalShiftConflictWithOtherNormalShiftsInThisWorkshop)),
                        Property = null
                    });
                    return;
                }

            }


        }
    }
}
