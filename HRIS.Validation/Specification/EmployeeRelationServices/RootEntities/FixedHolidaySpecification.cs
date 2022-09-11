using System;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.RootEntities
{
    public class FixedHolidaySpecification : Validates<FixedHoliday>
    {
        public FixedHolidaySpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Day).Required();
            Check(x => x.Month).Required();
            //Check(x => x.NumberOfHolidayDays).Required().LessThanEqualTo(GlobalConstant.DaysCountOfYearValue).And.GreaterThanEqualTo(GlobalConstant.MinimumValue);
            
            #endregion

            #region Indexes

            Check(x => x.HolidayName)
                .Required()
                .Expect((fixedHoliday, name) => name.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
