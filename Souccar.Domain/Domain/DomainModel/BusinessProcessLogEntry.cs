using System;

namespace Souccar.Domain.DomainModel
{
    public class BusinessProcessLogEntry : LogEntry, IAggregateRoot
    {
        /// <summary>
        /// BusinessProcessId
        /// </summary>
        public virtual Guid BusinessProcessId { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public virtual IContent Content { get; set; }

        public virtual string Bookmark { get; set; }

        public virtual string State { get; set; }

        public virtual string AssignedTo { get; set; }
    }
}