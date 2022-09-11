
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Recruitment.Entities
{

    public class Qualification : Entity
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 1)]
        public virtual MajorType MajorType { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual Major Major { get; set; }

        public virtual RecruitmentInformation RecruitmentInformation { get; set; }

        #endregion

    }
}
