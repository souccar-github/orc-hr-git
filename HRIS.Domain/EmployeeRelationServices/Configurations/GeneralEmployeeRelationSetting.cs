#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 04/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference

using HRIS.Domain.Global.Constant;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion
namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(30)]
    public class GeneralEmployeeRelationSetting : Entity, IConfigurationRoot
    {
        [UserInterfaceParameter(Order = 1, IsReference = true)]
        public virtual WorkflowSetting TerminationWorkflowName { get; set; }
        [UserInterfaceParameter(Order = 2, IsReference = true)]
        public virtual WorkflowSetting ResignationWorkflowName { get; set; }
        [UserInterfaceParameter(Order = 3, IsReference = true)]
        public virtual WorkflowSetting PromotionWorkflowName { get; set; }
        [UserInterfaceParameter(Order = 4, IsReference = true)]
        public virtual WorkflowSetting FinancialPromotionWorkflowName { get; set; }
        [UserInterfaceParameter(Order = 5, IsReference = true)]
        public virtual WorkflowSetting EntranceExitRequestWorkflowName { get; set; }
        [UserInterfaceParameter(Order = 5, IsReference = true)]
        public virtual WorkflowSetting MissionRequestWorkflowName { get; set; }
        [UserInterfaceParameter(Order = 6, IsReference = true)]
        public virtual WorkflowSetting AdvanceWorkflowName { get; set; }
        [UserInterfaceParameter(Order = 6, IsReference = true)]
        public virtual WorkflowSetting CustodiesWorkflowName { get; set; }
        [UserInterfaceParameter(Order = 7, IsReference = true)]
        public virtual WorkflowSetting EmployeeSourceTransferRequest { get; set; }
        [UserInterfaceParameter(Order = 7, IsReference = true)]
        public virtual WorkflowSetting EmployeeDistenationTransferRequest { get; set; }
        [UserInterfaceParameter(Order = 8, IsReference = true)]
        public virtual WorkflowSetting EmployeeLoanRequest { get; set; }

    }
}
