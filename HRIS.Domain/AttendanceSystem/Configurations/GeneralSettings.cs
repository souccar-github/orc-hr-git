using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.Configurations
{
    [Module(ModulesNames.AttendanceSystem)]
    [Order(22)]
    public class GeneralSettings : Entity, IConfigurationRoot
    {
        public GeneralSettings()
        {

        }

        //[UserInterfaceParameter(Order = 5)]
        //public virtual bool AttendanceDemand { get; set; } // مطالبة الدوام

        [UserInterfaceParameter(IsReference = true, Order = 10)]
        public virtual AttendanceForm AttendanceForm { get; set; } //نموذج الدوام

        [UserInterfaceParameter(IsReference = true, Order = 15)]
        public virtual NonAttendanceForm LatenessForm { get; set; } // نموذج التأخير

        [UserInterfaceParameter(IsReference = true, Order = 20)]
        public virtual OvertimeForm OvertimeForm { get; set; } //نموذج الاضافي

        [UserInterfaceParameter(IsReference = true, Order = 25)]
        public virtual NonAttendanceForm AbsenceForm { get; set; } //نموذج نقص الدوام
        [UserInterfaceParameter(Order =30)]
        public virtual int AttendanceCycleStartDay { get; set; }
    }
}