using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Configurations;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Configurations
{
    public class WorkshopSpecification : Validates<Workshop>
    {
        public WorkshopSpecification()
        {
            IsDefaultForType();
            Check(x => x.Number).Required().GreaterThanEqualTo(0);
            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

        }
    }
}
