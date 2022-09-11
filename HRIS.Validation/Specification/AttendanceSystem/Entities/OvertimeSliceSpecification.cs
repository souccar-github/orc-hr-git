using HRIS.Domain.AttendanceSystem.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Entities
{
    public class OvertimeSliceSpecification : Validates<OvertimeSlice>
    {
        public OvertimeSliceSpecification()
        {
            IsDefaultForType();
            Check(x => x.StartSlice).Required().GreaterThan(0).And.LessThanEqualTo(x => x.EndSlice);
            Check(x => x.EndSlice).Required().GreaterThan(0);

            Check(x => x.NormalPercentage).Required().GreaterThan(0);
            Check(x => x.NormalValue).Optional().GreaterThanEqualTo(0);
            Check(x => x.HolidayPercentage).Required().GreaterThan(0);
            Check(x => x.HolidayValue).Optional().GreaterThanEqualTo(0);
            Check(x => x.ParticularShiftPercentage).Required().GreaterThan(0);
            Check(x => x.ParticularShiftValue).Optional().GreaterThanEqualTo(0);
        }
    }
}
