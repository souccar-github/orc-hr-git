using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Core;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class EmployeeAdvanceMap : ClassMap<EmployeeAdvance>
    {
        public EmployeeAdvanceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.AdvanceStatus);
            Map(x => x.AdvanceAmount);
            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.DeservableAdvanceAmount);
            Map(x => x.OperationDate);
            Map(x => x.IsTransferToPayroll);
            Map(x => x.SourceId);


            References(x => x.OperationCreator);

            References(x => x.EmployeeCard);
            References(x => x.WorkflowItem);
           
        }
    }
}