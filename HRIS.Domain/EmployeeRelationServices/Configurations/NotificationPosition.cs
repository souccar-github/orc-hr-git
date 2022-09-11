using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.OrgChart.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    public class NotificationPosition : Entity, IConfigurationRoot
    {

        [UserInterfaceParameter(Order = 5, IsReference = true)]
        public virtual JobTitle NotifyJobTitle { get; set; }

        [UserInterfaceParameter(Order = 10, IsReference = true, ReferenceReadUrl = "JobDescription/Reference/ReadPositionCascadeJobTitle", CascadeFrom = "NotifyJobTitle")]
        public virtual Position NotifyPosition { get; set; }

        public virtual NotificationSetting NotificationSetting { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return NotifyPosition != null ? NotifyPosition.NameForDropdown : ""; } }
    }
}
