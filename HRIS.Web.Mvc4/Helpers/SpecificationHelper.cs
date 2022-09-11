using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Validation.Specification.EmployeeRelationServices.Entities;
using  Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using SpecExpress;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Validation.Specification.Personnel.RootEntities;

namespace Project.Web.Mvc4.Helpers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class SpecificationHelper
    {
        public static SpecificationBase GetSpecificationType(RequestInformation requestInformation, Entity entity)
        {
            var previous = requestInformation.NavigationInfo.Previous;
            //if (previous.Count == 1 && previous[0].Name == "TrainingNeed")
            //    return new TrainingNeedSpecification();
            //if (previous.Count == 1 && previous[0].Name == "Course")
            //    return new CourseSpecification();
            //if (previous.Count == 2 && previous[0].Name == "TrainingNeed" && previous[1].Name == "Courses")
            //    return new SummaryCourseSpecification();
            //if (previous.Count == 2 && previous[0].Name == "TrainingPlan" && previous[1].Name == "Courses")
            //    return new CourseSpecificationForPlan();
            //if (previous.Count == 2 && previous[0].Name == "Course" && previous[1].Name == "Conditions")
            //    return new CourseConditionSpecification();
            //if (previous.Count == 1 && previous[0].Name == "TrainingPlan")
            //    return new TrainingPlanSpecification();
           
            return null;
        }
        
    }
}