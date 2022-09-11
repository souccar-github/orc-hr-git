using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Souccar.Core.Extensions;
using System.Web;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class PayrollLocalizationHelper
    {
        public const string ResourceGroupName = "CustomMessageKeysPayrollSystemModule";

        public const string ParentCannotEqualToObjectItself = "ParentCannotEqualToObjectItself";// لا يجوز ان يكون الاب لتعويض هو نفس التعويض وكذلك بالنسبة للحسم
        public const string PrePayedAndMonthInstalmentMustBeLessLoanAmount = "PrePayedAndMonthInstalmentMustBeLessLoanAmount";// الدفعة الاولى وقيمة القسط يجب ان تكون اصغر او تساوي القيمة الاجمالية للقرض
        public const string TotalPaymentsCannotExceedTotalLoanValue = "TotalPaymentsCannotExceedTotalLoanValue";// اجمالي الدفعات مع الدفعة الاولى يجب ان لايتجاوز قيمة القرض الاجمالية
        public const string MoreThanOneRowNotAllowed = "MoreThanOneRowNotAllowed";// لايسمح باضافة أكثر من ريكورد في هذه الواجهة مثال الخيارات العامة
        public const string ConflictSlices = "ConflictSlices";//  تداخل مجال الشريحة مع شريحة اخرى
        public const string NoGeneralOptionInDatabase = "NoGeneralOptionInDatabase"; // في حال عدم وجود سجل الاعدادات العامة في الداتا بيز GeneralSettings Class
        //public const string NoTaxSlicesInDatabase = "NoTaxSlicesInDatabase"; // في حال عدم وجود سجلات شرائح الضريبة  في الداتا بيز Tax Slices Class

        public const string NoFamilyBenefitOptionInDatabase = "NoFamilyBenefitOptionInDatabase"; // الواجهات التي تعتمد على خيارات التعويض العائلي يتم اظهار رسالة خطأ تفيد بضرورة اضافة  الخيارات الى النظام مثل حالة توليد الشهر والبطاقات الشهرية
        public const string ConflictOption = "ConflictOption";

        public const string Required = "Required"; // في حال وجود حقل اجباري يتم اختباره خارج الفريموورك الخاصة بالاختبار
        public const string CannotGenerateLockedOrApprovedMonths = "CannotGenerateLockedOrApprovedMonths"; // لا يمكن توليد الاشهر المقفلة او الموافق عليها
        public const string CannotCalculateLockedOrApprovedOrCreatedMonths = "CannotCalculateLockedOrApprovedOrCreatedMonths"; // لا يمكن حساب الاشهر المقفلة او المواف عليها او الجديدة والغير مولدة بعد
        public const string CannotRejectOnlyCalculatedMonthsCanBeRejected = "CannotRejectOnlyCalculatedMonthsCanBeRejected"; // لا يمكن رفض سوى الاشهر المحسوبة
        public const string CannotApproveOnlyCalculatedMonthsCanBeApproved = "CannotApproveOnlyCalculatedMonthsCanBeApproved"; // لا يمكن قبول سوا الاشهر المحسوبة
        public const string CannotLockOnlyApprovedMonthsCanBeLocked = "CannotLockOnlyApprovedMonthsCanBeLocked"; //لا يمكن قفل سوا الاشهر الموافق عليها
        public const string CannotAddLoanPaymentToApprovedOrLockedMonth = "CannotAddLoanPaymentToApprovedOrLockedMonth"; //لا يمكن اضافة دفعة شهرية من خلال بطاقة اساسية لبطاقة شهرية تابعة لشهر مقفل او موافق عليه
        public const string NoTravelLicenceOptionInDatabase = "NoTravelLicenceOptionInDatabase"; // في حال عدم وجود سجل الاعدادات لأذونات السفر في الداتا بيز  LoanSettings Class
        public const string OnlyDraftCanBeAccepted = "OnlyDraftCanBeAccepted"; // عند محاولة تثبيت ريكورد ليس بحالة مسودة
        public const string OnlyAcceptedTravelLicenceCanBePaid = "OnlyAcceptedTravelLicenceCanBePaid"; // يمكن عمل امر صرف فقط لاذونات السفر المثبتة

        public const string CannotGenerateAcceptedSalaryIncreaseOrdinance = "CannotGenerateAcceptedSalaryIncreaseOrdinance"; // لا يمكن توليد موظفين في واجهة زيادة الرواتب بموجب مرسوم حالته مثبت


        public const string CannotCalculateAcceptedSalaryIncreaseOrdinance = "CannotCalculateAcceptedSalaryIncreaseOrdinance"; // لايمكن احتساب زيادات الرواتب بعد تثبيتها
        public const string OnlyDraftSalaryIncreaseOrdinanceCanBeAccepted = "OnlyDraftSalaryIncreaseOrdinanceCanBeAccepted"; // لايمكن تثبيت زيادة الرواتب بموجب مرسوم الا اذا كانت حالة الريكورد مسودة
        public const string YouHaveToCalculateSalaryIncreaseBeforeAcceptTheRecord = "YouHaveToCalculateSalaryIncreaseBeforeAcceptTheRecord"; // لايمكن تثبيت الزيادات على الرواتب بدون القيام بعملية الحساب للريكورد اي يشترط عدم وجود اي موظف راتبه بعد الزيادة يساوي الصفر


        public const string TotalRestDaysMustBeLessOrEqualActualTransferenceDays = "TotalRestDaysMustBeLessOrEqualActualTransferenceDays"; // عدد الايام التي سيتم تقديم عنها المبيت بالداخلي والاخارجي يجب ان يكون اصغر من عدد الايام الفعلي المحسوب
        public const string TotalFoodDaysMustBeLessOrEqualActualTransferenceDays = "TotalFoodDaysMustBeLessOrEqualActualTransferenceDays"; // عدد الايام التي سيتم تقديم عنها الطعام بالداخلي والاخارجي يجب ان يكون اصغر من عدد الايام الفعلي المحسوب
        public const string CannotDoActionsOnLockedOrApprovedMonths = "CannotDoActionsOnLockedOrApprovedMonths"; //اي عملية تتم على الشهور المقفلة او الموافق عليها ممنوعة

        public const string GenerateTitle = "GenerateTitle"; // توليد الشهر
        public const string CalculateTitle = "CalculateTitle"; // حساب الشهر
        public const string RejectTitle = "RejectTitle"; // رفض الشهر
        public const string ApproveTitle = "ApproveTitle"; // تثبيت الشهر
        public const string LockTitle = "LockTitle"; // قفل الشهر

        public const string AcceptTitle = "AcceptTitle"; // زر التثبيت الموجود بمعظم الاكشن لست
        public const string MonthlyCardGenerated = "MonthlyCardGenerated"; // عند توليد البطاقات الشهرية في الشهر تظهر رسالة تفيد أنه تم توليد البطاقات الشهرية مع عدد البطاقات المولدة


        public const string PerformAuditedTitle = "PerformAuditedTitle";// عنوان الاكشن ليست مدقق
        public const string CancelAuditedTitle = "CancelAuditedTitle"; //  عنوان الاكشن ليست الغاء التدقيق

        public const string SalaryIncreaseOrdinanceGenerated = "SalaryIncreaseOrdinanceGenerated"; // الرسالة التي ستظهر عند توليد موظفي زيادة الراتب بموجب مرسوم
        

        public const string AddBenefitToEmployees = "AddBenefitToEmployees"; //اضافة تعويض لمجموعة موظفين 
        public const string AddDeductionToEmployees = "AddDeductionToEmployees"; //اضافة حسم لمجموعة موظفين 
        public const string ForMonthlyCardsCheckboxTitle = "ForMonthlyCardsCheckboxTitle"; //الشيك بوكس الموجود في اضافة حسم او تعويض لمجموعة موظفين 
        public const string CopySalariesServiceTitle = "CopySalariesServiceTitle"; // خدمة نسخ الرواتب في البطاقة الاساسية 

        public const string CopySalariesServiceGridButtonTitle = "CopySalariesServiceGridButtonTitle"; // زر النسخ في الغريد في واجهة عملية نسخ راتب الى راتب
        public const string FromSalaryTitle = "FromSalaryTitle"; // من الراتب في واجهة النسخ
        public const string ToSalaryTitle = "ToSalaryTitle"; // الى الراتب في واجهة النسخ

        public const string MonthlyCardIsRequired = "MonthlyCardIsRequired"; // اختيار بطاقة شهرية للموظف اجباري
        public const string CannotSelectParentBenefit = "CannotSelectParentBenefit"; // لايمكن استخدام تعويض واعطاءه للموظف في حال كان هذا التعويض هو تعويض اب لتعويضات اخرى


        public const string ParentBenefitCardBrokenParentsChain = "ParentBenefitCardBrokenParentsChain"; // الهدف هو اختبار ان التعويض الحالي هو ليس اب لاي من التعويضات التي أعلى منه حتى لايكون لدينا سلسلة من نوع اب يشير الى ابن وابن يشير الى اب - 

        public const string CannotUseBenefitCardAsParentBecauseItsUsedAsNormalBenefit = "CannotUseBenefitCardAsParentBecauseItsUsedAsNormalBenefit"; // لا يمكن استخدام هذا التعويض كتعويض أب في واجهة بطاقة التعويض لأان التعويض الاب المختار قد تم استخدامه مسبقا كتعويض عادي اي تعويض في البطاقات الاساسية للموظفين او التغييرات الشهرية او البطاقات الشهرية


        public const string AllEmployeesWithPrimaryCardMustHavePosition = "AllEmployeesWithPrimaryCardMustHavePosition"; // عند الفلترة حسب اي فلتر من الفلاتر يحتاج الوصول الى البوزيشن للموظف سيتم منع العملية اذا لم يكون جميع الموظفين الذين لهم بطاقات مالية ليس لديهم بوزيشن


        public const string FilterByEmployeeTitle = "FilterByEmployeeTitle";// عنوان زر الراديو في مجموعة الفلاتر - الفلترة حسب الموظفين
        public const string FilterByPrimaryCardTitle = "FilterByPrimaryCardTitle";// عنوان زر الراديو في مجموعة الفلاتر - الفلترة حسب البطاقات المالية الاساسية
        public const string FilterByGradeTitle = "FilterByGradeTitle";// Grade عنوان زر الراديو في مجموعة الفلاتر - الفلترة حسب
        public const string FilterByJobTitleTitle = "FilterByJobTitleTitle";// JobTitle عنوان زر الراديو في مجموعة الفلاتر - الفلترة حسب 
        public const string FilterByJobDescriptionTitle = "FilterByJobDescriptionTitle";// JobDescription عنوان زر الراديو في مجموعة الفلاتر - الفلترة حسب 
        public const string FilterByPositionTitle = "FilterByPositionTitle";// Position عنوان زر الراديو في مجموعة الفلاتر - الفلترة حسب 
        public const string FilterByNodeTitle = "FilterByNodeTitle";// Node عنوان زر الراديو في مجموعة الفلاتر - الفلترة حسب 
        public const string FilterByMajorTypeTitle = "FilterByMajorTypeTitle";// MajorType عنوان زر الراديو في مجموعة الفلاتر - الفلترة حسب 
        public const string FilterByMajorTitle = "FilterByMajorTitle";// MajorType عنوان زر الراديو في مجموعة الفلاتر - الفلترة حسب 

        public const string CannotCreateNewMonthWhileNotAllPreviousMonthsNotLocked = "CannotCreateNewMonthWhileNotAllPreviousMonthsNotLocked";// لا يمكن توليد شهر جديد من نوع اجور وتعويض في حال الشهر السابق غير مقفل مفيد لحالة الارتباط مع وقوعات العمل بحيث نضمن انه لا يتم الاستيرات مرتين على اعتبار ان تثبيت استيراد الوقوعات يتم على قفل الشهر

        public const string RecordAlreadyAudited = "RecordAlreadyAudited";// عند محاولة تدقيق ريكورد مدقق تظهر رسالة تفيد ان الريكورد مدقق ولا يمكن اتمام العملية
        public const string RecordAlreadyNotAudited = "RecordAlreadyNotAudited";// عند محاولة الغاء تدقيق ريكورد غير مدقق تظهر رسالة تفيد ان الريكورد غير مدقق ولا يمكن اتمام العملية


        public const string EmployeeFamilyDeserve = "EmployeeFamilyDeserve";// عنوان استحقاق التعويض العائلي في الظيانات الرئيسية

        public const string SetValuesToAllNumericFieldsBeforeExecution = "SetValuesToAllNumericFieldsBeforeExecution";// رسالة عامة في أكثر من واجهة تفيد انه يجب ادخال الحقول الرقمية وعدم تركها فارغة مثل واجهة اضافة تعويض لمجموعة موظفين او حسم لمجموعة موظفين



        public const string GenerateSalaryIncreaseTitle = "GenerateSalaryIncreaseTitle";//زر التوليد في الاكشن ليست في واجهة الزيادة وفق مرسوم
        public const string CalculateSalaryIncreaseTitle = "CalculateSalaryIncreaseTitle";//زر حساب في الاكشن ليست في واجهة الزيادة وفق مرسوم


        public const string AutoGeneratedMsg = "AutoGeneratedMsg";// عند التوليد يتم اضافة جملة مولدة أليا أو اي عبارة اخرى تعبر عند التوليد الالي للسجل والمسج عامة وليست محددة لواجهة 


        public const string ImportedFromEmployeeRelationService = "ImportedFromEmployeeRelationService"; // عند استيراد البيانات من علاقات العمل نضيف الى حقل الملاحظات عبارة تفيد ان السجل تم استيراده من علاقات العمل
        public const string ImportedFromAttendance = "ImportedFromAttendance"; // عند استيراد البيانات من الدوام نضيف الى حقل الملاحظات عبارة تفيد ان السجل تم استيراده من الدوام



        public const string SelectedCountryDoesNotHasTravelCategory = "SelectedCountryDoesNotHasTravelCategory"; // في اذن السفر الخارجي في حال تم اختيار بلد ليس له فئة اغتراب تظهر هذه الرسالة


        public const string YouCanOnlySetIncreaseValueOrIncreasePercentage = "YouCanOnlySetIncreaseValueOrIncreasePercentage"; // واجهة الزيادة للراتب وفق مرسوم لايمكن كتابة نسبة الزيادة وقيمة الزيادة معا انما يجب كتابة النسبة او القيمة

        public const string TravelLicenceAllreadyPaid = "TravelLicenceAllreadyPaid"; // في حال محاولة اعادة صرف اذن السفر سواء الداخلي او الخارجي


        public const string CannotSelectCustomCrossTypeWithNothingCrossFormula = "CannotSelectCustomCrossTypeWithNothingCrossFormula";// في واجهة التقاطع لايمكن اختيار تقاطع مخصص مع الصيغة لاشيء

        public const string MsgArrivelTimeMustBeGreaterThanLeavingTime = "MsgArrivelTimeMustBeGreaterThanLeavingTime";// في واجهة اذن السفر يجب ان يكون وقت الوصول اكبر من وقت المغادرة

        public const string MsgDestinationArrivelTimeMustBeGreaterThanInternalLeavingTime = "MsgDestinationArrivelTimeMustBeGreaterThanInternalLeavingTime";

        public const string MsgInternalArrivelTimeMustBeGreaterThanDestinationLeavingTime = "MsgInternalArrivelTimeMustBeGreaterThanDestinationLeavingTime";
        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

        public const string YouCannotEditLockedMonth = "YouCannotEditLockedMonth";
    }
}