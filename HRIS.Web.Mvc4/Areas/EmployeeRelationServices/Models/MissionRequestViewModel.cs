
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
    public class MissionRequestViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(MissionRequestViewModel).FullName;            
        }
        public int EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public int MissionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHourlyMission { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public DateTime RequestDate { get; set; }
        public string Description { get; set; }
        public int WorkflowItemId { get; set; }
        public WorkflowPendingType PendingType { get; set; }
        public TravelMissionType Type { get; set; }
        public string TypeString
        {
            get
            {
                var s = ServiceFactory.LocalizationService.GetResource(typeof(TravelMissionType).FullName + "." + Type.ToString());
                return string.IsNullOrEmpty(s) ? Type.ToString().ToCapitalLetters() : s;
            }
        }
        public string Note { get; set; }
    }
}