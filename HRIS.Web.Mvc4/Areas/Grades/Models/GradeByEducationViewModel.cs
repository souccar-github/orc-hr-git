using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.OrganizationChart.RootEntities;
using  Project.Web.Mvc4.Models;
using Microsoft.Ajax.Utilities;
using Souccar.Domain.DomainModel;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

using Souccar.Domain.Validation;
using  Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Core;

using HRIS.Validation.MessageKeys;

namespace Project.Web.Mvc4.Areas.Grades.Models
{
    public class GradeByEducationViewModel:ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var grade = typeof(GradeByEducation).GetAll<GradeByEducation>()
                .SingleOrDefault(x => x.Name.Equals((entity as GradeByEducation).Name));
            if (grade != null && grade.Id != entity.Id)
            {
                var prop = typeof(HRIS.Domain.Grades.RootEntities.GradeByEducation).GetProperty("Name");
                validationResults.Add(new ValidationResult() { Message =string.Format("{0} {1}", prop.GetTitle(), GlobalResource.AlreadyexistMessage), Property =prop });
            } 
            var GradebyEducation = entity as GradeByEducation;
            if (GradebyEducation.MaxSalary < GradebyEducation.MinSalary)
            {
                validationResults.Add(new ValidationResult() { Message = string.Format("{0}", GlobalResource.TheMinSalaryGreaterThanMaxSalary) });
            }

          
        }

       public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof (GradeByEducationViewModel).FullName;
        }
    }

    public class GradeByEducationQualificationViewModel : ViewModel
    {
        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var gradeByEducation = ServiceFactory.ORMService.GetById<GradeByEducation>(requestInformation.NavigationInfo.Previous[0].RowId);
            var gradeByEducationQualification = entity as GradeByEducationQualification;

            if (gradeByEducationQualification.FirstSalary > gradeByEducation.MaxSalary ||
                gradeByEducationQualification.FirstSalary < gradeByEducation.MinSalary)
            {
                var prop = typeof(GradeByEducationQualification).GetProperty("FirstSalary");
                validationResults.Add(new ValidationResult() { Message = string.Format("{0} {1}", prop.GetTitle(), GlobalResource.TheFirstSalaryIsOutOfRangeOfTheSalaryLimits), Property = prop });
            }

        }

       public override void CustomizeGridModel(Mvc4.Models.GridModel.GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(GradeByEducationQualificationViewModel).FullName;
            model.Views[0].EditHandler = "GradeByEducationQualificationEditHandler";
        }
    }
}