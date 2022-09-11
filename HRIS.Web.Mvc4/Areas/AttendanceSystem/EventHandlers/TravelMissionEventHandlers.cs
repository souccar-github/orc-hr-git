using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using HRIS.Validation.MessageKeys;
using HRIS.Domain.Global.Enums;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{//todo : Mhd Update changeset no.1
    public class TravelMissionEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(TravelMissionEventHandlers).FullName;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var mission = (TravelMission)entity;
            var travelMissions = ServiceFactory.ORMService.All<TravelMission>().Where(x => x.Employee == mission.Employee);
            var hourlyMissions = ServiceFactory.ORMService.All<HourlyMission>().Where(x => x.Employee == mission.Employee);
           
            
            //اختبار تكرار الطلب
            if (travelMissions.Any(x =>
                ((x.Status == Status.Approved) || (x.Status == Status.Draft)) &&
               (((mission.FromDate >= x.FromDate && mission.FromDate <= x.ToDate) ||
                (mission.ToDate >= x.FromDate && mission.ToDate <= x.ToDate)))&& x.Id !=mission.Id ))
            {
                validationResults.Add(new ValidationResult
                {
                    Message = EmployeeRelationServicesLocalizationHelper
                                        .GetResource(EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod),
                });
            }
          
            if (hourlyMissions.Any(x =>
                ((x.Status == Status.Approved) || (x.Status == Status.Draft)) &&
               (((mission.ToDate >= x.Date && mission.FromDate <= x.Date)))))
            {
                validationResults.Add(new ValidationResult
                {
                    Message = EmployeeRelationServicesLocalizationHelper
                                                        .GetResource(EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod),
                });
            }
        }

        /*  public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
          {
              var travelMission = (TravelMission)entity;

              if (travelMission.ToDate < travelMission.FromDate)
              {
                  validationResults.Add(new ValidationResult
                  {
                      Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                          .GetFullKey(CustomMessageKeysAttendanceSystemModule.FromDateCannotBeGreaterThanToDate)),
                      Property = null
                  });
              }
          } */

        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }

        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            Type typeOfClass = typeof(TravelMission);
            var filteredDate = new List<TravelMission>();
            var allBeforeFilter = ((IQueryable<TravelMission>)result.Data).ToList();
            var user = UserExtensions.CurrentUser;
            if (UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result.Data = allBeforeFilter.Skip<TravelMission>(skip).Take<TravelMission>(pageSize).AsQueryable();
                result.Total = allBeforeFilter.Count();
            }
            else
            {
                var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
                filteredDate = allBeforeFilter.Where(x => employeesCanViewThem.Any(y => y.Id == x.Employee.Id)).ToList();
                result.Data = filteredDate.Skip<TravelMission>(skip).Take<TravelMission>(pageSize).AsQueryable();
                result.Total = filteredDate.Count();
            }
        }
        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            // عند محاولة قراءة المهمات اليومية عند حساب الدوام يفترض الشرط ان المهمة الوقت فيها مصفر ولكن هنا القيمة للوقت تساوي الوقت الحالي لذلك سنقوم بتصفير الوقت
            var travelMission = (TravelMission)entity;
            var fromDate = travelMission.FromDate;
            var toDate = travelMission.ToDate;
            travelMission.FromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);
            travelMission.ToDate = new DateTime(toDate.Year, toDate.Month, toDate.Day);
            travelMission.Status = HRIS.Domain.Global.Enums.Status.Approved;
        }

        public override void BeforeUpdate(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            string customInformation = null)
        {
            // عند محاولة قراءة المهمات اليومية عند حساب الدوام يفترض الشرط ان المهمة الوقت فيها مصفر ولكن هنا القيمة للوقت تساوي الوقت الحالي لذلك سنقوم بتصفير الوقت
            var travelMission = (TravelMission)entity;
            var fromDate = travelMission.FromDate;
            var toDate = travelMission.ToDate;
            travelMission.FromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);
            travelMission.ToDate = new DateTime(toDate.Year, toDate.Month, toDate.Day);
        }
    }
}