#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.OrgChart.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
//using Resources.Areas.OrgChart.ValueObjects.NonCashBenefit;

#endregion

namespace HRIS.Domain.OrgChart.Entities
{
    public class NonCashBenefit : Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual NoneCashBenefitType Type { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual float Amount { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual CurrencyType CurrencyType { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual string Description { get; set; }

        public virtual Grade Grade { get; set; }
    }
}
