#region

using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.JobDescription.Configurations
{
    [Module(ModulesNames.JobDescription)]
    [Order(3)]
    public class CompetenceCategory : Entity, IConfigurationRoot
    {
        public CompetenceCategory()
        {
            LevelDescriptions = new List<CompetenceCategoryLevelDescription>();
        }

        [UserInterfaceParameter(Order = 1, IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return Name != null ? Name.Name : string.Empty;
            }
        }

        [UserInterfaceParameter(Order = 2)]
        public virtual CompetenceType Type { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual CompetenceName Name { get; set; }
        
        public virtual IList<CompetenceCategoryLevelDescription> LevelDescriptions { get; protected set; }

        public virtual void AddLevelDescription(CompetenceCategoryLevelDescription levelDescription)
        {
            levelDescription.CompetenceCategory = this;
            LevelDescriptions.Add(levelDescription);
        }
    }
}