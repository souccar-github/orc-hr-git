using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.Configurations;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;


namespace HRIS.Domain.PayrollSystem.Entities
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class BankingInformation : Entity, IAggregateRoot
    {
        //[UserInterfaceParameter(Order = 5)]
        //public virtual PrimaryCard PrimaryCard { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual EmployeeCard EmployeeCard { get; set; }

        [UserInterfaceParameter(Order = 10, IsReference = true)]
        public virtual BankInformation BankInformation { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual string AccountNumber { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual string AccountName { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual DateTime? StartDate { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual DateTime? ExpiryDate { get; set; }
        //[UserInterfaceParameter(Order = 35)]
        //public virtual DateTime? AccountStartDate { get; set; }


    }
}
