using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Entities
{
    public class ParticularOvertimeShiftSpecification : Validates<ParticularOvertimeShift>
    {
        public ParticularOvertimeShiftSpecification()
        {

            IsDefaultForType();

            Check(x => x.StartTime).Required();
            Check(x => x.EndTime).Required();
        }
    }
}
