using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.AttendanceSystem.Configurations;
using Project.Web.Mvc4.Models.MasterDetailModels.DetailGridModels;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers
{//todo : Mhd Update changeset no.1
    public class WorkshopRecurrenceEventHandlers : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(WorkshopRecurrenceEventHandlers).FullName;
            model.Views[0].EditHandler = "WorkshopRecurrenceEditHandler";
        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState, IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null, Entity parententity = null, IList<DetailData> Details = null)
        {

            var workshopRecurrence = (WorkshopRecurrence)entity;
            //  var attendanceForm = ServiceFactory.ORMService.GetById<AttendanceForm>(requestInformation.NavigationInfo.Previous[0].RowId);

            if (workshopRecurrence.RecurrenceOrder > 31)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                        .GetFullKey(
                            CustomMessageKeysAttendanceSystemModule
                                .TheRecurrenceOrderMustBeLessOrEqualThan31)),
                    Property = null

                });

                return;
            }

            foreach (var detail in Details)
            {
                List<DetailObj> detailObjs = detail.List.ToList();

                if (detailObjs.Count() > 0)
                {
                    foreach (var detailObj in detailObjs)
                    {
                        if (Convert.ToInt32(detailObj.Properties.Single(x => x.PropName == "RecurrenceOrder").Value) == workshopRecurrence.RecurrenceOrder)
                        {

                            validationResults.Add(new ValidationResult()
                            {
                                Message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule
                           .GetFullKey(
                               CustomMessageKeysAttendanceSystemModule
                                   .ThereAreAnotherRecurrenceWithThisOrder)),
                                Property = null

                            });
                           
                            return;
                        }
                    }
                }
            }

            if (workshopRecurrence.Workshop==null)
            {
                validationResults.Add(new ValidationResult
                {
                    Message = ServiceFactory.LocalizationService.GetResource(PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required)),
                    Property = typeof(WorkshopRecurrence).GetProperty(typeof(WorkshopRecurrence).GetPropertyNameAsString<WorkshopRecurrence>(x => x.Workshop))
                });
            }
            

        }    
    }
}