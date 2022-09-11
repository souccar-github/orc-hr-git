using System;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class RJobRelatedInfoSpecification : Validates<RJobRelatedInfo>
    {
        public RJobRelatedInfoSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.ApplicationDate).Required().LessThanEqualTo(DateTime.Today);
            Check(x => x.LaborOfficeRegistrationDate).Required().LessThan(DateTime.Today);
            Check(x => x.WorkIdentificationNumber).Required();
            Check(x => x.WorkIdentificationDate).Required();

            #endregion

            #region Indexes

            Check(x => x.City)
                .Required()
                .Expect((jobRelatedInfo, city) => city.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
