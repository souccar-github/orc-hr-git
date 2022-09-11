#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 26/02/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using HRIS.Domain.Personnel.Configurations;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Validation.Specification.Personnel.Configurations
{
    public class DefineCustodiesDetailsSpecification : Validates<CustodieDetails>
    {
        public DefineCustodiesDetailsSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            //Check(x => x.SerialNumber).Required();
            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Description).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Cost).Required().GreaterThan(0);
            Check(x => x.PurchaseDate).Required();
            Check(x => x.DepreciationPeriod).Required().GreaterThan(0);
            Check(x => x.Period).Required();
            #endregion
            #region Indexes
            Check(x => x.CustodiesType)
               .Required()
               .Expect((defineCustodiesDetails, custodiesType) => custodiesType.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.Currency)
               .Required()
               .Expect((defineCustodiesDetails, currency) => currency.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            Check(x => x.Supplier)
               .Required()
               .Expect((defineCustodiesDetails, supplier) => supplier.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion
        }
    }
}
