using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.Entities
{
    public class EducationBase : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 10)]
        public virtual MajorType Type { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual Major Major { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual University University { get; set; }
        [UserInterfaceParameter(Order = 40)]
        public virtual Level Rank { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual ScoreType ScoreType { get; set; }

        [UserInterfaceParameter(Order = 60)]
        public virtual Score Score { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual DateTime? DateOfIssuance { get; set; }

        [UserInterfaceParameter(Order = 80)]
        public virtual Country Country { get; set; }

        [UserInterfaceParameter(Order = 90)]
        public virtual string AmendmentDocumentNo { get; set; }

        [UserInterfaceParameter(Order = 100)]
        public virtual DateTime? AmendmentDocumentDate { get; set; }

        [UserInterfaceParameter(Order = 110)]
        public virtual string Comments { get; set; }


        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Type.Name + "-" + Major.Name; } }
    }
}
