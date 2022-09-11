using HRIS.Domain.AttendanceSystem.Configurations;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.AttendanceSystem.Configurations
{
    public class GeneralSettingsSpecification : Validates<GeneralSettings>
    {
        public GeneralSettingsSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.AttendanceCycleStartDay).Required();
            #endregion

            #region Indexes
            Check(x => x.AttendanceForm).Required();
            Check(x => x.LatenessForm).Required();
            Check(x => x.OvertimeForm).Required();
            Check(x => x.AbsenceForm).Required();
            #endregion
        }
    }
}
