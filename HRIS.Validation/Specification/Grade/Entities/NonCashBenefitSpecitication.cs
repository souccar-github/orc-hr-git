using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HRIS.Domain.Grades.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Grade.Entities
{
   
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class NonCashBenefitSpecitication : Validates<NonCashBenefit>
    {
        public NonCashBenefitSpecitication()
        {
            IsDefaultForType();

            #region Primitive types
            
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexs

            Check(x => x.Type)
                   .Required()
                   .Expect((employee, majorType) => majorType.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            
            #endregion

        }
    }
}
