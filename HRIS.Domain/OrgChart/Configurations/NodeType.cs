#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.OrganizationChart.Configurations
{
    [Module(ModulesNames.OrganizationChart)]
    [Order(4)]
    public class NodeType : Entity, IConfigurationRoot
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual int Order { get; set; }

    }
}