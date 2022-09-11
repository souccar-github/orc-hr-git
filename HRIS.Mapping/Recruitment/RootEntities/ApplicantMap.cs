using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core;
using HRIS.Domain.Recruitment.RootEntities;

namespace HRIS.Mapping.Recruitment.RootEntities
{

    public sealed class ApplicantMap : ClassMap<Applicant>
    {
        public ApplicantMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.FirstName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FatherName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MotherName).Length(GlobalConstant.SimpleStringMaxLength);
            
            Map(x => x.FirstNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FatherNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MotherNameL2).Length(GlobalConstant.SimpleStringMaxLength);

            References(x => x.PlaceOfBirth).Nullable();
            References(x => x.CountryOfBirth).Nullable();
           
            Map(x => x.Gender);

            Map(x => x.MaritalStatus);

            Map(x => x.DateOfBirth);

            Map(x => x.CivilRecordPlaceAndNumber).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.PersonalRecordSource).Length(GlobalConstant.SimpleStringMaxLength);
            
            References(x => x.Nationality).Nullable();

            References(x => x.OtherNationality).Nullable();

            Map(x => x.BloodType).Not.Nullable();
            Map(x => x.Religion).Not.Nullable();
            Map(x => x.MilitaryStatus).Nullable();

            Map(x => x.Address).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Phone).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Mobile).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Email).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.POBox).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.WebSite).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Twitter).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Facebook).Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.IdentificationNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Code).Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.PhotoId);

            Map(x => x.DisabilityExist).Not.Nullable();
            References(x => x.DisabilityType);

            HasMany(x => x.RSpouse).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.RChildren).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.REducations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.RJobRelatedInfos).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }
}