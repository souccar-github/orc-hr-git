using System;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class BalanceSliceSpecification : Validates<BalanceSlice>
    {
        public BalanceSliceSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.FromYearOfServices).Optional().GreaterThanEqualTo(GlobalConstant.MinimumValue);
            Check(x => x.ToYearOfServices).Required().GreaterThan(x => x.FromYearOfServices);
            Check(x => x.Balance).Required().GreaterThanEqualTo(GlobalConstant.MinimumValue);

            #endregion

            #region Indexes

            #endregion
        }
    }
}
