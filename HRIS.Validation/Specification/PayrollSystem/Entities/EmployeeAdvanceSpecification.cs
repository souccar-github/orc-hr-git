using System;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class EmployeeAdvanceSpecification : Validates<EmployeeAdvance>
    {
        public EmployeeAdvanceSpecification()
        {
            IsDefaultForType();

            Check(x => x.AdvanceAmount).Required().GreaterThan(0);
             Check(x => x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

        }
    }
}
