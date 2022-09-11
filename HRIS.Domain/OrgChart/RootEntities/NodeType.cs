#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.OrgChart.RootEntities
{
    [Module(ModulesNames.OrganizationChart)]
    [Order(4)]
    public class NodeType : Entity, IAggregateRoot
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual int Order { get; set; }
        [UserInterfaceParameter(Order = 155, IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
    }
}