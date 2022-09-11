using System.Collections.Generic;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    [Order(1)]
    [Module(ModulesNames.AttendanceSystem)]
    public class InfractionForm : Entity, IAggregateRoot  // نموذج مخالفة
    {

        [UserInterfaceParameter(Order = 1)]
        public virtual  int Number { get; set; } // رقم المخالفة

        [UserInterfaceParameter(Order = 1)]
        public virtual  string Description { get; set; } // وصف المخالفة

        [UserInterfaceParameter(Order = 1)]
        public virtual  IList<InfractionSlice> InfractionSlices { get; set; } // شراح المخالفة
        public virtual void AddInfractionSlice(InfractionSlice infractionSlice)
        {
            InfractionSlices.Add(infractionSlice);
            infractionSlice.InfractionForm = this;
        }
    }
}
