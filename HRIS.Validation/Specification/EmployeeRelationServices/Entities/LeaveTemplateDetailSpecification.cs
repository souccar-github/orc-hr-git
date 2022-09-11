using System;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class LeaveTemplateDetailSpecification : Validates<LeaveTemplateDetail>
    {
        public LeaveTemplateDetailSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            #endregion

            #region Indexes

            Check(x => x.LeaveSetting)
                .Required()
                .Expect((leaveTemplateDetail, leaveSetting) => leaveSetting.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
