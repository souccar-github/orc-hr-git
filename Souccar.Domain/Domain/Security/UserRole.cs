using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;

namespace Souccar.Domain.Security
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class UserRole : Entity, IAggregateRoot
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
