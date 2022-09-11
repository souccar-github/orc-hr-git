#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 04/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using System;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using HRIS.Domain.EmployeeRelationServices.Enums;
#endregion

namespace HRIS.Validation.Specification.EmployeeRelationServices.Configurations
{
    public class DisciplinarySettingSpecification : Validates<DisciplinarySetting>
    {
        public DisciplinarySettingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Order1).Required();
            Check(x => x.Value).If(x => x.IsDeductFromSalary == true).Required().GreaterThanEqualTo(0);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion
            #region Indexes
            Check(x => x.DisciplinaryType)
                .Required()
                .Expect((disciplinarySetting, disciplinaryType) => disciplinaryType.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.WorkflowSetting)
                .Required()
                .Expect((disciplinarySetting, workflowSetting) => workflowSetting.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion
        }
    }
}
