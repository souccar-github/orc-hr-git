using System;
using HRIS.Domain.PayrollSystem.Enums;
using System.Collections.Generic;
using HRIS.Domain.PayrollSystem.Configurations;

namespace HRIS.Domain.EmployeeRelationServices.DTO
{
    public class PayrollSystemIntegrationDTO
    {
        public double Value { get; set; } // قيمة الوقوعة مثلا 3 ايام او 4 ساعات ويتم التمييز بين الساعة واليوم وغيره من خلال الحقل التالي وهو الصيغة
        public Formula Formula { get; set; }// صيغة القيمة الحقل السابق القيمة التي فيه يتم تفسيرها في هذا الحقل مثلا بالقيمة يوجد 2 يتم تفسيرها في هذا الحقل ان الرقم 2 هو يوم او ساعات
        public double ExtraValue { get; set; } 
        public ExtraValueFormula ExtraValueFormula { get; set; }
        public int Repetition { get; set; }// تكرار العملية أي هل سيتم حسم العقوبة عن شهر او شهرين او اكثر ومن اجل الادارية مثلا تكون القيمة دائما واحد اي حسم لمرة واحدة فقط
        public virtual IList<int> SourceId { get; set; }
        public virtual int SourceType { get; set; }
        public virtual DeductionCard DeductionCard { get; set; }
        public virtual string Note { get; set; }
    }
}
