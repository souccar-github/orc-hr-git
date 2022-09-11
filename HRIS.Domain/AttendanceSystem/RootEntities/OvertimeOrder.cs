using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(20)]
    [Module(ModulesNames.AttendanceSystem)]
    public class OvertimeOrder : Entity, IAggregateRoot //  تكليف الاضافي
    {// الاضافي الذي تم وضع علامة بحاجة الى تكليف لا يتم اخذه بعين الاعتبار الا اذا وجد امر تكليف اثناء حساب الدوام

        [UserInterfaceParameter(Order = 1)]
        public virtual int Number { get; set; } // رقم التكليف

        [UserInterfaceParameter(Order = 2, IsReference = true, ReferenceReadUrl = "AttendanceSystem/Home/FilterEmployee")]
        public virtual Employee Employee { get; set; } //  الموظف المكلف بالاضافي
        [UserInterfaceParameter(Order = 3)]
        public virtual string FatherName
        {
            get
            {
                return Employee.FatherName;
            }
        }
        [UserInterfaceParameter(Order = 4)]
        public virtual DateTime FromDate { get; set; } //  التكليف يبدأ من تاريخ

        [UserInterfaceParameter(Order = 5)]
        public virtual DateTime ToDate { get; set; } // التكليف ينتهي بتاريخ

        [UserInterfaceParameter(Order = 6)]
        public virtual double OvertimeHoursPerDay { get; set; } // عدد ساعات التكليف لليوم الواحد

        [UserInterfaceParameter(Order = 7, IsReference = true, ReferenceReadUrl = "AttendanceSystem/Home/EmployeesAreOnHeadOfHisWork")]
        public virtual Employee EmployeeManager { get; set; } // الموظف طالب التكليف وغالبا يكون مدير يتم التكليف بموافقته

        [UserInterfaceParameter(Order = 8)]
        public virtual bool TakeConsiderationHolidaysDeduction { get; set; }
        [UserInterfaceParameter(Order = 9)]
        public virtual string Note { get; set; } // ملاحظات

    }
}
