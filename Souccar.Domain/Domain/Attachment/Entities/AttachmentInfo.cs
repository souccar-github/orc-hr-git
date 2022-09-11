using System;
using Souccar.Domain.Attachment.Enums;
using Souccar.Domain.DomainModel;

namespace Souccar.Domain.Attachment.Entities
{
    public class AttachmentInfo : Entity, IAggregateRoot
    {
        public virtual string Path { get; set; }
        public virtual string PhysicalFileName { get; set; }
        public virtual string OriginalFileName { get; set; }
        public virtual string ModelFullClassName { get; set; }
        public virtual DateTime UploadDate { get; set; }
        public virtual string Description { get; set; }
        public virtual EntityType EntityType { get; set; }
        //public virtual int BaseId { get; set; }
    }
}