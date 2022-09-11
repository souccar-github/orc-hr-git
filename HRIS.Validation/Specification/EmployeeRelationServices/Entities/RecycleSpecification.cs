using System;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.Entities
{
    public class RecycleSpecification : Validates<Recycle>
    {
        public RecycleSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.RecycleType).Required();
            Check(x => x.Year).Required().GreaterThan(GlobalConstant.MinimumValue);

            #endregion

            #region Indexes

            #endregion
        }
    }
}
