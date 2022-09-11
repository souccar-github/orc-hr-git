using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Domain.DomainModel;

namespace Souccar.Domain
{
    public class SouccarEntityException : SouccarException
    {
        public Entity Entity { get; private set; }
        public SouccarEntityException(Entity entity)
        {
            this.Entity = entity;
        }
        public SouccarEntityException(Entity entity, string message):base(message)
        {
            this.Entity = entity;
        }
    }

}
