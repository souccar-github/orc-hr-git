#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Recruitment.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Recruitment.Entities
{
    public sealed class RJobRelatedInfoMap : ClassMap<RJobRelatedInfo>
    {
        public RJobRelatedInfoMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.ApplicationNumber).Default("0").Not.Nullable();

            Map(x => x.ApplicationDate).Not.Nullable();

            References(x => x.City).Not.Nullable();

            Map(x => x.LaborOfficeRegistrationDate).Not.Nullable();

            Map(x => x.WorkIdentificationNumber).Not.Nullable();

            Map(x => x.WorkIdentificationDate).Not.Nullable();

            Map(x => x.IsWorkPreviously).Not.Nullable();

            References(x => x.WorkSide);

            Map(x => x.WorkSideAgreementNumber);

            Map(x => x.WorkSideAgreementDate);

            Map(x => x.WorkSideAgreementFileName);

            Map(x => x.IsFamiliesMartyrs);

            References(x => x.KinshipType);

            Map(x => x.DocumentNumber);

            Map(x => x.DocumentDate);

            References(x => x.IssuedBy);

            References(x => x.Applicant);

        }
    }
}