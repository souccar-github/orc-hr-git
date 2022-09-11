using System;
using System.Collections.Generic;
using System.Linq;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class PublicHolidayViewModel : ViewModel
    {
       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(PublicHolidayViewModel).FullName;
            //model.Views[0].EditHandler = "PublicHolidayEditHandler";

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
      
        {
            var publicHoliday = (PublicHoliday) entity;
            var isExist = ServiceFactory.ORMService.All<PublicHoliday>().Any(x => x.DayOfWeek == publicHoliday.DayOfWeek);
            if (isExist)
            {
                var prop = typeof(PublicHoliday).GetProperty("DayOfWeek");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThisHolidyAlreadyExists),
                    Property = prop
                });
            }
                    
        }

        private bool IsValidHolidayDate(FixedHoliday newFixedHoliday)
        {
            //var fixedHolidaysCount =
            //    ServiceFactory.ORMService.All<FixedHoliday>().Where(x =>
            //        (newFixedHoliday.Month == x.Month) && 
            //        (newFixedHoliday.Day >= x.Day && (int)newFixedHoliday.Day + newFixedHoliday.NumberOfHolidayDays <= (int)x.Day + x.NumberOfHolidayDays - 1) ||
            //        ((int)newFixedHoliday.Day + newFixedHoliday.NumberOfHolidayDays >= (int)x.Day &&
            //        (int)newFixedHoliday.Day + newFixedHoliday.NumberOfHolidayDays <= (int)x.Day + x.NumberOfHolidayDays - 1)).Count();

            //if (fixedHolidaysCount > 0)
            //    return false;

            return true;
        }

        
    }
}