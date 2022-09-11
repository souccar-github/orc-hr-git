#region

using System.ComponentModel.DataAnnotations;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.OrgChart.Indexes
{
    //[Module(ModulesNames.Grade)]
    public class CashBenefitType : IndexEntity, IAggregateRoot
    {
    }
}