using System;
using System.Linq;
using System.Collections.Generic;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.RootEntities;

using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;

//using FluentNHibernate.Data;

namespace HRIS.Domain.Objectives.RootEntities
{
    //[Module(ModulesNames.Objective)]
    public class TestCascade : Entity, IAggregateRoot
    {


        [UserInterfaceParameter(Order = 70, IsReference = true)]
        public virtual Grade Grade { get; set; }

        [UserInterfaceParameter(Order = 70, IsReference = true, CascadeFrom = "Grade", ReferenceReadUrl = "Objectives/Reference/ReadJobTitleCascadeGrade")]
        public virtual JobTitle JobTitle { get; set; }

        [UserInterfaceParameter(Order = 70, IsReference = true, CascadeFrom = "JobTitle", ReferenceReadUrl = "Objectives/Reference/ReadJobDescriptionCascadeGrade")]
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }

        [UserInterfaceParameter(Order = 70, IsReference = true, CascadeFrom = "JobDescription", ReferenceReadUrl = "Objectives/Reference/ReadPositionCascadeJobDescription")]
        public virtual Position Position { get; set; }

        [UserInterfaceParameter(Order = 70,IsTime=true)]
        public virtual DateTime T { get; set; }
        public virtual DateTime D { get; set; }
        [UserInterfaceParameter(Order = 70, IsDateTime = true)]
        public virtual DateTime DT { get; set; }
        public virtual DateTime? NDT { get; set; }
    }
}