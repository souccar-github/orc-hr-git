using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AutoTasks.Enums;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.JobDesc.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using HRIS.Domain.OrgChart.Entities;
using ModulesNames = HRIS.Domain.Global.Constant.ModulesNames;
using Souccar.Infrastructure.Core;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    [Module(ModulesNames.EmployeeRelationServices)]
    public class NotificationSetting : Entity, IConfigurationRoot
    {
        public NotificationSetting()
        {
            NotificationPositions = new List<NotificationPosition>();
        }


        [UserInterfaceParameter(Order = 1)]
        public virtual NotificationTitle NotificationTitle { get; set; }
        public virtual IList<NotificationPosition> NotificationPositions { get; set; }

        public virtual void AddNotificationPosition(NotificationPosition notificationPosition)
        {
            this.NotificationPositions.Add(notificationPosition);
            notificationPosition.NotificationSetting = this;
        }
         public virtual string NameForDropdown
        {
            get
            {
             return   ServiceFactory.LocalizationService.GetResource("HRIS.Domain.EmployeeRelationServices.Enums.NotificationTitle."+Enum.GetName(typeof(NotificationTitle), NotificationTitle))
                ;}
         }
        
    }
}
