using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Entities
{
    public class NonAttendanceSliceSpecification : Validates<NonAttendanceSlice>
    {
        public NonAttendanceSliceSpecification()
        {
            IsDefaultForType();
            Check(x => x.StartSlice).Required().GreaterThan(0).And.LessThanEqualTo(x => x.EndSlice);
            Check(x => x.EndSlice).Required().GreaterThan(0);
            Check(x => x.Value).Optional().GreaterThanEqualTo(0);

            Check(x => x.InfractionForm)
                   .Optional()
                   .Expect((nonAttendanceSlice, infractionForm) => infractionForm.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
