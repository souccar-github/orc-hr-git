#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Grades.Entities
{
    public class GradeByEducationQualification : Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual MajorType MajorType { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual Major Major { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual float FirstSalary { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual CurrencyType CurrencyType { get; set; }

        [UserInterfaceParameter(Order = 40,IsHidden=true)]
        public virtual string Note { get; set; }

        public virtual GradeByEducation GradeByEducation { get; set; }
    }
}
