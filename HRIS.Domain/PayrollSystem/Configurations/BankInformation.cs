using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.PayrollSystem.BaseClasses;
using HRIS.Domain.PayrollSystem.Indexes;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
//using Status = HRIS.Domain.PayrollSystem.Enums.Status;

namespace HRIS.Domain.PayrollSystem.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    [Module(ModulesNames.PayrollSystem)]
    [Order(90)]
    [Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    [Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    public class BankInformation : AuditableEntity,IConfigurationRoot
    {

        #region Basic Info

        [UserInterfaceParameter(Order = 5)]
        public virtual string BankName { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual Nationality Nationality { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual string PhoneNumber { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual string Title { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual string ContactPerson { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual string JobTitle { get; set; }


        [UserInterfaceParameter(Order = 2, IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return BankName;
            }
        }
        #endregion

    }
}
