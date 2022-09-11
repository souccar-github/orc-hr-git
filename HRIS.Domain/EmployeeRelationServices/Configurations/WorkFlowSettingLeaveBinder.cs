
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    //[Command(CommandsNames.UpdateWorkFlowSetting, Order = 1)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(21)]
    public class WorkFlowSettingLeaveBinder : Entity, IConfigurationRoot
    {
        
        #region Basic Info

        [UserInterfaceParameter(Order = 1)]
        public virtual FixedLeaveType LeaveType { get; set; }

        [UserInterfaceParameter(Order = 2, ReferenceReadUrl = "EmployeeRelationServices/Reference/ReadWorkflowSetting", IsReference = true, IsHidden = false)]
        public virtual WorkflowSetting WorkflowSetting { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return ServiceFactory.LocalizationService.GetResource("HRIS.Domain.EmployeeRelationServices.Enums.FixedLeaveType." + Enum.GetName(typeof(FixedLeaveType), LeaveType)); } }
        #endregion


    }
}
