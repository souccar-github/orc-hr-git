#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Objectives.Indexes
{
    [Module(ModulesNames.Objectives)]
    public class ObjectiveConstraintType : IndexEntity, IAggregateRoot
    {
    }
}