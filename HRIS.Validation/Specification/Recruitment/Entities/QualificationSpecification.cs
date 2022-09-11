using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class QualificationSpecification : Validates<Qualification>
    {
        public QualificationSpecification()
        {

            IsDefaultForType();

            #region Primitive Types


            #endregion

            #region Indexes

            Check(x => x.MajorType)
                .Required()
                .Expect((qualification, majorType) => majorType.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Major)
                .Required()
                .Expect((qualification, major) => major.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
