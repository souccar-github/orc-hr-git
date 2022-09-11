using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.Helpers;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Personnel.Entities
{
    public class ChildBase:Entity,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 10, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual string FirstName { get; set; }


        [UserInterfaceParameter(Order = 20, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual string LastName { get; set; }

       

        [UserInterfaceParameter(Order = 30, IsReference = true, ReferenceReadUrl = "Personnel/Reference/ReadSpouseForChild", Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual Spouse Spouse { get; set; }

        [UserInterfaceParameter(Order = 35, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual int OrderInFamily { get; set; }

        [UserInterfaceParameter(Order = 37, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual Gender Gender { get; set; }

        [UserInterfaceParameter(Order = 38, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual MaritalStatus MaritalStatus { get; set; }

        [UserInterfaceParameter(Order = 40, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual bool IsEmployed { get; set; }

        [UserInterfaceParameter(Order = 42, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual bool IsStudying { get; set; }

        [UserInterfaceParameter(Order = 45, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual DateTime DateOfBirth { get; set; }

        [UserInterfaceParameter(Order = 47, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual bool DisabilityExist { get; set; }

        [UserInterfaceParameter(Order = 50, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual Country PlaceOfBirth { get; set; }

        [UserInterfaceParameter(Order = 60, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual Nationality Nationality { get; set; }

        [UserInterfaceParameter(Order = 70, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual string ResidencyNo { get; set; }

        [UserInterfaceParameter(Order = 80, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual DateTime? ResidencyExpiryDate { get; set; }

        [UserInterfaceParameter(Order = 90, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual string PassportNo { get; set; }

        [UserInterfaceParameter(Order = 100, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildInfo)]
        public virtual DateTime? PassportExpiryDate { get; set; }

        [UserInterfaceParameter(Order = 110, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildBenefitInfo, IsHidden = true)]
        public virtual bool HasChildBenefit { get; set; }

        [UserInterfaceParameter(Order = 120, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildBenefitInfo, IsHidden = true)]
        public virtual DateTime? ChildBenefitStartDate { get; set; }

        [UserInterfaceParameter(Order = 130, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildBenefitInfo, IsHidden = true)]
        public virtual DateTime? ChildBenefitEndDate { get; set; }

        [UserInterfaceParameter(Order = 140, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildBenefitInfo)]
        public virtual bool IsDeath { get; set; }
        [UserInterfaceParameter(Order = 150, Group = "PersonnelGoupesNames_" + PersonnelGoupesNames.ChildBenefitInfo)]
        public virtual DateTime? DeathDate { get; set; }
        [UserInterfaceParameter(Order = 155, IsHidden = true)]
        public virtual string NameForDropdown { get { return string.Format("{0} {1}", FirstName, LastName); } }
    }
}
