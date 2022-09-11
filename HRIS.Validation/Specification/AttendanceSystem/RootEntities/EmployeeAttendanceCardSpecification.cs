//using HRIS.Domain.AttendanceSystem.RootEntities;
//using HRIS.Validation.MessageKeys;
//using SpecExpress;

//namespace HRIS.Validation.Specification.AttendanceSystem.RootEntities
//{
//    public class EmployeeAttendanceCardSpecification : Validates<EmployeeAttendanceCard>
//    {
//        public EmployeeAttendanceCardSpecification()
//        {
//            IsDefaultForType();
//            Check(x => x.EmployeeMachineCode).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

//            Check(x => x.Employee)
//                    .Required()
//                    .Expect((employeeAttendanceCard, employee) => employee.IsTransient() == false, "")
//                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

//            Check(x => x.AttendanceForm)
//                    .Required()
//                    .Expect((employeeAttendanceCard, attendanceForm) => attendanceForm.IsTransient() == false, "")
//                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

//            Check(x => x.LatenessForm)
//                    .Required()
//                    .Expect((employeeAttendanceCard, latenessForm) => latenessForm.IsTransient() == false, "")
//                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

//            Check(x => x.AbsenceForm)
//                    .Required()
//                    .Expect((employeeAttendanceCard, absenceForm) => absenceForm.IsTransient() == false, "")
//                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            
//            Check(x => x.OvertimeForm)
//                    .Required()
//                    .Expect((employeeAttendanceCard, overtimeForm) => overtimeForm.IsTransient() == false, "")
//                    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));



//        }
//    }
//}
