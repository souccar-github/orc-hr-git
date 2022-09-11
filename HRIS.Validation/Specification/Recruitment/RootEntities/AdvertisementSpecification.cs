using System;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.RootEntities
{
    public class AdvertisementSpecification : Validates<Advertisement>
    {
        public AdvertisementSpecification()
        {

            IsDefaultForType();

            #region Primitive Types

            Check(x => x.RecruitmentType).Required();
            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Code).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CouncilOfMinistersAgreementNo).Required().GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.CouncilOfMinistersAgreementDate).Required();
            Check(x => x.CentralAgencyAgreementNo).Required().GreaterThan(GlobalConstant.MinimumValue);
            Check(x => x.CentralAgencyAgreementDate).Required();
            Check(x => x.Date).Required().LessThanEqualTo(DateTime.Now);
            Check(x => x.StartingDate).Required().GreaterThan(x => x.Date);
            Check(x => x.EndingDate).Required().GreaterThan(x => x.StartingDate);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes

            Check(x => x.BaseAdvertisement)
                .Optional()
                .Expect((advertisement, baseAdvertisement) => baseAdvertisement.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
