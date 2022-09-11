using System;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.AttendanceSystem.Configurations
{
    //  الورديات المؤقتة يمكن الاستفادة منها للورديات المؤقتة على مستوى الموظف مثل حالة تبيديل الورديات بين موظفين
    //     وكذلك الورديات المؤقتة على مستوى الوردية مثل حالة رمضان 
    [Order(26)]
    [Module(ModulesNames.AttendanceSystem)]
    public class TemporaryWorkshop : Entity, IConfigurationRoot
    {
        //[UserInterfaceParameter(Order = 1)]
        //public virtual  EmployeeAttendanceCard EmployeeAttendanceCard { get; set; } // نموذج الدوام الاب التي تتبع لها هذه الوردية الاستثنائية

        [UserInterfaceParameter(Order = 1)]
        public virtual EmployeeCard EmployeeCard { get; set; } // نموذج الدوام الاب التي تتبع لها هذه الوردية الاستثنائية

        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual Workshop Workshop { get; set; }  // الوردية التي تتبع لها هذه الوردية الاستثنائية

        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime FromDate { get; set; } // من تاريخ

        [UserInterfaceParameter(Order = 3)]
        public virtual DateTime ToDate { get; set; } // الى تاريخ

        [UserInterfaceParameter(Order = 4, IsReference = true)]
        public virtual Workshop AlternativeWorkshop { get; set; } // الوردية المؤقتة التي سيتم الحساب بناء عليها خلال الفترة المحددة
    }
}
