#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 08/03/2015
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
    public class EmployeeDisciplinarySpecification : Validates<EmployeeDisciplinary>
    {
        public EmployeeDisciplinarySpecification()
        {
            IsDefaultForType();
            #region Primitive Types

            Check(x => x.DisciplinaryDate).Required();
            Check(x => x.DisciplinaryReason).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion
            #region Indexes
            Check(x => x.DisciplinarySetting)
                .Required()
                .Expect((employeeDisciplinary, disciplinarySetting) => disciplinarySetting.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion

        }
    }
}
