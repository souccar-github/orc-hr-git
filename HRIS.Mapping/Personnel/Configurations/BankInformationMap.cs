using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Configurations;

namespace HRIS.Mapping.Personel.Configurations
{
    public sealed class BankInformationMap : ClassMap<BankInformation>
    {
        public BankInformationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            References(x => x.Nationality);

            Map(x => x.PhoneNumber);
            Map(x => x.Title);
            Map(x => x.ContactPerson);
            Map(x => x.JobTitle);
            //Map(x => x.AuditState);
            Map(x => x.BankName);

        }
    }

}
