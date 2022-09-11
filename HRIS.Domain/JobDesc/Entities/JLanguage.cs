#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class JLanguage : Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual LanguageName LanguageName { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual Level Writing { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual Level Reading { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual Level Speaking { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual Level Listening { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual bool Required { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
    }
}