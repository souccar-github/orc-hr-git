using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.RootEntities;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.RootEntities
{
    public class TravelMissionSpecification : Validates<TravelMission>
    {
        public TravelMissionSpecification()
        {
            IsDefaultForType();

            Check(x => x.FromDate).Required();
            Check(x => x.ToDate).Required().GreaterThanEqualTo(x => x.FromDate);
            Check(x => x.Employee).Required();
            Check(x => x.Type).Required();
            Check(x => x.Note).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
        }
    }
}
