using HRIS.Domain.Training.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using HRIS.Validation;

namespace HRIS.Validation.Specification.Training.Entities
{
    public class AppraisalTraineeSpecification: Validates<AppraisalTrainee>
    {
        public AppraisalTraineeSpecification()
        {
            IsDefaultForType();

            Check(x => x.Score).Optional().Between(1,100);
            Check(x => x.ExamDate).Required();
            Check(x => x.NumberOfHoursOfAbsence).Required().GreaterThan(0);
            Check(x => x.TrainerNote).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.AbsenceReason).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Employee)
                .Required()
                .Expect((appraisalTrainee, employee) => employee.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Level)
                .Required()
                .Expect((appraisalTrainee, level) => level.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
