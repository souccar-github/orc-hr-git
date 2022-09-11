using System;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Souccar.Core.CustomAttribute;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.RootEntities
{
    public class HourlyMissionSpecification : Validates<HourlyMission>
    {
        public HourlyMissionSpecification()
        {
            IsDefaultForType();

            Check(x => x.Note).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Date).Required();
            Check(x => x.StartTime).Required();
            Check(x => x.EndTime).Required();
         


            Check(x => x.Employee)
                    .Required()
                    .Expect((hourlyMission, employee) => employee.IsTransient() == false, "")
                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
