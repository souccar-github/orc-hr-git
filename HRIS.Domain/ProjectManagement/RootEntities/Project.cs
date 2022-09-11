using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Domain.ProjectManagement.Enums;
using HRIS.Domain.ProjectManagement.Helpers;
using HRIS.Domain.ProjectManagement.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.ProjectManagement.RootEntities
{
    [Command(CommandsNames.KPIinfo, Order = 1)]
    [Module(ModulesNames.ProjectManagement)]
    public class Project : Entity,IAggregateRoot
    {
        public Project()
        {
            Teams = new List<Team>();
            Phases = new List<Phase>();
            Constrains = new List<Constrain>();
            SuccessFactors = new List<SuccessFactor>();
            Resources = new List<Resource>();
        }

       

        [UserInterfaceParameter(Order = 5, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string Code { get; set; }

        [UserInterfaceParameter(Order = 10, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 15, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual ProjectType Type { get; set; }

        [UserInterfaceParameter(Order = 20, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual ProjectStatus Status { get; set; }

        [UserInterfaceParameter(Order = 22, Group = ProjectManagementGroupNames.BasicInformation, IsReference = true)]
        public virtual Node Node { get; set; }
       
        [UserInterfaceParameter(Order = 25, Group = ProjectManagementGroupNames.BasicInformation, IsReference = true)]
        public virtual Position Position { get; set; }

        [UserInterfaceParameter(Order = 30, Group = ProjectManagementGroupNames.BasicInformation, IsNonEditable = true)]
        public virtual string EmployeeName
        {
            get
            {
                return Position.Employee == null ? string.Empty : Position.Employee.TripleName;
            }
        }

        [UserInterfaceParameter(Order = 35, Group = ProjectManagementGroupNames.Dates)]
        public virtual DateTime PlannedStartingDate { get; set; }

        [UserInterfaceParameter(Order = 40, Group = ProjectManagementGroupNames.Dates)]
        public virtual DateTime PlannedEndingDate { get; set; }

        [UserInterfaceParameter(Order = 45, Group = ProjectManagementGroupNames.Dates)]
        public virtual DateTime ActualStartingDate { get; set; }

        [UserInterfaceParameter(Order = 50, Group = ProjectManagementGroupNames.Dates)]
        public virtual DateTime ActualEndingDate { get; set; }

        [UserInterfaceParameter(Order = 55, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 60, IsNonEditable = true)]
        public virtual KPItype KPItype { get; set; }

        [UserInterfaceParameter(Order = 65, IsNonEditable = true)]
        public virtual float KPIwieght { get; set; }

        [UserInterfaceParameter(Order = 70, IsNonEditable = true)]
        public virtual int KPIvalue { get; set; }

        [UserInterfaceParameter(Order = 75, IsNonEditable = true)]
        public virtual string KPIdescription { get; set; }

        public virtual IList<Team> Teams { get; set; }
        public virtual void AddTeamAndRole(Team team)
        {
            team.Project = this;
            Teams.Add(team);
        }
        public virtual IList<Phase> Phases { get; set; }
        public virtual void AddPhase(Phase phases)
        {
            phases.Project = this;
            Phases.Add(phases);
        }
        public virtual IList<Constrain> Constrains { get; set; }
        public virtual void AddConstrain(Constrain constrains)
        {
            constrains.Project = this;
            Constrains.Add(constrains);
        }
        public virtual IList<SuccessFactor> SuccessFactors { get; set; }
        public virtual void AddSuccessFactor(SuccessFactor successFactors)
        {
            successFactors.Project = this;
            SuccessFactors.Add(successFactors);
        }

        public virtual IList<Resource> Resources { get; set; }
        public virtual void AddResource(Resource resource)
        {
            resource.Project = this;
            Resources.Add(resource);
        }
    }
}
