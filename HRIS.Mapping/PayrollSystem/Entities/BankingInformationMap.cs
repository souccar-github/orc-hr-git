using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public sealed class BankingInformationMap : ClassMap<BankingInformation>
    {
        public BankingInformationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            References(x => x.BankInformation).Not.Nullable();

            Map(x => x.AccountNumber).Not.Nullable();
            Map(x => x.AccountName).Not.Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.ExpiryDate).Nullable();
            //Map(x => x.AuditState);
            //Map(x => x.AccountStartDate);
            References(x => x.EmployeeCard).Not.Nullable();


        }
    }
}
