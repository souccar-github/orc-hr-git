namespace HRIS.Domain.PayrollSystem.Enums
{
    // الصيغ
    public enum Formula
    {
        FixedValue,  // قيمة ثابتة
        PercentageOfPackageSalary, //نسبة من حزم الراتب المحددة بالاعدادات
        PercentageOfSalary,  // نسبة من المقطوع
        PercentageOfInsuranceSalary,  // نسبة من الراتب التأميني
        PercentageOfBenefitSalary,  // نسبة من الراتب الاحتياطي1
        PercentageOfTempSalary1,  // نسبة من الراتب الاحتياطي2
        PercentageOfTempSalary2,  // نسبة من الراتب الاحتياطي3
        PercentageOfCategoryCeil,  // نسبة من سقف الفئة
        DaysOfPackageSalary, //أيام من حزم الراتب المحددة بالاعدادات
        DaysOfSalary,  //  أيام من المقطوع
        DaysOfInsuranceSalary,  //  أيام من الراتب التأميني
        DaysOfBenefitSalary,  //  أيام من الراتب الاحتياطي1
        DaysOfTempSalary1,  //  أيام من الراتب الاحتياطي2
        DaysOfTempSalary2,  //  أيام من الراتب الاحتياطي3
        DaysOfCategoryCeil,  //  أيام من سقف الفئة
        HoursOfPackageSalary, //ساعات من حزم الراتب المحددة بالاعدادات
        HoursOfSalary,  //  ساعات من المقطوع
        HoursOfInsuranceSalary,  //  ساعات من الراتب التأميني
        HoursOfBenefitSalary,  //  ساعات من الراتب الاحتياطي1
        HoursOfTempSalary1,  //  ساعات من الراتب الاحتياطي2
        HoursOfTempSalary2,  //  ساعات من الراتب الاحتياطي3
        HoursOfCategoryCeil,  //  ساعات من سقف الفئة
        Nothing
    }
}
