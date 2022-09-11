
using HRIS.Domain.AttendanceSystem.Enums;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models
{
    public class EntranceExitRequestViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(EntranceExitRequestViewModel).FullName;            
        }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public int RecordId { get; set; }
        public  DateTime RecordDate { get; set; }
        public  DateTime RecordTime { get; set; }
        public  string Note { get; set; }
        public  LogType LogType { get; set; }
        public  string Desc { get; set; }

        public string LogTypeString
        {
            get
            {
                var s = ServiceFactory.LocalizationService.GetResource(typeof(LogType).FullName + "." + LogType.ToString());
                return string.IsNullOrEmpty(s) ? LogType.ToString().ToCapitalLetters() : s;
            }
        }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
    }
}