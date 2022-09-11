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
using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
#endregion

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class EmployeeCustodieSpecification : Validates<EmployeeCustodie>
    {
        public EmployeeCustodieSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.Quantity).Required();
            Check(x => x.CustodyStartDate).Required();
            #endregion

            #region Indexes
            Check(x => x.CustodyName)
                .Required()
                .Expect((employeeCustodie, custodyName) => custodyName.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.AdditionalInformation)
                 .Required()
                 .Expect((employeeCustodie, custodyName) => custodyName.IsTransient() == false, "")
                 .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
