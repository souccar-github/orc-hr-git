using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Configurations
{
    public class TemporaryWorkshopSpecification : Validates<TemporaryWorkshop>
    {
        public TemporaryWorkshopSpecification()
        {

            IsDefaultForType();

            Check(x => x.FromDate).Required().LessThanEqualTo(x=>x.ToDate);
            Check(x => x.ToDate).Required();

            Check(x => x.AlternativeWorkshop)
                    .Required()
                    .Expect((temporaryWorkshop, alternativeWorkshop) => alternativeWorkshop.IsTransient() == false, "")
                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
