#region

using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.Indexes;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.JobDesc.RootEntities
{
    [Module(ModulesNames.JobDescription)]
    [Order(3)]
    public class CompetenceCategory : Entity, IAggregateRoot
    {
        public CompetenceCategory()
        {
            LevelDescriptions = new List<CompetenceCategoryLevelDescription>();
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual CompetenceType Type { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual CompetenceName Name { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name.Name + " ," + Type.Name; } }

        public virtual IList<CompetenceCategoryLevelDescription> LevelDescriptions { get; protected set; }

        public virtual void AddLevelDescription(CompetenceCategoryLevelDescription levelDescription)
        {
            levelDescription.CompetenceCategory = this;
            LevelDescriptions.Add(levelDescription);
        }
    }
}