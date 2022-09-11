using System;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using Souccar.Domain.Workflow.RootEntities;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public class BalanceSlice : Entity
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual int FromYearOfServices { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual int ToYearOfServices { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual double Balance { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual bool HasMonthlyBalance { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual double MonthlyBalance { get; set; }

        [UserInterfaceParameter(Order = 82)]
        public virtual double MaximumRoundedLeaveDays { get; set; }

        public virtual LeaveSetting LeaveSetting { get; set; }

        #endregion

    }
}
