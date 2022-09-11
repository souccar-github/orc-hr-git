using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Core;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class MonthlyEmployeeAdvanceMap : ClassMap<MonthlyEmployeeAdvance>
    {
        public MonthlyEmployeeAdvanceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


           
            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.AdvanceAmount);
            Map(x => x.OperationDate);
           
           


            

            References(x => x.MonthlyCard);
        
           
        }
    }
}