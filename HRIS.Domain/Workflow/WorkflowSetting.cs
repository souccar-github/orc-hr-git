using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Workflow
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [Command(CommandsNames.OverwriteWorkflowSetting)]
    [Module(ModulesNames.Workflow)]
    [Order(0)]
    public class WorkflowSetting : Entity, IConfigurationRoot
    {
        public WorkflowSetting()
        {
            SettingPositions = new List<WorkflowSettingPosition>();
            SettingOverwritings = new List<WorkflowSettingOverwritingHistory>();
            SettingApprovals = new List<WorkflowSettingApproval>();
        }

        public virtual int InitStepCount { get; set; }
        public virtual string Title { get; set; }
        public virtual DateTime CreationDate { get; set; }

        public virtual IList<WorkflowSettingPosition> SettingPositions { get; set; }
        public virtual void AddPosition(WorkflowSettingPosition settingPosition)
        {
            settingPosition.WorkflowSetting = this;
            SettingPositions.Add(settingPosition);
        }
        public virtual IList<WorkflowSettingOverwritingHistory> SettingOverwritings { get; set; }
        public virtual void AddOverwirte(WorkflowSettingOverwritingHistory workflowSettingOverwritingHistory)
        {
            workflowSettingOverwritingHistory.WorkflowSetting = this;
            SettingOverwritings.Add(workflowSettingOverwritingHistory);
        }
        public virtual IList<WorkflowSettingApproval> SettingApprovals { get; set; }
        public virtual void AddApprovals(WorkflowSettingApproval settingApproval)
        {
            settingApproval.WorkflowSetting = this;
            SettingApprovals.Add(settingApproval);
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Title; } }
    }
}
