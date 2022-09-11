using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Helpers.Resource
{
    public class TrainingLocalizationHelper
    {
        public const string ResourceGroupName = "TrainingModule";
        public const string ActivateTrainingCourse = "ActivateTrainingCourse";
        public const string TrainingCourseCancellation = "TrainingCourseCancellation";
        public const string SaveResult = "SaveResult";
        public const string ApproachByCompetence = "ApproachByCompetence";
        public const string ApproachByEducation = "ApproachByEducation";
        public const string ApproachByExperiance = "ApproachByExperiance";
        public const string ApproachByCertificates = "ApproachByCertificates";
        public const string ApproachBySkills = "ApproachBySkills";
        public const string ApproachByLanguage = "ApproachByLanguage";
        public const string AddTrainingNeedsToCourse = "AddTrainingNeedsToCourse";
        public const string CompetenceWeight = "CompetenceWeight";
        public const string EducationWeight = "EducationWeight";
        public const string ExperianceWeight = "ExperianceWeight";
        public const string CertificatesWeight = "CertificatesWeight";
        public const string SkillsWeight = "SkillsWeight";
        public const string CloseTheTrainingCourse = "CloseTheTrainingCourse";
        public const string Level = "Level";
        public const string AllTrainingNeeds = "AllTrainingNeeds";
        public const string SelectedTrainingNeeds = "SelectedTrainingNeeds";
        public const string AddSelected = "AddSelected";
        public const string RemoveSelected = "RemoveSelected";
        public const string TheCourseStatusMustBePlanned = "TheCourseStatusMustBePlanned";
        public const string TheCourseStatusMustBePlannedOrActivated = "TheCourseStatusMustBePlannedOrActivated";
        public const string TheCourseStatusMustBeActivated = "TheCourseStatusMustBeActivated";
        public const string YouMustSelectAtLeastOneRow = "YouMustSelectAtLeastOneRow";
        public const string SuggestStaffsToTrainingCourse = "SuggestStaffsToTrainingCourse";
        public const string AllEmployees = "AllEmployees";
        public const string SelectedEmployees = "SelectedEmployees";
        public const string AddTraineesToTrainingCourse = "AddTraineesToTrainingCourse";
        public const string NumberOfTraineesMustBeEqualOrLessThanNumberOfEmployees =
            "NumberOfTraineesMustBeEqualOrLessThanNumberOfEmployees";

        public const string AreYouSureYouWantToCloseTheCourse = "AreYouSureYouWantToCloseTheCourse";
        public const string TheCourseHasBeenCanceledSuccessfully = "TheCourseHasBeenCanceledSuccessfully";
        public const string TheCourseHasBeenClosedSuccessfully = "TheCourseHasBeenClosedSuccessfully";
        public const string TheCourseHasBeenActivatedSuccessfully = "TheCourseHasBeenActivatedSuccessfully";

        public const string ThereIsPredefinedAppraisalForTheSameKpiAndLevel = "ThereIsPredefinedAppraisalForTheSameKpiAndLevel";
        public const string CourseDateMustBeWithinTheScopeOfThePlanDate = "CourseDateMustBeWithinTheScopeOfThePlanDate";
        public const string CoursePlannedDateMustBeWithinTheScopeOfThePlanDate = "CoursePlannedDateMustBeWithinTheScopeOfThePlanDate";
        public const string TrainingDashboard = "TrainingDashboard";

        public const string ThereIsAnotherAppraisalForSameTraineeAndSameCourse =
            "ThereIsAnotherAppraisalForSameTraineeAndSameCourse";

        public const string NumberOfHoursOfAbsentLessThanOrEqualCourseHours =
            "NumberOfHoursOfAbsentLessThanOrEqualCourseHours";

        public const string TraineesNotAttendedTheCourse = "TraineesNotAttendedTheCourse";
        public const string TraineesAttendedTheCourse = "TraineesAttendedTheCourse";
        public const string CourseAttendanceRate = "CourseAttendanceRate";
        public const string NumberOfTrainees= "NumberOfTrainees";
        public const string NumberOfTraineesFrom= "NumberOfTraineesFrom";
        public const string NumberOfCandidates = "NumberOfCandidates";
        public const string NumberOfEmployees = "NumberOfEmployees";
        public const string EmployeeParticipationRateInTheCourses = "EmployeeParticipationRateInTheCourses";
        public const string NumberOfCoursesTheEmployeeParticipated = "NumberOfCoursesTheEmployeeParticipated";
        public const string CourseCount = "CourseCount";
        public const string TrainingNeedsPercentage = "TrainingNeedsPercentage";
        public const string Appraisal = "Appraisal";
        public const string Probation = "Probation";
        public const string ManualEntry = "ManualEntry";
        public const string Planned = "Planned";
        public const string Activated = "Activated";
        public const string Closed = "Closed";
        public const string Cancelled = "Cancelled";
        public const string NumberOfTraineesAndCandidatesForEachTrainingCourse =
            "NumberOfTraineesAndCandidatesForEachTrainingCourse";
        public const string NumberOfCoursesPerTraineesNodes = "NumberOfCoursesPerTraineesNodes";
        public const string NumberOfCourses = "NumberOfCourses";

        public const string NumberOfTraineesInEachCourseDistributedByNode =
            "NumberOfTraineesInEachCourseDistributedByNode";

        public static string GetResource(string key)
        {
            var result = ServiceFactory.LocalizationService.GetResource(ResourceGroupName + "_" + key);
            return string.IsNullOrEmpty(result) ? key.ToCapitalLetters() : result;
        }

    }
}