using HRIS.Domain.EmployeeRelationServices.Entities;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class ServiceWorkflowSpecification: Validates<ServiceWorkflow>
    {
        public ServiceWorkflowSpecification()
        {
            IsDefaultForType();
            Check(x => x.ServiceType);
            Check(x => x.WorkflowSetting);
        }
    }
}
