using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Configurations;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Configurations
{
    public class NonAttendanceFormSpecification : Validates<NonAttendanceForm>
    {
        public NonAttendanceFormSpecification()
        {
            IsDefaultForType();
            Check(x => x.Number).Required().GreaterThan(0);
            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.ResetCounterRecurrence).Required().GreaterThan(0);
            //Check(x => x.InfractionForm).Required();
            Check(x => x.LastReset).Required();

        }
    }
}
