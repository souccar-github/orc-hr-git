
using System.Collections.Generic;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Workflow;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.PayrollSystem.Configurations;

namespace HRIS.Domain.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    

    [Module(ModulesNames.EmployeeRelationServices)]
    [Order(22)]
    //[Command(CommandsNames.Recycle, Order = 1)]
    public class LeaveSetting : Entity, IConfigurationRoot
    {
        public LeaveSetting()
        {
            BalanceSlices = new List<BalanceSlice>();
            PaidSlices = new List<PaidSlice>();
            Recycles = new List<Recycle>();
        }

        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual string AliasName { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual int MaturityPeriod { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual LeaveType Type { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual int IntervalDays { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual double Balance { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual bool HasMonthlyBalance { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual double MonthlyBalance { get; set; }

        [UserInterfaceParameter(Order = 35)]
        public virtual double PaidPercentage  { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual bool HasMaximumNumber { get; set; }

        [UserInterfaceParameter(Order = 45)]
        public virtual int MaximumNumber { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual bool IsIndivisible { get; set; }

        [UserInterfaceParameter(Order = 55)]
        public virtual bool IsContinuous { get; set; }

        [UserInterfaceParameter(Order = 60)]
        public virtual double RoundPercentage { get; set; }

        [UserInterfaceParameter(Order = 65)]
        public virtual bool IsDivisibleToHours { get; set; }

        [UserInterfaceParameter(Order = 70)]
        public virtual double MaximumHoursPerDay { get; set; }

        [UserInterfaceParameter(Order = 75)]
        public virtual double HoursEquivalentToOneLeaveDay { get; set; }

        [UserInterfaceParameter(Order = 80)]
        public virtual bool IsAffectedByAssigningDate { get; set; }

        [UserInterfaceParameter(Order = 81)]
        public virtual bool IsAffectedByStartWorkingDate { get; set; }

        [UserInterfaceParameter(Order = 82)]
        public virtual double MaximumRoundedLeaveDays { get; set; }

        [UserInterfaceParameter(IsReference = true, Order = 85)]
        public virtual WorkflowSetting WorkflowSetting { get; set; }
        [UserInterfaceParameter(Order = 85, IsReference = true)]
        public virtual DeductionCard DeductionCard { get; set; }

        [UserInterfaceParameter(Order = 90)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown {
            get { return AliasName; }   
        }
        #endregion

        #region BalanceSlices
        public virtual IList<BalanceSlice> BalanceSlices { get; set; }
        public virtual void AddBalanceSlice(BalanceSlice balanceSlice)
        {
            BalanceSlices.Add(balanceSlice);
            balanceSlice.LeaveSetting = this;
        }

        #endregion

        #region PaidSlices
        public virtual IList<PaidSlice> PaidSlices { get; set; }
        public virtual void AddPaidSlice(PaidSlice paidSlice)
        {
            PaidSlices.Add(paidSlice);
            paidSlice.LeaveSetting = this;
        }

        #endregion

        #region Recycles
        public virtual IList<Recycle> Recycles { get; set; }
        public virtual void AddRecycle(Recycle recycle)
        {
            Recycles.Add(recycle);
            recycle.LeaveSetting = this;
        }

        #endregion
       

    }
}
