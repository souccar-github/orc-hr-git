#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 05/03/2015
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
    public class EmployeeTerminationSpecification : Validates<EmployeeTermination>
    {
        public EmployeeTerminationSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            //Check(x => x.Comment).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.LastWorkingDate).Required();
            Check(x => x.TerminationReason).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            #endregion
            #region Indexes
            #endregion

        }
    }
}
