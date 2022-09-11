#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;
using System;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class DelegateAuthoritiesToPosition : Entity
    {
        #region Authorities To Position
        public DelegateAuthoritiesToPosition()
        {
            DelegateAuthority = new List<DelegateAuthoritiesToPositionAuthority>();
        }

        [UserInterfaceParameter(Order = 20)]
        public virtual Position DestinationPosition { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual bool PerformanceAppraisal { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual string DelegationReason { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual DateTime FromDate { get; set; }

        [UserInterfaceParameter(Order = 60)]
        public virtual DateTime ToDate { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual string DelegationComment { get; set; }

        public virtual Position SourcePosition { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual IList<DelegateAuthoritiesToPositionAuthority> DelegateAuthority { get; set; }
        public virtual void AddDelegateAuthorities(DelegateAuthoritiesToPositionAuthority DelegateAuthority)
        {
            DelegateAuthority.Delegate = this;
            this.DelegateAuthority.Add(DelegateAuthority);
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return DestinationPosition.NameForDropdown; } }
        #endregion
    }
}
