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
    public class OralExaminationSpecification : Validates<OralExamination>
    {
        public OralExaminationSpecification()
        {

            IsDefaultForType();

            #region Primitive Types

            Check(x => x.AcceptedPersonsDecisionNumber).Required();
            Check(x => x.AcceptedPersonsDecisionDate).Required();
            Check(x => x.Date).Required();
            Check(x => x.Time).Required();

            #endregion

            #region Indexes

            Check(x => x.Place)
                .Required()
                .Expect((oralExamination, place) => place.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
