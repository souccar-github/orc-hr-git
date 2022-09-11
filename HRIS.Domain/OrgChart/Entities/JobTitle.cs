#region

using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrgChart.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.OrgChart.Entities
{
    //[Module(ModulesNames.JobDescription)]
    [Order(2)]
    public class JobTitle : Entity, IAggregateRoot
    {
        public virtual string Name { get; set; }
        public virtual int Order { get; set; }
        public virtual int EmployeeCount { get; set; }
        public virtual int Vacancies { get; set; }
        public virtual string Description { get; set; }

        public virtual Grade Grade { get; set; }
        [UserInterfaceParameter(Order = 155, IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
        public virtual IList<JobTitleQualification> JobTitleQualifications { get; set; }
        public virtual void AddJobTitleQualification(JobTitleQualification qualification)
        {
            qualification.JobTitle = this;
            this.JobTitleQualifications.Add(qualification);
        }

    }
}