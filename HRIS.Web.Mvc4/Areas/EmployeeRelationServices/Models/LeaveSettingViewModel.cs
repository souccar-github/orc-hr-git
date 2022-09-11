using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using NHibernate.Criterion;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class LeaveSettingViewModel : ViewModel
    {

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(LeaveSettingViewModel).FullName;
            model.Views[0].EditHandler = "LeaveSettingEditHandler";
            model.Views[0].ViewHandler = "LeaveSettingViewHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var leaveSetting = (LeaveSetting)entity;

            //if (operationType == CrudOperationType.Insert)
            //{
            //    var leaveSettingsCount = ServiceFactory.ORMService.All<LeaveSetting>().Count(x => x.Type == leaveSetting.Type);

            //    if (leaveSettingsCount == 1)
            //    {
            //        validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
            //        {
            //            Message = EmployeeRelationServicesLocalizationHelper.MsgCannotAddMoreSettingOfThisLeaveItShouldBeOnce,
            //            Property = null
            //        });

            //        return;
            //    }
            //}

            #region Custom Validation
            //لايمكن تفعيل خيار غير قابل للتجزئة مع الخيار لها رصيد شهري في نفس الوقت
            if (leaveSetting.HasMonthlyBalance && leaveSetting.IsIndivisible)
            {
                var prop = typeof(LeaveSetting).GetProperty("IsIndivisible");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotCheckHasMonthlyBalanceAndIsIndivisibleTogether),
                    Property = prop
                });
                return;
            }


   
            //لايمكن تفعيل خيار غير قابل للتجزئة مع الخيار الرصيد قابل للتجزئة ساعياً في نفس الوقت
            if (leaveSetting.IsIndivisible && leaveSetting.IsDivisibleToHours)
            {
                var prop = typeof(LeaveSetting).GetProperty("IsIndivisible");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotCheckIsIndivisibleAndIsDivisibleToHoursTogether),
                    Property = prop
                });
                return;
            }

            //الرصيد الشهري يجب ان يكون أكبر من الصفر عند اختيار أن للاجازة رصيد شهري
            if (leaveSetting.HasMonthlyBalance && leaveSetting.MonthlyBalance <= 0)
            {
                var prop = typeof(LeaveSetting).GetProperty("MonthlyBalance");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgMonthlyBalanceShouldBeGreaterThanZero),
                    Property = prop
                });
                return;
            }

            //العدد الاعظمي للإجازة يجب ان يكون أكبر من الصفر عند اختيار للاجازة عدد مرات أعظمي
            if (leaveSetting.HasMaximumNumber && leaveSetting.MaximumNumber <= 0)
            {
                var prop = typeof(LeaveSetting).GetProperty("MaximumNumber");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgMaximumNumberShouldBeGreaterThanZero),
                    Property = prop
                });
                return;
            }

            //الحد الأعلى المسموح للساعات في اليوم يجب ان يكون أكبر من الصفر عند اختيار أن الرصيد قابل للتجزئة ساعياً
            if (leaveSetting.IsDivisibleToHours && leaveSetting.MaximumHoursPerDay <= 0)
            {
                var prop = typeof(LeaveSetting).GetProperty("MaximumHoursPerDay");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgMaximumHoursPerDayShouldBeGreaterThanZero),
                    Property = prop
                });
                return;
            }

            //عدد الساعات المكافئة ليوم إجازة يجب ان يكون أكبر من الصفر عند اختيار أن الرصيد قابل للتجزئة ساعياً
            if (leaveSetting.IsDivisibleToHours && leaveSetting.HoursEquivalentToOneLeaveDay <= 0)
            {
                var prop = typeof(LeaveSetting).GetProperty("HoursEquivalentToOneLeaveDay");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgHoursEquivalentToOneLeaveDayShouldBeGreaterThanZero),
                    Property = prop
                });
                return;
            }

            //الحد الأعلى المسموح للساعات في اليوم يجب ان يكون اصغر من عدد الساعات المكافئة ليوم إجازة
            if (leaveSetting.IsDivisibleToHours && (leaveSetting.MaximumHoursPerDay >= leaveSetting.HoursEquivalentToOneLeaveDay))
            {
                var prop = typeof(LeaveSetting).GetProperty("MaximumHoursPerDay");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgMaximumHoursPerDayShouldBeSmallerThanHoursEquivalentToOneLeaveDay),
                    Property = prop
                });
                return;
            } 
            #endregion
            
        }
    }

    public class BalanceSlicesViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(BalanceSlicesViewModel).FullName;
            model.Views[0].EditHandler = "BalanceSlicesEditHandler";
            model.Views[0].ViewHandler = "BalanceSlicesViewHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity,
            IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var balanceSlice = (BalanceSlice)entity;
            var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(requestInformation.NavigationInfo.Previous[0].RowId);

            //الرصيد الشهري يجب ان يكون أكبر من الصفر عند اختيار أن شرائح رصيد الاجازة لها رصيد شهري
            if (balanceSlice.HasMonthlyBalance && balanceSlice.MonthlyBalance <= 0)
            {
                var prop = typeof(BalanceSlice).GetProperty("MonthlyBalance");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgMonthlyBalanceShouldBeGreaterThanZero),
                    Property = prop
                });
                return;
            }

            if (balanceSlice.HasMonthlyBalance && balanceSlice.MaximumRoundedLeaveDays>balanceSlice.Balance)
            {
                var prop = typeof(BalanceSlice).GetProperty("MaximumRoundedLeaveDays");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgSliceBalanceShouldBeGreaterOrEqualMaximumRoundedLeaveDays),
                    Property = prop
                });
                return;
            }

            if (leaveSetting.BalanceSlices.Any(x => x.Id != balanceSlice.Id && 
                ((x.FromYearOfServices <= balanceSlice.FromYearOfServices &&
                  x.ToYearOfServices > balanceSlice.FromYearOfServices) ||
                 (x.FromYearOfServices < balanceSlice.ToYearOfServices &&
                  x.ToYearOfServices >= balanceSlice.ToYearOfServices))) || leaveSetting.BalanceSlices.Any(y=>y.FromYearOfServices> balanceSlice.FromYearOfServices && y.ToYearOfServices< balanceSlice.ToYearOfServices))
            {
                var prop = typeof (BalanceSlice).GetProperty("FromYearOfServices");
                validationResults.Add(new ValidationResult()
                                      {
                                          Message =
                                              EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper
                                              .MsgThereIsBalanceSliceInSameRange),
                                          Property = prop
                                      });

                return;
            }

        }
    }
    public class PaidSlicesViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(PaidSlicesViewModel).FullName;
            model.Views[0].EditHandler = "PaidSlicesViewModel";
            model.Views[0].ViewHandler = "PaidSlicesViewModel";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity,
            IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var PaidSlice = (PaidSlice)entity;
            var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(requestInformation.NavigationInfo.Previous[0].RowId);

            if (leaveSetting.PaidSlices.Any(x => x.Id != PaidSlice.Id &&
                ((x.FromBalance <= PaidSlice.FromBalance &&
                  x.ToBalance > PaidSlice.FromBalance) ||
                 (x.FromBalance < PaidSlice.ToBalance &&
                  x.ToBalance >= PaidSlice.ToBalance))) || leaveSetting.PaidSlices.Any(y => y.FromBalance > PaidSlice.FromBalance && y.ToBalance < PaidSlice.ToBalance))
            {
                var prop = typeof(BalanceSlice).GetProperty("FromBalance");
                validationResults.Add(new ValidationResult()
                {
                    Message =
                        EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper
                        .MsgThereIsPaidSliceInSameRange),
                    Property = prop
                });

                return;
            }

        }
    }
}