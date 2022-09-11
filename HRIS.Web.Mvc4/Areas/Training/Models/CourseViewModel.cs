using Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DotNetOpenAuth.Messaging;
using HRIS.Domain.Training.Entities;
using HRIS.Domain.Training.Enums;
using HRIS.Domain.Training.RootEntities;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Infrastructure.Core;
using HRIS.Validation.Specification.Training.Entities;

namespace Project.Web.Mvc4.Areas.Training.Models
{
    public class CourseViewModel: ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(CourseViewModel).FullName;
            model.ActionListHandler = "removeEditOrDeleteFromCourseActionList";
            model.Views[0].EditHandler = "courseEditHandler";
           
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var course = entity as Course;
            var trainingPlan =
                ServiceFactory.ORMService.GetById<TrainingPlan>(requestInformation.NavigationInfo.Previous[0].RowId);

            if (course.PlannedStartDate < trainingPlan.StartDate ||
                course.PlannedEndDate > trainingPlan.EndDate)
            {
                validationResults.Add(
                    new ValidationResult()
                    {
                        Message = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.CoursePlannedDateMustBeWithinTheScopeOfThePlanDate),
                    });
            }

            if (operationType == CrudOperationType.Update && course.Status == CourseStatus.Activated)
            {
                var courseSpecification = new ActivateCourseSpecification();
                var validationList= (List<ValidationResult>)ServiceFactory.ValidationService.Validate(course, courseSpecification);
                if (validationList.Any())
                {
                    validationResults.AddRange(validationList);
                }

                //if (course.StartDate < trainingPlan.StartDate ||
                //    course.EndDate > trainingPlan.EndDate)
                //{
                //    validationResults.Add(
                //        new ValidationResult()
                //        {
                //            Message = TrainingLocalizationHelper.GetResource(TrainingLocalizationHelper.CourseDateMustBeWithinTheScopeOfThePlanDate),
                //        });
                //}

            }
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var course = entity as Course;

            if (course != null)
                course.Status = CourseStatus.Planned;
        }

        public override void BeforeDelete(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var course = entity as Course;
            if (course != null && course.CourseTrainingNeeds.Count > 0)
            {
                PreventDefault = true;
            }
        }
    }
}