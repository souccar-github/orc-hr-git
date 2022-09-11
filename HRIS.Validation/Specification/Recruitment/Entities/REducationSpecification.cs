using System;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class REducationSpecification : Validates<REducation>
    {
        public REducationSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.DateOfIssuance).Required().LessThan(DateTime.Today);
            //Check(x => x.University).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.Comments).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes
            Check(x => x.University)
                .Required()
                .Expect((education, university) => university.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Major)
                .Required()
                .Expect((education, major) => major.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Type)
                .Required()
                .Expect((education, type) => type.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Country)
                .Required()
                .Expect((education, country) => country.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Rank)
                .Required()
                .Expect((education, rank) => rank.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.ScoreType)
                .Required()
                .Expect((education, scoreType) => scoreType.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
