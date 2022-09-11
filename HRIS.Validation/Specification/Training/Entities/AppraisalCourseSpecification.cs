using HRIS.Domain.Training.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.Training.Entities
{
    public class AppraisalCourseSpecification:Validates<AppraisalCourse>
    {
        public AppraisalCourseSpecification()
        {
            IsDefaultForType();

            Check(x => x.NumberOfTrainees).Required().GreaterThan(0);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.AppraisalKpi)
                  .Required()
                  .Expect((appraisalCourse, appraisalKpi) => appraisalKpi.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.AppraisalLevel)
                  .Required()
                  .Expect((appraisalCourse, appraisalLevel) => appraisalLevel.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
