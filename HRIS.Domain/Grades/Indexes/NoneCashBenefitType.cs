#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Grades.Indexes
{
    [Module(ModulesNames.Grades)]
    public class NoneCashBenefitType : IndexEntity, IAggregateRoot
    {
    }
}