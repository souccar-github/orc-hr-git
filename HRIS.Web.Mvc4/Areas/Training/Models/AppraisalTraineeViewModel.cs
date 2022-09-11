using System;
using System.Collections.Generic;
using System.Linq;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using System.Web.Mvc;
using HRIS.Domain.Training.Entities;
using HRIS.Domain.Training.Enums;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;

namespace Project.Web.Mvc4.Areas.Training.Models
{
    public class AppraisalTraineeViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(AppraisalTraineeViewModel).FullName;

            
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
            var appraisalTrainee = entity as AppraisalTrainee;
            if (appraisalTrainee != null)
            {
                var course = ServiceFactory.ORMService.GetById<Course>(requestInformation.NavigationInfo.Previous[1].RowId);

                #region Check if there is another appraisal for the same trainee and same course

                var thereIsAnotherAppraisal = course.AppraisalTrainees.Any(x =>
                    x.Employee.Id == appraisalTrainee.Employee.Id && x.Id != appraisalTrainee.Id);

                if (thereIsAnotherAppraisal)
                {
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.ThereIsAnotherAppraisalForSameTraineeAndSameCourse)
                        });
                }

                #endregion

                #region Number of hours of absent less than or equal course hours

                if (appraisalTrainee.NumberOfHoursOfAbsence > course.Duration)
                {
                    var prop = typeof(AppraisalCourse).GetProperty("NumberOfHoursOfAbsence");
                    validationResults.Add(
                        new ValidationResult()
                        {
                            Message = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.NumberOfHoursOfAbsentLessThanOrEqualCourseHours),
                            Property = prop
                        });
                }
                #endregion
            }

        }
    }
}