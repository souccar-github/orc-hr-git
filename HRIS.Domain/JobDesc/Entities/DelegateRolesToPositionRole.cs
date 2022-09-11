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
    public class DelegateRolesToPositionRole : Entity
    {
        #region Roles Position

        [UserInterfaceParameter(Order = 5)]
        public virtual Role Roles { get; set; } 

        [UserInterfaceParameter(Order = 10)]
        public virtual DelegateRolesToPosition Delegate { get; set; }
        
        [UserInterfaceParameter(Order = 15)]
        public virtual String RoleName
        {
            get
            {
                if (Roles != null)
                    return Roles.Name;
                else
                    return String.Empty;
            }
        }

        #endregion
    }
}
