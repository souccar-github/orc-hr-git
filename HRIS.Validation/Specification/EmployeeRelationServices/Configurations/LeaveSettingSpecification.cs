using System;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Configurations
{
    public class LeaveSettingSpecification : Validates<LeaveSetting>
    {
        public LeaveSettingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.AliasName).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.IntervalDays).Optional().GreaterThanEqualTo(GlobalConstant.MinimumValue);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion

            #region Indexes

            Check(x => x.Type)
                .Required()
                .Expect((leaveSetting, type) => type.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.WorkflowSetting)
                .Required()
                .Expect((leaveSetting, workflowSetting) => workflowSetting.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
