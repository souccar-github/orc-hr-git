using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.AttendanceSystem.RootEntities
{
    public class EntranceExitRecordSpecification : Validates<EntranceExitRecord>
    {
        public EntranceExitRecordSpecification()
        {
            IsDefaultForType();
            //Check(x => x.EmployeeCode).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.LogType).Required();
            Check(x => x.LogDateTime).Required();
            Check(x => x.LogTime).Required();
            Check(x => x.LogDate).Required();
            Check(x=>x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Employee)
                    .Required()
                    .Expect((entranceExitRecord, employee) => employee.IsTransient() == false, "")
                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
