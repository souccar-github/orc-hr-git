#region

using System;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.PMS.Entities.JobDescription
{
    /// <summary>
    /// Ammar Alziebak
    /// </summary>
    public class AppraisalJobDescription : Entity, IAggregateRoot
    {
        public virtual Responsibility Responsibility { get; set; }
        public virtual float Weight { get; set; }
        public virtual float Rate { get; set; }
        public virtual string Description { get; set; }

        public virtual Appraisal Appraisal { get; set; }

    }
}

