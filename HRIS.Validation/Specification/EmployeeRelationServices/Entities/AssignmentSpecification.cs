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
    public class AssignmentSpecification : Validates<Assignment>
    {
        public AssignmentSpecification()
        {
            IsDefaultForType();
            #region Primitive Types

            Check(x => x.AssigningDate).Required();
            Check(x => x.PositionCode).Required();
            //Check(x => x.Weight).If(x => x.IsPrimary != true).Required().GreaterThanEqualTo(0).And.LessThanEqualTo(100);
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            #endregion
            #region Indexes

            Check(x => x.JobTitle)
                .Required()
                .Expect((assignment, jobTitle) => jobTitle.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion

        }
    }

}