using System;
using System.Collections.Generic;
using System.Linq;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using  Project.Web.Mvc4.Helpers.Resource;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class ChangeableHolidayViewModel : ViewModel
    {

       public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(ChangeableHolidayViewModel).FullName;
            //model.Views[0].EditHandler = "ChangeableHolidayEditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<Souccar.Domain.Validation.ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
       
        {
            var changeableHoliday = (ChangeableHoliday)entity;
            //var changeableHolidayList = ServiceFactory.ORMService.All<ChangeableHoliday>()
            //     .Where(x => x.HolidayName == changeableHoliday.HolidayName)
            //     .ToList();
            //if (changeableHolidayList.Count() > 0)
            //{
            //    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
            //    {
            //        Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThisHolidyAlreadyExists),
            //        Property = typeof(ChangeableHoliday).GetProperty("Name")
            //    });
            //    return;
            //}
            if (changeableHoliday.StartDate == DateTime.MinValue)
            {
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgStartDateIsRequired),
                    Property = typeof(ChangeableHoliday).GetProperty("StartDate")
                });
                return;
            }

            if (changeableHoliday.EndDate == DateTime.MinValue)
            {
                validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                {
                    Message = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEndDateIsRequired),
                    Property = typeof(ChangeableHoliday).GetProperty("EndDate")
                });
                return;
            }

            if (operationType == CrudOperationType.Insert)
            {
                if (!IsValidHolidayDate(changeableHoliday.StartDate, changeableHoliday.EndDate))
                {
                    var prop = typeof (ChangeableHoliday).GetProperty("StartDate");
                    validationResults.Add(new Souccar.Domain.Validation.ValidationResult()
                                          {
                                              Message =
                                                  EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThisHolidyAlreadyExists),
                                              Property = prop
                                          });
                    return;
                }
            }
        }

        private bool IsValidHolidayDate(DateTime startDate, DateTime endDate)
        {
            var changeableHolidaysCount =
                ServiceFactory.ORMService.All<ChangeableHoliday>().Where(x =>
                    (startDate >= x.StartDate && startDate <= x.EndDate) || (endDate >= x.StartDate && endDate <= x.EndDate)).Count();

            if (changeableHolidaysCount > 0)
                return false;

            return true;
        }


    }
}