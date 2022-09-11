namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysAttendanceSystemModule
    {
        public const string ResourceGroupName = "CustomMessageKeysAttendanceSystemModule";

        public const string EntryTimeCannotBeLessThanShiftRangeStartTime = "EntryTimeCannotBeLessThanShiftRangeStartTime";// لايمكن أن يكون وقت الدخول اصغر من الحد الادنى للفترة
        public const string RestRangeStartTimeCannotBeLessThanEntryTime = "RestRangeStartTimeCannotBeLessThanEntryTime";//  لايمكن أن تكون فترة الاستراحة أصغر من وقت الدخول
        public const string RestRangeEndTimeCannotBeLessThanRestRangeStartTime = "RestRangeEndTimeCannotBeLessThanRestRangeStartTime";//لا يمكن ان تكون بداية الاستراحة أكبر من نهاية الاستراحة
        public const string ExitTimeCannotBeLessThanRestRangeEndTime = "ExitTimeCannotBeLessThanRestRangeEndTime";// لا يمكن أن تكون نهاية الاستراحة أكبر من  وقت الخروج
        public const string ShiftRangeEndTimeCannotBeLessThanExitTime = "ShiftRangeEndTimeCannotBeLessThanExitTime";// لا يمكن أن يكون وقت الخروج أكبر من الحد الاعلى للفترة
        public const string NormalShiftConflictWithOtherNormalShiftsInThisWorkshop = "NormalShiftConflictWithOtherNormalShiftsInThisWorkshop"; // الفترة المدخلة تتقاطع مع فترات اخرى في نفس الوردية

        public const string ParticularOvertimeShiftEndTimeCannotBeLessThanStartTime = "ParticularOvertimeShiftEndTimeCannotBeLessThanStartTime";// لايمكن أن يكون وقت الانتهاء اصغر من وقت البدء للفترة الخاصة للوردية
        public const string ParticularOvertimeShiftMustBeInAnyNormalShiftRanges = "ParticularOvertimeShiftMustBeInAnyNormalShiftRanges"; // الفترة الخاصة المدخلة لا تنتمي لأاي مجال من مجالات الفترات العادية للوردية - يجب ان تكون ضمن الحد الادنى والاعلى للوردية
        public const string ParticularOvertimeShiftConflictWithOtherParticularOvertimeShiftsInThisWorkshop = "ParticularOvertimeShiftConflictWithOtherParticularOvertimeShiftsInThisWorkshop";// الفترة الخاصة للوردية المدخلة تتقاطع مع فترات اخرى في نفس الوردية

        public const string TemporaryWorkshopConflictWithOtherTemporaryWorkshopsInThisWorkshop = "TemporaryWorkshopConflictWithOtherTemporaryWorkshopsInThisWorkshop";// الوردية الاستثنائية المدخلة تتقاطع مع ورديات استثنائية اخرى في نفس الوردية

        public const string NonAttendanceSliceConflictWithOtherNonAttendanceSlicesInThisNonAttendanceForm = "NonAttendanceSliceConflictWithOtherNonAttendanceSlicesInThisNonAttendanceForm";// شريحة عدم التواجد المدخلة تتقاطع مع شريحة اخرى في نفس النموذج

        public const string InfractionSliceConflictWithOtherInfractionSlicesInThisInfractionForm = "InfractionSliceConflictWithOtherInfractionSlicesInThisInfractionForm";// شريحة المخالفة المدخلة تتقاطع مع شريحة اخرى في نفس النموذج

        public const string OvertimeSliceConflictWithOtherOvertimeSlicesInThisOvertimeForm = "OvertimeSliceConflictWithOtherOvertimeSlicesInThisOvertimeForm";// شريحة نموذج الاضافي المدخلة تتقاطع مع شريحة اخرى في نفس النموذج

        public const string AutoGenerateAttendanceRecords = "AutoGenerateAttendanceRecords";// زر توليد ريكوردات تسجيل الدخول والخروج للموظفين
        public const string FromDateCannotBeGreaterThanToDate = "FromDateCannotBeGreaterThanToDate";// رسالة الخطأ من تاريخ لا يمكن ان تكون أكبر من الى تاريخ في واجهة السيرفيس الخاصة بالتوليد الالي لتسجيلات دخول وخروج الموظفين
        public const string SomeEmployeesDoNotHaveWorkshopRecurrence = "SomeEmployeesDoNotHaveWorkshopRecurrence";// عند محاولة توليد تلقائي لدوام الموظفين ووجود بعض الموظفين بدون تواترات للوردية في نموذج دوامهم
        public const string AutoGenerateAttendanceRecordNoteIsRequired = "AutoGenerateAttendanceRecordNoteIsRequired";// واجهة التوليد التلقائي لدوام الموظفين حقل الملاحظة فيها اجباري
        public const string AutoGenerateAttendanceRecordToDateMustBeGreaterThanEqualToFromDate = "AutoGenerateAttendanceRecordToDateMustBeGreaterThanEqualToFromDate";// واجهة التوليد التلقائي لدوام الموظفين حقل الى تاريخ يجب ان يكون اكبر او يساوي حقل من تاريخ

        public const string CannotCalculateLockedOrCreatedAttendanceRecords = "CannotCalculateLockedOrCreatedAttendanceRecords";//في حال محاولة حساب دوام شهر حالته مقفل او غير مولد
        public const string CannotGenerateLockedAttendanceRecord = "CannotGenerateLockedAttendanceRecord";// لايمكن إعادة توليد شهر مقفل
        //public const string CheckAttendanceRecordStabilityFaild = "CheckAttendanceRecordStabilityFaild";// عند محاولة حساب دوام شهر مع وجود عدم تحقق لشروط الحساب من شروط الحساب ان تكون جميع تسجيلات الدخول والخروج بحالة موافق عليها وان يكون كل دخول في السجل خروج موافق له وان يكون لكل نموذج سيتم استخدامه الشرائح الموافقة له يرجى الرجوع الى الميثود 
        // AttendanceService.CheckAttendanceRecordStability()
        //        لتأكيد باقي شروط الحساب

        public const string OnlyCalculatedAttendanceRecordCanBeLocked = "OnlyCalculatedAttendanceRecordCanBeLocked";// في حال محاولة قفل سجل دوام غير محسوب

        public const string ConflictSlices = "ConflictSlices";// في حال وجود تعارض بالمجالات مع ريكوردات اخرى مستخدم حاليا في تطليف الاضافي بحيث يمكن وجود تكليفين يتعارضان بالتاريخ

        public const string ImportEntranceExitRecordsFromExcel = "ImportEntranceExitRecordsFromExcel";

        public const string AttendanceRecordNameMustBeUnique = "AttendanceRecordNameMustBeUnique";
        public const string AttendanceRecordYearAndMonthMustBeUnique = "AttendanceRecordYearAndMonthMustBeUnique";

        public const string MonthCanNotBeNothing = "MonthCanNotBeNothing";
        public const string AttendanceCardGenerated = "AttendanceCardGenerated";//  رسالة تفيد بعدد سجلات الدوام المولدة


        public const string GenerateTitle = "GenerateTitle";// زر توليد سجلات الدوام في الفلتر
        public const string LockTitle = "LockTitle";// زر قفل سجل الدوام
        public const string CalculateTitle = "CalculateTitle";// زر حساب الدوام في سجلات الدوام  

        public const string SetValuesToAllFieldsBeforeAutoGeneratingRecords = "SetValuesToAllFieldsBeforeAutoGeneratingRecords";// في سيرفيس التوليد الالي لسجلات الدوام لابد من ادخال الحقول من تاريخ والى تاريخ وحقل الملاحظة لنتمكن من متابعة العملية  ومستخدمة أيضا في سيرفس توليد ذمم زيادة الاشتراك في الضمان الصحي

        public const string EmployeeDoesNotHasEmployeeAttendanceCard = "EmployeeDoesNotHasEmployeeAttendanceCard";// اي عملية على الموظف تتطلب اختيار موظف يتم منع العملية لعدم ربط الموظف ببطاقة دوام - مثل اضافة دخول او خروج للموظف بشكل يدوي

        public const string BioMetricInteraction = "BioMetricInteraction";// اسم واجهة السيرفس التي سيتم التخاطب من خلالها مع جهاز البصمة مثل حذف السجلات استيراد السجلات تصدير الى قاعدة البيانات



        public const string SyncDevicesButton = "SyncDevicesButton";// زر تحديث الاجهزة المدعومة في واجهة اعدادات الاجهزة



        public const string PerformBioMetricInteractionButtonTitle = "PerformBioMetricInteractionButtonTitle";// زر التنفيذ في واجهة السيرفس الخاصة بالتخاطب مع اجهزة البصمة
        public const string BioMetricInteractionSupportedDevicesTitle = "BioMetricInteractionSupportedDevicesTitle";// عنوان القائمة التي تحوي الاجهزة المدعوة في واجهة التعامل مع أجهزة البصمة
        public const string BioMetricInteractionTransferDataFromBioMetricTitle = "BioMetricInteractionTransferDataFromBioMetricTitle";// التشيك الخاص بخيار ترحيل البيانات من جهاز البصمة الى قاعدة البيانات
        public const string BioMetricInteractionClearDataFromBioMetricTitle = "BioMetricInteractionClearDataFromBioMetricTitle";// التشيك الخاص بخيار حذف جميع سجلات جهاز البصمة
        public const string SelectBioMetricDeviceBeforeExecuteMessage = "SelectBioMetricDeviceBeforeExecuteMessage";// الرسالة التي ستظهر عند تنفيذ العمليات على جهاز البصمة بدون اختيار الجهاز المطلوب
        public const string ChooseBioMetricActionsDeviceBeforeExecuteMessage = "ChooseBioMetricActionsDeviceBeforeExecuteMessage";// الرسالة التي ستظهر عند تنفيذ العمليات على جهاز البصمة بدون اختيار أي عملية لتنفيذها

        //public const string AcceptEntranceExitRecords = "AcceptEntranceExitRecords";
        //public const string EntranceExitRecordsAccepted = "EntranceExitRecordsAccepted";
        public const string EntranceExitRecordsDeleted = "EntranceExitRecordsDeleted";
        public const string ExitTimeCannotBeLessThanEntryTime = "ExitTimeCannotBeLessThanEntryTime";
        public const string ShiftRangeEndTimeCannotBeLessThanShiftRangeStartTime = "ShiftRangeEndTimeCannotBeLessThanShiftRangeStartTime";
        public const string DeleteEntranceExitRecords = "DeleteEntranceExitRecords";
        public const string CheckEntranceExitRecords = "CheckEntranceExitRecords";
        public const string EntranceExitRecordAlreadyExist = "EntranceExitRecordAlreadyExist";
        public const string GenerateEntranceExitRecordError = "GenerateEntranceExitRecordError";
        public const string EntranceExitRecordErrorsGenerated = "EntranceExitRecordErrorsGenerated";
        public const string CheckAllRangesAndTheShiftMastBeDuringOneDay = "CheckAllRangesAndTheShiftMastBeDuringOneDay";

        public const string shiftRangeStartTimeCannotBeLessThanEntryTime = "shiftRangeStartTimeCannotBeLessThanEntryTime";
        public const string EntryTimeCannotBeLessThanRangeStartTime = "EntryTimeCannotBeLessThanRangeStartTime";
        public const string RestRangeStartTimeCannotBeLessThanRestRangeEndTime = "RestRangeStartTimeCannotBeLessThanRestRangeEndTime";
        public const string TheShiftMastBeDuringOneDay = "TheShiftMastBeDuringOneDay";
        public const string CheckEntranceExitRecordStatusForThisMonth = "CheckEntranceExitRecordStatusForThisMonth";
        public const string CheckEntranceExitRecordsMustBePairOfRecords = "CheckEntranceExitRecordsMustBePairOfRecords";
        public const string MustBePairOfRecords = "MustBePairOfRecords";
        public const string MustAddGeneralSettings = "MustAddGeneralSettings";
        public const string UpdateReasonIsRequired = "UpdateReasonIsRequired";
        public const string RestRangeOutOfShiftRange = "RestRangeOutOfShiftRange";


        public const string CheckEntranceExitRecordsConsistencyFailed = "CheckEntranceExitRecordsConsistencyFailed";
        public const string ThereAreWrongRecordsPleaseCorrectItAndCheckAgain = "ThereAreWrongRecordsPleaseCorrectItAndCheckAgain";
        public const string OutOfRange = "OutOfRange";
        public const string EntrancewithoutExit = "EntrancewithoutExit";
        public const string ExitwithoutEntrance = "ExitwithoutEntrance";
        public const string MultipleExit = "MultipleExit";
        public const string MultipleEntrance = "MultipleEntrance";
        public const string EntranceGreaterThanExit = "EntranceGreaterThanExit";


        public const string ThereAreAnotherShiftWithThisOrder = "ThereAreAnotherShiftWithThisOrder";
        public const string YouMustAddOneOvertimeSliceAtLeast = "YouMustAddOneOvertimeSliceAtLeast";
        public const string YouMustAddOneWorkshopRecurrencesAtLeast = "YouMustAddOneWorkshopRecurrencesAtLeast";
        public const string YouMustAddOneInfractionSliceAtLeast = "YouMustAddOneInfractionSliceAtLeast";
        public const string YouMustAddOneNormalShiftAtLeast = "YouMustAddOneNormalShiftAtLeast";
        public const string YouMustAddOneNonAttendanceSliceAtLeast = "YouMustAddOneNonAttendanceSliceAtLeast";
        public const string YouMustAddOneNonAttendanceSlicePercentageAtLeast = "YouMustAddOneNonAttendanceSlicePercentageAtLeast";
        public const string ThereAreAnotherWorkshopWithThisNumber = "ThereAreAnotherWorkshopWithThisNumber";
        public const string ThereAreAnotherAttendanceTemplateWithThisNumber = "ThereAreAnotherAttendanceTemplateWithThisNumber";
        public const string ThereAreAnotherRecurrenceWithThisOrder = "ThereAreAnotherRecurrenceWithThisOrder";
        public const string ThereAreAnotherInfractionTemplateWithThisNumber = "ThereAreAnotherInfractionTemplateWithThisNumber";
        public const string ThereAreAnotherInfractionTemplateWithThisName = "ThereAreAnotherInfractionTemplateWithThisName";
        public const string ThereAreAnotherOverTimeTemplateWithThisNumber = "ThereAreAnotherOverTimeTemplateWithThisNumber";
        public const string TheRangeOfEntryTimeAndExitTimeMustBeWithinTheRangeOfMinimumTimeOfEntryAndMaximumTimeOfExit = "TheRangeOfEntryTimeAndExitTimeMustBeWithinTheRangeOfMinimumTimeOfEntryAndMaximumTimeOfExit";
        public const string TheRecurrenceOrderMustBeLessOrEqualThan31 = "TheRecurrenceOrderMustBeLessOrEqualThan31";
        public const string StartSliceValueMustBeLessOrEqualThanEndSliceValue = "StartSliceValueMustBeLessOrEqualThanEndSliceValue";
        public const string NonAttendanceSlicePercentageOrderMustBeUnique = "NonAttendanceSlicePercentageOrderMustBeUnique";
        public const string ThereAreAnotherNonAttendanceTemplateWithThisNumber = "ThereAreAnotherNonAttendanceTemplateWithThisNumber";
        public const string ParticularOvertimeShiftMustBelongToTheRangeFromTheMinimumEntryTimeToEntryTimeOrTheRangeFromExitTimeToTheMaximumExitTime = "ParticularOvertimeShiftMustBelongToTheRangeFromTheMinimumEntryTimeToEntryTimeOrTheRangeFromExitTimeToTheMaximumExitTime";
        public const string ParticularOvertimeShiftShouldBeNotCrossedWithOther = "ParticularOvertimeShiftShouldBeNotCrossedWithOther";
        public const string TheMonthNameIsAlreadyExists = "TheMonthNameIsAlreadyExists";

        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
