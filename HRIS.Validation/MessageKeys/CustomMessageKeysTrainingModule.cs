
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Validation.MessageKeys
{
    public static class CustomMessageKeysTrainingModule
    {
        public const string ResourceGroupName = "CustomMessageKeysTrainingModule";
        public const string AddCoursesFromNeed = "AddCoursesFromNeed";
        public const string CourseInformation = "CourseInformation";
        public const string StartDateMustBeLessThanEndDate = "StartDateMustBeLessThanEndDate";

        public const string PlannedEndDateMustBeGreaterThanPlannedStartDate =
            "PlannedEndDateMustBeGreaterThanPlannedStartDate";

        public const string EndDateMustBeGreaterThanStartDate =
            "EndDateMustBeGreaterThanStartDate";

        public const string NumberOfTraineesMustBeEqualOrLessThanNumberOfEmployees =
            "NumberOfTraineesMustBeEqualOrLessThanNumberOfEmployees";

        

        public static string GetFullKey(string key)
        {
            return ResourceGroupName + "_" + key;
        }
    }
}
