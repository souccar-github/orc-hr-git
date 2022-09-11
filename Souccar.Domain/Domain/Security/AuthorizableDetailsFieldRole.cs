using Souccar.Domain.DomainModel;

namespace Souccar.Domain.Security
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class AuthorizableDetailsFieldRole : Entity, IAggregateRoot
    {
        public virtual Role Role { get; set; }
        public virtual bool  IsHidden { get; set; }
        public virtual string AuthorizableElementId { get; set; }
        public virtual string AuthorizableFieldId { get; set; }
        
        public virtual string ModuleName { get; set; }
    }
}
