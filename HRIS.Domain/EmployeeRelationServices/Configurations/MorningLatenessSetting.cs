
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDesc.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(75)]

    public class MorningLatenessSetting : Entity, IConfigurationRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual int LatenessCountToSubtractLeave { set; get; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return LatenessCountToSubtractLeave.ToString(); } }
    }
}
