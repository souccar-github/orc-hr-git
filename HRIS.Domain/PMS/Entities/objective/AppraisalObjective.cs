
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;
namespace HRIS.Domain.PMS.Entities.objective
{
    public class AppraisalObjective : Entity, IAggregateRoot
    {
        public virtual Objective Objective { get; set; }
        public virtual float Weight { get; set; }
        public virtual float Rate { get; set; }
        public virtual string Description { get; set; }

        public virtual Appraisal Appraisal { get; set; }
    }
}
