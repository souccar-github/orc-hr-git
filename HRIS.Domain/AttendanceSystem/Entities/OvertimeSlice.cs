using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace HRIS.Domain.AttendanceSystem.Entities
{
    [Details(IsDetailHidden = false)]
    public class OvertimeSlice : Entity  //  شرائح نموذج الاضافي
    {

        [UserInterfaceParameter(Order = 1)]
        public virtual  OvertimeForm OvertimeForm { get; set; } // نموذج الاضافي الذي ترتبط به هذه الشريحة

        [UserInterfaceParameter(Order = 1)]
        public virtual  int StartSlice { get; set; } // الحد الادنى للشريحة

        [UserInterfaceParameter(Order = 1)]
        public virtual  int EndSlice { get; set; } // الحد الاعلى للشريحة

        // في حال عدم وجود قيمة يتم ضرب النتائج مباشرة بالنسبة
        // أما في حال وجود وجود قيمة يتم تقريب النتائج الى القيمة المحددة ثم نضرب بالنسبة
        [UserInterfaceParameter(Order = 1)]
        public virtual  int NormalPercentage { get; set; }// نسبة الاضافي العادي

        [UserInterfaceParameter(Order = 1)]
        public virtual  int NormalValue { get; set; }// قيمة الاضافي العادي

        [UserInterfaceParameter(Order = 1)]
        public virtual  int HolidayPercentage { get; set; } // نسبة اضافي العطل

        [UserInterfaceParameter(Order = 1)]
        public virtual  int HolidayValue { get; set; } // قيمة اضافي العطل

        [UserInterfaceParameter(Order = 1)]
        public virtual  int ParticularShiftPercentage { get; set; } // نسبة اضافي الفترة الخاصة

        [UserInterfaceParameter(Order = 1)]
        public virtual  int ParticularShiftValue { get; set; } //قمية اضافي الفترة الخاصة
    }
}
