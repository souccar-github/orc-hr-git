using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.AttendanceSystem.Helpers
{
    public static class AttendanceSystemGroupNames
    {

        public const string ResourceGroupName = "AttendanceSystemGroupNames";

        public const string ThisShiftOrderIsAlreadyExist = "ThisShiftOrderIsAlreadyExist";
        public const string YouMustAddOneOvertimeSliceAtLeast = "YouMustAddOneOvertimeSliceAtLeast";
        public const string YouMustAddOneWorkshopRecurrencesAtLeast = "YouMustAddOneWorkshopRecurrencesAtLeast";
        public const string YouMustAddOneInfractionSliceAtLeast = "YouMustAddOneInfractionSliceAtLeast";
        public const string YouMustAddOneNormalShiftAtLeast = "YouMustAddOneNormalShiftAtLeast";
        public const string YouMustAddOneNonAttendanceSliceAtLeast = "YouMustAddOneNonAttendanceSliceAtLeast";
        public const string YouMustAddOneNonAttendanceSlicePercentageAtLeast = "YouMustAddOneNonAttendanceSlicePercentageAtLeast";



        public static string GetResourceKey(string key)
        {
            return string.Format("{0}_{1}", ResourceGroupName, key);
        }
    }
}
