using System;
using Souccar.Core.CustomAttribute;

namespace Souccar.Domain.DomainModel
{
    [Serializable]
    public abstract class HistoryEntity : Entity, IHistory
    {
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsActive
        {
            get { return ExpireDate == null|| ExpireDate>=DateTime.Now; }
        }

        #region IHistory Members
        [UserInterfaceParameter(IsHidden = true)]
        public virtual DateTime FromDate { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual DateTime? ExpireDate { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string Comment { get; set; }

        #endregion

        public virtual void MakeHistory(DateTime date)
        {
            //this.ExpireDate = date.Midnight();
        }

        public virtual void MakeHistory()
        {
            // MakeHistory(DateTime.Now.Midnight());
        }
    }
}