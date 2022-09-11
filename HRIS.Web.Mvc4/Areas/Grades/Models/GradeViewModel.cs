using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Grades.Entities;
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
    public class GradeViewModel : ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
      
        {
            var oldGrade = typeof(HRIS.Domain.Grades.RootEntities.Grade).GetAll<HRIS.Domain.Grades.RootEntities.Grade>()
                .SingleOrDefault(x => x.Name.Equals((entity as HRIS.Domain.Grades.RootEntities.Grade).Name));
            if (oldGrade != null && oldGrade.Id != entity.Id)
            {
                var prop = typeof(HRIS.Domain.Grades.RootEntities.Grade).GetProperty("Name");
                validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage), Property = prop });
            }

            var grade = entity as HRIS.Domain.Grades.RootEntities.Grade;
            var stepsCount = (grade.Steps!=null) ? grade.Steps.Count : 0;
            if (stepsCount > 0 && grade.Steps.Any(x=>x.MinSalary < grade.MinSalary))
            {
                var prop = typeof(HRIS.Domain.Grades.RootEntities.Grade).GetProperty("MinSalary");
                validationResults.Add(new ValidationResult()
                {
                    Message = GlobalResource.ThereIsAnyStepMinSalaryLessThanGradeMinSalary,
                    Property = prop
                });
            }

            if (stepsCount > 0 && grade.Steps.Any(x => x.MaxSalary > grade.MaxSalary))
            {
                var prop = typeof(HRIS.Domain.Grades.RootEntities.Grade).GetProperty("MaxSalary");
                validationResults.Add(new ValidationResult()
                {
                    Message = GlobalResource.ThereIsAnyStepMaxSalaryGreaterThanGradeMaxSalary,
                    Property = prop
                });
            }

            //var grade = entity as Domain.OrganizationChart.RootEntities.Grade;
            //if (grade.MinSalary < grade.GradeByEducation.MinSalary)
            //{
            //    var prop = typeof(Domain.OrganizationChart.RootEntities.Grade).GetProperty("MinSalary");
            //    validationResults.Add(new ValidationResult()
            //    {
            //        Message = string.Format("{0} {1} {2}", prop.GetTitle(), GlobalResource.GreaterThanEqMessage, grade.GradeByEducation.MinSalary),
            //        Property = prop
            //    });
            //}

            //if (grade.MaxSalary > grade.GradeByEducation.MaxSalary)
            //{
            //    var prop = typeof(Domain.OrganizationChart.RootEntities.Grade).GetProperty("MaxSalary");
            //    validationResults.Add(new ValidationResult()
            //    {
            //        Message = string.Format("{0} {1} {2}", prop.GetTitle(), GlobalResource.LessThanEqMessage, grade.GradeByEducation.MaxSalary),
            //        Property = prop
            //    });
            //}
        }

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(GradeViewModel).FullName;


        }
    }
}