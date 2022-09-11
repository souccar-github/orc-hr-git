using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.OrganizationChart.RootEntities;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

using  Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Grades.Models
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class GradeStepViewModel : ViewModel
    {
        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, CrudOperationType operationType, string customInformation = null)
        {
            var step = entity as GradeStep;
            var grade = ServiceFactory.ORMService.GetById<HRIS.Domain.Grades.RootEntities.Grade>(requestInformation.NavigationInfo.Previous[0].RowId);
            step.Grade = grade;
            base.BeforeValidation(requestInformation, entity, originalState, operationType, customInformation);
        }
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {

            var grade = ServiceFactory.ORMService.GetById<HRIS.Domain.Grades.RootEntities.Grade>(requestInformation.NavigationInfo.Previous[0].RowId);
            var step = grade.Steps.SingleOrDefault(x => x.Name == ((GradeStep)entity).Name);

            var gradeStep = (GradeStep)entity;

            if (step != null && step.Id != entity.Id)
            {
                var prop = typeof(HRIS.Domain.Grades.RootEntities.GradeByEducation).GetProperty("Name");
                validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage), Property = prop });
            }

            if (operationType == CrudOperationType.Insert)
            {
                if (grade.Steps.Any(x => (x.MinSalary <= gradeStep.MinSalary && x.MaxSalary >= gradeStep.MinSalary) ||
                                     (x.MinSalary <= gradeStep.MaxSalary && x.MaxSalary >= gradeStep.MaxSalary)))
                {
                    var prop = typeof(GradeStep).GetProperty("Name");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = GradeLocalizationHelper.MsgThereIsStepInSameRange,
                        Property = prop
                    });

                    return;
                }
            }
            else
            {
                if (grade.Steps.Any(x => x.Id != gradeStep.Id && ((x.MinSalary <= gradeStep.MinSalary && x.MaxSalary >= gradeStep.MinSalary) ||
                                    (x.MinSalary <= gradeStep.MaxSalary && x.MaxSalary >= gradeStep.MaxSalary))))
                {
                    var prop = typeof(GradeStep).GetProperty("Name");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = GradeLocalizationHelper.MsgThereIsStepInSameRange,
                        Property = prop
                    });

                    return;
                }
            }


            if (gradeStep.MinSalary < grade.MinSalary)
            {
                var prop = typeof(GradeStep).GetProperty("MinSalary");
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message = GradeLocalizationHelper.MsgMinSalaryShouldBeGreaterThanOrEqualToGradeMinSalary,
                    Property = prop
                });

                return;
            }

            if (gradeStep.MaxSalary > grade.MaxSalary)
            {
                var prop = typeof(GradeStep).GetProperty("MaxSalary");
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message = GradeLocalizationHelper.MsgMaxSalaryShouldBeSmallerThanOrEqualToGradeMaxSalary,
                    Property = prop
                });

                return;
            }
        }

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(GradeStepViewModel).FullName;
        }
    }
}