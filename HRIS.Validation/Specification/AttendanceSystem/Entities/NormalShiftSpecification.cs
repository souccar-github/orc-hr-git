using HRIS.Domain.AttendanceSystem.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Entities
{
    public class NormalShiftSpecification : Validates<NormalShift>
    {
        public NormalShiftSpecification()
        {
            IsDefaultForType();
            Check(x=>x.NormalShiftOrder).Required().GreaterThan(0);
            Check(x => x.EntryTime).Required();
            Check(x => x.ExitTime).Required();
            Check(x => x.ShiftRangeStartTime).Required();
            Check(x => x.ShiftRangeEndTime).Required();
            Check(x => x.IgnoredPeriodBeforeEntryTime).Optional().GreaterThanEqualTo(0);
            Check(x => x.IgnoredPeriodAfterEntryTime).Optional().GreaterThanEqualTo(0);
            Check(x => x.IgnoredPeriodBeforeExitTime).Optional().GreaterThanEqualTo(0);
            Check(x => x.IgnoredPeriodAfterExitTime).Optional().GreaterThanEqualTo(0);
            Check(x => x.RestPeriod).Optional().GreaterThanEqualTo(0);
            Check(x => x.RestRangeStartTime).Optional();
            Check(x => x.RestRangeEndTime).Optional();
        }
    }
}
