using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using Project.Web.Mvc4.Areas.AttendanceSystem.Services;
using Project.Web.Mvc4.Controllers;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.AttendanceSystem.Configurations;
using System.Windows.Forms;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Project.Web.Mvc4.Areas.PayrollSystem.Controllers;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Audit;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{//todo : Mhd Update changeset no.1
    public class AttendanceRecordController : Controller
    {

        public ActionResult GetAttendanceCycleStartDay()
        {
            var settings = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            return Json(new { Day = settings.AttendanceCycleStartDay }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AttendanceRecordOperation(int attendanceRecordId, string operation)
        {
            var message = String.Empty;
            var messageInfo = String.Empty;
            var isSuccess = false;
            var error = false;
            var attendanceRecord = (AttendanceRecord)typeof(AttendanceRecord).GetById(attendanceRecordId);
            List<EntranceExitRecord> entranceExitRecordNotAccepted = new List<EntranceExitRecord>();
            List<EntranceExitRecord> entranceExitRecordNotPairOfRecords = new List<EntranceExitRecord>();

            try
            {
                switch (operation)
                {
                    case "Generate":
                        {
                            //generate record until current day
                            if(attendanceRecord.AttendanceMonthStatus == AttendanceMonthStatus.Locked)
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.CannotGenerateLockedAttendanceRecord));
                                error = true;
                                break;
                            }
                            break;
                        }
                    case "Calculate":
                        {
                            if (attendanceRecord.AttendanceMonthStatus == AttendanceMonthStatus.Locked ||
                                attendanceRecord.AttendanceMonthStatus == AttendanceMonthStatus.Created)
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.CannotCalculateLockedOrCreatedAttendanceRecords));
                                error = true;
                                break;
                            }

                            AttendanceService.GenerateAttendanceRecordDetailsUntillCurrentDay(attendanceRecord);
                            attendanceRecord.Save();
                            //entranceExitRecordNotAccepted = AttendanceService.CheckEntranceExitStatus(attendanceRecord);
                            //if (entranceExitRecordNotAccepted.Count > 0)
                            //{                               
                            //    message = ServiceFactory.LocalizationService
                            //        .GetResource(CustomMessageKeysAttendanceSystemModule
                            //            .GetFullKey(CustomMessageKeysAttendanceSystemModule.CheckEntranceExitRecordStatusForThisMonth));
                            //    break;
                            //}
                            //entranceExitRecordNotPairOfRecords = AttendanceService.CheckAttendanceRecordPairOfRecords(attendanceRecord);
                            //if (entranceExitRecordNotPairOfRecords.Count > 0)
                            //{
                            //    foreach (var record in entranceExitRecordNotPairOfRecords)
                            //    {
                            //        record.ErrorMessage = ServiceFactory.LocalizationService
                            //        .GetResource(CustomMessageKeysAttendanceSystemModule
                            //            .GetFullKey(CustomMessageKeysAttendanceSystemModule.MustBePairOfRecords));
                            //        record.Save();

                            //    }
                            //    message = ServiceFactory.LocalizationService
                            //        .GetResource(CustomMessageKeysAttendanceSystemModule
                            //            .GetFullKey(CustomMessageKeysAttendanceSystemModule.CheckEntranceExitRecordsMustBePairOfRecords));
                            //    break;
                            //}
                            if (!AttendanceService.CheckEntranceExitRecordsConsistency(attendanceRecord))
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.CheckEntranceExitRecordsConsistencyFailed));
                                error = true;
                                break;
                            }

                            AttendanceService.CalculateAttendanceRecord(attendanceRecord);
                            attendanceRecord.AttendanceMonthStatus = AttendanceMonthStatus.Calculated;
                            attendanceRecord.Save();                   
                            break;
                        }
                    case "Lock":
                        {
                            if (attendanceRecord.AttendanceMonthStatus != AttendanceMonthStatus.Calculated)
                            {
                                message = ServiceFactory.LocalizationService
                                    .GetResource(CustomMessageKeysAttendanceSystemModule
                                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.OnlyCalculatedAttendanceRecordCanBeLocked));
                                break;
                            }
                            AttendanceService.LockAttendanceRecord(attendanceRecord);
                            attendanceRecord.AttendanceMonthStatus = AttendanceMonthStatus.Locked;
                            attendanceRecord.Save();
                            break;
                        }
                }

                attendanceRecord.Save();
                if (String.IsNullOrEmpty(message) && !error)
                {
                    isSuccess = true;
                    message = Helpers.GlobalResource.DoneMessage;
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = ex.Message;
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message
            });
        }

        [HttpPost]
        public ActionResult GetEmployeeAttendanceCardGridModel()
        {
            var gridModel = GridViewModelFactory.Create(typeof(EmployeeCard), null);
            gridModel.Views[0].ReadUrl = "AttendanceSystem/AttendanceRecord/ReadEmployeeAttendanceCardData";
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    ClassName = "grid-action-button EmployeeAttendanceCardGenerator",
                    Handler = "GenerateFilteredEmployeeAttendanceCards",
                    ImageClass = "",
                    Text =ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.GenerateTitle)),
                    Name = "GenerateFilteredEmployeeAttendanceCards"
                }
            };

            gridModel.ActionList.Commands.RemoveAt(0);
            gridModel.ActionList.Commands.RemoveAt(1);
            //gridModel.ToolbarCommands.RemoveAt(0);


            var displayColumnsList = new List<string> { "Employee", "StartWorkingDate", "ContractType", "EmployeeType", "EmployeeMachineCode", "AttendanceForm", 
                "LatenessForm", "AbsenceForm", "OvertimeForm", "LeaveTemplateMaster" };
            gridModel.Views[0].Columns = gridModel.Views[0].Columns.Where(x => displayColumnsList.Contains(x.FieldName)).ToList();
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckGeneralSettings()
        {
            var message = String.Empty;
            var isSuccess = false;

            if (!ServiceFactory.ORMService.All<GeneralSettings>().Any())
            {
                message = ServiceFactory.LocalizationService
                                 .GetResource(CustomMessageKeysAttendanceSystemModule
                                 .GetFullKey(CustomMessageKeysAttendanceSystemModule.MustAddGeneralSettings));
                isSuccess = false;
            }
            else
            {
                isSuccess = true;
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message
            });
        }

        [HttpPost]
        public ActionResult ReadEmployeeAttendanceCardData(int pageSize = 10, int skip = 0, bool serverPaging = true, IEnumerable<GridSort> sort = null, GridFilter filter = null, IEnumerable<GridGroup> group = null, RequestInformation requestInformation = null, string viewModelTypeFullName = null)
        {
            var entityType = typeof(EmployeeCard);
            CrudController.UpdateFilter(filter, entityType);

            if (filter == null)
            {
                filter = new GridFilter();
                filter.Logic = "and";
            }
            if (filter.Filters == null)
            {
                filter.Filters = new List<GridFilter>().AsEnumerable();
                filter.Logic = "and";
            }
            var temp = filter.Filters.ToList();
            if (temp.Count == 0)
            {
                temp.Add(new GridFilter()
                {
                    Field = "IsVertualDeleted",
                    Operator = "eq",
                    Value = false
                });
            }
          
            temp.Add(new GridFilter()
            {
                Field = "CardStatus",
                Operator = "eq",
                Value = EmployeeCardStatus.OnHeadOfHisWork

            });
            temp.Add(new GridFilter()
            {
                Field = "AttendanceDemand",
                Operator = "eq",
                Value = true

            });
            filter.Filters = temp.AsEnumerable();
            IQueryable<IEntity> EmployeeCards = CrudController.GetAllWithVertualDeleted(entityType);


            var dataSourse = DataSourceResult.GetDataSourceResult(EmployeeCards , entityType, pageSize, skip, serverPaging, sort,filter);
            dataSourse.Data = (IQueryable<EmployeeCard>)dataSourse.Data;

            var data = entityType.ToDynamicData(dataSourse.Data);
            return Json(new { Data = data, TotalCount = dataSourse.Total });
        }

        [HttpPost]
        public ActionResult GenerateFilteredEmployeeAttendanceCards(int attendanceRecordId, GridFilter filter = null)
        {
            string message;
            var isSuccess = false;
            var attendanceRecord = (AttendanceRecord)typeof(AttendanceRecord).GetById(attendanceRecordId);
            if (attendanceRecord.AttendanceMonthStatus == AttendanceMonthStatus.Locked)
            {
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysAttendanceSystemModule
                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.CannotGenerateLockedAttendanceRecord));
            }
            else
            {
                GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
                var entityType = typeof(EmployeeCard);
                CrudController.UpdateFilter(filter, entityType);
                IQueryable<IEntity> queryable = CrudController.GetAllWithVertualDeleted(entityType);
                var filteredEmployeeAttendanceCards = DataSourceResult.GetDataSourceResult(queryable, entityType, 10, 0, false, null, filter);
                filteredEmployeeAttendanceCards.Data = ((IQueryable<EmployeeCard>)filteredEmployeeAttendanceCards.Data)
                    .Where(x => x.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && x.AttendanceDemand);
                var totalGeneratedCards = AttendanceService.GenerateAttendanceRecordForSelectedEmployees(filteredEmployeeAttendanceCards.Data, attendanceRecord, generalSetting);
                attendanceRecord.Save();
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysAttendanceSystemModule
                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.AttendanceCardGenerated)) + "{" + totalGeneratedCards + "}";
                isSuccess = true;
            }
            return Json(new
            {
                Success = isSuccess,
                Msg = message,
            });
        }
        [HttpPost]
        public ActionResult GenerateFilteredData(Type type, IQueryable<IEntity> allData, int monthId, GridFilter filter = null)
        {
            string message;
            var start = DateTime.Now;
            var attendanceRecord = (AttendanceRecord)typeof(AttendanceRecord).GetById(monthId);

            if (attendanceRecord.AttendanceMonthStatus == AttendanceMonthStatus.Locked)
            {
                message = ServiceFactory.LocalizationService
                    .GetResource(CustomMessageKeysAttendanceSystemModule
                    .GetFullKey(CustomMessageKeysAttendanceSystemModule.CannotGenerateLockedAttendanceRecord));
            }
            if (!ServiceFactory.ORMService.All<GeneralSettings>().Any())
            {
                message = ServiceFactory.LocalizationService
                                 .GetResource(CustomMessageKeysAttendanceSystemModule
                                 .GetFullKey(CustomMessageKeysAttendanceSystemModule.MustAddGeneralSettings));
               
            
                return Json(new
                {
                    Success = false,
                    Msg = message,
                });
            }

        

            var queryablePrimaryCards = FilterController.GetRelatedPrimaryCards(type, allData, filter);
            var entities = new List<IAggregateRoot>();
            GeneralSettings generalSetting = ServiceFactory.ORMService.All<GeneralSettings>().FirstOrDefault();
            var totalGeneratedCards = AttendanceService.GenerateAttendanceRecordForSelectedEmployees(queryablePrimaryCards, attendanceRecord, generalSetting);


            entities.Add(attendanceRecord);
            var commandName = ServiceFactory.LocalizationService.GetResource(CommandsNames.ResourceGroupName + "_" + CommandsNames.GenerateMonth);
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, attendanceRecord, OperationType.Update, commandName, start);


            message = ServiceFactory.LocalizationService
                .GetResource(CustomMessageKeysPayrollSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.AttendanceCardGenerated)) + "{" + totalGeneratedCards + "}";

            return Json(new
            {
                Success = true,
                Msg = message,
            });
        }
        [HttpPost]
        public ActionResult GenerateFilteredPrimaryCards(int attendanceRecordId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(EmployeeCard), ServiceFactory.ORMService.AllWithVertualDeleted<EmployeeCard>(), attendanceRecordId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredEmployees(int attendanceRecordId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(Employee), ServiceFactory.ORMService.AllWithVertualDeleted<Employee>(), attendanceRecordId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredGrades(int attendanceRecordId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(HRIS.Domain.Grades.RootEntities.Grade), ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.Grades.RootEntities.Grade>(), attendanceRecordId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredJobTitles(int attendanceRecordId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(JobTitle), ServiceFactory.ORMService.AllWithVertualDeleted<JobTitle>(), attendanceRecordId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredJobDescriptions(int attendanceRecordId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(HRIS.Domain.JobDescription.RootEntities.JobDescription), ServiceFactory.ORMService.AllWithVertualDeleted<HRIS.Domain.JobDescription.RootEntities.JobDescription>(), attendanceRecordId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredPositions(int attendanceRecordId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(Position), ServiceFactory.ORMService.AllWithVertualDeleted<Position>(), attendanceRecordId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredNodes(int attendanceRecordId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(Node), ServiceFactory.ORMService.AllWithVertualDeleted<Node>(), attendanceRecordId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredMajorTypes(int attendanceRecordId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(MajorType), ServiceFactory.ORMService.AllWithVertualDeleted<MajorType>(), attendanceRecordId, filter);
        }

        [HttpPost]
        public ActionResult GenerateFilteredMajors(int attendanceRecordId, GridFilter filter = null)
        {
            return GenerateFilteredData(typeof(Major), ServiceFactory.ORMService.AllWithVertualDeleted<Major>(), attendanceRecordId, filter);
        }

    }
}
