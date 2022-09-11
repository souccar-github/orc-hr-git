using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysHealthInsuranceModule
    {
        public const string ResourceGroupName = "CustomMessageKeysHealthInsuranceModule"; // 
        public const string MoreThanOneRowNotAllowed = "MoreThanOneRowNotAllowed"; // بعض الواجهات لا يسمح بوجود أكثر من ريكورد
        public const string RetiredSubscriptionReceiptNotPaid = "RetiredSubscriptionReceiptNotPaid";// ذمم الاشتراك للمتقاعد غير مسددة ولا يمكن صرف نفقة
        public const string CanNotEditPaidDebit = "CanNotEditPaidDebit";// لا يمكن تعديل ذمة مدفوعة
        public const string CrossDateConflict = "CrossDateConflict"; // في حال وجود تاريخ يتعارض مع فترة زمنية لسجل اخر
        public const string NoGlobalSettingsInDatabase = "NoGlobalSettingsInDatabase"; // في حال عدم وجود سجل الاعدادات العامة في الداتا بيز GlobalSetting Class
        public const string NoLoanSettingsInDatabase = "NoLoanSettingsInDatabase"; // في حال عدم وجود سجل الاعدادات للقروض في الداتا بيز والتي تشمل علامات ومعاملات القروض LoanSettings Class
        public const string ConflictSlices = "ConflictSlices"; // عند اضافة شرائح سنوات الخدمة يجب عدم وجود تداخلات بمجالات الشرائح
        public const string ThereAreunCalculatedDebits = "ThereAreunCalculatedDebits"; // عند تسديد اشتراك متقاعد يجب التأكد من عدم وجود ذمم غير محسوبة
        public const string ThereAreNoDebitsToPay = "ThereAreNoDebitsToPay"; // عند تسديد اشتراك متقاعد يجب التأكد من عدم وجود ذمم غير محسوبة
        public const string AdvanceOnlyAllowedForEmployees = "AdvanceOnlyAllowedForEmployees"; // يجب ان تكون الجهة المستلمة دائما موظف في حال طلب السلفة
        public const string LoanOnlyAllowedForEmployees = "LoanOnlyAllowedForEmployees"; // يجب ان تكون الجهة المستلمة دائما موظف في حال طلب القرض
        public const string EmployeeDoesNotHasBankAccount = "EmployeeDoesNotHasBankAccount"; // لا يمكن اعطاء سلفة للموظف اجرائية الدفع فيها تساوي تحويل للبنك وليس للموظف حساب بنكي
        public const string YouHaveToSetDoctorsUnionBankAccountOnGeneralSetting = "YouHaveToSetDoctorsUnionBankAccountOnGeneralSetting"; // في حال كانت النفقة مصروفة لنقابة الاطباء وتم اختيار طريقة الدفع تحويل للبنك ولم يكن للنقابة رقم حساب في الاعدادات العامة للضمان الصحي تظهر عندها هذه الرسالة
        public const string DriverOrInheritorDoesNotHasBankAccount = "DriverOrInheritorDoesNotHasBankAccount"; // في النفقة عند محاولة صرف النفقة لسائق او وريث وتم اختيار الية الصرف تحويل للبنك تظهر عندها هذه الرسالة

        public const string DoctorsUnionDoesNotHasBankAccount = "DoctorsUnionDoesNotHasBankAccount"; // نقابة الاطباء ليس لها حساب بنكي في الاعدادات
        public const string PaymentConflictWithWorkAccident = "PaymentConflictWithWorkAccident"; // لا يمكن ادخال سلفة او قرض خلال اصابة عمل لموظف
        
        public const string OnlyDraftCanBeAccepted = "OnlyDraftCanBeAccepted"; // عند محاولة تثبيت سلفة او نفقة او غير ذلك من التثبيتات وكانت الحالة ليست مسودة
        public const string OnlyDraftCanBeRejected = "OnlyDraftCanBeRejected"; // عند محاولة رفض القرض  وكانت الحالة ليست مسودة
        
        public const string BankTransferNotAllowdForDriverOrInheritor = "BankTransferNotAllowdForDriverOrInheritor"; //عند محاولة اضافة تحويل للبنك لسائق او وريث في صرف المدفوعات
        public const string NoPaymentDetailToAutoGenerate = "NoPaymentDetailToAutoGenerate"; // عند صرف مدفوعات عن طريق البنك ولم يتم توليد مدفوعات بسبب عدم تحقق الشروط يمنع اضافة المدفوعة
        public const string CannotAcceptChequeWithoutDetailsOrWithZeroAmountOfMoney = "CannotAcceptChequeWithoutDetailsOrWithZeroAmountOfMoney"; // محاولة تثبيت شيك بدون وجود تفاصيل للمدفوعة او اجمالي المبلغ المدفوع صفر
        public const string ChildProfitStartDateCannotBeGreaterThanDocumentDate = "ChildProfitStartDateCannotBeGreaterThanDocumentDate"; //تاريخ صدور الوثيقة المقدمة قبل استحقاق القريب للنفقات 
        public const string SpouseProfitStartDateCannotBeGreaterThanDocumentDate = "SpouseProfitStartDateCannotBeGreaterThanDocumentDate"; //تاريخ صدور الوثيقة المقدمة قبل استحقاق الزوجة للنفقات 

        public const string CannotFindChildProfitStartDate = "CannotFindChildProfitStartDate"; // عند محاولة معرفة تاريخ بداية الاستفادة من النفقات للطفل ولم نتمكن من الحصول على التاريخ تظهر رسالة تفيد انه لا يمكن اتمام العملية
        public const string CannotFindSpouseProfitStartDate = "CannotFindSpouseProfitStartDate"; // عند محاولة معرفة تاريخ بداية الاستفادة من النفقات للزوجة ولم نتمكن من الحصول على التاريخ تظهر رسالة تفيد انه لا يمكن اتمام العملية
        public const string CannotFindEmployeeHireDate = "CannotFindEmployeeHireDate"; // عند محاولة معرفة تاريخ التوظيف ولم نتمكن من الحصول على التاريخ تظهر رسالة تفيد انه لا يمكن اتمام العملية
        public const string ThisOperationMakesCashboxBalanceNegative = "ThisOperationMakesCashboxBalanceNegative"; // أي عملية دفع قد تؤدي الى رصيد سالب للصندوق يتم منهعا
        public const string CannotFindEmployeePrimaryCardInPayrollSystem = "CannotFindEmployeePrimaryCardInPayrollSystem"; // رسالة الخطأ التي تفيد انه يتم حساب علامات ومعاملات القروض لموظف ليس له بطاقة اساسية في نظام الرواتب
        
        public const string EmployeeStratExpenseProfitNotStartedYet = "EmployeeStratExpenseProfitNotStartedYet"; //تقديم الطلب عن فترة ما قبل الاستفادة من النفقات الصحية في حال طلب نفقة صحية
        public const string CannotAddExpenseInWorkAccidentPeriod = "CannotAddExpenseInWorkAccidentPeriod"; //لايمكن اضافة نفقة خلال فترة اصابة العمل
        public const string CannotSelectEmployeeForTransportationExpense = "CannotSelectEmployeeForTransportationExpense"; //اضافة مستفيد الى نفقة النقل
        public const string ExpenseAdvanceNeedAdvanceReturn = "ExpenseAdvanceNeedAdvanceReturn"; // في حال كانت النفقة لها سلفة بحاجة لاستراد سلفة
        public const string NoNurseryCostInDatabase = "NoNurseryCostInDatabase"; //
        public const string NoNurserySettingInDatabase = "NoNurserySettingInDatabase"; //
        public const string CannotGetProfitStartAndProfitEndDate = "CannotGetProfitStartAndProfitEndDate"; // عند اختيار الولد في احضانة يجب قراءة معلومات تاريخ البداية والنهاية وتظهر هذه الرسالة في حال حصول خطأ ما غير معروف
        public const string MoneyToPayCannotBeGreaterThanOrderedAmountOfMoney = "MoneyToPayCannotBeGreaterThanOrderedAmountOfMoney"; // عند التثبيت في السلفة لايمكن وضع قيمة الملبغ الموافق عليه اكبر من المطلوب

        public const string CannotSelectSpouseAndChildInOneToothReferralRecord = "CannotSelectSpouseAndChildInOneToothReferralRecord"; // لا يمكن عمل احالة سنية للولد او الزوجة بنفس الوقت بالتالي يجب اختيار احدهما

        public const string NurseryDetailRegistrationDateTheDayValueMustBeOne = "NurseryDetailRegistrationDateTheDayValueMustBeOne"; // بنود الحضانة تاريخ التسجيل يجب أن يكون اليوم فيها له القيمة واحد أي اول الشهر

        public const string NurseryDetailRegistrationDateEqualStartOrEndChildProfitDate = "NurseryDetailRegistrationDateEqualStartOrEndChildProfitDate";// رسالة التنبيه التي ستظهر في واجهة بنود الحضانة في حال كان تاريخ التسجيل يساوي تاريخ بدء الاستفادة من الحضانة او نهاية الاستفادة

        public const string CannotSelectSpouseAndChildInOneExpenseRecord = "CannotSelectSpouseAndChildInOneExpenseRecord";// لايمكن اختيار الزوجة والطفل معا في النفقة
        

        public const string MoneyToPayMustBeGreaterThanZero = "MoneyToPayMustBeGreaterThanZero"; // الرقم المطلوب دفعة في واجهة تثبيت السلفة يجب ان يكون اكبر او يساوي الصفر ومن المحتمل استخدام نفس المسج لواجهات التثبيت الاخرى

        public const string YouCanUpdateOnlyDraftRecord = "YouCanUpdateOnlyDraftRecord";// محاولة تعديل ريكورد حالته مثبت - مستخدم في النفقة والحضانة والقرض والسلفة


        public const string NurseryDetailRegistrationDateNotCorrespondWithChildProfitDates = "NurseryDetailRegistrationDateNotCorrespondWithChildProfitDates";// تاريخ تسجيل الحضانة للطفل يجب ان يكون أكبر تماما من تاريخ بدء الاستفادة واصغر تماما من تاريخ نهاية الاستفادة للطفل

        public const string Required = "Required"; // في حال وجود حقل اجباري يتم اختباره خارج الفريموورك الخاصة بالاختبار

        public const string AcceptChequeTitle = "AcceptChequeTitle"; // عنوان النافذة التي تظهر عند تثبيت شيك
        public const string RejectLoanTitle = "RejectLoanTitle"; // عنوان النافذة التي تظهر عند رفض قرض
        public const string AcceptLoanTitle = "AcceptLoanTitle"; // عنوان النافذة التي تظهر عند تثبيت قرض
        public const string AcceptNurseryTitle = "AcceptNurseryTitle"; // عنوان النافذة التي تظهر عند تثبيت حضانة
        public const string AcceptAdvanceTitle = "AcceptAdvanceTitle"; // عنوان النافذة التي تظهر عند تثبيت السلف
        public const string AcceptExpenseTitle = "AcceptExpenseTitle"; // عنوان النافذة التي تظهر عند تثبيت النفقة

        public const string CalculateLoanTitle = "CalculateLoanTitle"; // حساب القروض في جزء السيرفس
        public const string GenerateAnnualDebitsTitle = "GenerateAnnualDebitsTitle"; // سيرفس توليد الذمم السنوية
        public const string GenerateIncreasingAnnualDebitsTitle = "GenerateIncreasingAnnualDebitsTitle"; // سيرفس توليد ذمم زيادة الاشتراك
        public const string GenerateButtonTitle = "GenerateButtonTitle"; // زر توليد يتم ترجمته كلمة عامة لانه سيتم استخدامها في أكثر من مكان مثلا توليد الذمم السنوية او توليد زيادة الاشتراك الترجمة مثلا كلمة "توليد"
        


        public const string DebitsGenerated = "DebitsGenerated"; // عند توليد ذمم   تظهر رسالة تفيد أنه تم توليد  مع عدد الريكورد المولدة
        public const string LoansCalculated = "LoansCalculated"; // عند حساب القروض   تظهر رسالة تفيد أنه تم الحساب  مع عدد الريكورد المحسوبة

        

        public const string ChildProfitDateColumnTitle = "ChildProfitDateColumnTitle"; // عنوان الحقل تاريخ استحقاق الطفل للنفقات
        public const string SpouseProfitDateColumnTitle = "SpouseProfitDateColumnTitle"; // عنوان الحقل تاريخ استحقاق الزوجة للنفقات
        


        public const string ExpenseAdvanceValueColumnTitle = "ExpenseAdvanceValueColumnTitle"; // عنوان حقل مبلغ السلفة الموجود في واجهة النفقات
        public const string ExpenseEmployeeHireDateColumnTitle = "ExpenseEmployeeHireDateColumnTitle"; // عنوان حقل  تاريخ توظيف الموظف الموجود في واجهة النفقات وهو حقل للقراءة فقط

        public const string RetiredSubscriptionReceiptTotalDebitAmountOfMoneyColumnTitle = "RetiredSubscriptionReceiptTotalDebitAmountOfMoneyColumnTitle";// عنوان حقل اجمالي ذمم الموظف المترتبة عليه في واجهة تسديد اشتراك متقاعد
        

        // public const string EmployeeDoesNotHasHireDate = "EmployeeDoesNotHasHireDate"; //"لم يتم ادخال تاريخ الوظيف للموظف مسبقا - مستخدم في حساب القرض"
        
        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
