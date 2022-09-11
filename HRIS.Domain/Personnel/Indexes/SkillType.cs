#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Personnel.Indexes
{
    [Module(ModulesNames.Personnel)]
    [Module(ModulesNames.Recruitment)]
    [Module(ModulesNames.JobDescription)]
    public class SkillType : IndexEntity, IAggregateRoot
    {
    }
}