using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class RecruitmentMilitaryServiceViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(RecruitmentMilitaryServiceViewModel).FullName;
            model.Views[0].EditHandler = "RecruitmentMilitaryServiceEditHandler";
            model.Views[0].ViewHandler = "RecruitmentMilitaryServiceEditHandler";

        }

        public override ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
        {
            var msg = "";
            var jobApplication = ServiceFactory.ORMService.GetById<JobApplication>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (jobApplication.Gender == HRIS.Domain.Personnel.Enums.Gender.Female)
            {
                msg = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MilitaryServiceGenderValidationMessage);
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = false, message = msg });
            }
            else
            {
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = msg });
            }

        }

        public override void AfterValidation(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var jobApplication = ServiceFactory.ORMService.GetById<JobApplication>(requestInformation.NavigationInfo.Previous[0].RowId);

            var militaryService = entity as RecruitmentMilitaryService;
            var type = typeof(RecruitmentMilitaryService);

            if (militaryService.Status == HRIS.Domain.Personnel.Enums.MilitaryStatus.Exempt)
            {

                var befor = jobApplication.RecruitmentMilitaryServices.Where(x => x.DateOfExemption == militaryService.DateOfExemption).ToList();


                if (befor.Count > 0 && operationType != CrudOperationType.Update)
                {
                    var prop = type.GetProperty("DateOfExemption");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsYouCanNotAddDuplicateDateOfExemption), Property = prop });

                }

                if (string.IsNullOrEmpty(militaryService.ExemptionReason))
                {
                    var prop = type.GetProperty("ExemptionReason");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }
                if (militaryService.DateOfExemption == null)
                {
                    var prop = type.GetProperty("DateOfExemption");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }
                if (DateTime.Compare(militaryService.DateOfExemption.GetValueOrDefault(), jobApplication.DateOfBirth) <= 0)
                {
                    var prop = type.GetProperty("DateOfExemption");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgDateOfExemptionMustBeGreaterThanDateOfBirth), Property = prop });
                }
                militaryService.DateOfDelay = null;
                militaryService.HoldDate = null;
                militaryService.MilitiryServiceDocIssuance = null;
                militaryService.ReserveStartDate = null;
                militaryService.ServiceEndDate = null;
                militaryService.ServiceStartDate = null;

            }
            if (militaryService.Status == HRIS.Domain.Personnel.Enums.MilitaryStatus.Delayed)
            {

                var befor = jobApplication.RecruitmentMilitaryServices.Where(x => x.DateOfDelay == militaryService.DateOfDelay).ToList();

                if (befor.Count > 0 && operationType != CrudOperationType.Update)
                {
                    var prop = type.GetProperty("DateOfDelay");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsYouCanNotAddDuplicateDateOfDelay), Property = prop });

                }

                if (string.IsNullOrEmpty(militaryService.DelayReason))
                {
                    var prop = type.GetProperty("DelayReason");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }
                if (militaryService.DateOfDelay == null)
                {
                    var prop = type.GetProperty("DateOfDelay");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }
                if (DateTime.Compare(militaryService.DateOfDelay.GetValueOrDefault(), jobApplication.DateOfBirth) <= 0)
                {
                    var prop = type.GetProperty("DateOfDelay");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgDateOfDelayMustBeGreaterThanDateOfBirth), Property = prop });
                }
                militaryService.DateOfExemption = null;
                militaryService.HoldDate = null;
                militaryService.MilitiryServiceDocIssuance = null;
                militaryService.ReserveStartDate = null;
                militaryService.ServiceEndDate = null;
                militaryService.ServiceStartDate = null;
            }

            if (militaryService.Status == HRIS.Domain.Personnel.Enums.MilitaryStatus.Served ||
                militaryService.Status == HRIS.Domain.Personnel.Enums.MilitaryStatus.Hold ||
                militaryService.Status == HRIS.Domain.Personnel.Enums.MilitaryStatus.Reserve
                )
            {
                if (string.IsNullOrEmpty(militaryService.MilitiryServiceNo))
                {
                    var prop = type.GetProperty("MilitiryServiceNo");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }
                if (militaryService.MilitiryServiceDocIssuance == null)
                {
                    var prop = type.GetProperty("MilitiryServiceDocIssuance");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }
                if (DateTime.Compare(militaryService.MilitiryServiceDocIssuance.GetValueOrDefault(), jobApplication.DateOfBirth) <= 0)
                {
                    var prop = type.GetProperty("MilitiryServiceDocIssuance");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgMilitiryServiceDocIssuanceMustBeGreaterThanDateOfBirth), Property = prop });
                }
                if (militaryService.Granter == null || militaryService.Granter.IsTransient())
                {
                    var prop = type.GetProperty("Granter");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }

                if (militaryService.ServiceEndDate == null)
                {
                    var prop = type.GetProperty("ServiceEndDate");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }

                if (militaryService.ServiceStartDate == null)
                {
                    var prop = type.GetProperty("ServiceStartDate");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }
                if (DateTime.Compare(militaryService.ServiceStartDate.GetValueOrDefault(), jobApplication.DateOfBirth) <= 0)
                {
                    var prop = type.GetProperty("ServiceStartDate");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgServiceStartDateMustBeGreaterThanDateOfBirth), Property = prop });
                }
                if (DateTime.Compare(militaryService.MilitiryServiceDocIssuance.GetValueOrDefault(), militaryService.ServiceStartDate.GetValueOrDefault()) <= 0)
                {
                    var prop = type.GetProperty("MilitiryServiceDocIssuance");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgMilitiryServiceDocIssuanceMustBeGreaterThanServiceStartDate), Property = prop });
                }
                if (militaryService.MilitiryServiceDocIssuance.HasValue && militaryService.MilitiryServiceDocIssuance.Value >= DateTime.Now.Date)
                {
                    var prop = type.GetProperty("MilitiryServiceDocIssuance");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgMilitiryServiceDocIssuanceMustBeLessThanCurrentDate), Property = prop });
                }
                if (DateTime.Compare(militaryService.ServiceEndDate.GetValueOrDefault(), militaryService.ServiceStartDate.GetValueOrDefault()) <= 0)
                {
                    var prop = type.GetProperty("ServiceEndDate");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgServiceEndDateMustBeGreaterThanServiceStartDate), Property = prop });
                }
                militaryService.DateOfExemption = null;
                militaryService.DateOfDelay = null;

                if (militaryService.Days <= 0 || militaryService.Days > 31)
                {
                    var prop = type.GetProperty("Days");
                    validationResults.Add(new ValidationResult() { Message = GlobalResource.RequiredMessage, Property = prop });
                }
                if (militaryService.Months <= 0 || militaryService.Months > 12)
                {
                    var prop = type.GetProperty("Months");
                    validationResults.Add(new ValidationResult() { Message = GlobalResource.RequiredMessage, Property = prop });
                }
                if (militaryService.Years <= 0)
                {
                    var prop = type.GetProperty("Years");
                    validationResults.Add(new ValidationResult() { Message = GlobalResource.RequiredMessage, Property = prop });
                }
            }
            if (militaryService.Status == HRIS.Domain.Personnel.Enums.MilitaryStatus.Hold)
            {

                var befor = jobApplication.RecruitmentMilitaryServices.Where(x => x.HoldDate == militaryService.HoldDate).ToList();

                if (befor.Count > 0 && operationType != CrudOperationType.Update)
                {
                    var prop = type.GetProperty("HoldDate");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsYouCanNotAddDuplicateHoldDate), Property = prop });

                }
                if (militaryService.HoldDate <= militaryService.ServiceEndDate)
                {
                    var prop = type.GetProperty("HoldDate");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsHoldDateMustBeGreaterThanServiceEndDate), Property = prop });
                }
                if (militaryService.HoldDate == null)
                {
                    var prop = type.GetProperty("HoldDate");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }

                militaryService.ReserveStartDate = null;
            }
            if (militaryService.Status == HRIS.Domain.Personnel.Enums.MilitaryStatus.Reserve)
            {


                var befor = jobApplication.RecruitmentMilitaryServices.Where(x => x.ReserveStartDate == militaryService.ReserveStartDate).ToList();

                if (befor.Count > 0 && operationType != CrudOperationType.Update)
                {
                    var prop = type.GetProperty("ReserveStartDate");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsYouCanNotAddDuplicateReserveStartDate), Property = prop });

                }
                if (militaryService.ReserveStartDate <= militaryService.ServiceEndDate)
                {
                    var prop = type.GetProperty("ReserveStartDate");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsgReserveStartDateMustBeGreaterThanServiceEndDate), Property = prop });
                }
                if (militaryService.ReserveStartDate == null)
                {
                    var prop = type.GetProperty("ReserveStartDate");
                    validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.RequiredMessage), Property = prop });
                }

                militaryService.HoldDate = null;
            }
            if (militaryService.Status == HRIS.Domain.Personnel.Enums.MilitaryStatus.Served)
            {
                var befor = jobApplication.RecruitmentMilitaryServices.Where(x => x.ServiceStartDate == militaryService.ServiceStartDate).ToList();

                if (befor.Count > 0 && operationType != CrudOperationType.Update)
                {
                    var prop = type.GetProperty("ServiceStartDate");
                    validationResults.Add(new ValidationResult() { Message = PersonnelLocalizationHelper.GetResource(PersonnelLocalizationHelper.MsYouCanNotAddDuplicateServiceStartDate), Property = prop });

                }

                militaryService.HoldDate = null;
                militaryService.ReserveStartDate = null;
            }

        }

        public override void BeforeInsert(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, string customInformation = null)
        {
            var jobApplication = ServiceFactory.ORMService.GetById<JobApplication>(requestInformation.NavigationInfo.Previous[0].RowId);
            var militaryService = (RecruitmentMilitaryService)entity;
            jobApplication.MilitaryStatus = militaryService.Status;

            MilitaryServiceStatusEffiected(militaryService);
        }

        public override void BeforeUpdate(RequestInformation requestInformation, Souccar.Domain.DomainModel.Entity entity, IDictionary<string, object> originalState, string customInformation = null)
        {
            var jobApplication = ServiceFactory.ORMService.GetById<JobApplication>(requestInformation.NavigationInfo.Previous[0].RowId);
            var militaryService = (RecruitmentMilitaryService)entity;
            jobApplication.MilitaryStatus = militaryService.Status;

            MilitaryServiceStatusEffiected(militaryService);
        }


        private void MilitaryServiceStatusEffiected(RecruitmentMilitaryService militaryService)
        {
            switch (militaryService.Status)
            {
                case MilitaryStatus.Nothing: //لا شيء
                case MilitaryStatus.NotObligedToServe: //وحيد
                case MilitaryStatus.Serving: //على رأس خدمته
                    militaryService.MilitiryServiceNo = null;//رقم خدمة العلم
                    militaryService.MilitiryServiceDocIssuance = null;//تاريخ إصدار وثيقة الخدمة
                    militaryService.Granter = null;//الجهة المصدرة لوثيقة الخدمة
                    militaryService.Years = 0;//السنوات
                    militaryService.Months = 0;//الأشهر
                    militaryService.Days = 0;//الأيام
                    militaryService.ServiceStartDate = null;//تاريخ بداية الخدمة
                    militaryService.ServiceEndDate = null;//تاريخ نهاية الخدمة
                    //militaryService.Notes = null;//ملاحظات
                    militaryService.IsPermamentExemption = false;//معفى دائم
                    militaryService.ExemptionReason = null;//سبب الإعفاء
                    militaryService.DateOfExemption = null;//تاريخ الإعفاء
                    militaryService.DelayReason = null;//سبب التأجيل
                    militaryService.DateOfDelay = null;//تاريخ التأجيل
                    militaryService.SendDelayExpirationNotification = false;//التنبيه في حال استحقاق تاريخ التأجيل
                    militaryService.HoldDate = null;//تاريخ الاحتفاظ
                    militaryService.ReserveStartDate = null;//تاريخ بداية الاحتياط          

                    break;
                case MilitaryStatus.Served: //أنهى خدمته 

                    militaryService.IsPermamentExemption = false;//معفى دائم
                    militaryService.ExemptionReason = null;//سبب الإعفاء
                    militaryService.DateOfExemption = null;//تاريخ الإعفاء
                    militaryService.DelayReason = null;//سبب التأجيل
                    militaryService.DateOfDelay = null;//تاريخ التأجيل
                    militaryService.SendDelayExpirationNotification = false;//التنبيه في حال استحقاق تاريخ التأجيل
                    militaryService.HoldDate = null;//تاريخ الاحتفاظ
                    militaryService.ReserveStartDate = null;//تاريخ بداية الاحتياط

                    break;
                case MilitaryStatus.Exempt: //معفى

                    militaryService.MilitiryServiceNo = null;//رقم خدمة العلم
                    militaryService.MilitiryServiceDocIssuance = null;//تاريخ إصدار وثيقة الخدمة
                    militaryService.Granter = null;//الجهة المصدرة لوثيقة الخدمة
                    militaryService.Years = 0;//السنوات
                    militaryService.Months = 0;//الأشهر
                    militaryService.Days = 0;//الأيام
                    militaryService.ServiceStartDate = null;//تاريخ بداية الخدمة
                    militaryService.ServiceEndDate = null;//تاريخ نهاية الخدمة
                    militaryService.DelayReason = null;//سبب التأجيل
                    militaryService.DateOfDelay = null;//تاريخ التأجيل
                    militaryService.SendDelayExpirationNotification = false;//التنبيه في حال استحقاق تاريخ التأجيل
                    militaryService.HoldDate = null;//تاريخ الاحتفاظ
                    militaryService.ReserveStartDate = null;//تاريخ بداية الاحتياط

                    break;
                case MilitaryStatus.Delayed: //مؤجل

                    militaryService.MilitiryServiceNo = null;//رقم خدمة العلم
                    militaryService.MilitiryServiceDocIssuance = null;//تاريخ إصدار وثيقة الخدمة
                    militaryService.Granter = null;//الجهة المصدرة لوثيقة الخدمة
                    militaryService.Years = 0;//السنوات
                    militaryService.Months = 0;//الأشهر
                    militaryService.Days = 0;//الأيام
                    militaryService.ServiceStartDate = null;//تاريخ بداية الخدمة
                    militaryService.ServiceEndDate = null;//تاريخ نهاية الخدمة
                    militaryService.IsPermamentExemption = false;//معفى دائم
                    militaryService.ExemptionReason = null;//سبب الإعفاء
                    militaryService.DateOfExemption = null;//تاريخ الإعفاء
                    militaryService.HoldDate = null;//تاريخ الاحتفاظ
                    militaryService.ReserveStartDate = null;//تاريخ بداية الاحتياط

                    break;
                case MilitaryStatus.Hold: //احتفاظ

                    militaryService.IsPermamentExemption = false;//معفى دائم
                    militaryService.ExemptionReason = null;//سبب الإعفاء
                    militaryService.DateOfExemption = null;//تاريخ الإعفاء
                    militaryService.DelayReason = null;//سبب التأجيل
                    militaryService.DateOfDelay = null;//تاريخ التأجيل
                    militaryService.SendDelayExpirationNotification = false;//التنبيه في حال استحقاق تاريخ التأجيل
                    militaryService.ReserveStartDate = null;//تاريخ بداية الاحتياط

                    break;
                case MilitaryStatus.Reserve: //احتياط

                    militaryService.IsPermamentExemption = false;//معفى دائم
                    militaryService.ExemptionReason = null;//سبب الإعفاء
                    militaryService.DateOfExemption = null;//تاريخ الإعفاء
                    militaryService.DelayReason = null;//سبب التأجيل
                    militaryService.DateOfDelay = null;//تاريخ التأجيل
                    militaryService.SendDelayExpirationNotification = false;//التنبيه في حال استحقاق تاريخ التأجيل
                    militaryService.HoldDate = null;//تاريخ الاحتفاظ

                    break;
                default:
                    break;
            }
        }
    }
}