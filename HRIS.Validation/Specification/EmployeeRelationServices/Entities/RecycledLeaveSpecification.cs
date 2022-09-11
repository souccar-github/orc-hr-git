using System;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using HRIS.Validation.Specification.Index;
using HRIS.Domain.EmployeeRelationServices.Enums;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class RecycledLeaveSpecification : Validates<RecycledLeave>
    {
        public RecycledLeaveSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.RoundedBalance).Required();
            Check(x => x.RecycleType).Required();
            Check(x => x.Year).Required().GreaterThan(GlobalConstant.MinimumValue);


            #endregion
            #region Indexes

           
            Check(x => x.LeaveSetting)
                .Required()
                 .Expect((RecycledLeave, LeaveSetting) => LeaveSetting.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
