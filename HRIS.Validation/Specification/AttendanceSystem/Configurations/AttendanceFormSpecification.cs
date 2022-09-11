using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Configurations;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Configurations
{
    public class AttendanceFormSpecification : Validates<AttendanceForm>
    {
        public AttendanceFormSpecification()
        {
            IsDefaultForType();
            Check(x => x.Number).Required().GreaterThan(0);
            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.CalculationMethod).Required();
        }
    }
}
