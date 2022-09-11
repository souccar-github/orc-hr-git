using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.Indexes;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    [Details(IsDetailHidden = false)]
    public class InfractionSlice : Entity //  شرائح نموذج المخالفة
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual InfractionForm InfractionForm { get; set; } // المخالفة الاب التي تتبع لها الشريحة

        [UserInterfaceParameter(Order = 2)]
        public virtual int MinimumRecurrence { get; set; } // الحد الادنى للتكرار

        [UserInterfaceParameter(Order = 3)]
        public virtual int MaximumRecurrence { get; set; } // الحد الاعلى للتكرار

        //todo Mhd Alsaadi: العقوبة من علاقات العمل
        [UserInterfaceParameter(Order = 4, IsReference = true)]
        public virtual DisciplinarySetting Penalty { get; set; } // العقوبة المرتبطة بشريحة المخالفة
    }
}
