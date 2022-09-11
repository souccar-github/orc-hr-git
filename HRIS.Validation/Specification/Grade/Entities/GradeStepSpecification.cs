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
    public class GradeStepSpecification:Validates<GradeStep>
    {
        public GradeStepSpecification()
        {
            IsDefaultForType();

            #region Primitive types

            Check(x => x.Name).Required();
            Check(x => x.Order).Required();
            Check(x => x.MinSalary).Required().GreaterThanEqualTo(0);
            Check(x => x.MaxSalary).Required().GreaterThan(x => x.MinSalary);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            
            #endregion

            #region Indexs

            Check(x => x.CurrencyType)
                  .Required()
                  .Expect((employee, currencyType) => currencyType.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
          
        }
    }
}
