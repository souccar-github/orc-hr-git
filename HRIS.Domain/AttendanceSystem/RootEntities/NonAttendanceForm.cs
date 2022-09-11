using System;
using System.Collections.Generic;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(1)]
    [Module(ModulesNames.AttendanceSystem)]
    public class NonAttendanceForm : Entity, IAggregateRoot // نموذج عدم التواجد ويشمل الغياب والتأخر
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual int Number { get; set; } // رقم النموذج

        [UserInterfaceParameter(Order = 1)]
        public virtual string Description { get; set; } // وصف النموذج

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual InfractionForm InfractionForm { get; set; } // المخالفة المرتبطة بنموذج التأخير

        [UserInterfaceParameter(Order = 1)]
        public virtual int ResetCounterRecurrence { get; set; } // رقم يمثل عدد الاشهر التي سيتم تصفير عداد التأخير عندها


        //public virtual  InfractionForm InfractionForm { get; set; } // المخالفة المرتبطة بنموذج التأخير
        //[UserInterfaceParameter(Order = 1)]
        //public virtual int ResetLatenessCounterRecurrence { get; set; } // رقم يمثل عدد الاشهر التي سيتم تصفير عداد التأخير عندها

        //[UserInterfaceParameter(Order = 1)]
        //public virtual DateTime LastReset { get; set; } // تاريخ أخر تصفير للعداد مفيد لمعرفة التاريخ التالي للتصفير

        //[UserInterfaceParameter(Order = 1)]
        //public virtual DateTime NextReset
        //{
        //    get
        //    {
        //        return LastReset.AddMonths(ResetLatenessCounterRecurrence);
        //    }
        //} // التاريخ القادم للتصفير وهو للقراءة فقط

        [UserInterfaceParameter(Order = 1)]
        public virtual IList<NonAttendanceSlice> NonAttendanceSlices { get; set; } // شرائح التأخير
        public virtual void AddNonAttendanceSlice(NonAttendanceSlice nonAttendanceSlice)
        {
            NonAttendanceSlices.Add(nonAttendanceSlice);
            nonAttendanceSlice.NonAttendanceForm = this;
        }
    }
}
