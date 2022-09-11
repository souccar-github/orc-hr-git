using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class JobDescriptionDeductionDetail : EmployeeDeductionBase
    {
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
    } 
}
