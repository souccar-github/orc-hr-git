using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class ServiceWorkflowMap: ClassMap<ServiceWorkflow>
    {
        public ServiceWorkflowMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.ServiceType);
            References(x => x.WorkflowSetting);
            References(x => x.EmployeeCard);
        }
    }
}
