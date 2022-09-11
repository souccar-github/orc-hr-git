using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class EmployeeCustodieMap : ClassMap<EmployeeCustodie>
    {
        public EmployeeCustodieMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Quantity);
            Map(x => x.CustodyStartDate);
            Map(x => x.CustodyEndDate);
            Map(x => x.OperationDate);
          
            Map(x => x.Note);
            Map(x => x.CustodieStatus);
            
            References(x => x.CustodyName);
            References(x => x.WorkflowItem);
            References(x => x.EmployeeCard);
            References(x => x.AdditionalInformation);
            References(x => x.OperationCreator);

        }
    }
}
