using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.Configurations
{
    [Order(30)]
    [Module(ModulesNames.AttendanceSystem)]
    public class AttendanceForm : Entity, IConfigurationRoot  //  نموذج الدوام
    {

        public AttendanceForm()
        {
            WorkshopRecurrences = new List<WorkshopRecurrence>();
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual int Number { get; set; } // رقم النموذج

        [UserInterfaceParameter(Order = 2)]
        public virtual string Description { get; set; } // الوصف للنموذج

        [UserInterfaceParameter(Order = 3)]
        public virtual CalculationMethod CalculationMethod { get; set; } // طريقة حساب الدوام مع او بدون تقاص 
        
        [UserInterfaceParameter(Order = 4)]
        public virtual bool RelyHolidaies { get; set; } // اعتماد ايام العطل الثابتة والمتغيرة

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get { return Description; }
        }
        
        [UserInterfaceParameter(Order = 5)]
        public virtual IList<WorkshopRecurrence> WorkshopRecurrences { get; set; } // تواترات الوردية خلال الشهر
        public virtual void AddWorkshopRecurrence(WorkshopRecurrence workshopRecurrence)
        {
            workshopRecurrence.AttendanceForm = this;
            WorkshopRecurrences.Add(workshopRecurrence);
        }

    }
}
