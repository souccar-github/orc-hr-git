using HRIS.Domain.AttendanceSystem.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Entities
{
    public class NonAttendanceSlicePercentageSpecification : Validates<NonAttendanceSlicePercentage>
    {
        public NonAttendanceSlicePercentageSpecification()
        {

            IsDefaultForType();
            Check(x => x.PercentageOrder).Required().GreaterThan(0);
            Check(x => x.Percentage).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
        }
    }
}
