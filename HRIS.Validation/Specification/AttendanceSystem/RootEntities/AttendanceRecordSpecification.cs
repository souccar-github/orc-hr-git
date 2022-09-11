using HRIS.Domain.AttendanceSystem.RootEntities;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.AttendanceSystem.RootEntities
{
    public class AttendanceRecordSpecification : Validates<AttendanceRecord>
    {
        public AttendanceRecordSpecification()
        {
            IsDefaultForType();

            Check(x => x.Year).Required().GreaterThan(2000);
            Check(x => x.Month).Required();
            Check(x => x.Name).Required();
            Check(x => x.FromDate).Required();
            Check(x => x.ToDate).Required().GreaterThan(x=> x.FromDate);
            Check(x => x.Note).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

        }
    }
}
