using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Validation.MessageKeys;
using  Project.Web.Mvc4.Areas.AttendanceSystem.Services;
using  Project.Web.Mvc4.Controllers;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Resources.Shared.Messages;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Souccar.Web.Mvc.KendoGrid;
using HRIS.Domain.Personnel.Enums;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{
    public class AttendanceServiceController : Controller
    {
        public ActionResult AutoGenerateAttendanceRecords()
        {
            return PartialView("../Service/AutoGenerateAttendanceRecords");
        }

        [HttpPost]
        public ActionResult GetEmployeeAttendanceCardGridModel()
        {
            var gridModel = GridViewModelFactory.Create(typeof(EmployeeCard), null);
            gridModel.Views[0].ReadUrl = "AttendanceSystem/AttendanceService/ReadEmployeeAttendanceCardData";
            gridModel.ToolbarCommands = new List<ToolbarCommand>
            {
                new ToolbarCommand
                {
                    Additional = false,
                    Name = "AutoGenerateAttendanceRecords",
                    ClassName = "grid-action-button AutoGenerateAttendanceRecords",
                    Handler = "ApplyAutoGenerateAttendanceRecords",
                    ImageClass = "",
                    Text = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.AutoGenerateAttendanceRecords))
                }
            };
            var displayColumnsList = new List<string> { "Employee","FatherName", "StartWorkingDate", "ContractType", "EmployeeType", "EmployeeMachineCode", "AttendanceForm", 
                "LatenessForm", "AbsenceForm", "OvertimeForm", "LeaveTemplateMaster" };
            gridModel.Views[0].Columns = gridModel.Views[0].Columns.Where(x => displayColumnsList.Contains(x.FieldName)).ToList();
            return Json(gridModel, JsonRequestBehavior.AllowGet);
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
            IQueryable<IEntity> EmployeeCards = (IQueryable < EmployeeCard>) CrudController.GetAllWithVertualDeleted(entityType);

            var dataSourse = DataSourceResult.GetDataSourceResult(EmployeeCards, entityType, pageSize, skip, serverPaging, sort, filter);
            var cardsData = (IQueryable<EmployeeCard>)dataSourse.Data;
            var data = entityType.ToDynamicData(dataSourse.Data);
            return Json(new { Data = data, TotalCount = dataSourse.Total });
        }


        [HttpPost]
        public ActionResult ApplyAutoGenerateAttendanceRecords(DateTime fromDate, DateTime toDate, string note, GridFilter filter = null)
        {
            string message;
            bool success;
            try
            {
                if (String.IsNullOrEmpty(note))
                {
                    message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.AutoGenerateAttendanceRecordNoteIsRequired));
                    success = false;
                }
                else if (toDate < fromDate)
                {
                    message = ServiceFactory.LocalizationService.GetResource(CustomMessageKeysAttendanceSystemModule.GetFullKey(CustomMessageKeysAttendanceSystemModule.AutoGenerateAttendanceRecordToDateMustBeGreaterThanEqualToFromDate));
                    success = false;
                }
                else
                {
                    var entityType = typeof(EmployeeCard);
                    CrudController.UpdateFilter(filter, entityType);
                    IQueryable<IEntity> queryable = CrudController.GetAllWithVertualDeleted(entityType);
                    var dataSourse = (IEnumerable<EmployeeCard>)(DataSourceResult.GetDataSourceResult(queryable, entityType, 10, 0, false, null, filter).Data);

                    dataSourse = ((IQueryable<EmployeeCard>)dataSourse)
                    .Where(x => x.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && x.AttendanceDemand);


                    AttendanceService.GenerateAttendanceRecords(dataSourse, fromDate, toDate, note);
                    message = Helpers.GlobalResource.DoneMessage;
                    success = true;

                }
            }
            catch (Exception e )
            {
                message = Helpers.GlobalResource.Error;// ServiceFactory.LocalizationService.GetResource(General.GeneralErrorOccurred);
                success = false;
            }
            return Json(new
            {
                Success = success,
                Msg = message,
            });
        }


    }
}
