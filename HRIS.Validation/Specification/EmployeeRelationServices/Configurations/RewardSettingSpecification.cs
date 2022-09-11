#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 11/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using System;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using HRIS.Domain.EmployeeRelationServices.Configurations;
#endregion

namespace HRIS.Validation.Specification.EmployeeRelationServices.Configurations
{
    public class RewardSettingSpecification : Validates<RewardSetting>
    {
        public RewardSettingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Order1).Required();
            Check(x => x.FixedValue).If(x => x.IsPercentage != true && x.IsAddedToSalary == true).Required().GreaterThanEqualTo(0);
            Check(x => x.Percentage).If(x => x.IsPercentage == true && x.IsAddedToSalary == true).Required().GreaterThanEqualTo(0).And.LessThanEqualTo(100);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion
            #region Indexes
            Check(x => x.RewardType)
                .Required()
                .Expect((rewardSetting, rewardType) => rewardType.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.WorkflowSetting)
                .Required()
                .Expect((rewardSetting, workflowSetting) => workflowSetting.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion
        }
    }
}
