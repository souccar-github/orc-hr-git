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
    public class EndingSecondaryPositionEmployeeSpecification : Validates<EndingSecondaryPositionEmployee>
    {
        public EndingSecondaryPositionEmployeeSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.Position).Required();
            Check(x => x.LeavingDate).Required();
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            #endregion
            #region Indexes
            #endregion

        }
    }
}
