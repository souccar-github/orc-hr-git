#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Grades.Indexes;

#endregion

namespace HRIS.Domain.Grades.Entities
{
    public class NonCashBenefit : Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual NoneCashBenefitType Type { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual string Description { get; set; }

        public virtual Grade Grade { get; set; }
    }
}
