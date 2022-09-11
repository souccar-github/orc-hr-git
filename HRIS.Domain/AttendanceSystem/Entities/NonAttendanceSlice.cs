using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace HRIS.Domain.AttendanceSystem.Entities
{
    public class NonAttendanceSlice : Entity // شرائح نموذج عدم التواجد ويشمل التأخر الصباحي والغياب خلال الدوام
    {

        public NonAttendanceSlice()
        {
            NonAttendanceSlicePercentages = new List<NonAttendanceSlicePercentage>();
        }


        [UserInterfaceParameter(Order = 1)]
        public virtual  NonAttendanceForm NonAttendanceForm { get; set; } //  نموذج التأخير الاب الذي يرتبط به هذه الشريحة

        [UserInterfaceParameter(Order = 1)]
        public virtual  int StartSlice { get; set; } // الحد الادنى للشريحة

        [UserInterfaceParameter(Order = 1)]
        public virtual  int EndSlice { get; set; } // الحد الاعلى لشريحة

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual  InfractionForm InfractionForm { get; set; } // المخالفة المرتبطة بالشريحة
        
        // في حال عدم وجود قيمة يتم ضرب النتائج مباشرة بالنسبة
        // أما في حال وجود وجود قيمة يتم تقريب النتائج الى القيمة المحددة ثم نضرب بالنسبة
        [UserInterfaceParameter(Order = 1)]
        public virtual  int Value { get; set; } // القيمة التي سيتم حسمها بالدقائق

        [UserInterfaceParameter(Order = 1)]
        public virtual  IList<NonAttendanceSlicePercentage> NonAttendanceSlicePercentages { get; set; } // النسب المرتبطة بشرائح التأخير
        public virtual void AddNonAttendanceSlicePercentage(NonAttendanceSlicePercentage nonAttendanceSlicePercentage)
        {
            nonAttendanceSlicePercentage.NonAttendanceSlice = this;
            NonAttendanceSlicePercentages.Add(nonAttendanceSlicePercentage);
        }
    }
}
