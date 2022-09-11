#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Personnel.Indexes
{
    [Module(ModulesNames.Personnel)]
    [Module(ModulesNames.PayrollSystem)]
    [Module(ModulesNames.Recruitment)]
    public class Nationality : IndexEntity, IAggregateRoot
    {
        public override string ToString()
        {
            return Name;
        }
    }
}