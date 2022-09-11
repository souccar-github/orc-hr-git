#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Configurations;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System.Linq;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class Competence : Entity,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1, IsReference = true, ReferenceReadUrl = "JobDescription/Reference/GetCompetenceType")]
        public virtual CompetenceType Type { get; set; }
        [UserInterfaceParameter(Order = 2, IsReference = true, CascadeFrom = "Type", ReferenceReadUrl = "JobDescription/Reference/ReadCompetenceNameCascadeCompetenceType")]
        public virtual CompetenceCategory CompetenceCategory { get; set; }
        [UserInterfaceParameter(Order = 3, IsReference = true, CascadeFrom = "CompetenceCategory", ReferenceReadUrl = "JobDescription/Reference/ReadLevelCascadeCompetenceName")]
        public virtual CompetenceCategoryLevelDescription Level { get; set; }   
        [UserInterfaceParameter(Order = 4)]
        public virtual string Description
        {
            get
            {
                return Level != null ? Level.Description : "";
            }
        
        }
        [UserInterfaceParameter(Order = 5)]
        public virtual float Weight { get; set; }
        [UserInterfaceParameter(Order = 6)]
        public virtual float ActuallyWeight
        {
            get
            {
                if (this.JobDescription == null)
                    return 0;
                var sum = this.JobDescription.Competencies.Sum(x => x.Weight);
                if (sum == 0)
                    return 100;
                return 100 * Weight / sum;
            }
        }
        

        [UserInterfaceParameter(Order = 7)]
        public virtual bool Required { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
    }
}