using HRIS.Domain.Grades.Entities;
using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Workflow
{
    public class WorkflowSettingOverwritingHistory : Entity
    {
        public virtual OrganizationalLevel OrganizationalLevel { get; set; }
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
        public virtual JobTitle JobTitle { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual int Count { get; set; }
        public virtual WorkflowSetting WorkflowSetting { get; set; }
    }
}
