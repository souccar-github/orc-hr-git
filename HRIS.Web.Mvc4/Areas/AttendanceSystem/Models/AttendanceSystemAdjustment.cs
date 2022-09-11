using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.AttendanceSystem.Configurations;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers;
using Project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers;
using project.Web.Mvc4.Areas.AttendanceSystem.EventHandlers;
using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Helpers;
using  Project.Web.Mvc4.Models.GridModel;
using  Project.Web.Mvc4.Models.Navigation;
using  Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.ProjectManagement.RootEntities;
using HRIS.Domain.ProjectManagement.Entities;
using HRIS.Domain.AttendanceSystem.RootEntities;
using  Project.Web.Mvc4.Models;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Models
{
   

    public class AttendanceSystemAdjustment: ModelAdjustment
    {
        private static Dictionary<string, ViewModel> parent = new Dictionary<string, ViewModel>();
        public  override void AdjustModule(Module module)
        {
            var service = new Service
            {
                Controller = "AttendanceSystem/AttendanceService",
                Action = "AutoGenerateAttendanceRecords",
                Title = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.AutoGenerateAttendanceRecords)),
                ServiceId = "AutoGenerateAttendanceRecords",
                SecurityId = "AutoGenerateAttendanceRecords",
            };
            service.Title = string.IsNullOrEmpty(service.Title) ? CustomMessageKeysAttendanceSystemModule.AutoGenerateAttendanceRecords.ToCapitalLetters() : service.Title;
            module.Services.Add(service);

            service = new Service
            {
                Controller = "AttendanceSystem/BioMetricInteraction",
                Action = "Index",
                Title = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.BioMetricInteraction)),
                ServiceId = "BioMetricInteraction",
                SecurityId = "BioMetricInteraction",
            };
            var importEntranceExitRecordsFromExcelService = new Service
            {
                Controller = "AttendanceSystem/Service",
                Action = "ImportEntranceExitRecordsFromExcel",
                Title = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.ImportEntranceExitRecordsFromExcel)),
                ServiceId = "ImportEntranceExitRecordsFromExcel",
                SecurityId = "ImportEntranceExitRecordsFromExcel",
            };
            service.Title = string.IsNullOrEmpty(service.Title) ? CustomMessageKeysAttendanceSystemModule.BioMetricInteraction.ToCapitalLetters() : service.Title;
            importEntranceExitRecordsFromExcelService.Title = string.IsNullOrEmpty(importEntranceExitRecordsFromExcelService.Title) ? CustomMessageKeysAttendanceSystemModule.ImportEntranceExitRecordsFromExcel.ToCapitalLetters() : importEntranceExitRecordsFromExcelService.Title;
            module.Services.Add(service);
            module.Services.Add(importEntranceExitRecordsFromExcelService);


            var attendanceRecord = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(AttendanceRecord).FullName);

            var attendanceMonthlyAdjustment = attendanceRecord.Details.SingleOrDefault(x => x.TypeFullName == 
                typeof(AttendanceMonthlyAdjustment).FullName);
            attendanceMonthlyAdjustment.Details = DetailFactory.Create(typeof(AttendanceMonthlyAdjustment));

            var attendanceMonthlyAdjustmentDetail = attendanceMonthlyAdjustment.Details.SingleOrDefault(x => x.TypeFullName == 
                typeof(AttendanceMonthlyAdjustmentDetail).FullName);
            attendanceMonthlyAdjustmentDetail.Details = DetailFactory.Create(typeof(AttendanceMonthlyAdjustmentDetail));

            var attendanceDailyAdjustment = attendanceRecord.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(AttendanceDailyAdjustment).FullName);
            attendanceDailyAdjustment.Details = DetailFactory.Create(typeof(AttendanceDailyAdjustment));

            var attendanceDailyAdjustmentDetail = attendanceDailyAdjustment.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(AttendanceDailyAdjustmentDetail).FullName);
            attendanceDailyAdjustmentDetail.Details = DetailFactory.Create(typeof(AttendanceDailyAdjustmentDetail));

            var attendanceWithoutAdjustment = attendanceRecord.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(AttendanceWithoutAdjustment).FullName);
            attendanceWithoutAdjustment.Details = DetailFactory.Create(typeof(AttendanceWithoutAdjustment));

            var attendanceWithoutAdjustmentDetail = attendanceWithoutAdjustment.Details.SingleOrDefault(x => x.TypeFullName ==
                typeof(AttendanceWithoutAdjustmentDetail).FullName);
            attendanceWithoutAdjustmentDetail.Details = DetailFactory.Create(typeof(AttendanceWithoutAdjustmentDetail));
            
            
            //var nonAttendanceForm = module.Aggregates.SingleOrDefault(x => x.TypeFullName == typeof(NonAttendanceForm).FullName);

            //var nonAttendanceSlice = nonAttendanceForm.Details.SingleOrDefault(x => x.TypeFullName ==
            //    typeof(NonAttendanceSlice).FullName);
            //nonAttendanceSlice.Details = DetailFactory.Create(typeof(NonAttendanceSlice));

            //var nonAttendanceSlicePercentage = nonAttendanceSlice.Details.SingleOrDefault(x => x.TypeFullName ==
            //    typeof(NonAttendanceSlicePercentage).FullName);
            //nonAttendanceSlicePercentage.Details = DetailFactory.Create(typeof(NonAttendanceSlicePercentage));


            if (module.ModuleId.Equals(ModulesNames.AttendanceSystem))
            {
                var workshopDetails = new List<string>()
                {
                    "ParticularOvertimeShifts","NormalShifts"
                };

                module.Configurations.SingleOrDefault(x => x.TypeFullName == (typeof(Workshop).FullName))
               .Details = module.Configurations.SingleOrDefault(x => x.TypeFullName == (typeof(Workshop).FullName))
                   .Details.Where(x => workshopDetails.Contains(x.DetailId))
                   .ToList();
            }
        }

        public override ViewModel AdjustGridModel(string type)
        {
            if (parent.Count == 0)
            {

                parent.Add("AttendanceDailyAdjustmentDetail", new AttendanceDailyAdjustmentDetailEventHandlers());
                parent.Add("AttendanceDailyAdjustment", new AttendanceDailyAdjustmentEventHandlers());
                parent.Add("AttendanceForm", new AttendanceFormEventHandlers());
                parent.Add("AttendanceMonthlyAdjustmentDetail", new AttendanceMonthlyAdjustmentDetailEventHandlers());
                parent.Add("AttendanceMonthlyAdjustment", new AttendanceMonthlyAdjustmentEventHandlers());
                parent.Add("AttendanceRecord", new AttendanceRecordEventHandlers());
                parent.Add("AttendanceWithoutAdjustmentDetail", new AttendanceWithoutAdjustmentDetailEventHandlers());
                parent.Add("AttendanceWithoutAdjustment", new AttendanceWithoutAdjustmentEventHandlers());
                parent.Add("BioMetricSetting", new BioMetricSettingEventHandlers());
                parent.Add("EntranceExitRecord", new EntranceExitRecordEventHandlers());
                parent.Add("GeneralSettings", new GeneralSettingsEventHandlers());
                parent.Add("HourlyMission", new HourlyMissionEventHandlers()); 
                parent.Add("NonAttendanceForm", new NonAttendanceFormEventHandlers());
                parent.Add("NonAttendanceSlicePercentage", new NonAttendanceSlicePercentageEventHandlers());
                parent.Add("NonAttendanceSlice", new NonAttendanceSliceViewModel());
                parent.Add("NormalShift", new NormalShiftEventHandlers());
                parent.Add("OvertimeForm", new OvertimeFormEventHandlers());
                parent.Add("OvertimeOrder", new OvertimeOrderEventHandlers());
                parent.Add("OvertimeSlice", new OvertimeSliceViewModel());
                parent.Add("ParticularOvertimeShift", new ParticularOvertimeShiftEventHandlers());
                parent.Add("ParticularWorkshop", new ParticularWorkshopEventHandlers());
                parent.Add("TemporaryWorkshop", new TemporaryWorkshopEventHandlers());
                parent.Add("TravelMission", new TravelMissionEventHandlers());
                parent.Add("Workshop", new WorkshopEventHandlers());
                parent.Add("WorkshopRecurrence", new WorkshopRecurrenceEventHandlers());



            }
            try
            {
                return parent[type];
            }
            catch {

                return new ViewModel();
            }
        
       
        }
       
    }
}