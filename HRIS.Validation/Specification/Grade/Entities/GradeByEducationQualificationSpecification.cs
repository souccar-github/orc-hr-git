
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using HRIS.Domain.Grades.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Grade.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class GradeByEducationQualificationSpecification : Validates<GradeByEducationQualification>
    {
        public GradeByEducationQualificationSpecification()
        {
            IsDefaultForType();

            #region Primitive types

            Check(x => x.FirstSalary).Required();
            Check(x => x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            //Check(x => x.FirstSalary).Required().GreaterThanEqualTo(0);
            
            #endregion

            #region Indexs

            Check(x => x.MajorType)
                   .Required()
                   .Expect((employee, majorType) => majorType.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Major)
                  .Required()
                  .Expect((employee, major) => major.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

             Check(x => x.CurrencyType)
                   .Required()
                   .Expect((employee, currencyType) => currencyType.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
          
        }
    }
}
