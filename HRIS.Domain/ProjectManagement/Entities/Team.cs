using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.ProjectManagement.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.ProjectManagement.Entities
{
    public class Team:Entity,IAggregateRoot
    {
        public Team()
        {
            Roles = new List<TRole>();
        }

        #region Info

        [UserInterfaceParameter(Order=1)]
        public virtual string Name { get; set; }

        public virtual Project Project { get; set; }

        #endregion

        public virtual IList<TRole> Roles { get; set; }
        public virtual void AddTeamRoles(TRole role)
        {
            role.Team = this;
            Roles.Add(role);
        }
    }
}
