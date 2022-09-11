using HRIS.Domain.Grades.RootEntities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;

namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class GradeBenefitDetail : EmployeeBenefitBase
    {// تفاصيل التعويض المختار - الحقول بالكلاس الاب

        //public virtual GradeBenefitMaster GradeBenefitMaster { get; set; }
        public virtual Grade Grade { get; set; }
    } 
}
