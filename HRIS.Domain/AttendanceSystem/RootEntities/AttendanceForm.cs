using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(1)]
    [Module(ModulesNames.AttendanceSystem)]
    public class AttendanceForm : Entity, IAggregateRoot  //  نموذج الدوام
    {

        [UserInterfaceParameter(Order = 1)]
        public virtual int Number { get; set; } // رقم النموذج

        [UserInterfaceParameter(Order = 1)]
        public virtual string Description { get; set; } // الوصف للنموذج

        [UserInterfaceParameter(Order = 1)]
        public virtual CalculationMethod CalculationMethod { get; set; } // طريقة حساب الدوام مع او بدون تقاص 
        
        [UserInterfaceParameter(Order = 1)]
        public virtual bool RelyHolidaies { get; set; } // اعتماد ايام العطل الثابتة والمتغيرة
        
        [UserInterfaceParameter(Order = 1)]
        public virtual IList<WorkshopRecurrence> WorkshopRecurrences { get; set; } // تواترات الوردية خلال الشهر
        public virtual void AddWorkshopRecurrence(WorkshopRecurrence workshopRecurrence)
        {
            WorkshopRecurrences.Add(workshopRecurrence);
            workshopRecurrence.AttendanceForm = this;
        }
    }
}
