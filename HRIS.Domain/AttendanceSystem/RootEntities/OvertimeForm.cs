using System.Collections.Generic;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(1)]
    [Module(ModulesNames.AttendanceSystem)]
    public class OvertimeForm : Entity, IAggregateRoot  //  نموذج اضافي
    {

        [UserInterfaceParameter(Order = 1)]
        public virtual int Number { get; set; } // رقم النموذج

        [UserInterfaceParameter(Order = 1)]
        public virtual string Description { get; set; } //  وصف النموذج

        [UserInterfaceParameter(Order = 1)]
        public virtual bool NeedOverTimeAcceptance { get; set; } // الاضافي يحتاج الى تكليف من قبل المدير

        [UserInterfaceParameter(Order = 1)]
        public virtual IList<OvertimeSlice> OvertimeSlices { get; set; }
        public virtual void AddOvertimeSlice(OvertimeSlice overtimeSlice)
        {
            OvertimeSlices.Add(overtimeSlice);
            overtimeSlice.OvertimeForm = this;
        }
    }
}
