using Project.Web.Mvc4.APIAttribute;
using Souccar.Domain.Notification;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class NotifyHelper
    {
        public static List<Notify> CheckNotifications(BasicAuthenticationIdentity identity)
        {
            var user = ServiceFactory.ORMService.All<User>()
                    .FirstOrDefault(x => x.Username == identity.Name);
            var pendingWorkflows = ServiceFactory.ORMService.All<WorkflowItem>().Where(x => (x.CurrentUser.Id == user.Id ));

            var unreadedNotifications = ServiceFactory.ORMService.All<Notify>()
                .Where(x => pendingWorkflows.Select(y => y.Id).Contains(x.DestinationData["WorkflowId"]) 
                && (x.DestinationEntityId == "EmployeeLeaveRequest"
                || x.DestinationEntityId == "EntranceExitRequest"
                || x.DestinationEntityId == "TravelMission"
                || x.DestinationEntityId == "EmployeeAdvanceRequest"
                || x.DestinationEntityId == "EmployeeLoanRequest"
                || x.DestinationEntityId == "MissionRequest"
                || x.DestinationEntityId == "HourlyMission"))
                .Select(x => x).ToList();
            return unreadedNotifications;
        }
    }
}