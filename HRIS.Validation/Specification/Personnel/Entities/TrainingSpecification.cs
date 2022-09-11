
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class TrainingSpecification : Validates<HRIS.Domain.Personnel.Entities.Training>
    {
        public TrainingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.CourseName).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CourseDurationPerHour);
            Check(x => x.CourseStartDate).Optional().LessThanEqualTo(DateTime.Now);
            Check(x => x.CourseEndDate).Optional().LessThanEqualTo(DateTime.Now);
             
            Check(x => x.TrainingCenterLocation).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.instructor).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.instructorPhone).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CertificateIssuanceDate).Optional().LessThanEqualTo(DateTime.Now);
            Check(x => x.Notes).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes
            Check(x => x.Specialize)
               .Required()
               .Expect((training, status) => status.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

         
            #endregion 
        }
    }
}
