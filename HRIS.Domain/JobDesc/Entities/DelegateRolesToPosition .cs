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
    public class DelegateRolesToPosition : Entity
    {
        #region Roles To Position
        public DelegateRolesToPosition()
        {
            DelegateRoles = new List<DelegateRolesToPositionRole>();
        }
        [UserInterfaceParameter(Order = 20)]
        public virtual Position DestinationPosition { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual bool PerformanceAppraisal  { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual Position Superior { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual string DelegationReason  { get; set; }

        [UserInterfaceParameter(Order = 60)]
        public virtual DateTime FromDate   { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual DateTime ToDate { get; set; }

        [UserInterfaceParameter(Order = 80)]
        public virtual string DelegationComment { get; set; }
        public virtual Position SourcePosition { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual IList<DelegateRolesToPositionRole> DelegateRoles { get; set; }
        public virtual void AddDelegateRoles(DelegateRolesToPositionRole DelegateRole)
        {
            DelegateRole.Delegate = this;
            this.DelegateRoles.Add(DelegateRole);
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return DestinationPosition.NameForDropdown; } }
        #endregion
    }
}
