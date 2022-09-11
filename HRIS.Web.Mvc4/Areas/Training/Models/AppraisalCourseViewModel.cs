using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.Training.Entities;
using HRIS.Domain.Training.Enums;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Training.Models
{
    public class AppraisalCourseViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AppraisalCourseViewModel).FullName;
        }

        public override ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
        {
            var course = ServiceFactory.ORMService.GetById<Course>(requestInformation.NavigationInfo.Previous[1].RowId);

            if (course != null && course.Status != CourseStatus.Activated)
            {
                var message =
                    TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.TheCourseStatusMustBeActivated);
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = false, message = message });
            }

            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = true, message = "" });
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var appraisalCourse = entity as AppraisalCourse;

            var course = ServiceFactory.ORMService.GetById<Course>(requestInformation.NavigationInfo.Previous[1].RowId);
            if (appraisalCourse != null)
            {
                #region NumberOfTrainees
                if (appraisalCourse.NumberOfTrainees > course.CourseEmployees.Count(x=>x.Type == CourseEmployeeType.Trainee))
                {
                    var prop = typeof(AppraisalCourse).GetProperty("NumberOfTrainees");
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.NumberOfTraineesMustBeEqualOrLessThanNumberOfEmployees),
                            Property = prop
                        });
                }
                #endregion

                #region There is a predefined Appraisal for the same Kpi and level

                var thereIsSameKpi = course.AppraisalCourses.Any(x =>
                    x.AppraisalKpi == appraisalCourse.AppraisalKpi &&
                    x.AppraisalLevel == appraisalCourse.AppraisalLevel && x.Id != appraisalCourse.Id);

                if (thereIsSameKpi)
                {
                    var prop = typeof(AppraisalCourse).GetProperty("AppraisalKpi");

                    validationResults.Add(new ValidationResult()
                    {
                        Message = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.ThereIsPredefinedAppraisalForTheSameKpiAndLevel),
                        Property = prop
                    });
                }

                #endregion
            }
            
        }


        
    }
}