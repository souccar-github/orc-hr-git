using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;
using Souccar.Core;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class LoanPaymentMap : ClassMap<LoanPayment>
    {
        public LoanPaymentMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.PaymentValue);
            Map(x => x.IsExternalPayment);
            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);
            //Map(x => x.AuditState);
            Map(x => x.RemainingValueAfterPaymentValue);
            References(x => x.EmployeeLoan);
            References(x => x.MonthlyCard);
        }
    }
}