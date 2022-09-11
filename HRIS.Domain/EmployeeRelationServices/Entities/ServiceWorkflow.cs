using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    public class ServiceWorkflow :Entity
    {
        public virtual ServiceType ServiceType { get; set; }
        [UserInterfaceParameter(IsReference =true)]
        public virtual WorkflowSetting WorkflowSetting { get; set; }
        public virtual EmployeeCard EmployeeCard { get; set; }
    }
}
