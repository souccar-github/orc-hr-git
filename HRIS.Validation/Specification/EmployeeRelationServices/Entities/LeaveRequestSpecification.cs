using System;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class LeaveRequestSpecification : Validates<LeaveRequest>
    {
        public LeaveRequestSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.RequestDate).Required();
            Check(x => x.StartDate).Required();
            Check(x => x.EndDate).Required();//.GreaterThanEqualTo(x => x.StartDate);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes

            Check(x => x.LeaveSetting)
                .Required()
                .Expect((leaveRequest, leaveSetting) => leaveSetting.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.LeaveReason)
                .Required()
                .Expect((leaveRequest, leaveReason) => leaveReason.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
