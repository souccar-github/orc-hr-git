using HRIS.Domain.AttendanceSystem.RootEntities;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.AttendanceSystem.RootEntities
{
    public class DailyEnternaceExitRecordSpecification : Validates<DailyEnternaceExitRecord>
    {
        public DailyEnternaceExitRecordSpecification()
        {
            IsDefaultForType();

            Check(x => x.Employee).Required();
            Check(x => x.Date).Required();
            Check(x => x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

        }
    }
}
