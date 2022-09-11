using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HRIS.Domain.Grades.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Grade.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class GradeByEducationSpecification : Validates<GradeByEducation>
    {
        public GradeByEducationSpecification()
        {
            IsDefaultForType();

            #region Primitive types

            Check(x => x.Name).Required();
            Check(x => x.Order).Required().GreaterThan(0);

            //Check(x => x.MinSalary).Required().GreaterThanEqualTo(0);
            //Check(x => x.MaxSalary).Required().GreaterThanEqualTo(x => x.MinSalary);
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
