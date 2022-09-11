using System.Collections.Generic;
using HRIS.Domain.Training1.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training1.RootEntities
{
    [Module("Training1")]
    public class Trainer : Entity, IAggregateRoot
    {
        public virtual string Name{ get; set; }
        public virtual string ContactInfo { get; set; }
        public virtual CourseSpecialize Specialize { get; set; }

    }
}
