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
    public class EmployeeTransferSpecification : Validates<EmployeeTransfer>
    {
        public EmployeeTransferSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            //Check(x => x.AssigningDate).Required();
            Check(x => x.LeavingDate).Required();
            Check(x => x.StartingDate).Required();
            Check(x => x.TransferReason).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            //Check(x => x.Weight).If(x => x.IsPrimary != true).Required().GreaterThanEqualTo(0).And.LessThanEqualTo(100);
             
            #endregion
            #region Indexes
           Check(x => x.DestinationJobTitle)
                .Required()
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
             #endregion

        }
    }
}
