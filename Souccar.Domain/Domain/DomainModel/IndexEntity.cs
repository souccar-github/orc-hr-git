using System;

namespace Souccar.Domain.DomainModel
{
    [Serializable]
    public abstract class IndexEntity : Entity, IIndex,IAggregateRoot
    {
        #region IIndex Members

        public virtual string Name { get; set; }
        public virtual int Order { get; set; }


        #endregion
    }
}