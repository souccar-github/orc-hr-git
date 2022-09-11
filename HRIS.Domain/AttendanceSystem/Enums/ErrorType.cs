using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.Enums
{
    public enum ErrorType
    { //todo : Mhd Update changeset no.1
        None,
        MultipleEntrance,
        MultipleExit,
        EntranceWithoutExit,
        ExitWithoutEntrance,
        EntranceTimeGreaterThanExitTime,
        OutOfShiftRange

    }
}
