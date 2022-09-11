using System.Collections.Generic;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Indexes;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    [Order(35)]
    [Module(ModulesNames.EmployeeRelationServices)]
    public class InfractionForm : Entity, IConfigurationRoot  // نموذج مخالفة
    {

        public InfractionForm()
        {
            InfractionSlices = new List<InfractionSlice>();
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual  int Number { get; set; } // رقم المخالفة
        [UserInterfaceParameter(Order = 2)]
        public virtual InfractionName Infraction { get; set; } // اسم المخالفة

        [UserInterfaceParameter(Order = 3)]
        public virtual  string Description { get; set; } // وصف المخالفة

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        { 
            get { return Infraction != null ? Infraction.Name : string.Empty; }
        }

        [UserInterfaceParameter(Order = 4)]
        public virtual  IList<InfractionSlice> InfractionSlices { get; set; } // شراح المخالفة
        public virtual void AddInfractionSlice(InfractionSlice infractionSlice)
        {
            infractionSlice.InfractionForm = this;
            InfractionSlices.Add(infractionSlice);
        }
    }
}
