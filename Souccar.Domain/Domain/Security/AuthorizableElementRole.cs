using Souccar.Domain.DomainModel;

namespace Souccar.Domain.Security
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class AuthorizableElementRole : Entity, IAggregateRoot
    {
        public virtual Role Role { get; set; }
        public virtual AuthorizeType AuthorizeType { get; set; }
        public virtual string AuthorizableElementId { get; set; }
        public virtual AuthorizableElementType AuthorizableElementType { get; set; }
        public virtual string ModuleName { get; set; }
    }
}
