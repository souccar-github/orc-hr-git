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
    public class DelegateAuthoritiesToPositionAuthority : Entity
    {
        #region Authority Position

        [UserInterfaceParameter(Order = 5)]
        public virtual Authority Authorities { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual DelegateAuthoritiesToPosition Delegate { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual String AuthorityName
        {
            get
            {
                if (Authorities != null)
                    return Authorities.Name;
                else
                    return String.Empty;
            }
        }

        #endregion
    }
}
