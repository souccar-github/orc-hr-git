using System;
using System.Collections.Generic;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.RootEntities;

using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;


namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class PrimaryEmployeeBenefit : EmployeeBenefitBase
    {
        //[UserInterfaceParameter(Order = 5)]
        //public virtual PrimaryCard PrimaryCard { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual EmployeeCard EmployeeCard { get; set; }

        [UserInterfaceParameter(Order = 50)]
        public virtual bool HasStartDate { get; set; }

        [UserInterfaceParameter(Order = 55)]
        public virtual DateTime? StartDate { get; set; }

        [UserInterfaceParameter(Order = 60)]
        public virtual bool HasExpiryDate { get; set; }

        [UserInterfaceParameter(Order = 65)]
        public virtual DateTime? ExpiryDate { get; set; }

    }
}
