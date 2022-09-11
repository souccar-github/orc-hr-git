using System;
using System.Collections.Generic;
using System.Linq;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Infrastructure.Core;
using HRIS.Validation;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class FixedHolidayViewModel : ViewModel
    {

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var fixedHoliday = (FixedHoliday)entity;
            fixedHoliday.NumberOfHolidayDays = 1;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults,
            CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var fixedHoliday = (FixedHoliday)entity;
            //var fixedHolidayList = ServiceFactory.ORMService.All<FixedHoliday>()
            //   .Where(x => x.HolidayName == fixedHoliday.HolidayName)
            //   .ToList();
            //if (fixedHolidayList.Count() > 0)
            //{
            //    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
            //    {
            //        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThisHolidyAlreadyExists),
            //        Property = typeof(ChangeableHoliday).GetProperty("Name")
            //    });
            //    return;
            //}
           
                if (!HolidayService.IsRegularDate((int)fixedHoliday.Day, (int)fixedHoliday.Month))
            {
                var prop = typeof(FixedHoliday).GetProperty("Day");
                validationResults.Add(new ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThisIsNotRegularDate),
                    Property = prop
                });
                return;
            }
            if (operationType == CrudOperationType.Insert)
            {
                var isExist = ServiceFactory.ORMService.All<FixedHoliday>().Any(x => x.Day == fixedHoliday.Day && x.Month == fixedHoliday.Month);
                if (isExist)
                {
                    var prop = typeof(FixedHoliday).GetProperty("Name");
                    validationResults.Add(new ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThisHolidyAlreadyExists),
                        Property = prop
                    });
                    return;
                }
            }
            if (operationType == CrudOperationType.Update)
            {
                var fixedHolidayList = ServiceFactory.ORMService.All<FixedHoliday>()
                   .Where(x => x.HolidayName == fixedHoliday.HolidayName && x.Day==fixedHoliday.Day && x.Month == fixedHoliday.Month)
                   .ToList();
                if (fixedHolidayList.Count() > 0)
                {
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                    {
                        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThisHolidyAlreadyExists),
                        Property = typeof(ChangeableHoliday).GetProperty("Name")
                    });
                    return;
                }
            }
            //if (!IsValidHolidayDate(fixedHoliday))
            //{ 
            //    validationResults.Add(new ValidationResult() 
            //    {
            //        Message = EmployeeRelationServicesLocalizationHelper.MsgThisIsNotValidHolidayDate,
            //        Property = typeof(FixedHoliday).GetProperty("Name")
            //    });
            //}
        }

        private bool IsValidHolidayDate(FixedHoliday newFixedHoliday)
        {
            var fixedHolidaysCount =
                ServiceFactory.ORMService.All<FixedHoliday>().Where(x =>
                    (newFixedHoliday.Month == x.Month) && 
                    (newFixedHoliday.Day >= x.Day && (int)newFixedHoliday.Day + newFixedHoliday.NumberOfHolidayDays <= (int)x.Day + x.NumberOfHolidayDays - 1) ||
                    ((int)newFixedHoliday.Day + newFixedHoliday.NumberOfHolidayDays >= (int)x.Day &&
                    (int)newFixedHoliday.Day + newFixedHoliday.NumberOfHolidayDays <= (int)x.Day + x.NumberOfHolidayDays - 1)).Count();

            if (fixedHolidaysCount > 0)
                return false;

            return true;
        }

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(FixedHolidayViewModel).FullName;
            //model.Views[0].EditHandler = "FixedHolidayEditHandler";

        }
    }
}