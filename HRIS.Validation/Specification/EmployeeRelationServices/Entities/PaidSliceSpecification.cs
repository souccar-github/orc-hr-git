using System;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class PaidSliceSpecification : Validates<PaidSlice>
    {
        public PaidSliceSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.FromBalance).Required().GreaterThanEqualTo(GlobalConstant.MinimumValue);
            Check(x => x.ToBalance).Required().GreaterThan(x => x.FromBalance);
            Check(x=>x.PaidPercentage).Optional().GreaterThanEqualTo(0).And.LessThanEqualTo(100);
            #endregion

            #region Indexes

            #endregion
        }
    }
}
