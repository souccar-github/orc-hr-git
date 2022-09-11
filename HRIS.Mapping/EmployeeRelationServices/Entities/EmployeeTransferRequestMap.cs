using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.EmployeeRelationServices.Configurations
{
    public sealed class EmployeeTransferRequestMap : ClassMap<EmployeeTransferRequest>
    {
        public EmployeeTransferRequestMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Creator);
            References(x => x.Employee);
            References(x => x.WorkflowItem);
            References(x => x.DestNode);
            References(x => x.DestPosition);
            References(x => x.SourceNode);
            References(x => x.SourcePosition);

            Map(x => x.Date);
            Map(x => x.Note);
            Map(x => x.RequestStatus);
        }
    }
}
