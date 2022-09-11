using System;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.EmployeeRelationServices.RootEntities
{
    public class ChangeableHolidaySpecification : Validates<ChangeableHoliday>
    {
        public ChangeableHolidaySpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.StartDate).Required();
            Check(x => x.EndDate).Required().GreaterThanEqualTo(x => x.StartDate);

            #endregion

            #region Indexes

            Check(x => x.HolidayName)
                .Required()
                .Expect((changeableHoliday, name) => name.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion
        }
    }
}
