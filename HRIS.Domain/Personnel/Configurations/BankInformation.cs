
using HRIS.Domain.Global.Constant;

using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;


namespace HRIS.Domain.Personnel.Configurations
{
    //[Command(CommandsNames.PerformAudit_Handler, Order = 1)]
    //[Command(CommandsNames.CancelAudit_Handler, Order = 2)]
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>
    [Module(ModulesNames.Personnel)]
    [Order(90)]
    public class BankInformation : Entity, IConfigurationRoot
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
