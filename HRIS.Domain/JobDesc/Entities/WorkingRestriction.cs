#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.RootEntities;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class WorkingRestriction : Entity
    {
        public WorkingRestriction()
        {
            Restrictions = new List<Restriction>();
        }

        public virtual string InternalRelationships { get; set; }

        public virtual string ExternalRelationships { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }

        #region Conditions

        public virtual IList<Restriction> Restrictions { get; set; }

        public virtual void AddRestriction(Restriction restriction)
        {
            restriction.WorkingRestriction = this;
            Restrictions.Add(restriction);
        }

        #endregion
    }
}