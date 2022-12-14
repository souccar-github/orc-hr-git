using HRIS.Domain.AttendanceSystem.Indexes;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.Entities
{
    public class InfractionSlice : Entity //  شرائح نموذج المخالفة
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual InfractionForm InfractionForm { get; set; } // المخالفة الاب التي تتبع لها الشريحة

        [UserInterfaceParameter(Order = 2)]
        public virtual int MinimumRecurrence { get; set; } // الحد الادنى للتكرار

        [UserInterfaceParameter(Order = 3)]
        public virtual int MaximumRecurrence { get; set; } // الحد الاعلى للتكرار

        //todo Mhd Alsaadi: العقوبة من علاقات العمل
        [UserInterfaceParameter(Order = 4)]
        public virtual PenaltyType Penalty { get; set; } // العقوبة المرتبطة بشريحة المخالفة
    }
}
