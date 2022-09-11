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
    [Command("AddUserToRole", Order = 1)]
    [Command("ResetPassword", Order = 2)]
    public class User : Entity, IAggregateRoot
    {
        public User()
        {
            Roles = new List<UserRole>();
        }
        public virtual string Username { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Email { get; set; }
        public virtual string FCMToken { get; set; }
        public virtual string MobilePhone { get; set; }
        public virtual string Comment { get; set; }
        //[UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsEnabled { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsLockedOut { set; get; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsOnline { set; get; }
        public virtual ThemingType ThemingType { set; get; }
        public virtual IList<UserRole> Roles { set; get; }

        public virtual bool IsInRole(string role) { return Roles.Any(q => q.Role.Name == role); }

        public virtual void AddRole(Role role)
        {
            if (!Roles.Any(x => x.Id.Equals(role.Id)))
                Roles.Add(new UserRole() { User = this, Role = role });
        }

        public virtual void RemoveRole(Role role)
        {
            Roles.Remove(Roles.SingleOrDefault(x => x.Role.Id.Equals(role.Id)));
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return FullName != string.Empty ? FullName : string.Empty;
            }
        }
    }
}
