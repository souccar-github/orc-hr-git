using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace Souccar.Domain.Security
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [Module("Security")]
    public class Role : Entity, IAggregateRoot
    {
        #region Properties
        public virtual string Name { set; get; }
        public virtual string Description { set; get; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool Enabled { set; get; }

        #endregion

        #region Constructors
        public Role()
        {
            Enabled = true;
        }

        public Role(string roleName)
            : this()
        {
            Name = roleName;
        }

        #endregion

    }
}
