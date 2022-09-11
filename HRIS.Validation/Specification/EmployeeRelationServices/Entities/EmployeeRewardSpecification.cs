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
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
#endregion
namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class EmployeeRewardSpecification : Validates<EmployeeReward>
    {
        public EmployeeRewardSpecification()
        {
            IsDefaultForType();
            #region Primitive Types

            Check(x => x.RewardDate).Required();
            Check(x => x.RewardReason).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion
            #region Indexes
            Check(x => x.RewardSetting)
                .Required()
                .Expect((employeeReward, rewardSetting) => rewardSetting.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion

        }
    }
}
