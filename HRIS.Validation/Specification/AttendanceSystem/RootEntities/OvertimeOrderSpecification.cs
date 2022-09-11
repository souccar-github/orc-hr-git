using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.RootEntities
{
    public class OvertimeOrderSpecification : Validates<OvertimeOrder>
    {
        public OvertimeOrderSpecification()
        {
            IsDefaultForType();
            //Check(x => x.Number).Required().GreaterThan(0);
            Check(x => x.FromDate).Required().LessThanEqualTo(x => x.ToDate);
            Check(x => x.ToDate).Required();
            Check(x => x.OvertimeHoursPerDay).Required().GreaterThan(0).And.LessThanEqualTo(24);
            Check(x => x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);


            Check(x => x.Employee)
                    .Required()
                    .Expect((overtimeOrder, employee) => employee.IsTransient() == false, "")
                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            //Check(x => x.EmployeeManager)
            //        .Required()
            //        .Expect((overtimeOrder, employeeManager) => employeeManager.IsTransient() == false, "")
            //        .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
