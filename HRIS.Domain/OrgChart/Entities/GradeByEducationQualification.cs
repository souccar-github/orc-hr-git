#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.OrgChart.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
//using Resources.Areas.OrgChart.ValueObjects.CashBenefit;

#endregion

namespace HRIS.Domain.OrgChart.Entities
{
    public class GradeByEducationQualification : Entity
    {
        [UserInterfaceParameter(Order = 1, IsReference = true, ReferenceReadUrl = "Grade/Reference/ReadMajorType")]
        public virtual MajorType MajorType { get; set; }
        [UserInterfaceParameter(Order = 2, IsReference = true, ReferenceReadUrl = "Grade/Reference/ReadMajor")]
        public virtual Major Major { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual float FirstSalary { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual CurrencyType CurrencyType { get; set; }

        [UserInterfaceParameter(Order = 40,IsHidden=true)]
        public virtual string Note { get; set; }
        [UserInterfaceParameter(Order = 155, IsHidden = true)]
        public virtual string NameForDropdown {
            get {
                var _majorType = MajorType != null ? MajorType.Name : "";
                var _major = Major != null ? "," + Major.Name : "";
                return _majorType + _major ;
            }
        }
        public virtual GradeByEducation GradeByEducation { get; set; }
    }
}
