using System;

namespace Souccar.Domain.DomainModel
{
    public abstract class BusinessProcess : Entity, IBusinessProcess
    {
        protected BusinessProcess()
        {
            CreatedDate = ChangeDate = DateTime.Today;
        }

        protected BusinessProcess(string createdBy, Guid businessProcessId)
            : this()
        {
            CreatedBy = ChangeBy = createdBy;
            BusinessProcessId = businessProcessId;
        }

        #region IBusinessProcess Members

        public virtual Guid BusinessProcessId { get; set; }

        public virtual DateTime CreatedDate { get; private set; }

        public virtual DateTime ChangeDate { get; set; }

        public virtual string CreatedBy { get; private set; }

        public virtual string ChangeBy { get; private set; }

        public abstract IBusinessProcessContent Content { get; protected set; }

        #endregion
    }
}