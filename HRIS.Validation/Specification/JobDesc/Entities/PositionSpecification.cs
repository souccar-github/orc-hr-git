using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using SpecExpress;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
   
    public class PositionSpecification : Validates<Position>
    {
        public PositionSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            //Check(x => x.Code).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.PositionStatusType).Required();
            Check(x => x.WorkingHours).Required().GreaterThanEqualTo(0);
            Check(x => x.PositionStatusType).Required();
            Check(x => x.Budget).Required();
            //Check(x => x.ManagerJobTitle).Required();
            //Check(x => x.Manager).Required();
            Check(x => x.JobDescription).Required();
            //Check(x => x.Step).Required();

            #endregion Primitive Types
          
            #region Indexes

            //Check(x => x.Group)
            //    .Required()
            //    .Expect(IndexSpecification.IsTransient, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Type)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Per)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.CurrencyType)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            //Check(x => x.CostCenter)
            //    .Required()
            //    .Expect(IndexSpecification.IsTransient, "")
            //    .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes
        }
    }
}
