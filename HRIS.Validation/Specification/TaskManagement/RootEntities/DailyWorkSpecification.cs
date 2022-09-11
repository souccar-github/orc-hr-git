using HRIS.Domain.TaskManagement.RootEntities;
using HRIS.Validation;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.TaskManagement.RootEntities
{
   
    public class DailyWorkSpecification : Validates<DailyWork>
    {
        public DailyWorkSpecification()
        {

            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Date).Required();
            
            Check(x => x.Progress).Required().Between(0,100);
           
            #endregion

            #region Indexes

            Check(x => x.Task)
                .Required()
                .Expect((evaluationSettings, grade) => grade.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
