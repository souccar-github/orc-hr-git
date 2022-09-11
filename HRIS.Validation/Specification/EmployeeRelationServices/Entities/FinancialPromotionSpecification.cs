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
    public class FinancialPromotionSpecification : Validates<FinancialPromotion>
    {
        public FinancialPromotionSpecification()
        {
            IsDefaultForType();
            #region Primitive Types

            Check(x => x.FixedValue).If(x => x.IsPercentage != true).Required().GreaterThanEqualTo(0);
            Check(x => x.Percentage).If(x => x.IsPercentage == true).Required().GreaterThanEqualTo(0).And.LessThanEqualTo(100);
        
            Check(x => x.CreationDate).Required();
            Check(x => x.Salary).Required();
            Check(x => x.Comment).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            #endregion
            #region Indexes
            #endregion

        }
    }

}