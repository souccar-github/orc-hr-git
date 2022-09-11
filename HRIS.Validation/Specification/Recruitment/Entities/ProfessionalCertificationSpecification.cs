using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Recruitment.Entities;
using SpecExpress;
using HRIS.Validation.MessageKeys;
using SpecExpress.DSL;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class ProfessionalCertificationSpecification:Validates<ProfessionalCertification>
    {
        public ProfessionalCertificationSpecification()
        {
            IsDefaultForType();

            Check(x => x.DateOfIssuance).Optional().LessThan(x => x.ExpirationDate)
                .With(x =>
                    x.MessageKey = CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule
                        .CertificationDateOfIssuanceMustBeLessThanCertificationExpiry))
                .And
                .LessThan(DateTime.Now.Date).With(x =>
                    x.MessageKey =
                        CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule
                            .TheValueMustBeLessThanCurrentDate));

            Check(x => x.Type)
                .Required()
                .Expect((professionalCertification, certificationName) => certificationName.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
