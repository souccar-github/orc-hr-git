using System;
using System.Collections.Generic;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using HRIS.Domain.AttendanceSystem.RootEntities;
using System.Linq;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using HRIS.Validation.MessageKeys;
using HRIS.Domain.Global.Enums;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{//todo : Mhd Update changeset no.1
    public class HourlyMissionEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(HourlyMissionEventHandlers).FullName;
        }

        public override void BeforeValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            CrudOperationType operationType, string customInformation = null)
        {
            var hourlyMission = (HourlyMission)entity;
            Prepare(hourlyMission);
        }

        public override void BeforeInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var hourlyMission = (HourlyMission)entity;
            hourlyMission.Status = HRIS.Domain.Global.Enums.Status.Approved;
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null)
        {
            var mission = (HourlyMission)entity;
            var travelMissions = ServiceFactory.ORMService.All<TravelMission>().Where(x => x.Employee == mission.Employee);
            var hourlyMissions = ServiceFactory.ORMService.All<HourlyMission>().Where(x => x.Employee == mission.Employee);
            if (operationType == CrudOperationType.Insert)
            {
                //اختبار تكرار الطلب
                if (travelMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) && mission.Date.Year == x.FromDate.Year
                    && mission.Date.Month == x.FromDate.Month && mission.Date.Day == x.FromDate.Day))
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = EmployeeRelationServicesLocalizationHelper
                            .GetResource(EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod),
                    });
                }
                if (hourlyMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) && mission.Date.Year == x.Date.Year
                    && mission.Date.Month == x.Date.Month && mission.Date.Day == x.Date.Day &&
                   (((mission.StartTime >= x.StartTime && mission.StartTime <= x.EndTime) ||
                    (mission.EndTime >= x.StartTime && mission.EndTime <= x.EndTime)))))
                {
                    validationResults.Add(new ValidationResult
                    {
                        Message = EmployeeRelationServicesLocalizationHelper
                            .GetResource(EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod),
                    });
                }
            }
        }
        public override void BeforeRead(RequestInformation requestInformation)
        {
            PreventDefault = true;
        }

        public override void AfterRead(RequestInformation requestInformation, DataSourceResult result, int pageSize = 10, int skip = 0)
        {
            Type typeOfClass = typeof(HourlyMission);
            var filteredDate = new List<HourlyMission>();
            var allBeforeFilter = ((IQueryable<HourlyMission>)result.Data).ToList();
            var user = UserExtensions.CurrentUser;
            if (UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result.Data = allBeforeFilter.Skip<HourlyMission>(skip).Take<HourlyMission>(pageSize).AsQueryable();
                result.Total = allBeforeFilter.Count();
            }
            else
            {
                var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
                filteredDate = allBeforeFilter.Where(x => employeesCanViewThem.Any(y => y.Id == x.Employee.Id)).ToList();
                result.Data = filteredDate.Skip<HourlyMission>(skip).Take<HourlyMission>(pageSize).AsQueryable();
                result.Total = filteredDate.Count();
            }
        }
        private void Prepare(HourlyMission hourlyMission)
        {// عند محاولة قراءة المهمات  عند حساب الدوام يفترض الشرط ان المهمة الوقت فيها مصفر ولكن هنا القيمة للوقت تساوي الوقت الحالي لذلك سنقوم بتصفير الوقت
            hourlyMission.Date = hourlyMission.Date.Date;
            hourlyMission.StartDateTime = hourlyMission.Date.AddHours(hourlyMission.StartTime.Hour).AddMinutes(hourlyMission.StartTime.Minute);
            hourlyMission.EndDateTime = hourlyMission.Date.AddHours(hourlyMission.EndTime.Hour).AddMinutes(hourlyMission.EndTime.Minute);
            if (hourlyMission.EndDateTime < hourlyMission.StartDateTime)
            {
                hourlyMission.EndDateTime = hourlyMission.EndDateTime.AddDays(1);
            }
        }
    }
}