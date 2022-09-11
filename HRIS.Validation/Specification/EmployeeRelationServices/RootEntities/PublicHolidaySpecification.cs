using System;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.RootEntities
{
    public class PublicHolidaySpecification : Validates<PublicHoliday>
    {
        public PublicHolidaySpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.DayOfWeek).Required();

            #endregion

            #region Indexes

            #endregion
        }
    }
}
