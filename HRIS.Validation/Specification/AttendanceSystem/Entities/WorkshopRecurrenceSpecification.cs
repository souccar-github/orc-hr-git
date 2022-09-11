using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Entities;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.Entities
{
    public class WorkshopRecurrenceSpecification : Validates<WorkshopRecurrence>
    {
        public WorkshopRecurrenceSpecification()
        {

            IsDefaultForType();
            Check(x => x.RecurrenceOrder).Required().GreaterThan(0);
            Check(x => x.Workshop).Required();
        }
    }
}
