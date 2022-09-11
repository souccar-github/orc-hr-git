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
using HRIS.Domain.EmployeeRelationServices.RootEntities;
#endregion
namespace HRIS.Validation.Specification.EmployeeRelationServices.RootEntities
{
    public class EmployeeInfractionSpecification : Validates<EmployeeInfraction>
    {
        public EmployeeInfractionSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            //Check(x => x.Comment).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Employee).Required();
            Check(x => x.Infraction).Required();
           
            #endregion
            #region Indexes
            #endregion

        }
    }
}
